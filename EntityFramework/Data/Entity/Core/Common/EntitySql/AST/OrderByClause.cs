// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.OrderByClause
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class OrderByClause : Node
  {
    private readonly NodeList<System.Data.Entity.Core.Common.EntitySql.AST.OrderByClauseItem> _orderByClauseItem;
    private readonly Node _skipExpr;
    private readonly Node _limitExpr;
    private readonly uint _methodCallCount;

    internal OrderByClause(
      NodeList<System.Data.Entity.Core.Common.EntitySql.AST.OrderByClauseItem> orderByClauseItem,
      Node skipExpr,
      Node limitExpr,
      uint methodCallCount)
    {
      this._orderByClauseItem = orderByClauseItem;
      this._skipExpr = skipExpr;
      this._limitExpr = limitExpr;
      this._methodCallCount = methodCallCount;
    }

    internal NodeList<System.Data.Entity.Core.Common.EntitySql.AST.OrderByClauseItem> OrderByClauseItem => this._orderByClauseItem;

    internal Node SkipSubClause => this._skipExpr;

    internal Node LimitSubClause => this._limitExpr;

    internal bool HasMethodCall => this._methodCallCount > 0U;
  }
}
