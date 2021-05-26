// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.LeafCellTreeNode
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration;
using System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal class LeafCellTreeNode : CellTreeNode
  {
    internal static readonly IEqualityComparer<LeafCellTreeNode> EqualityComparer = (IEqualityComparer<LeafCellTreeNode>) new LeafCellTreeNode.LeafCellTreeNodeComparer();
    private readonly LeftCellWrapper m_cellWrapper;
    private readonly FragmentQuery m_rightFragmentQuery;

    internal LeafCellTreeNode(ViewgenContext context, LeftCellWrapper cellWrapper)
      : base(context)
    {
      this.m_cellWrapper = cellWrapper;
      this.m_rightFragmentQuery = FragmentQuery.Create(cellWrapper.OriginalCellNumberString, cellWrapper.CreateRoleBoolean(), cellWrapper.RightCellQuery);
    }

    internal LeafCellTreeNode(
      ViewgenContext context,
      LeftCellWrapper cellWrapper,
      FragmentQuery rightFragmentQuery)
      : base(context)
    {
      this.m_cellWrapper = cellWrapper;
      this.m_rightFragmentQuery = rightFragmentQuery;
    }

    internal LeftCellWrapper LeftCellWrapper => this.m_cellWrapper;

    internal override MemberDomainMap RightDomainMap => this.m_cellWrapper.RightDomainMap;

    internal override FragmentQuery LeftFragmentQuery => this.m_cellWrapper.FragmentQuery;

    internal override FragmentQuery RightFragmentQuery => this.m_rightFragmentQuery;

    internal override Set<MemberPath> Attributes => this.m_cellWrapper.Attributes;

    internal override List<CellTreeNode> Children => new List<CellTreeNode>();

    internal override CellTreeOpType OpType => CellTreeOpType.Leaf;

    internal override int NumProjectedSlots => this.LeftCellWrapper.RightCellQuery.NumProjectedSlots;

    internal override int NumBoolSlots => this.LeftCellWrapper.RightCellQuery.NumBoolVars;

    internal override TOutput Accept<TInput, TOutput>(
      CellTreeNode.CellTreeVisitor<TInput, TOutput> visitor,
      TInput param)
    {
      return visitor.VisitLeaf(this, param);
    }

    internal override TOutput Accept<TInput, TOutput>(
      CellTreeNode.SimpleCellTreeVisitor<TInput, TOutput> visitor,
      TInput param)
    {
      return visitor.VisitLeaf(this, param);
    }

    internal override bool IsProjectedSlot(int slot)
    {
      CellQuery rightCellQuery = this.LeftCellWrapper.RightCellQuery;
      return this.IsBoolSlot(slot) ? rightCellQuery.GetBoolVar(this.SlotToBoolIndex(slot)) != null : rightCellQuery.ProjectedSlotAt(slot) != null;
    }

    internal override CqlBlock ToCqlBlock(
      bool[] requiredSlots,
      CqlIdentifiers identifiers,
      ref int blockAliasNum,
      ref List<WithRelationship> withRelationships)
    {
      int length = requiredSlots.Length;
      CellQuery rightCellQuery = this.LeftCellWrapper.RightCellQuery;
      SlotInfo[] slotInfoArray = new SlotInfo[length];
      for (int index = 0; index < rightCellQuery.NumProjectedSlots; ++index)
      {
        ProjectedSlot slotValue = rightCellQuery.ProjectedSlotAt(index);
        if (requiredSlots[index] && slotValue == null)
        {
          ConstantProjectedSlot slot = new ConstantProjectedSlot(Domain.GetDefaultValueForMemberPath(this.ProjectedSlotMap[index], (IEnumerable<LeftCellWrapper>) this.GetLeaves(), this.ViewgenContext.Config));
          rightCellQuery.FixMissingSlotAsDefaultConstant(index, slot);
          slotValue = (ProjectedSlot) slot;
        }
        SlotInfo slotInfo = new SlotInfo(requiredSlots[index], slotValue != null, slotValue, this.ProjectedSlotMap[index]);
        slotInfoArray[index] = slotInfo;
      }
      for (int index = 0; index < rightCellQuery.NumBoolVars; ++index)
      {
        BoolExpression boolVar = rightCellQuery.GetBoolVar(index);
        BooleanProjectedSlot booleanProjectedSlot = boolVar == null ? new BooleanProjectedSlot(BoolExpression.False, identifiers, index) : new BooleanProjectedSlot(boolVar, identifiers, index);
        int slot = this.BoolIndexToSlot(index);
        SlotInfo slotInfo = new SlotInfo(requiredSlots[slot], boolVar != null, (ProjectedSlot) booleanProjectedSlot, (MemberPath) null);
        slotInfoArray[slot] = slotInfo;
      }
      IEnumerable<SlotInfo> source = (IEnumerable<SlotInfo>) slotInfoArray;
      if (rightCellQuery.Extent.EntityContainer.DataSpace == DataSpace.SSpace && this.m_cellWrapper.LeftExtent.BuiltInTypeKind == BuiltInTypeKind.EntitySet)
      {
        IEnumerable<AssociationSetMapping> relationshipSetMappingsFor = this.ViewgenContext.EntityContainerMapping.GetRelationshipSetMappingsFor(this.m_cellWrapper.LeftExtent, rightCellQuery.Extent);
        List<SlotInfo> foreignKeySlots = new List<SlotInfo>();
        foreach (AssociationSetMapping collocatedAssociationSetMap in relationshipSetMappingsFor)
        {
          WithRelationship withRelationship;
          if (LeafCellTreeNode.TryGetWithRelationship(collocatedAssociationSetMap, this.m_cellWrapper.LeftExtent, rightCellQuery.SourceExtentMemberPath, ref foreignKeySlots, out withRelationship))
          {
            withRelationships.Add(withRelationship);
            source = ((IEnumerable<SlotInfo>) slotInfoArray).Concat<SlotInfo>((IEnumerable<SlotInfo>) foreignKeySlots);
          }
        }
      }
      return (CqlBlock) new ExtentCqlBlock(rightCellQuery.Extent, rightCellQuery.SelectDistinctFlag, source.ToArray<SlotInfo>(), rightCellQuery.WhereClause, identifiers, ++blockAliasNum);
    }

    private static bool TryGetWithRelationship(
      AssociationSetMapping collocatedAssociationSetMap,
      EntitySetBase thisExtent,
      MemberPath sRootNode,
      ref List<SlotInfo> foreignKeySlots,
      out WithRelationship withRelationship)
    {
      withRelationship = (WithRelationship) null;
      EndPropertyMapping fromAssociationMap = LeafCellTreeNode.GetForeignKeyEndMapFromAssociationMap(collocatedAssociationSetMap);
      if (fromAssociationMap == null || fromAssociationMap.AssociationEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many)
        return false;
      AssociationEndMember associationEnd = fromAssociationMap.AssociationEnd;
      AssociationEndMember otherAssociationEnd = MetadataHelper.GetOtherAssociationEnd(associationEnd);
      EntityType elementType1 = (EntityType) ((RefType) associationEnd.TypeUsage.EdmType).ElementType;
      EntityType elementType2 = (EntityType) ((RefType) otherAssociationEnd.TypeUsage.EdmType).ElementType;
      AssociationSet set = (AssociationSet) collocatedAssociationSetMap.Set;
      MemberPath prefix = new MemberPath((EntitySetBase) set, (EdmMember) associationEnd);
      IEnumerable<ScalarPropertyMapping> source = fromAssociationMap.PropertyMappings.Cast<ScalarPropertyMapping>();
      List<MemberPath> memberPathList = new List<MemberPath>();
      foreach (EdmProperty keyMember in elementType1.KeyMembers)
      {
        EdmProperty edmProperty = keyMember;
        ScalarPropertyMapping scalarPropertyMapping = source.Where<ScalarPropertyMapping>((Func<ScalarPropertyMapping, bool>) (propMap => propMap.Property.Equals((object) edmProperty))).First<ScalarPropertyMapping>();
        MemberProjectedSlot memberProjectedSlot = new MemberProjectedSlot(new MemberPath(sRootNode, (EdmMember) scalarPropertyMapping.Column));
        MemberPath outputMember = new MemberPath(prefix, (EdmMember) edmProperty);
        memberPathList.Add(outputMember);
        foreignKeySlots.Add(new SlotInfo(true, true, (ProjectedSlot) memberProjectedSlot, outputMember));
      }
      if (!thisExtent.ElementType.IsAssignableFrom((EdmType) elementType2))
        return false;
      withRelationship = new WithRelationship(set, otherAssociationEnd, elementType2, associationEnd, elementType1, (IEnumerable<MemberPath>) memberPathList);
      return true;
    }

    private static EndPropertyMapping GetForeignKeyEndMapFromAssociationMap(
      AssociationSetMapping collocatedAssociationSetMap)
    {
      MappingFragment mappingFragment = collocatedAssociationSetMap.TypeMappings.First<TypeMapping>().MappingFragments.First<MappingFragment>();
      IEnumerable<EdmMember> keyMembers = (IEnumerable<EdmMember>) collocatedAssociationSetMap.StoreEntitySet.ElementType.KeyMembers;
      foreach (EndPropertyMapping propertyMapping in mappingFragment.PropertyMappings)
      {
        EndPropertyMapping endMap = propertyMapping;
        if (endMap.StoreProperties.SequenceEqual<EdmMember>(keyMembers, (IEqualityComparer<EdmMember>) System.Collections.Generic.EqualityComparer<EdmMember>.Default))
          return mappingFragment.PropertyMappings.OfType<EndPropertyMapping>().Where<EndPropertyMapping>((Func<EndPropertyMapping, bool>) (eMap => !eMap.Equals((object) endMap))).First<EndPropertyMapping>();
      }
      return (EndPropertyMapping) null;
    }

    internal override void ToCompactString(StringBuilder stringBuilder) => this.m_cellWrapper.ToCompactString(stringBuilder);

    private class LeafCellTreeNodeComparer : IEqualityComparer<LeafCellTreeNode>
    {
      public bool Equals(LeafCellTreeNode left, LeafCellTreeNode right)
      {
        if (left == right)
          return true;
        return left != null && right != null && left.m_cellWrapper.Equals((object) right.m_cellWrapper);
      }

      public int GetHashCode(LeafCellTreeNode node) => node.m_cellWrapper.GetHashCode();
    }
  }
}
