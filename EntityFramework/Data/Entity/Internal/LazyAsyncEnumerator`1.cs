// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.LazyAsyncEnumerator`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Internal
{
  internal class LazyAsyncEnumerator<T> : IDbAsyncEnumerator<T>, IDbAsyncEnumerator, IDisposable
  {
    private readonly Func<CancellationToken, Task<ObjectResult<T>>> _getObjectResultAsync;
    private IDbAsyncEnumerator<T> _objectResultAsyncEnumerator;

    public LazyAsyncEnumerator(
      Func<CancellationToken, Task<ObjectResult<T>>> getObjectResultAsync)
    {
      this._getObjectResultAsync = getObjectResultAsync;
    }

    public T Current => this._objectResultAsyncEnumerator != null ? this._objectResultAsyncEnumerator.Current : default (T);

    object IDbAsyncEnumerator.Current => (object) this.Current;

    public void Dispose()
    {
      if (this._objectResultAsyncEnumerator == null)
        return;
      this._objectResultAsyncEnumerator.Dispose();
    }

    public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return this._objectResultAsyncEnumerator != null ? this._objectResultAsyncEnumerator.MoveNextAsync(cancellationToken) : this.FirstMoveNextAsync(cancellationToken);
    }

    private async Task<bool> FirstMoveNextAsync(CancellationToken cancellationToken)
    {
      ObjectResult<T> objectResult = await this._getObjectResultAsync(cancellationToken).WithCurrentCulture<ObjectResult<T>>();
      try
      {
        this._objectResultAsyncEnumerator = ((IDbAsyncEnumerable<T>) objectResult).GetAsyncEnumerator();
      }
      catch
      {
        objectResult.Dispose();
        throw;
      }
      return await this._objectResultAsyncEnumerator.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>();
    }
  }
}
