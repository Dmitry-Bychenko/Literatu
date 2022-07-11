using System;

namespace Literatu {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Emoji
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class Emoji {
    #region Public

    /// <summary>
    /// Encode: having two characters get Emoji code
    /// </summary>
    /// <param name="left">Left symbol</param>
    /// <param name="right">Right symbol</param>
    /// <returns>Emoji code</returns>
    /// <exception cref="ArgumentOutOfRangeException">When either left or right is out of range</exception>
    public static int Encode(char left, char right) {
      if (left < 0xD800 || left > 0xDFFF)
        throw new ArgumentOutOfRangeException(nameof(left));
      if (right < 0xDC00 || right > 0xDFFF)
        throw new ArgumentOutOfRangeException(nameof(right));

      // 0xDFFF
      return 0x10000 + (left - 0xD800) * 0x0400 + right - 0xDC00;
    }

    /// <summary>
    /// Split single Emoji code into two chars
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">When code is out of range</exception>
    public static (char left, char right) Split(int code) {
      if (code < 0x10000)
        throw new ArgumentOutOfRangeException(nameof(code));

      return (
        (char)(((code - 0x10000) >> 10) + 0xD800),
        (char)(((code - 0x10000) & 0b1111_111_111) + 0xDC00)
      );
    }

    /// <summary>
    /// Decode singe Emoji code into string
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">When code is out of range</exception>
    public static string Decode(int code) {
      if (code < 0x10000)
        throw new ArgumentOutOfRangeException(nameof(code));

      return new string(new char[] {
        (char) (((code - 0x10000) >> 10) + 0xD800),
        (char) (((code - 0x10000) & 0b1111_111_111) + 0xDC00)
      });
    }

    #endregion Public
  }

}
