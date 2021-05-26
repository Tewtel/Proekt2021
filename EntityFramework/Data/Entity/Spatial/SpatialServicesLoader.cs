// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Spatial.SpatialServicesLoader
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;

namespace System.Data.Entity.Spatial
{
  internal class SpatialServicesLoader
  {
    private readonly IDbDependencyResolver _resolver;

    public SpatialServicesLoader(IDbDependencyResolver resolver) => this._resolver = resolver;

    public virtual DbSpatialServices LoadDefaultServices()
    {
      DbSpatialServices service1 = this._resolver.GetService<DbSpatialServices>();
      if (service1 != null)
        return service1;
      DbSpatialServices service2 = this._resolver.GetService<DbSpatialServices>((object) new DbProviderInfo("System.Data.SqlClient", "2012"));
      return service2 != null && service2.NativeTypesAvailable ? service2 : (DbSpatialServices) DefaultSpatialServices.Instance;
    }
  }
}
