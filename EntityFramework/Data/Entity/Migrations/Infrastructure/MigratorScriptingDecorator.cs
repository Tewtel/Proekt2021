﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Infrastructure.MigratorScriptingDecorator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Text;

namespace System.Data.Entity.Migrations.Infrastructure
{
  /// <summary>
  /// Decorator to produce a SQL script instead of applying changes to the database.
  /// Using this decorator to wrap <see cref="T:System.Data.Entity.Migrations.DbMigrator" /> will prevent <see cref="T:System.Data.Entity.Migrations.DbMigrator" />
  /// from applying any changes to the target database.
  /// </summary>
  public class MigratorScriptingDecorator : MigratorBase
  {
    private readonly StringBuilder _sqlBuilder = new StringBuilder();
    private UpdateDatabaseOperation _updateDatabaseOperation;

    /// <summary>
    /// Initializes a new instance of the  MigratorScriptingDecorator class.
    /// </summary>
    /// <param name="innerMigrator"> The migrator that this decorator is wrapping. </param>
    public MigratorScriptingDecorator(MigratorBase innerMigrator)
      : base(innerMigrator)
    {
      System.Data.Entity.Utilities.Check.NotNull<MigratorBase>(innerMigrator, nameof (innerMigrator));
    }

    /// <summary>Produces a script to update the database.</summary>
    /// <param name="sourceMigration">
    /// The migration to update from. If null is supplied, a script to update the
    /// current database will be produced.
    /// </param>
    /// <param name="targetMigration">
    /// The migration to update to. If null is supplied,
    /// a script to update to the latest migration will be produced.
    /// </param>
    /// <returns> The generated SQL script. </returns>
    public string ScriptUpdate(string sourceMigration, string targetMigration)
    {
      this._sqlBuilder.Clear();
      if (string.IsNullOrWhiteSpace(sourceMigration))
      {
        this.Update(targetMigration);
      }
      else
      {
        string sourceMigrationId = !sourceMigration.IsAutomaticMigration() ? this.GetMigrationId(sourceMigration) : throw System.Data.Entity.Resources.Error.AutoNotValidForScriptWindows((object) sourceMigration);
        IEnumerable<string> strings = this.GetLocalMigrations().Where<string>((Func<string, bool>) (m => string.CompareOrdinal(m, sourceMigrationId) > 0));
        string targetMigrationId = (string) null;
        if (!string.IsNullOrWhiteSpace(targetMigration))
        {
          targetMigrationId = !targetMigration.IsAutomaticMigration() ? this.GetMigrationId(targetMigration) : throw System.Data.Entity.Resources.Error.AutoNotValidForScriptWindows((object) targetMigration);
          if (string.CompareOrdinal(sourceMigrationId, targetMigrationId) > 0)
            throw System.Data.Entity.Resources.Error.DownScriptWindowsNotSupported();
          strings = strings.Where<string>((Func<string, bool>) (m => string.CompareOrdinal(m, targetMigrationId) <= 0));
        }
        this._updateDatabaseOperation = sourceMigration == "0" ? new UpdateDatabaseOperation((IList<DbQueryCommandTree>) this.CreateDiscoveryQueryTrees().ToList<DbQueryCommandTree>()) : (UpdateDatabaseOperation) null;
        this.Upgrade(strings, targetMigrationId, sourceMigrationId);
        if (this._updateDatabaseOperation != null)
          this.ExecuteStatements(base.GenerateStatements((IList<MigrationOperation>) new UpdateDatabaseOperation[1]
          {
            this._updateDatabaseOperation
          }, (string) null));
      }
      return this._sqlBuilder.ToString();
    }

    internal override IEnumerable<MigrationStatement> GenerateStatements(
      IList<MigrationOperation> operations,
      string migrationId)
    {
      if (this._updateDatabaseOperation == null)
        return base.GenerateStatements(operations, migrationId);
      this._updateDatabaseOperation.AddMigration(migrationId, operations);
      return Enumerable.Empty<MigrationStatement>();
    }

    internal override void EnsureDatabaseExists(Action mustSucceedToKeepDatabase) => mustSucceedToKeepDatabase();

    internal override void ExecuteStatements(
      IEnumerable<MigrationStatement> migrationStatements)
    {
      MigratorScriptingDecorator.BuildSqlScript(migrationStatements, this._sqlBuilder);
    }

    internal static void BuildSqlScript(
      IEnumerable<MigrationStatement> migrationStatements,
      StringBuilder sqlBuilder)
    {
      foreach (MigrationStatement migrationStatement in migrationStatements)
      {
        if (!string.IsNullOrWhiteSpace(migrationStatement.Sql))
        {
          if (!string.IsNullOrWhiteSpace(migrationStatement.BatchTerminator) && sqlBuilder.Length > 0)
          {
            sqlBuilder.AppendLine(migrationStatement.BatchTerminator);
            sqlBuilder.AppendLine();
          }
          sqlBuilder.AppendLine(migrationStatement.Sql);
        }
      }
    }

    internal override void SeedDatabase()
    {
    }

    internal override bool HistoryExists() => false;
  }
}
