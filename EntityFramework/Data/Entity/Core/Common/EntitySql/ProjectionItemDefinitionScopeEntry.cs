// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.ProjectionItemDefinitionScopeEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class ProjectionItemDefinitionScopeEntry : ScopeEntry
  {
    private readonly DbExpression _expression;

    internal ProjectionItemDefinitionScopeEntry(DbExpression expression)
      : base(ScopeEntryKind.ProjectionItemDefinition)
    {
      this._expression = expression;
    }

    internal override DbExpression GetExpression(string refName, ErrorContext errCtx) => this._expression;
  }
}
