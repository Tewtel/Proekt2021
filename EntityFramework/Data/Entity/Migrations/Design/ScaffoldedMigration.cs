// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Design.ScaffoldedMigration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Migrations.Design
{
  /// <summary>
  /// Represents a code-based migration that has been scaffolded and is ready to be written to a file.
  /// </summary>
  [Serializable]
  public class ScaffoldedMigration
  {
    private string _migrationId;
    private string _userCode;
    private string _designerCode;
    private string _language;
    private string _directory;
    private readonly Dictionary<string, object> _resources = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the unique identifier for this migration.
    /// Typically used for the file name of the generated code.
    /// </summary>
    public string MigrationId
    {
      get => this._migrationId;
      set
      {
        System.Data.Entity.Utilities.Check.NotEmpty(value, nameof (value));
        this._migrationId = value;
      }
    }

    /// <summary>
    /// Gets or sets the scaffolded migration code that the user can edit.
    /// </summary>
    public string UserCode
    {
      get => this._userCode;
      set
      {
        System.Data.Entity.Utilities.Check.NotEmpty(value, nameof (value));
        this._userCode = value;
      }
    }

    /// <summary>
    /// Gets or sets the scaffolded migration code that should be stored in a code behind file.
    /// </summary>
    public string DesignerCode
    {
      get => this._designerCode;
      set
      {
        System.Data.Entity.Utilities.Check.NotEmpty(value, nameof (value));
        this._designerCode = value;
      }
    }

    /// <summary>
    /// Gets or sets the programming language used for this migration.
    /// Typically used for the file extension of the generated code.
    /// </summary>
    public string Language
    {
      get => this._language;
      set
      {
        System.Data.Entity.Utilities.Check.NotEmpty(value, nameof (value));
        this._language = value;
      }
    }

    /// <summary>
    /// Gets or sets the subdirectory in the user's project that this migration should be saved in.
    /// </summary>
    public string Directory
    {
      get => this._directory;
      set
      {
        System.Data.Entity.Utilities.Check.NotEmpty(value, nameof (value));
        this._directory = value;
      }
    }

    /// <summary>
    /// Gets a dictionary of string resources to add to the migration resource file.
    /// </summary>
    public IDictionary<string, object> Resources => (IDictionary<string, object>) this._resources;

    /// <summary>Gets or sets whether the migration was re-scaffolded.</summary>
    public bool IsRescaffold { get; set; }
  }
}
