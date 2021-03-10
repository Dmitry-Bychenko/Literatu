using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Literatu.Text {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Quotation Interface
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public interface IQuotation {
    /// <summary>
    /// Open Quotation Mark
    /// </summary>
    char OpenQuotation { get; }

    /// <summary>
    /// Open Quotation Escapement
    /// </summary>
    char OpenQuotationEscapement { get; }

    /// <summary>
    /// Close Quotation Mark
    /// </summary>
    char CloseQuotation { get; }

    /// <summary>
    /// Close Quotation Escapement
    /// </summary>
    char CloseQuotationEscapement { get; }
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class Quotation {
    #region Inner Classes

    private sealed class PascalQuotation : IQuotation {
      public char OpenQuotation => '\'';

      public char OpenQuotationEscapement => '\'';

      public char CloseQuotation => '\'';

      public char CloseQuotationEscapement => '\'';
    }

    private sealed class CQuotation : IQuotation {
      public char OpenQuotation => '"';

      public char OpenQuotationEscapement => '\\';

      public char CloseQuotation => '"';

      public char CloseQuotationEscapement => '\\';
    }

    private sealed class OracleQuotation : IQuotation {
      public char OpenQuotation => '"';

      public char OpenQuotationEscapement => '"';

      public char CloseQuotation => '"';

      public char CloseQuotationEscapement => '"';
    }

    private sealed class MsSqlQuotation : IQuotation {
      public char OpenQuotation => '[';

      public char OpenQuotationEscapement => '\0';

      public char CloseQuotation => ']';

      public char CloseQuotationEscapement => ']';
    }

    private sealed class InterpolatedQuotation : IQuotation {
      public char OpenQuotation => '{';

      public char OpenQuotationEscapement => '{';

      public char CloseQuotation => '}';

      public char CloseQuotationEscapement => '}';
    }

    #endregion Inner Classes

    #region Public

    /// <summary>
    /// Pascal Quotation Style: ab\c'de"f[g]h => 'abc''de"f[g]h' 
    /// </summary>
    public static IQuotation Pascal { get; } = new PascalQuotation();

    /// <summary>
    /// С Quotation Style: ab\c'de"f[g]h => "ab\\c'de\"f[g]h" 
    /// </summary>
    public static IQuotation C { get; } = new CQuotation();

    /// <summary>
    /// Oracle Quotation Style: ab\c'de"f[g]h => "ab\c'de""f[g]h" 
    /// </summary>
    public static IQuotation Oracle { get; } = new OracleQuotation();

    /// <summary>
    /// MS SQL Quotation Style: ab\c'de"f[g]h => [ab\c'de"f[g]]h] 
    /// </summary>
    public static IQuotation MsSql { get; } = new MsSqlQuotation();

    /// <summary>
    /// Interpolated Quotation Style: ab\c'de"{f}[g]h => {ab\c'de"{{f}}[g]h} 
    /// </summary>
    public static IQuotation Interpolated { get; } = new InterpolatedQuotation();

    #endregion Public
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Quotations
  /// </summary>
  /// <example>
  /// <code language="C#">
  /// string demo = "abc\"d";
  /// Console.WriteLine($"{demo} -> {deno.QuotationAdd('\"')}");
  /// </code>
  /// </example>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class Quotations {
    #region Public

    /// <summary>
    /// Quotation Add
    /// </summary>
    /// <param name="value">String to Enquote</param>
    /// <param name="rules">Rules to use</param>
    /// <returns>Enquoted string</returns>
    public static string QuotationAdd(this string value, IQuotation rules) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));
      if (rules is null)
        throw new ArgumentNullException(nameof(rules));

      return QuotationAdd(value, 
                          rules.OpenQuotation, 
                          rules.OpenQuotationEscapement,
                          rules.CloseQuotation,
                          rules.CloseQuotationEscapement);
    }

    /// <summary>
    /// Quotation Add
    /// </summary>
    /// <param name="value">String to enquote</param>
    /// <param name="openQuotation">Open Quotation Mark</param>
    /// <param name="openEscapement">Open Quotation Escapement</param>
    /// <param name="closeQuotation">Close Quotation Mark</param>
    /// <param name="closeEscapement">Close Quotation Escapement</param>
    /// <returns>Enquoted String</returns>
    public static string QuotationAdd(this string value,
                                           char openQuotation,
                                           char openEscapement,
                                           char closeQuotation,
                                           char closeEscapement) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));

      StringBuilder sb = new StringBuilder(2 * value.Length + 2);

      sb.Append(openQuotation);

      foreach (char c in value) {
        if (c == openQuotation || c == openEscapement || openEscapement != '\0')
          sb.Append(openEscapement);
        else if (c == closeQuotation || c == closeEscapement)
          sb.Append(closeEscapement);

        sb.Append(c);
      }

      sb.Append(closeQuotation);

      return sb.ToString();
    }

    /// <summary>
    /// Quotation Add
    /// </summary>
    /// <param name="value">String to enquote</param>
    /// <param name="quotation">Quotation Mark</param>
    /// <param name="escapement">Escapement</param>
    /// <returns>Enquoted String</returns>
    public static string QuotationAdd(this string value,
                                           char quotation,
                                           char escapement) =>
      QuotationAdd(value, quotation, escapement, quotation, escapement);

    /// <summary>
    /// Quotation Add
    /// </summary>
    /// <param name="value">String to enquote</param>
    /// <param name="quotation">Quotation Mark</param>
    /// <returns>Enquoted String</returns>
    public static string QuotationAdd(this string value,
                                           char quotation) =>
      QuotationAdd(value, quotation, quotation, quotation, quotation);

    /// <summary>
    /// Quotation Add
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string QuotationAdd(this string value) =>
      QuotationAdd(value, '"', '"', '"', '"');

    /// <summary>
    /// Try Remove Quotation
    /// </summary>
    /// <param name="value">String to remove quotation from</param>
    /// <param name="result">string without quotation or null</param>
    /// <param name="rules">Rules to use</param>
    /// <returns>true, if quotation has been removed</returns>
    public static bool TryQuotationRemove(this string value, out string result, IQuotation rules) {
      result = default;

      if (value is null)
        return false;
      if (rules is null)
        return false;

      return TryQuotationRemove(value, 
                                out result,
                                rules.OpenQuotation,
                                rules.OpenQuotationEscapement,
                                rules.CloseQuotation,
                                rules.CloseQuotationEscapement);
    }

    /// <summary>
    /// Try Remove Quotation
    /// </summary>
    /// <param name="value">String to remove quotation from</param>
    /// <param name="result">String without quotation or null</param>
    /// <param name="openQuotation">Open Quotation Mark</param>
    /// <param name="openEscapement">Open Quotation Mark Escapement</param>
    /// <param name="closeQuotation">Close Quotation</param>
    /// <param name="closeEscapement">Close Escapement</param>
    /// <returns>true, if quotation has been removed; false otherwise</returns>
    public static bool TryQuotationRemove(this string value,
                                           out string result,
                                               char openQuotation,
                                               char openEscapement,
                                               char closeQuotation,
                                               char closeEscapement) {
      result = default;

      if (value is null)
        return false;
      else if (value.Length <= 1)
        return false;
      else if (value[0] != openQuotation || value[^1] != closeQuotation)
        return false;

      StringBuilder sb = new StringBuilder(value.Length);

      for (int i = 1; i < value.Length - 1; ++i) {
        char ch = value[i];

        if (ch == openEscapement || openEscapement != 0) {
          if (i == value.Length - 2)
            return false;

          i += 1;
          ch = value[i];

          if (ch != openEscapement && ch != openQuotation)
            return false;
        }
        else if (ch == openQuotation)
          return false;
        else if (ch == closeEscapement) {
          if (i == value.Length - 2)
            return false;

          i += 1;
          ch = value[i];

          if (ch != closeEscapement && ch != closeQuotation)
            return false;
        }
        else if (ch == closeQuotation)
          return false;

        sb.Append(ch);
      }

      return true;
    }

    /// <summary>
    /// Try Remove Quotation
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <param name="quotation"></param>
    /// <param name="escapement"></param>
    /// <returns></returns>
    public static bool TryQuotationRemove(this string value,
                                           out string result,
                                               char quotation,
                                               char escapement) =>
      TryQuotationRemove(value, out result, quotation, escapement, quotation, escapement);

    /// <summary>
    /// Try Remove Quotation
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <param name="quotation"></param>
    /// <returns></returns>
    public static bool TryQuotationRemove(this string value,
                                           out string result,
                                               char quotation) =>
      TryQuotationRemove(value, out result, quotation, quotation, quotation, quotation);

    /// <summary>
    /// Try Remove Quotation
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryQuotationRemove(this string value,
                                           out string result) =>
      TryQuotationRemove(value, out result, '"', '"', '"', '"');

    /// <summary>
    /// Remove Quotation
    /// </summary>
    /// <param name="value">String to remove quotation from</param>
    /// <param name="rules">Rules to use</param>
    /// <returns>string without quotation</returns>
    public static string QuotationRemove(this string value, IQuotation rules) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));
      if (rules is null)
        throw new ArgumentNullException(nameof(rules));

      return QuotationRemove(value,
                             rules.OpenQuotation,
                             rules.OpenQuotationEscapement,
                             rules.CloseQuotation,
                             rules.CloseQuotationEscapement);
    }

    /// <summary>
    /// Remove Quotation
    /// </summary>
    /// <param name="value"></param>
    /// <param name="openQuotation"></param>
    /// <param name="openEscapement"></param>
    /// <param name="closeQuotation"></param>
    /// <param name="closeEscapement"></param>
    /// <returns></returns>
    public static string QuotationRemove(this string value,
                                                char openQuotation,
                                                char openEscapement,
                                                char closeQuotation,
                                                char closeEscapement) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));

      if (value.Length <= 1)
        throw new FormatException("Incorrect value length");
      else if (value[0] != openQuotation || value[^1] != closeQuotation)
        throw new FormatException("Doesn't have start/final quotation marks");

      StringBuilder sb = new StringBuilder(value.Length);

      for (int i = 1; i < value.Length - 1; ++i) {
        char ch = value[i];

        if (ch == openEscapement) {
          if (i == value.Length - 2)
            throw new FormatException($"Dangling escapement '{openEscapement}'.");

          i += 1;
          ch = value[i];

          if (ch != openEscapement && ch != openQuotation)
            throw new FormatException($"Incorrect escapement '{openEscapement}'.");
        }
        else if (ch == openQuotation)
          throw new FormatException($"Dangling quotation '{openQuotation}'.");
        else if (ch == closeEscapement) {
          if (i == value.Length - 2)
            throw new FormatException($"Dangling escapement '{closeEscapement}'.");

          i += 1;
          ch = value[i];

          if (ch != closeEscapement && ch != closeQuotation)
            throw new FormatException($"Incorrect escapement '{closeEscapement}'.");
        }
        else if (ch == closeQuotation)
          throw new FormatException($"Dangling quotation '{closeQuotation}'.");

        sb.Append(ch);
      }

      return sb.ToString();
    }

    /// <summary>
    /// Remove Quotation
    /// </summary>
    /// <param name="value"></param>
    /// <param name="quotation"></param>
    /// <param name="escapement"></param>
    /// <returns></returns>
    public static string QuotationRemove(this string value,
                                                char quotation,
                                                char escapement) =>
      QuotationRemove(value, quotation, escapement, quotation, escapement);

    /// <summary>
    /// Remove Quotation
    /// </summary>
    /// <param name="value"></param>
    /// <param name="quotation"></param>
    /// <returns></returns>
    public static string QuotationRemove(this string value,
                                                char quotation) =>
      QuotationRemove(value, quotation, quotation, quotation, quotation);

    /// <summary>
    /// Remove Quotation
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string QuotationRemove(this string value) =>
      QuotationRemove(value, '"', '"', '"', '"');

    #endregion Public
  }

}
