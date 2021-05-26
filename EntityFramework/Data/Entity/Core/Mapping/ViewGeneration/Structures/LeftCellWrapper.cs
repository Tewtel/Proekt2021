﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.LeftCellWrapper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal class LeftCellWrapper : InternalBase
  {
    internal static readonly IEqualityComparer<LeftCellWrapper> BoolEqualityComparer = (IEqualityComparer<LeftCellWrapper>) new LeftCellWrapper.BoolWrapperComparer();
    private readonly Set<MemberPath> m_attributes;
    private readonly MemberMaps m_memberMaps;
    private readonly CellQuery m_leftCellQuery;
    private readonly CellQuery m_rightCellQuery;
    private readonly HashSet<Cell> m_mergedCells;
    private readonly ViewTarget m_viewTarget;
    private readonly FragmentQuery m_leftFragmentQuery;
    internal static readonly IComparer<LeftCellWrapper> Comparer = (IComparer<LeftCellWrapper>) new LeftCellWrapper.LeftCellWrapperComparer();
    internal static readonly IComparer<LeftCellWrapper> OriginalCellIdComparer = (IComparer<LeftCellWrapper>) new LeftCellWrapper.CellIdComparer();

    internal LeftCellWrapper(
      ViewTarget viewTarget,
      Set<MemberPath> attrs,
      FragmentQuery fragmentQuery,
      CellQuery leftCellQuery,
      CellQuery rightCellQuery,
      MemberMaps memberMaps,
      IEnumerable<Cell> inputCells)
    {
      this.m_leftFragmentQuery = fragmentQuery;
      this.m_rightCellQuery = rightCellQuery;
      this.m_leftCellQuery = leftCellQuery;
      this.m_attributes = attrs;
      this.m_viewTarget = viewTarget;
      this.m_memberMaps = memberMaps;
      this.m_mergedCells = new HashSet<Cell>(inputCells);
    }

    internal LeftCellWrapper(
      ViewTarget viewTarget,
      Set<MemberPath> attrs,
      FragmentQuery fragmentQuery,
      CellQuery leftCellQuery,
      CellQuery rightCellQuery,
      MemberMaps memberMaps,
      Cell inputCell)
      : this(viewTarget, attrs, fragmentQuery, leftCellQuery, rightCellQuery, memberMaps, Enumerable.Repeat<Cell>(inputCell, 1))
    {
    }

    internal FragmentQuery FragmentQuery => this.m_leftFragmentQuery;

    internal Set<MemberPath> Attributes => this.m_attributes;

    internal string OriginalCellNumberString => StringUtil.ToSeparatedString((IEnumerable) this.m_mergedCells.Select<Cell, string>((Func<Cell, string>) (cell => cell.CellNumberAsString)), "+", "");

    internal MemberDomainMap RightDomainMap => this.m_memberMaps.RightDomainMap;

    [Conditional("DEBUG")]
    internal void AssertHasUniqueCell()
    {
    }

    internal IEnumerable<Cell> Cells => (IEnumerable<Cell>) this.m_mergedCells;

    internal Cell OnlyInputCell => this.m_mergedCells.First<Cell>();

    internal CellQuery RightCellQuery => this.m_rightCellQuery;

    internal CellQuery LeftCellQuery => this.m_leftCellQuery;

    internal EntitySetBase LeftExtent => this.m_mergedCells.First<Cell>().GetLeftQuery(this.m_viewTarget).Extent;

    internal EntitySetBase RightExtent => this.m_rightCellQuery.Extent;

    internal static IEnumerable<Cell> GetInputCellsForWrappers(
      IEnumerable<LeftCellWrapper> wrappers)
    {
      foreach (LeftCellWrapper wrapper in wrappers)
      {
        foreach (Cell mergedCell in wrapper.m_mergedCells)
          yield return mergedCell;
      }
    }

    internal RoleBoolean CreateRoleBoolean()
    {
      if (this.RightExtent is AssociationSet)
      {
        Set<AssociationEndMember> forTablePrimaryKey = this.GetEndsForTablePrimaryKey();
        if (forTablePrimaryKey.Count == 1)
          return new RoleBoolean(((AssociationSet) this.RightExtent).AssociationSetEnds[forTablePrimaryKey.First<AssociationEndMember>().Name]);
      }
      return new RoleBoolean(this.RightExtent);
    }

    internal static string GetExtentListAsUserString(IEnumerable<LeftCellWrapper> wrappers)
    {
      Set<EntitySetBase> set = new Set<EntitySetBase>((IEqualityComparer<EntitySetBase>) EqualityComparer<EntitySetBase>.Default);
      foreach (LeftCellWrapper wrapper in wrappers)
        set.Add(wrapper.RightExtent);
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (EntitySetBase entitySetBase in set)
      {
        if (!flag)
          stringBuilder.Append(", ");
        flag = false;
        stringBuilder.Append(entitySetBase.Name);
      }
      return stringBuilder.ToString();
    }

    internal override void ToFullString(StringBuilder builder)
    {
      builder.Append("P[");
      StringUtil.ToSeparatedString(builder, (IEnumerable) this.m_attributes, ",");
      builder.Append("] = ");
      this.m_rightCellQuery.ToFullString(builder);
    }

    internal override void ToCompactString(StringBuilder stringBuilder) => stringBuilder.Append(this.OriginalCellNumberString);

    internal static void WrappersToStringBuilder(
      StringBuilder builder,
      List<LeftCellWrapper> wrappers,
      string header)
    {
      builder.AppendLine().Append(header).AppendLine();
      LeftCellWrapper[] array = wrappers.ToArray();
      Array.Sort<LeftCellWrapper>(array, LeftCellWrapper.OriginalCellIdComparer);
      foreach (LeftCellWrapper leftCellWrapper in array)
      {
        leftCellWrapper.ToCompactString(builder);
        builder.Append(" = ");
        leftCellWrapper.ToFullString(builder);
        builder.AppendLine();
      }
    }

    private Set<AssociationEndMember> GetEndsForTablePrimaryKey()
    {
      CellQuery rightCellQuery = this.RightCellQuery;
      Set<AssociationEndMember> set = new Set<AssociationEndMember>((IEqualityComparer<AssociationEndMember>) EqualityComparer<AssociationEndMember>.Default);
      foreach (int keySlot in this.m_memberMaps.ProjectedSlotMap.KeySlots)
      {
        AssociationEndMember rootEdmMember = (AssociationEndMember) ((MemberProjectedSlot) rightCellQuery.ProjectedSlotAt(keySlot)).MemberPath.RootEdmMember;
        set.Add(rootEdmMember);
      }
      return set;
    }

    internal MemberProjectedSlot GetLeftSideMappedSlotForRightSideMember(
      MemberPath member)
    {
      int projectedPosition = this.RightCellQuery.GetProjectedPosition(new MemberProjectedSlot(member));
      if (projectedPosition == -1)
        return (MemberProjectedSlot) null;
      ProjectedSlot projectedSlot = this.LeftCellQuery.ProjectedSlotAt(projectedPosition);
      switch (projectedSlot)
      {
        case null:
        case ConstantProjectedSlot _:
          return (MemberProjectedSlot) null;
        default:
          return projectedSlot as MemberProjectedSlot;
      }
    }

    internal MemberProjectedSlot GetRightSideMappedSlotForLeftSideMember(
      MemberPath member)
    {
      int projectedPosition = this.LeftCellQuery.GetProjectedPosition(new MemberProjectedSlot(member));
      if (projectedPosition == -1)
        return (MemberProjectedSlot) null;
      ProjectedSlot projectedSlot = this.RightCellQuery.ProjectedSlotAt(projectedPosition);
      switch (projectedSlot)
      {
        case null:
        case ConstantProjectedSlot _:
          return (MemberProjectedSlot) null;
        default:
          return projectedSlot as MemberProjectedSlot;
      }
    }

    internal MemberProjectedSlot GetCSideMappedSlotForSMember(MemberPath member) => this.m_viewTarget == ViewTarget.QueryView ? this.GetLeftSideMappedSlotForRightSideMember(member) : this.GetRightSideMappedSlotForLeftSideMember(member);

    private class BoolWrapperComparer : IEqualityComparer<LeftCellWrapper>
    {
      public bool Equals(LeftCellWrapper left, LeftCellWrapper right)
      {
        if (left == right)
          return true;
        if (left == null || right == null)
          return false;
        bool flag = BoolExpression.EqualityComparer.Equals(left.RightCellQuery.WhereClause, right.RightCellQuery.WhereClause);
        return left.RightExtent.Equals((object) right.RightExtent) & flag;
      }

      public int GetHashCode(LeftCellWrapper wrapper) => BoolExpression.EqualityComparer.GetHashCode(wrapper.RightCellQuery.WhereClause) ^ wrapper.RightExtent.GetHashCode();
    }

    private class LeftCellWrapperComparer : IComparer<LeftCellWrapper>
    {
      public int Compare(LeftCellWrapper x, LeftCellWrapper y)
      {
        if (x.FragmentQuery.Attributes.Count > y.FragmentQuery.Attributes.Count)
          return -1;
        return x.FragmentQuery.Attributes.Count < y.FragmentQuery.Attributes.Count ? 1 : string.CompareOrdinal(x.OriginalCellNumberString, y.OriginalCellNumberString);
      }
    }

    internal class CellIdComparer : IComparer<LeftCellWrapper>
    {
      public int Compare(LeftCellWrapper x, LeftCellWrapper y) => StringComparer.Ordinal.Compare(x.OriginalCellNumberString, y.OriginalCellNumberString);
    }
  }
}
