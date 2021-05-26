// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Design.ToolingFacade
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations.History;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Utilities;
using System.Data.Entity.Utilities;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;

namespace System.Data.Entity.Migrations.Design
{
  /// <summary>
  /// Helper class that is used by design time tools to run migrations related
  /// commands that need to interact with an application that is being edited
  /// in Visual Studio.
  /// Because the application is being edited the assemblies need to
  /// be loaded in a separate AppDomain to ensure the latest version
  /// is always loaded.
  /// The App/Web.config file from the startup project is also copied
  /// to ensure that any configuration is applied.
  /// </summary>
  [Obsolete("Use System.Data.Entity.Infrastructure.Design.Executor instead.")]
  public class ToolingFacade : IDisposable
  {
    private readonly string _migrationsAssemblyName;
    private readonly string _contextAssemblyName;
    private readonly string _configurationTypeName;
    private readonly string _configurationFile;
    private readonly DbConnectionInfo _connectionStringInfo;
    private AppDomain _appDomain;

    /// <summary>Gets or sets an action to be run to log information.</summary>
    public Action<string> LogInfoDelegate { get; set; }

    /// <summary>Gets or sets an action to be run to log warnings.</summary>
    public Action<string> LogWarningDelegate { get; set; }

    /// <summary>
    /// Gets or sets an action to be run to log verbose information.
    /// </summary>
    public Action<string> LogVerboseDelegate { get; set; }

    /// <summary>
    /// Initializes a new instance of the ToolingFacade class.
    /// </summary>
    /// <param name="migrationsAssemblyName"> The name of the assembly that contains the migrations configuration to be used. </param>
    /// <param name="contextAssemblyName"> The name of the assembly that contains the DbContext to be used. </param>
    /// <param name="configurationTypeName"> The namespace qualified name of migrations configuration to be used. </param>
    /// <param name="workingDirectory"> The working directory containing the compiled assemblies. </param>
    /// <param name="configurationFilePath"> The path of the config file from the startup project. </param>
    /// <param name="dataDirectory"> The path of the application data directory from the startup project. Typically the App_Data directory for web applications or the working directory for executables. </param>
    /// <param name="connectionStringInfo"> The connection to the database to be migrated. If null is supplied, the default connection for the context will be used. </param>
    public ToolingFacade(
      string migrationsAssemblyName,
      string contextAssemblyName,
      string configurationTypeName,
      string workingDirectory,
      string configurationFilePath,
      string dataDirectory,
      DbConnectionInfo connectionStringInfo)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(migrationsAssemblyName, nameof (migrationsAssemblyName));
      this._migrationsAssemblyName = migrationsAssemblyName;
      this._contextAssemblyName = contextAssemblyName;
      this._configurationTypeName = configurationTypeName;
      this._connectionStringInfo = connectionStringInfo;
      AppDomainSetup info = new AppDomainSetup()
      {
        ShadowCopyFiles = "true"
      };
      if (!string.IsNullOrWhiteSpace(workingDirectory))
        info.ApplicationBase = workingDirectory;
      this._configurationFile = new ConfigurationFileUpdater().Update(configurationFilePath);
      info.ConfigurationFile = this._configurationFile;
      this._appDomain = AppDomain.CreateDomain("MigrationsToolingFacade" + Convert.ToBase64String(Guid.NewGuid().ToByteArray()), (Evidence) null, info);
      if (string.IsNullOrWhiteSpace(dataDirectory))
        return;
      this._appDomain.SetData("DataDirectory", (object) dataDirectory);
    }

    internal ToolingFacade()
    {
    }

    /// <summary>Releases all unmanaged resources used by the facade.</summary>
    ~ToolingFacade() => this.Dispose(false);

    /// <summary>
    /// Gets the fully qualified name of all types deriving from <see cref="T:System.Data.Entity.DbContext" />.
    /// </summary>
    /// <returns> All context types found. </returns>
    public IEnumerable<string> GetContextTypes()
    {
      ToolingFacade.GetContextTypesRunner contextTypesRunner = new ToolingFacade.GetContextTypesRunner();
      this.ConfigureRunner((ToolingFacade.BaseRunner) contextTypesRunner);
      this.Run((ToolingFacade.BaseRunner) contextTypesRunner);
      return (IEnumerable<string>) this._appDomain.GetData("result");
    }

