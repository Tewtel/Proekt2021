﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.GroupKeyAggregateInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.EntitySql.AST;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class GroupKeyAggregateInfo : GroupAggregateInfo
  {
    internal GroupKeyAggregateInfo(
      GroupAggregateKind aggregateKind,
      ErrorContext errCtx,
      GroupAggregateInfo containingAggregate,
      ScopeRegion definingScopeRegion)
      : base(aggregateKind, (GroupAggregateExpr) null, errCtx, containingAggregate, definingScopeRegion)
    {
    }
  }
}
