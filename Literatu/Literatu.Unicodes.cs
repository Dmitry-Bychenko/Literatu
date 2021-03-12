using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Literatu {
  
  public static class Unicode {
    #region Private Data

    private static readonly HashSet<UnicodeCategory> s_Invisible = new() {
      UnicodeCategory.NonSpacingMark,
      UnicodeCategory.Control,
      UnicodeCategory.ModifierLetter,
      UnicodeCategory.LineSeparator,
      UnicodeCategory.ModifierSymbol,
      UnicodeCategory.ParagraphSeparator,
      UnicodeCategory.SpacingCombiningMark,
      UnicodeCategory.Surrogate,
    };

    #endregion Private Data

    #region Public

    /// <summary>
    /// Is Visible Category
    /// </summary>
    public static bool IsVisible(this UnicodeCategory category) =>
      !s_Invisible.Contains(category);

    /// <summary>
    /// Is Visible Character
    /// </summary>
    public static bool IsVisible(this char value) =>
      !s_Invisible.Contains(char.GetUnicodeCategory(value));

    #endregion Public
  }

}
