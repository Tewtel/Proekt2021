﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.VarRefOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class VarRefOp : ScalarOp
  {
    private readonly Var m_var;
    internal static readonly VarRefOp Pattern = new VarRefOp();

    internal VarRefOp(Var v)
      : base(OpType.VarRef, v.Type)
    {
      this.m_var = v;
    }

    private VarRefOp()
      : base(OpType.VarRef)
    {
    }

    internal override int Arity => 0;

    internal override bool IsEquivalent(Op other) => other is VarRefOp varRefOp && varRefOp.Var.Equals((object) this.Var);

    internal Var Var => this.m_var;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