    /// <summary>
    /// Gets the fully qualified name of a type deriving from <see cref="T:System.Data.Entity.DbContext" />.
    /// </summary>
    /// <param name="contextTypeName"> The name of the context type. If null, the single context type found in the assembly will be returned. </param>
    /// <returns> The context type found. </returns>
    public string GetContextType(string contextTypeName)
    {
      ToolingFacade.GetContextTypeRunner contextTypeRunner = new ToolingFacade.GetContextTypeRunner()
      {
        ContextTypeName = contextTypeName
      };
      this.ConfigureRunner((ToolingFacade.BaseRunner) contextTypeRunner);
      this.Run((ToolingFacade.BaseRunner) contextTypeRunner);
      return (string) this._appDomain.GetData("result");
    }

    /// <summary>
    /// Gets a list of all migrations that have been applied to the database.
    /// </summary>
    /// <returns> Ids of applied migrations. </returns>
    public virtual IEnumerable<string> GetDatabaseMigrations()
    {
      ToolingFacade.GetDatabaseMigrationsRunner migrationsRunner = new ToolingFacade.GetDatabaseMigrationsRunner();
      this.ConfigureRunner((ToolingFacade.BaseRunner) migrationsRunner);
      this.Run((ToolingFacade.BaseRunner) migrationsRunner);
      return (IEnumerable<string>) this._appDomain.GetData("result");
    }

    /// <summary>
    /// Gets a list of all migrations that have not been applied to the database.
    /// </summary>
    /// <returns> Ids of pending migrations. </returns>
    public virtual IEnumerable<string> GetPendingMigrations()
    {
      ToolingFacade.GetPendingMigrationsRunner migrationsRunner = new ToolingFacade.GetPendingMigrationsRunner();
      this.ConfigureRunner((ToolingFacade.BaseRunner) migrationsRunner);
      this.Run((ToolingFacade.BaseRunner) migrationsRunner);
      return (IEnumerable<string>) this._appDomain.GetData("result");
    }

    /// <summary>Updates the database to the specified migration.</summary>
    /// <param name="targetMigration"> The Id of the migration to migrate to. If null is supplied, the database will be updated to the latest migration. </param>
    /// <param name="force"> Value indicating if data loss during automatic migration is acceptable. </param>
    public void Update(string targetMigration, bool force)
    {
      ToolingFacade.UpdateRunner updateRunner = new ToolingFacade.UpdateRunner()
      {
        TargetMigration = targetMigration,
        Force = force
      };
      this.ConfigureRunner((ToolingFacade.BaseRunner) updateRunner);
      this.Run((ToolingFacade.BaseRunner) updateRunner);
    }

    /// <summary>
    /// Generates a SQL script to migrate between two migrations.
    /// </summary>
    /// <param name="sourceMigration"> The migration to update from. If null is supplied, a script to update the current database will be produced. </param>
    /// <param name="targetMigration"> The migration to update to. If null is supplied, a script to update to the latest migration will be produced. </param>
    /// <param name="force"> Value indicating if data loss during automatic migration is acceptable. </param>
    /// <returns> The generated SQL script. </returns>
    public string ScriptUpdate(string sourceMigration, string targetMigration, bool force)
    {
      ToolingFacade.ScriptUpdateRunner scriptUpdateRunner = new ToolingFacade.ScriptUpdateRunner()
      {
        SourceMigration = sourceMigration,
        TargetMigration = targetMigration,
        Force = force
      };
      this.ConfigureRunner((ToolingFacade.BaseRunner) scriptUpdateRunner);
      this.Run((ToolingFacade.BaseRunner) scriptUpdateRunner);
      return (string) this._appDomain.GetData("result");
    }

