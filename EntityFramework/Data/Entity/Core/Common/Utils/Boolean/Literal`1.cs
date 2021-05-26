// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.Literal`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal sealed class Literal<T_Identifier> : 
    NormalFormNode<T_Identifier>,
    IEquatable<Literal<T_Identifier>>
  {
    private readonly TermExpr<T_Identifier> _term;
    private readonly bool _isTermPositive;

    internal Literal(TermExpr<T_Identifier> term, bool isTermPositive)
      : base(isTermPositive ? (BoolExpr<T_Identifier>) term : (BoolExpr<T_Identifier>) new NotExpr<T_Identifier>((BoolExpr<T_Identifier>) term))
    {
      this._term = term;
      this._isTermPositive = isTermPositive;
    }

    internal TermExpr<T_Identifier> Term => this._term;

    internal bool IsTermPositive => this._isTermPositive;

    internal Literal<T_Identifier> MakeNegated() => IdentifierService<T_Identifier>.Instance.NegateLiteral(this);

    public override string ToString() => StringUtil.FormatInvariant("{0}{1}", this._isTermPositive ? (object) string.Empty : (object) "!", (object) this._term);

    public override bool Equals(object obj) => this.Equals(obj as Literal<T_Identifier>);

    public bool Equals(Literal<T_Identifier> other) => other != null && other._isTermPositive == this._isTermPositive && other._term.Equals(this._term);

    public override int GetHashCode() => this._term.GetHashCode();
  }
}
