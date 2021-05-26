// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.DbProviderServices
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Resources;
using System.Data.Entity.Spatial;
using System.Data.Entity.Utilities;
using System.Transactions;
using System.Xml;

namespace System.Data.Entity.Core.Common
{
  /// <summary>
  /// The factory for building command definitions; use the type of this object
  /// as the argument to the IServiceProvider.GetService method on the provider
  /// factory;
  /// </summary>
  public abstract class DbProviderServices : IDbDependencyResolver
  {
    private readonly Lazy<IDbDependencyResolver> _resolver;
    private readonly Lazy<DbCommandTreeDispatcher> _treeDispatcher;
    private static readonly ConcurrentDictionary<DbProviderInfo, DbSpatialServices> _spatialServices = new ConcurrentDictionary<DbProviderInfo, DbSpatialServices>();
    private static readonly ConcurrentDictionary<ExecutionStrategyKey, Func<IDbExecutionStrategy>> _executionStrategyFactories = new ConcurrentDictionary<ExecutionStrategyKey, Func<IDbExecutionStrategy>>();
    private readonly ResolverChain _resolvers = new ResolverChain();

    /// <summary>
    /// Constructs an EF provider that will use the <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver" /> obtained from
    /// the app domain <see cref="T:System.Data.Entity.DbConfiguration" /> Singleton for resolving EF dependencies such
    /// as the <see cref="T:System.Data.Entity.Spatial.DbSpatialServices" /> instance to use.
    /// </summary>
    protected DbProviderServices()
      : this((Func<IDbDependencyResolver>) (() => DbConfiguration.DependencyResolver))
    {
    }

    internal DbProviderServices(Func<IDbDependencyResolver> resolver)
      : this(resolver, new Lazy<DbCommandTreeDispatcher>((Func<DbCommandTreeDispatcher>) (() => DbInterception.Dispatch.CommandTree)))
    {
    }

    internal DbProviderServices(
      Func<IDbDependencyResolver> resolver,
      Lazy<DbCommandTreeDispatcher> treeDispatcher)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<IDbDependencyResolver>>(resolver, nameof (resolver));
      this._resolver = new Lazy<IDbDependencyResolver>(resolver);
      this._treeDispatcher = treeDispatcher;
    }

    /// <summary>
    /// Registers a handler to process non-error messages coming from the database provider.
    /// </summary>
    /// <param name="connection">The connection to receive information for.</param>
    /// <param name="handler">The handler to process messages.</param>
    public virtual void RegisterInfoMessageHandler(DbConnection connection, Action<string> handler)
    {
    }

