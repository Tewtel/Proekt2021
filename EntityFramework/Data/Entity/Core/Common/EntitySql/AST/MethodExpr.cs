// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.MethodExpr
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class MethodExpr : GroupAggregateExpr
  {
    private readonly Node _expr;
    private readonly NodeList<Node> _args;
    private readonly NodeList<RelshipNavigationExpr> _relationships;

    internal MethodExpr(Node expr, DistinctKind distinctKind, NodeList<Node> args)
      : this(expr, distinctKind, args, (NodeList<RelshipNavigationExpr>) null)
    {
    }

    internal MethodExpr(
      Node expr,
      DistinctKind distinctKind,
      NodeList<Node> args,
      NodeList<RelshipNavigationExpr> relationships)
      : base(distinctKind)
    {
      this._expr = expr;
      this._args = args;
      this._relationships = relationships;
    }

    internal Node Expr => this._expr;

    internal NodeList<Node> Args => this._args;

    internal bool HasRelationships => this._relationships != null && this._relationships.Count > 0;

    internal NodeList<RelshipNavigationExpr> Relationships => this._relationships;
  }
}
