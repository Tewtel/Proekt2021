// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.RenameColumnOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents renaming an existing column.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class RenameColumnOperation : MigrationOperation
  {
    private readonly string _table;
    private readonly string _name;
    private string _newName;

    /// <summary>
    /// Initializes a new instance of the RenameColumnOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="table"> Name of the table the column belongs to. </param>
    /// <param name="name"> Name of the column to be renamed. </param>
    /// <param name="newName"> New name for the column. </param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public RenameColumnOperation(
      string table,
      string name,
      string newName,
      object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(table, nameof (table));
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      System.Data.Entity.Utilities.Check.NotEmpty(newName, nameof (newName));
      this._table = table;
      this._name = name;
      this._newName = newName;
    }

    /// <summary>Gets the name of the table the column belongs to.</summary>
    public virtual string Table => this._table;

    /// <summary>Gets the name of the column to be renamed.</summary>
    public virtual string Name => this._name;

    /// <summary>Gets the new name for the column.</summary>
    public virtual string NewName
    {
      get => this._newName;
      internal set => this._newName = value;
    }

    /// <summary>Gets an operation that reverts the rename.</summary>
    public override MigrationOperation Inverse => (MigrationOperation) new RenameColumnOperation(this.Table, this.NewName, this.Name);

    /// <inheritdoc />
    public override bool IsDestructiveChange => false;
  }
}
