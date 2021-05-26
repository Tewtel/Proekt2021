// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.SqlOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents a provider specific SQL statement to be executed directly against the target database.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class SqlOperation : MigrationOperation
  {
    private readonly string _sql;

    /// <summary>
    /// Initializes a new instance of the SqlOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="sql"> The SQL to be executed. </param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public SqlOperation(string sql, object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(sql, nameof (sql));
      this._sql = sql;
    }

    /// <summary>Gets the SQL to be executed.</summary>
    public virtual string Sql => this._sql;

    /// <summary>
    /// Gets or sets a value indicating whether this statement should be performed outside of
    /// the transaction scope that is used to make the migration process transactional.
    /// If set to true, this operation will not be rolled back if the migration process fails.
    /// </summary>
    public virtual bool SuppressTransaction { get; set; }

    /// <inheritdoc />
    public override bool IsDestructiveChange => true;
  }
}
