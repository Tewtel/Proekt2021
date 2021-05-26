﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.IDbModelCacheKeyProvider
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Implement this interface on your context to use custom logic to calculate the key used to lookup an already created model in the cache.
  /// This interface allows you to have a single context type that can be used with different models in the same AppDomain,
  /// or multiple context types that use the same model.
  /// </summary>
  public interface IDbModelCacheKeyProvider
  {
    /// <summary>Gets the cached key associated with the provider.</summary>
    /// <returns>The cached key associated with the provider.</returns>
    string CacheKey { get; }
  }
}
