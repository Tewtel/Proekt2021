// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Mapping.SortedEntityTypeIndex
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Edm;

namespace System.Data.Entity.ModelConfiguration.Configuration.Mapping
{
  internal class SortedEntityTypeIndex
  {
    private static readonly EntityType[] _emptyTypes = new EntityType[0];
    private readonly Dictionary<EntitySet, List<EntityType>> _entityTypes;

    public SortedEntityTypeIndex() => this._entityTypes = new Dictionary<EntitySet, List<EntityType>>();

    public void Add(EntitySet entitySet, EntityType entityType)
    {
      int index = 0;
      List<EntityType> entityTypeList;
      if (!this._entityTypes.TryGetValue(entitySet, out entityTypeList))
      {
        entityTypeList = new List<EntityType>();
        this._entityTypes.Add(entitySet, entityTypeList);
      }
      for (; index < entityTypeList.Count; ++index)
      {
        if (entityTypeList[index] == entityType)
          return;
        if (entityType.IsAncestorOf(entityTypeList[index]))
          break;
      }
      entityTypeList.Insert(index, entityType);
    }

    public bool Contains(EntitySet entitySet, EntityType entityType)
    {
      List<EntityType> entityTypeList;
      return this._entityTypes.TryGetValue(entitySet, out entityTypeList) && entityTypeList.Contains(entityType);
    }

    public bool IsRoot(EntitySet entitySet, EntityType entityType)
    {
      bool flag = true;
      foreach (EntityType ancestor in this._entityTypes[entitySet])
      {
        if (ancestor != entityType && ancestor.IsAncestorOf(entityType))
          flag = false;
      }
      return flag;
    }

    public IEnumerable<EntitySet> GetEntitySets() => (IEnumerable<EntitySet>) this._entityTypes.Keys;

    public IEnumerable<EntityType> GetEntityTypes(EntitySet entitySet)
    {
      List<EntityType> entityTypeList;
      return this._entityTypes.TryGetValue(entitySet, out entityTypeList) ? (IEnumerable<EntityType>) entityTypeList : (IEnumerable<EntityType>) SortedEntityTypeIndex._emptyTypes;
    }
  }
}
