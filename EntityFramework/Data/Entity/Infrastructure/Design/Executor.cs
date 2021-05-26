// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.Executor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Design;
using System.Data.Entity.Migrations.History;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Utilities;
using System.Data.Entity.Utilities;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Data.Entity.Infrastructure.Design
{
  /// <summary>
  /// Used for design-time scenarios where the user's code needs to be executed inside
  /// of an isolated, runtime-like <see cref="T:System.AppDomain" />.
  /// 
  /// Instances of this class should be created inside of the guest domain.
  /// Handlers should be created inside of the host domain. To invoke operations,
  /// create instances of the nested classes inside
  /// </summary>
  public class Executor : MarshalByRefObject
  {
    private readonly Assembly _assembly;
    private readonly Reporter _reporter;
    private readonly string _language;
    private readonly string _rootNamespace;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Infrastructure.Design.Executor" /> class. Do this inside of the guest
    /// domain.
    /// </summary>
    /// <param name="assemblyFile">The path for the assembly containing the user's code.</param>
    /// <param name="anonymousArguments">The parameter is not used.</param>
    public Executor(string assemblyFile, IDictionary<string, object> anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(assemblyFile, nameof (assemblyFile));
      this._reporter = new Reporter((IReportHandler) new WrappedReportHandler(anonymousArguments?["reportHandler"]));
      this._language = (string) anonymousArguments?["language"];
      this._rootNamespace = (string) anonymousArguments?["rootNamespace"];
      this._assembly = Assembly.Load(AssemblyName.GetAssemblyName(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyFile)));
    }

    private Assembly LoadAssembly(string assemblyName)
    {
      if (string.IsNullOrEmpty(assemblyName))
        return (Assembly) null;
      try
      {
        return Assembly.Load(assemblyName);
      }
      catch (FileNotFoundException ex)
      {
        throw new MigrationsException(System.Data.Entity.Resources.Strings.ToolingFacade_AssemblyNotFound((object) ex.FileName), (Exception) ex);
      }
    }

    private string GetContextTypeInternal(string contextTypeName, string contextAssemblyName)
    {
      Assembly assembly1 = this.LoadAssembly(contextAssemblyName);
      if ((object) assembly1 == null)
        assembly1 = this._assembly;
      return new TypeFinder(assembly1).FindType(typeof (DbContext), contextTypeName, (Func<IEnumerable<Type>, IEnumerable<Type>>) (types => types.Where<Type>((Func<Type, bool>) (t => !typeof (HistoryContext).IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericType))), new Func<string, Exception>(System.Data.Entity.Resources.Error.EnableMigrations_NoContext), (Func<string, IEnumerable<Type>, Exception>) ((assembly, types) =>
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(System.Data.Entity.Resources.Strings.EnableMigrations_MultipleContexts((object) assembly));
        foreach (Type type in types)
        {
          stringBuilder.AppendLine();
          stringBuilder.Append(System.Data.Entity.Resources.Strings.EnableMigrationsForContext((object) type.FullName));
        }
        return (Exception) new MigrationsException(stringBuilder.ToString());
      }), new Func<string, string, Exception>(System.Data.Entity.Resources.Error.EnableMigrations_NoContextWithName), new Func<string, string, Exception>(System.Data.Entity.Resources.Error.EnableMigrations_MultipleContextsWithName)).FullName;
    }

    internal virtual string GetProviderServicesInternal(string invariantName)
    {
      DbConfiguration.LoadConfiguration(this._assembly);
      IDbDependencyResolver dependencyResolver = DbConfiguration.DependencyResolver;
      DbProviderServices providerServices = (DbProviderServices) null;
      try
      {
        providerServices = dependencyResolver.GetService<DbProviderServices>((object) invariantName);
      }
      catch
      {
      }
      return providerServices?.GetType().AssemblyQualifiedName;
    }

    private void OverrideConfiguration(
      DbMigrationsConfiguration configuration,
      DbConnectionInfo connectionInfo,
      bool force = false)
    {
      if (connectionInfo != null)
        configuration.TargetDatabase = connectionInfo;
      if (string.Equals(this._language, "VB", StringComparison.OrdinalIgnoreCase) && configuration.CodeGenerator is CSharpMigrationCodeGenerator)
        configuration.CodeGenerator = (MigrationCodeGenerator) new VisualBasicMigrationCodeGenerator();
      if (!force)
        return;
      configuration.AutomaticMigrationDataLossAllowed = true;
    }

    private MigrationScaffolder CreateMigrationScaffolder(
      DbMigrationsConfiguration configuration)
    {
      MigrationScaffolder migrationScaffolder = new MigrationScaffolder(configuration);
      string s2 = configuration.MigrationsNamespace;
      if (string.Equals(this._language, "VB", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(this._rootNamespace))
      {
        if (this._rootNamespace.EqualsIgnoreCase(s2))
        {
          s2 = (string) null;
        }
        else
        {
          if (s2 == null || !s2.StartsWith(this._rootNamespace + ".", StringComparison.OrdinalIgnoreCase))
            throw System.Data.Entity.Resources.Error.MigrationsNamespaceNotUnderRootNamespace((object) s2, (object) this._rootNamespace);
          s2 = s2.Substring(this._rootNamespace.Length + 1);
        }
      }
      migrationScaffolder.Namespace = s2;
      return migrationScaffolder;
    }

    private static IDictionary ToHashtable(ScaffoldedMigration result)
    {
      if (result == null)
        return (IDictionary) null;
      return (IDictionary) new Hashtable()
      {
        [(object) "MigrationId"] = (object) result.MigrationId,
        [(object) "UserCode"] = (object) result.UserCode,
        [(object) "DesignerCode"] = (object) result.DesignerCode,
        [(object) "Language"] = (object) result.Language,
        [(object) "Directory"] = (object) result.Directory,
        [(object) "Resources"] = (object) result.Resources,
        [(object) "IsRescaffold"] = (object) result.IsRescaffold
      };
    }

    internal virtual IDictionary ScaffoldInitialCreateInternal(
      DbConnectionInfo connectionInfo,
      string contextTypeName,
      string contextAssemblyName,
      string migrationsNamespace,
      bool auto,
      string migrationsDir)
    {
      Assembly assembly1 = this.LoadAssembly(contextAssemblyName);
      if ((object) assembly1 == null)
        assembly1 = this._assembly;
      Assembly assembly2 = assembly1;
      DbMigrationsConfiguration configuration = new DbMigrationsConfiguration()
      {
        ContextType = assembly2.GetType(contextTypeName, true),
        MigrationsAssembly = this._assembly,
        MigrationsNamespace = migrationsNamespace,
        AutomaticMigrationsEnabled = auto,
        MigrationsDirectory = migrationsDir
      };
      this.OverrideConfiguration(configuration, connectionInfo);
      return Executor.ToHashtable(this.CreateMigrationScaffolder(configuration).ScaffoldInitialCreate());
    }

    private DbMigrationsConfiguration GetMigrationsConfiguration(
      string migrationsConfigurationName)
    {
      return new MigrationsConfigurationFinder(new TypeFinder(this._assembly)).FindMigrationsConfiguration((Type) null, migrationsConfigurationName, new Func<string, Exception>(System.Data.Entity.Resources.Error.AssemblyMigrator_NoConfiguration), (Func<string, IEnumerable<Type>, Exception>) ((assembly, types) => System.Data.Entity.Resources.Error.AssemblyMigrator_MultipleConfigurations((object) assembly)), new Func<string, string, Exception>(System.Data.Entity.Resources.Error.AssemblyMigrator_NoConfigurationWithName), new Func<string, string, Exception>(System.Data.Entity.Resources.Error.AssemblyMigrator_MultipleConfigurationsWithName));
    }

    internal virtual IDictionary ScaffoldInternal(
      string name,
      DbConnectionInfo connectionInfo,
      string migrationsConfigurationName,
      bool ignoreChanges)
    {
      DbMigrationsConfiguration migrationsConfiguration = this.GetMigrationsConfiguration(migrationsConfigurationName);
      this.OverrideConfiguration(migrationsConfiguration, connectionInfo);
      return Executor.ToHashtable(this.CreateMigrationScaffolder(migrationsConfiguration).Scaffold(name, ignoreChanges));
    }

    internal IEnumerable<string> GetDatabaseMigrationsInternal(
      DbConnectionInfo connectionInfo,
      string migrationsConfigurationName)
    {
      DbMigrationsConfiguration migrationsConfiguration = this.GetMigrationsConfiguration(migrationsConfigurationName);
      this.OverrideConfiguration(migrationsConfiguration, connectionInfo);
      return this.CreateMigrator(migrationsConfiguration).GetDatabaseMigrations();
    }

    internal string ScriptUpdateInternal(
      string sourceMigration,
      string targetMigration,
      bool force,
      DbConnectionInfo connectionInfo,
      string migrationsConfigurationName)
    {
      DbMigrationsConfiguration migrationsConfiguration = this.GetMigrationsConfiguration(migrationsConfigurationName);
      this.OverrideConfiguration(migrationsConfiguration, connectionInfo, force);
      return new MigratorScriptingDecorator(this.CreateMigrator(migrationsConfiguration)).ScriptUpdate(sourceMigration, targetMigration);
    }

    internal void UpdateInternal(
      string targetMigration,
      bool force,
      DbConnectionInfo connectionInfo,
      string migrationsConfigurationName)
    {
      DbMigrationsConfiguration migrationsConfiguration = this.GetMigrationsConfiguration(migrationsConfigurationName);
      this.OverrideConfiguration(migrationsConfiguration, connectionInfo, force);
      this.CreateMigrator(migrationsConfiguration).Update(targetMigration);
    }

    private MigratorBase CreateMigrator(DbMigrationsConfiguration configuration) => (MigratorBase) new MigratorLoggingDecorator((MigratorBase) new DbMigrator(configuration), (MigrationsLogger) new Executor.ToolLogger(this._reporter));

    public class GetContextType : Executor.OperationBase
    {
      public GetContextType(Executor executor, object resultHandler, IDictionary args)
        : base(resultHandler)
      {
        System.Data.Entity.Utilities.Check.NotNull<Executor>(executor, nameof (executor));
        System.Data.Entity.Utilities.Check.NotNull<object>(resultHandler, nameof (resultHandler));
        System.Data.Entity.Utilities.Check.NotNull<IDictionary>(args, nameof (args));
        string contextTypeName = (string) args[(object) "contextTypeName"];
        string contextAssemblyName = (string) args[(object) "contextAssemblyName"];
        this.Execute<string>((Func<string>) (() => executor.GetContextTypeInternal(contextTypeName, contextAssemblyName)));
      }
    }

    /// <summary>
    /// Used to get the assembly-qualified name of the DbProviderServices type for the
    /// specified provider invariant name.
    /// </summary>
    internal class GetProviderServices : Executor.OperationBase
    {
      public GetProviderServices(
        Executor executor,
        object handler,
        string invariantName,
        IDictionary<string, object> anonymousArguments)
        : base(handler)
      {
        System.Data.Entity.Utilities.Check.NotNull<Executor>(executor, nameof (executor));
        System.Data.Entity.Utilities.Check.NotEmpty(invariantName, nameof (invariantName));
        this.Execute<string>((Func<string>) (() => executor.GetProviderServicesInternal(invariantName)));
      }
    }

    public class ScaffoldInitialCreate : Executor.OperationBase
    {
      public ScaffoldInitialCreate(Executor executor, object resultHandler, IDictionary args)
        : base(resultHandler)
      {
        System.Data.Entity.Utilities.Check.NotNull<Executor>(executor, nameof (executor));
        System.Data.Entity.Utilities.Check.NotNull<object>(resultHandler, nameof (resultHandler));
        System.Data.Entity.Utilities.Check.NotNull<IDictionary>(args, nameof (args));
        string connectionStringName = (string) args[(object) "connectionStringName"];
        string connectionString = (string) args[(object) "connectionString"];
        string connectionProviderName = (string) args[(object) "connectionProviderName"];
        string contextTypeName = (string) args[(object) "contextTypeName"];
        string contextAssemblyName = (string) args[(object) "contextAssemblyName"];
        string migrationsNamespace = (string) args[(object) "migrationsNamespace"];
        bool auto = (bool) args[(object) "auto"];
        string migrationsDir = (string) args[(object) "migrationsDir"];
        this.Execute<IDictionary>((Func<IDictionary>) (() => executor.ScaffoldInitialCreateInternal(Executor.OperationBase.CreateConnectionInfo(connectionStringName, connectionString, connectionProviderName), contextTypeName, contextAssemblyName, migrationsNamespace, auto, migrationsDir)));
      }
    }

    public class Scaffold : Executor.OperationBase
    {
      public Scaffold(Executor executor, object resultHandler, IDictionary args)
        : base(resultHandler)
      {
        System.Data.Entity.Utilities.Check.NotNull<Executor>(executor, nameof (executor));
        System.Data.Entity.Utilities.Check.NotNull<object>(resultHandler, nameof (resultHandler));
        System.Data.Entity.Utilities.Check.NotNull<IDictionary>(args, nameof (args));
        string name = (string) args[(object) "name"];
        string connectionStringName = (string) args[(object) "connectionStringName"];
        string connectionString = (string) args[(object) "connectionString"];
        string connectionProviderName = (string) args[(object) "connectionProviderName"];
        string migrationsConfigurationName = (string) args[(object) "migrationsConfigurationName"];
        bool ignoreChanges = (bool) args[(object) "ignoreChanges"];
        this.Execute<IDictionary>((Func<IDictionary>) (() => executor.ScaffoldInternal(name, Executor.OperationBase.CreateConnectionInfo(connectionStringName, connectionString, connectionProviderName), migrationsConfigurationName, ignoreChanges)));
      }
    }

    public class GetDatabaseMigrations : Executor.OperationBase
    {
      public GetDatabaseMigrations(Executor executor, object resultHandler, IDictionary args)
        : base(resultHandler)
      {
        System.Data.Entity.Utilities.Check.NotNull<Executor>(executor, nameof (executor));
        System.Data.Entity.Utilities.Check.NotNull<object>(resultHandler, nameof (resultHandler));
        System.Data.Entity.Utilities.Check.NotNull<IDictionary>(args, nameof (args));
        string connectionStringName = (string) args[(object) "connectionStringName"];
        string connectionString = (string) args[(object) "connectionString"];
        string connectionProviderName = (string) args[(object) "connectionProviderName"];
        string migrationsConfigurationName = (string) args[(object) "migrationsConfigurationName"];
        this.Execute<string>((Func<IEnumerable<string>>) (() => executor.GetDatabaseMigrationsInternal(Executor.OperationBase.CreateConnectionInfo(connectionStringName, connectionString, connectionProviderName), migrationsConfigurationName)));
      }
    }

    public class ScriptUpdate : Executor.OperationBase
    {
      public ScriptUpdate(Executor executor, object resultHandler, IDictionary args)
        : base(resultHandler)
      {
        System.Data.Entity.Utilities.Check.NotNull<Executor>(executor, nameof (executor));
        System.Data.Entity.Utilities.Check.NotNull<object>(resultHandler, nameof (resultHandler));
        System.Data.Entity.Utilities.Check.NotNull<IDictionary>(args, nameof (args));
        string sourceMigration = (string) args[(object) "sourceMigration"];
        string targetMigration = (string) args[(object) "targetMigration"];
        bool force = (bool) args[(object) "force"];
        string connectionStringName = (string) args[(object) "connectionStringName"];
        string connectionString = (string) args[(object) "connectionString"];
        string connectionProviderName = (string) args[(object) "connectionProviderName"];
        string migrationsConfigurationName = (string) args[(object) "migrationsConfigurationName"];
        this.Execute<string>((Func<string>) (() => executor.ScriptUpdateInternal(sourceMigration, targetMigration, force, Executor.OperationBase.CreateConnectionInfo(connectionStringName, connectionString, connectionProviderName), migrationsConfigurationName)));
      }
    }

    public class Update : Executor.OperationBase
    {
      public Update(Executor executor, object resultHandler, IDictionary args)
        : base(resultHandler)
      {
        System.Data.Entity.Utilities.Check.NotNull<Executor>(executor, nameof (executor));
        System.Data.Entity.Utilities.Check.NotNull<object>(resultHandler, nameof (resultHandler));
        System.Data.Entity.Utilities.Check.NotNull<IDictionary>(args, nameof (args));
        string targetMigration = (string) args[(object) "targetMigration"];
        bool force = (bool) args[(object) "force"];
        string connectionStringName = (string) args[(object) "connectionStringName"];
        string connectionString = (string) args[(object) "connectionString"];
        string connectionProviderName = (string) args[(object) "connectionProviderName"];
        string migrationsConfigurationName = (string) args[(object) "migrationsConfigurationName"];
        this.Execute((Action) (() => executor.UpdateInternal(targetMigration, force, Executor.OperationBase.CreateConnectionInfo(connectionStringName, connectionString, connectionProviderName), migrationsConfigurationName)));
      }
    }

    /// <summary>Represents an operation.</summary>
    public abstract class OperationBase : MarshalByRefObject
    {
      private readonly WrappedResultHandler _handler;

      /// <summary>
      /// Initializes a new instance of the <see cref="T:System.Data.Entity.Infrastructure.Design.Executor.OperationBase" /> class.
      /// </summary>
      /// <param name="handler">An object to handle callbacks during the operation.</param>
      protected OperationBase(object handler)
      {
        System.Data.Entity.Utilities.Check.NotNull<object>(handler, nameof (handler));
        this._handler = new WrappedResultHandler(handler);
      }

      protected static DbConnectionInfo CreateConnectionInfo(
        string connectionStringName,
        string connectionString,
        string connectionProviderName)
      {
        if (!string.IsNullOrWhiteSpace(connectionStringName))
          return new DbConnectionInfo(connectionStringName);
        return !string.IsNullOrWhiteSpace(connectionString) ? new DbConnectionInfo(connectionString, connectionProviderName) : (DbConnectionInfo) null;
      }

      /// <summary>
      ///     Executes an action passing exceptions to the handler.
      /// </summary>
      /// <param name="action"> The action to execute. </param>
      protected virtual void Execute(Action action)
      {
        System.Data.Entity.Utilities.Check.NotNull<Action>(action, nameof (action));
        try
        {
          action();
        }
        catch (Exception ex)
        {
          if (this._handler.SetError(ex.GetType().FullName, ex.Message, ex.ToString()))
            return;
          throw;
        }
      }

      /// <summary>
      ///     Executes an action passing the result or exceptions to the handler.
      /// </summary>
      /// <typeparam name="T"> The result type. </typeparam>
      /// <param name="action"> The action to execute. </param>
      protected virtual void Execute<T>(Func<T> action)
      {
        System.Data.Entity.Utilities.Check.NotNull<Func<T>>(action, nameof (action));
        this.Execute((Action) (() => this._handler.SetResult((object) action())));
      }

      /// <summary>
      ///     Executes an action passing results or exceptions to the handler.
      /// </summary>
      /// <typeparam name="T"> The type of results. </typeparam>
      /// <param name="action"> The action to execute. </param>
      protected virtual void Execute<T>(Func<IEnumerable<T>> action)
      {
        System.Data.Entity.Utilities.Check.NotNull<Func<IEnumerable<T>>>(action, nameof (action));
        this.Execute((Action) (() => this._handler.SetResult((object) action().ToArray<T>())));
      }
    }

    private class ToolLogger : MigrationsLogger
    {
      private readonly Reporter _reporter;

      public ToolLogger(Reporter reporter) => this._reporter = reporter;

      public override void Info(string message) => this._reporter.WriteInformation(message);

      public override void Warning(string message) => this._reporter.WriteWarning(message);

      public override void Verbose(string sql) => this._reporter.WriteVerbose(sql);
    }
  }
}
