// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.QueryCache.CompiledQueryCacheEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.Internal;

namespace System.Data.Entity.Core.Common.QueryCache
{
  internal sealed class CompiledQueryCacheEntry : QueryCacheEntry
  {
    public readonly MergeOption? PropagatedMergeOption;
    private readonly ConcurrentDictionary<string, ObjectQueryExecutionPlan> _plans;

    internal CompiledQueryCacheEntry(QueryCacheKey queryCacheKey, MergeOption? mergeOption)
      : base(queryCacheKey, (object) null)
    {
      this.PropagatedMergeOption = mergeOption;
      this._plans = new ConcurrentDictionary<string, ObjectQueryExecutionPlan>();
    }

    internal ObjectQueryExecutionPlan GetExecutionPlan(
      MergeOption mergeOption,
      bool useCSharpNullComparisonBehavior)
    {
      ObjectQueryExecutionPlan queryExecutionPlan;
      this._plans.TryGetValue(CompiledQueryCacheEntry.GenerateLocalCacheKey(mergeOption, useCSharpNullComparisonBehavior), out queryExecutionPlan);
      return queryExecutionPlan;
    }

    internal ObjectQueryExecutionPlan SetExecutionPlan(
      ObjectQueryExecutionPlan newPlan,
      bool useCSharpNullComparisonBehavior)
    {
      return this._plans.GetOrAdd(CompiledQueryCacheEntry.GenerateLocalCacheKey(newPlan.MergeOption, useCSharpNullComparisonBehavior), newPlan);
    }

    internal bool TryGetResultType(out TypeUsage resultType)
    {
      using (IEnumerator<ObjectQueryExecutionPlan> enumerator = this._plans.Values.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          ObjectQueryExecutionPlan current = enumerator.Current;
          resultType = current.ResultType;
          return true;
        }
      }
      resultType = (TypeUsage) null;
      return false;
    }

    internal override object GetTarget() => (object) this;

    private static string GenerateLocalCacheKey(
      MergeOption mergeOption,
      bool useCSharpNullComparisonBehavior)
    {
      switch (mergeOption)
      {
        case MergeOption.AppendOnly:
        case MergeOption.OverwriteChanges:
        case MergeOption.PreserveChanges:
        case MergeOption.NoTracking:
          return string.Join("", (object) Enum.GetName(typeof (MergeOption), (object) mergeOption), (object) useCSharpNullComparisonBehavior);
        default:
          throw new ArgumentOutOfRangeException("newPlan.MergeOption");
      }
    }
  }
}
