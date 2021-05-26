// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.DbConnectionExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;

namespace System.Data.Entity.Utilities
{
  internal static class DbConnectionExtensions
  {
    public static string GetProviderInvariantName(this DbConnection connection) => DbConfiguration.DependencyResolver.GetService<IProviderInvariantName>((object) DbProviderServices.GetProviderFactory(connection)).Name;

    public static DbProviderInfo GetProviderInfo(
      this DbConnection connection,
      out DbProviderManifest providerManifest)
    {
      string str = DbConfiguration.DependencyResolver.GetService<IManifestTokenResolver>().ResolveManifestToken(connection);
      DbProviderInfo dbProviderInfo = new DbProviderInfo(connection.GetProviderInvariantName(), str);
      providerManifest = DbProviderServices.GetProviderServices(connection).GetProviderManifest(str);
      return dbProviderInfo;
    }

    public static DbProviderFactory GetProviderFactory(this DbConnection connection) => DbConfiguration.DependencyResolver.GetService<IDbProviderFactoryResolver>().ResolveProviderFactory(connection);
  }
}
