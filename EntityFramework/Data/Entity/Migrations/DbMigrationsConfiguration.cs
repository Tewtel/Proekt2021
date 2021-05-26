// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.DbMigrationsConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.History;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.Resources;
using System.IO;
using System.Reflection;

namespace System.Data.Entity.Migrations
{
  /// <summary>
  /// Configuration relating to the use of migrations for a given model.
  /// You will typically create a configuration class that derives
  /// from <see cref="T:System.Data.Entity.Migrations.DbMigrationsConfiguration`1" /> rather than
  /// using this class.
  /// </summary>
  public class DbMigrationsConfiguration
  {
    /// <summary>The default directory that migrations are stored in.</summary>
    public const string DefaultMigrationsDirectory = "Migrations";
    private readonly Dictionary<string, MigrationSqlGenerator> _sqlGenerators = new Dictionary<string, MigrationSqlGenerator>();
    private readonly Dictionary<string, Func<DbConnection, string, HistoryContext>> _historyContextFactories = new Dictionary<string, Func<DbConnection, string, HistoryContext>>();
    private MigrationCodeGenerator _codeGenerator;
    private Type _contextType;
    private Assembly _migrationsAssembly;
    private EdmModelDiffer _modelDiffer = new EdmModelDiffer();
    private DbConnectionInfo _connectionInfo;
    private string _migrationsDirectory = "Migrations";
    private readonly Lazy<IDbDependencyResolver> _resolver;
    private string _contextKey;
    private int? _commandTimeout;

    /// <summary>
    /// Initializes a new instance of the DbMigrationsConfiguration class.
    /// </summary>
    public DbMigrationsConfiguration()
      : this(new Lazy<IDbDependencyResolver>((Func<IDbDependencyResolver>) (() => DbConfiguration.DependencyResolver)))
    {
      this.CodeGenerator = (MigrationCodeGenerator) new CSharpMigrationCodeGenerator();
      this.ContextKey = this.GetType().ToString();
    }

    internal DbMigrationsConfiguration(Lazy<IDbDependencyResolver> resolver) => this._resolver = resolver;

    /// <summary>
    /// Gets or sets a value indicating if automatic migrations can be used when migrating the database.
    /// </summary>
    public bool AutomaticMigrationsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the string used to distinguish migrations belonging to this configuration
    /// from migrations belonging to other configurations using the same database.
    /// This property enables migrations from multiple different models to be applied to a single database.
    /// </summary>
    public string ContextKey
    {
      get => this._contextKey;
      set
      {
        System.Data.Entity.Utilities.Check.NotEmpty(value, nameof (value));
        this._contextKey = value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating if data loss is acceptable during automatic migration.
    /// If set to false an exception will be thrown if data loss may occur as part of an automatic migration.
    /// </summary>
    public bool AutomaticMigrationDataLossAllowed { get; set; }

    /// <summary>
    /// Adds a new SQL generator to be used for a given database provider.
    /// </summary>
    /// <param name="providerInvariantName"> Name of the database provider to set the SQL generator for. </param>
    /// <param name="migrationSqlGenerator"> The SQL generator to be used. </param>
    public void SetSqlGenerator(
      string providerInvariantName,
      MigrationSqlGenerator migrationSqlGenerator)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(providerInvariantName, nameof (providerInvariantName));
      System.Data.Entity.Utilities.Check.NotNull<MigrationSqlGenerator>(migrationSqlGenerator, nameof (migrationSqlGenerator));
      this._sqlGenerators[providerInvariantName] = migrationSqlGenerator;
    }

    /// <summary>
    /// Gets the SQL generator that is set to be used with a given database provider.
    /// </summary>
    /// <param name="providerInvariantName"> Name of the database provider to get the SQL generator for. </param>
    /// <returns> The SQL generator that is set for the database provider. </returns>
    public MigrationSqlGenerator GetSqlGenerator(string providerInvariantName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(providerInvariantName, nameof (providerInvariantName));
      MigrationSqlGenerator migrationSqlGenerator;
      if (!this._sqlGenerators.TryGetValue(providerInvariantName, out migrationSqlGenerator))
      {
        Func<MigrationSqlGenerator> service = this._resolver.Value.GetService<Func<MigrationSqlGenerator>>((object) providerInvariantName);
        if (service == null)
          throw Error.NoSqlGeneratorForProvider((object) providerInvariantName);
        migrationSqlGenerator = service();
      }
      return migrationSqlGenerator;
    }

    /// <summary>
    /// Adds a new factory for creating <see cref="T:System.Data.Entity.Migrations.History.HistoryContext" /> instances to be used for a given database provider.
    /// </summary>
    /// <param name="providerInvariantName"> Name of the database provider to set the SQL generator for. </param>
    /// <param name="factory">
    /// A factory for creating <see cref="T:System.Data.Entity.Migrations.History.HistoryContext" /> instances for a given <see cref="T:System.Data.Common.DbConnection" /> and
    /// <see cref="T:System.String" /> representing the default schema.
    /// </param>
    public void SetHistoryContextFactory(
      string providerInvariantName,
      Func<DbConnection, string, HistoryContext> factory)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(providerInvariantName, nameof (providerInvariantName));
      System.Data.Entity.Utilities.Check.NotNull<Func<DbConnection, string, HistoryContext>>(factory, nameof (factory));
      this._historyContextFactories[providerInvariantName] = factory;
    }

