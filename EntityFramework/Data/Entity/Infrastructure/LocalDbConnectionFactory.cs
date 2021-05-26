// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.LocalDbConnectionFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Globalization;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Instances of this class are used to create DbConnection objects for
  /// SQL Server LocalDb based on a given database name or connection string.
  /// </summary>
  /// <remarks>
  /// An instance of this class can be set on the <see cref="T:System.Data.Entity.Database" /> class or in the
  /// app.config/web.config for the application to cause all DbContexts created with no
  /// connection information or just a database name to use SQL Server LocalDb by default.
  /// This class is immutable since multiple threads may access instances simultaneously
  /// when creating connections.
  /// </remarks>
  public sealed class LocalDbConnectionFactory : IDbConnectionFactory
  {
    private readonly string _baseConnectionString;
    private readonly string _localDbVersion;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Infrastructure.LocalDbConnectionFactory" /> class.
    /// </summary>
    public LocalDbConnectionFactory()
      : this("mssqllocaldb")
    {
    }

    /// <summary>
    /// Creates a new instance of the connection factory for the given version of LocalDb.
    /// For SQL Server 2012 LocalDb use "v11.0".
    /// For SQL Server 2014 and later LocalDb use "mssqllocaldb".
    /// </summary>
    /// <param name="localDbVersion"> The LocalDb version to use. </param>
    public LocalDbConnectionFactory(string localDbVersion)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(localDbVersion, nameof (localDbVersion));
      this._localDbVersion = localDbVersion;
      this._baseConnectionString = "Integrated Security=True; MultipleActiveResultSets=True;";
    }

    /// <summary>
    /// Creates a new instance of the connection factory for the given version of LocalDb.
    /// For SQL Server 2012 LocalDb use "v11.0".
    /// For SQL Server 2014 and later LocalDb use "mssqllocaldb".
    /// </summary>
    /// <param name="localDbVersion"> The LocalDb version to use. </param>
    /// <param name="baseConnectionString"> The connection string to use for options to the database other than the 'Initial Catalog', 'Data Source', and 'AttachDbFilename'. The 'Initial Catalog' and 'AttachDbFilename' will be prepended to this string based on the database name when CreateConnection is called. The 'Data Source' will be set based on the LocalDbVersion argument. </param>
    public LocalDbConnectionFactory(string localDbVersion, string baseConnectionString)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(localDbVersion, nameof (localDbVersion));
      System.Data.Entity.Utilities.Check.NotNull<string>(baseConnectionString, nameof (baseConnectionString));
      this._localDbVersion = localDbVersion;
      this._baseConnectionString = baseConnectionString;
    }

    /// <summary>
    /// The connection string to use for options to the database other than the 'Initial Catalog',
    /// 'Data Source', and 'AttachDbFilename'.
    /// The 'Initial Catalog' and 'AttachDbFilename' will be prepended to this string based on the
    /// database name when CreateConnection is called.
    /// The 'Data Source' will be set based on the LocalDbVersion argument.
    /// The default is 'Integrated Security=True;'.
    /// </summary>
    public string BaseConnectionString => this._baseConnectionString;

    /// <summary>
    /// Creates a connection for SQL Server LocalDb based on the given database name or connection string.
    /// If the given string contains an '=' character then it is treated as a full connection string,
    /// otherwise it is treated as a database name only.
    /// </summary>
    /// <param name="nameOrConnectionString"> The database name or connection string. </param>
    /// <returns> An initialized DbConnection. </returns>
    public DbConnection CreateConnection(string nameOrConnectionString)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(nameOrConnectionString, nameof (nameOrConnectionString));
      string str = " ";
      if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.GetData("DataDirectory") as string))
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, " AttachDbFilename=|DataDirectory|{0}.mdf; ", (object) nameOrConnectionString);
      return new SqlConnectionFactory(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Data Source=(localdb)\\{1};{0};{2}", (object) this._baseConnectionString, (object) this._localDbVersion, (object) str)).CreateConnection(nameOrConnectionString);
    }
  }
}
