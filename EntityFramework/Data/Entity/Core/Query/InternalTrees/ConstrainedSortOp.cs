// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ConstrainedSortOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class ConstrainedSortOp : SortBaseOp
  {
    internal static readonly ConstrainedSortOp Pattern = new ConstrainedSortOp();

    private ConstrainedSortOp()
      : base(OpType.ConstrainedSort)
    {
    }

    internal ConstrainedSortOp(List<SortKey> sortKeys, bool withTies)
      : base(OpType.ConstrainedSort, sortKeys)
    {
      this.WithTies = withTies;
    }

    internal bool WithTies { get; set; }

    internal override int Arity => 3;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
