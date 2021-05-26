// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.TransactionContextInitializerResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class TransactionContextInitializerResolver : IDbDependencyResolver
  {
    private readonly ConcurrentDictionary<Type, object> _initializers = new ConcurrentDictionary<Type, object>();

    public object GetService(Type type, object key)
    {
      System.Data.Entity.Utilities.Check.NotNull<Type>(type, nameof (type));
      Type elementType = type.TryGetElementType(typeof (IDatabaseInitializer<>));
      return elementType != (Type) null && typeof (TransactionContext).IsAssignableFrom(elementType) ? this._initializers.GetOrAdd(elementType, new Func<Type, object>(this.CreateInitializerInstance)) : (object) null;
    }

    private object CreateInitializerInstance(Type type) => Activator.CreateInstance(typeof (TransactionContextInitializer<>).MakeGenericType(type));

    public IEnumerable<object> GetServices(Type type, object key) => this.GetServiceAsServices(type, key);
  }
}
