// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataClasses.IEntityWithChangeTracker
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects.DataClasses
{
  /// <summary>
  /// Minimum interface that a data class must implement in order to be managed by a change tracker.
  /// </summary>
  public interface IEntityWithChangeTracker
  {
    /// <summary>
    /// Gets or sets the <see cref="T:System.Data.Entity.Core.Objects.DataClasses.IEntityChangeTracker" /> used to report changes.
    /// </summary>
    /// <param name="changeTracker">
    /// The <see cref="T:System.Data.Entity.Core.Objects.DataClasses.IEntityChangeTracker" /> used to report changes.
    /// </param>
    void SetChangeTracker(IEntityChangeTracker changeTracker);
  }
}
