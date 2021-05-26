// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.CellIdBoolean
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal class CellIdBoolean : TrueFalseLiteral
  {
    private readonly int m_index;
    private readonly string m_slotName;

    internal CellIdBoolean(CqlIdentifiers identifiers, int index)
    {
      this.m_index = index;
      this.m_slotName = identifiers.GetFromVariable(index);
    }

    internal string SlotName => this.m_slotName;

    internal override StringBuilder AsEsql(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull)
    {
      string qualifiedName = CqlWriter.GetQualifiedName(blockAlias, this.SlotName);
      builder.Append(qualifiedName);
      return builder;
    }

    internal override DbExpression AsCqt(DbExpression row, bool skipIsNotNull) => (DbExpression) row.Property(this.SlotName);

    internal override StringBuilder AsUserString(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull)
    {
      return this.AsEsql(builder, blockAlias, skipIsNotNull);
    }

    internal override StringBuilder AsNegatedUserString(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull)
    {
      builder.Append("NOT(");
      builder = this.AsUserString(builder, blockAlias, skipIsNotNull);
      builder.Append(")");
      return builder;
    }

    internal override void GetRequiredSlots(
      MemberProjectionIndex projectedSlotMap,
      bool[] requiredSlots)
    {
      int numBoolSlots = requiredSlots.Length - projectedSlotMap.Count;
      int slot = projectedSlotMap.BoolIndexToSlot(this.m_index, numBoolSlots);
      requiredSlots[slot] = true;
    }

    protected override bool IsEqualTo(BoolLiteral right) => right is CellIdBoolean cellIdBoolean && this.m_index == cellIdBoolean.m_index;

    public override int GetHashCode() => this.m_index.GetHashCode();

    internal override BoolLiteral RemapBool(Dictionary<MemberPath, MemberPath> remap) => (BoolLiteral) this;

    internal override void ToCompactString(StringBuilder builder) => builder.Append(this.SlotName);
  }
}
