// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.ForeignKeyOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Utilities;
using System.Globalization;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Base class for changes that affect foreign key constraints.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public abstract class ForeignKeyOperation : MigrationOperation
  {
    private string _principalTable;
    private string _dependentTable;
    private readonly List<string> _dependentColumns = new List<string>();
    private string _name;

    /// <summary>
    /// Initializes a new instance of the ForeignKeyOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    protected ForeignKeyOperation(object anonymousArguments = null)
      : base(anonymousArguments)
    {
    }

    /// <summary>
    /// Gets or sets the name of the table that the foreign key constraint targets.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    public string PrincipalTable
    {
      get => this._principalTable;
      set
      {
        System.Data.Entity.Utilities.Check.NotEmpty(value, nameof (value));
        this._principalTable = value;
      }
    }

    /// <summary>
    /// Gets or sets the name of the table that the foreign key columns exist in.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    public string DependentTable
    {
      get => this._dependentTable;
      set
      {
        System.Data.Entity.Utilities.Check.NotEmpty(value, nameof (value));
        this._dependentTable = value;
      }
    }

    /// <summary>
    /// The names of the foreign key column(s).
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    public IList<string> DependentColumns => (IList<string>) this._dependentColumns;

    /// <summary>
    /// Gets a value indicating if a specific name has been supplied for this foreign key constraint.
    /// </summary>
    public bool HasDefaultName => string.Equals(this.Name, this.DefaultName, StringComparison.Ordinal);

    /// <summary>
    /// Gets or sets the name of this foreign key constraint.
    /// If no name is supplied, a default name will be calculated.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    public string Name
    {
      get => this._name ?? this.DefaultName;
      set => this._name = value;
    }

    internal string DefaultName => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "FK_{0}_{1}_{2}", (object) this.DependentTable, (object) this.PrincipalTable, (object) this.DependentColumns.Join<string>(separator: "_")).RestrictTo(128);
  }
}
