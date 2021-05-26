// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Linq.DbQueryProvider
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Internal.Linq
{
  internal class DbQueryProvider : IQueryProvider, IDbAsyncQueryProvider
  {
    private readonly InternalContext _internalContext;
    private readonly IInternalQuery _internalQuery;

    public DbQueryProvider(InternalContext internalContext, IInternalQuery internalQuery)
    {
      this._internalContext = internalContext;
      this._internalQuery = internalQuery;
    }

    public virtual IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression>(expression, nameof (expression));
      ObjectQuery objectQuery = this.CreateObjectQuery(expression);
      return typeof (TElement) != ((IQueryable) objectQuery).ElementType ? (IQueryable<TElement>) this.CreateQuery(objectQuery) : (IQueryable<TElement>) new DbQuery<TElement>((IInternalQuery<TElement>) new InternalQuery<TElement>(this._internalContext, objectQuery));
    }

    public virtual IQueryable CreateQuery(Expression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression>(expression, nameof (expression));
      return this.CreateQuery(this.CreateObjectQuery(expression));
    }

    public virtual TResult Execute<TResult>(Expression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression>(expression, nameof (expression));
      this._internalContext.Initialize();
      return ((IQueryProvider) this._internalQuery.ObjectQueryProvider).Execute<TResult>(expression);
    }

    public virtual object Execute(Expression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression>(expression, nameof (expression));
      this._internalContext.Initialize();
      return ((IQueryProvider) this._internalQuery.ObjectQueryProvider).Execute(expression);
    }

    Task<TResult> IDbAsyncQueryProvider.ExecuteAsync<TResult>(
      Expression expression,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression>(expression, nameof (expression));
      cancellationToken.ThrowIfCancellationRequested();
      this._internalContext.Initialize();
      return ((IDbAsyncQueryProvider) this._internalQuery.ObjectQueryProvider).ExecuteAsync<TResult>(expression, cancellationToken);
    }

    Task<object> IDbAsyncQueryProvider.ExecuteAsync(
      Expression expression,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression>(expression, nameof (expression));
      cancellationToken.ThrowIfCancellationRequested();
      this._internalContext.Initialize();
      return ((IDbAsyncQueryProvider) this._internalQuery.ObjectQueryProvider).ExecuteAsync(expression, cancellationToken);
    }

    private IQueryable CreateQuery(ObjectQuery objectQuery)
    {
      IInternalQuery internalQuery = this.CreateInternalQuery(objectQuery);
      return (IQueryable) ((IEnumerable<ConstructorInfo>) typeof (DbQuery<>).MakeGenericType(internalQuery.ElementType).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)).Single<ConstructorInfo>().Invoke(new object[1]
      {
        (object) internalQuery
      });
    }

    protected ObjectQuery CreateObjectQuery(Expression expression)
    {
      expression = new DbQueryVisitor().Visit(expression);
      return (ObjectQuery) ((IQueryProvider) this._internalQuery.ObjectQueryProvider).CreateQuery(expression);
    }

    protected IInternalQuery CreateInternalQuery(ObjectQuery objectQuery) => (IInternalQuery) typeof (InternalQuery<>).MakeGenericType(((IQueryable) objectQuery).ElementType).GetDeclaredConstructor(typeof (InternalContext), typeof (ObjectQuery)).Invoke(new object[2]
    {
      (object) this._internalContext,
      (object) objectQuery
    });

    public InternalContext InternalContext => this._internalContext;
  }
}
