// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.TermExpr`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal sealed class TermExpr<T_Identifier> : 
    BoolExpr<T_Identifier>,
    IEquatable<TermExpr<T_Identifier>>
  {
    private readonly T_Identifier _identifier;
    private readonly IEqualityComparer<T_Identifier> _comparer;

    internal TermExpr(IEqualityComparer<T_Identifier> comparer, T_Identifier identifier)
    {
      this._identifier = identifier;
      if (comparer == null)
        this._comparer = (IEqualityComparer<T_Identifier>) EqualityComparer<T_Identifier>.Default;
      else
        this._comparer = comparer;
    }

    internal TermExpr(T_Identifier identifier)
      : this((IEqualityComparer<T_Identifier>) null, identifier)
    {
    }

    internal T_Identifier Identifier => this._identifier;

    internal override ExprType ExprType => ExprType.Term;

    public override bool Equals(object obj) => this.Equals(obj as TermExpr<T_Identifier>);

    public bool Equals(TermExpr<T_Identifier> other) => this._comparer.Equals(this._identifier, other._identifier);

    protected override bool EquivalentTypeEquals(BoolExpr<T_Identifier> other) => this._comparer.Equals(this._identifier, ((TermExpr<T_Identifier>) other)._identifier);

    public override int GetHashCode() => this._comparer.GetHashCode(this._identifier);

    public override string ToString() => StringUtil.FormatInvariant("{0}", (object) this._identifier);

    internal override T_Return Accept<T_Return>(Visitor<T_Identifier, T_Return> visitor) => visitor.VisitTerm(this);

    internal override BoolExpr<T_Identifier> MakeNegated()
    {
      Literal<T_Identifier> literal = new Literal<T_Identifier>(this, true).MakeNegated();
      return literal.IsTermPositive ? (BoolExpr<T_Identifier>) literal.Term : (BoolExpr<T_Identifier>) new NotExpr<T_Identifier>((BoolExpr<T_Identifier>) literal.Term);
    }
  }
}
