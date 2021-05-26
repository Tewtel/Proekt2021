// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ConstantPredicateOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class ConstantPredicateOp : ConstantBaseOp
  {
    internal static readonly ConstantPredicateOp Pattern = new ConstantPredicateOp();

    internal ConstantPredicateOp(TypeUsage type, bool value)
      : base(OpType.ConstantPredicate, type, (object) value)
    {
    }

    private ConstantPredicateOp()
      : base(OpType.ConstantPredicate)
    {
    }

    internal bool Value => (bool) base.Value;

    internal bool IsTrue => this.Value;

    internal bool IsFalse => !this.Value;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
