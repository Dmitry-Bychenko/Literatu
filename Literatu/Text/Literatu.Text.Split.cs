using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Literatu.Text {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// New Line standards
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public enum NewLine {
    Smart = 0,
    Default = 1,
    R = 2,
    N = 3,
    RN = 4,
    NR = 5,

    UnicodeNewLine = 6,
    UnicodeParagraph = 7,

    Windows = RN,
    Unix = N,
    Apple = NR,
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// New Line Extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class NewLineExtensions {
    #region Public

    /// <summary>
    /// Delimiter
    /// </summary>
    /// <param name="newLine">New Line</param>
    /// <returns>delimiter or null if doesn't exists</returns>
    public static string Delimiter(this NewLine newLine) {
      return newLine switch {
        NewLine.Smart => null,
        NewLine.Default => Environment.NewLine,
        NewLine.N => "\n",
        NewLine.R => "\r",
        NewLine.RN => "\r\n",
        NewLine.NR => "\n\r",
        NewLine.UnicodeNewLine => "\u2028",
        NewLine.UnicodeParagraph => "\u2029",
        _ => null,
      };
    }

    /// <summary>
    /// Join
    /// </summary>
    /// <param name="newLine">Delimiter</param>
    /// <param name="items">Items to join</param>
    /// <returns>Joined string</returns>
    public static string Join(this NewLine newLine, params object[] items) {
      if (items is null)
        throw new ArgumentNullException(nameof(items));

      return string.Join(Delimiter(newLine) ?? Environment.NewLine, items);
    }

    /// <summary>
    /// Split
    /// </summary>
    public static IEnumerable<string> Split(this NewLine newLine,
                                            string source,
                                            int count,
                                            StringSplitOptions splitOptions) =>
      source.SplitToLines(newLine, count, splitOptions);

    /// <summary>
    /// Split
    /// </summary>
    public static IEnumerable<string> Split(this NewLine newLine,
                                            string source,
                                            StringSplitOptions splitOptions) =>
      source.SplitToLines(newLine, splitOptions);

    /// <summary>
    /// Split
    /// </summary>
    public static IEnumerable<string> Split(this NewLine newLine,
                                            string source,
                                            int count) =>
      source.SplitToLines(newLine, count);

    /// <summary>
    /// Split
    /// </summary>
    public static IEnumerable<string> Split(this NewLine newLine,
                                            string source) =>
      source.SplitToLines(newLine);

    #endregion Public
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// String Extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static partial class StringExtensions {
    #region Algorithm

    private static IEnumerable<string> CoreSplitToLinesSmart(string source,
                                                             int count,
                                                             StringSplitOptions splitOptions) {

      if (string.IsNullOrEmpty(source))
        yield break;
      else if (count == 1) {
        yield return source;

        yield break;
      }

      int index = 0;
      int position = 0;

      for (int i = 0; i < source.Length; ++i) {
        char current = source[i];

        if (current == '\n' || current == '\r') {
          char next = i < source.Length - 1 ? source[i + 1] : '\0';

          if (position > i || splitOptions != StringSplitOptions.RemoveEmptyEntries) {
            index += 1;

            yield return source[position..i];
          }

          if (next != current && (next == '\n' || next == '\r'))
            i += 1;

          position = i + 1;

          if (count > 0 && index >= count - 1)
            break;
        }
      }

      // Tail if any
      if (position >= source.Length) {
        if (splitOptions != StringSplitOptions.RemoveEmptyEntries)
          yield return "";
      }
      else
        yield return source[position..];
    }

    #endregion Algorithm

    #region Public

    /// <summary>
    /// Split to Lines
    /// </summary>
    /// <param name="source"></param>
    /// <param name="newLine"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitToLines(this string source,
                                                   NewLine newLine,
                                                   int count,
                                                   StringSplitOptions splitOptions) {
      if (string.IsNullOrEmpty(source))
        yield break;
      else if (count == 1) {
        yield return source;

        yield break;
      }

      string delimiter = newLine.Delimiter();

      if (string.IsNullOrEmpty(delimiter)) {
        foreach (string line in CoreSplitToLinesSmart(source, count, splitOptions))
          yield return line;

        yield break;
      }

      int position = 0;
      int index = 0;

      while (true) {
        int next = source.IndexOf(delimiter, position);

        if (next < 0)
          break;

        string item = source.Substring(position, next - position - delimiter.Length);

        position = next + delimiter.Length;

        if (splitOptions == StringSplitOptions.RemoveEmptyEntries && string.IsNullOrEmpty(item))
          continue;

        index += 1;

        yield return item;

        if (count > 0 && count - index <= 1)
          break;
      }

      string tail = source[position..];

      if (splitOptions != StringSplitOptions.RemoveEmptyEntries || !string.IsNullOrEmpty(tail))
        yield return tail;
    }

    /// <summary>
    /// Split To Lines
    /// </summary>
    /// <param name="source"></param>
    /// <param name="newLine"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitToLines(this string source,
                                                   NewLine newLine,
                                                   int count) =>
      SplitToLines(source, newLine, count, StringSplitOptions.None);

    /// <summary>
    /// Split To Lines
    /// </summary>
    /// <param name="source"></param>
    /// <param name="newLine"></param>
    /// <param name="splitOptions"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitToLines(this string source,
                                                   NewLine newLine,
                                                   StringSplitOptions splitOptions) =>
      SplitToLines(source, newLine, 0, splitOptions);

    /// <summary>
    /// Split To Lines
    /// </summary>
    /// <param name="source"></param>
    /// <param name="newLine"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitToLines(this string source,
                                                   NewLine newLine) =>
      SplitToLines(source, newLine, 0, StringSplitOptions.None);

    /// <summary>
    /// Split To Lines
    /// </summary>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="splitOptions"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitToLines(this string source,
                                                   int count,
                                                   StringSplitOptions splitOptions) =>
      SplitToLines(source, NewLine.Smart, count, splitOptions);

    /// <summary>
    /// Split To Lines
    /// </summary>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitToLines(this string source,
                                                   int count) =>
      SplitToLines(source, NewLine.Smart, count, StringSplitOptions.None);

    /// <summary>
    /// Split To Lines
    /// </summary>
    /// <param name="source"></param>
    /// <param name="splitOptions"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitToLines(this string source,
                                                   StringSplitOptions splitOptions) =>
      SplitToLines(source, NewLine.Smart, 0, splitOptions);

    /// <summary>
    /// Split To Lines
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitToLines(this string source) =>
      SplitToLines(source, NewLine.Smart, 0, StringSplitOptions.None);

    #endregion Public
  }
}
