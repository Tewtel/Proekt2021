// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.Scope
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class Scope : IEnumerable<KeyValuePair<string, ScopeEntry>>, IEnumerable
  {
    private readonly Dictionary<string, ScopeEntry> _scopeEntries;

    internal Scope(IEqualityComparer<string> keyComparer) => this._scopeEntries = new Dictionary<string, ScopeEntry>(keyComparer);

    internal Scope Add(string key, ScopeEntry value)
    {
      this._scopeEntries.Add(key, value);
      return this;
    }

    internal void Remove(string key) => this._scopeEntries.Remove(key);

    internal void Replace(string key, ScopeEntry value) => this._scopeEntries[key] = value;

    internal bool Contains(string key) => this._scopeEntries.ContainsKey(key);

    internal bool TryLookup(string key, out ScopeEntry value) => this._scopeEntries.TryGetValue(key, out value);

    public Dictionary<string, ScopeEntry>.Enumerator GetEnumerator() => this._scopeEntries.GetEnumerator();

    IEnumerator<KeyValuePair<string, ScopeEntry>> IEnumerable<KeyValuePair<string, ScopeEntry>>.GetEnumerator() => (IEnumerator<KeyValuePair<string, ScopeEntry>>) this._scopeEntries.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._scopeEntries.GetEnumerator();
  }
}
