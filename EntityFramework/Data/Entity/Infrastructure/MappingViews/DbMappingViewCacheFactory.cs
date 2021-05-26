// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.MappingViews.DbMappingViewCacheFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Mapping;

namespace System.Data.Entity.Infrastructure.MappingViews
{
  /// <summary>
  /// Specifies the means to create concrete <see cref="T:System.Data.Entity.Infrastructure.MappingViews.DbMappingViewCache" /> instances.
  /// </summary>
  public abstract class DbMappingViewCacheFactory
  {
    /// <summary>
    /// Creates a generated view cache instance for the container mapping specified by
    /// the names of the mapped containers.
    /// </summary>
    /// <param name="conceptualModelContainerName">The name of a container in the conceptual model.</param>
    /// <param name="storeModelContainerName">The name of a container in the store model.</param>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Infrastructure.MappingViews.DbMappingViewCache" /> that specifies the generated view cache.
    /// </returns>
    public abstract DbMappingViewCache Create(
      string conceptualModelContainerName,
      string storeModelContainerName);

    internal DbMappingViewCache Create(EntityContainerMapping mapping) => this.Create(mapping.EdmEntityContainer.Name, mapping.StorageEntityContainer.Name);
  }
}
