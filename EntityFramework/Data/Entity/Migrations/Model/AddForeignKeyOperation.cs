// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.AddForeignKeyOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents a foreign key constraint being added to a table.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class AddForeignKeyOperation : ForeignKeyOperation
  {
    private readonly List<string> _principalColumns = new List<string>();

    /// <summary>
    /// Initializes a new instance of the AddForeignKeyOperation class.
    /// The PrincipalTable, PrincipalColumns, DependentTable and DependentColumns properties should also be populated.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public AddForeignKeyOperation(object anonymousArguments = null)
      : base(anonymousArguments)
    {
    }

    /// <summary>
    /// The names of the column(s) that the foreign key constraint should target.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    public IList<string> PrincipalColumns => (IList<string>) this._principalColumns;

    /// <summary>
    /// Gets or sets a value indicating if cascade delete should be configured on the foreign key constraint.
    /// </summary>
    public bool CascadeDelete { get; set; }

    /// <summary>
    /// Gets an operation to create an index on the foreign key column(s).
    /// </summary>
    /// <returns> An operation to add the index. </returns>
    public virtual CreateIndexOperation CreateCreateIndexOperation()
    {
      CreateIndexOperation createIndexOperation1 = new CreateIndexOperation();
      createIndexOperation1.Table = this.DependentTable;
      CreateIndexOperation createIndexOperation = createIndexOperation1;
      this.DependentColumns.Each<string>((Action<string>) (c => createIndexOperation.Columns.Add(c)));
      return createIndexOperation;
    }

    /// <summary>Gets an operation to drop the foreign key constraint.</summary>
    public override MigrationOperation Inverse
    {
      get
      {
        DropForeignKeyOperation foreignKeyOperation = new DropForeignKeyOperation();
        foreignKeyOperation.Name = this.Name;
        foreignKeyOperation.PrincipalTable = this.PrincipalTable;
        foreignKeyOperation.DependentTable = this.DependentTable;
        DropForeignKeyOperation dropForeignKeyOperation = foreignKeyOperation;
        this.DependentColumns.Each<string>((Action<string>) (c => dropForeignKeyOperation.DependentColumns.Add(c)));
        return (MigrationOperation) dropForeignKeyOperation;
      }
    }

    /// <inheritdoc />
    public override bool IsDestructiveChange => false;
  }
}
