﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.KeyToListMap`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace System.Data.Entity.Core.Common.Utils
{
  internal class KeyToListMap<TKey, TValue> : InternalBase
  {
    private readonly Dictionary<TKey, List<TValue>> m_map;

    internal KeyToListMap(IEqualityComparer<TKey> comparer) => this.m_map = new Dictionary<TKey, List<TValue>>(comparer);

    internal IEnumerable<TKey> Keys => (IEnumerable<TKey>) this.m_map.Keys;

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

    internal IEnumerable<KeyValuePair<TKey, List<TValue>>> KeyValuePairs => (IEnumerable<KeyValuePair<TKey, List<TValue>>>) this.m_map;

    internal bool ContainsKey(TKey key) => this.m_map.ContainsKey(key);

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

    internal bool RemoveKey(TKey key) => this.m_map.Remove(key);

    internal ReadOnlyCollection<TValue> ListForKey(TKey key) => new ReadOnlyCollection<TValue>((IList<TValue>) this.m_map[key]);

    internal bool TryGetListForKey(TKey key, out ReadOnlyCollection<TValue> valueCollection)
    {
      valueCollection = (ReadOnlyCollection<TValue>) null;
      List<TValue> objList;
      if (!this.m_map.TryGetValue(key, out objList))
        return false;
      valueCollection = new ReadOnlyCollection<TValue>((IList<TValue>) objList);
      return true;
    }

    internal IEnumerable<TValue> EnumerateValues(TKey key)
    {
      List<TValue> objList;
      if (this.m_map.TryGetValue(key, out objList))
      {
        foreach (TValue obj in objList)
          yield return obj;
      }
    }

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
  }
}
