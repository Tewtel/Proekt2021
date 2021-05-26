// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.Clause`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal abstract class Clause<T_Identifier> : NormalFormNode<T_Identifier>
  {
    private readonly Set<Literal<T_Identifier>> _literals;
    private readonly int _hashCode;

    protected Clause(Set<Literal<T_Identifier>> literals, ExprType treeType)
      : base(Clause<T_Identifier>.ConvertLiteralsToExpr(literals, treeType))
    {
      this._literals = literals.AsReadOnly();
      this._hashCode = this._literals.GetElementsHashCode();
    }

    internal Set<Literal<T_Identifier>> Literals => this._literals;

    private static BoolExpr<T_Identifier> ConvertLiteralsToExpr(
      Set<Literal<T_Identifier>> literals,
      ExprType treeType)
    {
      int num = treeType == ExprType.And ? 1 : 0;
      IEnumerable<BoolExpr<T_Identifier>> children = literals.Select<Literal<T_Identifier>, BoolExpr<T_Identifier>>(new Func<Literal<T_Identifier>, BoolExpr<T_Identifier>>(Clause<T_Identifier>.ConvertLiteralToExpression));
      return num != 0 ? (BoolExpr<T_Identifier>) new AndExpr<T_Identifier>(children) : (BoolExpr<T_Identifier>) new OrExpr<T_Identifier>(children);
    }

    private static BoolExpr<T_Identifier> ConvertLiteralToExpression(
      Literal<T_Identifier> literal)
    {
      return literal.Expr;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Clause{");
      stringBuilder.Append((object) this._literals);
      return stringBuilder.Append("}").ToString();
    }

    public override int GetHashCode() => this._hashCode;

    public override bool Equals(object obj) => base.Equals(obj);
  }
}
