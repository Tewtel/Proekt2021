// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.TreatOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class TreatOp : ScalarOp
  {
    private readonly bool m_isFake;
    internal static readonly TreatOp Pattern = new TreatOp();

    internal TreatOp(TypeUsage type, bool isFake)
      : base(OpType.Treat, type)
    {
      this.m_isFake = isFake;
    }

    private TreatOp()
      : base(OpType.Treat)
    {
    }

    internal override int Arity => 1;

    internal bool IsFakeTreat => this.m_isFake;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
