// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.MappingViews.DefaultDbMappingViewCacheFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.MappingViews
{
  internal class DefaultDbMappingViewCacheFactory : DbMappingViewCacheFactory
  {
    private readonly Type _cacheType;

    public DefaultDbMappingViewCacheFactory(Type cacheType) => this._cacheType = cacheType;

    public override DbMappingViewCache Create(
      string conceptualModelContainerName,
      string storeModelContainerName)
    {
      return (DbMappingViewCache) Activator.CreateInstance(this._cacheType);
    }

    public override int GetHashCode() => this._cacheType.GetHashCode() * 397 ^ typeof (DefaultDbMappingViewCacheFactory).GetHashCode();

    public override bool Equals(object obj) => obj is DefaultDbMappingViewCacheFactory viewCacheFactory && (object) viewCacheFactory._cacheType == (object) this._cacheType;
  }
}
