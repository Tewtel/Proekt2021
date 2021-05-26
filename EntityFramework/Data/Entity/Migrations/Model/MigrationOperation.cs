// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.MigrationOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Utilities;
using System.Reflection;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents an operation to modify a database schema.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public abstract class MigrationOperation
  {
    private readonly IDictionary<string, object> _anonymousArguments = (IDictionary<string, object>) new Dictionary<string, object>();

    /// <summary>
    /// Initializes a new instance of the MigrationOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="anonymousArguments">
    /// Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue"
    /// }'.
    /// </param>
    protected MigrationOperation(object anonymousArguments)
    {
      MigrationOperation migrationOperation = this;
      if (anonymousArguments == null)
        return;
      anonymousArguments.GetType().GetNonIndexerProperties().Each<PropertyInfo>((Action<PropertyInfo>) (p => migrationOperation._anonymousArguments.Add(p.Name, p.GetValue(anonymousArguments, (object[]) null))));
    }

    /// <summary>
    /// Gets additional arguments that may be processed by providers.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    public IDictionary<string, object> AnonymousArguments => this._anonymousArguments;

    /// <summary>Gets an operation that will revert this operation.</summary>
    public virtual MigrationOperation Inverse => (MigrationOperation) null;

    /// <summary>
    /// Gets a value indicating if this operation may result in data loss.
    /// </summary>
    public abstract bool IsDestructiveChange { get; }
  }
}
