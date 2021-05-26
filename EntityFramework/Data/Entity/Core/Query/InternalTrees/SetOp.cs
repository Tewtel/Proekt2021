// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.SetOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class SetOp : RelOp
  {
    private readonly System.Data.Entity.Core.Query.InternalTrees.VarMap[] m_varMap;
    private readonly VarVec m_outputVars;

    internal SetOp(OpType opType, VarVec outputs, System.Data.Entity.Core.Query.InternalTrees.VarMap left, System.Data.Entity.Core.Query.InternalTrees.VarMap right)
      : this(opType)
    {
      this.m_varMap = new System.Data.Entity.Core.Query.InternalTrees.VarMap[2];
      this.m_varMap[0] = left;
      this.m_varMap[1] = right;
      this.m_outputVars = outputs;
    }

    protected SetOp(OpType opType)
      : base(opType)
    {
    }

    internal override int Arity => 2;

    internal System.Data.Entity.Core.Query.InternalTrees.VarMap[] VarMap => this.m_varMap;

    internal VarVec Outputs => this.m_outputVars;
  }
}
