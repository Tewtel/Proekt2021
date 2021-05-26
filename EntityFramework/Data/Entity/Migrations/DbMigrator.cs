// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.DbMigrator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Internal;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.Edm;
using System.Data.Entity.Migrations.History;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Resources;
using System.Xml.Linq;

namespace System.Data.Entity.Migrations
{
  /// <summary>
  /// DbMigrator is used to apply existing migrations to a database.
  /// DbMigrator can be used to upgrade and downgrade to any given migration.
  /// To scaffold migrations based on changes to your model use <see cref="T:System.Data.Entity.Migrations.Design.MigrationScaffolder" />
  /// </summary>
  public class DbMigrator : MigratorBase
  {
    /// <summary>
    /// Migration Id representing the state of the database before any migrations are applied.
    /// </summary>
    public const string InitialDatabase = "0";
    private const string DefaultSchemaResourceKey = "DefaultSchema";
    private readonly Lazy<XDocument> _emptyModel;
    private readonly DbMigrationsConfiguration _configuration;
    private readonly XDocument _currentModel;
    private readonly DbProviderFactory _providerFactory;
    private readonly HistoryRepository _historyRepository;
    private readonly MigrationAssembly _migrationAssembly;
    private readonly DbContextInfo _usersContextInfo;
    private readonly EdmModelDiffer _modelDiffer;
    private readonly Lazy<ModificationCommandTreeGenerator> _modificationCommandTreeGenerator;
    private readonly DbContext _usersContext;
    private readonly Func<DbConnection, string, HistoryContext> _historyContextFactory;
    private readonly DbConnection _connection;
    private readonly bool _calledByCreateDatabase;
    private readonly DatabaseExistenceState _existenceState;
    private readonly string _providerManifestToken;
    private readonly string _targetDatabase;
    private readonly string _legacyContextKey;
    private readonly string _defaultSchema;
    private MigrationSqlGenerator _sqlGenerator;
    private bool _emptyMigrationNeeded;
    private bool _committedStatements;

    internal DbMigrator(
      DbContext usersContext = null,
      DbProviderFactory providerFactory = null,
      MigrationAssembly migrationAssembly = null)
      : base((MigratorBase) null)
    {
      this._usersContext = usersContext;
      this._providerFactory = providerFactory;
      this._migrationAssembly = migrationAssembly;
      this._usersContextInfo = new DbContextInfo(typeof (DbContext));
      this._configuration = new DbMigrationsConfiguration();
      this._calledByCreateDatabase = true;
    }

    /// <summary>Initializes a new instance of the DbMigrator class.</summary>
    /// <param name="configuration"> Configuration to be used for the migration process. </param>
    public DbMigrator(DbMigrationsConfiguration configuration)
      : this(configuration, (DbContext) null, DatabaseExistenceState.Unknown, false)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbMigrationsConfiguration>(configuration, nameof (configuration));
      System.Data.Entity.Utilities.Check.NotNull<Type>(configuration.ContextType, "configuration.ContextType");
    }

    /// <summary>
    /// Initializes a new instance of the DbMigrator class using the supplied context.
    /// Use this constructor when applying migrations from code to avoid having migrations attempt
    /// to create a context/connection for you.
    /// </summary>
    /// <param name="configuration"> Configuration to be used for the migration process. </param>
    /// <param name="context"> The <see cref="T:System.Data.Entity.DbContext" /> to use. </param>
    public DbMigrator(DbMigrationsConfiguration configuration, DbContext context)
      : this(configuration, context, DatabaseExistenceState.Unknown, false)
    {
    }

