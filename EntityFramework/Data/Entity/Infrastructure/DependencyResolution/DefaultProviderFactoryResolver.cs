// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.DefaultProviderFactoryResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class DefaultProviderFactoryResolver : IDbDependencyResolver
  {
    public virtual object GetService(Type type, object key) => DefaultProviderFactoryResolver.GetService(type, key, (Func<ArgumentException, string, object>) ((e, n) =>
    {
      throw new ArgumentException(System.Data.Entity.Resources.Strings.EntityClient_InvalidStoreProvider((object) n), (Exception) e);
    }));

    private static object GetService(
      Type type,
      object key,
      Func<ArgumentException, string, object> handleFailedLookup)
    {
      if (!(type == typeof (DbProviderFactory)))
        return (object) null;
      string str = key as string;
      if (string.IsNullOrWhiteSpace(str))
        throw new ArgumentException(System.Data.Entity.Resources.Strings.DbDependencyResolver_NoProviderInvariantName((object) typeof (DbProviderFactory).Name));
      try
      {
        return (object) DbProviderFactories.GetFactory(str);
      }
      catch (ArgumentException ex)
      {
        return string.Equals(str, "System.Data.SqlClient", StringComparison.OrdinalIgnoreCase) ? (object) SqlClientFactory.Instance : handleFailedLookup(ex, str);
      }
    }

    public IEnumerable<object> GetServices(Type type, object key)
    {
      object service = DefaultProviderFactoryResolver.GetService(type, key, (Func<ArgumentException, string, object>) ((e, n) => (object) null));
      if (service == null)
        return Enumerable.Empty<object>();
      return (IEnumerable<object>) new object[1]
      {
        service
      };
    }
  }
}
