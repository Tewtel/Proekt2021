// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.RenameProcedureOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Utilities;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents renaming a stored procedure in the database.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class RenameProcedureOperation : MigrationOperation
  {
    private readonly string _name;
    private readonly string _newName;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Migrations.Model.RenameProcedureOperation" /> class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="name">The name of the stored procedure to rename.</param>
    /// <param name="newName">The new name for the stored procedure.</param>
    /// <param name="anonymousArguments"> Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public RenameProcedureOperation(string name, string newName, object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      System.Data.Entity.Utilities.Check.NotEmpty(newName, nameof (newName));
      this._name = name;
      this._newName = newName;
    }

    /// <summary>Gets the name of the stored procedure to rename.</summary>
    /// <value>The name of the stored procedure to rename.</value>
    public virtual string Name => this._name;

    /// <summary>Gets the new name for the stored procedure.</summary>
    /// <value>The new name for the stored procedure.</value>
    public virtual string NewName => this._newName;

    /// <summary>Gets an operation that will revert this operation.</summary>
    public override MigrationOperation Inverse
    {
      get
      {
        DatabaseName databaseName = DatabaseName.Parse(this._name);
        return (MigrationOperation) new RenameProcedureOperation(new DatabaseName(DatabaseName.Parse(this._newName).Name, databaseName.Schema).ToString(), databaseName.Name);
      }
    }

    /// <summary>
    /// Gets a value indicating if this operation may result in data loss. Always returns false.
    /// </summary>
    public override bool IsDestructiveChange => false;
  }
}
