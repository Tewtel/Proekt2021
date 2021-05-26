// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Infrastructure.IDbMigration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Migrations.Model;

namespace System.Data.Entity.Migrations.Infrastructure
{
  /// <summary>
  /// Explicitly implemented by <see cref="T:System.Data.Entity.Migrations.DbMigration" /> to prevent certain members from showing up
  /// in the IntelliSense of scaffolded migrations.
  /// </summary>
  public interface IDbMigration
  {
    /// <summary>
    /// Adds a custom <see cref="T:System.Data.Entity.Migrations.Model.MigrationOperation" /> to the migration.
    /// Custom operation implementors are encouraged to create extension methods on
    /// <see cref="T:System.Data.Entity.Migrations.Infrastructure.IDbMigration" /> that provide a fluent-style API for adding new operations.
    /// </summary>
    /// <param name="migrationOperation"> The operation to add. </param>
    void AddOperation(MigrationOperation migrationOperation);
  }
}
