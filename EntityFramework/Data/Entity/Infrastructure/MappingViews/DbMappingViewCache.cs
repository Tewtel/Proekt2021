// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.MappingViews.DbMappingViewCache
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Infrastructure.MappingViews
{
  /// <summary>
  /// Base abstract class for mapping view cache implementations.
  /// Derived classes must have a parameterless constructor if used with <see cref="T:System.Data.Entity.Infrastructure.MappingViews.DbMappingViewCacheTypeAttribute" />.
  /// </summary>
  public abstract class DbMappingViewCache
  {
    /// <summary>Gets a hash value computed over the mapping closure.</summary>
    public abstract string MappingHashValue { get; }

    /// <summary>Gets a view corresponding to the specified extent.</summary>
    /// <param name="extent">An <see cref="T:System.Data.Entity.Core.Metadata.Edm.EntitySetBase" /> that specifies the extent.</param>
    /// <returns>A <see cref="T:System.Data.Entity.Infrastructure.MappingViews.DbMappingView" /> that specifies the mapping view,
    /// or null if the extent is not associated with a mapping view.</returns>
    public abstract DbMappingView GetView(EntitySetBase extent);
  }
}
