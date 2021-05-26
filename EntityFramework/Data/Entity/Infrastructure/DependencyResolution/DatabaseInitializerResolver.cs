// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.DatabaseInitializerResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class DatabaseInitializerResolver : IDbDependencyResolver
  {
    private readonly ConcurrentDictionary<Type, object> _initializers = new ConcurrentDictionary<Type, object>();

    public virtual object GetService(Type type, object key)
    {
      Type elementType = type.TryGetElementType(typeof (IDatabaseInitializer<>));
      object obj;
      return elementType != (Type) null && this._initializers.TryGetValue(elementType, out obj) ? obj : (object) null;
    }

    public virtual void SetInitializer(Type contextType, object initializer) => this._initializers.AddOrUpdate(contextType, initializer, (Func<Type, object, object>) ((c, i) => initializer));

    public IEnumerable<object> GetServices(Type type, object key) => this.GetServiceAsServices(type, key);
  }
}
