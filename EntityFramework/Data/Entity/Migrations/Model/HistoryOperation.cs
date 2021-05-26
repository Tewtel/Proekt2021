// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.HistoryOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Operation representing DML changes to the migrations history table.
  /// The migrations history table is used to store a log of the migrations that have been applied to the database.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class HistoryOperation : MigrationOperation
  {
    private readonly IList<DbModificationCommandTree> _commandTrees;

    /// <summary>
    /// Initializes a new instance of the HistoryOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="commandTrees"> A sequence of command trees representing the operations being applied to the history table. </param>
    /// <param name="anonymousArguments"> Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public HistoryOperation(
      IList<DbModificationCommandTree> commandTrees,
      object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotNull<IList<DbModificationCommandTree>>(commandTrees, nameof (commandTrees));
      this._commandTrees = commandTrees.Any<DbModificationCommandTree>() ? commandTrees : throw new ArgumentException(System.Data.Entity.Resources.Strings.CollectionEmpty((object) nameof (commandTrees), (object) nameof (HistoryOperation)));
    }

    /// <summary>
    /// A sequence of commands representing the operations being applied to the history table.
    /// </summary>
    public IList<DbModificationCommandTree> CommandTrees => this._commandTrees;

    /// <inheritdoc />
    public override bool IsDestructiveChange => false;
  }
}
