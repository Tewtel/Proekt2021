// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.DefaultModelCacheKeyFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Internal
{
  internal sealed class DefaultModelCacheKeyFactory
  {
    public IDbModelCacheKey Create(DbContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbContext>(context, nameof (context));
      string customKey = (string) null;
      if (context is IDbModelCacheKeyProvider cacheKeyProvider)
        customKey = cacheKeyProvider.CacheKey;
      return (IDbModelCacheKey) new DefaultModelCacheKey(context.GetType(), context.InternalContext.ProviderName, context.InternalContext.ProviderFactory.GetType(), customKey);
    }
  }
}