    /// <summary>
    /// Gets the history context factory that is set to be used with a given database provider.
    /// </summary>
    /// <param name="providerInvariantName"> Name of the database provider to get thefactory for. </param>
    /// <returns> The history context factory that is set for the database provider. </returns>
    public Func<DbConnection, string, HistoryContext> GetHistoryContextFactory(
      string providerInvariantName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(providerInvariantName, nameof (providerInvariantName));
      Func<DbConnection, string, HistoryContext> func;
      return !this._historyContextFactories.TryGetValue(providerInvariantName, out func) ? this._resolver.Value.GetService<Func<DbConnection, string, HistoryContext>>((object) providerInvariantName) ?? this._resolver.Value.GetService<Func<DbConnection, string, HistoryContext>>() : func;
    }

    /// <summary>
    /// Gets or sets the derived DbContext representing the model to be migrated.
    /// </summary>
    public Type ContextType
    {
      get => this._contextType;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<Type>(value, nameof (value));
        this._contextType = typeof (DbContext).IsAssignableFrom(value) ? value : throw new ArgumentException(Strings.DbMigrationsConfiguration_ContextType((object) value.Name));
        DbConfigurationManager.Instance.EnsureLoadedForContext(this._contextType);
      }
    }

    /// <summary>
    /// Gets or sets the namespace used for code-based migrations.
    /// </summary>
    public string MigrationsNamespace { get; set; }

    /// <summary>
    /// Gets or sets the sub-directory that code-based migrations are stored in.
    /// Note that this property must be set to a relative path for a sub-directory under the
    /// Visual Studio project root; it cannot be set to an absolute path.
    /// </summary>
    public string MigrationsDirectory
    {
      get => this._migrationsDirectory;
      set
      {
        System.Data.Entity.Utilities.Check.NotEmpty(value, nameof (value));
        this._migrationsDirectory = !Path.IsPathRooted(value) ? value : throw new MigrationsException(Strings.DbMigrationsConfiguration_RootedPath((object) value));
      }
    }

    /// <summary>
    /// Gets or sets the code generator to be used when scaffolding migrations.
    /// </summary>
    public MigrationCodeGenerator CodeGenerator
    {
      get => this._codeGenerator;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<MigrationCodeGenerator>(value, nameof (value));
        this._codeGenerator = value;
      }
    }

    /// <summary>
    /// Gets or sets the assembly containing code-based migrations.
    /// </summary>
    public Assembly MigrationsAssembly
    {
      get => this._migrationsAssembly;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<Assembly>(value, nameof (value));
        this._migrationsAssembly = value;
      }
    }

    /// <summary>
    /// Gets or sets a value to override the connection of the database to be migrated.
    /// </summary>
    public DbConnectionInfo TargetDatabase
    {
      get => this._connectionInfo;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<DbConnectionInfo>(value, nameof (value));
        this._connectionInfo = value;
      }
    }

    /// <summary>
    /// Gets or sets the timeout value used for the individual commands within a
    /// migration.
    /// </summary>
    /// <returns>
    /// The time in seconds to wait for the command to execute. A null value indicates
    /// that the default value of the underlying provider will be used.
    /// </returns>
    public int? CommandTimeout
    {
      get => this._commandTimeout;
      set
      {
        if (value.HasValue)
        {
          int? nullable = value;
          int num = 0;
          if (nullable.GetValueOrDefault() < num & nullable.HasValue)
            throw new ArgumentException(Strings.ObjectContext_InvalidCommandTimeout);
        }
        this._commandTimeout = value;
      }
    }

    internal virtual void OnSeed(DbContext context)
    {
    }

    internal EdmModelDiffer ModelDiffer
    {
      get => this._modelDiffer;
      set => this._modelDiffer = value;
    }
  }
}
