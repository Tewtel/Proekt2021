﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.DropColumnOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents a column being dropped from a table.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class DropColumnOperation : MigrationOperation, IAnnotationTarget
  {
    private readonly string _table;
    private readonly string _name;
    private readonly AddColumnOperation _inverse;
    private readonly IDictionary<string, object> _removedAnnotations;

    /// <summary>
    /// Initializes a new instance of the DropColumnOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="table"> The name of the table the column should be dropped from. </param>
    /// <param name="name"> The name of the column to be dropped. </param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public DropColumnOperation(string table, string name, object anonymousArguments = null)
      : this(table, name, (IDictionary<string, object>) null, (AddColumnOperation) null, anonymousArguments)
    {
    }

    /// <summary>
    /// Initializes a new instance of the DropColumnOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="table"> The name of the table the column should be dropped from. </param>
    /// <param name="name"> The name of the column to be dropped. </param>
    /// <param name="removedAnnotations">Custom annotations that exist on the column that is being dropped. May be null or empty.</param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public DropColumnOperation(
      string table,
      string name,
      IDictionary<string, object> removedAnnotations,
      object anonymousArguments = null)
      : this(table, name, removedAnnotations, (AddColumnOperation) null, anonymousArguments)
    {
    }

    /// <summary>
    /// Initializes a new instance of the DropColumnOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="table"> The name of the table the column should be dropped from. </param>
    /// <param name="name"> The name of the column to be dropped. </param>
    /// <param name="inverse"> The operation that represents reverting the drop operation. </param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public DropColumnOperation(
      string table,
      string name,
      AddColumnOperation inverse,
      object anonymousArguments = null)
      : this(table, name, (IDictionary<string, object>) null, inverse, anonymousArguments)
    {
    }

    /// <summary>
    /// Initializes a new instance of the DropColumnOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="table"> The name of the table the column should be dropped from. </param>
    /// <param name="name"> The name of the column to be dropped. </param>
    /// <param name="removedAnnotations">Custom annotations that exist on the column that is being dropped. May be null or empty.</param>
    /// <param name="inverse"> The operation that represents reverting the drop operation. </param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public DropColumnOperation(
      string table,
      string name,
      IDictionary<string, object> removedAnnotations,
      AddColumnOperation inverse,
      object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(table, nameof (table));
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      this._table = table;
      this._name = name;
      this._removedAnnotations = removedAnnotations ?? (IDictionary<string, object>) new Dictionary<string, object>();
      this._inverse = inverse;
    }

    /// <summary>
    /// Gets the name of the table the column should be dropped from.
    /// </summary>
    public string Table => this._table;

    /// <summary>Gets the name of the column to be dropped.</summary>
    public string Name => this._name;

    /// <summary>
    /// Gets custom annotations that exist on the column that is being dropped.
    /// </summary>
    public IDictionary<string, object> RemovedAnnotations => this._removedAnnotations;

    /// <summary>
    /// Gets an operation that represents reverting dropping the column.
    /// The inverse cannot be automatically calculated,
    /// if it was not supplied to the constructor this property will return null.
    /// </summary>
    public override MigrationOperation Inverse => (MigrationOperation) this._inverse;

    /// <inheritdoc />
    public override bool IsDestructiveChange => true;

    bool IAnnotationTarget.HasAnnotations
    {
      get
      {
        AddColumnOperation inverse = this.Inverse as AddColumnOperation;
        if (this.RemovedAnnotations.Any<KeyValuePair<string, object>>())
          return true;
        return inverse != null && ((IAnnotationTarget) inverse).HasAnnotations;
      }
    }
  }
}
