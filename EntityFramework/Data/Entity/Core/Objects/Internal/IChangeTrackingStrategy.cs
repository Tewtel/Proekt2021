// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.IChangeTrackingStrategy
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects.DataClasses;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal interface IChangeTrackingStrategy
  {
    void SetChangeTracker(IEntityChangeTracker changeTracker);

    void TakeSnapshot(EntityEntry entry);

    void SetCurrentValue(
      EntityEntry entry,
      StateManagerMemberMetadata member,
      int ordinal,
      object target,
      object value);

    void UpdateCurrentValueRecord(object value, EntityEntry entry);
  }
}
