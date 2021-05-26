// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.DropPrimaryKeyOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Utilities;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents dropping a primary key from a table.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class DropPrimaryKeyOperation : PrimaryKeyOperation
  {
    /// <summary>
    /// Initializes a new instance of the DropPrimaryKeyOperation class.
    /// The Table and Columns properties should also be populated.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public DropPrimaryKeyOperation(object anonymousArguments = null)
      : base(anonymousArguments)
    {
    }

    /// <summary>Gets an operation to add the primary key.</summary>
    public override MigrationOperation Inverse
    {
      get
      {
        AddPrimaryKeyOperation primaryKeyOperation = new AddPrimaryKeyOperation();
        primaryKeyOperation.Name = this.Name;
        primaryKeyOperation.Table = this.Table;
        primaryKeyOperation.IsClustered = this.IsClustered;
        AddPrimaryKeyOperation addPrimaryKeyOperation = primaryKeyOperation;
        this.Columns.Each<string>((Action<string>) (c => addPrimaryKeyOperation.Columns.Add(c)));
        return (MigrationOperation) addPrimaryKeyOperation;
      }
    }

    /// <summary>
    /// Used when altering the migrations history table so that the table can be rebuilt rather than just dropping and adding the primary key.
    /// </summary>
    /// <value>
    /// The create table operation for the migrations history table.
    /// </value>
    public CreateTableOperation CreateTableOperation { get; internal set; }
  }
}
