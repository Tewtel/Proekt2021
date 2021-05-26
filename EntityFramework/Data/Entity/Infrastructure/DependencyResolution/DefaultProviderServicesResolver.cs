// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.DefaultProviderServicesResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Linq;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class DefaultProviderServicesResolver : IDbDependencyResolver
  {
    public virtual object GetService(Type type, object key)
    {
      if (type == typeof (DbProviderServices))
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.EF6Providers_NoProviderFound((object) DefaultProviderServicesResolver.CheckKey(key)));
      return (object) null;
    }

    private static string CheckKey(object key)
    {
      string str = key as string;
      return !string.IsNullOrWhiteSpace(str) ? str : throw new ArgumentException(System.Data.Entity.Resources.Strings.DbDependencyResolver_NoProviderInvariantName((object) typeof (DbProviderServices).Name));
    }

    public virtual IEnumerable<object> GetServices(Type type, object key)
    {
      if (type == typeof (DbProviderServices))
        DefaultProviderServicesResolver.CheckKey(key);
      return Enumerable.Empty<object>();
    }
  }
}
