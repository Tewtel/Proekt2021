// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ObjectItemCachedAssemblyLoader
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class ObjectItemCachedAssemblyLoader : ObjectItemAssemblyLoader
  {
    private ImmutableAssemblyCacheEntry CacheEntry => (ImmutableAssemblyCacheEntry) base.CacheEntry;

    internal ObjectItemCachedAssemblyLoader(
      Assembly assembly,
      ImmutableAssemblyCacheEntry cacheEntry,
      ObjectItemLoadingSessionData sessionData)
      : base(assembly, (AssemblyCacheEntry) cacheEntry, sessionData)
    {
    }

    protected override void AddToAssembliesLoaded()
    {
    }

    protected override void LoadTypesFromAssembly()
    {
      foreach (EdmType edmType in (IEnumerable<EdmType>) this.CacheEntry.TypesInAssembly)
      {
        if (!this.SessionData.TypesInLoading.ContainsKey(edmType.Identity))
          this.SessionData.TypesInLoading.Add(edmType.Identity, edmType);
      }
    }
  }
}
