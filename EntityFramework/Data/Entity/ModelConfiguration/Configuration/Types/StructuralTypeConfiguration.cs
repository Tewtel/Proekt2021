// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Types.StructuralTypeConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Hierarchy;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.ModelConfiguration.Utilities;
using System.Data.Entity.Spatial;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration.Types
{
  internal abstract class StructuralTypeConfiguration : ConfigurationBase
  {
    private readonly Dictionary<PropertyPath, System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration> _primitivePropertyConfigurations = new Dictionary<PropertyPath, System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>();
    private readonly HashSet<PropertyInfo> _ignoredProperties = new HashSet<PropertyInfo>();
    private readonly Type _clrType;

    internal static Type GetPropertyConfigurationType(Type propertyType)
    {
      propertyType.TryUnwrapNullableType(out propertyType);
      if (propertyType == typeof (string))
        return typeof (System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.StringPropertyConfiguration);
      if (propertyType == typeof (Decimal))
        return typeof (System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DecimalPropertyConfiguration);
      if (propertyType == typeof (DateTime) || propertyType == typeof (TimeSpan) || propertyType == typeof (DateTimeOffset))
        return typeof (System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DateTimePropertyConfiguration);
      if (propertyType == typeof (byte[]))
        return typeof (System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.BinaryPropertyConfiguration);
      return !propertyType.IsValueType() && !(propertyType == typeof (HierarchyId)) && (!(propertyType == typeof (DbGeography)) && !(propertyType == typeof (DbGeometry))) ? typeof (NavigationPropertyConfiguration) : typeof (System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration);
    }

    internal StructuralTypeConfiguration()
    {
    }

    internal StructuralTypeConfiguration(Type clrType) => this._clrType = clrType;

    internal StructuralTypeConfiguration(StructuralTypeConfiguration source)
    {
      source._primitivePropertyConfigurations.Each<KeyValuePair<PropertyPath, System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>>((Action<KeyValuePair<PropertyPath, System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>>) (c => this._primitivePropertyConfigurations.Add(c.Key, c.Value.Clone())));
      this._ignoredProperties.AddRange<PropertyInfo>((IEnumerable<PropertyInfo>) source._ignoredProperties);
      this._clrType = source._clrType;
    }

    internal virtual IEnumerable<PropertyInfo> ConfiguredProperties => this._primitivePropertyConfigurations.Keys.Select<PropertyPath, PropertyInfo>((Func<PropertyPath, PropertyInfo>) (p => p.Last<PropertyInfo>()));

    internal IEnumerable<PropertyInfo> IgnoredProperties => (IEnumerable<PropertyInfo>) this._ignoredProperties;

    internal Type ClrType => this._clrType;

    internal IEnumerable<KeyValuePair<PropertyPath, System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>> PrimitivePropertyConfigurations => (IEnumerable<KeyValuePair<PropertyPath, System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>>) this._primitivePropertyConfigurations;

    public void Ignore(PropertyInfo propertyInfo)
    {
      System.Data.Entity.Utilities.Check.NotNull<PropertyInfo>(propertyInfo, nameof (propertyInfo));
      this._ignoredProperties.Add(propertyInfo);
    }

    internal System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration Property(
      PropertyPath propertyPath,
      OverridableConfigurationParts? overridableConfigurationParts = null)
    {
      return this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>(propertyPath, (Func<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>) (() =>
      {
        System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration instance = (System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration) Activator.CreateInstance(StructuralTypeConfiguration.GetPropertyConfigurationType(propertyPath.Last<PropertyInfo>().PropertyType));
        if (overridableConfigurationParts.HasValue)
          instance.OverridableConfigurationParts = overridableConfigurationParts.Value;
        return instance;
      }));
    }

    internal virtual void RemoveProperty(PropertyPath propertyPath) => this._primitivePropertyConfigurations.Remove(propertyPath);

    internal TPrimitivePropertyConfiguration Property<TPrimitivePropertyConfiguration>(
      PropertyPath propertyPath,
      Func<TPrimitivePropertyConfiguration> primitivePropertyConfigurationCreator)
      where TPrimitivePropertyConfiguration : System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration
    {
      System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration propertyConfiguration;
      if (!this._primitivePropertyConfigurations.TryGetValue(propertyPath, out propertyConfiguration))
      {
        propertyConfiguration = (System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration) primitivePropertyConfigurationCreator();
        propertyConfiguration.TypeConfiguration = this;
        this._primitivePropertyConfigurations.Add(propertyPath, propertyConfiguration);
      }
      return (TPrimitivePropertyConfiguration) propertyConfiguration;
    }

    internal void ConfigurePropertyMappings(
      IList<Tuple<ColumnMappingBuilder, EntityType>> propertyMappings,
      DbProviderManifest providerManifest,
      bool allowOverride = false)
    {
      foreach (KeyValuePair<PropertyPath, System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration> propertyConfiguration in this.PrimitivePropertyConfigurations)
      {
        PropertyPath propertyPath = propertyConfiguration.Key;
        propertyConfiguration.Value.Configure(propertyMappings.Where<Tuple<ColumnMappingBuilder, EntityType>>((Func<Tuple<ColumnMappingBuilder, EntityType>, bool>) (pm => propertyPath.Equals(new PropertyPath(pm.Item1.PropertyPath.Skip<EdmProperty>(pm.Item1.PropertyPath.Count - propertyPath.Count).Select<EdmProperty, PropertyInfo>((Func<EdmProperty, PropertyInfo>) (p => p.GetClrPropertyInfo())))))), providerManifest, allowOverride);
      }
    }

    internal void ConfigureFunctionParameters(
      IList<ModificationFunctionParameterBinding> parameterBindings)
    {
      foreach (KeyValuePair<PropertyPath, System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration> propertyConfiguration in this.PrimitivePropertyConfigurations)
      {
        PropertyPath propertyPath = propertyConfiguration.Key;
        propertyConfiguration.Value.ConfigureFunctionParameters(parameterBindings.Where<ModificationFunctionParameterBinding>((Func<ModificationFunctionParameterBinding, bool>) (pb => pb.MemberPath.AssociationSetEnd == null && propertyPath.Equals(new PropertyPath(pb.MemberPath.Members.Skip<EdmMember>(pb.MemberPath.Members.Count - propertyPath.Count).Select<EdmMember, PropertyInfo>((Func<EdmMember, PropertyInfo>) (m => m.GetClrPropertyInfo())))))).Select<ModificationFunctionParameterBinding, FunctionParameter>((Func<ModificationFunctionParameterBinding, FunctionParameter>) (pb => pb.Parameter)));
      }
    }

    internal void Configure(
      string structuralTypeName,
      IEnumerable<EdmProperty> properties,
      ICollection<MetadataProperty> dataModelAnnotations)
    {
      dataModelAnnotations.SetConfiguration((object) this);
      foreach (KeyValuePair<PropertyPath, System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration> propertyConfiguration1 in this._primitivePropertyConfigurations)
      {
        PropertyPath key = propertyConfiguration1.Key;
        System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration propertyConfiguration2 = propertyConfiguration1.Value;
        StructuralTypeConfiguration.Configure(structuralTypeName, properties, (IEnumerable<PropertyInfo>) key, propertyConfiguration2);
      }
    }

    private static void Configure(
      string structuralTypeName,
      IEnumerable<EdmProperty> properties,
      IEnumerable<PropertyInfo> propertyPath,
      System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration propertyConfiguration)
    {
      EdmProperty property = properties.SingleOrDefault<EdmProperty>((Func<EdmProperty, bool>) (p => p.GetClrPropertyInfo().IsSameAs(propertyPath.First<PropertyInfo>())));
      if (property == null)
        throw System.Data.Entity.Resources.Error.PropertyNotFound((object) propertyPath.First<PropertyInfo>().Name, (object) structuralTypeName);
      if (property.IsUnderlyingPrimitiveType)
        propertyConfiguration.Configure(property);
      else
        StructuralTypeConfiguration.Configure(property.ComplexType.Name, (IEnumerable<EdmProperty>) property.ComplexType.Properties, (IEnumerable<PropertyInfo>) new PropertyPath(propertyPath.Skip<PropertyInfo>(1)), propertyConfiguration);
    }
  }
}
