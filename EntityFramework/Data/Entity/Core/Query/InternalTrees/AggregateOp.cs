// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.AggregateOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class AggregateOp : ScalarOp
  {
    private readonly EdmFunction m_aggFunc;
    private readonly bool m_distinctAgg;
    internal static readonly AggregateOp Pattern = new AggregateOp();

    internal AggregateOp(EdmFunction aggFunc, bool distinctAgg)
      : base(OpType.Aggregate, aggFunc.ReturnParameter.TypeUsage)
    {
      this.m_aggFunc = aggFunc;
      this.m_distinctAgg = distinctAgg;
    }

    private AggregateOp()
      : base(OpType.Aggregate)
    {
    }

    internal EdmFunction AggFunc => this.m_aggFunc;

    internal bool IsDistinctAggregate => this.m_distinctAgg;

    internal override bool IsAggregateOp => true;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