    /// <summary>
    /// Scaffolds a code-based migration to apply any pending model changes.
    /// </summary>
    /// <param name="migrationName"> The name for the generated migration. </param>
    /// <param name="language"> The programming language of the generated migration. </param>
    /// <param name="rootNamespace"> The root namespace of the project the migration will be added to. </param>
    /// <param name="ignoreChanges"> Whether or not to include model changes. </param>
    /// <returns> The scaffolded migration. </returns>
    public virtual ScaffoldedMigration Scaffold(
      string migrationName,
      string language,
      string rootNamespace,
      bool ignoreChanges)
    {
      ToolingFacade.ScaffoldRunner scaffoldRunner = new ToolingFacade.ScaffoldRunner()
      {
        MigrationName = migrationName,
        Language = language,
        RootNamespace = rootNamespace,
        IgnoreChanges = ignoreChanges
      };
      this.ConfigureRunner((ToolingFacade.BaseRunner) scaffoldRunner);
      this.Run((ToolingFacade.BaseRunner) scaffoldRunner);
      return (ScaffoldedMigration) this._appDomain.GetData("result");
    }

    /// <summary>
    /// Scaffolds the initial code-based migration corresponding to a previously run database initializer.
    /// </summary>
    /// <param name="language"> The programming language of the generated migration. </param>
    /// <param name="rootNamespace"> The root namespace of the project the migration will be added to. </param>
    /// <returns> The scaffolded migration. </returns>
    public ScaffoldedMigration ScaffoldInitialCreate(
      string language,
      string rootNamespace)
    {
      ToolingFacade.InitialCreateScaffoldRunner createScaffoldRunner1 = new ToolingFacade.InitialCreateScaffoldRunner();
      createScaffoldRunner1.Language = language;
      createScaffoldRunner1.RootNamespace = rootNamespace;
      ToolingFacade.InitialCreateScaffoldRunner createScaffoldRunner2 = createScaffoldRunner1;
      this.ConfigureRunner((ToolingFacade.BaseRunner) createScaffoldRunner2);
      this.Run((ToolingFacade.BaseRunner) createScaffoldRunner2);
      return (ScaffoldedMigration) this._appDomain.GetData("result");
    }

