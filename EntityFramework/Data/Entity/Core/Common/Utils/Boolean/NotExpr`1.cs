// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.NotExpr`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal sealed class NotExpr<T_Identifier> : TreeExpr<T_Identifier>
  {
    internal NotExpr(BoolExpr<T_Identifier> child)
      : base((IEnumerable<BoolExpr<T_Identifier>>) new BoolExpr<T_Identifier>[1]
      {
        child
      })
    {
    }

    internal override ExprType ExprType => ExprType.Not;

    internal BoolExpr<T_Identifier> Child => this.Children.First<BoolExpr<T_Identifier>>();

    internal override T_Return Accept<T_Return>(Visitor<T_Identifier, T_Return> visitor) => visitor.VisitNot(this);

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "!{0}", (object) this.Child);

    internal override BoolExpr<T_Identifier> MakeNegated() => this.Child;
  }
}
