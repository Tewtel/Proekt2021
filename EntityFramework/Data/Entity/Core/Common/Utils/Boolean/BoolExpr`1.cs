// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.BoolExpr`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal abstract class BoolExpr<T_Identifier> : IEquatable<BoolExpr<T_Identifier>>
  {
    internal abstract ExprType ExprType { get; }

    internal abstract T_Return Accept<T_Return>(Visitor<T_Identifier, T_Return> visitor);

    internal BoolExpr<T_Identifier> Simplify() => IdentifierService<T_Identifier>.Instance.LocalSimplify(this);

    internal BoolExpr<T_Identifier> ExpensiveSimplify(out Converter<T_Identifier> converter)
    {
      ConversionContext<T_Identifier> conversionContext = IdentifierService<T_Identifier>.Instance.CreateConversionContext();
      converter = new Converter<T_Identifier>(this, conversionContext);
      if (converter.Vertex.IsOne())
        return (BoolExpr<T_Identifier>) TrueExpr<T_Identifier>.Value;
      if (converter.Vertex.IsZero())
        return (BoolExpr<T_Identifier>) FalseExpr<T_Identifier>.Value;
      return BoolExpr<T_Identifier>.ChooseCandidate(this, converter.Cnf.Expr, converter.Dnf.Expr);
    }

    private static BoolExpr<T_Identifier> ChooseCandidate(
      params BoolExpr<T_Identifier>[] candidates)
    {
      int num1 = 0;
      int num2 = 0;
      BoolExpr<T_Identifier> boolExpr1 = (BoolExpr<T_Identifier>) null;
      foreach (BoolExpr<T_Identifier> candidate in candidates)
      {
        BoolExpr<T_Identifier> boolExpr2 = candidate.Simplify();
        int num3 = boolExpr2.GetTerms().Distinct<TermExpr<T_Identifier>>().Count<TermExpr<T_Identifier>>();
        int num4 = boolExpr2.CountTerms();
        if (boolExpr1 == null || num3 < num1 || num3 == num1 && num4 < num2)
        {
          boolExpr1 = boolExpr2;
          num1 = num3;
          num2 = num4;
        }
      }
      return boolExpr1;
    }

    internal List<TermExpr<T_Identifier>> GetTerms() => LeafVisitor<T_Identifier>.GetTerms(this);

    internal int CountTerms() => TermCounter<T_Identifier>.CountTerms(this);

    public static implicit operator BoolExpr<T_Identifier>(T_Identifier value) => (BoolExpr<T_Identifier>) new TermExpr<T_Identifier>(value);

    internal virtual BoolExpr<T_Identifier> MakeNegated() => (BoolExpr<T_Identifier>) new NotExpr<T_Identifier>(this);

    public override string ToString() => this.ExprType.ToString();

    public bool Equals(BoolExpr<T_Identifier> other) => other != null && this.ExprType == other.ExprType && this.EquivalentTypeEquals(other);

    protected abstract bool EquivalentTypeEquals(BoolExpr<T_Identifier> other);
  }
}
