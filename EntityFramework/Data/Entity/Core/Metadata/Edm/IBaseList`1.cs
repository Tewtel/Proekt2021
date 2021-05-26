// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.IBaseList`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal interface IBaseList<T> : IList, ICollection, IEnumerable
  {
    T this[string identity] { get; }

    T this[int index] { get; }

    int IndexOf(T item);
  }
}
