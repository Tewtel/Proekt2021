// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.EdmModelSyntacticValidationRules
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal static class EdmModelSyntacticValidationRules
  {
    internal static readonly EdmModelValidationRule<INamedDataModelItem> EdmModel_NameMustNotBeEmptyOrWhiteSpace = new EdmModelValidationRule<INamedDataModelItem>((Action<EdmModelValidationContext, INamedDataModelItem>) ((context, item) =>
    {
      if (!string.IsNullOrWhiteSpace(item.Name))
        return;
      context.AddError((MetadataItem) item, "Name", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_MissingName);
    }));
    internal static readonly EdmModelValidationRule<INamedDataModelItem> EdmModel_NameIsTooLong = new EdmModelValidationRule<INamedDataModelItem>((Action<EdmModelValidationContext, INamedDataModelItem>) ((context, item) =>
    {
      if (string.IsNullOrWhiteSpace(item.Name) || item.Name.Length <= 480)
        return;
      switch (item)
      {
        case RowType _:
          break;
        case CollectionType _:
          break;
        default:
          context.AddError((MetadataItem) item, "Name", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmModel_NameIsTooLong((object) item.Name));
          break;
      }
    }));
    internal static readonly EdmModelValidationRule<INamedDataModelItem> EdmModel_NameIsNotAllowed = new EdmModelValidationRule<INamedDataModelItem>((Action<EdmModelValidationContext, INamedDataModelItem>) ((context, item) =>
    {
      if (string.IsNullOrWhiteSpace(item.Name) || item is RowType || item is CollectionType || !context.IsCSpace && item is EdmProperty || !item.Name.Contains(".") && (!context.IsCSpace || item.Name.IsValidUndottedName()))
        return;
      context.AddError((MetadataItem) item, "Name", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmModel_NameIsNotAllowed((object) item.Name));
    }));
    internal static readonly EdmModelValidationRule<AssociationType> EdmAssociationType_AssociationEndMustNotBeNull = new EdmModelValidationRule<AssociationType>((Action<EdmModelValidationContext, AssociationType>) ((context, edmAssociationType) =>
    {
      if (edmAssociationType.SourceEnd != null && edmAssociationType.TargetEnd != null)
        return;
      context.AddError((MetadataItem) edmAssociationType, "End", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmAssociationType_AssociationEndMustNotBeNull);
    }));
    internal static readonly EdmModelValidationRule<ReferentialConstraint> EdmAssociationConstraint_DependentEndMustNotBeNull = new EdmModelValidationRule<ReferentialConstraint>((Action<EdmModelValidationContext, ReferentialConstraint>) ((context, edmAssociationConstraint) =>
    {
      if (edmAssociationConstraint.ToRole != null)
        return;
      context.AddError((MetadataItem) edmAssociationConstraint, "Dependent", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmAssociationConstraint_DependentEndMustNotBeNull);
    }));
    internal static readonly EdmModelValidationRule<ReferentialConstraint> EdmAssociationConstraint_DependentPropertiesMustNotBeEmpty = new EdmModelValidationRule<ReferentialConstraint>((Action<EdmModelValidationContext, ReferentialConstraint>) ((context, edmAssociationConstraint) =>
    {
      if (edmAssociationConstraint.ToProperties != null && edmAssociationConstraint.ToProperties.Any<EdmProperty>())
        return;
      context.AddError((MetadataItem) edmAssociationConstraint, "Dependent", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmAssociationConstraint_DependentPropertiesMustNotBeEmpty);
    }));
    internal static readonly EdmModelValidationRule<NavigationProperty> EdmNavigationProperty_AssociationMustNotBeNull = new EdmModelValidationRule<NavigationProperty>((Action<EdmModelValidationContext, NavigationProperty>) ((context, edmNavigationProperty) =>
    {
      if (edmNavigationProperty.Association != null)
        return;
      context.AddError((MetadataItem) edmNavigationProperty, "Relationship", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmNavigationProperty_AssociationMustNotBeNull);
    }));
    internal static readonly EdmModelValidationRule<NavigationProperty> EdmNavigationProperty_ResultEndMustNotBeNull = new EdmModelValidationRule<NavigationProperty>((Action<EdmModelValidationContext, NavigationProperty>) ((context, edmNavigationProperty) =>
    {
      if (edmNavigationProperty.ToEndMember != null)
        return;
      context.AddError((MetadataItem) edmNavigationProperty, "ToRole", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmNavigationProperty_ResultEndMustNotBeNull);
    }));
    internal static readonly EdmModelValidationRule<AssociationEndMember> EdmAssociationEnd_EntityTypeMustNotBeNull = new EdmModelValidationRule<AssociationEndMember>((Action<EdmModelValidationContext, AssociationEndMember>) ((context, edmAssociationEnd) =>
    {
      if (edmAssociationEnd.GetEntityType() != null)
        return;
      context.AddError((MetadataItem) edmAssociationEnd, "Type", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmAssociationEnd_EntityTypeMustNotBeNull);
    }));
    internal static readonly EdmModelValidationRule<EntitySet> EdmEntitySet_ElementTypeMustNotBeNull = new EdmModelValidationRule<EntitySet>((Action<EdmModelValidationContext, EntitySet>) ((context, edmEntitySet) =>
    {
      if (edmEntitySet.ElementType != null)
        return;
      context.AddError((MetadataItem) edmEntitySet, "ElementType", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmEntitySet_ElementTypeMustNotBeNull);
    }));
    internal static readonly EdmModelValidationRule<AssociationSet> EdmAssociationSet_ElementTypeMustNotBeNull = new EdmModelValidationRule<AssociationSet>((Action<EdmModelValidationContext, AssociationSet>) ((context, edmAssociationSet) =>
    {
      if (edmAssociationSet.ElementType != null)
        return;
      context.AddError((MetadataItem) edmAssociationSet, "ElementType", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmAssociationSet_ElementTypeMustNotBeNull);
    }));
    internal static readonly EdmModelValidationRule<AssociationSet> EdmAssociationSet_SourceSetMustNotBeNull = new EdmModelValidationRule<AssociationSet>((Action<EdmModelValidationContext, AssociationSet>) ((context, edmAssociationSet) =>
    {
      if (!context.IsCSpace || edmAssociationSet.SourceSet != null)
        return;
      context.AddError((MetadataItem) edmAssociationSet, "FromRole", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmAssociationSet_SourceSetMustNotBeNull);
    }));
    internal static readonly EdmModelValidationRule<AssociationSet> EdmAssociationSet_TargetSetMustNotBeNull = new EdmModelValidationRule<AssociationSet>((Action<EdmModelValidationContext, AssociationSet>) ((context, edmAssociationSet) =>
    {
      if (!context.IsCSpace || edmAssociationSet.TargetSet != null)
        return;
      context.AddError((MetadataItem) edmAssociationSet, "ToRole", System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmAssociationSet_TargetSetMustNotBeNull);
    }));
    internal static readonly EdmModelValidationRule<TypeUsage> EdmTypeReference_TypeNotValid = new EdmModelValidationRule<TypeUsage>((Action<EdmModelValidationContext, TypeUsage>) ((context, edmTypeReference) =>
    {
      if (EdmModelSyntacticValidationRules.IsEdmTypeUsageValid(edmTypeReference))
        return;
      context.AddError((MetadataItem) edmTypeReference, (string) null, System.Data.Entity.Resources.Strings.EdmModel_Validator_Syntactic_EdmTypeReferenceNotValid);
    }));

    private static bool IsEdmTypeUsageValid(TypeUsage typeUsage)
    {
      HashSet<TypeUsage> visitedValidTypeUsages = new HashSet<TypeUsage>();
      return EdmModelSyntacticValidationRules.IsEdmTypeUsageValid(typeUsage, visitedValidTypeUsages);
    }

    private static bool IsEdmTypeUsageValid(
      TypeUsage typeUsage,
      HashSet<TypeUsage> visitedValidTypeUsages)
    {
      if (visitedValidTypeUsages.Contains(typeUsage))
        return false;
      visitedValidTypeUsages.Add(typeUsage);
      return true;
    }
  }
}
