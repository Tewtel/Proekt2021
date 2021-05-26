// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.CreateTableOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents creating a table.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class CreateTableOperation : MigrationOperation, IAnnotationTarget
  {
    private readonly string _name;
    private readonly List<ColumnModel> _columns = new List<ColumnModel>();
    private AddPrimaryKeyOperation _primaryKey;
    private readonly IDictionary<string, object> _annotations;

    /// <summary>
    /// Initializes a new instance of the CreateTableOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="name"> Name of the table to be created. </param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public CreateTableOperation(string name, object anonymousArguments = null)
      : this(name, (IDictionary<string, object>) null, anonymousArguments)
    {
    }

    /// <summary>
    /// Initializes a new instance of the CreateTableOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="name"> Name of the table to be created. </param>
    /// <param name="annotations">Custom annotations that exist on the table to be created. May be null or empty.</param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public CreateTableOperation(
      string name,
      IDictionary<string, object> annotations,
      object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      this._name = name;
      this._annotations = annotations ?? (IDictionary<string, object>) new Dictionary<string, object>();
    }

    /// <summary>Gets the name of the table to be created.</summary>
    public virtual string Name => this._name;

    /// <summary>Gets the columns to be included in the new table.</summary>
    public virtual IList<ColumnModel> Columns => (IList<ColumnModel>) this._columns;

    /// <summary>Gets or sets the primary key for the new table.</summary>
    public AddPrimaryKeyOperation PrimaryKey
    {
      get => this._primaryKey;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<AddPrimaryKeyOperation>(value, nameof (value));
        this._primaryKey = value;
        this._primaryKey.Table = this.Name;
      }
    }

    /// <summary>
    /// Gets custom annotations that exist on the table to be created.
    /// </summary>
    public virtual IDictionary<string, object> Annotations => this._annotations;

    /// <summary>Gets an operation to drop the table.</summary>
    public override MigrationOperation Inverse => (MigrationOperation) new DropTableOperation(this.Name, this.Annotations, (IDictionary<string, IDictionary<string, object>>) this.Columns.Where<ColumnModel>((Func<ColumnModel, bool>) (c => c.Annotations.Count > 0)).ToDictionary<ColumnModel, string, IDictionary<string, object>>((Func<ColumnModel, string>) (c => c.Name), (Func<ColumnModel, IDictionary<string, object>>) (c => (IDictionary<string, object>) c.Annotations.ToDictionary<KeyValuePair<string, AnnotationValues>, string, object>((Func<KeyValuePair<string, AnnotationValues>, string>) (a => a.Key), (Func<KeyValuePair<string, AnnotationValues>, object>) (a => a.Value.NewValue)))));

    /// <inheritdoc />
    public override bool IsDestructiveChange => false;

    bool IAnnotationTarget.HasAnnotations => this.Annotations.Any<KeyValuePair<string, object>>() || this.Columns.SelectMany<ColumnModel, KeyValuePair<string, AnnotationValues>>((Func<ColumnModel, IEnumerable<KeyValuePair<string, AnnotationValues>>>) (c => (IEnumerable<KeyValuePair<string, AnnotationValues>>) c.Annotations)).Any<KeyValuePair<string, AnnotationValues>>();
  }
}
