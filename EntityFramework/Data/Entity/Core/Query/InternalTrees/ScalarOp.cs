// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ScalarOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class ScalarOp : Op
  {
    private TypeUsage m_type;

    internal ScalarOp(OpType opType, TypeUsage type)
      : this(opType)
    {
      this.m_type = type;
    }

    protected ScalarOp(OpType opType)
      : base(opType)
    {
    }

    internal override bool IsScalarOp => true;

    internal override bool IsEquivalent(Op other) => other.OpType == this.OpType && TypeSemantics.IsStructurallyEqual(this.Type, other.Type);

    internal override TypeUsage Type
    {
      get => this.m_type;
      set => this.m_type = value;
    }

    internal virtual bool IsAggregateOp => false;
  }
}
