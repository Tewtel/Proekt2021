// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.History.HistoryRow
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Migrations.History
{
  /// <summary>
  /// This class is used by Code First Migrations to read and write migration history
  /// from the database.
  /// </summary>
  public class HistoryRow
  {
    /// <summary>
    /// Gets or sets the Id of the migration this row represents.
    /// </summary>
    public string MigrationId { get; set; }

    /// <summary>
    /// Gets or sets a key representing to which context the row applies.
    /// </summary>
    public string ContextKey { get; set; }

    /// <summary>
    /// Gets or sets the state of the model after this migration was applied.
    /// </summary>
    public byte[] Model { get; set; }

    /// <summary>
    /// Gets or sets the version of Entity Framework that created this entry.
    /// </summary>
    public string ProductVersion { get; set; }
  }
}
