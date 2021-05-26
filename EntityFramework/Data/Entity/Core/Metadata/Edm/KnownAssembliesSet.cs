// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.KnownAssembliesSet
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class KnownAssembliesSet
  {
    private readonly Dictionary<Assembly, KnownAssemblyEntry> _assemblies;

    internal KnownAssembliesSet() => this._assemblies = new Dictionary<Assembly, KnownAssemblyEntry>();

    internal KnownAssembliesSet(KnownAssembliesSet set) => this._assemblies = new Dictionary<Assembly, KnownAssemblyEntry>((IDictionary<Assembly, KnownAssemblyEntry>) set._assemblies);

    internal virtual bool TryGetKnownAssembly(
      Assembly assembly,
      object loaderCookie,
      EdmItemCollection itemCollection,
      out KnownAssemblyEntry entry)
    {
      return this._assemblies.TryGetValue(assembly, out entry) && entry.HaveSeenInCompatibleContext(loaderCookie, itemCollection);
    }

    internal IEnumerable<Assembly> Assemblies => (IEnumerable<Assembly>) this._assemblies.Keys;

    public IEnumerable<KnownAssemblyEntry> GetEntries(
      object loaderCookie,
      EdmItemCollection itemCollection)
    {
      return this._assemblies.Values.Where<KnownAssemblyEntry>((Func<KnownAssemblyEntry, bool>) (e => e.HaveSeenInCompatibleContext(loaderCookie, itemCollection)));
    }

    internal bool Contains(
      Assembly assembly,
      object loaderCookie,
      EdmItemCollection itemCollection)
    {
      return this.TryGetKnownAssembly(assembly, loaderCookie, itemCollection, out KnownAssemblyEntry _);
    }

    internal void Add(Assembly assembly, KnownAssemblyEntry knownAssemblyEntry)
    {
      if (this._assemblies.TryGetValue(assembly, out KnownAssemblyEntry _))
        this._assemblies[assembly] = knownAssemblyEntry;
      else
        this._assemblies.Add(assembly, knownAssemblyEntry);
    }
  }
}
