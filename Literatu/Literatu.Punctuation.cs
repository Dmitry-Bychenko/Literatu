using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Literatu.Text {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Punctuation
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class Punctuation {
    #region Private Data

    private static readonly Dictionary<char, char> s_Pairs;

    #endregion Private Data

    #region Create

    static Punctuation() {
      s_Pairs = new Dictionary<char, char>();

      Stack<char> opens = new ();

      for (char c = char.MinValue; c < char.MaxValue - 1; ++c) {
        var category = char.GetUnicodeCategory(c);

        if (category == UnicodeCategory.OpenPunctuation)
          opens.Push(c);
        else if (category == UnicodeCategory.ClosePunctuation) {
          char open = opens.Pop();

          s_Pairs.Add(open, c);
          s_Pairs.Add(c, open);
        }
      }

      s_Pairs.Add('<', '>');
      s_Pairs.Add('>', '<');
    }

    #endregion Create

    #region Public

    /// <summary>
    /// if value is an open punctuation, e.g. [ ( opening quotation etc.
    /// </summary>
    public static bool IsOpen(char value) =>
      value == '<' || char.GetUnicodeCategory(value) == UnicodeCategory.OpenPunctuation;

    /// <summary>
    /// if value is a close punctuation, e.g. ] ) opening quotation etc.
    /// </summary>
    public static bool IsClose(char value) =>
      value == '>' || char.GetUnicodeCategory(value) == UnicodeCategory.ClosePunctuation;

    /// <summary>
    /// Reverse open and close punctuation, i.e ( => ), ] => [ etc.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static char ReverseOpenAndClose(char value) =>
      s_Pairs.TryGetValue(value, out char result) ? result : value;

    #endregion Public
  }

}
