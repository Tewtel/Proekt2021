﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.RenameTableOperation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Utilities;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents renaming an existing table.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class RenameTableOperation : MigrationOperation
  {
    private readonly string _name;
    private string _newName;

    /// <summary>
    /// Initializes a new instance of the RenameTableOperation class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="name"> Name of the table to be renamed. </param>
    /// <param name="newName"> New name for the table. </param>
    /// <param name="anonymousArguments"> Additional arguments that may be processed by providers. Use anonymous type syntax to specify arguments e.g. 'new { SampleArgument = "MyValue" }'. </param>
    public RenameTableOperation(string name, string newName, object anonymousArguments = null)
      : base(anonymousArguments)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      System.Data.Entity.Utilities.Check.NotEmpty(newName, nameof (newName));
      this._name = name;
      this._newName = newName;
    }

    /// <summary>Gets the name of the table to be renamed.</summary>
    public virtual string Name => this._name;

    /// <summary>Gets the new name for the table.</summary>
    public virtual string NewName
    {
      get => this._newName;
      internal set => this._newName = value;
    }

    /// <summary>Gets an operation that reverts the rename.</summary>
    public override MigrationOperation Inverse
    {
      get
      {
        DatabaseName databaseName = DatabaseName.Parse(this._name);
        return (MigrationOperation) new RenameTableOperation(new DatabaseName(DatabaseName.Parse(this._newName).Name, databaseName.Schema).ToString(), databaseName.Name);
      }
    }

    /// <inheritdoc />
    public override bool IsDestructiveChange => false;
  }
}
