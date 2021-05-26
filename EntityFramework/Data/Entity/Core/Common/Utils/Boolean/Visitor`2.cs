// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.Visitor`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal abstract class Visitor<T_Identifier, T_Return>
  {
    internal abstract T_Return VisitTrue(TrueExpr<T_Identifier> expression);

    internal abstract T_Return VisitFalse(FalseExpr<T_Identifier> expression);

    internal abstract T_Return VisitTerm(TermExpr<T_Identifier> expression);

    internal abstract T_Return VisitNot(NotExpr<T_Identifier> expression);

    internal abstract T_Return VisitAnd(AndExpr<T_Identifier> expression);

    internal abstract T_Return VisitOr(OrExpr<T_Identifier> expression);
  }
}
