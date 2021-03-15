using System;

namespace Literatu {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Prefixes And Suffixes
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class PrefixesAndSuffixes {
    #region Public

    /// <summary>
    /// Length of the common prefix "abc", "abdefg" -> 2 ("ab")
    /// </summary>
    public static int CommonPrefixLength(string source, string other) {
      if ((source is null) || (other is null))
        return 0;

      int min = Math.Min(source.Length, other.Length);

      for (int i = 0; i < min; ++i)
        if (source[i] != other[i])
          return i;

      return min;
    }

    /// <summary>
    /// Length of the common suffix "abc", "pqrdebc" -> 2 ("bc")
    /// </summary>
    public static int CommonSuffixLength(string source, string other) {
      if ((source is null) || (other is null))
        return 0;

      int min = Math.Min(source.Length, other.Length);

      for (int i = 0; i < min; ++i)
        if (source[source.Length - 1 - i] != other[other.Length - 1 - i])
          return i;

      return min;
    }

    #endregion Public
  }

}
