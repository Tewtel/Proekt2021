// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.EntityWithChangeTrackerStrategy
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects.DataClasses;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal sealed class EntityWithChangeTrackerStrategy : IChangeTrackingStrategy
  {
    private readonly IEntityWithChangeTracker _entity;

    public EntityWithChangeTrackerStrategy(IEntityWithChangeTracker entity) => this._entity = entity;

    public void SetChangeTracker(IEntityChangeTracker changeTracker) => this._entity.SetChangeTracker(changeTracker);

    public void TakeSnapshot(EntityEntry entry)
    {
      if (entry == null || !entry.RequiresComplexChangeTracking)
        return;
      entry.TakeSnapshot(true);
    }

    public void SetCurrentValue(
      EntityEntry entry,
      StateManagerMemberMetadata member,
      int ordinal,
      object target,
      object value)
    {
      member.SetValue(target, value);
    }

    public void UpdateCurrentValueRecord(object value, EntityEntry entry)
    {
      int num = entry.WrappedEntity.IdentityType != this._entity.GetType() ? 1 : 0;
      entry.UpdateRecordWithoutSetModified(value, (DbUpdatableDataRecord) entry.CurrentValues);
      if (num == 0)
        return;
      entry.DetectChangesInProperties(true);
    }
  }
}
