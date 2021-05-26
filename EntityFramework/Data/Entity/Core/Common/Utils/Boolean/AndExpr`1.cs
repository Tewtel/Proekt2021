// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.AndExpr`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal class AndExpr<T_Identifier> : TreeExpr<T_Identifier>
  {
    internal AndExpr(params BoolExpr<T_Identifier>[] children)
      : this((IEnumerable<BoolExpr<T_Identifier>>) children)
    {
    }

    internal AndExpr(IEnumerable<BoolExpr<T_Identifier>> children)
      : base(children)
    {
    }

    internal override ExprType ExprType => ExprType.And;

    internal override T_Return Accept<T_Return>(Visitor<T_Identifier, T_Return> visitor) => visitor.VisitAnd(this);
  }
}
