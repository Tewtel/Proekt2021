// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.KeyToListMap`2
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime;
using System.Text;

namespace System.Data.SQLite.Linq
{
  internal sealed class KeyToListMap<TKey, TValue> : InternalBase
  {
    private Dictionary<TKey, List<TValue>> m_map;

    internal KeyToListMap(IEqualityComparer<TKey> comparer) => this.m_map = new Dictionary<TKey, List<TValue>>(comparer);

    internal void Add(TKey key, TValue value)
    {
      List<TValue> objList;
      if (!this.m_map.TryGetValue(key, out objList))
      {
        objList = new List<TValue>();
        this.m_map[key] = objList;
      }
      objList.Add(value);
    }

    internal void AddRange(TKey key, IEnumerable<TValue> values)
    {
      foreach (TValue obj in values)
        this.Add(key, obj);
    }

    internal bool ContainsKey(TKey key) => this.m_map.ContainsKey(key);

    internal IEnumerable<TValue> EnumerateValues(TKey key)
    {
      List<TValue> values;
      if (this.m_map.TryGetValue(key, out values))
      {
        foreach (TValue obj in values)
          yield return obj;
      }
    }

    internal ReadOnlyCollection<TValue> ListForKey(TKey key) => new ReadOnlyCollection<TValue>((IList<TValue>) this.m_map[key]);

    internal bool RemoveKey(TKey key) => this.m_map.Remove(key);

    internal override void ToCompactString(StringBuilder builder)
    {
      foreach (TKey key in this.Keys)
      {
        StringUtil.FormatStringBuilder(builder, "{0}", (object) key);
        builder.Append(": ");
        IEnumerable<TValue> objs = (IEnumerable<TValue>) this.ListForKey(key);
        StringUtil.ToSeparatedString(builder, (IEnumerable) objs, ",", "null");
        builder.Append("; ");
      }
    }

    internal bool TryGetListForKey(TKey key, out ReadOnlyCollection<TValue> valueCollection)
    {
      valueCollection = (ReadOnlyCollection<TValue>) null;
      List<TValue> objList;
      if (!this.m_map.TryGetValue(key, out objList))
        return false;
      valueCollection = new ReadOnlyCollection<TValue>((IList<TValue>) objList);
      return true;
    }

    internal IEnumerable<TValue> AllValues
    {
      get
      {
        foreach (TKey key in this.Keys)
        {
          foreach (TValue obj in this.ListForKey(key))
            yield return obj;
        }
      }
    }

    internal IEnumerable<TKey> Keys => (IEnumerable<TKey>) this.m_map.Keys;

    internal IEnumerable<KeyValuePair<TKey, List<TValue>>> KeyValuePairs
    {
      [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")] get => (IEnumerable<KeyValuePair<TKey, List<TValue>>>) this.m_map;
    }
  }
}
