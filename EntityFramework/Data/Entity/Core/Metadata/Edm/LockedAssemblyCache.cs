﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.LockedAssemblyCache
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class LockedAssemblyCache : IDisposable
  {
    private object _lockObject;
    private Dictionary<Assembly, ImmutableAssemblyCacheEntry> _globalAssemblyCache;

    internal LockedAssemblyCache(
      object lockObject,
      Dictionary<Assembly, ImmutableAssemblyCacheEntry> globalAssemblyCache)
    {
      this._lockObject = lockObject;
      this._globalAssemblyCache = globalAssemblyCache;
      Monitor.Enter(this._lockObject);
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      Monitor.Exit(this._lockObject);
      this._lockObject = (object) null;
      this._globalAssemblyCache = (Dictionary<Assembly, ImmutableAssemblyCacheEntry>) null;
    }

    [Conditional("DEBUG")]
    private void AssertLockedByThisThread()
    {
      bool lockTaken = false;
      Monitor.TryEnter(this._lockObject, ref lockTaken);
      if (!lockTaken)
        return;
      Monitor.Exit(this._lockObject);
    }

    internal bool TryGetValue(Assembly assembly, out ImmutableAssemblyCacheEntry cacheEntry) => this._globalAssemblyCache.TryGetValue(assembly, out cacheEntry);

    internal void Add(Assembly assembly, ImmutableAssemblyCacheEntry assemblyCacheEntry) => this._globalAssemblyCache.Add(assembly, assemblyCacheEntry);

    internal void Clear() => this._globalAssemblyCache.Clear();
  }
}
