// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.SqlConnectionFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Internal;
using System.Data.Entity.Resources;
using System.Data.SqlClient;
using System.Globalization;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Instances of this class are used to create DbConnection objects for
  /// SQL Server based on a given database name or connection string. By default, the connection is
  /// made to '.\SQLEXPRESS'.  This can be changed by changing the base connection
  /// string when constructing a factory instance.
  /// </summary>
  /// <remarks>
  /// An instance of this class can be set on the <see cref="T:System.Data.Entity.Database" /> class to
  /// cause all DbContexts created with no connection information or just a database
  /// name or connection string to use SQL Server by default.
  /// This class is immutable since multiple threads may access instances simultaneously
  /// when creating connections.
  /// </remarks>
  public sealed class SqlConnectionFactory : IDbConnectionFactory
  {
    private readonly string _baseConnectionString;
    private Func<string, DbProviderFactory> _providerFactoryCreator;

    /// <summary>
    /// Creates a new connection factory with a default BaseConnectionString property of
    /// 'Data Source=.\SQLEXPRESS; Integrated Security=True; MultipleActiveResultSets=True;'.
    /// </summary>
    public SqlConnectionFactory() => this._baseConnectionString = "Data Source=.\\SQLEXPRESS; Integrated Security=True; MultipleActiveResultSets=True;";

    /// <summary>
    /// Creates a new connection factory with the given BaseConnectionString property.
    /// </summary>
    /// <param name="baseConnectionString"> The connection string to use for options to the database other than the 'Initial Catalog'. The 'Initial Catalog' will be prepended to this string based on the database name when CreateConnection is called. </param>
    public SqlConnectionFactory(string baseConnectionString)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(baseConnectionString, nameof (baseConnectionString));
      this._baseConnectionString = baseConnectionString;
    }

    internal Func<string, DbProviderFactory> ProviderFactory
    {
      get => this._providerFactoryCreator ?? new Func<string, DbProviderFactory>(((DbDependencyResolverExtensions) DbConfiguration.DependencyResolver).GetService<DbProviderFactory>);
      set => this._providerFactoryCreator = value;
    }

    /// <summary>
    /// The connection string to use for options to the database other than the 'Initial Catalog'.
    /// The 'Initial Catalog' will  be prepended to this string based on the database name when
    /// CreateConnection is called.
    /// The default is 'Data Source=.\SQLEXPRESS; Integrated Security=True;'.
    /// </summary>
    public string BaseConnectionString => this._baseConnectionString;

    /// <summary>
    /// Creates a connection for SQL Server based on the given database name or connection string.
    /// If the given string contains an '=' character then it is treated as a full connection string,
    /// otherwise it is treated as a database name only.
    /// </summary>
    /// <param name="nameOrConnectionString"> The database name or connection string. </param>
    /// <returns> An initialized DbConnection. </returns>
    public DbConnection CreateConnection(string nameOrConnectionString)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(nameOrConnectionString, nameof (nameOrConnectionString));
      string str = nameOrConnectionString;
      if (!DbHelpers.TreatAsConnectionString(nameOrConnectionString))
      {
        if (nameOrConnectionString.EndsWith(".mdf", true, (CultureInfo) null))
          throw Error.SqlConnectionFactory_MdfNotSupported((object) nameOrConnectionString);
        str = new SqlConnectionStringBuilder(this.BaseConnectionString)
        {
          InitialCatalog = nameOrConnectionString
        }.ConnectionString;
      }
      DbConnection connection;
      try
      {
        connection = this.ProviderFactory("System.Data.SqlClient").CreateConnection();
        DbInterception.Dispatch.Connection.SetConnectionString(connection, new DbConnectionPropertyInterceptionContext<string>().WithValue(str));
      }
      catch
      {
        connection = (DbConnection) new SqlConnection();
        DbInterception.Dispatch.Connection.SetConnectionString(connection, new DbConnectionPropertyInterceptionContext<string>().WithValue(str));
      }
      return connection;
    }
  }
}
