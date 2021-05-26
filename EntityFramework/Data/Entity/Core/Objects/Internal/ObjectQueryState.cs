﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.ObjectQueryState
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Utilities;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal abstract class ObjectQueryState
  {
    internal static readonly MergeOption DefaultMergeOption = MergeOption.AppendOnly;
    internal static readonly MethodInfo CreateObjectQueryMethod = typeof (ObjectQueryState).GetOnlyDeclaredMethod("CreateObjectQuery");
    private readonly ObjectContext _context;
    private readonly Type _elementType;
    private ObjectParameterCollection _parameters;
    private readonly Span _span;
    private MergeOption? _userMergeOption;
    private bool _cachingEnabled = true;
    protected ObjectQueryExecutionPlan _cachedPlan;

    protected ObjectQueryState(
      Type elementType,
      ObjectContext context,
      ObjectParameterCollection parameters,
      Span span)
    {
      this._elementType = elementType;
      this._context = context;
      this._span = span;
      this._parameters = parameters;
    }

    protected ObjectQueryState(Type elementType, ObjectQuery query)
      : this(elementType, query.Context, (ObjectParameterCollection) null, (Span) null)
    {
      this._cachingEnabled = query.EnablePlanCaching;
      this.UserSpecifiedStreamingBehavior = query.QueryState.UserSpecifiedStreamingBehavior;
      this.ExecutionStrategy = query.QueryState.ExecutionStrategy;
    }

    internal bool EffectiveStreamingBehavior => this.UserSpecifiedStreamingBehavior ?? this.DefaultStreamingBehavior;

    internal bool? UserSpecifiedStreamingBehavior { get; set; }

    internal bool DefaultStreamingBehavior => !(this.ExecutionStrategy ?? DbProviderServices.GetExecutionStrategy(this.ObjectContext.Connection, this.ObjectContext.MetadataWorkspace)).RetriesOnFailure;

    internal IDbExecutionStrategy ExecutionStrategy { get; set; }

    internal Type ElementType => this._elementType;

    internal ObjectContext ObjectContext => this._context;

    internal ObjectParameterCollection Parameters => this._parameters;

    internal ObjectParameterCollection EnsureParameters()
    {
      if (this._parameters == null)
      {
        this._parameters = new ObjectParameterCollection(this.ObjectContext.Perspective);
        if (this._cachedPlan != null)
          this._parameters.SetReadOnly(true);
      }
      return this._parameters;
    }

    internal Span Span => this._span;

    internal MergeOption EffectiveMergeOption
    {
      get
      {
        if (this._userMergeOption.HasValue)
          return this._userMergeOption.Value;
        ObjectQueryExecutionPlan cachedPlan = this._cachedPlan;
        return cachedPlan != null ? cachedPlan.MergeOption : ObjectQueryState.DefaultMergeOption;
      }
    }

    internal MergeOption? UserSpecifiedMergeOption
    {
      get => this._userMergeOption;
      set => this._userMergeOption = value;
    }

    internal bool PlanCachingEnabled
    {
      get => this._cachingEnabled;
      set => this._cachingEnabled = value;
    }

    internal TypeUsage ResultType
    {
      get
      {
        ObjectQueryExecutionPlan cachedPlan = this._cachedPlan;
        return cachedPlan != null ? cachedPlan.ResultType : this.GetResultType();
      }
    }

    internal void ApplySettingsTo(ObjectQueryState other)
    {
      other.PlanCachingEnabled = this.PlanCachingEnabled;
      other.UserSpecifiedMergeOption = this.UserSpecifiedMergeOption;
    }

    internal abstract bool TryGetCommandText(out string commandText);

    internal abstract bool TryGetExpression(out Expression expression);

    internal abstract ObjectQueryExecutionPlan GetExecutionPlan(
      MergeOption? forMergeOption);

    internal abstract ObjectQueryState Include<TElementType>(
      ObjectQuery<TElementType> sourceQuery,
      string includePath);

    protected abstract TypeUsage GetResultType();

    protected static MergeOption EnsureMergeOption(
      params MergeOption?[] preferredMergeOptions)
    {
      foreach (MergeOption? preferredMergeOption in preferredMergeOptions)
      {
        if (preferredMergeOption.HasValue)
          return preferredMergeOption.Value;
      }
      return ObjectQueryState.DefaultMergeOption;
    }

    protected static MergeOption? GetMergeOption(
      params MergeOption?[] preferredMergeOptions)
    {
      foreach (MergeOption? preferredMergeOption in preferredMergeOptions)
      {
        if (preferredMergeOption.HasValue)
          return new MergeOption?(preferredMergeOption.Value);
      }
      return new MergeOption?();
    }

    public ObjectQuery CreateQuery() => (ObjectQuery) ObjectQueryState.CreateObjectQueryMethod.MakeGenericMethod(this._elementType).Invoke((object) this, new object[0]);

    public ObjectQuery<TResultType> CreateObjectQuery<TResultType>() => new ObjectQuery<TResultType>(this);
  }
}
