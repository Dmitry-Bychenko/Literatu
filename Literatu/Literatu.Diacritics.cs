using System;
using System.Collections.Generic; 
using System.Globalization;
using System.Linq;
using System.Text;

namespace Literatu {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Diacritics
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class Diacritics {
    #region Public

    /// <summary>
    /// If given character is diacritics
    /// </summary>
    public static bool Is(char value) =>
      CharUnicodeInfo.GetUnicodeCategory(value) == UnicodeCategory.NonSpacingMark;

    /// <summary>
    /// Contains
    /// </summary>
    public static bool Contains(string value) {
      if (string.IsNullOrEmpty(value))
        return false;

      return value
        .Normalize(NormalizationForm.FormD)
        .Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark);
    }

    /// <summary>
    /// Contains
    /// </summary>
    public static bool Contains(char value) => value
      .ToString()
      .Normalize(NormalizationForm.FormD)
      .Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark);

    /// <summary>
    /// Remove diacritics è => e
    /// </summary>
    public static char Remove(char value) {
      string st = value.ToString();

      if (!Contains(st))
        return value;

      return st
        .Normalize(NormalizationForm.FormD)
        .FirstOrDefault(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
    }

    /// <summary>
    /// Remove diacritics crème brûlée => creme brulee
    /// </summary>
    public static string Remove(string value) {
      if (value is null)
        return null;

      return string
        .Concat(value
           .Normalize(NormalizationForm.FormD)
           .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
        .Normalize(NormalizationForm.FormC);
    }

    /// <summary>
    /// Get All diacritics
    /// </summary>
    public static char[] Get(char value) => value
      .ToString()
      .Normalize(NormalizationForm.FormD)
      .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
      .ToArray();

    /// <summary>
    /// Add diacritics
    /// </summary>
    public static char Add(char value, IEnumerable<char> diacritics) {
      if (diacritics is null)
        throw new ArgumentNullException(nameof(diacritics));

      List<char> list = diacritics
        .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
        .ToList();

      if (list.Count <= 0)
        return value;

      list.Insert(0, value);

      string result = string.Concat(string
           .Concat(list)
           .Normalize(NormalizationForm.FormD)
           .Distinct())
        .Normalize(NormalizationForm.FormC);

      if (result.Length == 1)
        return result[0];

      throw new ArgumentException("Diacritics can't be added", nameof(diacritics));
    }
  
    #endregion Public
  }

}
