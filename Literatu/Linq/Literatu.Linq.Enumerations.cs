using System.Collections.Generic;

namespace Literatu.Linq {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Indexes, Findings
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static partial class StringExtensions {
    #region Public

    /// <summary>
    /// Indexes Of
    /// </summary>
    public static IEnumerable<int> IndexesOf(this string source, string value, int startIndex) {
      if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value))
        yield break;

      for (int index = source.IndexOf(value, startIndex);
               index >= 0;
               index = source.IndexOf(value, ++index))
        yield return index;
    }

    /// <summary>
    /// Indexes Of
    /// </summary>
    public static IEnumerable<int> IndexesOf(this string source, string value) =>
      IndexesOf(source, value, 0);

    /// <summary>
    /// Last Indexes Of
    /// </summary>
    public static IEnumerable<int> LastIndexesOf(this string source, string value, int startIndex) {
      if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value))
        yield break;

      for (int index = source.LastIndexOf(value, startIndex);
               index >= 0;
               index = index == 0 ? -1 : source.LastIndexOf(value, --index))
        yield return index;
    }

    /// <summary>
    /// Last Indexes Of
    /// </summary>
    public static IEnumerable<int> LastIndexesOf(this string source, string value) =>
      LastIndexesOf(source, value, source is null ? 0 : source.Length - 1);

    #endregion Public
  }

}
