// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Validation.EntityValidatorBuilder
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Utilities;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Internal.Validation
{
  internal class EntityValidatorBuilder
  {
    private readonly AttributeProvider _attributeProvider;

    public EntityValidatorBuilder(AttributeProvider attributeProvider) => this._attributeProvider = attributeProvider;

    public virtual EntityValidator BuildEntityValidator(
      InternalEntityEntry entityEntry)
    {
      return this.BuildTypeValidator<EntityValidator>(entityEntry.EntityType, (IEnumerable<EdmProperty>) entityEntry.EdmEntityType.Properties, (IEnumerable<NavigationProperty>) entityEntry.EdmEntityType.NavigationProperties, (Func<IEnumerable<PropertyValidator>, IEnumerable<IValidator>, EntityValidator>) ((propertyValidators, typeLevelValidators) => new EntityValidator(propertyValidators, typeLevelValidators)));
    }

    protected virtual ComplexTypeValidator BuildComplexTypeValidator(
      Type clrType,
      ComplexType complexType)
    {
      return this.BuildTypeValidator<ComplexTypeValidator>(clrType, (IEnumerable<EdmProperty>) complexType.Properties, Enumerable.Empty<NavigationProperty>(), (Func<IEnumerable<PropertyValidator>, IEnumerable<IValidator>, ComplexTypeValidator>) ((propertyValidators, typeLevelValidators) => new ComplexTypeValidator(propertyValidators, typeLevelValidators)));
    }

    private T BuildTypeValidator<T>(
      Type clrType,
      IEnumerable<EdmProperty> edmProperties,
      IEnumerable<NavigationProperty> navigationProperties,
      Func<IEnumerable<PropertyValidator>, IEnumerable<IValidator>, T> validatorFactoryFunc)
      where T : TypeValidator
    {
      IList<PropertyValidator> source1 = this.BuildValidatorsForProperties(this.GetPublicInstanceProperties(clrType), edmProperties, navigationProperties);
      IEnumerable<Attribute> attributes = this._attributeProvider.GetAttributes(clrType);
      IList<IValidator> source2 = this.BuildValidationAttributeValidators(attributes);
      if (typeof (IValidatableObject).IsAssignableFrom(clrType))
        source2.Add((IValidator) new ValidatableObjectValidator(attributes.OfType<DisplayAttribute>().SingleOrDefault<DisplayAttribute>()));
      return !source1.Any<PropertyValidator>() && !source2.Any<IValidator>() ? default (T) : validatorFactoryFunc((IEnumerable<PropertyValidator>) source1, (IEnumerable<IValidator>) source2);
    }

    protected virtual IList<PropertyValidator> BuildValidatorsForProperties(
      IEnumerable<PropertyInfo> clrProperties,
      IEnumerable<EdmProperty> edmProperties,
      IEnumerable<NavigationProperty> navigationProperties)
    {
      List<PropertyValidator> propertyValidatorList = new List<PropertyValidator>();
      foreach (PropertyInfo clrProperty in clrProperties)
      {
        PropertyInfo property = clrProperty;
        EdmProperty edmProperty = edmProperties.Where<EdmProperty>((Func<EdmProperty, bool>) (p => p.Name == property.Name)).SingleOrDefault<EdmProperty>();
        PropertyValidator propertyValidator;
        if (edmProperty != null)
        {
          IEnumerable<ReferentialConstraint> source = navigationProperties.Select(navigationProperty => new
          {
            navigationProperty = navigationProperty,
            associationType = navigationProperty.RelationshipType as AssociationType
          }).Where(_param1 => _param1.associationType != null).SelectMany(_param1 => (IEnumerable<ReferentialConstraint>) _param1.associationType.ReferentialConstraints, (_param1, constraint) => new
          {
            \u003C\u003Eh__TransparentIdentifier0 = _param1,
            constraint = constraint
          }).Where(_param1 => _param1.constraint.ToProperties.Contains(edmProperty)).Select(_param1 => _param1.constraint);
          propertyValidator = this.BuildPropertyValidator(property, edmProperty, !source.Any<ReferentialConstraint>());
        }
        else
          propertyValidator = this.BuildPropertyValidator(property);
        if (propertyValidator != null)
          propertyValidatorList.Add(propertyValidator);
      }
      return (IList<PropertyValidator>) propertyValidatorList;
    }

    protected virtual PropertyValidator BuildPropertyValidator(
      PropertyInfo clrProperty,
      EdmProperty edmProperty,
      bool buildFacetValidators)
    {
      List<IValidator> source = new List<IValidator>();
      IEnumerable<Attribute> attributes = this._attributeProvider.GetAttributes(clrProperty);
      source.AddRange((IEnumerable<IValidator>) this.BuildValidationAttributeValidators(attributes));
      if (edmProperty.TypeUsage.EdmType.BuiltInTypeKind == BuiltInTypeKind.ComplexType)
      {
        ComplexType edmType = (ComplexType) edmProperty.TypeUsage.EdmType;
        ComplexTypeValidator complexTypeValidator = this.BuildComplexTypeValidator(clrProperty.PropertyType, edmType);
        return !source.Any<IValidator>() && complexTypeValidator == null ? (PropertyValidator) null : (PropertyValidator) new ComplexPropertyValidator(clrProperty.Name, (IEnumerable<IValidator>) source, complexTypeValidator);
      }
      if (buildFacetValidators)
        source.AddRange(this.BuildFacetValidators(clrProperty, (EdmMember) edmProperty, attributes));
      return !source.Any<IValidator>() ? (PropertyValidator) null : new PropertyValidator(clrProperty.Name, (IEnumerable<IValidator>) source);
    }

    protected virtual PropertyValidator BuildPropertyValidator(
      PropertyInfo clrProperty)
    {
      IList<IValidator> validatorList = this.BuildValidationAttributeValidators(this._attributeProvider.GetAttributes(clrProperty));
      return validatorList.Count <= 0 ? (PropertyValidator) null : new PropertyValidator(clrProperty.Name, (IEnumerable<IValidator>) validatorList);
    }

    protected virtual IList<IValidator> BuildValidationAttributeValidators(
      IEnumerable<Attribute> attributes)
    {
      return (IList<IValidator>) ((IEnumerable<IValidator>) attributes.Where<Attribute>((Func<Attribute, bool>) (validationAttribute => validationAttribute is ValidationAttribute)).Select<Attribute, ValidationAttributeValidator>((Func<Attribute, ValidationAttributeValidator>) (validationAttribute => new ValidationAttributeValidator((ValidationAttribute) validationAttribute, attributes.OfType<DisplayAttribute>().SingleOrDefault<DisplayAttribute>())))).ToList<IValidator>();
    }

    protected virtual IEnumerable<PropertyInfo> GetPublicInstanceProperties(
      Type type)
    {
      return type.GetInstanceProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.IsPublic() && p.GetIndexParameters().Length == 0 && p.Getter() != (MethodInfo) null));
    }

    protected virtual IEnumerable<IValidator> BuildFacetValidators(
      PropertyInfo clrProperty,
      EdmMember edmProperty,
      IEnumerable<Attribute> existingAttributes)
    {
      List<ValidationAttribute> source = new List<ValidationAttribute>();
      MetadataProperty metadataProperty;
      edmProperty.MetadataProperties.TryGetValue("http://schemas.microsoft.com/ado/2009/02/edm/annotation:StoreGeneratedPattern", false, out metadataProperty);
      bool flag = metadataProperty != null && metadataProperty.Value != null;
      Facet facet1;
      edmProperty.TypeUsage.Facets.TryGetValue("Nullable", false, out facet1);
      if ((facet1 == null || facet1.Value == null ? 0 : (!(bool) facet1.Value ? 1 : 0)) != 0 && !flag && clrProperty.PropertyType.IsNullable() && !existingAttributes.Any<Attribute>((Func<Attribute, bool>) (a => a is RequiredAttribute)))
        source.Add((ValidationAttribute) new RequiredAttribute()
        {
          AllowEmptyStrings = true
        });
      Facet facet2;
      edmProperty.TypeUsage.Facets.TryGetValue("MaxLength", false, out facet2);
      if (facet2 != null && facet2.Value != null && facet2.Value is int && !existingAttributes.Any<Attribute>((Func<Attribute, bool>) (a => a is MaxLengthAttribute)) && !existingAttributes.Any<Attribute>((Func<Attribute, bool>) (a => a is StringLengthAttribute)))
        source.Add((ValidationAttribute) new MaxLengthAttribute((int) facet2.Value));
      return (IEnumerable<IValidator>) source.Select<ValidationAttribute, ValidationAttributeValidator>((Func<ValidationAttribute, ValidationAttributeValidator>) (attribute => new ValidationAttributeValidator(attribute, existingAttributes.OfType<DisplayAttribute>().SingleOrDefault<DisplayAttribute>())));
    }
  }
}
