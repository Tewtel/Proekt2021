// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.ResolverChain
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class ResolverChain : IDbDependencyResolver
  {
    private readonly ConcurrentStack<IDbDependencyResolver> _resolvers = new ConcurrentStack<IDbDependencyResolver>();
    private volatile IDbDependencyResolver[] _resolversSnapshot = new IDbDependencyResolver[0];

    public virtual void Add(IDbDependencyResolver resolver)
    {
      System.Data.Entity.Utilities.Check.NotNull<IDbDependencyResolver>(resolver, nameof (resolver));
      this._resolvers.Push(resolver);
      this._resolversSnapshot = this._resolvers.ToArray();
    }

    public virtual IEnumerable<IDbDependencyResolver> Resolvers => ((IEnumerable<IDbDependencyResolver>) this._resolversSnapshot).Reverse<IDbDependencyResolver>();

    public virtual object GetService(Type type, object key) => ((IEnumerable<IDbDependencyResolver>) this._resolversSnapshot).Select<IDbDependencyResolver, object>((Func<IDbDependencyResolver, object>) (r => r.GetService(type, key))).FirstOrDefault<object>((Func<object, bool>) (s => s != null));

    public virtual IEnumerable<object> GetServices(Type type, object key) => ((IEnumerable<IDbDependencyResolver>) this._resolversSnapshot).SelectMany<IDbDependencyResolver, object>((Func<IDbDependencyResolver, IEnumerable<object>>) (r => r.GetServices(type, key)));
  }
}
