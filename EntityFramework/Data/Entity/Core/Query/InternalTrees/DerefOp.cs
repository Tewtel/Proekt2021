// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.DerefOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class DerefOp : ScalarOp
  {
    internal static readonly DerefOp Pattern = new DerefOp();

    internal DerefOp(TypeUsage type)
      : base(OpType.Deref, type)
    {
    }

    private DerefOp()
      : base(OpType.Deref)
    {
    }

    internal override int Arity => 1;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
