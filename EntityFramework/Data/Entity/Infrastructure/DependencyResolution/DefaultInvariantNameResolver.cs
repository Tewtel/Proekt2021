// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.DefaultInvariantNameResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class DefaultInvariantNameResolver : IDbDependencyResolver
  {
    public virtual object GetService(Type type, object key)
    {
      if (!(type == typeof (IProviderInvariantName)))
        return (object) null;
      return key is DbProviderFactory factory ? (object) new ProviderInvariantName(factory.GetProviderInvariantName()) : throw new ArgumentException(Strings.DbDependencyResolver_InvalidKey((object) typeof (DbProviderFactory).Name, (object) typeof (IProviderInvariantName)));
    }

    public IEnumerable<object> GetServices(Type type, object key) => this.GetServiceAsServices(type, key);
  }
}
