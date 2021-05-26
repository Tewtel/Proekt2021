// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.GroupPartitionInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.EntitySql.AST;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class GroupPartitionInfo : GroupAggregateInfo
  {
    internal DbExpression AggregateDefinition;

    internal GroupPartitionInfo(
      GroupPartitionExpr groupPartitionExpr,
      ErrorContext errCtx,
      GroupAggregateInfo containingAggregate,
      ScopeRegion definingScopeRegion)
      : base(GroupAggregateKind.Partition, (GroupAggregateExpr) groupPartitionExpr, errCtx, containingAggregate, definingScopeRegion)
    {
    }

    internal void AttachToAstNode(string aggregateName, DbExpression aggregateDefinition)
    {
      this.AttachToAstNode(aggregateName, aggregateDefinition.ResultType);
      this.AggregateDefinition = aggregateDefinition;
    }
  }
}
