// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.Services.PropertyMappingGenerator
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
  internal class PropertyMappingGenerator : StructuralTypeMappingGenerator
  {
    public PropertyMappingGenerator(DbProviderManifest providerManifest)
      : base(providerManifest)
    {
    }

    public void Generate(
      EntityType entityType,
      IEnumerable<EdmProperty> properties,
      EntitySetMapping entitySetMapping,
      MappingFragment entityTypeMappingFragment,
      IList<EdmProperty> propertyPath,
      bool createNewColumn)
    {
      ReadOnlyMetadataCollection<EdmProperty> declaredProperties = entityType.GetRootType().DeclaredProperties;
      foreach (EdmProperty property1 in properties)
      {
        EdmProperty property = property1;
        if (property.IsComplexType && propertyPath.Any<EdmProperty>((Func<EdmProperty, bool>) (p => p.IsComplexType && p.ComplexType == property.ComplexType)))
          throw System.Data.Entity.Resources.Error.CircularComplexTypeHierarchy();
        propertyPath.Add(property);
        if (property.IsComplexType)
        {
          this.Generate(entityType, (IEnumerable<EdmProperty>) property.ComplexType.Properties, entitySetMapping, entityTypeMappingFragment, propertyPath, createNewColumn);
        }
        else
        {
          EdmProperty edmProperty = entitySetMapping.EntityTypeMappings.SelectMany<EntityTypeMapping, MappingFragment>((Func<EntityTypeMapping, IEnumerable<MappingFragment>>) (etm => (IEnumerable<MappingFragment>) etm.MappingFragments)).SelectMany<MappingFragment, ColumnMappingBuilder>((Func<MappingFragment, IEnumerable<ColumnMappingBuilder>>) (etmf => etmf.ColumnMappings)).Where<ColumnMappingBuilder>((Func<ColumnMappingBuilder, bool>) (pm => pm.PropertyPath.SequenceEqual<EdmProperty>((IEnumerable<EdmProperty>) propertyPath))).Select<ColumnMappingBuilder, EdmProperty>((Func<ColumnMappingBuilder, EdmProperty>) (pm => pm.ColumnProperty)).FirstOrDefault<EdmProperty>();
          if (edmProperty == null | createNewColumn)
          {
            string columnName = string.Join("_", propertyPath.Select<EdmProperty, string>((Func<EdmProperty, string>) (p => p.Name)));
            edmProperty = this.MapTableColumn(property, columnName, !declaredProperties.Contains(propertyPath.First<EdmProperty>()));
            entityTypeMappingFragment.Table.AddColumn(edmProperty);
            if (entityType.KeyProperties().Contains<EdmProperty>(property))
              entityTypeMappingFragment.Table.AddKeyMember((EdmMember) edmProperty);
          }
          entityTypeMappingFragment.AddColumnMapping(new ColumnMappingBuilder(edmProperty, (IList<EdmProperty>) propertyPath.ToList<EdmProperty>()));
        }
        propertyPath.Remove(property);
      }
    }
  }
}
