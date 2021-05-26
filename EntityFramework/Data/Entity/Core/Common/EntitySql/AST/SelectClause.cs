// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.SelectClause
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class SelectClause : Node
  {
    private readonly NodeList<AliasedExpr> _selectClauseItems;
    private readonly SelectKind _selectKind;
    private readonly DistinctKind _distinctKind;
    private readonly Node _topExpr;
    private readonly uint _methodCallCount;

    internal SelectClause(
      NodeList<AliasedExpr> items,
      SelectKind selectKind,
      DistinctKind distinctKind,
      Node topExpr,
      uint methodCallCount)
    {
      this._selectKind = selectKind;
      this._selectClauseItems = items;
      this._distinctKind = distinctKind;
      this._topExpr = topExpr;
      this._methodCallCount = methodCallCount;
    }

    internal NodeList<AliasedExpr> Items => this._selectClauseItems;

    internal SelectKind SelectKind => this._selectKind;

    internal DistinctKind DistinctKind => this._distinctKind;

    internal Node TopExpr => this._topExpr;

    internal bool HasMethodCall => this._methodCallCount > 0U;
  }
}
