// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.DbProviderFactoryExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.EntityClient.Internal;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.SqlClient;
using System.Linq;

namespace System.Data.Entity.Utilities
{
  internal static class DbProviderFactoryExtensions
  {
    public static string GetProviderInvariantName(this DbProviderFactory factory)
    {
      IEnumerable<DataRow> dataRows = DbProviderFactories.GetFactoryClasses().Rows.OfType<DataRow>();
      DataRow row = new ProviderRowFinder().FindRow(factory.GetType(), (Func<DataRow, bool>) (r => DbProviderFactories.GetFactory(r).GetType() == factory.GetType()), dataRows);
      if (row != null)
        return (string) row[2];
      if (factory.GetType() == typeof (SqlClientFactory))
        return "System.Data.SqlClient";
      throw new NotSupportedException(System.Data.Entity.Resources.Strings.ProviderNameNotFound((object) factory));
    }

    internal static DbProviderServices GetProviderServices(
      this DbProviderFactory factory)
    {
      return factory is EntityProviderFactory ? (DbProviderServices) EntityProviderServices.Instance : DbConfiguration.DependencyResolver.GetService<DbProviderServices>((object) DbConfiguration.DependencyResolver.GetService<IProviderInvariantName>((object) factory).Name);
    }
  }
}
