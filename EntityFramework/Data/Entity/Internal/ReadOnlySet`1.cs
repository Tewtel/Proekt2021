// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ReadOnlySet`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Internal
{
  internal class ReadOnlySet<T> : ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable
  {
    private readonly ISet<T> _set;

    public ReadOnlySet(ISet<T> set) => this._set = set;

    public bool Add(T item) => throw Error.DbPropertyValues_PropertyValueNamesAreReadonly();

    public void ExceptWith(IEnumerable<T> other) => this._set.ExceptWith(other);

    public void IntersectWith(IEnumerable<T> other) => this._set.IntersectWith(other);

    public bool IsProperSubsetOf(IEnumerable<T> other) => this._set.IsProperSubsetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other) => this._set.IsProperSupersetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other) => this._set.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other) => this._set.IsSupersetOf(other);

    public bool Overlaps(IEnumerable<T> other) => this._set.Overlaps(other);

    public bool SetEquals(IEnumerable<T> other) => this._set.SetEquals(other);

    public void SymmetricExceptWith(IEnumerable<T> other) => this._set.SymmetricExceptWith(other);

    public void UnionWith(IEnumerable<T> other) => this._set.UnionWith(other);

    void ICollection<T>.Add(T item) => throw Error.DbPropertyValues_PropertyValueNamesAreReadonly();

    public void Clear() => throw Error.DbPropertyValues_PropertyValueNamesAreReadonly();

    public bool Contains(T item) => this._set.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => this._set.CopyTo(array, arrayIndex);

    public int Count => this._set.Count;

    public bool IsReadOnly => true;

    public bool Remove(T item) => throw Error.DbPropertyValues_PropertyValueNamesAreReadonly();

    public IEnumerator<T> GetEnumerator() => this._set.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this._set.GetEnumerator();
  }
}
