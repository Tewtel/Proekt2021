// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.Simplifier`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal class Simplifier<T_Identifier> : BasicVisitor<T_Identifier>
  {
    internal static readonly Simplifier<T_Identifier> Instance = new Simplifier<T_Identifier>();

    protected Simplifier()
    {
    }

    internal override BoolExpr<T_Identifier> VisitNot(NotExpr<T_Identifier> expression)
    {
      BoolExpr<T_Identifier> boolExpr = expression.Child.Accept<BoolExpr<T_Identifier>>((Visitor<T_Identifier, BoolExpr<T_Identifier>>) this);
      switch (boolExpr.ExprType)
      {
        case ExprType.Not:
          return ((NotExpr<T_Identifier>) boolExpr).Child;
        case ExprType.True:
          return (BoolExpr<T_Identifier>) FalseExpr<T_Identifier>.Value;
        case ExprType.False:
          return (BoolExpr<T_Identifier>) TrueExpr<T_Identifier>.Value;
        default:
          return base.VisitNot(expression);
      }
    }

    internal override BoolExpr<T_Identifier> VisitAnd(AndExpr<T_Identifier> expression) => this.SimplifyTree((TreeExpr<T_Identifier>) expression);

    internal override BoolExpr<T_Identifier> VisitOr(OrExpr<T_Identifier> expression) => this.SimplifyTree((TreeExpr<T_Identifier>) expression);

    private BoolExpr<T_Identifier> SimplifyTree(TreeExpr<T_Identifier> tree)
    {
      bool flag = tree.ExprType == ExprType.And;
      List<BoolExpr<T_Identifier>> boolExprList1 = new List<BoolExpr<T_Identifier>>(tree.Children.Count);
      foreach (BoolExpr<T_Identifier> child in tree.Children)
      {
        BoolExpr<T_Identifier> boolExpr = child.Accept<BoolExpr<T_Identifier>>((Visitor<T_Identifier, BoolExpr<T_Identifier>>) this);
        if (boolExpr.ExprType == tree.ExprType)
          boolExprList1.AddRange((IEnumerable<BoolExpr<T_Identifier>>) ((TreeExpr<T_Identifier>) boolExpr).Children);
        else
          boolExprList1.Add(boolExpr);
      }
      Dictionary<BoolExpr<T_Identifier>, bool> dictionary = new Dictionary<BoolExpr<T_Identifier>, bool>(tree.Children.Count);
      List<BoolExpr<T_Identifier>> boolExprList2 = new List<BoolExpr<T_Identifier>>(tree.Children.Count);
      foreach (BoolExpr<T_Identifier> boolExpr in boolExprList1)
      {
        switch (boolExpr.ExprType)
        {
          case ExprType.Not:
            dictionary[((NotExpr<T_Identifier>) boolExpr).Child] = true;
            continue;
          case ExprType.True:
            if (!flag)
              return (BoolExpr<T_Identifier>) TrueExpr<T_Identifier>.Value;
            continue;
          case ExprType.False:
            if (flag)
              return (BoolExpr<T_Identifier>) FalseExpr<T_Identifier>.Value;
            continue;
          default:
            boolExprList2.Add(boolExpr);
            continue;
        }
      }
      List<BoolExpr<T_Identifier>> boolExprList3 = new List<BoolExpr<T_Identifier>>();
      foreach (BoolExpr<T_Identifier> key in boolExprList2)
      {
        if (dictionary.ContainsKey(key))
          return flag ? (BoolExpr<T_Identifier>) FalseExpr<T_Identifier>.Value : (BoolExpr<T_Identifier>) TrueExpr<T_Identifier>.Value;
        boolExprList3.Add(key);
      }
      foreach (BoolExpr<T_Identifier> key in dictionary.Keys)
        boolExprList3.Add(key.MakeNegated());
      return boolExprList3.Count == 0 ? (flag ? (BoolExpr<T_Identifier>) TrueExpr<T_Identifier>.Value : (BoolExpr<T_Identifier>) FalseExpr<T_Identifier>.Value) : (1 == boolExprList3.Count ? boolExprList3[0] : (!flag ? (BoolExpr<T_Identifier>) new OrExpr<T_Identifier>((IEnumerable<BoolExpr<T_Identifier>>) boolExprList3) : (BoolExpr<T_Identifier>) new AndExpr<T_Identifier>((IEnumerable<BoolExpr<T_Identifier>>) boolExprList3)));
    }
  }
}
