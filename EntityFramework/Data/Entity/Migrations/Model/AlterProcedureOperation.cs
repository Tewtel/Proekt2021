// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.AlterProcedureOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents altering an existing stored procedure.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class AlterProcedureOperation : ProcedureOperation
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Migrations.Model.AlterProcedureOperation" /> class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="name">The name of the stored procedure.</param>
    /// <param name="bodySql">The body of the stored procedure expressed in SQL.</param>
    /// <param name="anonymousArguments">Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'.</param>
    public AlterProcedureOperation(string name, string bodySql, object anonymousArguments = null)
      : base(name, bodySql, anonymousArguments)
    {
    }

    /// <summary>
    /// Gets an operation that will revert this operation.
    /// Always returns a <see cref="T:System.Data.Entity.Migrations.Model.NotSupportedOperation" />.
    /// </summary>
    public override MigrationOperation Inverse => (MigrationOperation) NotSupportedOperation.Instance;
  }
}
