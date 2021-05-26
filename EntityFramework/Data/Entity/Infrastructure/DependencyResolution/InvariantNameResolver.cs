// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.InvariantNameResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class InvariantNameResolver : IDbDependencyResolver
  {
    private readonly IProviderInvariantName _invariantName;
    private readonly Type _providerFactoryType;

    public InvariantNameResolver(DbProviderFactory providerFactory, string invariantName)
    {
      this._invariantName = (IProviderInvariantName) new ProviderInvariantName(invariantName);
      this._providerFactoryType = providerFactory.GetType();
    }

    public virtual object GetService(Type type, object key)
    {
      if (type == typeof (IProviderInvariantName))
      {
        if (!(key is DbProviderFactory))
          throw new ArgumentException(Strings.DbDependencyResolver_InvalidKey((object) typeof (DbProviderFactory).Name, (object) typeof (IProviderInvariantName)));
        if (key.GetType() == this._providerFactoryType)
          return (object) this._invariantName;
      }
      return (object) null;
    }

    public override bool Equals(object obj) => obj is InvariantNameResolver invariantNameResolver && this._providerFactoryType == invariantNameResolver._providerFactoryType && this._invariantName.Name == invariantNameResolver._invariantName.Name;

    public override int GetHashCode() => this._invariantName.Name.GetHashCode();

    public IEnumerable<object> GetServices(Type type, object key) => this.GetServiceAsServices(type, key);
  }
}
