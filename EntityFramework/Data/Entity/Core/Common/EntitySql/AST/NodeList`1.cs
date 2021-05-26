﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.NodeList`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class NodeList<T> : Node, IEnumerable<T>, IEnumerable where T : Node
  {
    private readonly List<T> _list = new List<T>();

    internal NodeList()
    {
    }

    internal NodeList(T item) => this._list.Add(item);

    internal NodeList<T> Add(T item)
    {
      this._list.Add(item);
      return this;
    }

    internal int Count => this._list.Count;

    internal T this[int index] => this._list[index];

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this._list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();
  }
}
