// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.QueryCache.CompiledQueryCacheKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.QueryCache
{
  internal sealed class CompiledQueryCacheKey : QueryCacheKey
  {
    private readonly Guid _cacheIdentity;

    internal CompiledQueryCacheKey(Guid cacheIdentity) => this._cacheIdentity = cacheIdentity;

    public override bool Equals(object compareTo) => !(typeof (CompiledQueryCacheKey) != compareTo.GetType()) && ((CompiledQueryCacheKey) compareTo)._cacheIdentity.Equals(this._cacheIdentity);

    public override int GetHashCode() => this._cacheIdentity.GetHashCode();

    public override string ToString() => this._cacheIdentity.ToString();
  }
}
