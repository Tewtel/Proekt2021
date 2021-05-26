// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ELinq.ELinqQueryState
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.Internal;
using System.Data.Entity.Core.Common.QueryCache;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.Internal;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.Core.Objects.ELinq
{
  internal class ELinqQueryState : ObjectQueryState
  {
    private readonly Expression _expression;
    private Func<bool> _recompileRequired;
    private IEnumerable<Tuple<ObjectParameter, QueryParameterExpression>> _linqParameters;
    private bool _useCSharpNullComparisonBehavior;
    private bool _disableFilterOverProjectionSimplificationForCustomFunctions;
    private readonly ObjectQueryExecutionPlanFactory _objectQueryExecutionPlanFactory;

    internal ELinqQueryState(
      Type elementType,
      ObjectContext context,
      Expression expression,
      ObjectQueryExecutionPlanFactory objectQueryExecutionPlanFactory = null)
      : base(elementType, context, (ObjectParameterCollection) null, (Span) null)
    {
      this._expression = expression;
      this._useCSharpNullComparisonBehavior = context.ContextOptions.UseCSharpNullComparisonBehavior;
      this._disableFilterOverProjectionSimplificationForCustomFunctions = context.ContextOptions.DisableFilterOverProjectionSimplificationForCustomFunctions;
      this._objectQueryExecutionPlanFactory = objectQueryExecutionPlanFactory ?? new ObjectQueryExecutionPlanFactory();
    }

    internal ELinqQueryState(
      Type elementType,
      ObjectQuery query,
      Expression expression,
      ObjectQueryExecutionPlanFactory objectQueryExecutionPlanFactory = null)
      : base(elementType, query)
    {
      this._expression = expression;
      this._objectQueryExecutionPlanFactory = objectQueryExecutionPlanFactory ?? new ObjectQueryExecutionPlanFactory();
    }

    protected override TypeUsage GetResultType() => this.CreateExpressionConverter().Convert().ResultType;

    internal override ObjectQueryExecutionPlan GetExecutionPlan(
      MergeOption? forMergeOption)
    {
      ObjectQueryExecutionPlan queryExecutionPlan1 = this._cachedPlan;
      if (queryExecutionPlan1 != null)
      {
        MergeOption? mergeOption = ObjectQueryState.GetMergeOption(forMergeOption, this.UserSpecifiedMergeOption);
        if (mergeOption.HasValue && mergeOption.Value != queryExecutionPlan1.MergeOption || (this._recompileRequired() || this.ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior != this._useCSharpNullComparisonBehavior) || this.ObjectContext.ContextOptions.DisableFilterOverProjectionSimplificationForCustomFunctions != this._disableFilterOverProjectionSimplificationForCustomFunctions)
          queryExecutionPlan1 = (ObjectQueryExecutionPlan) null;
      }
      if (queryExecutionPlan1 == null)
      {
        this._recompileRequired = (Func<bool>) null;
        this.ResetParameters();
        ExpressionConverter expressionConverter = this.CreateExpressionConverter();
        DbExpression dbExpression = expressionConverter.Convert();
        this._recompileRequired = expressionConverter.RecompileRequired;
        MergeOption mergeOption = ObjectQueryState.EnsureMergeOption(forMergeOption, this.UserSpecifiedMergeOption, expressionConverter.PropagatedMergeOption);
        this._useCSharpNullComparisonBehavior = this.ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior;
        this._disableFilterOverProjectionSimplificationForCustomFunctions = this.ObjectContext.ContextOptions.DisableFilterOverProjectionSimplificationForCustomFunctions;
        this._linqParameters = expressionConverter.GetParameters();
        if (this._linqParameters != null && this._linqParameters.Any<Tuple<ObjectParameter, QueryParameterExpression>>())
        {
          ObjectParameterCollection parameterCollection = this.EnsureParameters();
          parameterCollection.SetReadOnly(false);
          foreach (Tuple<ObjectParameter, QueryParameterExpression> linqParameter in this._linqParameters)
          {
            ObjectParameter objectParameter = linqParameter.Item1;
            parameterCollection.Add(objectParameter);
          }
          parameterCollection.SetReadOnly(true);
        }
        QueryCacheManager queryCacheManager = (QueryCacheManager) null;
        LinqQueryCacheKey key1 = (LinqQueryCacheKey) null;
        string key2;
        if (this.PlanCachingEnabled && !this._recompileRequired() && ExpressionKeyGen.TryGenerateKey(dbExpression, out key2))
        {
          key1 = new LinqQueryCacheKey(key2, this.Parameters == null ? 0 : this.Parameters.Count, this.Parameters == null ? (string) null : this.Parameters.GetCacheKey(), expressionConverter.PropagatedSpan == null ? (string) null : expressionConverter.PropagatedSpan.GetCacheKey(), mergeOption, this.EffectiveStreamingBehavior, this._useCSharpNullComparisonBehavior, this.ElementType);
          queryCacheManager = this.ObjectContext.MetadataWorkspace.GetQueryCacheManager();
          ObjectQueryExecutionPlan queryExecutionPlan2 = (ObjectQueryExecutionPlan) null;
          if (queryCacheManager.TryCacheLookup<LinqQueryCacheKey, ObjectQueryExecutionPlan>(key1, out queryExecutionPlan2))
            queryExecutionPlan1 = queryExecutionPlan2;
        }
        if (queryExecutionPlan1 == null)
        {
          queryExecutionPlan1 = this._objectQueryExecutionPlanFactory.Prepare(this.ObjectContext, DbQueryCommandTree.FromValidExpression(this.ObjectContext.MetadataWorkspace, DataSpace.CSpace, dbExpression, !this._useCSharpNullComparisonBehavior, this._disableFilterOverProjectionSimplificationForCustomFunctions), this.ElementType, mergeOption, this.EffectiveStreamingBehavior, expressionConverter.PropagatedSpan, (IEnumerable<Tuple<ObjectParameter, QueryParameterExpression>>) null, expressionConverter.AliasGenerator);
          if (key1 != null)
          {
            QueryCacheEntry inQueryCacheEntry = new QueryCacheEntry((QueryCacheKey) key1, (object) queryExecutionPlan1);
            QueryCacheEntry outQueryCacheEntry = (QueryCacheEntry) null;
            if (queryCacheManager.TryLookupAndAdd(inQueryCacheEntry, out outQueryCacheEntry))
              queryExecutionPlan1 = (ObjectQueryExecutionPlan) outQueryCacheEntry.GetTarget();
          }
        }
        this._cachedPlan = queryExecutionPlan1;
      }
      if (this._linqParameters != null)
      {
        foreach (Tuple<ObjectParameter, QueryParameterExpression> linqParameter in this._linqParameters)
        {
          ObjectParameter objectParameter = linqParameter.Item1;
          QueryParameterExpression parameterExpression = linqParameter.Item2;
          if (parameterExpression != null)
            objectParameter.Value = parameterExpression.EvaluateParameter((object[]) null);
        }
      }
      return queryExecutionPlan1;
    }

    internal override ObjectQueryState Include<TElementType>(
      ObjectQuery<TElementType> sourceQuery,
      string includePath)
    {
      MethodInfo includeMethod = ELinqQueryState.GetIncludeMethod<TElementType>(sourceQuery);
      ObjectQueryState other = (ObjectQueryState) new ELinqQueryState(this.ElementType, this.ObjectContext, (Expression) Expression.Call((Expression) Expression.Constant((object) sourceQuery), includeMethod, (Expression) Expression.Constant((object) includePath, typeof (string))));
      this.ApplySettingsTo(other);
      return other;
    }

    internal static MethodInfo GetIncludeMethod<TElementType>(
      ObjectQuery<TElementType> sourceQuery)
    {
      return sourceQuery.GetType().GetOnlyDeclaredMethod("Include");
    }

    internal override bool TryGetCommandText(out string commandText)
    {
      commandText = (string) null;
      return false;
    }

    internal override bool TryGetExpression(out Expression expression)
    {
      expression = this.Expression;
      return true;
    }

    internal virtual Expression Expression => this._expression;

    protected virtual ExpressionConverter CreateExpressionConverter() => new ExpressionConverter(Funcletizer.CreateQueryFuncletizer(this.ObjectContext), this._expression);

    private void ResetParameters()
    {
      if (this.Parameters != null)
      {
        int num = ((ICollection<ObjectParameter>) this.Parameters).IsReadOnly ? 1 : 0;
        if (num != 0)
          this.Parameters.SetReadOnly(false);
        this.Parameters.Clear();
        if (num != 0)
          this.Parameters.SetReadOnly(true);
      }
      this._linqParameters = (IEnumerable<Tuple<ObjectParameter, QueryParameterExpression>>) null;
    }
  }
}
