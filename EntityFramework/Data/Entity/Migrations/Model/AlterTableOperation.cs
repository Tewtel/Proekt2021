// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.AlterTableOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents changes made to custom annotations on a table.
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class AlterTableOperation : MigrationOperation, IAnnotationTarget
  {
    private readonly string _name;
    private readonly List<ColumnModel> _columns = new List<ColumnModel>();
    private readonly IDictionary<string, AnnotationValues> _annotations;

    /// <summary>
    /// Initializes a new instance of the AlterTableOperation class.
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="name"> Name of the table on which annotations have changed. </param>
    /// <param name="annotations">The custom annotations on the table that have changed.</param>
    /// <param name="anonymousArguments">
    /// Additional arguments that may be processed by providers. Use anonymous type syntax to
    /// specify arguments e.g. 'new { SampleArgument = "MyValue" }'.
    /// </param>
    public AlterTableOperation(
      string name,
      IDictionary<string, AnnotationValues> annotations,
      object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      this._name = name;
      this._annotations = annotations ?? (IDictionary<string, AnnotationValues>) new Dictionary<string, AnnotationValues>();
    }

    /// <summary>
    /// Gets the name of the table on which annotations have changed.
    /// </summary>
    public virtual string Name => this._name;

    /// <summary>
    /// Gets the columns to be included in the table for which annotations have changed.
    /// </summary>
    public virtual IList<ColumnModel> Columns => (IList<ColumnModel>) this._columns;

    /// <summary>
    /// Gets the custom annotations that have changed on the table.
    /// </summary>
    public virtual IDictionary<string, AnnotationValues> Annotations => this._annotations;

    /// <summary>
    /// Gets an operation that is the inverse of this one such that annotations will be changed back to how
    /// they were before this operation was applied.
    /// </summary>
    public override MigrationOperation Inverse
    {
      get
      {
        AlterTableOperation alterTableOperation = new AlterTableOperation(this.Name, (IDictionary<string, AnnotationValues>) this.Annotations.ToDictionary<KeyValuePair<string, AnnotationValues>, string, AnnotationValues>((Func<KeyValuePair<string, AnnotationValues>, string>) (a => a.Key), (Func<KeyValuePair<string, AnnotationValues>, AnnotationValues>) (a => new AnnotationValues(a.Value.NewValue, a.Value.OldValue))));
        alterTableOperation._columns.AddRange((IEnumerable<ColumnModel>) this._columns);
        return (MigrationOperation) alterTableOperation;
      }
    }

    /// <inheritdoc />
    public override bool IsDestructiveChange => false;

    bool IAnnotationTarget.HasAnnotations => this.Annotations.Any<KeyValuePair<string, AnnotationValues>>() || this.Columns.SelectMany<ColumnModel, KeyValuePair<string, AnnotationValues>>((Func<ColumnModel, IEnumerable<KeyValuePair<string, AnnotationValues>>>) (c => (IEnumerable<KeyValuePair<string, AnnotationValues>>) c.Annotations)).Any<KeyValuePair<string, AnnotationValues>>();
  }
}
