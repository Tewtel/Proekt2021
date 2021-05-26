// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Validation.BasicKeyConstraint
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Validation
{
  internal class BasicKeyConstraint : KeyConstraint<BasicCellRelation, MemberProjectedSlot>
  {
    internal BasicKeyConstraint(
      BasicCellRelation relation,
      IEnumerable<MemberProjectedSlot> keySlots)
      : base(relation, keySlots, (IEqualityComparer<MemberProjectedSlot>) ProjectedSlot.EqualityComparer)
    {
    }

    internal ViewKeyConstraint Propagate()
    {
      ViewCellRelation viewCellRelation = this.CellRelation.ViewCellRelation;
      List<ViewCellSlot> viewCellSlotList = new List<ViewCellSlot>();
      foreach (MemberProjectedSlot keySlot in this.KeySlots)
      {
        ViewCellSlot viewCellSlot = viewCellRelation.LookupViewSlot(keySlot);
        if (viewCellSlot == null)
          return (ViewKeyConstraint) null;
        viewCellSlotList.Add(viewCellSlot);
      }
      return new ViewKeyConstraint(viewCellRelation, (IEnumerable<ViewCellSlot>) viewCellSlotList);
    }
  }
}
