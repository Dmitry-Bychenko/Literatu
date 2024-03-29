﻿using System;
using System.Collections.Generic;

namespace Literatu.Text {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Special String Functions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class StringFunctions {
    #region Public

    /// <summary>
    /// ZArray from given string
    /// https://www.geeksforgeeks.org/z-algorithm-linear-time-pattern-searching-algorithm/
    /// </summary>
    public static int[] ZArray(string value) {
      if (string.IsNullOrEmpty(value))
        return Array.Empty<int>();

      int n = value.Length;
      int L = 0, R = 0;

      int[] result = new int[n];

      result[0] = n;

      for (int i = 1; i < n; ++i) {
        if (i > R) {
          L = R = i;

          while (R < n && value[R - L] == value[R])
            ++R;

          result[i] = R - L;
          --R;
        }
        else {
          int k = i - L;

          if (result[k] < R - i + 1)
            result[i] = result[k];
          else {
            L = i;

            while (R < n && value[R - L] == value[R])
              ++R;

            result[i] = R - L;
            --R;
          }
        }
      }

      return result;
    }

    /// <summary>
    /// Prefix function from given string
    /// https://ru.wikipedia.org/wiki/%D0%9F%D1%80%D0%B5%D1%84%D0%B8%D0%BA%D1%81-%D1%84%D1%83%D0%BD%D0%BA%D1%86%D0%B8%D1%8F
    /// </summary>
    public static int[] PrefixFunction(string value) {
      if (string.IsNullOrEmpty(value))
        return Array.Empty<int>();

      int[] result = new int[value.Length];

      for (int i = 1; i < value.Length; ++i) {
        int k = result[i - 1];

        while (k > 0 && value[k] != value[i])
          k = result[k - 1];

        if (value[k] == value[i])
          k += 1;

        result[i] = k;
      }

      return result;
    }

    /// <summary>
    /// Lyndon factorization (Duval algorithm) 
    /// </summary>
    /// <param name="value">String to factorize</param>
    /// <returns>Lyndon simple substrings</returns>
    public static IEnumerable<ReadOnlyMemory<char>> Duval(string value) {
      if (string.IsNullOrEmpty(value))
        yield break;

      int n = value.Length;

      ReadOnlyMemory<char> memory = value.AsMemory();

      for (int i = 0; i < n;) {
        int j = i + 1;
        int k = i;

        for (; j < n && value[k] <= value[j]; ++j)
          if (value[k] < value[j])
            k = i;
          else
            k += 1;

        for (; i <= k; i += j - k)
          yield return memory.Slice(i, j - k);
      }
    }

    #endregion Public
  }

}
