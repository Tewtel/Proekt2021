// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.QueryCache.QueryCacheEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.QueryCache
{
  internal class QueryCacheEntry
  {
    private readonly QueryCacheKey _queryCacheKey;
    protected readonly object _target;

    internal QueryCacheEntry(QueryCacheKey queryCacheKey, object target)
    {
      this._queryCacheKey = queryCacheKey;
      this._target = target;
    }

    internal virtual object GetTarget() => this._target;

    internal QueryCacheKey QueryCacheKey => this._queryCacheKey;
  }
}
