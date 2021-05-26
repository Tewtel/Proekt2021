// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.Services.StructuralTypeMappingGenerator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Edm.Services
{
  internal abstract class StructuralTypeMappingGenerator
  {
    protected readonly DbProviderManifest _providerManifest;

    protected StructuralTypeMappingGenerator(DbProviderManifest providerManifest) => this._providerManifest = providerManifest;

    protected EdmProperty MapTableColumn(
      EdmProperty property,
      string columnName,
      bool isInstancePropertyOnDerivedType)
    {
      TypeUsage storeType = this._providerManifest.GetStoreType(TypeUsage.Create((EdmType) property.UnderlyingPrimitiveType, (IEnumerable<Facet>) property.TypeUsage.Facets));
      EdmProperty column = new EdmProperty(columnName, storeType)
      {
        Nullable = isInstancePropertyOnDerivedType || property.Nullable
      };
      if (column.IsPrimaryKeyColumn)
        column.Nullable = false;
      StoreGeneratedPattern? generatedPattern = property.GetStoreGeneratedPattern();
      if (generatedPattern.HasValue)
        column.StoreGeneratedPattern = generatedPattern.Value;
      StructuralTypeMappingGenerator.MapPrimitivePropertyFacets(property, column, storeType);
      return column;
    }

    internal static void MapPrimitivePropertyFacets(
      EdmProperty property,
      EdmProperty column,
      TypeUsage typeUsage)
    {
      bool? nullable1;
      if (StructuralTypeMappingGenerator.IsValidFacet(typeUsage, "FixedLength"))
      {
        nullable1 = property.IsFixedLength;
        if (nullable1.HasValue)
          column.IsFixedLength = property.IsFixedLength;
      }
      if (StructuralTypeMappingGenerator.IsValidFacet(typeUsage, "MaxLength"))
      {
        column.IsMaxLength = property.IsMaxLength;
        if (!column.IsMaxLength || property.MaxLength.HasValue)
          column.MaxLength = property.MaxLength;
      }
      if (StructuralTypeMappingGenerator.IsValidFacet(typeUsage, "Unicode"))
      {
        nullable1 = property.IsUnicode;
        if (nullable1.HasValue)
          column.IsUnicode = property.IsUnicode;
      }
      byte? nullable2;
      if (StructuralTypeMappingGenerator.IsValidFacet(typeUsage, "Precision"))
      {
        nullable2 = property.Precision;
        if (nullable2.HasValue)
          column.Precision = property.Precision;
      }
      if (!StructuralTypeMappingGenerator.IsValidFacet(typeUsage, "Scale"))
        return;
      nullable2 = property.Scale;
      if (!nullable2.HasValue)
        return;
      column.Scale = property.Scale;
    }

    private static bool IsValidFacet(TypeUsage typeUsage, string name)
    {
      Facet facet;
      return typeUsage.Facets.TryGetValue(name, false, out facet) && !facet.Description.IsConstant;
    }

    protected static EntityTypeMapping GetEntityTypeMappingInHierarchy(
      DbDatabaseMapping databaseMapping,
      EntityType entityType)
    {
      EntityTypeMapping entityTypeMapping = databaseMapping.GetEntityTypeMapping(entityType);
      if (entityTypeMapping == null)
      {
        EntitySetMapping entitySetMapping = databaseMapping.GetEntitySetMapping(databaseMapping.Model.GetEntitySet(entityType));
        if (entitySetMapping != null)
          entityTypeMapping = entitySetMapping.EntityTypeMappings.First<EntityTypeMapping>((Func<EntityTypeMapping, bool>) (etm => entityType.DeclaredProperties.All<EdmProperty>((Func<EdmProperty, bool>) (dp => etm.MappingFragments.First<MappingFragment>().ColumnMappings.Select<ColumnMappingBuilder, EdmProperty>((Func<ColumnMappingBuilder, EdmProperty>) (pm => pm.PropertyPath.First<EdmProperty>())).Contains<EdmProperty>(dp)))));
      }
      return entityTypeMapping;
    }
  }
}
