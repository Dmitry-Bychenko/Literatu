using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Literatu.Text {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Escaper (applies and removes escapement)
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public interface IEscaper {
    /// <summary>
    /// Escape required characters within string
    /// </summary>
    string Escape(string value);

    /// <summary>
    /// Try to Remove Escapement
    /// </summary>
    bool TryUnescape(string value, out string result);
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Escaper extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static class EscaperExtensions {
    #region Public

    /// <summary>
    /// Unescape
    /// </summary>
    public static string Unescape(this IEscaper rules, string value) {
      if (rules is null)
        throw new ArgumentNullException(nameof(rules));

      if (rules.TryUnescape(value, out string result))
        return result;

      throw new FormatException("String is not properly escaped.");
    }

    #endregion Public
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Escaper 
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public class Escaper : IEscaper, IEquatable<Escaper>, ISerializable {
    #region Private Data

    private readonly HashSet<char> m_MustBeEscaped;

    #endregion Private Data

    #region Algorithm

    /// <summary>
    /// Escape required characters within string
    /// </summary>
    public string CoreEscape(string value) {
      if (string.IsNullOrEmpty(value))
        return value;

      StringBuilder sb = new(value.Length * 2);

      foreach (char c in value) {
        if (c == EscapeSymbol && HasEscapeSymbol) 
          sb.Append(c);
        else if (m_MustBeEscaped.Contains(c)) {
          if (HasEscapeSymbol)
            sb.Append(EscapeSymbol);
          else
            sb.Append(c);
        }

        sb.Append(c);
      }

      return sb.ToString();
    }

    /// <summary>
    /// Try to Remove Escapement
    /// </summary>
    public bool CoreTryUnescape(string value, out string result) {
      if (string.IsNullOrEmpty(value)) {
        result = value;

        return true;
      }

      result = default;

      StringBuilder sb = new (value.Length);

      for (int i = 0; i < value.Length; ++i) {
        char c = value[i];

        if (c == EscapeSymbol && HasEscapeSymbol) {
          if (i == value.Length - 1)
            return false;

          c = value[++i];

          if (c == EscapeSymbol || m_MustBeEscaped.Contains(c))
            sb.Append(c);
          else
            return false;
        }
        else if (m_MustBeEscaped.Contains(c)) {
          if (HasEscapeSymbol)
            return false;

          if (i == value.Length - 1)
            return false;

          char d = value[++i];

          if (c == d)
            sb.Append(c);
          else
            return false;
        }
        else
          sb.Append(c);
      }

      result = sb.ToString();

      return true;
    }

    #endregion Algorithm

    #region Create

    // Deserialization
    private Escaper(SerializationInfo info, StreamingContext context) {
      if (info is null)
        throw new ArgumentNullException(nameof(info));

      string typeName = info.GetString("comparer");

      IEqualityComparer<char> comparer = Activator.CreateInstance(Type.GetType(typeName)) as IEqualityComparer<char>;

      m_MustBeEscaped = new HashSet<char>(info.GetString("escape"), comparer);

      HasEscapeSymbol = info.GetBoolean("hasSymbol");

      if (HasEscapeSymbol)
        EscapeSymbol = info.GetChar("symbol");
    }

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="mustBeEscaped">Symbols which must be escaped</param>
    /// <param name="escapeSymbol">Escape Symbol to use</param>
    /// <param name="comparer">Comparer to use</param>
    public Escaper(IEnumerable<char> mustBeEscaped, char escapeSymbol, IEqualityComparer<char> comparer) {
      if (null == mustBeEscaped)
        throw new ArgumentNullException(nameof(mustBeEscaped));

      m_MustBeEscaped = new HashSet<char>(mustBeEscaped, comparer ?? EqualityComparer<char>.Default);

      EscapeSymbol = escapeSymbol;
      HasEscapeSymbol = true;
    }

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="mustBeEscaped">Symbols which must be escaped</param>
    /// <param name="escapeSymbol">Escape Symbol to use</param>
    public Escaper(IEnumerable<char> mustBeEscaped, char escapeSymbol)
      : this(mustBeEscaped, escapeSymbol, null) { }

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="mustBeEscaped">Symbols which must be escaped</param>
    /// <param name="comparer">Comparer to use</param>
    public Escaper(IEnumerable<char> mustBeEscaped, IEqualityComparer<char> comparer) {
      if (null == mustBeEscaped)
        throw new ArgumentNullException(nameof(mustBeEscaped));

      m_MustBeEscaped = new HashSet<char>(mustBeEscaped, comparer ?? EqualityComparer<char>.Default);

      EscapeSymbol = '\0';
      HasEscapeSymbol = false;
    }

    /// <summary>
    /// Standard constructor
    /// </summary>
    /// <param name="mustBeEscaped">Symbols which must be escaped</param>
    public Escaper(IEnumerable<char> mustBeEscaped)
      : this(mustBeEscaped, null) { }

    #endregion Create

    #region Standard Instances

    /// <summary>
    /// Empty escaper, doesn't escape anything
    /// </summary>
    public static Escaper Empty { get; } = new Escaper(Array.Empty<char>());

    /// <summary>
    /// C String
    /// </summary>
    public static Escaper CString { get; } = new Escaper(new char[] { '"' }, '\\');

    /// <summary>
    /// Pascal String
    /// </summary>
    public static Escaper PascalString { get; } = new Escaper(new char[] { '\'' });

    /// <summary>
    /// Oracle String
    /// </summary>
    public static Escaper OracleString { get; } = new Escaper(new char[] { '"' });

    /// <summary>
    /// Regular String
    /// </summary>
    public static Escaper RegularString { get; } = new Escaper(new char[] { 
      '(', ')', '[', ']', '{', '}', '|', '.', ',', '-', 
      '^', '$', '+', '*', '?', '!', '#'
    }, '\\');

    #endregion Standard Instances

    #region Public

    /// <summary>
    /// Escape Symbol
    /// </summary>
    public char EscapeSymbol { get; }

    /// <summary>
    /// Has Escape Symbol
    /// </summary>
    public bool HasEscapeSymbol { get; }

    /// <summary>
    /// Symbols to Escape
    /// </summary>
    public char[] SymbolsToEscape => m_MustBeEscaped.OrderBy(x => x).ToArray();

    /// <summary>
    /// To String (debug only)
    /// </summary>
    public override string ToString() {
      string symbols = string.Join(", ", m_MustBeEscaped.Select(c => $"'{c}'"));

      if (HasEscapeSymbol)
        return $"Escape {symbols} with '{EscapeSymbol}'";
      else
        return $"Escape {symbols} by duplicating";
    }

    #endregion Public

    #region Operators

    /// <summary>
    /// Equal
    /// </summary>
    public static bool operator == (Escaper left, Escaper right) {
      if (ReferenceEquals(left, right))
        return true;
      if ((left is null) || (right is null))
        return false;

      return left.Equals(right);
    }

    /// <summary>
    /// Not Equal
    /// </summary>
    public static bool operator !=(Escaper left, Escaper right) {
      if (ReferenceEquals(left, right))
        return false;
      if ((left is null) || (right is null))
        return true;

      return !left.Equals(right);
    }

    #endregion Operators

    #region IEscaper

    /// <summary>
    /// Escape required characters within string
    /// </summary>
    public string Escape(string value) => CoreEscape(value);

    /// <summary>
    /// Try to Remove Escapement
    /// </summary>
    public bool TryUnescape(string value, out string result) => CoreTryUnescape(value, out result);

    #endregion IEscaper

    #region IEqualityComparer<Escapement>

    /// <summary>
    /// Equals
    /// </summary>
    public bool Equals(Escaper other) {
      if (other is null)
        return false;

      if (HasEscapeSymbol != other.HasEscapeSymbol)
        return false;

      if (HasEscapeSymbol && EscapeSymbol != other.EscapeSymbol)
        return false;

      if (m_MustBeEscaped.Comparer != other.m_MustBeEscaped.Comparer)
        return false;

      return m_MustBeEscaped.SetEquals(other.m_MustBeEscaped);
    }

    /// <summary>
    /// Equals
    /// </summary>
    public override bool Equals(object o) => o is Escaper other && Equals(other);

    /// <summary>
    /// Hash Code
    /// </summary>
    public override int GetHashCode() {
      unchecked {
        return (m_MustBeEscaped.Count << 16) ^ (HasEscapeSymbol ? EscapeSymbol : '\0');
      }
    }

    #endregion IEqualityComparer<Escapement>

    #region ISerializable 

    /// <summary>
    /// Serialization
    /// </summary>
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
      if (info is null)
        throw new ArgumentNullException(nameof(info));

      info.AddValue("hasSymbol", HasEscapeSymbol);
      info.AddValue("symbol", EscapeSymbol);
      info.AddValue("escape", string.Concat(m_MustBeEscaped));
      info.AddValue("comparer", m_MustBeEscaped.Comparer.GetType().AssemblyQualifiedName);
    }

    #endregion ISerializable 
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// String Extensions
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public static partial class StringExtensions {
    #region Public

    /// <summary>
    /// Escape
    /// </summary>
    public static string Escape(this string value, IEscaper escaper) { 
      if (escaper is null)
        throw new ArgumentNullException(nameof(escaper));

      return escaper.Escape(value);
    }

    /// <summary>
    /// Try Unescape
    /// </summary>
    public static bool TryUnescape(this string value, out string result, IEscaper escaper) {
      if (escaper is null)
        throw new ArgumentNullException(nameof(escaper));

      return escaper.TryUnescape(value, out result);
    }

    /// <summary>
    /// Unscape
    /// </summary>
    public static string Unescape(this string value, IEscaper escaper) {
      if (escaper is null)
        throw new ArgumentNullException(nameof(escaper));

      return escaper.Unescape(value);
    }

    #endregion Public
  }

}
