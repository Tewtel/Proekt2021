// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.MappingMetadataHelper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping;
using System.Linq;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal static class MappingMetadataHelper
  {
    internal static IEnumerable<TypeMapping> GetMappingsForEntitySetAndType(
      StorageMappingItemCollection mappingCollection,
      EntityContainer container,
      EntitySetBase entitySet,
      EntityTypeBase entityType)
    {
      EntitySetBaseMapping setMapping = MappingMetadataHelper.GetEntityContainerMap(mappingCollection, container).GetSetMapping(entitySet.Name);
      if (setMapping != null)
      {
        foreach (TypeMapping typeMapping in setMapping.TypeMappings.Where<TypeMapping>((Func<TypeMapping, bool>) (map => map.Types.Union<EntityTypeBase>((IEnumerable<EntityTypeBase>) map.IsOfTypes).Contains<EntityTypeBase>(entityType))))
          yield return typeMapping;
      }
    }

    internal static IEnumerable<TypeMapping> GetMappingsForEntitySetAndSuperTypes(
      StorageMappingItemCollection mappingCollection,
      EntityContainer container,
      EntitySetBase entitySet,
      EntityTypeBase childEntityType)
    {
      return (IEnumerable<TypeMapping>) MetadataHelper.GetTypeAndParentTypesOf((EdmType) childEntityType, true).SelectMany<EdmType, TypeMapping>((Func<EdmType, IEnumerable<TypeMapping>>) (edmType =>
      {
        EntityTypeBase entityType = edmType as EntityTypeBase;
        return !edmType.EdmEquals((MetadataItem) childEntityType) ? MappingMetadataHelper.GetIsTypeOfMappingsForEntitySetAndType(mappingCollection, container, entitySet, entityType, childEntityType) : MappingMetadataHelper.GetMappingsForEntitySetAndType(mappingCollection, container, entitySet, entityType);
      })).ToList<TypeMapping>();
    }

    private static IEnumerable<TypeMapping> GetIsTypeOfMappingsForEntitySetAndType(
      StorageMappingItemCollection mappingCollection,
      EntityContainer container,
      EntitySetBase entitySet,
      EntityTypeBase entityType,
      EntityTypeBase childEntityType)
    {
      foreach (TypeMapping typeMapping in MappingMetadataHelper.GetMappingsForEntitySetAndType(mappingCollection, container, entitySet, entityType))
      {
        if (typeMapping.IsOfTypes.Any<EntityTypeBase>((Func<EntityTypeBase, bool>) (parentType => parentType.IsAssignableFrom((EdmType) childEntityType))) || typeMapping.Types.Contains(childEntityType))
          yield return typeMapping;
      }
    }

    internal static IEnumerable<EntityTypeModificationFunctionMapping> GetModificationFunctionMappingsForEntitySetAndType(
      StorageMappingItemCollection mappingCollection,
      EntityContainer container,
      EntitySetBase entitySet,
      EntityTypeBase entityType)
    {
      if (MappingMetadataHelper.GetEntityContainerMap(mappingCollection, container).GetSetMapping(entitySet.Name) is EntitySetMapping setMapping && setMapping != null)
      {
        foreach (EntityTypeModificationFunctionMapping modificationFunctionMapping in setMapping.ModificationFunctionMappings.Where<EntityTypeModificationFunctionMapping>((Func<EntityTypeModificationFunctionMapping, bool>) (functionMap => functionMap.EntityType.Equals((object) entityType))))
          yield return modificationFunctionMapping;
      }
    }

    internal static EntityContainerMapping GetEntityContainerMap(
      StorageMappingItemCollection mappingCollection,
      EntityContainer entityContainer)
    {
      ReadOnlyCollection<EntityContainerMapping> items = mappingCollection.GetItems<EntityContainerMapping>();
      EntityContainerMapping containerMapping1 = (EntityContainerMapping) null;
      foreach (EntityContainerMapping containerMapping2 in items)
      {
        if (entityContainer.Equals((object) containerMapping2.EdmEntityContainer) || entityContainer.Equals((object) containerMapping2.StorageEntityContainer))
        {
          containerMapping1 = containerMapping2;
          break;
        }
      }
      return containerMapping1 != null ? containerMapping1 : throw new MappingException(System.Data.Entity.Resources.Strings.Mapping_NotFound_EntityContainer((object) entityContainer.Name));
    }
  }
}
