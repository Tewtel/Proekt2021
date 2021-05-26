// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Validation.ErrorPatternMatcher
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting;
using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;
using System.Data.Entity.Core.Mapping.ViewGeneration.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Validation
{
  internal class ErrorPatternMatcher
  {
    private readonly ViewgenContext m_viewgenContext;
    private readonly MemberDomainMap m_domainMap;
    private readonly ErrorLog m_errorLog;
    private readonly int m_originalErrorCount;
    private const int NUM_PARTITION_ERR_TO_FIND = 5;

    private ErrorPatternMatcher(
      ViewgenContext context,
      MemberDomainMap domainMap,
      ErrorLog errorLog)
    {
      this.m_viewgenContext = context;
      this.m_domainMap = domainMap;
      MemberPath.GetKeyMembers(context.Extent, domainMap);
      this.m_errorLog = errorLog;
      this.m_originalErrorCount = this.m_errorLog.Count;
    }

    public static bool FindMappingErrors(
      ViewgenContext context,
      MemberDomainMap domainMap,
      ErrorLog errorLog)
    {
      if (context.ViewTarget == ViewTarget.QueryView && !context.Config.IsValidationEnabled)
        return false;
      ErrorPatternMatcher errorPatternMatcher = new ErrorPatternMatcher(context, domainMap, errorLog);
      errorPatternMatcher.MatchMissingMappingErrors();
      errorPatternMatcher.MatchConditionErrors();
      errorPatternMatcher.MatchSplitErrors();
      if (errorPatternMatcher.m_errorLog.Count == errorPatternMatcher.m_originalErrorCount)
        errorPatternMatcher.MatchPartitionErrors();
      if (errorPatternMatcher.m_errorLog.Count > errorPatternMatcher.m_originalErrorCount)
        ExceptionHelpers.ThrowMappingException(errorPatternMatcher.m_errorLog, errorPatternMatcher.m_viewgenContext.Config);
      return false;
    }

    private void MatchMissingMappingErrors()
    {
      if (this.m_viewgenContext.ViewTarget != ViewTarget.QueryView)
        return;
      Set<EdmType> set = new Set<EdmType>(MetadataHelper.GetTypeAndSubtypesOf((EdmType) this.m_viewgenContext.Extent.ElementType, (ItemCollection) this.m_viewgenContext.EdmItemCollection, false));
      foreach (LeftCellWrapper leftCellWrapper in this.m_viewgenContext.AllWrappersForExtent)
      {
        foreach (Cell cell in leftCellWrapper.Cells)
        {
          foreach (MemberRestriction condition in cell.CQuery.Conditions)
          {
            foreach (Constant constant in condition.Domain.Values)
            {
              if (constant is TypeConstant typeConstant4)
                set.Remove(typeConstant4.EdmType);
            }
          }
        }
      }
      if (set.Count <= 0)
        return;
      this.m_errorLog.AddEntry(new ErrorLog.Record(ViewGenErrorCode.ErrorPatternMissingMappingError, System.Data.Entity.Resources.Strings.ViewGen_Missing_Type_Mapping((object) ErrorPatternMatcher.BuildCommaSeparatedErrorString<EdmType>((IEnumerable<EdmType>) set)), (IEnumerable<LeftCellWrapper>) this.m_viewgenContext.AllWrappersForExtent, ""));
    }

    private static bool HasNotNullCondition(CellQuery cellQuery, MemberPath member)
    {
      foreach (MemberRestriction memberRestriction in cellQuery.GetConjunctsFromWhereClause())
      {
        if (memberRestriction.RestrictedMemberSlot.MemberPath.Equals(member))
        {
          if (memberRestriction.Domain.Values.Contains<Constant>(Constant.NotNull))
            return true;
          foreach (NegatedConstant negatedConstant in memberRestriction.Domain.Values.Select<Constant, NegatedConstant>((Func<Constant, NegatedConstant>) (cellConstant => cellConstant as NegatedConstant)).Where<NegatedConstant>((Func<NegatedConstant, bool>) (negated => negated != null)))
          {
            if (negatedConstant.Elements.Contains<Constant>(Constant.Null))
              return true;
          }
        }
      }
      return false;
    }

    private static bool IsMemberPartOfNotNullCondition(
      IEnumerable<LeftCellWrapper> wrappers,
      MemberPath leftMember,
      ViewTarget viewTarget)
    {
      foreach (LeftCellWrapper wrapper in wrappers)
      {
        CellQuery leftQuery = wrapper.OnlyInputCell.GetLeftQuery(viewTarget);
        if (ErrorPatternMatcher.HasNotNullCondition(leftQuery, leftMember))
          return true;
        CellQuery rightQuery = wrapper.OnlyInputCell.GetRightQuery(viewTarget);
        int slotNum = leftQuery.GetProjectedMembers().TakeWhile<MemberPath>((Func<MemberPath, bool>) (path => !path.Equals(leftMember))).Count<MemberPath>();
        if (slotNum < leftQuery.GetProjectedMembers().Count<MemberPath>())
        {
          MemberPath memberPath = ((MemberProjectedSlot) rightQuery.ProjectedSlotAt(slotNum)).MemberPath;
          if (ErrorPatternMatcher.HasNotNullCondition(rightQuery, memberPath))
            return true;
        }
      }
      return false;
    }

    private void MatchConditionErrors()
    {
      List<LeftCellWrapper> wrappersForExtent = this.m_viewgenContext.AllWrappersForExtent;
      Set<MemberPath> mappedConditionMembers = new Set<MemberPath>();
      Set<Dictionary<MemberPath, Set<Constant>>> set1 = new Set<Dictionary<MemberPath, Set<Constant>>>((IEqualityComparer<Dictionary<MemberPath, Set<Constant>>>) new ConditionComparer());
      Dictionary<Dictionary<MemberPath, Set<Constant>>, LeftCellWrapper> dictionary1 = new Dictionary<Dictionary<MemberPath, Set<Constant>>, LeftCellWrapper>((IEqualityComparer<Dictionary<MemberPath, Set<Constant>>>) new ConditionComparer());
      foreach (LeftCellWrapper leftCellWrapper in wrappersForExtent)
      {
        Dictionary<MemberPath, Set<Constant>> dictionary2 = new Dictionary<MemberPath, Set<Constant>>();
        foreach (MemberRestriction memberRestriction in leftCellWrapper.OnlyInputCell.GetLeftQuery(this.m_viewgenContext.ViewTarget).GetConjunctsFromWhereClause())
        {
          MemberPath memberPath = memberRestriction.RestrictedMemberSlot.MemberPath;
          if (this.m_domainMap.IsConditionMember(memberPath))
          {
            if (memberRestriction is ScalarRestriction scalarRestriction6 && !mappedConditionMembers.Contains(memberPath) && (!leftCellWrapper.OnlyInputCell.CQuery.WhereClause.Equals((object) leftCellWrapper.OnlyInputCell.SQuery.WhereClause) && !ErrorPatternMatcher.IsMemberPartOfNotNullCondition((IEnumerable<LeftCellWrapper>) wrappersForExtent, memberPath, this.m_viewgenContext.ViewTarget)))
              this.CheckThatConditionMemberIsNotMapped(memberPath, wrappersForExtent, mappedConditionMembers);
            if (this.m_viewgenContext.ViewTarget == ViewTarget.UpdateView && scalarRestriction6 != null && memberPath.IsNullable)
            {
              if (ErrorPatternMatcher.IsMemberPartOfNotNullCondition((IEnumerable<LeftCellWrapper>) new LeftCellWrapper[1]
              {
                leftCellWrapper
              }, memberPath, this.m_viewgenContext.ViewTarget))
              {
                MemberPath rightMemberPath = ErrorPatternMatcher.GetRightMemberPath(memberPath, leftCellWrapper);
                if (rightMemberPath != null && rightMemberPath.IsNullable)
                {
                  if (!ErrorPatternMatcher.IsMemberPartOfNotNullCondition((IEnumerable<LeftCellWrapper>) new LeftCellWrapper[1]
                  {
                    leftCellWrapper
                  }, rightMemberPath, this.m_viewgenContext.ViewTarget))
                    this.m_errorLog.AddEntry(new ErrorLog.Record(ViewGenErrorCode.ErrorPatternConditionError, System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_NotNullConditionMappedToNullableMember((object) memberPath, (object) rightMemberPath), leftCellWrapper.OnlyInputCell, ""));
                }
              }
            }
            foreach (Constant element in memberRestriction.Domain.Values)
            {
              Set<Constant> set2;
              if (!dictionary2.TryGetValue(memberPath, out set2))
              {
                set2 = new Set<Constant>(Constant.EqualityComparer);
                dictionary2.Add(memberPath, set2);
              }
              set2.Add(element);
            }
          }
        }
        if (dictionary2.Count > 0)
        {
          if (set1.Contains(dictionary2))
          {
            if (!this.RightSideEqual(dictionary1[dictionary2], leftCellWrapper))
              this.m_errorLog.AddEntry(new ErrorLog.Record(ViewGenErrorCode.ErrorPatternConditionError, System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_DuplicateConditionValue((object) ErrorPatternMatcher.BuildCommaSeparatedErrorString<MemberPath>((IEnumerable<MemberPath>) dictionary2.Keys)), ErrorPatternMatcher.ToIEnum(dictionary1[dictionary2].OnlyInputCell, leftCellWrapper.OnlyInputCell), ""));
          }
          else
          {
            set1.Add(dictionary2);
            dictionary1.Add(dictionary2, leftCellWrapper);
          }
        }
      }
    }

    private static MemberPath GetRightMemberPath(
      MemberPath conditionMember,
      LeftCellWrapper leftCellWrapper)
    {
      List<int> projectedPositions = leftCellWrapper.OnlyInputCell.GetRightQuery(ViewTarget.QueryView).GetProjectedPositions(conditionMember);
      if (projectedPositions.Count != 1)
        return (MemberPath) null;
      int slotNum = projectedPositions.First<int>();
      return ((MemberProjectedSlot) leftCellWrapper.OnlyInputCell.GetLeftQuery(ViewTarget.QueryView).ProjectedSlotAt(slotNum)).MemberPath;
    }

    private void MatchSplitErrors()
    {
      IEnumerable<LeftCellWrapper> source = this.m_viewgenContext.AllWrappersForExtent.Where<LeftCellWrapper>((Func<LeftCellWrapper, bool>) (r => !(r.LeftExtent is AssociationSet) && !(r.RightCellQuery.Extent is AssociationSet)));
      if (this.m_viewgenContext.ViewTarget != ViewTarget.UpdateView || !source.Any<LeftCellWrapper>())
        return;
      LeftCellWrapper wrapper2 = source.First<LeftCellWrapper>();
      EntitySetBase extent = wrapper2.RightCellQuery.Extent;
      foreach (LeftCellWrapper wrapper1 in source)
      {
        if (!wrapper1.RightCellQuery.Extent.EdmEquals((MetadataItem) extent) && !this.RightSideEqual(wrapper1, wrapper2))
          this.m_errorLog.AddEntry(new ErrorLog.Record(ViewGenErrorCode.ErrorPatternSplittingError, System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_TableMappedToMultipleES((object) wrapper1.LeftExtent.ToString(), (object) wrapper1.RightCellQuery.Extent.ToString(), (object) extent.ToString()), wrapper1.Cells.First<Cell>(), ""));
      }
    }

    private void MatchPartitionErrors()
    {
      List<LeftCellWrapper> wrappersForExtent = this.m_viewgenContext.AllWrappersForExtent;
      int num1 = 0;
      foreach (LeftCellWrapper leftCellWrapper1 in wrappersForExtent)
      {
        foreach (LeftCellWrapper leftCellWrapper2 in wrappersForExtent.Skip<LeftCellWrapper>(++num1))
        {
          FragmentQuery rightFragmentQuery1 = this.CreateRightFragmentQuery(leftCellWrapper1);
          FragmentQuery rightFragmentQuery2 = this.CreateRightFragmentQuery(leftCellWrapper2);
          int num2 = this.CompareS(ErrorPatternMatcher.ComparisonOP.IsDisjointFrom, this.m_viewgenContext, leftCellWrapper1, leftCellWrapper2, rightFragmentQuery1, rightFragmentQuery2) ? 1 : 0;
          bool flag1 = this.CompareC(ErrorPatternMatcher.ComparisonOP.IsDisjointFrom, this.m_viewgenContext, leftCellWrapper1, leftCellWrapper2, rightFragmentQuery1, rightFragmentQuery2);
          bool flag2;
          bool flag3;
          if (num2 != 0)
          {
            if (!flag1)
            {
              flag2 = this.CompareC(ErrorPatternMatcher.ComparisonOP.IsContainedIn, this.m_viewgenContext, leftCellWrapper1, leftCellWrapper2, rightFragmentQuery1, rightFragmentQuery2);
              flag3 = this.CompareC(ErrorPatternMatcher.ComparisonOP.IsContainedIn, this.m_viewgenContext, leftCellWrapper2, leftCellWrapper1, rightFragmentQuery2, rightFragmentQuery1);
              bool flag4 = flag2 & flag3;
              StringBuilder stringBuilder = new StringBuilder();
              if (flag4)
                stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Disj_Eq);
              else if (flag2 | flag3)
              {
                if (this.CSideHasDifferentEntitySets(leftCellWrapper1, leftCellWrapper2))
                  stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Disj_Subs_Ref);
                else
                  stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Disj_Subs);
              }
              else
                stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Disj_Unk);
              this.m_errorLog.AddEntry(new ErrorLog.Record(ViewGenErrorCode.ErrorPatternInvalidPartitionError, stringBuilder.ToString(), ErrorPatternMatcher.ToIEnum(leftCellWrapper1.OnlyInputCell, leftCellWrapper2.OnlyInputCell), ""));
              if (this.FoundTooManyErrors())
                return;
            }
            else
              continue;
          }
          else
          {
            flag2 = this.CompareC(ErrorPatternMatcher.ComparisonOP.IsContainedIn, this.m_viewgenContext, leftCellWrapper1, leftCellWrapper2, rightFragmentQuery1, rightFragmentQuery2);
            flag3 = this.CompareC(ErrorPatternMatcher.ComparisonOP.IsContainedIn, this.m_viewgenContext, leftCellWrapper2, leftCellWrapper1, rightFragmentQuery2, rightFragmentQuery1);
          }
          bool flag5 = this.CompareS(ErrorPatternMatcher.ComparisonOP.IsContainedIn, this.m_viewgenContext, leftCellWrapper1, leftCellWrapper2, rightFragmentQuery1, rightFragmentQuery2);
          bool flag6 = this.CompareS(ErrorPatternMatcher.ComparisonOP.IsContainedIn, this.m_viewgenContext, leftCellWrapper2, leftCellWrapper1, rightFragmentQuery2, rightFragmentQuery1);
          bool flag7 = flag2 & flag3;
          if (flag5 & flag6)
          {
            if (!flag7)
            {
              StringBuilder stringBuilder = new StringBuilder();
              if (flag1)
                stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Eq_Disj);
              else if (flag2 | flag3)
              {
                if (this.CSideHasDifferentEntitySets(leftCellWrapper1, leftCellWrapper2))
                {
                  stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Eq_Subs_Ref);
                }
                else
                {
                  if (leftCellWrapper1.LeftExtent.Equals((object) leftCellWrapper2.LeftExtent))
                  {
                    bool hasCondition1;
                    List<EdmType> edmTypes1;
                    ErrorPatternMatcher.GetTypesAndConditionForWrapper(leftCellWrapper1, out hasCondition1, out edmTypes1);
                    bool hasCondition2;
                    List<EdmType> edmTypes2;
                    ErrorPatternMatcher.GetTypesAndConditionForWrapper(leftCellWrapper2, out hasCondition2, out edmTypes2);
                    if (!hasCondition1 && !hasCondition2 && (edmTypes1.Except<EdmType>((IEnumerable<EdmType>) edmTypes2).Count<EdmType>() != 0 || edmTypes2.Except<EdmType>((IEnumerable<EdmType>) edmTypes1).Count<EdmType>() != 0) && (!ErrorPatternMatcher.CheckForStoreConditions(leftCellWrapper1) || !ErrorPatternMatcher.CheckForStoreConditions(leftCellWrapper2)))
                    {
                      this.m_errorLog.AddEntry(new ErrorLog.Record(ViewGenErrorCode.ErrorPatternConditionError, System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_MultipleTypesMappedToSameTable_WithoutCondition((object) StringUtil.ToCommaSeparatedString((IEnumerable) edmTypes1.Select<EdmType, string>((Func<EdmType, string>) (it => it.FullName)).Union<string>(edmTypes2.Select<EdmType, string>((Func<EdmType, string>) (it => it.FullName)))), (object) leftCellWrapper1.LeftExtent), ErrorPatternMatcher.ToIEnum(leftCellWrapper1.OnlyInputCell, leftCellWrapper2.OnlyInputCell), ""));
                      return;
                    }
                  }
                  stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Eq_Subs);
                }
              }
              else if (!this.IsQueryView() && (leftCellWrapper1.OnlyInputCell.CQuery.Extent is AssociationSet || leftCellWrapper2.OnlyInputCell.CQuery.Extent is AssociationSet))
                stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Eq_Unk_Association);
              else
                stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Eq_Unk);
              this.m_errorLog.AddEntry(new ErrorLog.Record(ViewGenErrorCode.ErrorPatternInvalidPartitionError, stringBuilder.ToString(), ErrorPatternMatcher.ToIEnum(leftCellWrapper1.OnlyInputCell, leftCellWrapper2.OnlyInputCell), ""));
              if (this.FoundTooManyErrors())
                return;
            }
          }
          else if (flag5 | flag6 && (!(flag5 & flag2) || flag3) && (!(flag6 & flag3) || flag2))
          {
            StringBuilder stringBuilder = new StringBuilder();
            if (flag1)
              stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Sub_Disj);
            else if (flag7)
            {
              if (this.CSideHasDifferentEntitySets(leftCellWrapper1, leftCellWrapper2))
                stringBuilder.Append(" " + System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Sub_Eq_Ref);
              else
                stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Sub_Eq);
            }
            else
              stringBuilder.Append(System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_Partition_Sub_Unk);
            this.m_errorLog.AddEntry(new ErrorLog.Record(ViewGenErrorCode.ErrorPatternInvalidPartitionError, stringBuilder.ToString(), ErrorPatternMatcher.ToIEnum(leftCellWrapper1.OnlyInputCell, leftCellWrapper2.OnlyInputCell), ""));
            if (this.FoundTooManyErrors())
              return;
          }
        }
      }
    }

    private static void GetTypesAndConditionForWrapper(
      LeftCellWrapper wrapper,
      out bool hasCondition,
      out List<EdmType> edmTypes)
    {
      hasCondition = false;
      edmTypes = new List<EdmType>();
      foreach (Cell cell in wrapper.Cells)
      {
        foreach (MemberRestriction condition in cell.CQuery.Conditions)
        {
          foreach (Constant constant in condition.Domain.Values)
          {
            if (constant is TypeConstant typeConstant3)
              edmTypes.Add(typeConstant3.EdmType);
            else
              hasCondition = true;
          }
        }
      }
    }

    private static bool CheckForStoreConditions(LeftCellWrapper wrapper) => wrapper.Cells.SelectMany<Cell, MemberRestriction>((Func<Cell, IEnumerable<MemberRestriction>>) (c => c.SQuery.Conditions)).Any<MemberRestriction>();

    private void CheckThatConditionMemberIsNotMapped(
      MemberPath conditionMember,
      List<LeftCellWrapper> mappingFragments,
      Set<MemberPath> mappedConditionMembers)
    {
      foreach (LeftCellWrapper mappingFragment in mappingFragments)
      {
        foreach (Cell cell in mappingFragment.Cells)
        {
          if (cell.GetLeftQuery(this.m_viewgenContext.ViewTarget).GetProjectedMembers().Contains<MemberPath>(conditionMember))
          {
            mappedConditionMembers.Add(conditionMember);
            this.m_errorLog.AddEntry(new ErrorLog.Record(ViewGenErrorCode.ErrorPatternConditionError, System.Data.Entity.Resources.Strings.Viewgen_ErrorPattern_ConditionMemberIsMapped((object) conditionMember.ToString()), cell, ""));
          }
        }
      }
    }

    private bool FoundTooManyErrors() => this.m_errorLog.Count > this.m_originalErrorCount + 5;

    private static string BuildCommaSeparatedErrorString<T>(IEnumerable<T> members)
    {
      StringBuilder stringBuilder = new StringBuilder();
      T obj = members.First<T>();
      foreach (T member in members)
      {
        if (!member.Equals((object) obj))
          stringBuilder.Append(", ");
        stringBuilder.Append("'" + member?.ToString() + "'");
      }
      return stringBuilder.ToString();
    }

    private bool CSideHasDifferentEntitySets(LeftCellWrapper a, LeftCellWrapper b) => this.IsQueryView() ? a.LeftExtent == b.LeftExtent : a.RightCellQuery == b.RightCellQuery;

    private bool CompareC(
      ErrorPatternMatcher.ComparisonOP op,
      ViewgenContext context,
      LeftCellWrapper leftWrapper1,
      LeftCellWrapper leftWrapper2,
      FragmentQuery rightQuery1,
      FragmentQuery rightQuery2)
    {
      return this.Compare(true, op, context, leftWrapper1, leftWrapper2, rightQuery1, rightQuery2);
    }

    private bool CompareS(
      ErrorPatternMatcher.ComparisonOP op,
      ViewgenContext context,
      LeftCellWrapper leftWrapper1,
      LeftCellWrapper leftWrapper2,
      FragmentQuery rightQuery1,
      FragmentQuery rightQuery2)
    {
      return this.Compare(false, op, context, leftWrapper1, leftWrapper2, rightQuery1, rightQuery2);
    }

    private bool Compare(
      bool lookingForC,
      ErrorPatternMatcher.ComparisonOP op,
      ViewgenContext context,
      LeftCellWrapper leftWrapper1,
      LeftCellWrapper leftWrapper2,
      FragmentQuery rightQuery1,
      FragmentQuery rightQuery2)
    {
      if (lookingForC && this.IsQueryView() || !lookingForC && !this.IsQueryView())
      {
        LCWComparer lcwComparer;
        switch (op)
        {
          case ErrorPatternMatcher.ComparisonOP.IsContainedIn:
            lcwComparer = new LCWComparer(context.LeftFragmentQP.IsContainedIn);
            break;
          case ErrorPatternMatcher.ComparisonOP.IsDisjointFrom:
            lcwComparer = new LCWComparer(context.LeftFragmentQP.IsDisjointFrom);
            break;
          default:
            return false;
        }
        return lcwComparer(leftWrapper1.FragmentQuery, leftWrapper2.FragmentQuery);
      }
      LCWComparer lcwComparer1;
      switch (op)
      {
        case ErrorPatternMatcher.ComparisonOP.IsContainedIn:
          lcwComparer1 = new LCWComparer(context.RightFragmentQP.IsContainedIn);
          break;
        case ErrorPatternMatcher.ComparisonOP.IsDisjointFrom:
          lcwComparer1 = new LCWComparer(context.RightFragmentQP.IsDisjointFrom);
          break;
        default:
          return false;
      }
      return lcwComparer1(rightQuery1, rightQuery2);
    }

    private bool RightSideEqual(LeftCellWrapper wrapper1, LeftCellWrapper wrapper2) => this.m_viewgenContext.RightFragmentQP.IsEquivalentTo(this.CreateRightFragmentQuery(wrapper1), this.CreateRightFragmentQuery(wrapper2));

    private FragmentQuery CreateRightFragmentQuery(LeftCellWrapper wrapper) => FragmentQuery.Create(wrapper.OnlyInputCell.CellLabel.ToString(), wrapper.CreateRoleBoolean(), wrapper.OnlyInputCell.GetRightQuery(this.m_viewgenContext.ViewTarget));

    private static IEnumerable<Cell> ToIEnum(Cell one, Cell two) => (IEnumerable<Cell>) new List<Cell>()
    {
      one,
      two
    };

    private bool IsQueryView() => this.m_viewgenContext.ViewTarget == ViewTarget.QueryView;

    private enum ComparisonOP
    {
      IsContainedIn,
      IsDisjointFrom,
    }
  }
}
