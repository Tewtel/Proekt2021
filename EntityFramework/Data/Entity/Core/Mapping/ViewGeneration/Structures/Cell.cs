﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.Cell
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.Validation;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal class Cell : InternalBase
  {
    private readonly CellQuery m_cQuery;
    private readonly CellQuery m_sQuery;
    private readonly int m_cellNumber;
    private readonly CellLabel m_label;
    private ViewCellRelation m_viewCellRelation;

    private Cell(CellQuery cQuery, CellQuery sQuery, CellLabel label, int cellNumber)
    {
      this.m_cQuery = cQuery;
      this.m_sQuery = sQuery;
      this.m_label = label;
      this.m_cellNumber = cellNumber;
    }

    internal Cell(Cell source)
    {
      this.m_cQuery = new CellQuery(source.m_cQuery);
      this.m_sQuery = new CellQuery(source.m_sQuery);
      this.m_label = new CellLabel(source.m_label);
      this.m_cellNumber = source.m_cellNumber;
    }

    internal CellQuery CQuery => this.m_cQuery;

    internal CellQuery SQuery => this.m_sQuery;

    internal CellLabel CellLabel => this.m_label;

    internal int CellNumber => this.m_cellNumber;

    internal string CellNumberAsString => StringUtil.FormatInvariant("V{0}", (object) this.CellNumber);

    internal void GetIdentifiers(CqlIdentifiers identifiers)
    {
      this.m_cQuery.GetIdentifiers(identifiers);
      this.m_sQuery.GetIdentifiers(identifiers);
    }

    internal Set<EdmProperty> GetCSlotsForTableColumns(IEnumerable<MemberPath> columns)
    {
      List<int> projectedPositions = this.SQuery.GetProjectedPositions(columns);
      if (projectedPositions == null)
        return (Set<EdmProperty>) null;
      Set<EdmProperty> set = new Set<EdmProperty>();
      foreach (int slotNum in projectedPositions)
      {
        if (!(this.CQuery.ProjectedSlotAt(slotNum) is MemberProjectedSlot memberProjectedSlot2))
          return (Set<EdmProperty>) null;
        set.Add((EdmProperty) memberProjectedSlot2.MemberPath.LeafEdmMember);
      }
      return set;
    }

    internal CellQuery GetLeftQuery(ViewTarget side) => side != ViewTarget.QueryView ? this.m_sQuery : this.m_cQuery;

    internal CellQuery GetRightQuery(ViewTarget side) => side != ViewTarget.QueryView ? this.m_cQuery : this.m_sQuery;

    internal ViewCellRelation CreateViewCellRelation(int cellNumber)
    {
      if (this.m_viewCellRelation != null)
        return this.m_viewCellRelation;
      this.GenerateCellRelations(cellNumber);
      return this.m_viewCellRelation;
    }

    private void GenerateCellRelations(int cellNumber)
    {
      List<ViewCellSlot> slots = new List<ViewCellSlot>();
      for (int slotNum = 0; slotNum < this.CQuery.NumProjectedSlots; ++slotNum)
      {
        ProjectedSlot projectedSlot1 = this.CQuery.ProjectedSlotAt(slotNum);
        ProjectedSlot projectedSlot2 = this.SQuery.ProjectedSlotAt(slotNum);
        MemberProjectedSlot cSlot = (MemberProjectedSlot) projectedSlot1;
        MemberProjectedSlot sSlot = (MemberProjectedSlot) projectedSlot2;
        ViewCellSlot viewCellSlot = new ViewCellSlot(slotNum, cSlot, sSlot);
        slots.Add(viewCellSlot);
      }
      this.m_viewCellRelation = new ViewCellRelation(this, slots, cellNumber);
    }

    internal override void ToCompactString(StringBuilder builder)
    {
      this.CQuery.ToCompactString(builder);
      builder.Append(" = ");
      this.SQuery.ToCompactString(builder);
    }

    internal override void ToFullString(StringBuilder builder)
    {
      this.CQuery.ToFullString(builder);
      builder.Append(" = ");
      this.SQuery.ToFullString(builder);
    }

    public override string ToString() => this.ToFullString();

    internal static void CellsToBuilder(StringBuilder builder, IEnumerable<Cell> cells)
    {
      builder.AppendLine();
      builder.AppendLine("=========================================================================");
      foreach (Cell cell in cells)
      {
        builder.AppendLine();
        StringUtil.FormatStringBuilder(builder, "Mapping Cell V{0}:", (object) cell.CellNumber);
        builder.AppendLine();
        builder.Append("C: ");
        cell.CQuery.ToFullString(builder);
        builder.AppendLine();
        builder.AppendLine();
        builder.Append("S: ");
        cell.SQuery.ToFullString(builder);
        builder.AppendLine();
      }
    }

    internal static Cell CreateCS(
      CellQuery cQuery,
      CellQuery sQuery,
      CellLabel label,
      int cellNumber)
    {
      return new Cell(cQuery, sQuery, label, cellNumber);
    }
  }
}
