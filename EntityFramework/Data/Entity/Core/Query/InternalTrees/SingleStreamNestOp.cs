// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.SingleStreamNestOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class SingleStreamNestOp : NestBaseOp
  {
    private readonly VarVec m_keys;
    private readonly Var m_discriminator;
    private readonly List<SortKey> m_postfixSortKeys;

    internal override int Arity => 1;

    internal Var Discriminator => this.m_discriminator;

    internal List<SortKey> PostfixSortKeys => this.m_postfixSortKeys;

    internal VarVec Keys => this.m_keys;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);

    internal SingleStreamNestOp(
      VarVec keys,
      List<SortKey> prefixSortKeys,
      List<SortKey> postfixSortKeys,
      VarVec outputVars,
      List<CollectionInfo> collectionInfoList,
      Var discriminatorVar)
      : base(OpType.SingleStreamNest, prefixSortKeys, outputVars, collectionInfoList)
    {
      this.m_keys = keys;
      this.m_postfixSortKeys = postfixSortKeys;
      this.m_discriminator = discriminatorVar;
    }
  }
}
