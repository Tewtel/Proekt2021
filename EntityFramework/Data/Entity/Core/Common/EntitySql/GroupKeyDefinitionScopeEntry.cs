// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.GroupKeyDefinitionScopeEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class GroupKeyDefinitionScopeEntry : 
    ScopeEntry,
    IGroupExpressionExtendedInfo,
    IGetAlternativeName
  {
    private readonly DbExpression _varBasedExpression;
    private readonly DbExpression _groupVarBasedExpression;
    private readonly DbExpression _groupAggBasedExpression;
    private readonly string[] _alternativeName;

    internal GroupKeyDefinitionScopeEntry(
      DbExpression varBasedExpression,
      DbExpression groupVarBasedExpression,
      DbExpression groupAggBasedExpression,
      string[] alternativeName)
      : base(ScopeEntryKind.GroupKeyDefinition)
    {
      this._varBasedExpression = varBasedExpression;
      this._groupVarBasedExpression = groupVarBasedExpression;
      this._groupAggBasedExpression = groupAggBasedExpression;
      this._alternativeName = alternativeName;
    }

    internal override DbExpression GetExpression(string refName, ErrorContext errCtx) => this._varBasedExpression;

    DbExpression IGroupExpressionExtendedInfo.GroupVarBasedExpression => this._groupVarBasedExpression;

    DbExpression IGroupExpressionExtendedInfo.GroupAggBasedExpression => this._groupAggBasedExpression;

    string[] IGetAlternativeName.AlternativeName => this._alternativeName;
  }
}
