// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Linq.InternalDbQuery`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace System.Data.Entity.Internal.Linq
{
  internal class InternalDbQuery<TElement> : 
    DbQuery,
    IOrderedQueryable<TElement>,
    IQueryable<TElement>,
    IEnumerable<TElement>,
    IEnumerable,
    IQueryable,
    IOrderedQueryable,
    IDbAsyncEnumerable<TElement>,
    IDbAsyncEnumerable
  {
    private readonly IInternalQuery<TElement> _internalQuery;

    public InternalDbQuery(IInternalQuery<TElement> internalQuery) => this._internalQuery = internalQuery;

    internal override IInternalQuery InternalQuery => (IInternalQuery) this._internalQuery;

    public override DbQuery Include(string path)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(path, nameof (path));
      return (DbQuery) new InternalDbQuery<TElement>(this._internalQuery.Include(path));
    }

    public override DbQuery AsNoTracking() => (DbQuery) new InternalDbQuery<TElement>(this._internalQuery.AsNoTracking());

    [Obsolete("Queries are now streaming by default unless a retrying ExecutionStrategy is used. Calling this method will have no effect.")]
    public override DbQuery AsStreaming() => (DbQuery) new InternalDbQuery<TElement>(this._internalQuery.AsStreaming());

    internal override DbQuery WithExecutionStrategy(IDbExecutionStrategy executionStrategy) => (DbQuery) new InternalDbQuery<TElement>(this._internalQuery.WithExecutionStrategy(executionStrategy));

    internal override IInternalQuery GetInternalQueryWithCheck(string memberName) => (IInternalQuery) this._internalQuery;

    public IEnumerator<TElement> GetEnumerator() => this._internalQuery.GetEnumerator();

    public IDbAsyncEnumerator<TElement> GetAsyncEnumerator() => this._internalQuery.GetAsyncEnumerator();
  }
}
