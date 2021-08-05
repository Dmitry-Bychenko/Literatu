using System;
using System.Collections.Generic;

namespace Literatu {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Control Characters
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class ControlCharacter {
    #region Private Data

    private static readonly Dictionary<string, char> s_Reverse = new(StringComparer.OrdinalIgnoreCase) {
      { @"\0", '\0' },
      { @"\a", '\a' },
      { @"\b", '\b' },
      { @"\f", '\f' },
      { @"\n", '\n' },
      { @"\r", '\r' },
      { @"\t", '\t' },
      { @"\v", '\v' },

      { @"NUL", (char)0 },
      { @"SOH", (char)1 },
      { @"STX", (char)2 },
      { @"ETX", (char)3 },
      { @"EOT", (char)4 },
      { @"ENQ", (char)5 },
      { @"ACK", (char)6 },
      { @"BEL", (char)7 },
      { @"BS", (char)8 },
      { @"TAB", (char)9 },
      { @"LF", (char)10 },
      { @"VT", (char)11 },
      { @"FF", (char)12 },
      { @"CR", (char)13 },
      { @"SO", (char)14 },
      { @"SI", (char)15 },

      { @"DLE", (char)16 },
      { @"DC1", (char)17 },
      { @"DC2", (char)18 },
      { @"DC3", (char)19 },
      { @"DC4", (char)20 },
      { @"NAK", (char)21 },
      { @"SYN", (char)22 },
      { @"ETB", (char)23 },
      { @"CAN", (char)24 },
      { @"EM", (char)25 },
      { @"SUB", (char)26 },
      { @"ESC", (char)27 },
      { @"FS", (char)28 },
      { @"GS", (char)29 },
      { @"RS", (char)30 },
      { @"US", (char)31 },
    };

    private static readonly List<string> s_Names = new() {
      "NUL",
      "SOH",
      "STX",
      "ETX",
      "EOT",
      "ENQ",
      "ACK",
      "BEL",
      "BS",
      "TAB",
      "LF",
      "VT",
      "FF",
      "CR",
      "SO",
      "SI",
      "DLE",
      "DC1",
      "DC2",
      "DC3",
      "DC4",
      "NAK",
      "SYN",
      "ETB",
      "CAN",
      "EM",
      "SUB",
      "ESC",
      "FS",
      "GS",
      "RS",
      "US",
    };

    #endregion Private Data

    #region Constants

    /// <summary>
    /// Null constant (0x00)
    /// </summary>
    public const char NULL = '\0';

    /// <summary>
    /// Start Of Heading (0x01)
    /// </summary>
    public const char SOH = (char)0x01;

    /// <summary>
    /// Start of TeXt 
    /// </summary>
    public const char STX = (char)0x02;

    /// <summary>
    /// End of TeXt  
    /// </summary>
    public const char ETX = (char)0x03;

    /// <summary>
    /// End Of Transmission
    /// </summary>
    public const char EOT = (char)0x04;

    /// <summary>
    /// Enquiry 
    /// </summary>
    public const char ENQ = (char)0x05;

    /// <summary>
    /// Acknowledge 
    /// </summary>
    public const char ACK = (char)0x06;

    /// <summary>
    /// Bell 
    /// </summary>
    public const char BEL = (char)0x07;

    /// <summary>
    /// BackSpace 
    /// </summary>
    public const char BS = (char)0x08;

    /// <summary>
    /// Horizontal Tabulation 
    /// </summary>
    public const char HT = (char)0x09;

    /// <summary>
    /// Line feed
    /// </summary>
    public const char LF = (char)0x0A;

    /// <summary>
    /// Vertical tabulation 
    /// </summary>
    public const char VT = (char)0x0B;

    /// <summary>
    /// Form Feed
    /// </summary>
    public const char FF = (char)0x0C;

    /// <summary>
    /// Carriage Return 
    /// </summary>
    public const char CR = (char)0x0D;

    /// <summary>
    /// Shift Out 
    /// </summary>
    public const char SO = (char)0x0E;

    /// <summary>
    /// Shift In 
    /// </summary>
    public const char SI = (char)0x0F;

    /// <summary>
    /// Data Link Escape 
    /// </summary>
    public const char DLE = (char)0x10;

    /// <summary>
    /// Device Control 1 
    /// </summary>
    public const char DC1 = (char)0x11;

    /// <summary>
    /// Device Control 2 
    /// </summary>
    public const char DC2 = (char)0x12;

    /// <summary>
    /// Device control 3 
    /// </summary>
    public const char DC3 = (char)0x13;

    /// <summary>
    /// Device Control 4 
    /// </summary>
    public const char DC4 = (char)0x14;

    /// <summary>
    /// Negative AcKnowledge 
    /// </summary>
    public const char NAK = (char)0x15;

    /// <summary>
    /// Synchronize 
    /// </summary>
    public const char SYN = (char)0x16;

    /// <summary>
    /// End of Transmission Block 
    /// </summary>
    public const char ETB = (char)0x17;

    /// <summary>
    /// Cancel 
    /// </summary>
    public const char CAN = (char)0x18;

    /// <summary>
    /// End of Medium 
    /// </summary>
    public const char EM = (char)0x19;

    /// <summary>
    /// Substitute 
    /// </summary>
    public const char SUB = (char)0x1A;

    /// <summary>
    /// Escape 
    /// </summary>
    public const char ESC = (char)0x1B;

    /// <summary>
    /// File Separator 
    /// </summary>
    public const char FS = (char)0x1C;

    /// <summary>
    /// Group Separator 
    /// </summary>
    public const char GS = (char)0x1D;

    /// <summary>
    /// Record Separator 
    /// </summary>
    public const char RS = (char)0x1E;

    /// <summary>
    /// Unit Separator 
    /// </summary>
    public const char US = (char)0x1F;

    /// <summary>
    /// Space 
    /// </summary>
    public const char SPC = (char)0x20;

    /// <summary>
    /// Standard replacement: �
    /// </summary>
    public const char REPLACEMENT = (char)0xFFFD;

    #endregion Constants

    #region Public

    /// <summary>
    /// Names
    /// </summary>
    public static IReadOnlyList<string> Names => s_Names;

    /// <summary>
    /// Slashed Name, like \n, \r etc.
    /// </summary>
    public static string SlashedName(char value) => value switch {
      '\0' => "\\0",
      '\a' => "\\a",
      '\b' => "\\b",
      '\f' => "\\f",
      '\n' => "\\n",
      '\r' => "\\r",
      '\t' => "\\t",
      '\v' => "\\v",
      _ => ""
    };

    /// <summary>
    /// Try Parse
    /// </summary>
    public static bool TryParse(string value, out char result) {
      result = default;

      if (string.IsNullOrEmpty(value))
        return false;

      if (value.Length == 1) {
        result = value[0];

        return true;
      }

      value = value.Trim();

      if (value.Length == 1) {
        result = value[0];

        return true;
      }

      if (s_Reverse.TryGetValue(value, out var r)) {
        result = r;

        return true;
      }

      return false;
    }

    #endregion Public
  }

}
