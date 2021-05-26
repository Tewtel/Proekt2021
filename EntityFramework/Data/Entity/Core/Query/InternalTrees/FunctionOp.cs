// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.FunctionOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class FunctionOp : ScalarOp
  {
    private readonly EdmFunction m_function;
    internal static readonly FunctionOp Pattern = new FunctionOp();

    internal FunctionOp(EdmFunction function)
      : base(OpType.Function, function.ReturnParameter.TypeUsage)
    {
      this.m_function = function;
    }

    private FunctionOp()
      : base(OpType.Function)
    {
    }

    internal EdmFunction Function => this.m_function;

    internal override bool IsEquivalent(Op other) => other is FunctionOp functionOp && functionOp.Function.EdmEquals((MetadataItem) this.Function);

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
