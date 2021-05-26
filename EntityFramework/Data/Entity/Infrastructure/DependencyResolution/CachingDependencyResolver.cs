// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.CachingDependencyResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class CachingDependencyResolver : IDbDependencyResolver
  {
    private readonly IDbDependencyResolver _underlyingResolver;
    private readonly ConcurrentDictionary<Tuple<Type, object>, object> _resolvedDependencies = new ConcurrentDictionary<Tuple<Type, object>, object>();
    private readonly ConcurrentDictionary<Tuple<Type, object>, IEnumerable<object>> _resolvedAllDependencies = new ConcurrentDictionary<Tuple<Type, object>, IEnumerable<object>>();

    public CachingDependencyResolver(IDbDependencyResolver underlyingResolver) => this._underlyingResolver = underlyingResolver;

    public virtual object GetService(Type type, object key) => this._resolvedDependencies.GetOrAdd(Tuple.Create<Type, object>(type, key), (Func<Tuple<Type, object>, object>) (k => this._underlyingResolver.GetService(type, key)));

    public IEnumerable<object> GetServices(Type type, object key) => this._resolvedAllDependencies.GetOrAdd(Tuple.Create<Type, object>(type, key), (Func<Tuple<Type, object>, IEnumerable<object>>) (k => this._underlyingResolver.GetServices(type, key)));
  }
}
