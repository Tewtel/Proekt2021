﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ConditionalOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class ConditionalOp : ScalarOp
  {
    internal static readonly ConditionalOp PatternAnd = new ConditionalOp(OpType.And);
    internal static readonly ConditionalOp PatternOr = new ConditionalOp(OpType.Or);
    internal static readonly ConditionalOp PatternIn = new ConditionalOp(OpType.In);
    internal static readonly ConditionalOp PatternNot = new ConditionalOp(OpType.Not);
    internal static readonly ConditionalOp PatternIsNull = new ConditionalOp(OpType.IsNull);

    internal ConditionalOp(OpType optype, TypeUsage type)
      : base(optype, type)
    {
    }

    private ConditionalOp(OpType opType)
      : base(opType)
    {
    }

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
