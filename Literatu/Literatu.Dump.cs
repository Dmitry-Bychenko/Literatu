using System.Linq;

namespace Literatu {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Text Dump
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class TextDump {
    #region Algorithm

    private static string InStringChar(char value) {
      if (value == '\'')
        return $"'\\''[0x{((int)value):x4}]";
      else if (char.IsLetterOrDigit(value) || char.IsPunctuation(value) || char.IsSymbol(value))
        return $"'{value}'[0x{((int)value):x4}]";
      else if (value < ' ') {
        string st = ControlCharacter.SlashedName(value);

        if (!string.IsNullOrEmpty(st))
          return $"'{st}'[0x{((int)value):x4}]";
        else
          return $"'\\u{((int)value):x4}'";
      }
      else
        return $"'\\u{((int)value):x4}'";
    }

    #endregion Algorithm

    #region Public

    /// <summary>
    /// Character dump
    /// </summary>
    public static string Dump(this char value) {
      var category = char.GetUnicodeCategory(value);

      string result = string.Join(" ",
        $"{(value.IsVisible() ? "'{value}'" : "[invisible]")}",
        $"(\\u{((int)value):x4})",
        $"{category}"
      );

      return result;
    }

    /// <summary>
    /// String Dump
    /// </summary>
    public static string Dump(this string value) {
      if (value is null)
        return "[null]";
      if (string.IsNullOrEmpty(value))
        return "[empty]";

      return string.Join(" ", value
        .Select(c => InStringChar(c)));
    }

    #endregion Public
  }

}
