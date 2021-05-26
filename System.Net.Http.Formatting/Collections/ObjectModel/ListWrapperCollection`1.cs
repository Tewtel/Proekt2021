// Decompiled with JetBrains decompiler
// Type: System.Collections.ObjectModel.ListWrapperCollection`1
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;

namespace System.Collections.ObjectModel
{
  internal sealed class ListWrapperCollection<T> : Collection<T>
  {
    private readonly List<T> _items;

    internal ListWrapperCollection()
      : this(new List<T>())
    {
    }

    internal ListWrapperCollection(List<T> list)
      : base((IList<T>) list)
    {
      this._items = list;
    }

    internal List<T> ItemsList => this._items;
  }
}