    /// <summary>
    /// Create a Command Definition object given a command tree.
    /// </summary>
    /// <param name="commandTree"> command tree for the statement </param>
    /// <returns> an executable command definition object </returns>
    /// <remarks>
    /// This method simply delegates to the provider's implementation of CreateDbCommandDefinition.
    /// </remarks>
    public DbCommandDefinition CreateCommandDefinition(DbCommandTree commandTree)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommandTree>(commandTree, nameof (commandTree));
      return this.CreateCommandDefinition(commandTree, new DbInterceptionContext());
    }

    internal DbCommandDefinition CreateCommandDefinition(
      DbCommandTree commandTree,
      DbInterceptionContext interceptionContext)
    {
      this.ValidateDataSpace(commandTree);
      StoreItemCollection itemCollection = (StoreItemCollection) commandTree.MetadataWorkspace.GetItemCollection(DataSpace.SSpace);
      commandTree = this._treeDispatcher.Value.Created(commandTree, interceptionContext);
      return this.CreateDbCommandDefinition(itemCollection.ProviderManifest, commandTree, interceptionContext);
    }

    internal virtual DbCommandDefinition CreateDbCommandDefinition(
      DbProviderManifest providerManifest,
      DbCommandTree commandTree,
      DbInterceptionContext interceptionContext)
    {
      return this.CreateDbCommandDefinition(providerManifest, commandTree);
    }

    /// <summary>Creates command definition from specified manifest and command tree.</summary>
    /// <returns>The created command definition.</returns>
    /// <param name="providerManifest">The manifest.</param>
    /// <param name="commandTree">The command tree.</param>
    public DbCommandDefinition CreateCommandDefinition(
      DbProviderManifest providerManifest,
      DbCommandTree commandTree)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbProviderManifest>(providerManifest, nameof (providerManifest));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandTree>(commandTree, nameof (commandTree));
      try
      {
        return this.CreateDbCommandDefinition(providerManifest, commandTree);
      }
      catch (ProviderIncompatibleException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          throw new ProviderIncompatibleException(Strings.ProviderDidNotCreateACommandDefinition, ex);
        throw;
      }
    }

    /// <summary>Creates a command definition object for the specified provider manifest and command tree.</summary>
    /// <returns>An executable command definition object.</returns>
    /// <param name="providerManifest">Provider manifest previously retrieved from the store provider.</param>
    /// <param name="commandTree">Command tree for the statement.</param>
    protected abstract DbCommandDefinition CreateDbCommandDefinition(
      DbProviderManifest providerManifest,
      DbCommandTree commandTree);

    internal virtual void ValidateDataSpace(DbCommandTree commandTree)
    {
      if (commandTree.DataSpace != DataSpace.SSpace)
        throw new ProviderIncompatibleException(Strings.ProviderRequiresStoreCommandTree);
    }

    internal virtual DbCommand CreateCommand(
      DbCommandTree commandTree,
      DbInterceptionContext interceptionContext)
    {
      return this.CreateCommandDefinition(commandTree, interceptionContext).CreateCommand();
    }

    /// <summary>
    /// Create the default DbCommandDefinition object based on the prototype command
    /// This method is intended for provider writers to build a default command definition
    /// from a command.
    /// Note: This will clone the prototype
    /// </summary>
    /// <param name="prototype"> the prototype command </param>
    /// <returns> an executable command definition object </returns>
    public virtual DbCommandDefinition CreateCommandDefinition(
      DbCommand prototype)
    {
      return new DbCommandDefinition(prototype, new Func<DbCommand, DbCommand>(this.CloneDbCommand));
    }

    /// <summary>
    /// See issue 2390 - cloning the DesignTimeVisible property on the
    /// DbCommand can cause deadlocks. So here allow sub-classes to override.
    /// </summary>
    /// <param name="fromDbCommand"> the <see cref="T:System.Data.Common.DbCommand" /> object to clone </param>
    /// <returns>a clone of the <see cref="T:System.Data.Common.DbCommand" /> </returns>
    protected virtual DbCommand CloneDbCommand(DbCommand fromDbCommand)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(fromDbCommand, nameof (fromDbCommand));
      return fromDbCommand is ICloneable cloneable ? (DbCommand) cloneable.Clone() : throw new ProviderIncompatibleException(Strings.EntityClient_CannotCloneStoreProvider);
    }

    /// <summary>Clones the connection.</summary>
    /// <param name="connection">The original connection.</param>
    /// <returns>Cloned connection</returns>
    public virtual DbConnection CloneDbConnection(DbConnection connection) => this.CloneDbConnection(connection, DbProviderServices.GetProviderFactory(connection));

    /// <summary>Clones the connection.</summary>
    /// <param name="connection">The original connection.</param>
    /// <param name="factory">The factory to use.</param>
    /// <returns>Cloned connection</returns>
    public virtual DbConnection CloneDbConnection(
      DbConnection connection,
      DbProviderFactory factory)
    {
      return factory.CreateConnection();
    }

    /// <summary>Returns provider manifest token given a connection.</summary>
    /// <returns>The provider manifest token.</returns>
    /// <param name="connection">Connection to provider.</param>
    public string GetProviderManifestToken(DbConnection connection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      try
      {
        string providerManifestToken;
        using (new TransactionScope(TransactionScopeOption.Suppress))
          providerManifestToken = this.GetDbProviderManifestToken(connection);
        return providerManifestToken != null ? providerManifestToken : throw new ProviderIncompatibleException(Strings.ProviderDidNotReturnAProviderManifestToken);
      }
      catch (ProviderIncompatibleException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          throw new ProviderIncompatibleException(Strings.ProviderDidNotReturnAProviderManifestToken, ex);
        throw;
      }
    }

    /// <summary>
    /// Returns provider manifest token for a given connection.
    /// </summary>
    /// <param name="connection"> Connection to find manifest token from. </param>
    /// <returns> The provider manifest token for the specified connection. </returns>
    protected abstract string GetDbProviderManifestToken(DbConnection connection);

    /// <summary>Returns the provider manifest by using the specified version information.</summary>
    /// <returns>The provider manifest by using the specified version information.</returns>
    /// <param name="manifestToken">The token information associated with the provider manifest.</param>
    public DbProviderManifest GetProviderManifest(string manifestToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(manifestToken, nameof (manifestToken));
      try
      {
        return this.GetDbProviderManifest(manifestToken) ?? throw new ProviderIncompatibleException(Strings.ProviderDidNotReturnAProviderManifest);
      }
      catch (ProviderIncompatibleException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          throw new ProviderIncompatibleException(Strings.ProviderDidNotReturnAProviderManifest, ex);
        throw;
      }
    }

    /// <summary>When overridden in a derived class, returns an instance of a class that derives from the DbProviderManifest.</summary>
    /// <returns>A DbProviderManifest object that represents the provider manifest.</returns>
    /// <param name="manifestToken">The token information associated with the provider manifest.</param>
    protected abstract DbProviderManifest GetDbProviderManifest(
      string manifestToken);

    /// <summary>
    /// Gets the <see cref="T:System.Data.Entity.Infrastructure.IDbExecutionStrategy" /> that will be used to execute methods that use the specified connection.
    /// </summary>
    /// <param name="connection">The database connection</param>
    /// <returns>
    /// A new instance of <see cref="T:System.Data.Entity.Infrastructure.DbExecutionStrategy" />
    /// </returns>
    public static IDbExecutionStrategy GetExecutionStrategy(
      DbConnection connection)
    {
      return DbProviderServices.GetExecutionStrategy(connection, DbProviderServices.GetProviderFactory(connection));
    }

    internal static IDbExecutionStrategy GetExecutionStrategy(
      DbConnection connection,
      MetadataWorkspace metadataWorkspace)
    {
      StoreItemCollection itemCollection = (StoreItemCollection) metadataWorkspace.GetItemCollection(DataSpace.SSpace);
      return DbProviderServices.GetExecutionStrategy(connection, itemCollection.ProviderFactory);
    }

    /// <summary>
    /// Gets the <see cref="T:System.Data.Entity.Infrastructure.IDbExecutionStrategy" /> that will be used to execute methods that use the specified connection.
    /// This overload should be used by the derived classes for compatability with wrapping providers.
    /// </summary>
    /// <param name="connection">The database connection</param>
    /// <param name="providerInvariantName">The provider invariant name</param>
    /// <returns>
    /// A new instance of <see cref="T:System.Data.Entity.Infrastructure.DbExecutionStrategy" />
    /// </returns>
    protected static IDbExecutionStrategy GetExecutionStrategy(
      DbConnection connection,
      string providerInvariantName)
    {
      return DbProviderServices.GetExecutionStrategy(connection, DbProviderServices.GetProviderFactory(connection), providerInvariantName);
    }

    private static IDbExecutionStrategy GetExecutionStrategy(
      DbConnection connection,
      DbProviderFactory providerFactory,
      string providerInvariantName = null)
    {
      if (connection is EntityConnection entityConnection)
        connection = entityConnection.StoreConnection;
      string dataSource = DbInterception.Dispatch.Connection.GetDataSource(connection, new DbInterceptionContext());
      ExecutionStrategyKey key = new ExecutionStrategyKey(providerFactory.GetType().FullName, dataSource);
      return DbProviderServices._executionStrategyFactories.GetOrAdd(key, (Func<ExecutionStrategyKey, Func<IDbExecutionStrategy>>) (k => DbConfiguration.DependencyResolver.GetService<Func<IDbExecutionStrategy>>((object) new ExecutionStrategyKey(providerInvariantName ?? DbConfiguration.DependencyResolver.GetService<IProviderInvariantName>((object) providerFactory).Name, dataSource))))();
    }

    /// <summary>
    /// Gets the spatial data reader for the <see cref="T:System.Data.Entity.Core.Common.DbProviderServices" />.
    /// </summary>
    /// <returns>The spatial data reader.</returns>
    /// <param name="fromReader">The reader where the spatial data came from.</param>
    /// <param name="manifestToken">The manifest token associated with the provider manifest.</param>
    public DbSpatialDataReader GetSpatialDataReader(
      DbDataReader fromReader,
      string manifestToken)
    {
      try
      {
        return this.GetDbSpatialDataReader(fromReader, manifestToken);
      }
      catch (ProviderIncompatibleException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          throw new ProviderIncompatibleException(Strings.ProviderDidNotReturnSpatialServices, ex);
        throw;
      }
    }

    /// <summary>
    /// Gets the spatial services for the <see cref="T:System.Data.Entity.Core.Common.DbProviderServices" />.
    /// </summary>
    /// <returns>The spatial services.</returns>
    /// <param name="manifestToken">The token information associated with the provider manifest.</param>
    [Obsolete("Use GetSpatialServices(DbProviderInfo) or DbConfiguration to ensure the configured spatial services are used. See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.")]
    public DbSpatialServices GetSpatialServices(string manifestToken)
    {
      try
      {
        return this.DbGetSpatialServices(manifestToken);
      }
      catch (ProviderIncompatibleException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new ProviderIncompatibleException(Strings.ProviderDidNotReturnSpatialServices, ex);
      }
    }

    internal static DbSpatialServices GetSpatialServices(
      IDbDependencyResolver resolver,
      EntityConnection connection)
    {
      StoreItemCollection itemCollection = (StoreItemCollection) connection.GetMetadataWorkspace().GetItemCollection(DataSpace.SSpace);
      DbProviderInfo key = new DbProviderInfo(itemCollection.ProviderInvariantName, itemCollection.ProviderManifestToken);
      return DbProviderServices.GetSpatialServices(resolver, key, (Func<DbProviderServices>) (() => DbProviderServices.GetProviderServices(connection.StoreConnection)));
    }

    /// <summary>Gets the spatial services for the <see cref="T:System.Data.Entity.Core.Common.DbProviderServices" />.</summary>
    /// <returns>The spatial services.</returns>
    /// <param name="key">Information about the database that the spatial services will be used for.</param>
    public DbSpatialServices GetSpatialServices(DbProviderInfo key) => DbProviderServices.GetSpatialServices(this._resolver.Value, key, (Func<DbProviderServices>) (() => this));

    private static DbSpatialServices GetSpatialServices(
      IDbDependencyResolver resolver,
      DbProviderInfo key,
      Func<DbProviderServices> providerServices)
    {
      return DbProviderServices._spatialServices.GetOrAdd(key, (Func<DbProviderInfo, DbSpatialServices>) (k => resolver.GetService<DbSpatialServices>((object) k) ?? providerServices().GetSpatialServices(k.ProviderManifestToken) ?? resolver.GetService<DbSpatialServices>())) ?? throw new ProviderIncompatibleException(Strings.ProviderDidNotReturnSpatialServices);
    }

    /// <summary>
    /// Gets the spatial data reader for the <see cref="T:System.Data.Entity.Core.Common.DbProviderServices" />.
    /// </summary>
    /// <returns>The spatial data reader.</returns>
    /// <param name="fromReader">The reader where the spatial data came from.</param>
    /// <param name="manifestToken">The token information associated with the provider manifest.</param>
    protected virtual DbSpatialDataReader GetDbSpatialDataReader(
      DbDataReader fromReader,
      string manifestToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDataReader>(fromReader, nameof (fromReader));
      return (DbSpatialDataReader) null;
    }

    /// <summary>
    /// Gets the spatial services for the <see cref="T:System.Data.Entity.Core.Common.DbProviderServices" />.
    /// </summary>
    /// <returns>The spatial services.</returns>
    /// <param name="manifestToken">The token information associated with the provider manifest.</param>
    [Obsolete("Return DbSpatialServices from the GetService method. See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.")]
    protected virtual DbSpatialServices DbGetSpatialServices(string manifestToken) => (DbSpatialServices) null;

    /// <summary>
    /// Sets the parameter value and appropriate facets for the given <see cref="T:System.Data.Entity.Core.Metadata.Edm.TypeUsage" />.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <param name="parameterType">The type of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    public void SetParameterValue(DbParameter parameter, TypeUsage parameterType, object value)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbParameter>(parameter, nameof (parameter));
      System.Data.Entity.Utilities.Check.NotNull<TypeUsage>(parameterType, nameof (parameterType));
      this.SetDbParameterValue(parameter, parameterType, value);
    }

    /// <summary>
    /// Sets the parameter value and appropriate facets for the given <see cref="T:System.Data.Entity.Core.Metadata.Edm.TypeUsage" />.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <param name="parameterType">The type of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    protected virtual void SetDbParameterValue(
      DbParameter parameter,
      TypeUsage parameterType,
      object value)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbParameter>(parameter, nameof (parameter));
      System.Data.Entity.Utilities.Check.NotNull<TypeUsage>(parameterType, nameof (parameterType));
      parameter.Value = value;
    }

    /// <summary>Returns providers given a connection.</summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.Common.DbProviderServices" /> instanced based on the specified connection.
    /// </returns>
    /// <param name="connection">Connection to provider.</param>
    public static DbProviderServices GetProviderServices(DbConnection connection) => DbProviderServices.GetProviderFactory(connection).GetProviderServices();

    /// <summary>Retrieves the DbProviderFactory based on the specified DbConnection.</summary>
    /// <returns>The retrieved DbProviderFactory.</returns>
    /// <param name="connection">The connection to use.</param>
    public static DbProviderFactory GetProviderFactory(DbConnection connection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      return connection.GetProviderFactory() ?? throw new ProviderIncompatibleException(Strings.EntityClient_ReturnedNullOnProviderMethod((object) "get_ProviderFactory", (object) connection.GetType().ToString()));
    }

    /// <summary>
    /// Return an XML reader which represents the CSDL description
    /// </summary>
    /// <param name="csdlName">The name of the CSDL description.</param>
    /// <returns> An XmlReader that represents the CSDL description </returns>
    public static XmlReader GetConceptualSchemaDefinition(string csdlName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(csdlName, nameof (csdlName));
      return DbProviderServices.GetXmlResource("System.Data.Resources.DbProviderServices." + csdlName + ".csdl");
    }

    internal static XmlReader GetXmlResource(string resourceName) => XmlReader.Create(typeof (DbProviderServices).Assembly().GetManifestResourceStream(resourceName) ?? throw Error.InvalidResourceName((object) resourceName));

    /// <summary>Generates a data definition language (DDL script that creates schema objects (tables, primary keys, foreign keys) based on the contents of the StoreItemCollection parameter and targeted for the version of the database corresponding to the provider manifest token.</summary>
    /// <remarks>
    /// Individual statements should be separated using database-specific DDL command separator.
    /// It is expected that the generated script would be executed in the context of existing database with
    /// sufficient permissions, and it should not include commands to create the database, but it may include
    /// commands to create schemas and other auxiliary objects such as sequences, etc.
    /// </remarks>
    /// <returns>A DDL script that creates schema objects based on the contents of the StoreItemCollection parameter and targeted for the version of the database corresponding to the provider manifest token.</returns>
    /// <param name="providerManifestToken">The provider manifest token identifying the target version.</param>
    /// <param name="storeItemCollection">The structure of the database.</param>
    public string CreateDatabaseScript(
      string providerManifestToken,
      StoreItemCollection storeItemCollection)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(providerManifestToken, nameof (providerManifestToken));
      System.Data.Entity.Utilities.Check.NotNull<StoreItemCollection>(storeItemCollection, nameof (storeItemCollection));
      return this.DbCreateDatabaseScript(providerManifestToken, storeItemCollection);
    }

    /// <summary>
    /// Generates a data definition language (DDL) script that creates schema objects
    /// (tables, primary keys, foreign keys) based on the contents of the StoreItemCollection
    /// parameter and targeted for the version of the database corresponding to the provider manifest token.
    /// </summary>
    /// <remarks>
    /// Individual statements should be separated using database-specific DDL command separator.
    /// It is expected that the generated script would be executed in the context of existing database with
    /// sufficient permissions, and it should not include commands to create the database, but it may include
    /// commands to create schemas and other auxiliary objects such as sequences, etc.
    /// </remarks>
    /// <param name="providerManifestToken"> The provider manifest token identifying the target version. </param>
    /// <param name="storeItemCollection"> The structure of the database. </param>
    /// <returns>
    /// A DDL script that creates schema objects based on the contents of the StoreItemCollection parameter
    /// and targeted for the version of the database corresponding to the provider manifest token.
    /// </returns>
    protected virtual string DbCreateDatabaseScript(
      string providerManifestToken,
      StoreItemCollection storeItemCollection)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(providerManifestToken, nameof (providerManifestToken));
      System.Data.Entity.Utilities.Check.NotNull<StoreItemCollection>(storeItemCollection, nameof (storeItemCollection));
      throw new ProviderIncompatibleException(Strings.ProviderDoesNotSupportCreateDatabaseScript);
    }

    /// <summary>
    /// Creates a database indicated by connection and creates schema objects
    /// (tables, primary keys, foreign keys) based on the contents of storeItemCollection.
    /// </summary>
    /// <param name="connection">Connection to a non-existent database that needs to be created and populated with the store objects indicated with the storeItemCollection parameter.</param>
    /// <param name="commandTimeout">Execution timeout for any commands needed to create the database.</param>
    /// <param name="storeItemCollection">The collection of all store items based on which the script should be created.</param>
    public void CreateDatabase(
      DbConnection connection,
      int? commandTimeout,
      StoreItemCollection storeItemCollection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<StoreItemCollection>(storeItemCollection, nameof (storeItemCollection));
      this.DbCreateDatabase(connection, commandTimeout, storeItemCollection);
    }

    /// <summary>Creates a database indicated by connection and creates schema objects (tables, primary keys, foreign keys) based on the contents of a StoreItemCollection.</summary>
    /// <param name="connection">Connection to a non-existent database that needs to be created and populated with the store objects indicated with the storeItemCollection parameter.</param>
    /// <param name="commandTimeout">Execution timeout for any commands needed to create the database.</param>
    /// <param name="storeItemCollection">The collection of all store items based on which the script should be created.</param>
    protected virtual void DbCreateDatabase(
      DbConnection connection,
      int? commandTimeout,
      StoreItemCollection storeItemCollection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<StoreItemCollection>(storeItemCollection, nameof (storeItemCollection));
      throw new ProviderIncompatibleException(Strings.ProviderDoesNotSupportCreateDatabase);
    }

    /// <summary>Returns a value indicating whether a given database exists on the server.</summary>
    /// <returns>True if the provider can deduce the database only based on the connection.</returns>
    /// <param name="connection">Connection to a database whose existence is checked by this method.</param>
    /// <param name="commandTimeout">Execution timeout for any commands needed to determine the existence of the database.</param>
    /// <param name="storeItemCollection">The collection of all store items from the model. This parameter is no longer used for determining database existence.</param>
    public bool DatabaseExists(
      DbConnection connection,
      int? commandTimeout,
      StoreItemCollection storeItemCollection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<StoreItemCollection>(storeItemCollection, nameof (storeItemCollection));
      using (new TransactionScope(TransactionScopeOption.Suppress))
        return this.DbDatabaseExists(connection, commandTimeout, storeItemCollection);
    }

    /// <summary>Returns a value indicating whether a given database exists on the server.</summary>
    /// <returns>True if the provider can deduce the database only based on the connection.</returns>
    /// <param name="connection">Connection to a database whose existence is checked by this method.</param>
    /// <param name="commandTimeout">Execution timeout for any commands needed to determine the existence of the database.</param>
    /// <param name="storeItemCollection">The collection of all store items from the model. This parameter is no longer used for determining database existence.</param>
    public bool DatabaseExists(
      DbConnection connection,
      int? commandTimeout,
      Lazy<StoreItemCollection> storeItemCollection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<Lazy<StoreItemCollection>>(storeItemCollection, nameof (storeItemCollection));
      using (new TransactionScope(TransactionScopeOption.Suppress))
        return this.DbDatabaseExists(connection, commandTimeout, storeItemCollection);
    }

    /// <summary>Returns a value indicating whether a given database exists on the server.</summary>
    /// <returns>True if the provider can deduce the database only based on the connection.</returns>
    /// <param name="connection">Connection to a database whose existence is checked by this method.</param>
    /// <param name="commandTimeout">Execution timeout for any commands needed to determine the existence of the database.</param>
    /// <param name="storeItemCollection">The collection of all store items from the model. This parameter is no longer used for determining database existence.</param>
    protected virtual bool DbDatabaseExists(
      DbConnection connection,
      int? commandTimeout,
      StoreItemCollection storeItemCollection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<StoreItemCollection>(storeItemCollection, nameof (storeItemCollection));
      throw new ProviderIncompatibleException(Strings.ProviderDoesNotSupportDatabaseExists);
    }

    /// <summary>Returns a value indicating whether a given database exists on the server.</summary>
    /// <returns>True if the provider can deduce the database only based on the connection.</returns>
    /// <param name="connection">Connection to a database whose existence is checked by this method.</param>
    /// <param name="commandTimeout">Execution timeout for any commands needed to determine the existence of the database.</param>
    /// <param name="storeItemCollection">The collection of all store items from the model. This parameter is no longer used for determining database existence.</param>
    /// <remarks>Override this method to avoid creating the store item collection if it is not needed. The default implementation evaluates the Lazy and calls the other overload of this method.</remarks>
    protected virtual bool DbDatabaseExists(
      DbConnection connection,
      int? commandTimeout,
      Lazy<StoreItemCollection> storeItemCollection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<Lazy<StoreItemCollection>>(storeItemCollection, nameof (storeItemCollection));
      return this.DbDatabaseExists(connection, commandTimeout, storeItemCollection.Value);
    }

    /// <summary>Deletes the specified database.</summary>
    /// <param name="connection">Connection to an existing database that needs to be deleted.</param>
    /// <param name="commandTimeout">Execution timeout for any commands needed to delete the database.</param>
    /// <param name="storeItemCollection">The collection of all store items from the model. This parameter is no longer used for database deletion.</param>
    public void DeleteDatabase(
      DbConnection connection,
      int? commandTimeout,
      StoreItemCollection storeItemCollection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<StoreItemCollection>(storeItemCollection, nameof (storeItemCollection));
      this.DbDeleteDatabase(connection, commandTimeout, storeItemCollection);
    }

    /// <summary>Deletes the specified database.</summary>
    /// <param name="connection">Connection to an existing database that needs to be deleted.</param>
    /// <param name="commandTimeout">Execution timeout for any commands needed to delete the database.</param>
    /// <param name="storeItemCollection">The collection of all store items from the model. This parameter is no longer used for database deletion.</param>
    protected virtual void DbDeleteDatabase(
      DbConnection connection,
      int? commandTimeout,
      StoreItemCollection storeItemCollection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<StoreItemCollection>(storeItemCollection, nameof (storeItemCollection));
      throw new ProviderIncompatibleException(Strings.ProviderDoesNotSupportDeleteDatabase);
    }

    /// <summary>
    /// Expands |DataDirectory| in the given path if it begins with |DataDirectory| and returns the expanded path,
    /// or returns the given string if it does not start with |DataDirectory|.
    /// </summary>
    /// <param name="path"> The path to expand. </param>
    /// <returns> The expanded path. </returns>
    public static string ExpandDataDirectory(string path)
    {
      if (string.IsNullOrEmpty(path) || !path.StartsWith("|datadirectory|", StringComparison.OrdinalIgnoreCase))
        return path;
      object data = AppDomain.CurrentDomain.GetData("DataDirectory");
      string str = data as string;
      if (data != null && str == null)
        throw new InvalidOperationException(Strings.ADP_InvalidDataDirectory);
      if (str == string.Empty)
        str = AppDomain.CurrentDomain.BaseDirectory;
      if (str == null)
        str = string.Empty;
      path = path.Substring("|datadirectory|".Length);
      if (path.StartsWith("\\", StringComparison.Ordinal))
        path = path.Substring(1);
      path = (str.EndsWith("\\", StringComparison.Ordinal) ? str : str + "\\") + path;
      if (str.Contains(".."))
        throw new ArgumentException(Strings.ExpandingDataDirectoryFailed);
      return path;
    }

    /// <summary>
    /// Adds an <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver" /> that will be used to resolve additional default provider
    /// services when a derived type is registered as an EF provider either using an entry in the application's
    /// config file or through code-based registration in <see cref="T:System.Data.Entity.DbConfiguration" />.
    /// </summary>
    /// <param name="resolver">The resolver to add.</param>
    protected void AddDependencyResolver(IDbDependencyResolver resolver)
    {
      System.Data.Entity.Utilities.Check.NotNull<IDbDependencyResolver>(resolver, nameof (resolver));
      this._resolvers.Add(resolver);
    }

    /// <summary>
    /// Called to resolve additional default provider services when a derived type is registered as an
    /// EF provider either using an entry in the application's config file or through code-based
    /// registration in <see cref="T:System.Data.Entity.DbConfiguration" />. The implementation of this method in this
    /// class uses the resolvers added with the AddDependencyResolver method to resolve
    /// dependencies.
    /// </summary>
    /// <remarks>
    /// Use this method to set, add, or change other provider-related services. Note that this method
    /// will only be called for such services if they are not already explicitly configured in some
    /// other way by the application. This allows providers to set default services while the
    /// application is still able to override and explicitly configure each service if required.
    /// See <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver" /> and <see cref="T:System.Data.Entity.DbConfiguration" /> for more details.
    /// </remarks>
    /// <param name="type">The type of the service to be resolved.</param>
    /// <param name="key">An optional key providing additional information for resolving the service.</param>
    /// <returns>An instance of the given type, or null if the service could not be resolved.</returns>
    public virtual object GetService(Type type, object key) => this._resolvers.GetService(type, key);

    /// <summary>
    /// Called to resolve additional default provider services when a derived type is registered as an
    /// EF provider either using an entry in the application's config file or through code-based
    /// registration in <see cref="T:System.Data.Entity.DbConfiguration" />. The implementation of this method in this
    /// class uses the resolvers added with the AddDependencyResolver method to resolve
    /// dependencies.
    /// </summary>
    /// <param name="type">The type of the service to be resolved.</param>
    /// <param name="key">An optional key providing additional information for resolving the service.</param>
    /// <returns>All registered services that satisfy the given type and key, or an empty enumeration if there are none.</returns>
    public virtual IEnumerable<object> GetServices(Type type, object key) => this._resolvers.GetServices(type, key);
  }
}
