// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.ProcedureOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// A migration operation that affects stored procedures.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public abstract class ProcedureOperation : MigrationOperation
  {
    private readonly string _name;
    private readonly string _bodySql;
    private readonly List<ParameterModel> _parameters = new List<ParameterModel>();

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Migrations.Model.ProcedureOperation" /> class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="name">The name of the stored procedure.</param>
    /// <param name="bodySql">The body of the stored procedure expressed in SQL.</param>
    /// <param name="anonymousArguments"> Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    protected ProcedureOperation(string name, string bodySql, object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      this._name = name;
      this._bodySql = bodySql;
    }

    /// <summary>Gets the name of the stored procedure.</summary>
    /// <value>The name of the stored procedure.</value>
    public virtual string Name => this._name;

    /// <summary>
    /// Gets the body of the stored procedure expressed in SQL.
    /// </summary>
    /// <value>The body of the stored procedure expressed in SQL.</value>
    public string BodySql => this._bodySql;

    /// <summary>Gets the parameters of the stored procedure.</summary>
    /// <value>The parameters of the stored procedure.</value>
    public virtual IList<ParameterModel> Parameters => (IList<ParameterModel>) this._parameters;

    /// <summary>
    /// Gets a value indicating if this operation may result in data loss. Always returns false.
    /// </summary>
    public override bool IsDestructiveChange => false;
  }
}
