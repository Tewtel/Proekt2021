// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.LazyEnumerator`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Internal
{
  internal class LazyEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
  {
    private readonly Func<ObjectResult<T>> _getObjectResult;
    private IEnumerator<T> _objectResultEnumerator;

    public LazyEnumerator(Func<ObjectResult<T>> getObjectResult) => this._getObjectResult = getObjectResult;

    public T Current => this._objectResultEnumerator != null ? this._objectResultEnumerator.Current : default (T);

    object IEnumerator.Current => (object) this.Current;

    public void Dispose()
    {
      if (this._objectResultEnumerator == null)
        return;
      this._objectResultEnumerator.Dispose();
    }

    public bool MoveNext()
    {
      if (this._objectResultEnumerator == null)
      {
        ObjectResult<T> objectResult = this._getObjectResult();
        try
        {
          this._objectResultEnumerator = objectResult.GetEnumerator();
        }
        catch
        {
          objectResult.Dispose();
          throw;
        }
      }
      return this._objectResultEnumerator.MoveNext();
    }

    public void Reset()
    {
      if (this._objectResultEnumerator == null)
        return;
      this._objectResultEnumerator.Reset();
    }
  }
}
