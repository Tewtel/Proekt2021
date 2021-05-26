// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ObjectItemAssemblyLoader
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm.Provider;
using System.Data.Entity.Resources;
using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal abstract class ObjectItemAssemblyLoader
  {
    private readonly ObjectItemLoadingSessionData _sessionData;
    private readonly Assembly _assembly;
    private readonly AssemblyCacheEntry _cacheEntry;

    protected ObjectItemAssemblyLoader(
      Assembly assembly,
      AssemblyCacheEntry cacheEntry,
      ObjectItemLoadingSessionData sessionData)
    {
      this._assembly = assembly;
      this._cacheEntry = cacheEntry;
      this._sessionData = sessionData;
    }

    internal virtual void Load()
    {
      this.AddToAssembliesLoaded();
      this.LoadTypesFromAssembly();
      this.AddToKnownAssemblies();
      this.LoadClosureAssemblies();
    }

    protected abstract void AddToAssembliesLoaded();

    protected abstract void LoadTypesFromAssembly();

    protected virtual void LoadClosureAssemblies() => ObjectItemAssemblyLoader.LoadAssemblies((IEnumerable<Assembly>) this.CacheEntry.ClosureAssemblies, this.SessionData);

    internal virtual void OnLevel1SessionProcessing()
    {
    }

    internal virtual void OnLevel2SessionProcessing()
    {
    }

    internal static ObjectItemAssemblyLoader CreateLoader(
      Assembly assembly,
      ObjectItemLoadingSessionData sessionData)
    {
      if (sessionData.KnownAssemblies.Contains(assembly, (object) sessionData.ObjectItemAssemblyLoaderFactory, sessionData.EdmItemCollection))
        return (ObjectItemAssemblyLoader) new ObjectItemNoOpAssemblyLoader(assembly, sessionData);
      ImmutableAssemblyCacheEntry cacheEntry;
      if (sessionData.LockedAssemblyCache.TryGetValue(assembly, out cacheEntry))
      {
        if (sessionData.ObjectItemAssemblyLoaderFactory == null)
        {
          if (cacheEntry.TypesInAssembly.Count != 0)
            sessionData.ObjectItemAssemblyLoaderFactory = new Func<Assembly, ObjectItemLoadingSessionData, ObjectItemAssemblyLoader>(ObjectItemAttributeAssemblyLoader.Create);
        }
        else if (sessionData.ObjectItemAssemblyLoaderFactory != new Func<Assembly, ObjectItemLoadingSessionData, ObjectItemAssemblyLoader>(ObjectItemAttributeAssemblyLoader.Create))
          sessionData.EdmItemErrors.Add(new EdmItemError(Strings.Validator_OSpace_Convention_AttributeAssemblyReferenced((object) assembly.FullName)));
        return (ObjectItemAssemblyLoader) new ObjectItemCachedAssemblyLoader(assembly, cacheEntry, sessionData);
      }
      if (sessionData.EdmItemCollection != null && sessionData.EdmItemCollection.ConventionalOcCache.TryGetConventionalOcCacheFromAssemblyCache(assembly, out cacheEntry))
      {
        sessionData.ObjectItemAssemblyLoaderFactory = new Func<Assembly, ObjectItemLoadingSessionData, ObjectItemAssemblyLoader>(ObjectItemConventionAssemblyLoader.Create);
        return (ObjectItemAssemblyLoader) new ObjectItemCachedAssemblyLoader(assembly, cacheEntry, sessionData);
      }
      if (sessionData.ObjectItemAssemblyLoaderFactory == null)
      {
        if (ObjectItemAttributeAssemblyLoader.IsSchemaAttributePresent(assembly))
          sessionData.ObjectItemAssemblyLoaderFactory = new Func<Assembly, ObjectItemLoadingSessionData, ObjectItemAssemblyLoader>(ObjectItemAttributeAssemblyLoader.Create);
        else if (ObjectItemConventionAssemblyLoader.SessionContainsConventionParameters(sessionData))
          sessionData.ObjectItemAssemblyLoaderFactory = new Func<Assembly, ObjectItemLoadingSessionData, ObjectItemAssemblyLoader>(ObjectItemConventionAssemblyLoader.Create);
      }
      return sessionData.ObjectItemAssemblyLoaderFactory != null ? sessionData.ObjectItemAssemblyLoaderFactory(assembly, sessionData) : (ObjectItemAssemblyLoader) new ObjectItemNoOpAssemblyLoader(assembly, sessionData);
    }

    internal static bool IsAttributeLoader(object loaderCookie) => ObjectItemAssemblyLoader.IsAttributeLoader(loaderCookie as Func<Assembly, ObjectItemLoadingSessionData, ObjectItemAssemblyLoader>);

    internal static bool IsAttributeLoader(
      Func<Assembly, ObjectItemLoadingSessionData, ObjectItemAssemblyLoader> loaderFactory)
    {
      return loaderFactory != null && loaderFactory == new Func<Assembly, ObjectItemLoadingSessionData, ObjectItemAssemblyLoader>(ObjectItemAttributeAssemblyLoader.Create);
    }

    internal static bool IsConventionLoader(
      Func<Assembly, ObjectItemLoadingSessionData, ObjectItemAssemblyLoader> loaderFactory)
    {
      return loaderFactory != null && loaderFactory == new Func<Assembly, ObjectItemLoadingSessionData, ObjectItemAssemblyLoader>(ObjectItemConventionAssemblyLoader.Create);
    }

    protected virtual void AddToKnownAssemblies() => this._sessionData.KnownAssemblies.Add(this._assembly, new KnownAssemblyEntry(this.CacheEntry, this.SessionData.EdmItemCollection != null));

    protected static void LoadAssemblies(
      IEnumerable<Assembly> assemblies,
      ObjectItemLoadingSessionData sessionData)
    {
      foreach (Assembly assembly in assemblies)
        ObjectItemAssemblyLoader.CreateLoader(assembly, sessionData).Load();
    }

    protected static bool TryGetPrimitiveType(Type type, out PrimitiveType primitiveType)
    {
      ClrProviderManifest instance = ClrProviderManifest.Instance;
      Type clrType = Nullable.GetUnderlyingType(type);
      if ((object) clrType == null)
        clrType = type;
      ref PrimitiveType local = ref primitiveType;
      return instance.TryGetPrimitiveType(clrType, out local);
    }

    protected ObjectItemLoadingSessionData SessionData => this._sessionData;

    protected Assembly SourceAssembly => this._assembly;

    protected AssemblyCacheEntry CacheEntry => this._cacheEntry;
  }
}
