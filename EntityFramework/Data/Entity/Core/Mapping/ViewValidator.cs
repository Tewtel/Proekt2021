// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewValidator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Mapping
{
  internal static class ViewValidator
  {
    internal static IEnumerable<EdmSchemaError> ValidateQueryView(
      DbQueryCommandTree view,
      EntitySetBaseMapping setMapping,
      EntityTypeBase elementType,
      bool includeSubtypes)
    {
      ViewValidator.ViewExpressionValidator expressionValidator = new ViewValidator.ViewExpressionValidator(setMapping, elementType, includeSubtypes);
      expressionValidator.VisitExpression(view.Query);
      if (expressionValidator.Errors.Count<EdmSchemaError>() != 0 || setMapping.Set.BuiltInTypeKind != BuiltInTypeKind.AssociationSet)
        return expressionValidator.Errors;
      ViewValidator.AssociationSetViewValidator setViewValidator = new ViewValidator.AssociationSetViewValidator(setMapping);
      setViewValidator.VisitExpression(view.Query);
      return (IEnumerable<EdmSchemaError>) setViewValidator.Errors;
    }

    private sealed class ViewExpressionValidator : BasicExpressionVisitor
    {
      private readonly EntitySetBaseMapping _setMapping;
      private readonly List<EdmSchemaError> _errors;
      private readonly EntityTypeBase _elementType;
      private readonly bool _includeSubtypes;

      private EdmItemCollection EdmItemCollection => this._setMapping.EntityContainerMapping.StorageMappingItemCollection.EdmItemCollection;

      private StoreItemCollection StoreItemCollection => this._setMapping.EntityContainerMapping.StorageMappingItemCollection.StoreItemCollection;

      internal ViewExpressionValidator(
        EntitySetBaseMapping setMapping,
        EntityTypeBase elementType,
        bool includeSubtypes)
      {
        this._setMapping = setMapping;
        this._elementType = elementType;
        this._includeSubtypes = includeSubtypes;
        this._errors = new List<EdmSchemaError>();
      }

      internal IEnumerable<EdmSchemaError> Errors => (IEnumerable<EdmSchemaError>) this._errors;

      public override void VisitExpression(DbExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbExpression>(expression, nameof (expression));
        this.ValidateExpressionKind(expression.ExpressionKind);
        base.VisitExpression(expression);
      }

      private void ValidateExpressionKind(DbExpressionKind expressionKind)
      {
        switch (expressionKind)
        {
          case DbExpressionKind.And:
            break;
          case DbExpressionKind.Case:
            break;
          case DbExpressionKind.Cast:
            break;
          case DbExpressionKind.Constant:
            break;
          case DbExpressionKind.EntityRef:
            break;
          case DbExpressionKind.Equals:
            break;
          case DbExpressionKind.Filter:
            break;
          case DbExpressionKind.FullOuterJoin:
            break;
          case DbExpressionKind.Function:
            break;
          case DbExpressionKind.GreaterThan:
            break;
          case DbExpressionKind.GreaterThanOrEquals:
            break;
          case DbExpressionKind.InnerJoin:
            break;
          case DbExpressionKind.IsNull:
            break;
          case DbExpressionKind.LeftOuterJoin:
            break;
          case DbExpressionKind.LessThan:
            break;
          case DbExpressionKind.LessThanOrEquals:
            break;
          case DbExpressionKind.NewInstance:
            break;
          case DbExpressionKind.Not:
            break;
          case DbExpressionKind.NotEquals:
            break;
          case DbExpressionKind.Null:
            break;
          case DbExpressionKind.Or:
            break;
          case DbExpressionKind.Project:
            break;
          case DbExpressionKind.Property:
            break;
          case DbExpressionKind.Ref:
            break;
          case DbExpressionKind.Scan:
            break;
          case DbExpressionKind.UnionAll:
          case DbExpressionKind.VariableReference:
            break;
          default:
            this._errors.Add(new EdmSchemaError(System.Data.Entity.Resources.Strings.Mapping_UnsupportedExpressionKind_QueryView((object) this._setMapping.Set.Name, this._includeSubtypes ? (object) ("IsTypeOf(" + this._elementType?.ToString() + ")") : (object) this._elementType.ToString(), (object) expressionKind), 2071, EdmSchemaErrorSeverity.Error, this._setMapping.EntityContainerMapping.SourceLocation, this._setMapping.StartLineNumber, this._setMapping.StartLinePosition));
            break;
        }
      }

      public override void Visit(DbPropertyExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbPropertyExpression>(expression, nameof (expression));
        base.Visit(expression);
        if (expression.Property.BuiltInTypeKind == BuiltInTypeKind.EdmProperty)
          return;
        this._errors.Add(new EdmSchemaError(System.Data.Entity.Resources.Strings.Mapping_UnsupportedPropertyKind_QueryView((object) this._setMapping.Set.Name, (object) expression.Property.Name, (object) expression.Property.BuiltInTypeKind), 2073, EdmSchemaErrorSeverity.Error, this._setMapping.EntityContainerMapping.SourceLocation, this._setMapping.StartLineNumber, this._setMapping.StartLinePosition));
      }

      public override void Visit(DbNewInstanceExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbNewInstanceExpression>(expression, nameof (expression));
        base.Visit(expression);
        EdmType edmType = expression.ResultType.EdmType;
        if (edmType.BuiltInTypeKind == BuiltInTypeKind.RowType || edmType == this._elementType || this._includeSubtypes && this._elementType.IsAssignableFrom(edmType) || edmType.BuiltInTypeKind == BuiltInTypeKind.ComplexType && this.GetComplexTypes().Contains<ComplexType>((ComplexType) edmType))
          return;
        this._errors.Add(new EdmSchemaError(System.Data.Entity.Resources.Strings.Mapping_UnsupportedInitialization_QueryView((object) this._setMapping.Set.Name, (object) edmType.FullName), 2074, EdmSchemaErrorSeverity.Error, this._setMapping.EntityContainerMapping.SourceLocation, this._setMapping.StartLineNumber, this._setMapping.StartLinePosition));
      }

      private IEnumerable<ComplexType> GetComplexTypes() => this.GetComplexTypes(this.GetEntityTypes().SelectMany<EntityType, EdmProperty>((Func<EntityType, IEnumerable<EdmProperty>>) (entityType => (IEnumerable<EdmProperty>) entityType.Properties)).Distinct<EdmProperty>());

      private IEnumerable<ComplexType> GetComplexTypes(
        IEnumerable<EdmProperty> properties)
      {
        foreach (ComplexType complexType1 in properties.Select<EdmProperty, EdmType>((Func<EdmProperty, EdmType>) (p => p.TypeUsage.EdmType)).OfType<ComplexType>())
        {
          ComplexType complexType = complexType1;
          yield return complexType;
          foreach (ComplexType complexType2 in this.GetComplexTypes((IEnumerable<EdmProperty>) complexType.Properties))
            yield return complexType2;
          complexType = (ComplexType) null;
        }
      }

      private IEnumerable<EntityType> GetEntityTypes()
      {
        if (this._includeSubtypes)
          return MetadataHelper.GetTypeAndSubtypesOf((EdmType) this._elementType, (ItemCollection) this.EdmItemCollection, true).OfType<EntityType>();
        return this._elementType.BuiltInTypeKind == BuiltInTypeKind.EntityType ? Enumerable.Repeat<EntityType>((EntityType) this._elementType, 1) : Enumerable.Empty<EntityType>();
      }

      public override void Visit(DbFunctionExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbFunctionExpression>(expression, nameof (expression));
        base.Visit(expression);
        if (ViewValidator.ViewExpressionValidator.IsStoreSpaceOrCanonicalFunction(this.StoreItemCollection, expression.Function))
          return;
        this._errors.Add(new EdmSchemaError(System.Data.Entity.Resources.Strings.Mapping_UnsupportedFunctionCall_QueryView((object) this._setMapping.Set.Name, (object) expression.Function.Identity), 2112, EdmSchemaErrorSeverity.Error, this._setMapping.EntityContainerMapping.SourceLocation, this._setMapping.StartLineNumber, this._setMapping.StartLinePosition));
      }

      internal static bool IsStoreSpaceOrCanonicalFunction(
        StoreItemCollection sSpace,
        EdmFunction function)
      {
        return TypeHelpers.IsCanonicalFunction(function) || sSpace.GetCTypeFunctions(function.FullName, false).Contains(function);
      }

      public override void Visit(DbScanExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbScanExpression>(expression, nameof (expression));
        base.Visit(expression);
        EntitySetBase target = expression.Target;
        if (target.EntityContainer.DataSpace == DataSpace.SSpace)
          return;
        this._errors.Add(new EdmSchemaError(System.Data.Entity.Resources.Strings.Mapping_UnsupportedScanTarget_QueryView((object) this._setMapping.Set.Name, (object) target.Name), 2072, EdmSchemaErrorSeverity.Error, this._setMapping.EntityContainerMapping.SourceLocation, this._setMapping.StartLineNumber, this._setMapping.StartLinePosition));
      }
    }

    private class AssociationSetViewValidator : 
      DbExpressionVisitor<ViewValidator.DbExpressionEntitySetInfo>
    {
      private readonly Stack<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>> variableScopes = new Stack<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>>();
      private readonly EntitySetBaseMapping _setMapping;
      private readonly List<EdmSchemaError> _errors = new List<EdmSchemaError>();

      internal AssociationSetViewValidator(EntitySetBaseMapping setMapping) => this._setMapping = setMapping;

      internal List<EdmSchemaError> Errors => this._errors;

      internal ViewValidator.DbExpressionEntitySetInfo VisitExpression(
        DbExpression expression)
      {
        return expression.Accept<ViewValidator.DbExpressionEntitySetInfo>((DbExpressionVisitor<ViewValidator.DbExpressionEntitySetInfo>) this);
      }

      private ViewValidator.DbExpressionEntitySetInfo VisitExpressionBinding(
        DbExpressionBinding binding)
      {
        return binding != null ? this.VisitExpression(binding.Expression) : (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      private void VisitExpressionBindingEnterScope(DbExpressionBinding binding)
      {
        ViewValidator.DbExpressionEntitySetInfo expressionEntitySetInfo = this.VisitExpressionBinding(binding);
        this.variableScopes.Push(new KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>(binding.VariableName, expressionEntitySetInfo));
      }

      private void VisitExpressionBindingExitScope() => this.variableScopes.Pop();

      private void ValidateEntitySetsMappedForAssociationSetMapping(
        ViewValidator.DbExpressionStructuralTypeEntitySetInfo setInfos)
      {
        AssociationSet set = this._setMapping.Set as AssociationSet;
        int index = 0;
        if (!setInfos.SetInfos.All<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>>((Func<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>, bool>) (it => it.Value != null && it.Value is ViewValidator.DbExpressionSimpleTypeEntitySetInfo)) || setInfos.SetInfos.Count<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>>() != 2)
          return;
        foreach (ViewValidator.DbExpressionSimpleTypeEntitySetInfo typeEntitySetInfo in setInfos.SetInfos.Select<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>, ViewValidator.DbExpressionEntitySetInfo>((Func<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>, ViewValidator.DbExpressionEntitySetInfo>) (it => it.Value)))
        {
          AssociationSetEnd associationSetEnd = set.AssociationSetEnds[index];
          EntitySet entitySet = associationSetEnd.EntitySet;
          if (!entitySet.Equals((object) typeEntitySetInfo.EntitySet))
            this._errors.Add(new EdmSchemaError(System.Data.Entity.Resources.Strings.Mapping_EntitySetMismatchOnAssociationSetEnd_QueryView((object) typeEntitySetInfo.EntitySet.Name, (object) entitySet.Name, (object) associationSetEnd.Name, (object) this._setMapping.Set.Name), 2074, EdmSchemaErrorSeverity.Error, this._setMapping.EntityContainerMapping.SourceLocation, this._setMapping.StartLineNumber, this._setMapping.StartLinePosition));
          ++index;
        }
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbVariableReferenceExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbVariableReferenceExpression>(expression, nameof (expression));
        return this.variableScopes.Where<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>>((Func<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>, bool>) (it => it.Key == expression.VariableName)).Select<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>, ViewValidator.DbExpressionEntitySetInfo>((Func<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>, ViewValidator.DbExpressionEntitySetInfo>) (it => it.Value)).FirstOrDefault<ViewValidator.DbExpressionEntitySetInfo>();
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbPropertyExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbPropertyExpression>(expression, nameof (expression));
        return this.VisitExpression(expression.Instance) is ViewValidator.DbExpressionStructuralTypeEntitySetInfo typeEntitySetInfo ? typeEntitySetInfo.GetEntitySetInfoForMember(expression.Property.Name) : (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbProjectExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbProjectExpression>(expression, nameof (expression));
        this.VisitExpressionBindingEnterScope(expression.Input);
        ViewValidator.DbExpressionEntitySetInfo expressionEntitySetInfo = this.VisitExpression(expression.Projection);
        this.VisitExpressionBindingExitScope();
        return expressionEntitySetInfo;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbNewInstanceExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbNewInstanceExpression>(expression, nameof (expression));
        ViewValidator.DbExpressionMemberCollectionEntitySetInfo collectionEntitySetInfo = this.VisitExpressionList(expression.Arguments);
        StructuralType edmType = expression.ResultType.EdmType as StructuralType;
        if (collectionEntitySetInfo == null || edmType == null)
          return (ViewValidator.DbExpressionEntitySetInfo) null;
        ViewValidator.DbExpressionStructuralTypeEntitySetInfo setInfos = new ViewValidator.DbExpressionStructuralTypeEntitySetInfo();
        int index = 0;
        foreach (ViewValidator.DbExpressionEntitySetInfo entitySetInfo in collectionEntitySetInfo.entitySetInfos)
        {
          setInfos.Add(edmType.Members[index].Name, entitySetInfo);
          ++index;
        }
        if (expression.ResultType.EdmType.BuiltInTypeKind == BuiltInTypeKind.AssociationType)
          this.ValidateEntitySetsMappedForAssociationSetMapping(setInfos);
        return (ViewValidator.DbExpressionEntitySetInfo) setInfos;
      }

      private ViewValidator.DbExpressionMemberCollectionEntitySetInfo VisitExpressionList(
        IList<DbExpression> list)
      {
        return new ViewValidator.DbExpressionMemberCollectionEntitySetInfo(list.Select<DbExpression, ViewValidator.DbExpressionEntitySetInfo>((Func<DbExpression, ViewValidator.DbExpressionEntitySetInfo>) (it => this.VisitExpression(it))));
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbRefExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbRefExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) new ViewValidator.DbExpressionSimpleTypeEntitySetInfo(expression.EntitySet);
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbComparisonExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbComparisonExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbLikeExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbLikeExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbLimitExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbLimitExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbIsNullExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbIsNullExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbArithmeticExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbArithmeticExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbAndExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbAndExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbOrExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbOrExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbInExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbInExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbNotExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbNotExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbDistinctExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbDistinctExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbElementExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbElementExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbIsEmptyExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbIsEmptyExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbUnionAllExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbUnionAllExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbIntersectExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbIntersectExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbExceptExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbExceptExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbTreatExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbTreatExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbIsOfExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbIsOfExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbCastExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbCastExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbCaseExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbCaseExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbOfTypeExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbOfTypeExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbRelationshipNavigationExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbRelationshipNavigationExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbDerefExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbDerefExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbRefKeyExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbRefKeyExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbEntityRefExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbEntityRefExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbScanExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbScanExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbFilterExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbFilterExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbConstantExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbConstantExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbNullExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbNullExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbCrossJoinExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbCrossJoinExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbJoinExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbJoinExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbParameterReferenceExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbParameterReferenceExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbFunctionExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbFunctionExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbLambdaExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbLambdaExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbApplyExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbApplyExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbGroupByExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbGroupByExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbSkipExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbSkipExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbSortExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbSortExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }

      public override ViewValidator.DbExpressionEntitySetInfo Visit(
        DbQuantifierExpression expression)
      {
        System.Data.Entity.Utilities.Check.NotNull<DbQuantifierExpression>(expression, nameof (expression));
        return (ViewValidator.DbExpressionEntitySetInfo) null;
      }
    }

    internal abstract class DbExpressionEntitySetInfo
    {
    }

    private class DbExpressionSimpleTypeEntitySetInfo : ViewValidator.DbExpressionEntitySetInfo
    {
      private readonly EntitySet m_entitySet;

      internal EntitySet EntitySet => this.m_entitySet;

      internal DbExpressionSimpleTypeEntitySetInfo(EntitySet entitySet) => this.m_entitySet = entitySet;
    }

    private class DbExpressionStructuralTypeEntitySetInfo : ViewValidator.DbExpressionEntitySetInfo
    {
      private readonly Dictionary<string, ViewValidator.DbExpressionEntitySetInfo> m_entitySetInfos;

      internal DbExpressionStructuralTypeEntitySetInfo() => this.m_entitySetInfos = new Dictionary<string, ViewValidator.DbExpressionEntitySetInfo>();

      internal void Add(string key, ViewValidator.DbExpressionEntitySetInfo value) => this.m_entitySetInfos.Add(key, value);

      internal IEnumerable<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>> SetInfos => (IEnumerable<KeyValuePair<string, ViewValidator.DbExpressionEntitySetInfo>>) this.m_entitySetInfos;

      internal ViewValidator.DbExpressionEntitySetInfo GetEntitySetInfoForMember(
        string memberName)
      {
        return this.m_entitySetInfos[memberName];
      }
    }

    private class DbExpressionMemberCollectionEntitySetInfo : ViewValidator.DbExpressionEntitySetInfo
    {
      private readonly IEnumerable<ViewValidator.DbExpressionEntitySetInfo> m_entitySets;

      internal DbExpressionMemberCollectionEntitySetInfo(
        IEnumerable<ViewValidator.DbExpressionEntitySetInfo> entitySetInfos)
      {
        this.m_entitySets = entitySetInfos;
      }

      internal IEnumerable<ViewValidator.DbExpressionEntitySetInfo> entitySetInfos => this.m_entitySets;
    }
  }
}
