// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataClasses.IEntityChangeTracker
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects.DataClasses
{
  /// <summary>
  /// This interface is implemented by a change tracker and is used by data classes to report changes
  /// </summary>
  public interface IEntityChangeTracker
  {
    /// <summary>Notifies the change tracker of a pending change to a property of an entity type.</summary>
    /// <param name="entityMemberName">The name of the property that is changing.</param>
    void EntityMemberChanging(string entityMemberName);

    /// <summary>Notifies the change tracker that a property of an entity type has changed.</summary>
    /// <param name="entityMemberName">The name of the property that has changed.</param>
    void EntityMemberChanged(string entityMemberName);

    /// <summary>Notifies the change tracker of a pending change to a complex property.</summary>
    /// <param name="entityMemberName">The name of the top-level entity property that is changing.</param>
    /// <param name="complexObject">The complex type that contains the property that is changing.</param>
    /// <param name="complexObjectMemberName">The name of the property that is changing on complex type.</param>
    void EntityComplexMemberChanging(
      string entityMemberName,
      object complexObject,
      string complexObjectMemberName);

    /// <summary>Notifies the change tracker that a property of a complex type has changed.</summary>
    /// <param name="entityMemberName">The name of the complex property of the entity type that has changed.</param>
    /// <param name="complexObject">The complex type that contains the property that changed.</param>
    /// <param name="complexObjectMemberName">The name of the property that changed on complex type.</param>
    void EntityComplexMemberChanged(
      string entityMemberName,
      object complexObject,
      string complexObjectMemberName);

    /// <summary>Gets current state of a tracked object.</summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.EntityState" /> that is the state of the tracked object.For more information, see Identity Resolution, State Management, and Change Tracking and Tracking Changes in POCO Entities.
    /// </returns>
    EntityState EntityState { get; }
  }
}
