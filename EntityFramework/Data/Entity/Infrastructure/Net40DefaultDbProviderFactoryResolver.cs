// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Net40DefaultDbProviderFactoryResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.Infrastructure
{
  internal class Net40DefaultDbProviderFactoryResolver : IDbProviderFactoryResolver
  {
    private readonly ConcurrentDictionary<Type, DbProviderFactory> _cache = new ConcurrentDictionary<Type, DbProviderFactory>((IEnumerable<KeyValuePair<Type, DbProviderFactory>>) new KeyValuePair<Type, DbProviderFactory>[1]
    {
      new KeyValuePair<Type, DbProviderFactory>(typeof (EntityConnection), (DbProviderFactory) EntityProviderFactory.Instance)
    });
    private readonly ProviderRowFinder _finder;

    public Net40DefaultDbProviderFactoryResolver()
      : this(new ProviderRowFinder())
    {
    }

    public Net40DefaultDbProviderFactoryResolver(ProviderRowFinder finder) => this._finder = finder;

    public DbProviderFactory ResolveProviderFactory(DbConnection connection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      return this.GetProviderFactory(connection, DbProviderFactories.GetFactoryClasses().Rows.OfType<DataRow>());
    }

    public DbProviderFactory GetProviderFactory(
      DbConnection connection,
      IEnumerable<DataRow> dataRows)
    {
      return this._cache.GetOrAdd(connection.GetType(), (Func<Type, DbProviderFactory>) (t =>
      {
        return DbProviderFactories.GetFactory((this._finder.FindRow(t, (Func<DataRow, bool>) (r => Net40DefaultDbProviderFactoryResolver.ExactMatch(r, t)), dataRows) ?? this._finder.FindRow((Type) null, (Func<DataRow, bool>) (r => Net40DefaultDbProviderFactoryResolver.ExactMatch(r, t)), dataRows) ?? this._finder.FindRow(t, (Func<DataRow, bool>) (r => Net40DefaultDbProviderFactoryResolver.AssignableMatch(r, t)), dataRows) ?? this._finder.FindRow((Type) null, (Func<DataRow, bool>) (r => Net40DefaultDbProviderFactoryResolver.AssignableMatch(r, t)), dataRows)) ?? throw new NotSupportedException(System.Data.Entity.Resources.Strings.ProviderNotFound((object) connection.ToString())));
      }));
    }

    private static bool ExactMatch(DataRow row, Type connectionType) => DbProviderFactories.GetFactory(row).CreateConnection().GetType() == connectionType;

    private static bool AssignableMatch(DataRow row, Type connectionType) => connectionType.IsInstanceOfType((object) DbProviderFactories.GetFactory(row).CreateConnection());
  }
}
