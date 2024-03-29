﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.MappingInheritedPropertiesSupportConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to ensure an invalid/unsupported mapping is not created when mapping inherited properties
  /// </summary>
  public class MappingInheritedPropertiesSupportConvention : IDbMappingConvention, IConvention
  {
    void IDbMappingConvention.Apply(DbDatabaseMapping databaseMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDatabaseMapping>(databaseMapping, nameof (databaseMapping));
      databaseMapping.EntityContainerMappings.SelectMany<EntityContainerMapping, EntitySetMapping>((Func<EntityContainerMapping, IEnumerable<EntitySetMapping>>) (ecm => ecm.EntitySetMappings)).Each<EntitySetMapping>((Action<EntitySetMapping>) (esm =>
      {
        foreach (EntityTypeMapping entityTypeMapping in esm.EntityTypeMappings)
        {
          if (MappingInheritedPropertiesSupportConvention.RemapsInheritedProperties(databaseMapping, entityTypeMapping) && MappingInheritedPropertiesSupportConvention.HasBaseWithIsTypeOf(esm, entityTypeMapping.EntityType))
            throw System.Data.Entity.Resources.Error.UnsupportedHybridInheritanceMapping((object) entityTypeMapping.EntityType.Name);
        }
      }));
    }

    private static bool RemapsInheritedProperties(
      DbDatabaseMapping databaseMapping,
      EntityTypeMapping entityTypeMapping)
    {
      foreach (EdmProperty edmProperty in entityTypeMapping.EntityType.Properties.Except<EdmProperty>((IEnumerable<EdmProperty>) entityTypeMapping.EntityType.DeclaredProperties).Except<EdmProperty>((IEnumerable<EdmProperty>) entityTypeMapping.EntityType.GetKeyProperties()))
      {
        EdmProperty property = edmProperty;
        MappingFragment fragment = MappingInheritedPropertiesSupportConvention.GetFragmentForPropertyMapping(entityTypeMapping, property);
        if (fragment != null)
        {
          for (EntityType baseType = (EntityType) entityTypeMapping.EntityType.BaseType; baseType != null; baseType = (EntityType) baseType.BaseType)
          {
            if (databaseMapping.GetEntityTypeMappings(baseType).Select<EntityTypeMapping, MappingFragment>((Func<EntityTypeMapping, MappingFragment>) (baseTypeMapping => MappingInheritedPropertiesSupportConvention.GetFragmentForPropertyMapping(baseTypeMapping, property))).Any<MappingFragment>((Func<MappingFragment, bool>) (baseFragment => baseFragment != null && baseFragment.Table != fragment.Table)))
              return true;
          }
        }
      }
      return false;
    }

    private static MappingFragment GetFragmentForPropertyMapping(
      EntityTypeMapping entityTypeMapping,
      EdmProperty property)
    {
      return entityTypeMapping.MappingFragments.SingleOrDefault<MappingFragment>((Func<MappingFragment, bool>) (tmf => tmf.ColumnMappings.Any<ColumnMappingBuilder>((Func<ColumnMappingBuilder, bool>) (pm => pm.PropertyPath.Last<EdmProperty>() == property))));
    }

    private static bool HasBaseWithIsTypeOf(
      EntitySetMapping entitySetMapping,
      EntityType entityType)
    {
      for (EdmType baseType = entityType.BaseType; baseType != null; baseType = baseType.BaseType)
      {
        if (entitySetMapping.EntityTypeMappings.Where<EntityTypeMapping>((Func<EntityTypeMapping, bool>) (etm => etm.EntityType == baseType)).Any<EntityTypeMapping>((Func<EntityTypeMapping, bool>) (etm => etm.IsHierarchyMapping)))
          return true;
      }
      return false;
    }
  }
}
