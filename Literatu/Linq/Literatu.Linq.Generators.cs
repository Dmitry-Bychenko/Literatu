﻿using System;
using System.Collections.Generic;

namespace Literatu.Linq {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// String Generator: Prefixes, Suffixes, Chunks
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class StringGenerator {
    #region Public

    /// <summary>
    /// Prefixes
    /// </summary>
    public static IEnumerable<string> Prefixes(this string value, bool addEmpty) {
      if (value is null)
        yield break;

      if (addEmpty)
        yield return "";

      for (int i = 1; i < value.Length; ++i)
        yield return value[0..1]; // value.Substring(0, i);
    }

    /// <summary>
    /// Prefixes
    /// </summary>
    public static IEnumerable<string> Prefixes(this string value) => Prefixes(value, true);

    /// <summary>
    /// Suffixes
    /// </summary>
    public static IEnumerable<string> Suffixes(this string value, bool addEmpty) {
      if (value is null)
        yield break;

      for (int i = 0; i < value.Length - 1; ++i)
        yield return value[i..];

      if (addEmpty)
        yield return "";
    }

    /// <summary>
    /// Suffixes
    /// </summary>
    public static IEnumerable<string> Suffixes(this string value) => Suffixes(value, true);

    /// <summary>
    /// Chunks (bi-, tri- grams etc.)
    /// </summary>
    public static IEnumerable<string> Chunks(this string value, int chunkSize) {
      if (chunkSize <= 0)
        throw new ArgumentOutOfRangeException(nameof(chunkSize), $"Chunk size must be positive.");

      if (string.IsNullOrEmpty(value))
        yield break;

      for (int i = 0; i < value.Length - chunkSize + 1; ++i)
        yield return value.Substring(i, chunkSize);
    }

    #endregion Public
  }

}
