// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.VarMap
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class VarMap : 
    IDictionary<Var, Var>,
    ICollection<KeyValuePair<Var, Var>>,
    IEnumerable<KeyValuePair<Var, Var>>,
    IEnumerable
  {
    private Dictionary<Var, Var> map;
    private Dictionary<Var, Var> reverseMap;

    internal VarMap GetReverseMap() => new VarMap(this.reverseMap, this.map);

    public bool ContainsValue(Var value) => this.reverseMap.ContainsKey(value);

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = string.Empty;
      foreach (Var key in this.map.Keys)
      {
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}({1},{2})", (object) str, (object) key.Id, (object) this[key].Id);
        str = ",";
      }
      return stringBuilder.ToString();
    }

    public Var this[Var key]
    {
      get => this.map[key];
      set => this.map[key] = value;
    }

    public ICollection<Var> Keys => (ICollection<Var>) this.map.Keys;

    public ICollection<Var> Values => (ICollection<Var>) this.map.Values;

    public int Count => this.map.Count;

    public bool IsReadOnly => false;

    public void Add(Var key, Var value)
    {
      if (!this.reverseMap.ContainsKey(value))
        this.reverseMap.Add(value, key);
      this.map.Add(key, value);
    }

    public void Add(KeyValuePair<Var, Var> item)
    {
      if (!this.reverseMap.ContainsKey(item.Value))
        ((ICollection<KeyValuePair<Var, Var>>) this.reverseMap).Add(new KeyValuePair<Var, Var>(item.Value, item.Key));
      ((ICollection<KeyValuePair<Var, Var>>) this.map).Add(item);
    }

    public void Clear()
    {
      this.map.Clear();
      this.reverseMap.Clear();
    }

    public bool Contains(KeyValuePair<Var, Var> item) => ((ICollection<KeyValuePair<Var, Var>>) this.map).Contains(item);

    public bool ContainsKey(Var key) => this.map.ContainsKey(key);

    public void CopyTo(KeyValuePair<Var, Var>[] array, int arrayIndex) => ((ICollection<KeyValuePair<Var, Var>>) this.map).CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<Var, Var>> GetEnumerator() => (IEnumerator<KeyValuePair<Var, Var>>) this.map.GetEnumerator();

    public bool Remove(Var key)
    {
      this.reverseMap.Remove(this.map[key]);
      return this.map.Remove(key);
    }

    public bool Remove(KeyValuePair<Var, Var> item)
    {
      this.reverseMap.Remove(this.map[item.Value]);
      return ((ICollection<KeyValuePair<Var, Var>>) this.map).Remove(item);
    }

    public bool TryGetValue(Var key, out Var value) => this.map.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.map.GetEnumerator();

    public VarMap()
    {
      this.map = new Dictionary<Var, Var>();
      this.reverseMap = new Dictionary<Var, Var>();
    }

    private VarMap(Dictionary<Var, Var> map, Dictionary<Var, Var> reverseMap)
    {
      this.map = map;
      this.reverseMap = reverseMap;
    }
  }
}
