// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.ToDecisionDiagramConverter`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Linq;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal class ToDecisionDiagramConverter<T_Identifier> : Visitor<T_Identifier, Vertex>
  {
    private readonly ConversionContext<T_Identifier> _context;

    private ToDecisionDiagramConverter(ConversionContext<T_Identifier> context) => this._context = context;

    internal static Vertex TranslateToRobdd(
      BoolExpr<T_Identifier> expr,
      ConversionContext<T_Identifier> context)
    {
      ToDecisionDiagramConverter<T_Identifier> diagramConverter = new ToDecisionDiagramConverter<T_Identifier>(context);
      return expr.Accept<Vertex>((Visitor<T_Identifier, Vertex>) diagramConverter);
    }

    internal override Vertex VisitTrue(TrueExpr<T_Identifier> expression) => Vertex.One;

    internal override Vertex VisitFalse(FalseExpr<T_Identifier> expression) => Vertex.Zero;

    internal override Vertex VisitTerm(TermExpr<T_Identifier> expression) => this._context.TranslateTermToVertex(expression);

    internal override Vertex VisitNot(NotExpr<T_Identifier> expression) => this._context.Solver.Not(expression.Child.Accept<Vertex>((Visitor<T_Identifier, Vertex>) this));

    internal override Vertex VisitAnd(AndExpr<T_Identifier> expression) => this._context.Solver.And(expression.Children.Select<BoolExpr<T_Identifier>, Vertex>((Func<BoolExpr<T_Identifier>, Vertex>) (child => child.Accept<Vertex>((Visitor<T_Identifier, Vertex>) this))));

    internal override Vertex VisitOr(OrExpr<T_Identifier> expression) => this._context.Solver.Or(expression.Children.Select<BoolExpr<T_Identifier>, Vertex>((Func<BoolExpr<T_Identifier>, Vertex>) (child => child.Accept<Vertex>((Visitor<T_Identifier, Vertex>) this))));
  }
}
