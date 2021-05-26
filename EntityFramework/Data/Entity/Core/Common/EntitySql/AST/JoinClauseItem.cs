// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.JoinClauseItem
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class JoinClauseItem : Node
  {
    private readonly FromClauseItem _joinLeft;
    private readonly FromClauseItem _joinRight;
    private readonly Node _onExpr;

    internal JoinClauseItem(FromClauseItem joinLeft, FromClauseItem joinRight, JoinKind joinKind)
      : this(joinLeft, joinRight, joinKind, (Node) null)
    {
    }

    internal JoinClauseItem(
      FromClauseItem joinLeft,
      FromClauseItem joinRight,
      JoinKind joinKind,
      Node onExpr)
    {
      this._joinLeft = joinLeft;
      this._joinRight = joinRight;
      this.JoinKind = joinKind;
      this._onExpr = onExpr;
    }

    internal FromClauseItem LeftExpr => this._joinLeft;

    internal FromClauseItem RightExpr => this._joinRight;

    internal JoinKind JoinKind { get; set; }

    internal Node OnExpr => this._onExpr;
  }
}
