// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Validation.ViewCellSlot
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Validation
{
  internal class ViewCellSlot : ProjectedSlot
  {
    private readonly int m_slotNum;
    private readonly MemberProjectedSlot m_cSlot;
    private readonly MemberProjectedSlot m_sSlot;

    internal ViewCellSlot(int slotNum, MemberProjectedSlot cSlot, MemberProjectedSlot sSlot)
    {
      this.m_slotNum = slotNum;
      this.m_cSlot = cSlot;
      this.m_sSlot = sSlot;
    }

    internal MemberProjectedSlot CSlot => this.m_cSlot;

    internal MemberProjectedSlot SSlot => this.m_sSlot;

    protected override bool IsEqualTo(ProjectedSlot right) => right is ViewCellSlot viewCellSlot && this.m_slotNum == viewCellSlot.m_slotNum && ProjectedSlot.EqualityComparer.Equals((ProjectedSlot) this.m_cSlot, (ProjectedSlot) viewCellSlot.m_cSlot) && ProjectedSlot.EqualityComparer.Equals((ProjectedSlot) this.m_sSlot, (ProjectedSlot) viewCellSlot.m_sSlot);

    protected override int GetHash() => ProjectedSlot.EqualityComparer.GetHashCode((ProjectedSlot) this.m_cSlot) ^ ProjectedSlot.EqualityComparer.GetHashCode((ProjectedSlot) this.m_sSlot) ^ this.m_slotNum;

    internal static string SlotsToUserString(IEnumerable<ViewCellSlot> slots, bool isFromCside)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (ViewCellSlot slot in slots)
      {
        if (!flag)
          stringBuilder.Append(", ");
        stringBuilder.Append(ViewCellSlot.SlotToUserString(slot, isFromCside));
        flag = false;
      }
      return stringBuilder.ToString();
    }

    internal static string SlotToUserString(ViewCellSlot slot, bool isFromCside) => StringUtil.FormatInvariant("{0}", (object) (isFromCside ? slot.CSlot : slot.SSlot));

    internal override string GetCqlFieldAlias(MemberPath outputMember) => (string) null;

    internal override StringBuilder AsEsql(
      StringBuilder builder,
      MemberPath outputMember,
      string blockAlias,
      int indentLevel)
    {
      return (StringBuilder) null;
    }

    internal override DbExpression AsCqt(DbExpression row, MemberPath outputMember) => (DbExpression) null;

    internal override void ToCompactString(StringBuilder builder)
    {
      builder.Append('<');
      StringUtil.FormatStringBuilder(builder, "{0}", (object) this.m_slotNum);
      builder.Append(':');
      this.m_cSlot.ToCompactString(builder);
      builder.Append('-');
      this.m_sSlot.ToCompactString(builder);
      builder.Append('>');
    }
  }
}
