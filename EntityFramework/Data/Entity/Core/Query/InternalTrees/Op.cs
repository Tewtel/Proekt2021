// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.Op
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class Op
  {
    private readonly OpType m_opType;
    internal const int ArityVarying = -1;

    internal Op(OpType opType) => this.m_opType = opType;

    internal OpType OpType => this.m_opType;

    internal virtual int Arity => -1;

    internal virtual bool IsScalarOp => false;

    internal virtual bool IsRulePatternOp => false;

    internal virtual bool IsRelOp => false;

    internal virtual bool IsAncillaryOp => false;

    internal virtual bool IsPhysicalOp => false;

    internal virtual bool IsEquivalent(Op other) => false;

    internal virtual TypeUsage Type
    {
      get => (TypeUsage) null;
      set => throw Error.NotSupported();
    }

    [DebuggerNonUserCode]
    internal virtual void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal virtual TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
