// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.QueryCacheConfig
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Internal.ConfigFile;

namespace System.Data.Entity.Internal
{
  internal class QueryCacheConfig
  {
    private const int DefaultSize = 1000;
    private const int DefaultCleaningIntervalInSeconds = 60;
    private readonly EntityFrameworkSection _entityFrameworkSection;

    public QueryCacheConfig(EntityFrameworkSection entityFrameworkSection) => this._entityFrameworkSection = entityFrameworkSection;

    public int GetQueryCacheSize()
    {
      int size = this._entityFrameworkSection.QueryCache.Size;
      return size == 0 ? 1000 : size;
    }

    public int GetCleaningIntervalInSeconds()
    {
      int intervalInSeconds = this._entityFrameworkSection.QueryCache.CleaningIntervalInSeconds;
      return intervalInSeconds == 0 ? 60 : intervalInSeconds;
    }
  }
}
