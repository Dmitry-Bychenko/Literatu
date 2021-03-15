using System;
using System.Numerics;
using System.Text;

namespace Literatu.Numerics {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Radix manipulation
  /// radix should be in [2..36] range
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class Radix {
    #region Public

    /// <summary>
    /// Character to digit
    /// </summary>
    public static int ToDigit(char value) {
      if (value >= '0' && value <= '9')
        return value - '0';
      else if (value >= 'a' && value <= 'z')
        return value - 'a' + 10;
      else if (value >= 'A' && value <= 'Z')
        return value - 'A' + 10;
      else
        return -1;
    }

    /// <summary>
    /// From digit to correponding character
    /// </summary>
    public static char FromDigit(int value) {
      if (value < 0 || value > 36)
        return '?';
      else if (value < 10)
        return (char)('0' + value);
      else
        return (char)('a' + value - 10);
    }

    /// <summary>
    /// Is value a valid radix number representation
    /// </summary>
    public static bool IsValid(string value, int radix) {
      if (string.IsNullOrEmpty(value))
        return false;
      else if (radix <= 1 || radix > 36)
        return false;

      bool first = true;
      bool hasDigit = false;

      foreach (char c in value) {
        if (first) {
          first = false;

          if (c == '-' || c == '+')
            continue;
        }

        int v = ToDigit(c);

        if (v < 0 || v >= radix)
          return false;

        hasDigit = true;
      }

      return hasDigit;
    }

    /// <summary>
    /// Try Convert from radix to radix
    /// </summary>
    public static bool TryConvert(string value, int fromRadix, int toRadix, out string result) {
      result = null;

      if (string.IsNullOrEmpty(value))
        return false;
      else if (fromRadix <= 1 || fromRadix > 36)
        return false;
      else if (toRadix <= 1 || toRadix > 36)
        return false;

      if (fromRadix == toRadix) {
        if (IsValid(value, fromRadix)) {
          result = value;

          return true;
        }

        return false;
      }

      bool first = true;
      bool sign = false;
      bool hasDigit = false;

      BigInteger number = 0;

      for (int i = 0; i < value.Length; ++i) {
        char c = value[i];

        if (first) {
          first = false;

          if (c == '+')
            continue;
          else if (c == '-') {
            sign = true;

            continue;
          }
        }

        int digit = ToDigit(c);

        if (digit < 0 || digit >= fromRadix)
          return false;

        number = number * fromRadix + digit;
        hasDigit = true;
      }

      if (!hasDigit)
        return false;

      StringBuilder sb = new();

      for (; number > 0; number /= toRadix) {
        int v = (int)(number % toRadix);

        sb.Append(FromDigit(v));
      }

      if (sb.Length <= 0)
        sb.Append('0');
      else if (sign)
        sb.Append('-');

      sb.Reverse();

      result = sb.ToString();

      return true;
    }

    /// <summary>
    /// Convert from radix to radix
    /// </summary>
    public static string Convert(string value, int fromRadix, int toRadix) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));
      else if (string.IsNullOrEmpty(value))
        throw new FormatException("Empty String can't be converted");
      else if (fromRadix <= 1 || fromRadix > 36)
        throw new ArgumentOutOfRangeException(nameof(fromRadix));
      else if (toRadix <= 1 || toRadix > 36)
        throw new ArgumentOutOfRangeException(nameof(toRadix));

      if (TryConvert(value, fromRadix, toRadix, out string result))
        return result;
      else
        throw new FormatException("Invalid value format.");
    }

    #endregion Public
  }

}
