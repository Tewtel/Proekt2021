﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.MigrateDatabaseToLatestVersion`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Migrations;

namespace System.Data.Entity
{
  /// <summary>
  /// An implementation of <see cref="T:System.Data.Entity.IDatabaseInitializer`1" /> that will use Code First Migrations
  /// to update the database to the latest version.
  /// </summary>
  /// <typeparam name="TContext">The type of the context.</typeparam>
  /// <typeparam name="TMigrationsConfiguration">The type of the migrations configuration to use during initialization.</typeparam>
  public class MigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration> : 
    IDatabaseInitializer<TContext>
    where TContext : DbContext
    where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
  {
    private readonly DbMigrationsConfiguration _config;
    private readonly bool _useSuppliedContext;

    static MigrateDatabaseToLatestVersion() => DbConfigurationManager.Instance.EnsureLoadedForContext(typeof (TContext));

    /// <summary>
    /// Initializes a new instance of the MigrateDatabaseToLatestVersion class that will use
    /// the connection information from a context constructed using the default constructor
    /// or registered factory if applicable
    /// </summary>
    public MigrateDatabaseToLatestVersion()
      : this(false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the MigrateDatabaseToLatestVersion class specifying whether to
    /// use the connection information from the context that triggered initialization to perform the migration.
    /// </summary>
    /// <param name="useSuppliedContext">
    /// If set to <c>true</c> the initializer is run using the connection information from the context that
    /// triggered initialization. Otherwise, the connection information will be taken from a context constructed
    /// using the default constructor or registered factory if applicable.
    /// </param>
    public MigrateDatabaseToLatestVersion(bool useSuppliedContext)
      : this(useSuppliedContext, new TMigrationsConfiguration())
    {
    }

    /// <summary>
    /// Initializes a new instance of the MigrateDatabaseToLatestVersion class specifying whether to
    /// use the connection information from the context that triggered initialization to perform the migration.
    /// Also allows specifying migrations configuration to use during initialization.
    /// </summary>
    /// <param name="useSuppliedContext">
    /// If set to <c>true</c> the initializer is run using the connection information from the context that
    /// triggered initialization. Otherwise, the connection information will be taken from a context constructed
    /// using the default constructor or registered factory if applicable.
    /// </param>
    /// <param name="configuration"> Migrations configuration to use during initialization. </param>
    public MigrateDatabaseToLatestVersion(
      bool useSuppliedContext,
      TMigrationsConfiguration configuration)
    {
      System.Data.Entity.Utilities.Check.NotNull<TMigrationsConfiguration>(configuration, nameof (configuration));
      this._config = (DbMigrationsConfiguration) configuration;
      this._useSuppliedContext = useSuppliedContext;
    }

    /// <summary>
    /// Initializes a new instance of the MigrateDatabaseToLatestVersion class that will
    /// use a specific connection string from the configuration file to connect to
    /// the database to perform the migration.
    /// </summary>
    /// <param name="connectionStringName"> The name of the connection string to use for migration. </param>
    public MigrateDatabaseToLatestVersion(string connectionStringName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(connectionStringName, nameof (connectionStringName));
      TMigrationsConfiguration migrationsConfiguration = new TMigrationsConfiguration();
      migrationsConfiguration.TargetDatabase = new DbConnectionInfo(connectionStringName);
      this._config = (DbMigrationsConfiguration) migrationsConfiguration;
    }

    /// <inheritdoc />
    public virtual void InitializeDatabase(TContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<TContext>(context, nameof (context));
      new DbMigrator(this._config, (DbContext) (this._useSuppliedContext ? context : default (TContext))).Update();
    }
  }
}
