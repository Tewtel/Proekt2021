// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Mappers.PropertyFilter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Hierarchy;
using System.Data.Entity.Spatial;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Mappers
{
  internal sealed class PropertyFilter
  {
    private readonly DbModelBuilderVersion _modelBuilderVersion;

    public PropertyFilter(DbModelBuilderVersion modelBuilderVersion = DbModelBuilderVersion.Latest) => this._modelBuilderVersion = modelBuilderVersion;

    public IEnumerable<PropertyInfo> GetProperties(
      Type type,
      bool declaredOnly,
      IEnumerable<PropertyInfo> explicitlyMappedProperties = null,
      IEnumerable<Type> knownTypes = null,
      bool includePrivate = false)
    {
      explicitlyMappedProperties = explicitlyMappedProperties ?? Enumerable.Empty<PropertyInfo>();
      knownTypes = knownTypes ?? Enumerable.Empty<Type>();
      this.ValidatePropertiesForModelVersion(type, explicitlyMappedProperties);
      return (declaredOnly ? type.GetDeclaredProperties() : type.GetNonHiddenProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => !p.IsStatic() && p.IsValidStructuralProperty())).Select(p => new
      {
        p = p,
        m = p.Getter()
      }).Where(_param1 =>
      {
        if (!includePrivate && !_param1.m.IsPublic && (!explicitlyMappedProperties.Contains<PropertyInfo>(_param1.p) && !knownTypes.Contains<Type>(_param1.p.PropertyType)) || declaredOnly && !type.BaseType().GetInstanceProperties().All<PropertyInfo>((Func<PropertyInfo, bool>) (bp => bp.Name != _param1.p.Name)) || !this.EdmV3FeaturesSupported && (PropertyFilter.IsEnumType(_param1.p.PropertyType) || PropertyFilter.IsSpatialType(_param1.p.PropertyType) || PropertyFilter.IsHierarchyIdType(_param1.p.PropertyType)))
          return false;
        return this.Ef6FeaturesSupported || !_param1.p.PropertyType.IsNested;
      }).Select(_param1 => _param1.p);
    }

    public void ValidatePropertiesForModelVersion(
      Type type,
      IEnumerable<PropertyInfo> explicitlyMappedProperties)
    {
      if (this._modelBuilderVersion == DbModelBuilderVersion.Latest || this.EdmV3FeaturesSupported)
        return;
      PropertyInfo propertyInfo = explicitlyMappedProperties.FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (p => PropertyFilter.IsEnumType(p.PropertyType) || PropertyFilter.IsSpatialType(p.PropertyType) || PropertyFilter.IsHierarchyIdType(p.PropertyType)));
      if (propertyInfo != (PropertyInfo) null)
        throw System.Data.Entity.Resources.Error.UnsupportedUseOfV3Type((object) type.Name, (object) propertyInfo.Name);
    }

    public bool EdmV3FeaturesSupported => this._modelBuilderVersion.GetEdmVersion() >= 3.0;

    public bool Ef6FeaturesSupported => this._modelBuilderVersion == DbModelBuilderVersion.Latest || this._modelBuilderVersion >= DbModelBuilderVersion.V6_0;

    private static bool IsEnumType(Type type)
    {
      type.TryUnwrapNullableType(out type);
      return type.IsEnum();
    }

    private static bool IsHierarchyIdType(Type type)
    {
      type.TryUnwrapNullableType(out type);
      return type == typeof (HierarchyId);
    }

    private static bool IsSpatialType(Type type)
    {
      type.TryUnwrapNullableType(out type);
      return type == typeof (DbGeometry) || type == typeof (DbGeography);
    }
  }
}
