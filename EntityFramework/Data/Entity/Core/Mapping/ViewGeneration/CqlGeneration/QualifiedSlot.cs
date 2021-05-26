// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration.QualifiedSlot
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration
{
  internal sealed class QualifiedSlot : ProjectedSlot
  {
    private readonly CqlBlock m_block;
    private readonly ProjectedSlot m_slot;

    internal QualifiedSlot(CqlBlock block, ProjectedSlot slot)
    {
      this.m_block = block;
      this.m_slot = slot;
    }

    internal override ProjectedSlot DeepQualify(CqlBlock block) => (ProjectedSlot) new QualifiedSlot(block, this.m_slot);

    internal override string GetCqlFieldAlias(MemberPath outputMember) => this.GetOriginalSlot().GetCqlFieldAlias(outputMember);

    internal ProjectedSlot GetOriginalSlot()
    {
      ProjectedSlot slot = this.m_slot;
      while (slot is QualifiedSlot qualifiedSlot)
        slot = qualifiedSlot.m_slot;
      return slot;
    }

    internal string GetQualifiedCqlName(MemberPath outputMember) => CqlWriter.GetQualifiedName(this.m_block.CqlAlias, this.GetCqlFieldAlias(outputMember));

    internal override StringBuilder AsEsql(
      StringBuilder builder,
      MemberPath outputMember,
      string blockAlias,
      int indentLevel)
    {
      builder.Append(this.GetQualifiedCqlName(outputMember));
      return builder;
    }

    internal override DbExpression AsCqt(DbExpression row, MemberPath outputMember) => (DbExpression) this.m_block.GetInput(row).Property(this.GetCqlFieldAlias(outputMember));

    internal override void ToCompactString(StringBuilder builder)
    {
      StringUtil.FormatStringBuilder(builder, "{0} ", (object) this.m_block.CqlAlias);
      this.m_slot.ToCompactString(builder);
    }
  }
}
