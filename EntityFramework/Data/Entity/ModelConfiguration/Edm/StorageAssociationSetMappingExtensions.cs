// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.StorageAssociationSetMappingExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Mapping;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class StorageAssociationSetMappingExtensions
  {
    public static AssociationSetMapping Initialize(
      this AssociationSetMapping associationSetMapping)
    {
      associationSetMapping.SourceEndMapping = new EndPropertyMapping();
      associationSetMapping.TargetEndMapping = new EndPropertyMapping();
      return associationSetMapping;
    }

    public static object GetConfiguration(this AssociationSetMapping associationSetMapping) => associationSetMapping.Annotations.GetConfiguration();

    public static void SetConfiguration(
      this AssociationSetMapping associationSetMapping,
      object configuration)
    {
      associationSetMapping.Annotations.SetConfiguration(configuration);
    }
  }
}
