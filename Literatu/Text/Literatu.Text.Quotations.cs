using System;
using System.Runtime.Serialization;
using System.Text;

namespace Literatu.Text {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Quotation  
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public interface IQuotation {
    /// <summary>
    /// Enquote String 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    string Enquote(string value);

    /// <summary>
    /// Try Dequote String
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool TryDequote(string value, out string result);
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Quotation Extensions 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class QuotationExtensions {
    #region Public

    /// <summary>
    /// Dequote
    /// </summary>
    public static string Dequote(this IQuotation rule, string value) {
      if (rule is null)
        throw new ArgumentNullException(nameof(rule));
      if (value is null)
        throw new ArgumentNullException(nameof(value));

      if (rule.TryDequote(value, out string result))
        return result;

      throw new FormatException("Failed to return quoatation.");
    }

    #endregion Public
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Quotation 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public class Quotation : IQuotation, IEquatable<Quotation>, ISerializable {
    #region Algorithm

    /// <summary>
    /// Core Make Quotation
    /// </summary>
    /// <param name="value">value to quote</param>
    /// <returns>enquoted string</returns>
    protected virtual string CoreEnquote(string value) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));

      StringBuilder sb = new(2 * value.Length + 2);

      sb.Append(OpenQuotation);

      foreach (char c in value) {
        if (c == CloseQuotation | c == EscapeCloseQuotation)
          sb.Append(EscapeCloseQuotation);
        else if ((c == OpenQuotation | c == EscapeOpenQuotation) & HasOpenQuotation)
          sb.Append(EscapeOpenQuotation);

        sb.Append(c);
      }

      sb.Append(CloseQuotation);

      return sb.ToString();
    }

    /// <summary>
    /// Try to Remove Quotation 
    /// </summary>
    /// <param name="value">String to Remove Quotation from</param>
    /// <param name="result">String with Quotation removed or null</param>
    /// <returns>true if quotation removed, false otherwise</returns>
    protected virtual bool CoreTryDequote(string value, out string result) {
      result = default;

      if (value is null)
        return false;
      else if (value.Length <= 1)
        return false;
      else if (value[0] != OpenQuotation || value[^1] != CloseQuotation)
        return false;

      StringBuilder sb = new(value.Length);

      for (int i = 1; i < value.Length - 1; ++i) {
        char ch = value[i];

        if (ch == EscapeCloseQuotation) {
          if (i == value.Length - 2)
            return false;

          i += 1;
          ch = value[i];

          if (ch == EscapeCloseQuotation || ch == CloseQuotation)
            sb.Append(ch);
          else
            return false;
        }
        else if (ch == EscapeOpenQuotation && HasOpenQuotation) {
          if (i == value.Length - 2)
            return false;

          i += 1;
          ch = value[i];

          if (ch == EscapeOpenQuotation || ch == OpenQuotation)
            sb.Append(ch);
          else
            return false;
        }
        else if (ch == OpenQuotation && HasOpenQuotation || ch == CloseQuotation)
          return false;
        else
          sb.Append(ch);
      }

      result = sb.ToString();

      return true;
    }

    #endregion Algorithm

    #region Create

