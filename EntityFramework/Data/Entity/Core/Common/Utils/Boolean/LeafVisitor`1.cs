﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.LeafVisitor`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal class LeafVisitor<T_Identifier> : Visitor<T_Identifier, bool>
  {
    private readonly List<TermExpr<T_Identifier>> _terms;

    private LeafVisitor() => this._terms = new List<TermExpr<T_Identifier>>();

    internal static List<TermExpr<T_Identifier>> GetTerms(
      BoolExpr<T_Identifier> expression)
    {
      LeafVisitor<T_Identifier> leafVisitor = new LeafVisitor<T_Identifier>();
      expression.Accept<bool>((Visitor<T_Identifier, bool>) leafVisitor);
      return leafVisitor._terms;
    }

    internal static IEnumerable<T_Identifier> GetLeaves(BoolExpr<T_Identifier> expression) => LeafVisitor<T_Identifier>.GetTerms(expression).Select<TermExpr<T_Identifier>, T_Identifier>((Func<TermExpr<T_Identifier>, T_Identifier>) (term => term.Identifier));

    internal override bool VisitTrue(TrueExpr<T_Identifier> expression) => true;

    internal override bool VisitFalse(FalseExpr<T_Identifier> expression) => true;

    internal override bool VisitTerm(TermExpr<T_Identifier> expression)
    {
      this._terms.Add(expression);
      return true;
    }

    internal override bool VisitNot(NotExpr<T_Identifier> expression) => expression.Child.Accept<bool>((Visitor<T_Identifier, bool>) this);

    internal override bool VisitAnd(AndExpr<T_Identifier> expression) => this.VisitTree((TreeExpr<T_Identifier>) expression);

    internal override bool VisitOr(OrExpr<T_Identifier> expression) => this.VisitTree((TreeExpr<T_Identifier>) expression);

    private bool VisitTree(TreeExpr<T_Identifier> expression)
    {
      foreach (BoolExpr<T_Identifier> child in expression.Children)
        child.Accept<bool>((Visitor<T_Identifier, bool>) this);
      return true;
    }
  }
}
