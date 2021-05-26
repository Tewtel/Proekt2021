// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Infrastructure.MigratorLoggingDecorator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Xml.Linq;

namespace System.Data.Entity.Migrations.Infrastructure
{
  /// <summary>
  /// Decorator to provide logging during migrations operations..
  /// </summary>
  public class MigratorLoggingDecorator : MigratorBase
  {
    private readonly MigrationsLogger _logger;
    private string _lastInfoMessage;

    /// <summary>
    /// Initializes a new instance of the MigratorLoggingDecorator class.
    /// </summary>
    /// <param name="innerMigrator"> The migrator that this decorator is wrapping. </param>
    /// <param name="logger"> The logger to write messages to. </param>
    public MigratorLoggingDecorator(MigratorBase innerMigrator, MigrationsLogger logger)
      : base(innerMigrator)
    {
      System.Data.Entity.Utilities.Check.NotNull<MigratorBase>(innerMigrator, nameof (innerMigrator));
      System.Data.Entity.Utilities.Check.NotNull<MigrationsLogger>(logger, nameof (logger));
      this._logger = logger;
      this._logger.Verbose(System.Data.Entity.Resources.Strings.LoggingTargetDatabase((object) this.TargetDatabase));
    }

    internal override void AutoMigrate(
      string migrationId,
      VersionedModel sourceModel,
      VersionedModel targetModel,
      bool downgrading)
    {
      this._logger.Info(downgrading ? System.Data.Entity.Resources.Strings.LoggingRevertAutoMigrate((object) migrationId) : System.Data.Entity.Resources.Strings.LoggingAutoMigrate((object) migrationId));
      base.AutoMigrate(migrationId, sourceModel, targetModel, downgrading);
    }

    internal override void ExecuteSql(
      MigrationStatement migrationStatement,
      DbConnection connection,
      DbTransaction transaction,
      DbInterceptionContext interceptionContext)
    {
      this._logger.Verbose(migrationStatement.Sql);
      DbProviderServices.GetProviderServices(connection)?.RegisterInfoMessageHandler(connection, (Action<string>) (message =>
      {
        if (string.Equals(message, this._lastInfoMessage, StringComparison.OrdinalIgnoreCase))
          return;
        this._logger.Warning(message);
        this._lastInfoMessage = message;
      }));
      base.ExecuteSql(migrationStatement, connection, transaction, interceptionContext);
    }

    internal override void Upgrade(
      IEnumerable<string> pendingMigrations,
      string targetMigrationId,
      string lastMigrationId)
    {
      int num = pendingMigrations.Count<string>();
      this._logger.Info(num > 0 ? System.Data.Entity.Resources.Strings.LoggingPendingMigrations((object) num, (object) pendingMigrations.Join<string>()) : (string.IsNullOrWhiteSpace(targetMigrationId) ? System.Data.Entity.Resources.Strings.LoggingNoExplicitMigrations : System.Data.Entity.Resources.Strings.LoggingAlreadyAtTarget((object) targetMigrationId)));
      base.Upgrade(pendingMigrations, targetMigrationId, lastMigrationId);
    }

    internal override void Downgrade(IEnumerable<string> pendingMigrations)
    {
      IEnumerable<string> strings = pendingMigrations.Take<string>(pendingMigrations.Count<string>() - 1);
      this._logger.Info(System.Data.Entity.Resources.Strings.LoggingPendingMigrationsDown((object) strings.Count<string>(), (object) strings.Join<string>()));
      base.Downgrade(pendingMigrations);
    }

    internal override void ApplyMigration(DbMigration migration, DbMigration lastMigration)
    {
      this._logger.Info(System.Data.Entity.Resources.Strings.LoggingApplyMigration((object) ((IMigrationMetadata) migration).Id));
      base.ApplyMigration(migration, lastMigration);
    }

    internal override void RevertMigration(
      string migrationId,
      DbMigration migration,
      XDocument targetModel)
    {
      this._logger.Info(System.Data.Entity.Resources.Strings.LoggingRevertMigration((object) migrationId));
      base.RevertMigration(migrationId, migration, targetModel);
    }

    internal override void SeedDatabase()
    {
      this._logger.Info(System.Data.Entity.Resources.Strings.LoggingSeedingDatabase);
      base.SeedDatabase();
    }

    internal override void UpgradeHistory(IEnumerable<MigrationOperation> upgradeOperations)
    {
      this._logger.Info(System.Data.Entity.Resources.Strings.UpgradingHistoryTable);
      base.UpgradeHistory(upgradeOperations);
    }
  }
}
