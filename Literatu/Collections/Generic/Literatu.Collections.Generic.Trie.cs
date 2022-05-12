using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Literatu.Collections.Generic {

  //-------------------------------------------------------------------------------------------------------------------
  //
  /// <summary>
  /// Trie<K, V>
  /// </summary>
  /// <typeparam name="K">Key Type</typeparam>
  /// <typeparam name="V">Value Type</typeparam>
  //
  //-------------------------------------------------------------------------------------------------------------------

  public sealed class Trie<K, V> : IEnumerable<Trie<K, V>> {
    #region Private Data

    // Children
    private readonly Dictionary<K, Trie<K, V>> m_Items;

    #endregion Private Data

    #region Algorithm

    private void UpdateFrequency(int delta = 1) {
      for (Trie<K, V> current = this; current is not null; current = current.Parent)
        current.Frequency += delta;
    }

    #endregion Algorithm

    #region Create

    private Trie(Trie<K, V> parent, K key, V value = default) {
      if (parent is not null) {
        parent.m_Items.Add(key, this);

        m_Items = new Dictionary<K, Trie<K, V>>(parent.m_Items.Comparer);

        Parent = parent!;
        Key = key ?? throw new ArgumentNullException(nameof(key));
        Value = value;
      }
    }

    /// <summary>
    /// Creates Trie (root)
    /// </summary>
    public Trie(IEqualityComparer<K> comparer) {
      m_Items = (comparer is null)
        ? new Dictionary<K, Trie<K, V>>()
        : new Dictionary<K, Trie<K, V>>(comparer);

      Key = default!;
      Parent = default!;
    }

    /// <summary>
    /// Creates Trie (root)
    /// </summary>
    public Trie() : this(null) { }

    #endregion Create

    #region Public

    /// <summary>
    /// Comparer
    /// </summary>
    public IEqualityComparer<K> Comparer => m_Items.Comparer;

    /// <summary>
    /// Key
    /// </summary>
    public K Key { get; private set; }

    /// <summary>
    /// Value
    /// </summary>
    public V Value { get; set; }

    /// <summary>
    /// Prefix
    /// </summary>
    public K[] Prefix => EnumerateParents()
      .TakeWhile(node => !node.IsRoot)
      .Select(node => node.Key)
      .Reverse()
      .ToArray();

    /// <summary>
    /// Frequency
    /// </summary>
    public int Frequency { get; private set; }

    /// <summary>
    /// Frequency Terminal
    /// </summary>
    public int FrequencyTerminal => Frequency - m_Items.Sum(pair => pair.Value.Frequency);

    /// <summary>
    /// Parent
    /// </summary>
    public Trie<K, V> Parent { get; private set; }

    /// <summary>
    /// Root
    /// </summary>
    public Trie<K, V> Root {
      get {
        var result = this;

        while (result.Parent is not null)
          result = result.Parent;

        return result;
      }
    }

    /// <summary>
    /// Is Parent
    /// </summary>
    public bool IsRoot => Parent is null;

    /// <summary>
    /// Is Leaf
    /// </summary>
    public bool IsLeaf => m_Items.Count <= 0;

    /// <summary>
    /// Is Terminal
    /// </summary>
    public bool IsTerminal => Frequency - m_Items.Sum(pair => pair.Value.Frequency) > 0;

    /// <summary>
    /// Level
    /// </summary>
    public int Level {
      get {
        int level = 0;

        var current = this;

        while (current.Parent is not null) {
          current = current.Parent;

          level += 1;
        }

        return level;
      }
    }

    /// <summary>
    /// Items
    /// </summary>
    public IReadOnlyDictionary<K, Trie<K, V>> Items => m_Items;

    /// <summary>
    /// Add Trie Node
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <returns>Existing or Created trie node</returns>
    public Trie<K, V> Add(K key, V value) {
      if (m_Items.TryGetValue(key, out var result)) {
        result.Value = value;

        return result;
      }

      result = new Trie<K, V>(this, key, value);

      result.UpdateFrequency();

      return result;
    }

    /// <summary>
    /// Add Trie Sequence
    /// </summary>
    /// <param name="keys">Keys</param>
    /// <param name="value">Value</param>
    /// <param name="empty">Empty Value if Required</param>
    /// <returns>Created Node or Existing One</returns>
    public Trie<K, V> Add(IEnumerable<K> keys, V value, V empty = default) {
      if (keys is null)
        throw new ArgumentNullException(nameof(keys));

      Trie<K, V> result = this;

      foreach (K key in keys) {
        if (result.m_Items.TryGetValue(key, out var next))
          result = next!;
        else
          result = new Trie<K, V>(result, key, empty);
      }

      result.Value = value;

      result.UpdateFrequency();

      return result!;
    }

    /// <summary>
    /// Add 
    /// </summary>
    /// <param name="keys">Key Sequence</param>
    /// <param name="map">Map</param>
    /// <returns>Created or Existing Item</returns>
    /// <exception cref="ArgumentNullException">When keys or map are null</exception>
    public Trie<K, V> Add(IEnumerable<K> keys, Func<Trie<K, V>, V> map) {
      if (keys is null)
        throw new ArgumentNullException(nameof(keys));
      if (map is null)
        throw new ArgumentNullException(nameof(map));

      Trie<K, V> result = this;

      foreach (var key in keys) {
        if (result.m_Items.TryGetValue(key, out var next))
          result = next!;
        else
          result = new Trie<K, V>(result, key, default);

        result.Value = map(result);
      }

      result.UpdateFrequency();

      return result!;
    }

    /// <summary>
    /// Try Remove
    /// </summary>
    /// <returns>true, if node is a terminal one and can be removed</returns>
    public bool TryRemove() {
      if (FrequencyTerminal <= 0)
        return false;
      if (IsRoot)
        return false;

      for (Trie<K, V> current = this; current is not null; current = current.Parent) {
        current.Frequency -= 1;

        if (current.Frequency <= 0 && current.Parent is not null) {
          current.Parent.m_Items.Remove(current.Key);
          current.Parent = null!;
          current.Key = default!;
          current.Value = default!;
        }
      }

      return true;
    }

    /// <summary>
    /// Remove
    /// </summary>
    /// <exception cref="InvalidOperationException">On non-terminal node removing</exception>
    public void Remove() {
      if (!TryRemove())
        throw new InvalidOperationException("Only terminal nodes can be deleted");
    }

    /// <summary>
    /// Find Child
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>Found Child or Null</returns>
    public Trie<K, V> Find(K key) => m_Items.TryGetValue(key, out var result) ? result : default;

    /// <summary>
    /// Find Child
    /// </summary>
    /// <param name="keys">Key Sequence to Find</param>
    /// <returns>Child or null</returns>
    /// <exception cref="ArgumentNullException">When Key Sequence is null</exception>
    public Trie<K, V> Find(IEnumerable<K> keys) {
      if (keys is null)
        throw new ArgumentNullException(nameof(keys));

      Trie<K, V> result = this;

      foreach (var key in keys) {
        result = result.Find(key)!;

        if (result is null)
          break;
      }

      return result!;
    }

    /// <summary>
    /// Enumerate Children (can be null) and Self 
    /// </summary>
    /// <param name="keys">Keys Sequence</param>
    /// <returns>Children Sequence (last children can be null)</returns>
    /// <exception cref="ArgumentNullException">When keys is null</exception>
    public IEnumerable<Trie<K, V>> EnumerateChildren(IEnumerable<K> keys) {
      if (keys is null)
        throw new ArgumentNullException(nameof(keys));

      Trie<K, V> result = this;

      yield return this;

      foreach (var key in keys) {
        if (result is not null)
          result = result.Find(key)!;

        yield return result!;
      }
    }

    /// <summary>
    /// All Children and Self
    /// </summary>
    /// <returns>All Children</returns>
    public IEnumerable<Trie<K, V>> EnumerateChildren() {
      Queue<Trie<K, V>> agenda = new();

      agenda.Enqueue(this);

      while (agenda.Count > 0) {
        var node = agenda.Dequeue();

        yield return node;

        foreach (var child in node.m_Items.Values)
          agenda.Enqueue(child);
      }
    }

    /// <summary>
    /// All Parents (Self included)
    /// </summary>
    public IEnumerable<Trie<K, V>> EnumerateParents() {
      for (Trie<K, V> current = this; current is not null; current = current.Parent)
        yield return current;
    }

    /// <summary>
    /// To String
    /// </summary>
    public override string ToString() => Parent is null
      ? $"(Root)"
      : $"({Key}, {Value}) {(FrequencyTerminal > 0 ? "T" : "")}";

    #endregion Public

    #region IEnumerable<Trie<K, V>>

    /// <summary>
    /// Enumerate (children and self)
    /// </summary>
    public IEnumerator<Trie<K, V>> GetEnumerator() => EnumerateChildren().GetEnumerator();

    /// <summary>
    /// Enumerate (children and self)
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    #endregion IEnumerable<Trie<K, V>>
  }

}
