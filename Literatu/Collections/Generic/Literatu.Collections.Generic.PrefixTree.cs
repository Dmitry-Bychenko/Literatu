using System;
using System.Collections.Generic;

namespace Literatu.Collections.Generic {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Prefix Tree Node
  /// </summary>
  /// <typeparam name="T"></typeparam>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class PrefixTreeNode<T> {
    #region Private Data

    private readonly Dictionary<T, PrefixTreeNode<T>> m_Items;

    #endregion Private Data

    #region Algorithm

    internal PrefixTreeNode<T> Add(T value) {
      if (m_Items.TryGetValue(value, out var result))
        return result;

      result = new PrefixTreeNode<T>(value, Comparer);

      m_Items.Add(value, result);

      return result;
    }

    internal bool Remove(T value) => m_Items.Remove(value);

    internal void Clear() => m_Items.Clear();

    #endregion Algorithm

    #region Create

    internal PrefixTreeNode(T value, IEqualityComparer<T> comparer) {
      Value = value;

      m_Items = new Dictionary<T, PrefixTreeNode<T>>(comparer);
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Value
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Comparer
    /// </summary>
    public IEqualityComparer<T> Comparer => m_Items.Comparer;

    /// <summary>
    /// Final Count
    /// </summary>
    public int FinalCount { get; internal set; }

    /// <summary>
    /// Is Final
    /// </summary>
    public bool IsFinal => FinalCount > 0;

    /// <summary>
    /// Frequency
    /// </summary>
    public int Frequency {
      get {
        int result = 0;

        foreach (var item in m_Items.Values)
          result += item.Frequency;

        return result;
      }
    }

    /// <summary>
    /// Items
    /// </summary>
    public IReadOnlyDictionary<T, PrefixTreeNode<T>> Items => m_Items;

    /// <summary>
    /// To String
    /// </summary>
    public override string ToString() => Value?.ToString() ?? "[null]";

    #endregion Public
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Prefix Tree (Trie), Generic
  /// </summary>
  /// <typeparam name="T"></typeparam>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public class PrefixTree<T> : IEnumerable<T[]> {
    #region Create

    /// <summary>
    /// Standard Constructor
    /// </summary>
    public PrefixTree(IEnumerable<T> value, IEqualityComparer<T> comparer) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));

      Comparer = comparer ?? EqualityComparer<T>.Default;

      Root = new PrefixTreeNode<T>(default, Comparer);

      Add(value);
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    public PrefixTree(IEnumerable<T> value) : this(value, null) { }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    public PrefixTree(IEqualityComparer<T> comparer) {
      Comparer = comparer ?? EqualityComparer<T>.Default;

      Root = new PrefixTreeNode<T>(default, Comparer);
    }

    /// <summary>
    /// Standard Constructor
    /// </summary>
    public PrefixTree() {
      Comparer = EqualityComparer<T>.Default;

      Root = new PrefixTreeNode<T>(default, Comparer);
    }

    #endregion Create

    #region Public

    /// <summary>
    /// Comparer
    /// </summary>
    public IEqualityComparer<T> Comparer { get; }

    /// <summary>
    /// Root
    /// </summary>
    public PrefixTreeNode<T> Root { get; private set; }

    /// <summary>
    /// Add Sequence To Trie
    /// </summary>
    public PrefixTreeNode<T> Add(IEnumerable<T> value) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));

      PrefixTreeNode<T> result = Root;

      foreach (var item in value)
        result = result.Add(item);

      result.FinalCount += 1;

      Count += 1;

      return result;
    }

    /// <summary>
    /// AddRange
    /// </summary>
    public int AddRange(IEnumerable<IEnumerable<T>> values) {
      if (values is null)
        throw new ArgumentNullException(nameof(values));

      int result = 0;

      foreach (var value in values) {
        if (value is not null) {
          Add(value);

          result += 1;
        }
      }

      return result;
    }

    /// <summary>
    /// Find if sequence (value is in the trie)
    /// </summary>
    public PrefixTreeNode<T> Find(IEnumerable<T> value) {
      if (value is null)
        throw new ArgumentNullException(nameof(value));

      PrefixTreeNode<T> result = Root;

      foreach (var item in value)
        if (!result.Items.TryGetValue(item, out result))
          return null;

      return result;
    }

    /// <summary>
    /// Has Prefix
    /// </summary>
    public bool HasPrefix(IEnumerable<T> value) {
      PrefixTreeNode<T> result = Find(value);

      if (result is null)
        return false;

      return result.Items.Count > 0;
    }

    /// <summary>
    /// Has Value
    /// </summary>
    public bool HasValue(IEnumerable<T> value) {
      PrefixTreeNode<T> result = Find(value);

      if (result is null)
        return false;

      return result.FinalCount > 0;
    }

    /// <summary>
    /// Remove
    /// </summary>
    public bool Remove(IEnumerable<T> value) {
      if (value is null)
        return false;

      LinkedList<PrefixTreeNode<T>> list = new();

      PrefixTreeNode<T> last = Root;

      foreach (T item in value) {
        if (!last.Items.TryGetValue(item, out last))
          return false;

        list.AddLast(last);
      }

      if (!last.IsFinal)
        return false;

      last.FinalCount -= 1;

      while (true) {
        if (last is not null || last.Items.Count > 0 || last.FinalCount > 0)
          break;

        PrefixTreeNode<T> prior = list.Last.Previous?.Value;

        list.RemoveLast();

        prior?.Remove(last.Value);

        last = prior;
      }

      Count -= 1;

      return true;
    }

    /// <summary>
    /// Remove Range
    /// </summary>
    public int RemoveRange(IEnumerable<IEnumerable<T>> values) {
      if (values is null)
        throw new ArgumentNullException(nameof(values));

      int result = 0;

      foreach (var value in values)
        if (Remove(value))
          result += 1;

      return result;
    }

    /// <summary>
    /// Clear
    /// </summary>
    public void Clear() {
      Root.Clear();

      Count = 0;
    }

    /// <summary>
    /// Count
    /// </summary>
    public int Count {
      get;
      private set;
    }

    /// <summary>
    /// Sequences
    /// </summary>
    public IEnumerable<T[]> Sequences() {
      if (Root.Items.Count <= 0)
        yield break;

      Stack<PrefixTreeNode<T>> agenda = new();

      agenda.Push(Root);

      Dictionary<PrefixTreeNode<T>, PrefixTreeNode<T>> parents = new();

      parents.Add(Root, null);

      while (agenda.Count > 0) {
        var node = agenda.Pop();

        if (node.IsFinal) {
          List<T> list = new();

          for (var item = node; item != Root; item = parents[item])
            list.Add(item.Value);

          list.Reverse();

          for (int i = 0; i < node.FinalCount; ++i)
            yield return list.ToArray();
        }

        foreach (var item in node.Items.Values) {
          agenda.Push(item);
          parents.Add(item, node);
        }
      }
    }

    #endregion Public

    #region IEnumerable<T[]>

    /// <summary>
    /// Typed Enumerator
    /// </summary>
    public IEnumerator<T[]> GetEnumerator() => Sequences().GetEnumerator();

    /// <summary>
    /// Typed Enumerator
    /// </summary>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => Sequences().GetEnumerator();

    #endregion IEnumerable<T[]>
  }

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Prefix Tree (Trie), Character
  /// </summary>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class PrefixTree : PrefixTree<char> {
    #region Public

    /// <summary>
    /// Words
    /// </summary>
    public IEnumerable<string> Words() {
      foreach (var array in Sequences())
        yield return new string(array);
    }

    #endregion Public
  }

}
