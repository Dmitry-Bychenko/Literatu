using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Literatu.Security {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Secure String
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class SecureStringHelper {
    #region Public

    /// <summary>
    /// From Ordinal String
    /// </summary>
    public static SecureString FromOrdinalString(string value) {
      if (value is null)
        return null;

      SecureString result = new();

      foreach (char c in value)
        result.AppendChar(c);

      return result;
    }

    /// <summary>
    /// To Ordinal String
    /// </summary>
    public static string ToOrdinalString(this SecureString value) {
      if (value is null)
        return null;

      IntPtr ptr = IntPtr.Zero;

      try {
        ptr = Marshal.SecureStringToGlobalAllocUnicode(value);

        return Marshal.PtrToStringUni(ptr);
      }
      finally {
        Marshal.ZeroFreeGlobalAllocUnicode(ptr);
      }
    }

    #endregion Public
  }

}