    /// <inheritdoc />
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases all resources used by the facade.</summary>
    /// <param name="disposing">
    /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing && this._appDomain != null)
      {
        AppDomain.Unload(this._appDomain);
        this._appDomain = (AppDomain) null;
      }
      if (this._configurationFile == null)
        return;
      File.Delete(this._configurationFile);
    }

    private void ConfigureRunner(ToolingFacade.BaseRunner runner)
    {
      runner.MigrationsAssemblyName = this._migrationsAssemblyName;
      runner.ContextAssemblyName = this._contextAssemblyName;
      runner.ConfigurationTypeName = this._configurationTypeName;
      runner.ConnectionStringInfo = this._connectionStringInfo;
      runner.Log = new ToolingFacade.ToolLogger(this);
    }

    private void Run(ToolingFacade.BaseRunner runner)
    {
      this._appDomain.SetData("error", (object) null);
      this._appDomain.SetData("typeName", (object) null);
      this._appDomain.SetData("stackTrace", (object) null);
      this._appDomain.DoCallBack(new CrossAppDomainDelegate(runner.Run));
      string data1 = (string) this._appDomain.GetData("error");
      if (data1 != null)
      {
        string data2 = (string) this._appDomain.GetData("typeName");
        string data3 = (string) this._appDomain.GetData("stackTrace");
        throw new ToolingException(data1, data2, data3);
      }
    }

    private class ToolLogger : MigrationsLogger
    {
      private readonly ToolingFacade _facade;

      public ToolLogger(ToolingFacade facade) => this._facade = facade;

      public override void Info(string message)
      {
        if (this._facade.LogInfoDelegate == null)
          return;
        this._facade.LogInfoDelegate(message);
      }

      public override void Warning(string message)
      {
        if (this._facade.LogWarningDelegate == null)
          return;
        this._facade.LogWarningDelegate(message);
      }

      public override void Verbose(string sql)
      {
        if (this._facade.LogVerboseDelegate == null)
          return;
        this._facade.LogVerboseDelegate(sql);
      }
    }

    [Serializable]
    private abstract class BaseRunner
    {
      public string MigrationsAssemblyName { get; set; }

      public string ContextAssemblyName { get; set; }

      public string ConfigurationTypeName { get; set; }

      public DbConnectionInfo ConnectionStringInfo { get; set; }

      public ToolingFacade.ToolLogger Log { get; set; }

      public void Run()
      {
        try
        {
          this.RunCore();
        }
        catch (Exception ex)
        {
          AppDomain.CurrentDomain.SetData("error", (object) ex.Message);
          AppDomain.CurrentDomain.SetData("typeName", (object) ex.GetType().FullName);
          AppDomain.CurrentDomain.SetData("stackTrace", (object) ex.ToString());
        }
      }

      protected abstract void RunCore();

      protected MigratorBase GetMigrator() => this.DecorateMigrator(new DbMigrator(this.GetConfiguration()));

      protected DbMigrationsConfiguration GetConfiguration()
      {
        DbMigrationsConfiguration configuration = this.FindConfiguration();
        this.OverrideConfiguration(configuration);
        return configuration;
      }

      protected virtual void OverrideConfiguration(DbMigrationsConfiguration configuration)
      {
        if (this.ConnectionStringInfo == null)
          return;
        configuration.TargetDatabase = this.ConnectionStringInfo;
      }

      private MigratorBase DecorateMigrator(DbMigrator migrator) => (MigratorBase) new MigratorLoggingDecorator((MigratorBase) migrator, (MigrationsLogger) this.Log);

      private DbMigrationsConfiguration FindConfiguration() => new MigrationsConfigurationFinder(new TypeFinder(this.LoadMigrationsAssembly())).FindMigrationsConfiguration((Type) null, this.ConfigurationTypeName, new Func<string, Exception>(System.Data.Entity.Resources.Error.AssemblyMigrator_NoConfiguration), (Func<string, IEnumerable<Type>, Exception>) ((assembly, types) => System.Data.Entity.Resources.Error.AssemblyMigrator_MultipleConfigurations((object) assembly)), new Func<string, string, Exception>(System.Data.Entity.Resources.Error.AssemblyMigrator_NoConfigurationWithName), new Func<string, string, Exception>(System.Data.Entity.Resources.Error.AssemblyMigrator_MultipleConfigurationsWithName));

      protected Assembly LoadMigrationsAssembly() => ToolingFacade.BaseRunner.LoadAssembly(this.MigrationsAssemblyName);

      protected Assembly LoadContextAssembly() => ToolingFacade.BaseRunner.LoadAssembly(this.ContextAssemblyName);

      private static Assembly LoadAssembly(string name)
      {
        try
        {
          return Assembly.Load(name);
        }
        catch (FileNotFoundException ex)
        {
          throw new MigrationsException(System.Data.Entity.Resources.Strings.ToolingFacade_AssemblyNotFound((object) ex.FileName), (Exception) ex);
        }
      }
    }

    [Serializable]
    private class GetDatabaseMigrationsRunner : ToolingFacade.BaseRunner
    {
      protected override void RunCore() => AppDomain.CurrentDomain.SetData("result", (object) this.GetMigrator().GetDatabaseMigrations());
    }

    [Serializable]
    private class GetPendingMigrationsRunner : ToolingFacade.BaseRunner
    {
      protected override void RunCore() => AppDomain.CurrentDomain.SetData("result", (object) this.GetMigrator().GetPendingMigrations());
    }

    [Serializable]
    private class UpdateRunner : ToolingFacade.BaseRunner
    {
      public string TargetMigration { get; set; }

      public bool Force { get; set; }

      protected override void RunCore() => this.GetMigrator().Update(this.TargetMigration);

      protected override void OverrideConfiguration(DbMigrationsConfiguration configuration)
      {
        base.OverrideConfiguration(configuration);
        if (!this.Force)
          return;
        configuration.AutomaticMigrationDataLossAllowed = true;
      }
    }

    [Serializable]
    private class ScriptUpdateRunner : ToolingFacade.BaseRunner
    {
      public string SourceMigration { get; set; }

      public string TargetMigration { get; set; }

      public bool Force { get; set; }

      protected override void RunCore() => AppDomain.CurrentDomain.SetData("result", (object) new MigratorScriptingDecorator(this.GetMigrator()).ScriptUpdate(this.SourceMigration, this.TargetMigration));

      protected override void OverrideConfiguration(DbMigrationsConfiguration configuration)
      {
        base.OverrideConfiguration(configuration);
        if (!this.Force)
          return;
        configuration.AutomaticMigrationDataLossAllowed = true;
      }
    }

    [Serializable]
    private class ScaffoldRunner : ToolingFacade.BaseRunner
    {
      public string MigrationName { get; set; }

      public string Language { get; set; }

      public string RootNamespace { get; set; }

      public bool IgnoreChanges { get; set; }

      protected override void RunCore()
      {
        DbMigrationsConfiguration configuration = this.GetConfiguration();
        MigrationScaffolder scaffolder = new MigrationScaffolder(configuration);
        string s2 = configuration.MigrationsNamespace;
        if (this.Language == "vb" && !string.IsNullOrWhiteSpace(this.RootNamespace))
        {
          if (this.RootNamespace.EqualsIgnoreCase(s2))
          {
            s2 = (string) null;
          }
          else
          {
            if (s2 == null || !s2.StartsWith(this.RootNamespace + ".", StringComparison.OrdinalIgnoreCase))
              throw System.Data.Entity.Resources.Error.MigrationsNamespaceNotUnderRootNamespace((object) s2, (object) this.RootNamespace);
            s2 = s2.Substring(this.RootNamespace.Length + 1);
          }
        }
        scaffolder.Namespace = s2;
        AppDomain.CurrentDomain.SetData("result", (object) this.Scaffold(scaffolder));
      }

      protected virtual ScaffoldedMigration Scaffold(
        MigrationScaffolder scaffolder)
      {
        return scaffolder.Scaffold(this.MigrationName, this.IgnoreChanges);
      }

      protected override void OverrideConfiguration(DbMigrationsConfiguration configuration)
      {
        base.OverrideConfiguration(configuration);
        if (!(this.Language == "vb") || !(configuration.CodeGenerator is CSharpMigrationCodeGenerator))
          return;
        configuration.CodeGenerator = (MigrationCodeGenerator) new VisualBasicMigrationCodeGenerator();
      }
    }

    [Serializable]
    private class InitialCreateScaffoldRunner : ToolingFacade.ScaffoldRunner
    {
      protected override ScaffoldedMigration Scaffold(
        MigrationScaffolder scaffolder)
      {
        return scaffolder.ScaffoldInitialCreate();
      }
    }

    [Serializable]
    private class GetContextTypesRunner : ToolingFacade.BaseRunner
    {
      protected override void RunCore() => AppDomain.CurrentDomain.SetData("result", (object) this.LoadContextAssembly().GetAccessibleTypes().Where<Type>((Func<Type, bool>) (t => !t.IsAbstract && !t.IsGenericType && typeof (DbContext).IsAssignableFrom(t))).Select<Type, string>((Func<Type, string>) (t => t.FullName)).ToList<string>());
    }

    [Serializable]
    private class GetContextTypeRunner : ToolingFacade.BaseRunner
    {
      public string ContextTypeName { get; set; }

      protected override void RunCore() => AppDomain.CurrentDomain.SetData("result", (object) new TypeFinder(this.LoadContextAssembly()).FindType(typeof (DbContext), this.ContextTypeName, (Func<IEnumerable<Type>, IEnumerable<Type>>) (types => types.Where<Type>((Func<Type, bool>) (t => !typeof (HistoryContext).IsAssignableFrom(t) && !t.IsAbstract && !t.IsGenericType))), new Func<string, Exception>(System.Data.Entity.Resources.Error.EnableMigrations_NoContext), (Func<string, IEnumerable<Type>, Exception>) ((assembly, types) =>
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(System.Data.Entity.Resources.Strings.EnableMigrations_MultipleContexts((object) assembly));
        foreach (Type type in types)
        {
          stringBuilder.AppendLine();
          stringBuilder.Append(System.Data.Entity.Resources.Strings.EnableMigrationsForContext((object) type.FullName));
        }
        return (Exception) new MigrationsException(stringBuilder.ToString());
      }), new Func<string, string, Exception>(System.Data.Entity.Resources.Error.EnableMigrations_NoContextWithName), new Func<string, string, Exception>(System.Data.Entity.Resources.Error.EnableMigrations_MultipleContextsWithName)).FullName);
    }
  }
}
