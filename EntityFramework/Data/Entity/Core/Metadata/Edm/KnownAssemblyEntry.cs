// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.KnownAssemblyEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class KnownAssemblyEntry
  {
    private readonly AssemblyCacheEntry _cacheEntry;

    internal KnownAssemblyEntry(AssemblyCacheEntry cacheEntry, bool seenWithEdmItemCollection)
    {
      this._cacheEntry = cacheEntry;
      this.ReferencedAssembliesAreLoaded = false;
      this.SeenWithEdmItemCollection = seenWithEdmItemCollection;
    }

    internal AssemblyCacheEntry CacheEntry => this._cacheEntry;

    public bool ReferencedAssembliesAreLoaded { get; set; }

    public bool SeenWithEdmItemCollection { get; set; }

    public bool HaveSeenInCompatibleContext(object loaderCookie, EdmItemCollection itemCollection) => this.SeenWithEdmItemCollection || itemCollection == null || ObjectItemAssemblyLoader.IsAttributeLoader(loaderCookie);
  }
}
