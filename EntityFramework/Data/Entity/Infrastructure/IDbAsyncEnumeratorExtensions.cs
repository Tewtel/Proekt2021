// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.IDbAsyncEnumeratorExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure
{
  internal static class IDbAsyncEnumeratorExtensions
  {
    public static Task<bool> MoveNextAsync(this IDbAsyncEnumerator enumerator)
    {
      System.Data.Entity.Utilities.Check.NotNull<IDbAsyncEnumerator>(enumerator, nameof (enumerator));
      return enumerator.MoveNextAsync(CancellationToken.None);
    }

    internal static IDbAsyncEnumerator<TResult> Cast<TResult>(
      this IDbAsyncEnumerator source)
    {
      return (IDbAsyncEnumerator<TResult>) new IDbAsyncEnumeratorExtensions.CastDbAsyncEnumerator<TResult>(source);
    }

    private class CastDbAsyncEnumerator<TResult> : 
      IDbAsyncEnumerator<TResult>,
      IDbAsyncEnumerator,
      IDisposable
    {
      private readonly IDbAsyncEnumerator _underlyingEnumerator;

      public CastDbAsyncEnumerator(IDbAsyncEnumerator sourceEnumerator) => this._underlyingEnumerator = sourceEnumerator;

      public Task<bool> MoveNextAsync(CancellationToken cancellationToken) => this._underlyingEnumerator.MoveNextAsync(cancellationToken);

      public TResult Current => (TResult) this._underlyingEnumerator.Current;

      object IDbAsyncEnumerator.Current => this._underlyingEnumerator.Current;

      public void Dispose() => this._underlyingEnumerator.Dispose();
    }
  }
}
