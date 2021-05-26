// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Linq.InternalQuery`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.ELinq;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Entity.Internal.Linq
{
  internal class InternalQuery<TElement> : IInternalQuery<TElement>, IInternalQuery
  {
    private readonly InternalContext _internalContext;
    private System.Data.Entity.Core.Objects.ObjectQuery<TElement> _objectQuery;

    public InternalQuery(InternalContext internalContext) => this._internalContext = internalContext;

    public InternalQuery(InternalContext internalContext, System.Data.Entity.Core.Objects.ObjectQuery objectQuery)
    {
      this._internalContext = internalContext;
      this._objectQuery = (System.Data.Entity.Core.Objects.ObjectQuery<TElement>) objectQuery;
    }

    public virtual void ResetQuery() => this._objectQuery = (System.Data.Entity.Core.Objects.ObjectQuery<TElement>) null;

    public virtual InternalContext InternalContext => this._internalContext;

    public virtual IInternalQuery<TElement> Include(string path) => (IInternalQuery<TElement>) new InternalQuery<TElement>(this._internalContext, (System.Data.Entity.Core.Objects.ObjectQuery) this._objectQuery.Include(path));

    public virtual IInternalQuery<TElement> AsNoTracking() => (IInternalQuery<TElement>) new InternalQuery<TElement>(this._internalContext, (System.Data.Entity.Core.Objects.ObjectQuery) DbHelpers.CreateNoTrackingQuery((System.Data.Entity.Core.Objects.ObjectQuery) this._objectQuery));

    public virtual IInternalQuery<TElement> AsStreaming() => (IInternalQuery<TElement>) new InternalQuery<TElement>(this._internalContext, (System.Data.Entity.Core.Objects.ObjectQuery) DbHelpers.CreateStreamingQuery((System.Data.Entity.Core.Objects.ObjectQuery) this._objectQuery));

    public virtual IInternalQuery<TElement> WithExecutionStrategy(
      IDbExecutionStrategy executionStrategy)
    {
      return (IInternalQuery<TElement>) new InternalQuery<TElement>(this._internalContext, (System.Data.Entity.Core.Objects.ObjectQuery) DbHelpers.CreateQueryWithExecutionStrategy((System.Data.Entity.Core.Objects.ObjectQuery) this._objectQuery, executionStrategy));
    }

    public virtual System.Data.Entity.Core.Objects.ObjectQuery<TElement> ObjectQuery => this._objectQuery;

    System.Data.Entity.Core.Objects.ObjectQuery IInternalQuery.ObjectQuery => (System.Data.Entity.Core.Objects.ObjectQuery) this.ObjectQuery;

    protected void InitializeQuery(System.Data.Entity.Core.Objects.ObjectQuery<TElement> objectQuery) => this._objectQuery = objectQuery;

    public virtual string ToTraceString() => this._objectQuery.ToTraceString();

    public virtual Expression Expression => ((IQueryable) this._objectQuery).Expression;

    public virtual ObjectQueryProvider ObjectQueryProvider => this._objectQuery.ObjectQueryProvider;

    public Type ElementType => typeof (TElement);

    public virtual IEnumerator<TElement> GetEnumerator()
    {
      this.InternalContext.Initialize();
      return ((IEnumerable<TElement>) this._objectQuery).GetEnumerator();
    }

    IEnumerator IInternalQuery.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public virtual IDbAsyncEnumerator<TElement> GetAsyncEnumerator()
    {
      this.InternalContext.Initialize();
      return ((IDbAsyncEnumerable<TElement>) this._objectQuery).GetAsyncEnumerator();
    }

    IDbAsyncEnumerator IInternalQuery.GetAsyncEnumerator() => (IDbAsyncEnumerator) this.GetAsyncEnumerator();
  }
}
