// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.DropProcedureOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Drops a stored procedure from the database.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class DropProcedureOperation : MigrationOperation
  {
    private readonly string _name;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Migrations.Model.DropProcedureOperation" /> class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="name">The name of the stored procedure to drop.</param>
    /// <param name="anonymousArguments"> Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public DropProcedureOperation(string name, object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      this._name = name;
    }

    /// <summary>Gets the name of the stored procedure to drop.</summary>
    /// <value>The name of the stored procedure to drop.</value>
    public virtual string Name => this._name;

    /// <summary>
    /// Gets an operation that will revert this operation.
    /// Always returns a <see cref="T:System.Data.Entity.Migrations.Model.NotSupportedOperation" />.
    /// </summary>
    public override MigrationOperation Inverse => (MigrationOperation) NotSupportedOperation.Instance;

    /// <summary>
    /// Gets a value indicating if this operation may result in data loss. Always returns false.
    /// </summary>
    public override bool IsDestructiveChange => false;
  }
}
