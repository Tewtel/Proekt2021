// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.WrappingDependencyResolver`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class WrappingDependencyResolver<TService> : IDbDependencyResolver
  {
    private readonly IDbDependencyResolver _snapshot;
    private readonly Func<TService, object, TService> _serviceWrapper;

    public WrappingDependencyResolver(
      IDbDependencyResolver snapshot,
      Func<TService, object, TService> serviceWrapper)
    {
      this._snapshot = snapshot;
      this._serviceWrapper = serviceWrapper;
    }

    public object GetService(Type type, object key) => !(type == typeof (TService)) ? (object) null : (object) this._serviceWrapper(this._snapshot.GetService<TService>(key), key);

    public IEnumerable<object> GetServices(Type type, object key) => !(type == typeof (TService)) ? Enumerable.Empty<object>() : (IEnumerable<object>) this._snapshot.GetServices<TService>(key).Select<TService, TService>((Func<TService, TService>) (s => this._serviceWrapper(s, key)));
  }
}
