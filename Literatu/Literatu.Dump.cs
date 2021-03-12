using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Literatu {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Text Dump
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class TextDump {
    #region Public

    //-----------------------------------------------------------------------------------------------------------------
    //
    /// <summary>
    /// Dump 
    /// </summary>
    //
    //-----------------------------------------------------------------------------------------------------------------

    public static string Dump(this char value) {
      var category = char.GetUnicodeCategory(value);

      string result = string.Join(" ",
        $"{(value.IsVisible() ? "'{value}'": "[invisible]")}",
        $"(\\u{((int) value):x4})",
        $"{category}"
      );

      return result;
    }

    #endregion Public
  }

}