    private Quotation(SerializationInfo info, StreamingContext context) {
      if (info is null)
        throw new ArgumentNullException(nameof(info));

      OpenQuotation = info.GetChar("open");
      CloseQuotation = info.GetChar("close");
      EscapeOpenQuotation = info.GetChar("escOpen");
      EscapeCloseQuotation = info.GetChar("escClose");
      HasOpenQuotation = info.GetBoolean("hasOpen");
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="openQuotation">Open Quotation Mark</param>
    /// <param name="escapeOpenQuotation">Open Quotation Escapement</param>
    /// <param name="closeQuotation">Close Quotation Mark</param>
    /// <param name="escapeCloseQuotation">Close Quotation Escapement</param>
    public Quotation(char openQuotation,
                     char escapeOpenQuotation,
                     char closeQuotation,
                     char escapeCloseQuotation) {
      OpenQuotation = openQuotation;
      EscapeOpenQuotation = escapeOpenQuotation;
      CloseQuotation = closeQuotation;
      EscapeCloseQuotation = escapeCloseQuotation;

      HasOpenQuotation = true;
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="openQuotation">Open Quotation Mark</param>
    /// <param name="closeQuotation">Close Quotation Mark</param>
    /// <param name="escapement">Escapement</param>
    public Quotation(char openQuotation,
                     char closeQuotation,
                     char escapement) {
      OpenQuotation = openQuotation;
      CloseQuotation = closeQuotation;
      EscapeCloseQuotation = escapement;

      HasOpenQuotation = false;
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="quotation">Open Quotation Mark</param>
    /// <param name="escapement">Escapement</param>
    public Quotation(char quotation,
                     char escapement) {
      OpenQuotation = quotation;
      CloseQuotation = quotation;
      EscapeCloseQuotation = escapement;

      HasOpenQuotation = false;
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    /// <param name="quotation">Open Quotation Mark</param>
    public Quotation(char quotation) {
      OpenQuotation = quotation;
      CloseQuotation = quotation;
      EscapeCloseQuotation = quotation;

      HasOpenQuotation = false;
    }

    #endregion Create

    #region Standard Instances

    /// <summary>
    /// Default
    /// </summary>
    public static Quotation Default => C;

    /// <summary>
    /// Pascal Quotation Style: ab\c'de"f[g]h => 'abc''de"f[g]h' 
    /// </summary>
    public static Quotation Pascal { get; } = new Quotation('\'');

    /// <summary>
    /// С Quotation Style: ab\c'de"f[g]h => "ab\\c'de\"f[g]h" 
    /// </summary>
    public static Quotation C { get; } = new Quotation('"', '\\');

    /// <summary>
    /// Oracle Quotation Style: ab\c'de"f[g]h => "ab\c'de""f[g]h" 
    /// </summary>
    public static Quotation Oracle { get; } = new Quotation('"');

    /// <summary>
    /// MS SQL Quotation Style: ab\c'de"f[g]h => [ab\c'de"f[g]]h] 
    /// </summary>
    public static Quotation MsSql { get; } = new Quotation('[', ']', ']');

    /// <summary>
    /// Interpolated Quotation Style: ab\c'de"{f}[g]h => {ab\c'de"{{f}}[g]h} 
    /// </summary>
    public static Quotation Interpolated { get; } = new Quotation('{', '{', '}', '}');

    #endregion Standard Instances

    #region Public

    /// <summary>
    /// Open Quotation Mark
    /// </summary>
    public char OpenQuotation { get; }

    /// <summary>
    /// Escape Quotation
    /// </summary>
    public char EscapeOpenQuotation { get; }

    /// <summary>
    /// Close Quotation
    /// </summary>
    public char CloseQuotation { get; }

    /// <summary>
    /// Escape Close Quotation
    /// </summary>
    public char EscapeCloseQuotation { get; }

    /// <summary>
    /// Has Open Quotation
    /// </summary>
    public bool HasOpenQuotation { get; }

    /// <summary>
    /// To String (Debug)
    /// </summary>
    public override string ToString() {
      if (HasOpenQuotation)
        return $"{OpenQuotation} .. {EscapeOpenQuotation}{OpenQuotation} .. {EscapeCloseQuotation}{CloseQuotation} .. {CloseQuotation}";
      else
        return $"{OpenQuotation} .. {EscapeCloseQuotation}{CloseQuotation} .. {CloseQuotation}";
    }

    #endregion Public

    #region Operators

    /// <summary>
    /// Equals 
    /// </summary>
    public static bool operator ==(Quotation left, Quotation right) {
      if (ReferenceEquals(left, right))
        return true;
      if ((left is null) || (right is null))
        return false;

      return left.Equals(right);
    }

    /// <summary>
    /// Not Equals 
    /// </summary>
    public static bool operator !=(Quotation left, Quotation right) {
      if (ReferenceEquals(left, right))
        return false;
      if ((left is null) || (right is null))
        return true;

      return !left.Equals(right);
    }

    #endregion Operators 

    #region IQuotation

    /// <summary>
    /// Enquote
    /// </summary>
    public string Enquote(string value) => CoreEnquote(value);

    /// <summary>
    /// Try Dequote
    /// </summary>
    public bool TryDequote(string value, out string result) => CoreTryDequote(value, out result);

    #endregion IQuotation

    #region IEquatable<Quotation>

    /// <summary>
    /// Equals
    /// </summary>
    public bool Equals(Quotation other) {
      if (other is null)
        return false;

      return OpenQuotation == other.OpenQuotation &&
             CloseQuotation == other.CloseQuotation &&
             EscapeOpenQuotation == other.EscapeOpenQuotation &&
             EscapeCloseQuotation == other.EscapeCloseQuotation &&
             HasOpenQuotation == other.HasOpenQuotation;
    }

    /// <summary>
    /// Equals
    /// </summary>
    public override bool Equals(object o) => o is Quotation other && Equals(other);

    /// <summary>
    /// Hash Code
    /// </summary>
    public override int GetHashCode() {
      unchecked {
        return (OpenQuotation << 24) ^
               (EscapeCloseQuotation << 16) ^
               (CloseQuotation);
      }
    }

    #endregion IEquatable<Quotation>

    #region ISerializable 

    /// <summary>
    /// Serialization
    /// </summary>
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
      if (info is null)
        throw new ArgumentNullException(nameof(info));

      info.AddValue("open", OpenQuotation);
      info.AddValue("close", CloseQuotation);
      info.AddValue("escOpen", EscapeOpenQuotation);
      info.AddValue("escClose", EscapeCloseQuotation);
      info.AddValue("hasOpen", HasOpenQuotation);
    }

    #endregion ISerializable 
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// String Enquote and Dequote
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static partial class StringExtensions {
    #region Public

    /// <summary>
    /// Enquote
    /// </summary>
    /// <param name="source">String to Enquote</param>
    /// <param name="rule">Rule to Use</param>
    /// <returns>Enquoted String</returns>
    public static string Enquote(this string source, IQuotation rule) {
      rule ??= Quotation.Default;

      return rule.Enquote(source);
    }

    /// <summary>
    /// Enquote
    /// </summary>
    /// <param name="source">String to Enquote</param>
    /// <returns>Enquoted String</returns>
    public static string Enquote(this string source) => Enquote(source, null);

    /// <summary>
    /// Try Dequote 
    /// </summary>
    /// <param name="source">String to Dequote</param>
    /// <param name="result">Dequoted string or null</param>
    /// <param name="rule">Rule to use</param>
    /// <returns>true if dequoted, false otherwise</returns>
    public static bool TryDequote(this string source, out string result, IQuotation rule) {
      rule ??= Quotation.Default;

      return rule.TryDequote(source, out result);
    }

    /// <summary>
    /// Try Dequote 
    /// </summary>
    /// <param name="source">String to Dequote</param>
    /// <param name="result">Dequoted string or null</param>
    /// <returns>true if dequoted, false otherwise</returns>
    public static bool TryDequote(this string source, out string result) => TryDequote(source, out result, null);

    /// <summary>
    /// Dequote
    /// </summary>
    /// <param name="source">String to Dequote</param>
    /// <param name="rule">Rule to Use</param>
    /// <returns>Dequoted String</returns>
    public static string Dequote(this string source, IQuotation rule) {
      rule ??= Quotation.Default;

      return rule.Dequote(source);
    }

    /// <summary>
    /// Enquote
    /// </summary>
    /// <param name="source">String to Dequote</param>
    /// <returns>Dequoted String</returns>
    public static string Dequote(this string source) => Dequote(source, null);

    #endregion Public
  }

}
