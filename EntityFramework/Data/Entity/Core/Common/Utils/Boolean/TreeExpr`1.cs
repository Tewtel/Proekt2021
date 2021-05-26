// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.TreeExpr`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal abstract class TreeExpr<T_Identifier> : BoolExpr<T_Identifier>
  {
    private readonly Set<BoolExpr<T_Identifier>> _children;
    private readonly int _hashCode;

    protected TreeExpr(IEnumerable<BoolExpr<T_Identifier>> children)
    {
      this._children = new Set<BoolExpr<T_Identifier>>(children);
      this._children.MakeReadOnly();
      this._hashCode = this._children.GetElementsHashCode();
    }

    internal Set<BoolExpr<T_Identifier>> Children => this._children;

    public override bool Equals(object obj) => this.Equals(obj as BoolExpr<T_Identifier>);

    public override int GetHashCode() => this._hashCode;

    public override string ToString() => StringUtil.FormatInvariant("{0}({1})", (object) this.ExprType, (object) this._children);

    protected override bool EquivalentTypeEquals(BoolExpr<T_Identifier> other) => ((TreeExpr<T_Identifier>) other).Children.SetEquals(this.Children);
  }
}
