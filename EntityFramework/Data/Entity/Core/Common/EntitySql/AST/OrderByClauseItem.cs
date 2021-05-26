// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.OrderByClauseItem
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class OrderByClauseItem : Node
  {
    private readonly Node _orderExpr;
    private readonly OrderKind _orderKind;
    private readonly Identifier _optCollationIdentifier;

    internal OrderByClauseItem(Node orderExpr, OrderKind orderKind)
      : this(orderExpr, orderKind, (Identifier) null)
    {
    }

    internal OrderByClauseItem(
      Node orderExpr,
      OrderKind orderKind,
      Identifier optCollationIdentifier)
    {
      this._orderExpr = orderExpr;
      this._orderKind = orderKind;
      this._optCollationIdentifier = optCollationIdentifier;
    }

    internal Node OrderExpr => this._orderExpr;

    internal OrderKind OrderKind => this._orderKind;

    internal Identifier Collation => this._optCollationIdentifier;
  }
}
