// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.StorageEntityTypeMappingExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class StorageEntityTypeMappingExtensions
  {
    public static object GetConfiguration(this EntityTypeMapping entityTypeMapping) => entityTypeMapping.Annotations.GetConfiguration();

    public static void SetConfiguration(
      this EntityTypeMapping entityTypeMapping,
      object configuration)
    {
      entityTypeMapping.Annotations.SetConfiguration(configuration);
    }

    public static ColumnMappingBuilder GetPropertyMapping(
      this EntityTypeMapping entityTypeMapping,
      params EdmProperty[] propertyPath)
    {
      return entityTypeMapping.MappingFragments.SelectMany<MappingFragment, ColumnMappingBuilder>((Func<MappingFragment, IEnumerable<ColumnMappingBuilder>>) (f => f.ColumnMappings)).Single<ColumnMappingBuilder>((Func<ColumnMappingBuilder, bool>) (p => p.PropertyPath.SequenceEqual<EdmProperty>((IEnumerable<EdmProperty>) propertyPath)));
    }

    public static EntityType GetPrimaryTable(this EntityTypeMapping entityTypeMapping) => entityTypeMapping.MappingFragments.First<MappingFragment>().Table;

    public static bool UsesOtherTables(this EntityTypeMapping entityTypeMapping, EntityType table) => entityTypeMapping.MappingFragments.Any<MappingFragment>((Func<MappingFragment, bool>) (f => f.Table != table));

    public static Type GetClrType(this EntityTypeMapping entityTypeMappping) => entityTypeMappping.Annotations.GetClrType();

    public static void SetClrType(this EntityTypeMapping entityTypeMapping, Type type) => entityTypeMapping.Annotations.SetClrType(type);

    public static EntityTypeMapping Clone(this EntityTypeMapping entityTypeMapping)
    {
      EntityTypeMapping entityTypeMapping1 = new EntityTypeMapping((EntitySetMapping) null);
      entityTypeMapping1.AddType(entityTypeMapping.EntityType);
      entityTypeMapping.Annotations.Copy((ICollection<MetadataProperty>) entityTypeMapping1.Annotations);
      return entityTypeMapping1;
    }
  }
}
