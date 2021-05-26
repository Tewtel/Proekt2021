// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ComparisonOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class ComparisonOp : ScalarOp
  {
    internal static readonly ComparisonOp PatternEq = new ComparisonOp(OpType.EQ);

    internal ComparisonOp(OpType opType, TypeUsage type)
      : base(opType, type)
    {
    }

    private ComparisonOp(OpType opType)
      : base(opType)
    {
    }

    internal override int Arity => 2;

    internal bool UseDatabaseNullSemantics { get; set; }

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
