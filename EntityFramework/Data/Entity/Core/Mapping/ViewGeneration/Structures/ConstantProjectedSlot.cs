// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.ConstantProjectedSlot
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal sealed class ConstantProjectedSlot : ProjectedSlot
  {
    private readonly Constant m_constant;

    internal ConstantProjectedSlot(Constant value) => this.m_constant = value;

    internal Constant CellConstant => this.m_constant;

    internal override ProjectedSlot DeepQualify(CqlBlock block) => (ProjectedSlot) this;

    internal override StringBuilder AsEsql(
      StringBuilder builder,
      MemberPath outputMember,
      string blockAlias,
      int indentLevel)
    {
      return this.m_constant.AsEsql(builder, outputMember, blockAlias);
    }

    internal override DbExpression AsCqt(DbExpression row, MemberPath outputMember) => this.m_constant.AsCqt(row, outputMember);

    protected override bool IsEqualTo(ProjectedSlot right) => right is ConstantProjectedSlot constantProjectedSlot && Constant.EqualityComparer.Equals(this.m_constant, constantProjectedSlot.m_constant);

    protected override int GetHash() => Constant.EqualityComparer.GetHashCode(this.m_constant);

    internal override void ToCompactString(StringBuilder builder) => this.m_constant.ToCompactString(builder);
  }
}
