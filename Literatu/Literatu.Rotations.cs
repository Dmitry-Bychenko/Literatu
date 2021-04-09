using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Literatu {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// String Rotations
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class StringRotation {
    #region Public

    /// <summary>
    /// String rotation (to left if shift is positive)
    /// </summary>
    /// <param name="value">String to rotate</param>
    /// <param name="shift">Positions to rotate (positive to left, negative to right)</param>
    public static string Rotate(string value, int shift) {
      if (string.IsNullOrEmpty(value))
        return value;

      shift = (shift % value.Length + value.Length) % value.Length;

      if (shift == 0)
        return value;

      return value[shift..] + value[0..shift]; 
    }

    /// <summary>
    /// Shift which returns smallest rotated string
    /// "bcda" -> 3 since Rotate("bcda", s) == "abcd" 
    /// </summary>
    ///<remarks>Booth's Algorithm</remarks>
    public static int LeastRotationShift(string value) {
      if (string.IsNullOrEmpty(value))
        return 0;

      int result = 0;
      int[] fails = new int[value.Length];

      for (int i = fails.Length - 1; i >= 0; --i)
        fails[i] = -1;

      for (int i = 1; i < value.Length; ++i) {
        char ch = value[i];
        int index = fails[i - result - 1];

        while (index != -1 && ch != value[(result + index + 1) % value.Length]) {
          if (ch < value[(result + index + 1) % value.Length])
            result = i - index - 1;

          index = fails[index];
        }

        if (ch != value[(result + index + 1) % value.Length]) {
          if (ch < value[result])
            result = i;

          fails[i - result] = -1;
        }
        else
          fails[i - result] = index + 1;
      }

      return result;
    }

    /// <summary>
    /// Cyclic string character at position
    /// </summary>
    /// <param name="value">String</param>
    /// <param name="index">Index (cyclic)</param>
    /// <returns></returns>
    public static char CharAt(this string value, int index) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));
      if (value.Length == 0)
        throw new ArgumentOutOfRangeException(nameof(index));

      return value[(index % value.Length + value.Length) % value.Length];
    }

    #endregion Public
  }

}
