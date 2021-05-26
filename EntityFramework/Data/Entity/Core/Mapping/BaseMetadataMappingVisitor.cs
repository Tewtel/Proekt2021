// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.BaseMetadataMappingVisitor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Mapping
{
  internal abstract class BaseMetadataMappingVisitor
  {
    private readonly bool _sortSequence;

    protected BaseMetadataMappingVisitor(bool sortSequence) => this._sortSequence = sortSequence;

    protected virtual void Visit(EntityContainerMapping entityContainerMapping)
    {
      this.Visit(entityContainerMapping.EdmEntityContainer);
      this.Visit(entityContainerMapping.StorageEntityContainer);
      foreach (EntitySetBaseMapping setMapping in this.GetSequence<EntitySetBaseMapping>((IEnumerable<EntitySetBaseMapping>) entityContainerMapping.EntitySetMaps, (Func<EntitySetBaseMapping, string>) (it => BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(it))))
        this.Visit(setMapping);
    }

    protected virtual void Visit(EntitySetBase entitySetBase)
    {
      switch (entitySetBase.BuiltInTypeKind)
      {
        case BuiltInTypeKind.AssociationSet:
          this.Visit((AssociationSet) entitySetBase);
          break;
        case BuiltInTypeKind.EntitySet:
          this.Visit((EntitySet) entitySetBase);
          break;
      }
    }

    protected virtual void Visit(EntitySetBaseMapping setMapping)
    {
      foreach (TypeMapping typeMapping in this.GetSequence<TypeMapping>(setMapping.TypeMappings, (Func<TypeMapping, string>) (it => BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(it))))
        this.Visit(typeMapping);
      this.Visit(setMapping.EntityContainerMapping);
    }

    protected virtual void Visit(EntityContainer entityContainer)
    {
      foreach (EntitySetBase entitySetBase in this.GetSequence<EntitySetBase>((IEnumerable<EntitySetBase>) entityContainer.BaseEntitySets, (Func<EntitySetBase, string>) (it => it.Identity)))
        this.Visit(entitySetBase);
    }

    protected virtual void Visit(EntitySet entitySet)
    {
      this.Visit(entitySet.ElementType);
      this.Visit(entitySet.EntityContainer);
    }

    protected virtual void Visit(AssociationSet associationSet)
    {
      this.Visit(associationSet.ElementType);
      this.Visit(associationSet.EntityContainer);
      foreach (AssociationSetEnd associationSetEnd in this.GetSequence<AssociationSetEnd>((IEnumerable<AssociationSetEnd>) associationSet.AssociationSetEnds, (Func<AssociationSetEnd, string>) (it => it.Identity)))
        this.Visit(associationSetEnd);
    }

    protected virtual void Visit(EntityType entityType)
    {
      foreach (EdmMember edmMember in this.GetSequence<EdmMember>((IEnumerable<EdmMember>) entityType.KeyMembers, (Func<EdmMember, string>) (it => it.Identity)))
        this.Visit(edmMember);
      foreach (EdmMember edmMember in this.GetSequence<EdmMember>((IEnumerable<EdmMember>) entityType.GetDeclaredOnlyMembers<EdmMember>(), (Func<EdmMember, string>) (it => it.Identity)))
        this.Visit(edmMember);
      foreach (NavigationProperty navigationProperty in this.GetSequence<NavigationProperty>((IEnumerable<NavigationProperty>) entityType.NavigationProperties, (Func<NavigationProperty, string>) (it => it.Identity)))
        this.Visit(navigationProperty);
      foreach (EdmProperty edmProperty in this.GetSequence<EdmProperty>((IEnumerable<EdmProperty>) entityType.Properties, (Func<EdmProperty, string>) (it => it.Identity)))
        this.Visit(edmProperty);
    }

    protected virtual void Visit(AssociationType associationType)
    {
      foreach (AssociationEndMember associationEndMember in this.GetSequence<AssociationEndMember>((IEnumerable<AssociationEndMember>) associationType.AssociationEndMembers, (Func<AssociationEndMember, string>) (it => it.Identity)))
        this.Visit(associationEndMember);
      this.Visit(associationType.BaseType);
      foreach (EdmMember edmMember in this.GetSequence<EdmMember>((IEnumerable<EdmMember>) associationType.KeyMembers, (Func<EdmMember, string>) (it => it.Identity)))
        this.Visit(edmMember);
      foreach (EdmMember edmMember in this.GetSequence<EdmMember>((IEnumerable<EdmMember>) associationType.GetDeclaredOnlyMembers<EdmMember>(), (Func<EdmMember, string>) (it => it.Identity)))
        this.Visit(edmMember);
      foreach (ReferentialConstraint referentialConstraint in this.GetSequence<ReferentialConstraint>((IEnumerable<ReferentialConstraint>) associationType.ReferentialConstraints, (Func<ReferentialConstraint, string>) (it => it.Identity)))
        this.Visit(referentialConstraint);
      foreach (RelationshipEndMember relationshipEndMember in this.GetSequence<RelationshipEndMember>((IEnumerable<RelationshipEndMember>) associationType.RelationshipEndMembers, (Func<RelationshipEndMember, string>) (it => it.Identity)))
        this.Visit(relationshipEndMember);
    }

    protected virtual void Visit(AssociationSetEnd associationSetEnd)
    {
      this.Visit(associationSetEnd.CorrespondingAssociationEndMember);
      this.Visit(associationSetEnd.EntitySet);
      this.Visit(associationSetEnd.ParentAssociationSet);
    }

    protected virtual void Visit(EdmProperty edmProperty) => this.Visit(edmProperty.TypeUsage);

    protected virtual void Visit(NavigationProperty navigationProperty)
    {
      this.Visit(navigationProperty.FromEndMember);
      this.Visit(navigationProperty.RelationshipType);
      this.Visit(navigationProperty.ToEndMember);
      this.Visit(navigationProperty.TypeUsage);
    }

    protected virtual void Visit(EdmMember edmMember) => this.Visit(edmMember.TypeUsage);

    protected virtual void Visit(AssociationEndMember associationEndMember) => this.Visit(associationEndMember.TypeUsage);

    protected virtual void Visit(ReferentialConstraint referentialConstraint)
    {
      foreach (EdmProperty edmProperty in this.GetSequence<EdmProperty>((IEnumerable<EdmProperty>) referentialConstraint.FromProperties, (Func<EdmProperty, string>) (it => it.Identity)))
        this.Visit(edmProperty);
      this.Visit(referentialConstraint.FromRole);
      foreach (EdmProperty edmProperty in this.GetSequence<EdmProperty>((IEnumerable<EdmProperty>) referentialConstraint.ToProperties, (Func<EdmProperty, string>) (it => it.Identity)))
        this.Visit(edmProperty);
      this.Visit(referentialConstraint.ToRole);
    }

    protected virtual void Visit(RelationshipEndMember relationshipEndMember) => this.Visit(relationshipEndMember.TypeUsage);

    protected virtual void Visit(TypeUsage typeUsage)
    {
      this.Visit(typeUsage.EdmType);
      foreach (Facet facet in this.GetSequence<Facet>((IEnumerable<Facet>) typeUsage.Facets, (Func<Facet, string>) (it => it.Identity)))
        this.Visit(facet);
    }

    protected virtual void Visit(RelationshipType relationshipType)
    {
      if (relationshipType == null || relationshipType.BuiltInTypeKind != BuiltInTypeKind.AssociationType)
        return;
      this.Visit((AssociationType) relationshipType);
    }

    protected virtual void Visit(EdmType edmType)
    {
      if (edmType == null)
        return;
      switch (edmType.BuiltInTypeKind)
      {
        case BuiltInTypeKind.AssociationType:
          this.Visit((AssociationType) edmType);
          break;
        case BuiltInTypeKind.CollectionType:
          this.Visit((CollectionType) edmType);
          break;
        case BuiltInTypeKind.ComplexType:
          this.Visit((ComplexType) edmType);
          break;
        case BuiltInTypeKind.EntityType:
          this.Visit((EntityType) edmType);
          break;
        case BuiltInTypeKind.EnumType:
          this.Visit((EnumType) edmType);
          break;
        case BuiltInTypeKind.EdmFunction:
          this.Visit((EdmFunction) edmType);
          break;
        case BuiltInTypeKind.PrimitiveType:
          this.Visit((PrimitiveType) edmType);
          break;
        case BuiltInTypeKind.RefType:
          this.Visit((RefType) edmType);
          break;
      }
    }

    protected virtual void Visit(Facet facet) => this.Visit(facet.FacetType);

    protected virtual void Visit(EdmFunction edmFunction)
    {
      this.Visit(edmFunction.BaseType);
      foreach (EntitySet entitySet in this.GetSequence<EntitySet>((IEnumerable<EntitySet>) edmFunction.EntitySets, (Func<EntitySet, string>) (it => it.Identity)))
      {
        if (entitySet != null)
          this.Visit(entitySet);
      }
      foreach (FunctionParameter functionParameter in this.GetSequence<FunctionParameter>((IEnumerable<FunctionParameter>) edmFunction.Parameters, (Func<FunctionParameter, string>) (it => it.Identity)))
        this.Visit(functionParameter);
      foreach (FunctionParameter functionParameter in this.GetSequence<FunctionParameter>((IEnumerable<FunctionParameter>) edmFunction.ReturnParameters, (Func<FunctionParameter, string>) (it => it.Identity)))
        this.Visit(functionParameter);
    }

    protected virtual void Visit(PrimitiveType primitiveType)
    {
    }

    protected virtual void Visit(ComplexType complexType)
    {
      this.Visit(complexType.BaseType);
      foreach (EdmMember edmMember in this.GetSequence<EdmMember>((IEnumerable<EdmMember>) complexType.Members, (Func<EdmMember, string>) (it => it.Identity)))
        this.Visit(edmMember);
      foreach (EdmProperty edmProperty in this.GetSequence<EdmProperty>((IEnumerable<EdmProperty>) complexType.Properties, (Func<EdmProperty, string>) (it => it.Identity)))
        this.Visit(edmProperty);
    }

    protected virtual void Visit(RefType refType)
    {
      this.Visit(refType.BaseType);
      this.Visit(refType.ElementType);
    }

    protected virtual void Visit(EnumType enumType)
    {
      foreach (EnumMember enumMember in this.GetSequence<EnumMember>((IEnumerable<EnumMember>) enumType.Members, (Func<EnumMember, string>) (it => it.Identity)))
        this.Visit(enumMember);
    }

    protected virtual void Visit(EnumMember enumMember)
    {
    }

    protected virtual void Visit(CollectionType collectionType)
    {
      this.Visit(collectionType.BaseType);
      this.Visit(collectionType.TypeUsage);
    }

    protected virtual void Visit(EntityTypeBase entityTypeBase)
    {
      if (entityTypeBase == null)
        return;
      switch (entityTypeBase.BuiltInTypeKind)
      {
        case BuiltInTypeKind.AssociationType:
          this.Visit((AssociationType) entityTypeBase);
          break;
        case BuiltInTypeKind.EntityType:
          this.Visit((EntityType) entityTypeBase);
          break;
      }
    }

    protected virtual void Visit(FunctionParameter functionParameter)
    {
      this.Visit(functionParameter.DeclaringFunction);
      this.Visit(functionParameter.TypeUsage);
    }

    protected virtual void Visit(DbProviderManifest providerManifest)
    {
    }

    protected virtual void Visit(TypeMapping typeMapping)
    {
      foreach (EntityTypeBase entityTypeBase in this.GetSequence<EntityTypeBase>((IEnumerable<EntityTypeBase>) typeMapping.IsOfTypes, (Func<EntityTypeBase, string>) (it => it.Identity)))
        this.Visit(entityTypeBase);
      foreach (MappingFragment mappingFragment in this.GetSequence<MappingFragment>((IEnumerable<MappingFragment>) typeMapping.MappingFragments, (Func<MappingFragment, string>) (it => BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(it))))
        this.Visit(mappingFragment);
      this.Visit(typeMapping.SetMapping);
      foreach (EntityTypeBase entityTypeBase in this.GetSequence<EntityTypeBase>((IEnumerable<EntityTypeBase>) typeMapping.Types, (Func<EntityTypeBase, string>) (it => it.Identity)))
        this.Visit(entityTypeBase);
    }

    protected virtual void Visit(MappingFragment mappingFragment)
    {
      foreach (PropertyMapping propertyMapping in this.GetSequence<PropertyMapping>((IEnumerable<PropertyMapping>) mappingFragment.AllProperties, (Func<PropertyMapping, string>) (it => BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(it))))
        this.Visit(propertyMapping);
      this.Visit((EntitySetBase) mappingFragment.TableSet);
    }

    protected virtual void Visit(PropertyMapping propertyMapping)
    {
      if (propertyMapping.GetType() == typeof (ComplexPropertyMapping))
        this.Visit((ComplexPropertyMapping) propertyMapping);
      else if (propertyMapping.GetType() == typeof (ConditionPropertyMapping))
      {
        this.Visit((ConditionPropertyMapping) propertyMapping);
      }
      else
      {
        if (!(propertyMapping.GetType() == typeof (ScalarPropertyMapping)))
          return;
        this.Visit((ScalarPropertyMapping) propertyMapping);
      }
    }

    protected virtual void Visit(ComplexPropertyMapping complexPropertyMapping)
    {
      this.Visit(complexPropertyMapping.Property);
      foreach (ComplexTypeMapping complexTypeMapping in this.GetSequence<ComplexTypeMapping>((IEnumerable<ComplexTypeMapping>) complexPropertyMapping.TypeMappings, (Func<ComplexTypeMapping, string>) (it => BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(it))))
        this.Visit(complexTypeMapping);
    }

    protected virtual void Visit(ConditionPropertyMapping conditionPropertyMapping)
    {
      this.Visit(conditionPropertyMapping.Column);
      this.Visit(conditionPropertyMapping.Property);
    }

    protected virtual void Visit(ScalarPropertyMapping scalarPropertyMapping)
    {
      this.Visit(scalarPropertyMapping.Column);
      this.Visit(scalarPropertyMapping.Property);
    }

    protected virtual void Visit(ComplexTypeMapping complexTypeMapping)
    {
      foreach (PropertyMapping propertyMapping in this.GetSequence<PropertyMapping>((IEnumerable<PropertyMapping>) complexTypeMapping.AllProperties, (Func<PropertyMapping, string>) (it => BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(it))))
        this.Visit(propertyMapping);
      foreach (ComplexType complexType in this.GetSequence<ComplexType>((IEnumerable<ComplexType>) complexTypeMapping.IsOfTypes, (Func<ComplexType, string>) (it => it.Identity)))
        this.Visit(complexType);
      foreach (ComplexType complexType in this.GetSequence<ComplexType>((IEnumerable<ComplexType>) complexTypeMapping.Types, (Func<ComplexType, string>) (it => it.Identity)))
        this.Visit(complexType);
    }

    protected IEnumerable<T> GetSequence<T>(
      IEnumerable<T> sequence,
      Func<T, string> keySelector)
    {
      return !this._sortSequence ? sequence : (IEnumerable<T>) sequence.OrderBy<T, string>(keySelector, (IComparer<string>) StringComparer.Ordinal);
    }

    internal static class IdentityHelper
    {
      public static string GetIdentity(EntitySetBaseMapping mapping) => mapping.Set.Identity;

      public static string GetIdentity(TypeMapping mapping) => mapping is EntityTypeMapping mapping1 ? BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(mapping1) : BaseMetadataMappingVisitor.IdentityHelper.GetIdentity((AssociationTypeMapping) mapping);

      public static string GetIdentity(EntityTypeMapping mapping) => string.Join(",", mapping.Types.Select<EntityTypeBase, string>((Func<EntityTypeBase, string>) (it => it.Identity)).OrderBy<string, string>((Func<string, string>) (it => it), (IComparer<string>) StringComparer.Ordinal).Concat<string>((IEnumerable<string>) mapping.IsOfTypes.Select<EntityTypeBase, string>((Func<EntityTypeBase, string>) (it => it.Identity)).OrderBy<string, string>((Func<string, string>) (it => it), (IComparer<string>) StringComparer.Ordinal)));

      public static string GetIdentity(AssociationTypeMapping mapping) => mapping.AssociationType.Identity;

      public static string GetIdentity(ComplexTypeMapping mapping)
      {
        IOrderedEnumerable<string> first = mapping.AllProperties.Select<PropertyMapping, string>((Func<PropertyMapping, string>) (it => BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(it))).OrderBy<string, string>((Func<string, string>) (it => it), (IComparer<string>) StringComparer.Ordinal);
        IOrderedEnumerable<string> orderedEnumerable1 = mapping.Types.Select<ComplexType, string>((Func<ComplexType, string>) (it => it.Identity)).OrderBy<string, string>((Func<string, string>) (it => it), (IComparer<string>) StringComparer.Ordinal);
        IOrderedEnumerable<string> orderedEnumerable2 = mapping.IsOfTypes.Select<ComplexType, string>((Func<ComplexType, string>) (it => it.Identity)).OrderBy<string, string>((Func<string, string>) (it => it), (IComparer<string>) StringComparer.Ordinal);
        return string.Join(",", first.Concat<string>((IEnumerable<string>) orderedEnumerable1).Concat<string>((IEnumerable<string>) orderedEnumerable2));
      }

      public static string GetIdentity(MappingFragment mapping) => mapping.TableSet.Identity;

      public static string GetIdentity(PropertyMapping mapping)
      {
        switch (mapping)
        {
          case ScalarPropertyMapping mapping1:
            return BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(mapping1);
          case ComplexPropertyMapping mapping2:
            return BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(mapping2);
          case EndPropertyMapping mapping3:
            return BaseMetadataMappingVisitor.IdentityHelper.GetIdentity(mapping3);
          default:
            return BaseMetadataMappingVisitor.IdentityHelper.GetIdentity((ConditionPropertyMapping) mapping);
        }
      }

      public static string GetIdentity(ScalarPropertyMapping mapping) => "ScalarProperty(Identity=" + mapping.Property.Identity + ",ColumnIdentity=" + mapping.Column.Identity + ")";

      public static string GetIdentity(ComplexPropertyMapping mapping) => "ComplexProperty(Identity=" + mapping.Property.Identity + ")";

      public static string GetIdentity(ConditionPropertyMapping mapping) => mapping.Property == null ? "ConditionProperty(ColumnIdentity=" + mapping.Column.Identity + ")" : "ConditionProperty(Identity=" + mapping.Property.Identity + ")";

      public static string GetIdentity(EndPropertyMapping mapping) => "EndProperty(Identity=" + mapping.AssociationEnd.Identity + ")";
    }
  }
}
