// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.RepositoryBase
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure.Interception;

namespace System.Data.Entity.Internal
{
  internal abstract class RepositoryBase
  {
    private readonly InternalContext _usersContext;
    private readonly string _connectionString;
    private readonly DbProviderFactory _providerFactory;

    protected RepositoryBase(
      InternalContext usersContext,
      string connectionString,
      DbProviderFactory providerFactory)
    {
      this._usersContext = usersContext;
      this._connectionString = connectionString;
      this._providerFactory = providerFactory;
    }

    protected DbConnection CreateConnection()
    {
      DbConnection connection1;
      DbConnection connection2;
      if (!this._usersContext.IsDisposed && (connection1 = this._usersContext.Connection) != null)
      {
        if (connection1.State == ConnectionState.Open)
          return connection1;
        connection2 = DbProviderServices.GetProviderServices(connection1).CloneDbConnection(connection1, this._providerFactory);
      }
      else
        connection2 = this._providerFactory.CreateConnection();
      DbInterception.Dispatch.Connection.SetConnectionString(connection2, new DbConnectionPropertyInterceptionContext<string>().WithValue(this._connectionString));
      return connection2;
    }

    protected void DisposeConnection(DbConnection connection)
    {
      if (connection == null || !this._usersContext.IsDisposed && connection == this._usersContext.Connection)
        return;
      DbInterception.Dispatch.Connection.Dispose(connection, new DbInterceptionContext());
    }
  }
}
