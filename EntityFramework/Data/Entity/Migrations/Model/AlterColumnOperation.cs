// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.AlterColumnOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents altering an existing column.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class AlterColumnOperation : MigrationOperation, IAnnotationTarget
  {
    private readonly string _table;
    private readonly ColumnModel _column;
    private readonly AlterColumnOperation _inverse;
    private readonly bool _destructiveChange;

    /// <summary>
    /// Initializes a new instance of the AlterColumnOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="table"> The name of the table that the column belongs to. </param>
    /// <param name="column"> Details of what the column should be altered to. </param>
    /// <param name="isDestructiveChange"> Value indicating if this change will result in data loss. </param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public AlterColumnOperation(
      string table,
      ColumnModel column,
      bool isDestructiveChange,
      object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(table, nameof (table));
      System.Data.Entity.Utilities.Check.NotNull<ColumnModel>(column, nameof (column));
      this._table = table;
      this._column = column;
      this._destructiveChange = isDestructiveChange;
    }

    /// <summary>
    /// Initializes a new instance of the AlterColumnOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="table"> The name of the table that the column belongs to. </param>
    /// <param name="column"> Details of what the column should be altered to. </param>
    /// <param name="isDestructiveChange"> Value indicating if this change will result in data loss. </param>
    /// <param name="inverse"> An operation to revert this alteration of the column. </param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public AlterColumnOperation(
      string table,
      ColumnModel column,
      bool isDestructiveChange,
      AlterColumnOperation inverse,
      object anonymousArguments = null)
      : this(table, column, isDestructiveChange, anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotNull<AlterColumnOperation>(inverse, nameof (inverse));
      this._inverse = inverse;
    }

    /// <summary>
    /// Gets the name of the table that the column belongs to.
    /// </summary>
    public string Table => this._table;

    /// <summary>Gets the new definition for the column.</summary>
    public ColumnModel Column => this._column;

    /// <summary>
    /// Gets an operation that represents reverting the alteration.
    /// The inverse cannot be automatically calculated,
    /// if it was not supplied to the constructor this property will return null.
    /// </summary>
    public override MigrationOperation Inverse => (MigrationOperation) this._inverse;

    /// <inheritdoc />
    public override bool IsDestructiveChange => this._destructiveChange;

    bool IAnnotationTarget.HasAnnotations
    {
      get
      {
        AlterColumnOperation inverse = this.Inverse as AlterColumnOperation;
        if (this.Column.Annotations.Any<KeyValuePair<string, AnnotationValues>>())
          return true;
        return inverse != null && inverse.Column.Annotations.Any<KeyValuePair<string, AnnotationValues>>();
      }
    }
  }
}
