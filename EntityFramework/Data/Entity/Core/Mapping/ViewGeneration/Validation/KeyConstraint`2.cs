// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Validation.KeyConstraint`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Validation
{
  internal class KeyConstraint<TCellRelation, TSlot> : InternalBase where TCellRelation : System.Data.Entity.Core.Mapping.ViewGeneration.Validation.CellRelation
  {
    private readonly TCellRelation m_relation;
    private readonly Set<TSlot> m_keySlots;

    internal KeyConstraint(
      TCellRelation relation,
      IEnumerable<TSlot> keySlots,
      IEqualityComparer<TSlot> comparer)
    {
      this.m_relation = relation;
      this.m_keySlots = new Set<TSlot>(keySlots, comparer).MakeReadOnly();
    }

    protected TCellRelation CellRelation => this.m_relation;

    protected Set<TSlot> KeySlots => this.m_keySlots;

    internal override void ToCompactString(StringBuilder builder)
    {
      StringUtil.FormatStringBuilder(builder, "Key (V{0}) - ", (object) this.m_relation.CellNumber);
      StringUtil.ToSeparatedStringSorted(builder, (IEnumerable) this.KeySlots, ", ");
    }
  }
}
