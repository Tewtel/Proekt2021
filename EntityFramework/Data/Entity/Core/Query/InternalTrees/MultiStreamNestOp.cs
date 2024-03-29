﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.MultiStreamNestOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class MultiStreamNestOp : NestBaseOp
  {
    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);

    internal MultiStreamNestOp(
      List<SortKey> prefixSortKeys,
      VarVec outputVars,
      List<CollectionInfo> collectionInfoList)
      : base(OpType.MultiStreamNest, prefixSortKeys, outputVars, collectionInfoList)
    {
    }
  }
}
