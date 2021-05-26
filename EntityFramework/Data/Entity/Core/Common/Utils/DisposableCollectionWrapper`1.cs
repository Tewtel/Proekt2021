// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.DisposableCollectionWrapper`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.Utils
{
  internal class DisposableCollectionWrapper<T> : IDisposable, IEnumerable<T>, IEnumerable
    where T : IDisposable
  {
    private readonly IEnumerable<T> _enumerable;

    internal DisposableCollectionWrapper(IEnumerable<T> enumerable) => this._enumerable = enumerable;

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      if (this._enumerable == null)
        return;
      foreach (T obj in this._enumerable)
        obj?.Dispose();
    }

    public IEnumerator<T> GetEnumerator() => this._enumerable.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this._enumerable.GetEnumerator();
  }
}
