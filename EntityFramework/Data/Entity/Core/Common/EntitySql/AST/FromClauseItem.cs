// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.FromClauseItem
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class FromClauseItem : Node
  {
    private readonly Node _fromClauseItemExpr;
    private readonly FromClauseItemKind _fromClauseItemKind;

    internal FromClauseItem(AliasedExpr aliasExpr)
    {
      this._fromClauseItemExpr = (Node) aliasExpr;
      this._fromClauseItemKind = FromClauseItemKind.AliasedFromClause;
    }

    internal FromClauseItem(JoinClauseItem joinClauseItem)
    {
      this._fromClauseItemExpr = (Node) joinClauseItem;
      this._fromClauseItemKind = FromClauseItemKind.JoinFromClause;
    }

    internal FromClauseItem(ApplyClauseItem applyClauseItem)
    {
      this._fromClauseItemExpr = (Node) applyClauseItem;
      this._fromClauseItemKind = FromClauseItemKind.ApplyFromClause;
    }

    internal Node FromExpr => this._fromClauseItemExpr;

    internal FromClauseItemKind FromClauseItemKind => this._fromClauseItemKind;
  }
}
