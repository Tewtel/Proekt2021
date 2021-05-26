// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbQuery
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.ComponentModel;
using System.Data.Entity.Internal.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Represents a non-generic LINQ to Entities query against a DbContext.
  /// </summary>
  [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay()}")]
  public abstract class DbQuery : 
    IOrderedQueryable,
    IQueryable,
    IEnumerable,
    IListSource,
    IInternalQueryAdapter,
    IDbAsyncEnumerable
  {
    private IQueryProvider _provider;

    internal DbQuery()
    {
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
    /// Returns an <see cref="T:System.Collections.IEnumerator" /> which when enumerated will execute the query against the database.
    /// </summary>
    /// <returns> The query results. </returns>
    IEnumerator IEnumerable.GetEnumerator() => this.GetInternalQueryWithCheck("IEnumerable.GetEnumerator").GetEnumerator();

    /// <summary>
    /// Returns an <see cref="T:System.Data.Entity.Infrastructure.IDbAsyncEnumerator" /> which when enumerated will execute the query against the database.
    /// </summary>
    /// <returns> The query results. </returns>
    IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() => this.GetInternalQueryWithCheck("IDbAsyncEnumerable.GetAsyncEnumerator").GetAsyncEnumerator();

    /// <summary>The IQueryable element type.</summary>
    public virtual Type ElementType => this.GetInternalQueryWithCheck(nameof (ElementType)).ElementType;

    /// <summary>The IQueryable LINQ Expression.</summary>
    Expression IQueryable.Expression => this.GetInternalQueryWithCheck("IQueryable.Expression").Expression;

    /// <summary>The IQueryable provider.</summary>
    IQueryProvider IQueryable.Provider => this._provider ?? (this._provider = (IQueryProvider) new NonGenericDbQueryProvider(this.GetInternalQueryWithCheck("IQueryable.Provider").InternalContext, this.GetInternalQueryWithCheck("IQueryable.Provider")));

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
    /// <returns>A new DbQuery&lt;T&gt; with the defined query path.</returns>
    public virtual DbQuery Include(string path) => this;

    /// <summary>
    /// Returns a new query where the entities returned will not be cached in the <see cref="T:System.Data.Entity.DbContext" />.
    /// </summary>
    /// <returns> A new query with NoTracking applied. </returns>
    public virtual DbQuery AsNoTracking() => this;

    /// <summary>
    /// Returns a new query that will stream the results instead of buffering.
    /// </summary>
    /// <returns> A new query with AsStreaming applied. </returns>
    [Obsolete("Queries are now streaming by default unless a retrying ExecutionStrategy is used. Calling this method will have no effect.")]
    public virtual DbQuery AsStreaming() => this;

    internal virtual DbQuery WithExecutionStrategy(IDbExecutionStrategy executionStrategy) => this;

    /// <summary>
    /// Returns the equivalent generic <see cref="T:System.Data.Entity.Infrastructure.DbQuery`1" /> object.
    /// </summary>
    /// <typeparam name="TElement"> The type of element for which the query was created. </typeparam>
    /// <returns> The generic set object. </returns>
    public DbQuery<TElement> Cast<TElement>()
    {
      if (this.InternalQuery == null)
        throw new NotSupportedException(System.Data.Entity.Resources.Strings.TestDoublesCannotBeConverted);
      return !(typeof (TElement) != this.InternalQuery.ElementType) ? new DbQuery<TElement>((IInternalQuery<TElement>) this.InternalQuery) : throw System.Data.Entity.Resources.Error.DbEntity_BadTypeForCast((object) typeof (DbQuery).Name, (object) typeof (TElement).Name, (object) this.InternalQuery.ElementType.Name);
    }

    /// <summary>
    /// Returns a <see cref="T:System.String" /> representation of the underlying query.
    /// </summary>
    /// <returns> The query string. </returns>
    public override string ToString() => this.InternalQuery != null ? this.InternalQuery.ToTraceString() : base.ToString();

    private string DebuggerDisplay() => base.ToString();

    /// <summary>
    /// Gets a <see cref="T:System.String" /> representation of the underlying query.
    /// </summary>
    public string Sql => this.ToString();

    internal virtual IInternalQuery InternalQuery => (IInternalQuery) null;

    internal virtual IInternalQuery GetInternalQueryWithCheck(string memberName) => throw new NotImplementedException(System.Data.Entity.Resources.Strings.TestDoubleNotImplemented((object) memberName, (object) this.GetType().Name, (object) typeof (DbSet).Name));

    IInternalQuery IInternalQueryAdapter.InternalQuery => this.InternalQuery;

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
