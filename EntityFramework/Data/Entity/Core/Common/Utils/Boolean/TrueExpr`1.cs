// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.TrueExpr`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal sealed class TrueExpr<T_Identifier> : BoolExpr<T_Identifier>
  {
    private static readonly TrueExpr<T_Identifier> _value = new TrueExpr<T_Identifier>();

    private TrueExpr()
    {
    }

    internal static TrueExpr<T_Identifier> Value => TrueExpr<T_Identifier>._value;

    internal override ExprType ExprType => ExprType.True;

    internal override T_Return Accept<T_Return>(Visitor<T_Identifier, T_Return> visitor) => visitor.VisitTrue(this);

    internal override BoolExpr<T_Identifier> MakeNegated() => (BoolExpr<T_Identifier>) FalseExpr<T_Identifier>.Value;

    protected override bool EquivalentTypeEquals(BoolExpr<T_Identifier> other) => this == other;
  }
}
