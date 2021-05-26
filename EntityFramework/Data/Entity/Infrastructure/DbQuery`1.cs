// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbQuery`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Internal.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Represents a LINQ to Entities query against a DbContext.
  /// </summary>
  /// <typeparam name="TResult"> The type of entity to query for. </typeparam>
  [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay()}")]
  public class DbQuery<TResult> : 
    IOrderedQueryable<TResult>,
    IQueryable<TResult>,
    IEnumerable<TResult>,
    IEnumerable,
    IQueryable,
    IOrderedQueryable,
    IListSource,
    IInternalQueryAdapter,
    IDbAsyncEnumerable<TResult>,
    IDbAsyncEnumerable
  {
    private readonly IInternalQuery<TResult> _internalQuery;
    private IQueryProvider _provider;

    internal DbQuery(IInternalQuery<TResult> internalQuery) => this._internalQuery = internalQuery;

    /// <summary>
    /// Specifies the related objects to include in the query results.
    /// </summary>
    /// <remarks>
    /// Paths are all-inclusive. For example, if an include call indicates Include("Orders.OrderLines"), not only will
    /// OrderLines be included, but also Orders.  When you call the Include method, the query path is only valid on
    /// the returned instance of the DbQuery&lt;T&gt;. Other instances of DbQuery&lt;T&gt; and the object context itself are not affected.
    /// Because the Include method returns the query object, you can call this method multiple times on an DbQuery&lt;T&gt; to
    /// specify multiple paths for the query.
    /// </remarks>
    /// <param name="path"> The dot-separated list of related objects to return in the query results. </param>
    /// <returns>
    /// A new <see cref="T:System.Data.Entity.Infrastructure.DbQuery`1" /> with the defined query path.
    /// </returns>
    public virtual DbQuery<TResult> Include(string path)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(path, nameof (path));
      return this._internalQuery != null ? new DbQuery<TResult>(this._internalQuery.Include(path)) : this;
    }

    /// <summary>
    /// Returns a new query where the entities returned will not be cached in the <see cref="T:System.Data.Entity.DbContext" />.
    /// </summary>
    /// <returns> A new query with NoTracking applied. </returns>
    public virtual DbQuery<TResult> AsNoTracking() => this._internalQuery != null ? new DbQuery<TResult>(this._internalQuery.AsNoTracking()) : this;

    /// <summary>
    /// Returns a new query that will stream the results instead of buffering.
    /// </summary>
    /// <returns> A new query with AsStreaming applied. </returns>
    [Obsolete("Queries are now streaming by default unless a retrying ExecutionStrategy is used. Calling this method will have no effect.")]
    public virtual DbQuery<TResult> AsStreaming() => this._internalQuery != null ? new DbQuery<TResult>(this._internalQuery.AsStreaming()) : this;

    internal virtual DbQuery<TResult> WithExecutionStrategy(
      IDbExecutionStrategy executionStrategy)
    {
      return this._internalQuery != null ? new DbQuery<TResult>(this._internalQuery.WithExecutionStrategy(executionStrategy)) : this;
    }

    /// <summary>
    /// Returns <c>false</c>.
    /// </summary>
    /// <returns>
    /// <c>false</c> .
    /// </returns>
    bool IListSource.ContainsListCollection => false;

    /// <summary>
    /// Throws an exception indicating that binding directly to a store query is not supported.
    /// Instead populate a DbSet with data, for example by using the Load extension method, and
    /// then bind to local data.  For WPF bind to DbSet.Local.  For Windows Forms bind to
    /// DbSet.Local.ToBindingList().
    /// </summary>
    /// <returns> Never returns; always throws. </returns>
    IList IListSource.GetList() => throw System.Data.Entity.Resources.Error.DbQuery_BindingToDbQueryNotSupported();

    /// <summary>
    /// Returns an <see cref="T:System.Collections.Generic.IEnumerator`1" /> which when enumerated will execute the query against the database.
    /// </summary>
    /// <returns> The query results. </returns>
    IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator() => this.GetInternalQueryWithCheck("IEnumerable<TResult>.GetEnumerator").GetEnumerator();

    /// <summary>
    /// Returns an <see cref="T:System.Collections.Generic.IEnumerator`1" /> which when enumerated will execute the query against the database.
    /// </summary>
    /// <returns> The query results. </returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetInternalQueryWithCheck("IEnumerable.GetEnumerator").GetEnumerator();

    /// <summary>
    /// Returns an <see cref="T:System.Data.Entity.Infrastructure.IDbAsyncEnumerator" /> which when enumerated will execute the query against the database.
    /// </summary>
    /// <returns> The query results. </returns>
    IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() => (IDbAsyncEnumerator) this.GetInternalQueryWithCheck("IDbAsyncEnumerable.GetAsyncEnumerator").GetAsyncEnumerator();

    /// <summary>
    /// Returns an <see cref="T:System.Data.Entity.Infrastructure.IDbAsyncEnumerator`1" /> which when enumerated will execute the query against the database.
    /// </summary>
    /// <returns> The query results. </returns>
    IDbAsyncEnumerator<TResult> IDbAsyncEnumerable<TResult>.GetAsyncEnumerator() => this.GetInternalQueryWithCheck("IDbAsyncEnumerable<TResult>.GetAsyncEnumerator").GetAsyncEnumerator();

    /// <summary>The IQueryable element type.</summary>
    Type IQueryable.ElementType => this.GetInternalQueryWithCheck("IQueryable.ElementType").ElementType;

    /// <summary>The IQueryable LINQ Expression.</summary>
    Expression IQueryable.Expression => this.GetInternalQueryWithCheck("IQueryable.Expression").Expression;

    /// <summary>The IQueryable provider.</summary>
    IQueryProvider IQueryable.Provider => this._provider ?? (this._provider = (IQueryProvider) new DbQueryProvider(this.GetInternalQueryWithCheck("IQueryable.Provider").InternalContext, (IInternalQuery) this.GetInternalQueryWithCheck("IQueryable.Provider")));

    IInternalQuery IInternalQueryAdapter.InternalQuery => (IInternalQuery) this._internalQuery;

    internal IInternalQuery<TResult> InternalQuery => this._internalQuery;

    private IInternalQuery<TResult> GetInternalQueryWithCheck(string memberName) => this._internalQuery != null ? this._internalQuery : throw new NotImplementedException(System.Data.Entity.Resources.Strings.TestDoubleNotImplemented((object) memberName, (object) this.GetType().Name, (object) typeof (DbSet<>).Name));

    /// <summary>
    /// Returns a <see cref="T:System.String" /> representation of the underlying query.
    /// </summary>
    /// <returns> The query string. </returns>
    public override string ToString() => this._internalQuery != null ? this._internalQuery.ToTraceString() : base.ToString();

    private string DebuggerDisplay() => base.ToString();

    /// <summary>
    /// Gets a <see cref="T:System.String" /> representation of the underlying query.
    /// </summary>
    public string Sql => this.ToString();

    /// <summary>
    /// Returns a new instance of the non-generic <see cref="T:System.Data.Entity.Infrastructure.DbQuery" /> class for this query.
    /// </summary>
    /// <param name="entry">The query.</param>
    /// <returns> A non-generic version. </returns>
    public static implicit operator DbQuery(DbQuery<TResult> entry) => entry._internalQuery != null ? (DbQuery) new InternalDbQuery<TResult>(entry._internalQuery) : throw new NotSupportedException(System.Data.Entity.Resources.Strings.TestDoublesCannotBeConverted);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
