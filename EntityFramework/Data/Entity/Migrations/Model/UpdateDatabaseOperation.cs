﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.UpdateDatabaseOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Used when scripting an update database operation to store the operations that would have been performed against the database.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class UpdateDatabaseOperation : MigrationOperation
  {
    private readonly IList<DbQueryCommandTree> _historyQueryTrees;
    private readonly IList<UpdateDatabaseOperation.Migration> _migrations = (IList<UpdateDatabaseOperation.Migration>) new List<UpdateDatabaseOperation.Migration>();

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Migrations.Model.UpdateDatabaseOperation" /> class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="historyQueryTrees">
    /// The queries used to determine if this migration needs to be applied to the database.
    /// This is used to generate an idempotent SQL script that can be run against a database at any version.
    /// </param>
    public UpdateDatabaseOperation(IList<DbQueryCommandTree> historyQueryTrees)
      : base((object) null)
    {
      System.Data.Entity.Utilities.Check.NotNull<IList<DbQueryCommandTree>>(historyQueryTrees, nameof (historyQueryTrees));
      this._historyQueryTrees = historyQueryTrees;
    }

    /// <summary>
    /// The queries used to determine if this migration needs to be applied to the database.
    /// This is used to generate an idempotent SQL script that can be run against a database at any version.
    /// </summary>
    public IList<DbQueryCommandTree> HistoryQueryTrees => this._historyQueryTrees;

    /// <summary>
    /// Gets the migrations applied during the update database operation.
    /// </summary>
    /// <value>
    /// The migrations applied during the update database operation.
    /// </value>
    public IList<UpdateDatabaseOperation.Migration> Migrations => this._migrations;

    /// <summary>
    /// Adds a migration to this update database operation.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="migrationId">The id of the migration.</param>
    /// <param name="operations">The individual operations applied by the migration.</param>
    public void AddMigration(string migrationId, IList<MigrationOperation> operations)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(migrationId, nameof (migrationId));
      System.Data.Entity.Utilities.Check.NotNull<IList<MigrationOperation>>(operations, nameof (operations));
      this._migrations.Add(new UpdateDatabaseOperation.Migration(migrationId, operations));
    }

    /// <summary>
    /// Gets a value indicating if any of the operations may result in data loss.
    /// </summary>
    public override bool IsDestructiveChange => false;

    /// <summary>Represents a migration to be applied to the database.</summary>
    public class Migration
    {
      private readonly string _migrationId;
      private readonly IList<MigrationOperation> _operations;

      internal Migration(string migrationId, IList<MigrationOperation> operations)
      {
        this._migrationId = migrationId;
        this._operations = operations;
      }

      /// <summary>Gets the id of the migration.</summary>
      /// <value>The id of the migration.</value>
      public string MigrationId => this._migrationId;

      /// <summary>
      /// Gets the individual operations applied by this migration.
      /// </summary>
      /// <value>The individual operations applied by this migration.</value>
      public IList<MigrationOperation> Operations => this._operations;
    }
  }
}
