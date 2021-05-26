﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.CnfClause`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal sealed class CnfClause<T_Identifier> : 
    Clause<T_Identifier>,
    IEquatable<CnfClause<T_Identifier>>
  {
    internal CnfClause(Set<Literal<T_Identifier>> literals)
      : base(literals, ExprType.Or)
    {
    }

    public bool Equals(CnfClause<T_Identifier> other) => other != null && other.Literals.SetEquals(this.Literals);
  }
}
