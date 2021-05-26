﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Design.MigrationScaffolder
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Migrations.Design
{
  /// <summary>
  /// Scaffolds code-based migrations to apply pending model changes to the database.
  /// </summary>
  public class MigrationScaffolder
  {
    private readonly DbMigrator _migrator;
    private string _namespace;
    private bool _namespaceSpecified;

    /// <summary>
    /// Initializes a new instance of the MigrationScaffolder class.
    /// </summary>
    /// <param name="migrationsConfiguration"> Configuration to be used for scaffolding. </param>
    public MigrationScaffolder(DbMigrationsConfiguration migrationsConfiguration)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbMigrationsConfiguration>(migrationsConfiguration, nameof (migrationsConfiguration));
      this._migrator = new DbMigrator(migrationsConfiguration);
    }

    /// <summary>
    /// Gets or sets the namespace used in the migration's generated code.
    /// By default, this is the same as MigrationsNamespace on the migrations
    /// configuration object passed into the constructor. For VB.NET projects, this
    /// will need to be updated to take into account the project's root namespace.
    /// </summary>
    public string Namespace
    {
      get => !this._namespaceSpecified ? this._migrator.Configuration.MigrationsNamespace : this._namespace;
      set
      {
        this._namespaceSpecified = this._migrator.Configuration.MigrationsNamespace != value;
        this._namespace = value;
      }
    }

    /// <summary>
    /// Scaffolds a code based migration to apply any pending model changes to the database.
    /// </summary>
    /// <param name="migrationName"> The name to use for the scaffolded migration. </param>
    /// <returns> The scaffolded migration. </returns>
    public virtual ScaffoldedMigration Scaffold(string migrationName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(migrationName, nameof (migrationName));
      return this._migrator.Scaffold(migrationName, this.Namespace, false);
    }

    /// <summary>
    /// Scaffolds a code based migration to apply any pending model changes to the database.
    /// </summary>
    /// <param name="migrationName"> The name to use for the scaffolded migration. </param>
    /// <param name="ignoreChanges"> Whether or not to include model changes. </param>
    /// <returns> The scaffolded migration. </returns>
    public virtual ScaffoldedMigration Scaffold(
      string migrationName,
      bool ignoreChanges)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(migrationName, nameof (migrationName));
      return this._migrator.Scaffold(migrationName, this.Namespace, ignoreChanges);
    }

    /// <summary>
    /// Scaffolds the initial code-based migration corresponding to a previously run database initializer.
    /// </summary>
    /// <returns> The scaffolded migration. </returns>
    public virtual ScaffoldedMigration ScaffoldInitialCreate() => this._migrator.ScaffoldInitialCreate(this.Namespace);
  }
}
