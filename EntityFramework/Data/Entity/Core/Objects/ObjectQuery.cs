// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ObjectQuery
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.ComponentModel;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.ELinq;
using System.Data.Entity.Core.Objects.Internal;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Core.Objects
{
  /// <summary>
  /// This class implements untyped queries at the object-layer.
  /// </summary>
  public abstract class ObjectQuery : 
    IEnumerable,
    IOrderedQueryable,
    IQueryable,
    IListSource,
    IDbAsyncEnumerable
  {
    private readonly ObjectQueryState _state;
    private TypeUsage _resultType;
    private ObjectQueryProvider _provider;

    internal ObjectQuery(ObjectQueryState queryState) => this._state = queryState;

    internal ObjectQuery()
    {
    }

    internal ObjectQueryState QueryState => this._state;

    internal virtual ObjectQueryProvider ObjectQueryProvider
    {
      get
      {
        if (this._provider == null)
          this._provider = new ObjectQueryProvider(this);
        return this._provider;
      }
    }

    internal IDbExecutionStrategy ExecutionStrategy
    {
      get => this.QueryState.ExecutionStrategy;
      set => this.QueryState.ExecutionStrategy = value;
    }

    bool IListSource.ContainsListCollection => false;

    /// <summary>Returns the command text for the query.</summary>
    /// <returns>A string value.</returns>
    public string CommandText
    {
      get
      {
        string commandText;
        return !this._state.TryGetCommandText(out commandText) ? string.Empty : commandText;
      }
    }

    /// <summary>Gets the object context associated with this object query.</summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" /> associated with this
    /// <see cref="T:System.Data.Entity.Core.Objects.ObjectQuery`1" />
    /// instance.
    /// </returns>
    public ObjectContext Context => this._state.ObjectContext;

    /// <summary>Gets or sets how objects returned from a query are added to the object context. </summary>
    /// <returns>
    /// The query <see cref="T:System.Data.Entity.Core.Objects.MergeOption" />.
    /// </returns>
    public MergeOption MergeOption
    {
      get => this._state.EffectiveMergeOption;
      set
      {
        EntityUtil.CheckArgumentMergeOption(value);
        this._state.UserSpecifiedMergeOption = new MergeOption?(value);
      }
    }

    /// <summary>Whether the query is streaming or buffering</summary>
    public bool Streaming
    {
      get => this._state.EffectiveStreamingBehavior;
      set => this._state.UserSpecifiedStreamingBehavior = new bool?(value);
    }

    /// <summary>Gets the parameter collection for this object query.</summary>
    /// <returns>
    /// The parameter collection for this <see cref="T:System.Data.Entity.Core.Objects.ObjectQuery`1" />.
    /// </returns>
    public ObjectParameterCollection Parameters => this._state.EnsureParameters();

    /// <summary>Gets or sets a value that indicates whether the query plan should be cached.</summary>
    /// <returns>A value that indicates whether the query plan should be cached.</returns>
    public bool EnablePlanCaching
    {
      get => this._state.PlanCachingEnabled;
      set => this._state.PlanCachingEnabled = value;
    }

    /// <summary>Returns the commands to execute against the data source.</summary>
    /// <returns>A string that represents the commands that the query executes against the data source.</returns>
    [Browsable(false)]
    public string ToTraceString() => this._state.GetExecutionPlan(new MergeOption?()).ToTraceString();

    /// <summary>Returns information about the result type of the query.</summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.TypeUsage" /> value that contains information about the result type of the query.
    /// </returns>
    public TypeUsage GetResultType()
    {
      if (this._resultType == null)
      {
        TypeUsage resultType = this._state.ResultType;
        TypeUsage elementType;
        if (!TypeHelpers.TryGetCollectionElementType(resultType, out elementType))
          elementType = resultType;
        this._resultType = this._state.ObjectContext.Perspective.MetadataWorkspace.GetOSpaceTypeUsage(elementType) ?? throw new InvalidOperationException(System.Data.Entity.Resources.Strings.ObjectQuery_UnableToMapResultType);
      }
      return this._resultType;
    }

    /// <summary>Executes the untyped object query with the specified merge option.</summary>
    /// <param name="mergeOption">
    /// The <see cref="T:System.Data.Entity.Core.Objects.MergeOption" /> to use when executing the query.
    /// The default is <see cref="F:System.Data.Entity.Core.Objects.MergeOption.AppendOnly" />.
    /// </param>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Objects.ObjectResult`1" /> that contains a collection of entity objects returned by the query.
    /// </returns>
    public ObjectResult Execute(MergeOption mergeOption)
    {
      EntityUtil.CheckArgumentMergeOption(mergeOption);
      return this.ExecuteInternal(mergeOption);
    }

    /// <summary>
    /// Asynchronously executes the untyped object query with the specified merge option.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="mergeOption">
    /// The <see cref="T:System.Data.Entity.Core.Objects.MergeOption" /> to use when executing the query.
    /// The default is <see cref="F:System.Data.Entity.Core.Objects.MergeOption.AppendOnly" />.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="T:System.Data.Entity.Core.Objects.ObjectResult`1" />
    /// that contains a collection of entity objects returned by the query.
    /// </returns>
    public Task<ObjectResult> ExecuteAsync(MergeOption mergeOption) => this.ExecuteAsync(mergeOption, CancellationToken.None);

    /// <summary>
    /// Asynchronously executes the untyped object query with the specified merge option.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="mergeOption">
    /// The <see cref="T:System.Data.Entity.Core.Objects.MergeOption" /> to use when executing the query.
    /// The default is <see cref="F:System.Data.Entity.Core.Objects.MergeOption.AppendOnly" />.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an <see cref="T:System.Data.Entity.Core.Objects.ObjectResult`1" />
    /// that contains a collection of entity objects returned by the query.
    /// </returns>
    public Task<ObjectResult> ExecuteAsync(
      MergeOption mergeOption,
      CancellationToken cancellationToken)
    {
      EntityUtil.CheckArgumentMergeOption(mergeOption);
      cancellationToken.ThrowIfCancellationRequested();
      return this.ExecuteInternalAsync(mergeOption, cancellationToken);
    }

    /// <summary>
    /// Returns the collection as an <see cref="T:System.Collections.IList" /> used for data binding.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IList" /> of entity objects.
    /// </returns>
    IList IListSource.GetList() => this.GetIListSourceListInternal();

    /// <summary>Gets the result element type for this query instance.</summary>
    Type IQueryable.ElementType => this._state.ElementType;

    /// <summary>
    /// Gets the expression describing this query. For queries built using
    /// LINQ builder patterns, returns a full LINQ expression tree; otherwise,
    /// returns a constant expression wrapping this query. Note that the
    /// default expression is not cached. This allows us to differentiate
    /// between LINQ and Entity-SQL queries.
    /// </summary>
    Expression IQueryable.Expression => this.GetExpression();

    /// <summary>
    /// Gets the <see cref="T:System.Linq.IQueryProvider" /> associated with this query instance.
    /// </summary>
    IQueryProvider IQueryable.Provider => (IQueryProvider) this.ObjectQueryProvider;

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumeratorInternal();

    /// <summary>
    /// Returns an <see cref="T:System.Data.Entity.Infrastructure.IDbAsyncEnumerator" /> which when enumerated will execute the given SQL query against the database.
    /// </summary>
    /// <returns> The query results. </returns>
    IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() => this.GetAsyncEnumeratorInternal();

    internal abstract Expression GetExpression();

    internal abstract IEnumerator GetEnumeratorInternal();

    internal abstract IDbAsyncEnumerator GetAsyncEnumeratorInternal();

    internal abstract Task<ObjectResult> ExecuteInternalAsync(
      MergeOption mergeOption,
      CancellationToken cancellationToken);

    internal abstract IList GetIListSourceListInternal();

    internal abstract ObjectResult ExecuteInternal(MergeOption mergeOption);
  }
}
