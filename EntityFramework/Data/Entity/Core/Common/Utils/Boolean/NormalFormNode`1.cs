// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.NormalFormNode`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal abstract class NormalFormNode<T_Identifier>
  {
    private readonly BoolExpr<T_Identifier> _expr;

    protected NormalFormNode(BoolExpr<T_Identifier> expr) => this._expr = expr.Simplify();

    internal BoolExpr<T_Identifier> Expr => this._expr;

    protected static BoolExpr<T_Identifier> ExprSelector<T_NormalFormNode>(
      T_NormalFormNode node)
      where T_NormalFormNode : NormalFormNode<T_Identifier>
    {
      return node._expr;
    }
  }
}
