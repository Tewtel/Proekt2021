// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbConnectionInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Configuration;
using System.Data.Entity.Internal;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>Represents information about a database connection.</summary>
  [Serializable]
  public class DbConnectionInfo
  {
    private readonly string _connectionName;
    private readonly string _connectionString;
    private readonly string _providerInvariantName;

    /// <summary>
    /// Creates a new instance of DbConnectionInfo representing a connection that is specified in the application configuration file.
    /// </summary>
    /// <param name="connectionName"> The name of the connection string in the application configuration. </param>
    public DbConnectionInfo(string connectionName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(connectionName, nameof (connectionName));
      this._connectionName = connectionName;
    }

    /// <summary>
    /// Creates a new instance of DbConnectionInfo based on a connection string.
    /// </summary>
    /// <param name="connectionString"> The connection string to use for the connection. </param>
    /// <param name="providerInvariantName"> The name of the provider to use for the connection. Use 'System.Data.SqlClient' for SQL Server. </param>
    public DbConnectionInfo(string connectionString, string providerInvariantName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(connectionString, nameof (connectionString));
      System.Data.Entity.Utilities.Check.NotEmpty(providerInvariantName, nameof (providerInvariantName));
      this._connectionString = connectionString;
      this._providerInvariantName = providerInvariantName;
    }

    internal ConnectionStringSettings GetConnectionString(AppConfig config)
    {
      if (this._connectionName == null)
        return new ConnectionStringSettings((string) null, this._connectionString, this._providerInvariantName);
      return config.GetConnectionString(this._connectionName) ?? throw Error.DbConnectionInfo_ConnectionStringNotFound((object) this._connectionName);
    }

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the current instance.
    /// </summary>
    /// <returns>The exact runtime type of the current instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
