// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.FalseExpr`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal sealed class FalseExpr<T_Identifier> : BoolExpr<T_Identifier>
  {
    private static readonly FalseExpr<T_Identifier> _value = new FalseExpr<T_Identifier>();

    private FalseExpr()
    {
    }

    internal static FalseExpr<T_Identifier> Value => FalseExpr<T_Identifier>._value;

    internal override ExprType ExprType => ExprType.False;

    internal override T_Return Accept<T_Return>(Visitor<T_Identifier, T_Return> visitor) => visitor.VisitFalse(this);

    internal override BoolExpr<T_Identifier> MakeNegated() => (BoolExpr<T_Identifier>) TrueExpr<T_Identifier>.Value;

    protected override bool EquivalentTypeEquals(BoolExpr<T_Identifier> other) => this == other;
  }
}
