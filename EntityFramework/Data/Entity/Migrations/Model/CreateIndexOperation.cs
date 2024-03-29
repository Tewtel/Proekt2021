﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.CreateIndexOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Utilities;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents creating a database index.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class CreateIndexOperation : IndexOperation
  {
    /// <summary>
    /// Initializes a new instance of the CreateIndexOperation class.
    /// The Table and Columns properties should also be populated.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public CreateIndexOperation(object anonymousArguments = null)
      : base(anonymousArguments)
    {
    }

    /// <summary>
    /// Gets or sets a value indicating if this is a unique index.
    /// </summary>
    public bool IsUnique { get; set; }

    /// <summary>Gets an operation to drop this index.</summary>
    public override MigrationOperation Inverse
    {
      get
      {
        DropIndexOperation dropIndexOperation1 = new DropIndexOperation(this, (object) null);
        dropIndexOperation1.Name = this.Name;
        dropIndexOperation1.Table = this.Table;
        DropIndexOperation dropIndexOperation = dropIndexOperation1;
        this.Columns.Each<string>((Action<string>) (c => dropIndexOperation.Columns.Add(c)));
        return (MigrationOperation) dropIndexOperation;
      }
    }

    /// <inheritdoc />
    public override bool IsDestructiveChange => false;

    /// <summary>Gets or sets whether this is a clustered index.</summary>
    public bool IsClustered { get; set; }
  }
}
