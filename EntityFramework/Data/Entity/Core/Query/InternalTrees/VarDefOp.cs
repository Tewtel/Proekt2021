// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.VarDefOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class VarDefOp : AncillaryOp
  {
    private readonly Var m_var;
    internal static readonly VarDefOp Pattern = new VarDefOp();

    internal VarDefOp(Var v)
      : this()
    {
      this.m_var = v;
    }

    private VarDefOp()
      : base(OpType.VarDef)
    {
    }

    internal override int Arity => 1;

    internal Var Var => this.m_var;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
