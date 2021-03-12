using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Literatu.IO;

namespace Literatu.Text {
  
  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Comma Separated Values Options 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  [Flags]
  public enum CommaSeparateValueOptions {
    /// <summary>
    /// None
    /// </summary>
    None = 0,

    /// <summary>
    /// Comments allowed
    /// </summary>
    Comments = 1,

    /// <summary>
    /// Use current culture instead of Invariant
    /// </summary>
    CurrenCulture = 2,
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Comma Separated Values
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public class CommaSeparateValue : IEquatable<CommaSeparateValue>, ISerializable {
    #region Algorithm

    private void CoreUpdate() {
      Quotation = new Quotation(QuotationMark);
    }

    /// <summary>
    /// Enquote when required
    /// </summary>
    protected virtual string CoreEnquote(object value) {
      if (value is null)
        return Quotation.Enquote("");

      string data = value.ToString();

      if (data.Contains(Separator) || data.Contains(QuotationMark))
        return Quotation.Enquote(data);
      if (Options.HasFlag(CommaSeparateValueOptions.Comments) && data.Contains(Comment))
        return Quotation.Enquote(data);
      if (data.Any(c => c < ' '))
        return Quotation.Enquote(data);
      if (data.Any(c => c == '\u2028' || c == '\u2029')) // New Line and New paragraph
        return Quotation.Enquote(data);

      return data;
    }

    /// <summary>
    /// Parse CSV lines
    /// </summary>
    protected virtual IEnumerable<string[]> CoreParseCsv(IEnumerable<string> lines) {
      if (lines is null)
        throw new ArgumentNullException(nameof(lines));

      List<string> items = new ();
      bool inQuotation = false;
      StringBuilder sb = new ();

      foreach (var line in lines) {
        if (line is null)
          continue;

        for (int i = 0; i < line.Length; ++i) {
          char ch = line[i];

          if (inQuotation) {
            if (ch == QuotationMark) {
              i += 1;

              if (i >= line.Length || line[i] != QuotationMark) {
                i -= 1;
                inQuotation = false;
              }
              else
                sb.Append(ch);
            }
            else
              sb.Append(ch);
          }
          else if (ch == QuotationMark) 
            inQuotation = true;
          else if (ch == Separator) {
            items.Add(sb.ToString());

            sb.Clear();
          }
          else if (ch == Comment && Options.HasFlag(CommaSeparateValueOptions.Comments))
            break;
          else
            sb.Append(ch);
        }

        // Line completed
        if (!inQuotation) {
          if (sb.Length > 0 || items.Any()) {
            items.Add(sb.ToString());

            yield return items.ToArray();
          }

          sb.Clear();
          items.Clear();
        }
      }

      if (inQuotation)
        throw new FormatException("Unterminated quotation");

      if (items.Any())
        yield return items.ToArray();
    }

    #endregion Algorithm

    #region Create

    // Deserialization
    private CommaSeparateValue(SerializationInfo info, StreamingContext context) {
      if (info is null)
        throw new ArgumentNullException(nameof(info));

      Separator = info.GetChar("separator");
      QuotationMark = info.GetChar("quotation");
      Comment = info.GetChar("comment");
      Options = (CommaSeparateValueOptions) (info.GetInt32("options"));

      CoreUpdate();
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="separator">Separator</param>
    /// <param name="quotation">Quotation Mark</param>
    /// <param name="comment">Commentary Mark, if any</param>
    /// <param name="options">Options</param>
    public CommaSeparateValue(char separator, char quotation, char comment, CommaSeparateValueOptions options) {
      Separator = separator;
      QuotationMark = quotation;
      Comment = comment;
      Options = options;

      CoreUpdate();
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="separator">Separator</param>
    /// <param name="quotation">Quotation Mark</param>
    /// <param name="comment">Commentary Mark, if any</param>
    public CommaSeparateValue(char separator, char quotation, char comment)
      : this(separator, quotation, comment, CommaSeparateValueOptions.Comments) { }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="separator">Separator</param>
    /// <param name="quotation">Quotation Mark</param>
    public CommaSeparateValue(char separator, char quotation)
      : this(separator, quotation, '\0', CommaSeparateValueOptions.None) { }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="separator">Separator</param>
    /// <param name="quotation">Quotation Mark</param>
    public CommaSeparateValue(char separator)
      : this(separator, '"', '\0', CommaSeparateValueOptions.None) { }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="separator">Separator</param>
    /// <param name="quotation">Quotation Mark</param>
    public CommaSeparateValue()
      : this(',', '"', '\0', CommaSeparateValueOptions.None) { }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="separator">Separator</param>
    /// <param name="quotation">Quotation Mark</param>
    /// <param name="options">Options</param>
    public CommaSeparateValue(char separator, char quotation, CommaSeparateValueOptions options) {
      Separator = separator;
      QuotationMark = quotation;
      Comment = options.HasFlag(CommaSeparateValueOptions.Comments) ? '#' : '\0';
      Options = options;

      CoreUpdate();
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="separator">Separator</param>
    /// <param name="options">Options</param>
    public CommaSeparateValue(char separator, CommaSeparateValueOptions options)
      : this(separator, '"', options) { }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="options">Options</param>
    public CommaSeparateValue(CommaSeparateValueOptions options)
      : this(',', '"', options) { }

    #endregion Create

    #region Standard Instances

    /// <summary>
    /// Standard
    /// </summary>
    public static CommaSeparateValue Standard { get; } = new CommaSeparateValue();

    /// <summary>
    /// Standard
    /// </summary>
    public static CommaSeparateValue StandardWithComments { get; } = 
      new CommaSeparateValue(CommaSeparateValueOptions.Comments);

    /// <summary>
    /// Excel
    /// </summary>
    public static CommaSeparateValue Excel { get; } = new CommaSeparateValue(';');

    /// <summary>
    /// Excel With Comments
    /// </summary>
    public static CommaSeparateValue ExcelWithComments { get; } =
      new CommaSeparateValue(';', CommaSeparateValueOptions.Comments);

    /// <summary>
    /// Tabbed
    /// </summary>
    public static CommaSeparateValue Tabbed { get; } = new CommaSeparateValue('\t');

    /// <summary>
    /// Tabbed With Comments
    /// </summary>
    public static CommaSeparateValue TabbedWithComments { get; } =
      new CommaSeparateValue('\t', CommaSeparateValueOptions.Comments);

    #endregion Standard Instances

    #region Public

    /// <summary>
    /// Quotation
    /// </summary>
    public Quotation Quotation { get; private set; }
       
    /// <summary>
    /// Separator
    /// </summary>
    public char Separator { get; }

    /// <summary>
    /// Quotation mark
    /// </summary>
    public char QuotationMark { get; }

    /// <summary>
    /// Comment mark
    /// </summary>
    public char Comment { get; }

    /// <summary>
    /// Options
    /// </summary>
    public CommaSeparateValueOptions Options { get; }

    /// <summary>
    /// Single Value to CSV 
    /// </summary>
    public string ItemToCsv(object item) {
      if (Options.HasFlag(CommaSeparateValueOptions.CurrenCulture))
        return CoreEnquote(item);

      var savedCulture = CultureInfo.CurrentCulture;

      try {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        return CoreEnquote(item);
      }
      finally {
        CultureInfo.CurrentCulture = savedCulture;
      }
    }

    /// <summary>
    /// To Csv
    /// </summary>
    public IEnumerable<string> ToCsv(IEnumerable<IEnumerable<object>> lines) {
      if (lines is null)
        throw new ArgumentNullException(nameof(lines));

      string delimiter = Separator.ToString();

      foreach (IEnumerable<object> line in lines) {
        if (line is null)
          continue;

        var savedCulture = CultureInfo.CurrentCulture;

        try {
          CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
          
          yield return string.Join(delimiter, line.Select(item => CoreEnquote(item)));
        }
        finally {
          CultureInfo.CurrentCulture = savedCulture;
        }
      }
    }

    /// <summary>
    /// From CSV
    /// </summary>
    public IEnumerable<string[]> ParseCsv(IEnumerable<string> lines) {
      if (lines is null)
        throw new ArgumentNullException(nameof(lines));

      return CoreParseCsv(lines);
    }

    /// <summary>
    /// Parse Csv
    /// </summary>
    public IEnumerable<string[]> ParseCsv(TextReader reader) {
      if (reader is null)
        throw new ArgumentNullException(nameof(reader));

      return ParseCsv(reader.AsEnumerable());
    }

    /// <summary>
    /// Parse Csv
    /// </summary>
    public IEnumerable<string[]> ParseCsv(Stream stream, Encoding encoding) {
      if (stream is null)
        throw new ArgumentNullException(nameof(stream));

      using StreamReader reader = new (stream, encoding, true, 8192, true);

      return ParseCsv(reader);
    }

    /// <summary>
    /// Parse Csv
    /// </summary>
    public IEnumerable<string[]> ParseCsv(Stream stream) => ParseCsv(stream, Encoding.Default);

    /// <summary>
    /// Read Csv Lines from File
    /// </summary>
    public IEnumerable<string[]> ReadCsvLines(string fileName, Encoding encoding) {
      if (fileName is null)
        throw new ArgumentNullException(nameof(fileName));

      return ParseCsv(File.ReadLines(fileName, encoding));
    }

    /// <summary>
    /// Read Csv Lines from File
    /// </summary>
    public IEnumerable<string[]> ReadCsvLines(string fileName) {
      if (fileName is null)
        throw new ArgumentNullException(nameof(fileName));

      return ParseCsv(File.ReadLines(fileName));
    }

    /// <summary>
    /// Write All Csv Lines
    /// </summary>
    public void WriteAllCsvLines(IEnumerable<object[]> lines, string fileName, Encoding encoding) {
      if (lines is null)
        throw new ArgumentNullException(nameof(lines));
      if (fileName is null)
        throw new ArgumentNullException(nameof(fileName));

      using FileStream fs = new (fileName, FileMode.Create);
      using StreamWriter sw = new(fs, encoding);

      bool first = true;

      foreach (string line in ToCsv(lines)) {
        if (!first)
          sw.Write(Environment.NewLine);

          sw.Write(line);

          first = false;
      }
    }

    /// <summary>
    /// Write All Csv Lines
    /// </summary>
    public void WriteAllCsvLines(IEnumerable<object[]> lines, string fileName) =>
      WriteAllCsvLines(lines, fileName, Encoding.Default);

    /// <summary>
    /// Append All Csv Lines
    /// </summary>
    public void AppendAllCsvLines(IEnumerable<object[]> lines, string fileName, Encoding encoding) {
      if (lines is null)
        throw new ArgumentNullException(nameof(lines));
      if (fileName is null)
        throw new ArgumentNullException(nameof(fileName));

      bool first = true;

      using FileStream fs = new(fileName, FileMode.OpenOrCreate);

      if (fs.Position > 0) {
        first = false;
      }

      using StreamWriter sw = new(fs, encoding);

      foreach (string line in ToCsv(lines)) {
        if (!first)
          sw.Write(Environment.NewLine);

        sw.Write(line);

        first = false;
      }
    }

    /// <summary>
    /// Append All Csv Lines
    /// </summary>
    public void AppendAllCsvLines(IEnumerable<object[]> lines, string fileName) =>
      AppendAllCsvLines(lines, fileName, Encoding.Default);

    #endregion Public

    #region Operators

    /// <summary>
    /// Equal
    /// </summary>
    public static bool operator == (CommaSeparateValue left, CommaSeparateValue right) {
      if (ReferenceEquals(left, right))
        return true;
      if ((left is null) || (right is null))
        return false;

      return left.Equals(right);
    }

    /// <summary>
    /// Not Equal
    /// </summary>
    public static bool operator !=(CommaSeparateValue left, CommaSeparateValue right) {
      if (ReferenceEquals(left, right))
        return false;
      if ((left is null) || (right is null))
        return true;

      return !left.Equals(right);
    }

    #endregion Operators

    #region IEquatable<CommaSeparateValue>

    /// <summary>
    /// Equals
    /// </summary>
    public bool Equals(CommaSeparateValue other) {
      if (other is null)
        return false;

      return Separator == other.Separator &&
             QuotationMark == other.QuotationMark &&
             Options == other.Options &&
             (Comment == other.Comment || !Options.HasFlag(CommaSeparateValueOptions.Comments));
    }

    /// <summary>
    /// Equals
    /// </summary>
    public override bool Equals(object o) => o is CommaSeparateValue other && Equals(other);

    /// <summary>
    /// Hash Code
    /// </summary>
    public override int GetHashCode() {
      unchecked {
        return ((int)Options << 28) |
               (QuotationMark << 16) |
               (Separator);
      }
    }

    #endregion IEquatable<CommaSeparateValue>

    #region ISerializable 

    /// <summary>
    /// Serialization
    /// </summary>
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
      if (info is null)
        throw new ArgumentNullException(nameof(info));

      info.AddValue("separator", Separator);
      info.AddValue("quotation", QuotationMark);
      info.AddValue("comment", Comment);
      info.AddValue("options", (int) Options);
    }

    #endregion ISerializable 
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Strings Extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static partial class StringsExtensions {
    #region Public

    /// <summary>
    /// Parse Lines from CSV
    /// </summary>
    /// <param name="lines">Lines to Parse</param>
    /// <param name="parser">Parser to Use</param>
    public static IEnumerable<string[]> ParseCsv(this IEnumerable<string> lines, CommaSeparateValue parser) {
      if (lines is null)
        throw new ArgumentNullException(nameof(lines));

      parser ??= CommaSeparateValue.Standard;

      return parser.ParseCsv(lines);
    }

    #endregion Public
  }

}
