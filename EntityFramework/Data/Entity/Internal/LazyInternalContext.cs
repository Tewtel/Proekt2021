// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.LazyInternalContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Migrations.History;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration.Utilities;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Internal
{
  internal class LazyInternalContext : InternalContext
  {
    private static readonly CreateDatabaseIfNotExists<DbContext> _defaultCodeFirstInitializer = new CreateDatabaseIfNotExists<DbContext>();
    private static readonly ConcurrentDictionary<IDbModelCacheKey, RetryLazy<LazyInternalContext, DbCompiledModel>> _cachedModels = new ConcurrentDictionary<IDbModelCacheKey, RetryLazy<LazyInternalContext, DbCompiledModel>>();
    private static readonly ConcurrentDictionary<Tuple<DbCompiledModel, string>, RetryAction<InternalContext>> InitializedDatabases = new ConcurrentDictionary<Tuple<DbCompiledModel, string>, RetryAction<InternalContext>>();
    private IInternalConnection _internalConnection;
    private bool _creatingModel;
    private ObjectContext _objectContext;
    private DbCompiledModel _model;
    private readonly bool _createdWithExistingModel;
    private bool _initialEnsureTransactionsForFunctionsAndCommands = true;
    private bool _initialLazyLoadingFlag = true;
    private bool _initialProxyCreationFlag = true;
    private bool _useDatabaseNullSemanticsFlag;
    private int? _commandTimeout;
    private bool _inDatabaseInitialization;
    private Action<DbModelBuilder> _onModelCreating;
    private readonly Func<DbContext, IDbModelCacheKey> _cacheKeyFactory;
    private readonly AttributeProvider _attributeProvider;
    private DbModel _modelBeingInitialized;
    private DbProviderInfo _modelProviderInfo;
    private bool _disableFilterOverProjectionSimplificationForCustomFunctions;

    public LazyInternalContext(
      DbContext owner,
      IInternalConnection internalConnection,
      DbCompiledModel model,
      Func<DbContext, IDbModelCacheKey> cacheKeyFactory = null,
      AttributeProvider attributeProvider = null,
      Lazy<DbDispatchers> dispatchers = null,
      ObjectContext objectContext = null)
      : base(owner, dispatchers)
    {
      this._internalConnection = internalConnection;
      this._model = model;
      this._cacheKeyFactory = cacheKeyFactory ?? new Func<DbContext, IDbModelCacheKey>(new DefaultModelCacheKeyFactory().Create);
      this._attributeProvider = attributeProvider ?? new AttributeProvider();
      this._objectContext = objectContext;
      this._createdWithExistingModel = model != null;
      this.LoadContextConfigs();
    }

    public override ObjectContext ObjectContext
    {
      get
      {
        this.Initialize();
        return this.ObjectContextInUse;
      }
    }

    public override DbCompiledModel CodeFirstModel
    {
      get
      {
        this.InitializeContext();
        return this._model;
      }
    }

    public override DbModel ModelBeingInitialized
    {
      get
      {
        this.InitializeContext();
        return this._modelBeingInitialized;
      }
    }

    public override ObjectContext GetObjectContextWithoutDatabaseInitialization()
    {
      this.InitializeContext();
      return this.ObjectContextInUse;
    }

    public virtual ObjectContext ObjectContextInUse => this.TempObjectContext ?? this._objectContext;

    public override int SaveChanges() => this.ObjectContextInUse != null ? base.SaveChanges() : 0;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return this.ObjectContextInUse != null ? base.SaveChangesAsync(cancellationToken) : Task.FromResult<int>(0);
    }

    public override void DisposeContext(bool disposing)
    {
      if (this.IsDisposed)
        return;
      base.DisposeContext(disposing);
      if (!disposing)
        return;
      if (this._objectContext != null)
        this._objectContext.Dispose();
      this._internalConnection.Dispose();
    }

    public override DbConnection Connection
    {
      get
      {
        this.CheckContextNotDisposed();
        return this.TempObjectContext != null ? ((EntityConnection) this.TempObjectContext.Connection).StoreConnection : this._internalConnection.Connection;
      }
    }

    public override string OriginalConnectionString => this._internalConnection.OriginalConnectionString;

    public override DbConnectionStringOrigin ConnectionStringOrigin
    {
      get
      {
        this.CheckContextNotDisposed();
        return this._internalConnection.ConnectionStringOrigin;
      }
    }

    public override AppConfig AppConfig
    {
      get => base.AppConfig;
      set
      {
        base.AppConfig = value;
        this._internalConnection.AppConfig = value;
      }
    }

    public override string ConnectionStringName
    {
      get
      {
        this.CheckContextNotDisposed();
        return this._internalConnection.ConnectionStringName;
      }
    }

    public override DbProviderInfo ModelProviderInfo
    {
      get
      {
        this.CheckContextNotDisposed();
        return this._modelProviderInfo;
      }
      set
      {
        this.CheckContextNotDisposed();
        this._modelProviderInfo = value;
        this._internalConnection.ProviderName = this._modelProviderInfo.ProviderInvariantName;
      }
    }

    public override string ProviderName => this._internalConnection.ProviderName;

    public override Action<DbModelBuilder> OnModelCreating
    {
      get
      {
        this.CheckContextNotDisposed();
        return this._onModelCreating;
      }
      set
      {
        this.CheckContextNotDisposed();
        this._onModelCreating = value;
      }
    }

    public override void OverrideConnection(IInternalConnection connection)
    {
      connection.AppConfig = this.AppConfig;
      if (connection.ConnectionHasModel != this._internalConnection.ConnectionHasModel)
        throw this._internalConnection.ConnectionHasModel ? System.Data.Entity.Resources.Error.LazyInternalContext_CannotReplaceEfConnectionWithDbConnection() : System.Data.Entity.Resources.Error.LazyInternalContext_CannotReplaceDbConnectionWithEfConnection();
      this._internalConnection.Dispose();
      this._internalConnection = connection;
    }

    protected override void InitializeContext()
    {
      this.CheckContextNotDisposed();
      if (this._objectContext != null)
        return;
      if (this._creatingModel)
        throw System.Data.Entity.Resources.Error.DbContext_ContextUsedInModelCreating();
      try
      {
        DbContextInfo currentInfo = DbContextInfo.CurrentInfo;
        if (currentInfo != null)
          this.ApplyContextInfo(currentInfo);
        this._creatingModel = true;
        if (this._createdWithExistingModel)
        {
          if (this._internalConnection.ConnectionHasModel)
            throw System.Data.Entity.Resources.Error.DbContext_ConnectionHasModel();
          this._objectContext = this._model.CreateObjectContext<ObjectContext>(this._internalConnection.Connection);
        }
        else if (this._internalConnection.ConnectionHasModel)
        {
          this._objectContext = this._internalConnection.CreateObjectContextFromConnectionModel();
        }
        else
        {
          IDbModelCacheKey key = this._cacheKeyFactory(this.Owner);
          DbCompiledModel dbCompiledModel = LazyInternalContext._cachedModels.GetOrAdd(key, (Func<IDbModelCacheKey, RetryLazy<LazyInternalContext, DbCompiledModel>>) (t => new RetryLazy<LazyInternalContext, DbCompiledModel>(new Func<LazyInternalContext, DbCompiledModel>(LazyInternalContext.CreateModel)))).GetValue(this);
          this._objectContext = dbCompiledModel.CreateObjectContext<ObjectContext>(this._internalConnection.Connection);
          this._model = dbCompiledModel;
        }
        this._objectContext.ContextOptions.EnsureTransactionsForFunctionsAndCommands = this._initialEnsureTransactionsForFunctionsAndCommands;
        this._objectContext.ContextOptions.LazyLoadingEnabled = this._initialLazyLoadingFlag;
        this._objectContext.ContextOptions.ProxyCreationEnabled = this._initialProxyCreationFlag;
        this._objectContext.ContextOptions.UseCSharpNullComparisonBehavior = !this._useDatabaseNullSemanticsFlag;
        this._objectContext.ContextOptions.DisableFilterOverProjectionSimplificationForCustomFunctions = this._disableFilterOverProjectionSimplificationForCustomFunctions;
        this._objectContext.CommandTimeout = this._commandTimeout;
        this._objectContext.ContextOptions.UseConsistentNullReferenceBehavior = true;
        this._objectContext.InterceptionContext = this._objectContext.InterceptionContext.WithDbContext(this.Owner);
        this.ResetDbSets();
        this._objectContext.InitializeMappingViewCacheFactory(this.Owner);
      }
      finally
      {
        this._creatingModel = false;
      }
    }

    public static DbCompiledModel CreateModel(LazyInternalContext internalContext)
    {
      Type type = internalContext.Owner.GetType();
      DbModelStore dbModelStore = (DbModelStore) null;
      if (!(internalContext.Owner is HistoryContext))
      {
        dbModelStore = DbConfiguration.DependencyResolver.GetService<DbModelStore>();
        if (dbModelStore != null)
        {
          DbCompiledModel dbCompiledModel = dbModelStore.TryLoad(type);
          if (dbCompiledModel != null)
            return dbCompiledModel;
        }
      }
      DbModelBuilder modelBuilder = internalContext.CreateModelBuilder();
      DbModel model = internalContext._modelProviderInfo == null ? modelBuilder.Build(internalContext._internalConnection.Connection) : modelBuilder.Build(internalContext._modelProviderInfo);
      internalContext._modelBeingInitialized = model;
      dbModelStore?.Save(type, model);
      return model.Compile();
    }

    public DbModelBuilder CreateModelBuilder()
    {
      DbModelBuilderVersionAttribute versionAttribute = this._attributeProvider.GetAttributes(this.Owner.GetType()).OfType<DbModelBuilderVersionAttribute>().FirstOrDefault<DbModelBuilderVersionAttribute>();
      DbModelBuilder modelBuilder = new DbModelBuilder(versionAttribute != null ? versionAttribute.Version : DbModelBuilderVersion.Latest);
      string modelNamespace = LazyInternalContext.StripInvalidCharacters(this.Owner.GetType().Namespace);
      if (!string.IsNullOrWhiteSpace(modelNamespace))
        modelBuilder.Conventions.Add((IConvention) new ModelNamespaceConvention(modelNamespace));
      string containerName = LazyInternalContext.StripInvalidCharacters(this.Owner.GetType().Name);
      if (!string.IsNullOrWhiteSpace(containerName))
        modelBuilder.Conventions.Add((IConvention) new ModelContainerConvention(containerName));
      new DbSetDiscoveryService(this.Owner).RegisterSets(modelBuilder);
      this.Owner.CallOnModelCreating(modelBuilder);
      if (this.OnModelCreating != null)
        this.OnModelCreating(modelBuilder);
      return modelBuilder;
    }

    private static string StripInvalidCharacters(string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder(value.Length);
      bool flag = true;
      foreach (char c in value)
      {
        if (c == '.')
        {
          if (!flag)
            stringBuilder.Append(c);
        }
        else
        {
          switch (char.GetUnicodeCategory(c))
          {
            case UnicodeCategory.UppercaseLetter:
            case UnicodeCategory.LowercaseLetter:
            case UnicodeCategory.TitlecaseLetter:
            case UnicodeCategory.ModifierLetter:
            case UnicodeCategory.OtherLetter:
            case UnicodeCategory.LetterNumber:
              flag = false;
              stringBuilder.Append(c);
              continue;
            case UnicodeCategory.NonSpacingMark:
            case UnicodeCategory.SpacingCombiningMark:
            case UnicodeCategory.DecimalDigitNumber:
            case UnicodeCategory.ConnectorPunctuation:
              if (!flag)
              {
                stringBuilder.Append(c);
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      return stringBuilder.ToString();
    }

    public override void MarkDatabaseNotInitialized()
    {
      if (this.InInitializationAction)
        return;
      LazyInternalContext.InitializedDatabases.TryRemove(Tuple.Create<DbCompiledModel, string>(this._model, this._internalConnection.ConnectionKey), out RetryAction<InternalContext> _);
    }

    public override void MarkDatabaseInitialized()
    {
      this.InitializeContext();
      this.InitializeDatabaseAction((Action<InternalContext>) (c => { }));
    }

    protected override void InitializeDatabase() => this.InitializeDatabaseAction((Action<InternalContext>) (c => c.PerformDatabaseInitialization()));

    private void InitializeDatabaseAction(Action<InternalContext> action)
    {
      if (this._inDatabaseInitialization)
        return;
      if (this.InitializerDisabled)
        return;
      try
      {
        this._inDatabaseInitialization = true;
        LazyInternalContext.InitializedDatabases.GetOrAdd(Tuple.Create<DbCompiledModel, string>(this._model, this._internalConnection.ConnectionKey), (Func<Tuple<DbCompiledModel, string>, RetryAction<InternalContext>>) (t => new RetryAction<InternalContext>(action))).PerformAction((InternalContext) this);
      }
      finally
      {
        this._inDatabaseInitialization = false;
        this._modelBeingInitialized = (DbModel) null;
      }
    }

    public override IDatabaseInitializer<DbContext> DefaultInitializer => this._model == null ? (IDatabaseInitializer<DbContext>) null : (IDatabaseInitializer<DbContext>) LazyInternalContext._defaultCodeFirstInitializer;

    public override bool EnsureTransactionsForFunctionsAndCommands
    {
      get
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        return objectContextInUse == null ? this._initialEnsureTransactionsForFunctionsAndCommands : objectContextInUse.ContextOptions.EnsureTransactionsForFunctionsAndCommands;
      }
      set
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        if (objectContextInUse != null)
          objectContextInUse.ContextOptions.EnsureTransactionsForFunctionsAndCommands = value;
        else
          this._initialEnsureTransactionsForFunctionsAndCommands = value;
      }
    }

    public override bool LazyLoadingEnabled
    {
      get
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        return objectContextInUse == null ? this._initialLazyLoadingFlag : objectContextInUse.ContextOptions.LazyLoadingEnabled;
      }
      set
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        if (objectContextInUse != null)
          objectContextInUse.ContextOptions.LazyLoadingEnabled = value;
        else
          this._initialLazyLoadingFlag = value;
      }
    }

    public override bool ProxyCreationEnabled
    {
      get
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        return objectContextInUse == null ? this._initialProxyCreationFlag : objectContextInUse.ContextOptions.ProxyCreationEnabled;
      }
      set
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        if (objectContextInUse != null)
          objectContextInUse.ContextOptions.ProxyCreationEnabled = value;
        else
          this._initialProxyCreationFlag = value;
      }
    }

    public override bool UseDatabaseNullSemantics
    {
      get
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        return objectContextInUse == null ? this._useDatabaseNullSemanticsFlag : !objectContextInUse.ContextOptions.UseCSharpNullComparisonBehavior;
      }
      set
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        if (objectContextInUse != null)
          objectContextInUse.ContextOptions.UseCSharpNullComparisonBehavior = !value;
        else
          this._useDatabaseNullSemanticsFlag = value;
      }
    }

    public override bool DisableFilterOverProjectionSimplificationForCustomFunctions
    {
      get
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        return objectContextInUse == null ? this._disableFilterOverProjectionSimplificationForCustomFunctions : !objectContextInUse.ContextOptions.DisableFilterOverProjectionSimplificationForCustomFunctions;
      }
      set
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        if (objectContextInUse != null)
          objectContextInUse.ContextOptions.DisableFilterOverProjectionSimplificationForCustomFunctions = !value;
        else
          this._disableFilterOverProjectionSimplificationForCustomFunctions = value;
      }
    }

    public override int? CommandTimeout
    {
      get
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        return objectContextInUse == null ? this._commandTimeout : objectContextInUse.CommandTimeout;
      }
      set
      {
        ObjectContext objectContextInUse = this.ObjectContextInUse;
        if (objectContextInUse != null)
          objectContextInUse.CommandTimeout = value;
        else
          this._commandTimeout = value;
      }
    }

    public override string DefaultSchema => this.CodeFirstModel.DefaultSchema;
  }
}
