// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ConstantBaseOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class ConstantBaseOp : ScalarOp
  {
    private readonly object m_value;

    protected ConstantBaseOp(OpType opType, TypeUsage type, object value)
      : base(opType, type)
    {
      this.m_value = value;
    }

    protected ConstantBaseOp(OpType opType)
      : base(opType)
    {
    }

    internal virtual object Value => this.m_value;

    internal override int Arity => 0;

    internal override bool IsEquivalent(Op other)
    {
      if (!(other is ConstantBaseOp constantBaseOp) || this.OpType != other.OpType || !constantBaseOp.Type.EdmEquals((MetadataItem) this.Type))
        return false;
      return constantBaseOp.Value == null && this.Value == null || constantBaseOp.Value.Equals(this.Value);
    }
  }
}
