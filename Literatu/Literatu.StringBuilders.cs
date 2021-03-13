using System;
using System.Collections.Generic;
using System.Text;

namespace Literatu {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// StringBuilder Extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static partial class StringBuilderExtensions {
    #region Public

    /// <summary>
    /// Reverse (at place)
    /// </summary>
    public static StringBuilder Reverse(this StringBuilder value) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));

      for (int i = 0; i < value.Length / 2; ++i) {
        char right = value[value.Length - 1 - i];

        value[value.Length - 1 - i] = value[i];
        value[i] = right;
      }

      return value;
    }

    /// <summary>
    /// Filter (in place)
    /// </summary>
    /// <param name="value">StringBuilder to filter</param>
    /// <param name="condition">condition to use</param>
    /// <returns>filtered StringBuilder</returns>
    public static StringBuilder Filter(this StringBuilder value, Func<char, int, bool> condition) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));
      if (condition is null)
        throw new ArgumentNullException(nameof(condition));

      int index = 0;

      for (int i = 0; i < value.Length; ++i) {
        char c = value[i];

        if (condition(c, i))
          value[index++] = c;
      }

      value.Length = index;

      return value;
    }

    /// <summary>
    /// As Enumerable
    /// </summary>
    /// <param name="value">StringBuilder</param>
    public static IEnumerable<char> AsEnumerable(this StringBuilder value) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));

      for (int i = 0; i < value.Length; ++i)
        yield return value[i];
    }

    #endregion Public
  }

}
