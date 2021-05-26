// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Infrastructure.IMigrationMetadata
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Migrations.Infrastructure
{
  /// <summary>
  /// Provides additional metadata about a code-based migration.
  /// </summary>
  public interface IMigrationMetadata
  {
    /// <summary>Gets the unique identifier for the migration.</summary>
    string Id { get; }

    /// <summary>
    /// Gets the state of the model before this migration is run.
    /// </summary>
    string Source { get; }

    /// <summary>
    /// Gets the state of the model after this migration is run.
    /// </summary>
    string Target { get; }
  }
}
