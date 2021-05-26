// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.DynamicEqualityComparer`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Utilities
{
  internal sealed class DynamicEqualityComparer<T> : IEqualityComparer<T> where T : class
  {
    private readonly Func<T, T, bool> _func;

    public DynamicEqualityComparer(Func<T, T, bool> func) => this._func = func;

    public bool Equals(T x, T y) => this._func(x, y);

    public int GetHashCode(T obj) => 0;
  }
}