    internal DbMigrator(
      DbMigrationsConfiguration configuration,
      DbContext usersContext,
      DatabaseExistenceState existenceState,
      bool calledByCreateDatabase)
      : base((MigratorBase) null)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbMigrationsConfiguration>(configuration, nameof (configuration));
      System.Data.Entity.Utilities.Check.NotNull<Type>(configuration.ContextType, "configuration.ContextType");
      this._configuration = configuration;
      this._calledByCreateDatabase = calledByCreateDatabase;
      this._existenceState = existenceState;
      if (usersContext != null)
      {
        this._usersContextInfo = new DbContextInfo(usersContext);
      }
      else
      {
        this._usersContextInfo = configuration.TargetDatabase == null ? new DbContextInfo(configuration.ContextType) : new DbContextInfo(configuration.ContextType, configuration.TargetDatabase);
        if (!this._usersContextInfo.IsConstructible)
          throw System.Data.Entity.Resources.Error.ContextNotConstructible((object) configuration.ContextType);
      }
      this._modelDiffer = this._configuration.ModelDiffer;
      DbContext context = usersContext ?? this._usersContextInfo.CreateInstance();
      this._usersContext = context;
      try
      {
        this._migrationAssembly = new MigrationAssembly(this._configuration.MigrationsAssembly, this._configuration.MigrationsNamespace);
        this._currentModel = context.GetModel();
        this._connection = context.Database.Connection;
        this._providerFactory = DbProviderServices.GetProviderFactory(this._connection);
        this._defaultSchema = context.InternalContext.DefaultSchema ?? "dbo";
        this._historyContextFactory = this._configuration.GetHistoryContextFactory(this._usersContextInfo.ConnectionProviderName);
        this._historyRepository = new HistoryRepository(context.InternalContext, this._usersContextInfo.ConnectionString, this._providerFactory, this._configuration.ContextKey, this._configuration.CommandTimeout, this._historyContextFactory, ((IEnumerable<string>) new string[1]
        {
          this._defaultSchema
        }).Concat<string>(this.GetHistorySchemas()), this._usersContext, this._existenceState, (Func<Exception, bool>) (e => this.SqlGenerator.IsPermissionDeniedError(e)));
        this._providerManifestToken = context.InternalContext.ModelProviderInfo != null ? context.InternalContext.ModelProviderInfo.ProviderManifestToken : DbConfiguration.DependencyResolver.GetService<IManifestTokenResolver>().ResolveManifestToken(this._connection);
        DbModelBuilder modelBuilder = context.InternalContext.CodeFirstModel.CachedModelBuilder;
        this._modificationCommandTreeGenerator = new Lazy<ModificationCommandTreeGenerator>((Func<ModificationCommandTreeGenerator>) (() => new ModificationCommandTreeGenerator(modelBuilder.BuildDynamicUpdateModel(new DbProviderInfo(this._usersContextInfo.ConnectionProviderName, this._providerManifestToken)), this.CreateConnection())));
        DbInterceptionContext interceptionContext = new DbInterceptionContext().WithDbContext(this._usersContext);
        this._targetDatabase = System.Data.Entity.Resources.Strings.LoggingTargetDatabaseFormat((object) DbInterception.Dispatch.Connection.GetDataSource(this._connection, interceptionContext), (object) DbInterception.Dispatch.Connection.GetDatabase(this._connection, interceptionContext), (object) this._usersContextInfo.ConnectionProviderName, this._usersContextInfo.ConnectionStringOrigin == DbConnectionStringOrigin.DbContextInfo ? (object) System.Data.Entity.Resources.Strings.LoggingExplicit : (object) this._usersContextInfo.ConnectionStringOrigin.ToString());
        this._legacyContextKey = context.InternalContext.DefaultContextKey;
        this._emptyModel = this.GetEmptyModel();
      }
      finally
      {
        if (usersContext == null)
        {
          this._usersContext = (DbContext) null;
          this._connection = (DbConnection) null;
          context.Dispose();
        }
      }
    }

    private Lazy<XDocument> GetEmptyModel() => new Lazy<XDocument>((Func<XDocument>) (() => new DbModelBuilder().Build(new DbProviderInfo(this._usersContextInfo.ConnectionProviderName, this._providerManifestToken)).GetModel()));

    private XDocument GetHistoryModel(string defaultSchema)
    {
      DbConnection connection = (DbConnection) null;
      try
      {
        connection = this.CreateConnection();
        using (HistoryContext context = this._historyContextFactory(connection, defaultSchema))
          return context.GetModel();
      }
      finally
      {
        if (connection != null)
          DbInterception.Dispatch.Connection.Dispose(connection, new DbInterceptionContext());
      }
    }

    private IEnumerable<string> GetHistorySchemas() => this._migrationAssembly.MigrationIds.Select(migrationId => new
    {
      migrationId = migrationId,
      migration = this._migrationAssembly.GetMigration(migrationId)
    }).Select(_param1 => DbMigrator.GetDefaultSchema(_param1.migration));

    /// <summary>
    /// Gets the configuration that is being used for the migration process.
    /// </summary>
    public override DbMigrationsConfiguration Configuration => this._configuration;

    internal override string TargetDatabase => this._targetDatabase;

    private MigrationSqlGenerator SqlGenerator => this._sqlGenerator ?? (this._sqlGenerator = this._configuration.GetSqlGenerator(this._usersContextInfo.ConnectionProviderName));

    /// <summary>
    /// Gets all migrations that are defined in the configured migrations assembly.
    /// </summary>
    /// <returns>The list of migrations.</returns>
    public override IEnumerable<string> GetLocalMigrations() => this._migrationAssembly.MigrationIds;

    /// <summary>
    /// Gets all migrations that have been applied to the target database.
    /// </summary>
    /// <returns>The list of migrations.</returns>
    public override IEnumerable<string> GetDatabaseMigrations() => this._historyRepository.GetMigrationsSince("0");

    /// <summary>
    /// Gets all migrations that are defined in the assembly but haven't been applied to the target database.
    /// </summary>
    /// <returns>The list of migrations.</returns>
    public override IEnumerable<string> GetPendingMigrations() => this._historyRepository.GetPendingMigrations(this._migrationAssembly.MigrationIds);

    internal ScaffoldedMigration ScaffoldInitialCreate(string @namespace)
    {
      string migrationId;
      XDocument lastModel = this._historyRepository.GetLastModel(out migrationId, out string _, this._legacyContextKey);
      if (lastModel == null || !migrationId.MigrationName().Equals(System.Data.Entity.Resources.Strings.InitialCreate))
        return (ScaffoldedMigration) null;
      List<MigrationOperation> list = this._modelDiffer.Diff(this._emptyModel.Value, lastModel, this._modificationCommandTreeGenerator, this.SqlGenerator).ToList<MigrationOperation>();
      ScaffoldedMigration scaffoldedMigration = this._configuration.CodeGenerator.Generate(migrationId, (IEnumerable<MigrationOperation>) list, (string) null, Convert.ToBase64String(new ModelCompressor().Compress(this._currentModel)), @namespace, System.Data.Entity.Resources.Strings.InitialCreate);
      scaffoldedMigration.MigrationId = migrationId;
      scaffoldedMigration.Directory = this._configuration.MigrationsDirectory;
      scaffoldedMigration.Resources.Add("DefaultSchema", (object) this._defaultSchema);
      return scaffoldedMigration;
    }

    internal ScaffoldedMigration Scaffold(
      string migrationName,
      string @namespace,
      bool ignoreChanges)
    {
      string migrationId1 = (string) null;
      bool flag = false;
      List<string> list = this.GetPendingMigrations().ToList<string>();
      if (list.Any<string>())
      {
        string str = list.Last<string>();
        if (!str.EqualsIgnoreCase(migrationName) && !str.MigrationName().EqualsIgnoreCase(migrationName))
          throw System.Data.Entity.Resources.Error.MigrationsPendingException((object) list.Join<string>());
        flag = true;
        migrationId1 = str;
        migrationName = str.MigrationName();
      }
      XDocument sourceModel = (XDocument) null;
      this.CheckLegacyCompatibility((Action) (() => sourceModel = this._currentModel));
      string migrationId2 = (string) null;
      string productVersion = (string) null;
      sourceModel = sourceModel ?? this._historyRepository.GetLastModel(out migrationId2, out productVersion) ?? this._emptyModel.Value;
      IEnumerable<MigrationOperation> operations = ignoreChanges ? Enumerable.Empty<MigrationOperation>() : (IEnumerable<MigrationOperation>) this._modelDiffer.Diff(sourceModel, this._currentModel, this._modificationCommandTreeGenerator, this.SqlGenerator, productVersion).ToList<MigrationOperation>();
      if (!flag)
      {
        migrationName = this._migrationAssembly.UniquifyName(migrationName);
        migrationId1 = MigrationAssembly.CreateMigrationId(migrationName);
      }
      ModelCompressor modelCompressor = new ModelCompressor();
      ScaffoldedMigration scaffoldedMigration = this._configuration.CodeGenerator.Generate(migrationId1, operations, sourceModel == this._emptyModel.Value || sourceModel == this._currentModel || !migrationId2.IsAutomaticMigration() ? (string) null : Convert.ToBase64String(modelCompressor.Compress(sourceModel)), Convert.ToBase64String(modelCompressor.Compress(this._currentModel)), @namespace, migrationName);
      scaffoldedMigration.MigrationId = migrationId1;
      scaffoldedMigration.Directory = this._configuration.MigrationsDirectory;
      scaffoldedMigration.IsRescaffold = flag;
      scaffoldedMigration.Resources.Add("DefaultSchema", (object) this._defaultSchema);
      return scaffoldedMigration;
    }

    private void CheckLegacyCompatibility(Action onCompatible)
    {
      if (this._calledByCreateDatabase || this._historyRepository.Exists())
        return;
      DbContext dbContext = this._usersContext ?? this._usersContextInfo.CreateInstance();
      try
      {
        bool flag;
        try
        {
          flag = dbContext.Database.CompatibleWithModel(true);
        }
        catch
        {
          return;
        }
        if (!flag)
          throw System.Data.Entity.Resources.Error.MetadataOutOfDate();
        onCompatible();
      }
      finally
      {
        if (this._usersContext == null)
          dbContext.Dispose();
      }
    }

    /// <summary>Updates the target database to a given migration.</summary>
    /// <param name="targetMigration"> The migration to upgrade/downgrade to. </param>
    public override void Update(string targetMigration) => base.EnsureDatabaseExists((Action) (() => this.UpdateInternal(targetMigration)));

    private void UpdateInternal(string targetMigration)
    {
      IEnumerable<MigrationOperation> upgradeOperations = this._historyRepository.GetUpgradeOperations();
      if (upgradeOperations.Any<MigrationOperation>())
        base.UpgradeHistory(upgradeOperations);
      IEnumerable<string> strings = this.GetPendingMigrations();
      if (!strings.Any<string>())
        this.CheckLegacyCompatibility((Action) (() => this.ExecuteOperations(MigrationAssembly.CreateBootstrapMigrationId(), new VersionedModel(this._currentModel), Enumerable.Empty<MigrationOperation>(), (IEnumerable<MigrationOperation>) this._modelDiffer.Diff(this._emptyModel.Value, this.GetHistoryModel(this._defaultSchema), this._modificationCommandTreeGenerator, this.SqlGenerator), false)));
      string targetMigrationId = targetMigration;
      if (!string.IsNullOrWhiteSpace(targetMigrationId))
      {
        if (!targetMigrationId.IsValidMigrationId())
          targetMigrationId = !(targetMigrationId == System.Data.Entity.Resources.Strings.AutomaticMigration) ? this.GetMigrationId(targetMigration) : throw System.Data.Entity.Resources.Error.AutoNotValidTarget((object) System.Data.Entity.Resources.Strings.AutomaticMigration);
        if (strings.Any<string>((Func<string, bool>) (m => m.EqualsIgnoreCase(targetMigrationId))))
        {
          strings = strings.Where<string>((Func<string, bool>) (m => string.CompareOrdinal(m.ToLowerInvariant(), targetMigrationId.ToLowerInvariant()) <= 0));
        }
        else
        {
          strings = this._historyRepository.GetMigrationsSince(targetMigrationId);
          if (strings.Any<string>())
          {
            base.Downgrade(strings.Concat<string>((IEnumerable<string>) new string[1]
            {
              targetMigrationId
            }));
            return;
          }
        }
      }
      base.Upgrade(strings, targetMigrationId, (string) null);
    }

    internal override void UpgradeHistory(IEnumerable<MigrationOperation> upgradeOperations) => base.ExecuteStatements(this.SqlGenerator.Generate(upgradeOperations, this._providerManifestToken));

    internal override string GetMigrationId(string migration)
    {
      if (migration.IsValidMigrationId())
        return migration;
      return (this.GetPendingMigrations().SingleOrDefault<string>((Func<string, bool>) (m => m.MigrationName().EqualsIgnoreCase(migration))) ?? this._historyRepository.GetMigrationId(migration)) ?? throw System.Data.Entity.Resources.Error.MigrationNotFound((object) migration);
    }

    internal override void Upgrade(
      IEnumerable<string> pendingMigrations,
      string targetMigrationId,
      string lastMigrationId)
    {
      DbMigration lastMigration = (DbMigration) null;
      if (lastMigrationId != null)
        lastMigration = this._migrationAssembly.GetMigration(lastMigrationId);
      foreach (string pendingMigration in pendingMigrations)
      {
        DbMigration migration = this._migrationAssembly.GetMigration(pendingMigration);
        base.ApplyMigration(migration, lastMigration);
        lastMigration = migration;
        this._emptyMigrationNeeded = false;
        if (pendingMigration.EqualsIgnoreCase(targetMigrationId))
          break;
      }
      if (string.IsNullOrWhiteSpace(targetMigrationId) && (this._emptyMigrationNeeded && this._configuration.AutomaticMigrationsEnabled || this.IsModelOutOfDate(this._currentModel, lastMigration)))
      {
        if (!this._configuration.AutomaticMigrationsEnabled)
          throw System.Data.Entity.Resources.Error.AutomaticDisabledException();
        base.AutoMigrate(MigrationAssembly.CreateMigrationId(this._calledByCreateDatabase ? System.Data.Entity.Resources.Strings.InitialCreate : System.Data.Entity.Resources.Strings.AutomaticMigration), this._calledByCreateDatabase ? new VersionedModel(this._emptyModel.Value) : this.GetLastModel(lastMigration), new VersionedModel(this._currentModel), false);
      }
      if (this._calledByCreateDatabase || this.IsModelOutOfDate(this._currentModel, lastMigration))
        return;
      base.SeedDatabase();
    }

    internal override void SeedDatabase()
    {
      DbContext context = this._usersContext ?? this._usersContextInfo.CreateInstance();
      if (this._usersContext != null)
        context.InternalContext.UseTempObjectContext();
      try
      {
        this._configuration.OnSeed(context);
        context.SaveChanges();
      }
      finally
      {
        if (this._usersContext == null)
          context.Dispose();
        else
          context.InternalContext.DisposeTempObjectContext();
      }
    }

    internal virtual bool IsModelOutOfDate(XDocument model, DbMigration lastMigration)
    {
      VersionedModel lastModel = this.GetLastModel(lastMigration);
      return this._modelDiffer.Diff(lastModel.Model, model, sourceModelVersion: lastModel.Version).Any<MigrationOperation>();
    }

    private VersionedModel GetLastModel(
      DbMigration lastMigration,
      string currentMigrationId = null)
    {
      if (lastMigration != null)
        return lastMigration.GetTargetModel();
      string migrationId;
      string productVersion;
      XDocument lastModel = this._historyRepository.GetLastModel(out migrationId, out productVersion);
      return lastModel != null && (currentMigrationId == null || string.CompareOrdinal(migrationId, currentMigrationId) < 0) ? new VersionedModel(lastModel, productVersion) : new VersionedModel(this._emptyModel.Value);
    }

    internal override void Downgrade(IEnumerable<string> pendingMigrations)
    {
      for (int index = 0; index < pendingMigrations.Count<string>() - 1; ++index)
      {
        string migrationId1 = pendingMigrations.ElementAt<string>(index);
        DbMigration migration = this._migrationAssembly.GetMigration(migrationId1);
        string migrationId2 = pendingMigrations.ElementAt<string>(index + 1);
        string productVersion = (string) null;
        XDocument xdocument = migrationId2 != "0" ? this._historyRepository.GetModel(migrationId2, out productVersion) : this._emptyModel.Value;
        XDocument model = this._historyRepository.GetModel(migrationId1, out string _);
        if (migration == null)
          base.AutoMigrate(migrationId1, new VersionedModel(model), new VersionedModel(xdocument, productVersion), true);
        else
          base.RevertMigration(migrationId1, migration, xdocument);
      }
    }

    internal override void RevertMigration(
      string migrationId,
      DbMigration migration,
      XDocument targetModel)
    {
      IEnumerable<MigrationOperation> systemOperations = Enumerable.Empty<MigrationOperation>();
      string defaultSchema = DbMigrator.GetDefaultSchema(migration);
      XDocument historyModel1 = this.GetHistoryModel(defaultSchema);
      if (targetModel == this._emptyModel.Value && !this._historyRepository.IsShared())
      {
        systemOperations = (IEnumerable<MigrationOperation>) this._modelDiffer.Diff(historyModel1, this._emptyModel.Value);
      }
      else
      {
        string lastDefaultSchema = this.GetLastDefaultSchema(migrationId);
        if (!string.Equals(lastDefaultSchema, defaultSchema, StringComparison.Ordinal))
        {
          XDocument historyModel2 = this.GetHistoryModel(lastDefaultSchema);
          systemOperations = (IEnumerable<MigrationOperation>) this._modelDiffer.Diff(historyModel1, historyModel2);
        }
      }
      migration.Down();
      this.ExecuteOperations(migrationId, new VersionedModel(targetModel), migration.Operations, systemOperations, true);
    }

    internal override void ApplyMigration(DbMigration migration, DbMigration lastMigration)
    {
      IMigrationMetadata migrationMetadata = (IMigrationMetadata) migration;
      VersionedModel sourceModel1 = this.GetLastModel(lastMigration, migrationMetadata.Id);
      VersionedModel sourceModel2 = migration.GetSourceModel();
      VersionedModel targetModel = migration.GetTargetModel();
      if (sourceModel2 != null && this.IsModelOutOfDate(sourceModel2.Model, lastMigration))
      {
        base.AutoMigrate(migrationMetadata.Id.ToAutomaticMigrationId(), sourceModel1, sourceModel2, false);
        sourceModel1 = sourceModel2;
      }
      string defaultSchema = DbMigrator.GetDefaultSchema(migration);
      XDocument historyModel = this.GetHistoryModel(defaultSchema);
      IEnumerable<MigrationOperation> systemOperations = Enumerable.Empty<MigrationOperation>();
      if (sourceModel1.Model == this._emptyModel.Value && !base.HistoryExists())
      {
        systemOperations = (IEnumerable<MigrationOperation>) this._modelDiffer.Diff(this._emptyModel.Value, historyModel);
      }
      else
      {
        string lastDefaultSchema = this.GetLastDefaultSchema(migrationMetadata.Id);
        if (!string.Equals(lastDefaultSchema, defaultSchema, StringComparison.Ordinal))
          systemOperations = (IEnumerable<MigrationOperation>) this._modelDiffer.Diff(this.GetHistoryModel(lastDefaultSchema), historyModel);
      }
      migration.Up();
      this.ExecuteOperations(migrationMetadata.Id, targetModel, migration.Operations, systemOperations, false);
    }

    private static string GetDefaultSchema(DbMigration migration)
    {
      try
      {
        string str = new ResourceManager(migration.GetType()).GetString("DefaultSchema");
        return !string.IsNullOrWhiteSpace(str) ? str : "dbo";
      }
      catch (MissingManifestResourceException ex)
      {
        return "dbo";
      }
    }

    private string GetLastDefaultSchema(string migrationId)
    {
      string migrationId1 = this._migrationAssembly.MigrationIds.LastOrDefault<string>((Func<string, bool>) (m => string.CompareOrdinal(m, migrationId) < 0));
      return migrationId1 != null ? DbMigrator.GetDefaultSchema(this._migrationAssembly.GetMigration(migrationId1)) : "dbo";
    }

    internal override bool HistoryExists() => this._historyRepository.Exists();

    internal override void AutoMigrate(
      string migrationId,
      VersionedModel sourceModel,
      VersionedModel targetModel,
      bool downgrading)
    {
      IEnumerable<MigrationOperation> systemOperations = Enumerable.Empty<MigrationOperation>();
      if (!this._historyRepository.IsShared())
      {
        if (targetModel.Model == this._emptyModel.Value)
          systemOperations = (IEnumerable<MigrationOperation>) this._modelDiffer.Diff(this.GetHistoryModel("dbo"), this._emptyModel.Value);
        else if (sourceModel.Model == this._emptyModel.Value)
          systemOperations = (IEnumerable<MigrationOperation>) this._modelDiffer.Diff(this._emptyModel.Value, this._calledByCreateDatabase ? this.GetHistoryModel(this._defaultSchema) : this.GetHistoryModel("dbo"));
      }
      List<MigrationOperation> list = this._modelDiffer.Diff(sourceModel.Model, targetModel.Model, targetModel.Model == this._currentModel ? this._modificationCommandTreeGenerator : (Lazy<ModificationCommandTreeGenerator>) null, this.SqlGenerator, sourceModel.Version, targetModel.Version).ToList<MigrationOperation>();
      if (!this._calledByCreateDatabase && targetModel.Model == this._currentModel && !string.Equals(this.GetLastDefaultSchema(migrationId), this._defaultSchema, StringComparison.Ordinal))
        throw System.Data.Entity.Resources.Error.UnableToMoveHistoryTableWithAuto();
      if (!this._configuration.AutomaticMigrationDataLossAllowed && list.Any<MigrationOperation>((Func<MigrationOperation, bool>) (o => o.IsDestructiveChange)))
        throw System.Data.Entity.Resources.Error.AutomaticDataLoss();
      if (targetModel.Model != this._currentModel && list.Any<MigrationOperation>((Func<MigrationOperation, bool>) (o => o is ProcedureOperation)))
        throw System.Data.Entity.Resources.Error.AutomaticStaleFunctions((object) migrationId);
      this.ExecuteOperations(migrationId, targetModel, (IEnumerable<MigrationOperation>) list, systemOperations, downgrading, true);
    }

    private void ExecuteOperations(
      string migrationId,
      VersionedModel targetModel,
      IEnumerable<MigrationOperation> operations,
      IEnumerable<MigrationOperation> systemOperations,
      bool downgrading,
      bool auto = false)
    {
      this.FillInForeignKeyOperations(operations, targetModel.Model);
      List<AddForeignKeyOperation> list1 = operations.OfType<CreateTableOperation>().SelectMany((Func<CreateTableOperation, IEnumerable<AddForeignKeyOperation>>) (ct => operations.OfType<AddForeignKeyOperation>()), (ct, afk) => new
      {
        ct = ct,
        afk = afk
      }).Where(_param1 => _param1.ct.Name.EqualsIgnoreCase(_param1.afk.DependentTable)).Select(_param1 => _param1.afk).ToList<AddForeignKeyOperation>();
      List<MigrationOperation> list2 = operations.Except<MigrationOperation>((IEnumerable<MigrationOperation>) list1).Concat<MigrationOperation>((IEnumerable<MigrationOperation>) list1).Concat<MigrationOperation>(systemOperations).ToList<MigrationOperation>();
      CreateTableOperation createTableOperation = systemOperations.OfType<CreateTableOperation>().FirstOrDefault<CreateTableOperation>();
      if (createTableOperation != null)
        this._historyRepository.CurrentSchema = DatabaseName.Parse(createTableOperation.Name).Schema;
      MoveTableOperation moveTableOperation = systemOperations.OfType<MoveTableOperation>().FirstOrDefault<MoveTableOperation>();
      if (moveTableOperation != null)
      {
        this._historyRepository.CurrentSchema = moveTableOperation.NewSchema;
        moveTableOperation.ContextKey = this._configuration.ContextKey;
        moveTableOperation.IsSystem = true;
      }
      if (!downgrading)
        list2.Add(this._historyRepository.CreateInsertOperation(migrationId, targetModel));
      else if (!systemOperations.Any<MigrationOperation>((Func<MigrationOperation, bool>) (o => o is DropTableOperation)))
        list2.Add(this._historyRepository.CreateDeleteOperation(migrationId));
      IEnumerable<MigrationStatement> migrationStatements = base.GenerateStatements((IList<MigrationOperation>) list2, migrationId);
      if (auto)
        migrationStatements = migrationStatements.Distinct<MigrationStatement>((Func<MigrationStatement, MigrationStatement, bool>) ((m1, m2) => string.Equals(m1.Sql, m2.Sql, StringComparison.Ordinal)));
      base.ExecuteStatements(migrationStatements);
      this._historyRepository.ResetExists();
    }

    internal override IEnumerable<DbQueryCommandTree> CreateDiscoveryQueryTrees() => this._historyRepository.CreateDiscoveryQueryTrees();

    internal override IEnumerable<MigrationStatement> GenerateStatements(
      IList<MigrationOperation> operations,
      string migrationId)
    {
      return this.SqlGenerator.Generate((IEnumerable<MigrationOperation>) operations, this._providerManifestToken);
    }

    internal override void ExecuteStatements(
      IEnumerable<MigrationStatement> migrationStatements)
    {
      this.ExecuteStatements(migrationStatements, (DbTransaction) null);
    }

    internal void ExecuteStatements(
      IEnumerable<MigrationStatement> migrationStatements,
      DbTransaction existingTransaction)
    {
      DbConnection connection = (DbConnection) null;
      try
      {
        if (existingTransaction != null)
        {
          DbInterceptionContext interceptionContext = new DbInterceptionContext().WithDbContext(this._usersContext);
          this.ExecuteStatementsWithinTransaction(migrationStatements, existingTransaction, interceptionContext);
        }
        else
        {
          connection = this.CreateConnection();
          DbProviderServices.GetExecutionStrategy(connection).Execute((Action) (() => this.ExecuteStatementsInternal(migrationStatements, connection)));
        }
      }
      finally
      {
        if (connection != null)
          DbInterception.Dispatch.Connection.Dispose(connection, new DbInterceptionContext());
      }
    }

    private void ExecuteStatementsInternal(
      IEnumerable<MigrationStatement> migrationStatements,
      DbConnection connection)
    {
      DbContext context = this._usersContext ?? this._usersContextInfo.CreateInstance();
      DbInterceptionContext interceptionContext = new DbInterceptionContext().WithDbContext(context);
      TransactionHandler transactionHandler = (TransactionHandler) null;
      try
      {
        if (DbInterception.Dispatch.Connection.GetState(connection, interceptionContext) == ConnectionState.Broken)
          DbInterception.Dispatch.Connection.Close(connection, interceptionContext);
        if (DbInterception.Dispatch.Connection.GetState(connection, interceptionContext) == ConnectionState.Closed)
          DbInterception.Dispatch.Connection.Open(connection, interceptionContext);
        if (!(context is TransactionContext))
        {
          Func<TransactionHandler> service = DbConfiguration.DependencyResolver.GetService<Func<TransactionHandler>>((object) new ExecutionStrategyKey(DbConfiguration.DependencyResolver.GetService<IProviderInvariantName>((object) DbProviderServices.GetProviderFactory(connection)).Name, DbInterception.Dispatch.Connection.GetDataSource(connection, interceptionContext)));
          if (service != null)
          {
            transactionHandler = service();
            transactionHandler.Initialize(context, connection);
          }
        }
        this.ExecuteStatementsInternal(migrationStatements, connection, interceptionContext);
        this._committedStatements = true;
      }
      finally
      {
        transactionHandler?.Dispose();
        if (this._usersContext == null)
          context.Dispose();
      }
    }

    private void ExecuteStatementsInternal(
      IEnumerable<MigrationStatement> migrationStatements,
      DbConnection connection,
      DbTransaction transaction,
      DbInterceptionContext interceptionContext)
    {
      foreach (MigrationStatement migrationStatement in migrationStatements)
        base.ExecuteSql(migrationStatement, connection, transaction, interceptionContext);
    }

    private void ExecuteStatementsInternal(
      IEnumerable<MigrationStatement> migrationStatements,
      DbConnection connection,
      DbInterceptionContext interceptionContext)
    {
      List<MigrationStatement> source = new List<MigrationStatement>();
      foreach (MigrationStatement migrationStatement in migrationStatements.Where<MigrationStatement>((Func<MigrationStatement, bool>) (s => !string.IsNullOrEmpty(s.Sql))))
      {
        if (!migrationStatement.SuppressTransaction)
        {
          source.Add(migrationStatement);
        }
        else
        {
          if (source.Any<MigrationStatement>())
          {
            this.ExecuteStatementsWithinNewTransaction((IEnumerable<MigrationStatement>) source, connection, interceptionContext);
            source.Clear();
          }
          base.ExecuteSql(migrationStatement, connection, (DbTransaction) null, interceptionContext);
        }
      }
      if (!source.Any<MigrationStatement>())
        return;
      this.ExecuteStatementsWithinNewTransaction((IEnumerable<MigrationStatement>) source, connection, interceptionContext);
    }

    private void ExecuteStatementsWithinTransaction(
      IEnumerable<MigrationStatement> migrationStatements,
      DbTransaction transaction,
      DbInterceptionContext interceptionContext)
    {
      DbConnection connection = DbInterception.Dispatch.Transaction.GetConnection(transaction, interceptionContext);
      this.ExecuteStatementsInternal(migrationStatements, connection, transaction, interceptionContext);
    }

    private void ExecuteStatementsWithinNewTransaction(
      IEnumerable<MigrationStatement> migrationStatements,
      DbConnection connection,
      DbInterceptionContext interceptionContext)
    {
      BeginTransactionInterceptionContext interceptionContext1 = new BeginTransactionInterceptionContext(interceptionContext).WithIsolationLevel(IsolationLevel.Serializable);
      DbTransaction transaction = (DbTransaction) null;
      try
      {
        transaction = DbInterception.Dispatch.Connection.BeginTransaction(connection, interceptionContext1);
        this.ExecuteStatementsWithinTransaction(migrationStatements, transaction, interceptionContext);
        DbInterception.Dispatch.Transaction.Commit(transaction, interceptionContext);
      }
      finally
      {
        if (transaction != null)
          DbInterception.Dispatch.Transaction.Dispose(transaction, interceptionContext);
      }
    }

    internal override void ExecuteSql(
      MigrationStatement migrationStatement,
      DbConnection connection,
      DbTransaction transaction,
      DbInterceptionContext interceptionContext)
    {
      if (string.IsNullOrWhiteSpace(migrationStatement.Sql))
        return;
      using (InterceptableDbCommand interceptableDbCommand = this.ConfigureCommand(connection.CreateCommand(), migrationStatement.Sql, interceptionContext))
      {
        if (transaction != null)
          interceptableDbCommand.Transaction = transaction;
        interceptableDbCommand.ExecuteNonQuery();
      }
    }

    private InterceptableDbCommand ConfigureCommand(
      DbCommand command,
      string commandText,
      DbInterceptionContext interceptionContext)
    {
      command.CommandText = commandText;
      if (this._configuration.CommandTimeout.HasValue)
        command.CommandTimeout = this._configuration.CommandTimeout.Value;
      return new InterceptableDbCommand(command, interceptionContext);
    }

    private void FillInForeignKeyOperations(
      IEnumerable<MigrationOperation> operations,
      XDocument targetModel)
    {
      foreach (AddForeignKeyOperation foreignKeyOperation1 in operations.OfType<AddForeignKeyOperation>().Where<AddForeignKeyOperation>((Func<AddForeignKeyOperation, bool>) (fk => fk.PrincipalTable != null && !fk.PrincipalColumns.Any<string>())))
      {
        AddForeignKeyOperation foreignKeyOperation = foreignKeyOperation1;
        string principalTable = this.GetStandardizedTableName(foreignKeyOperation.PrincipalTable);
        string entitySetName = targetModel.Descendants(EdmXNames.Ssdl.EntitySetNames).Where<XElement>((Func<XElement, bool>) (es => new DatabaseName(es.TableAttribute(), es.SchemaAttribute()).ToString().EqualsIgnoreCase(principalTable))).Select<XElement, string>((Func<XElement, string>) (es => es.NameAttribute())).SingleOrDefault<string>();
        if (entitySetName != null)
        {
          targetModel.Descendants(EdmXNames.Ssdl.EntityTypeNames).Single<XElement>((Func<XElement, bool>) (et => et.NameAttribute().EqualsIgnoreCase(entitySetName))).Descendants(EdmXNames.Ssdl.PropertyRefNames).Each<XElement>((Action<XElement>) (pr => foreignKeyOperation.PrincipalColumns.Add(pr.NameAttribute())));
        }
        else
        {
          CreateTableOperation createTableOperation = operations.OfType<CreateTableOperation>().SingleOrDefault<CreateTableOperation>((Func<CreateTableOperation, bool>) (ct => this.GetStandardizedTableName(ct.Name).EqualsIgnoreCase(principalTable)));
          if (createTableOperation == null || createTableOperation.PrimaryKey == null)
            throw System.Data.Entity.Resources.Error.PartialFkOperation((object) foreignKeyOperation.DependentTable, (object) foreignKeyOperation.DependentColumns.Join<string>());
          createTableOperation.PrimaryKey.Columns.Each<string>((Action<string>) (c => foreignKeyOperation.PrincipalColumns.Add(c)));
        }
      }
    }

    private string GetStandardizedTableName(string tableName) => !string.IsNullOrWhiteSpace(DatabaseName.Parse(tableName).Schema) ? tableName : new DatabaseName(tableName, this._defaultSchema).ToString();

    internal override void EnsureDatabaseExists(Action mustSucceedToKeepDatabase)
    {
      bool flag = false;
      System.Data.Entity.Migrations.Utilities.DatabaseCreator databaseCreator = new System.Data.Entity.Migrations.Utilities.DatabaseCreator(this._configuration.CommandTimeout);
      DbConnection connection1 = (DbConnection) null;
      try
      {
        connection1 = this.CreateConnection();
        if (this._existenceState != DatabaseExistenceState.DoesNotExist)
        {
          if (this._existenceState == DatabaseExistenceState.Unknown)
          {
            if (databaseCreator.Exists(connection1))
              goto label_8;
          }
          else
            goto label_8;
        }
        databaseCreator.Create(connection1);
        flag = true;
      }
      finally
      {
        if (connection1 != null)
          DbInterception.Dispatch.Connection.Dispose(connection1, new DbInterceptionContext());
      }
label_8:
      this._emptyMigrationNeeded = flag;
      try
      {
        this._committedStatements = false;
        mustSucceedToKeepDatabase();
      }
      catch
      {
        if (flag && !this._committedStatements)
        {
          DbConnection connection2 = (DbConnection) null;
          try
          {
            connection2 = this.CreateConnection();
            databaseCreator.Delete(connection2);
          }
          catch
          {
          }
          finally
          {
            if (connection2 != null)
              DbInterception.Dispatch.Connection.Dispose(connection2, new DbInterceptionContext());
          }
        }
        throw;
      }
    }

    private DbConnection CreateConnection()
    {
      DbConnection connection = this._connection == null ? this._providerFactory.CreateConnection() : DbProviderServices.GetProviderServices(this._connection).CloneDbConnection(this._connection, this._providerFactory);
      DbConnectionPropertyInterceptionContext<string> interceptionContext = new DbConnectionPropertyInterceptionContext<string>().WithValue(this._usersContextInfo.ConnectionString);
      if (this._usersContext != null)
        interceptionContext = interceptionContext.WithDbContext(this._usersContext);
      DbInterception.Dispatch.Connection.SetConnectionString(connection, interceptionContext);
      return connection;
    }
  }
}
