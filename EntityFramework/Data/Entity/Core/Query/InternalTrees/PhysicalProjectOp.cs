// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.PhysicalProjectOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class PhysicalProjectOp : PhysicalOp
  {
    internal static readonly PhysicalProjectOp Pattern = new PhysicalProjectOp();
    private readonly SimpleCollectionColumnMap m_columnMap;
    private readonly VarList m_outputVars;

    internal SimpleCollectionColumnMap ColumnMap => this.m_columnMap;

    internal VarList Outputs => this.m_outputVars;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);

    internal PhysicalProjectOp(VarList outputVars, SimpleCollectionColumnMap columnMap)
      : this()
    {
      this.m_outputVars = outputVars;
      this.m_columnMap = columnMap;
    }

    private PhysicalProjectOp()
      : base(OpType.PhysicalProject)
    {
    }
  }
}
