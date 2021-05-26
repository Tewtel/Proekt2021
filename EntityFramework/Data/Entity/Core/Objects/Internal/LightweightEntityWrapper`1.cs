// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.LightweightEntityWrapper`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal sealed class LightweightEntityWrapper<TEntity> : BaseEntityWrapper<TEntity>
    where TEntity : class, IEntityWithRelationships, IEntityWithKey, IEntityWithChangeTracker
  {
    private readonly TEntity _entity;

    internal LightweightEntityWrapper(TEntity entity, bool overridesEquals)
      : base(entity, entity.RelationshipManager, overridesEquals)
    {
      this._entity = entity;
    }

    internal LightweightEntityWrapper(
      TEntity entity,
      EntityKey key,
      EntitySet entitySet,
      ObjectContext context,
      MergeOption mergeOption,
      Type identityType,
      bool overridesEquals)
      : base(entity, entity.RelationshipManager, entitySet, context, mergeOption, identityType, overridesEquals)
    {
      this._entity = entity;
      this._entity.EntityKey = key;
    }

    public override void SetChangeTracker(IEntityChangeTracker changeTracker) => this._entity.SetChangeTracker(changeTracker);

    public override void TakeSnapshot(EntityEntry entry)
    {
    }

    public override void TakeSnapshotOfRelationships(EntityEntry entry)
    {
    }

    public override EntityKey EntityKey
    {
      get => this._entity.EntityKey;
      set => this._entity.EntityKey = value;
    }

    public override bool OwnsRelationshipManager => true;

    public override EntityKey GetEntityKeyFromEntity() => this._entity.EntityKey;

    public override void CollectionAdd(RelatedEnd relatedEnd, object value)
    {
    }

    public override bool CollectionRemove(RelatedEnd relatedEnd, object value) => false;

    public override void SetNavigationPropertyValue(RelatedEnd relatedEnd, object value)
    {
    }

    public override void RemoveNavigationPropertyValue(RelatedEnd relatedEnd, object value)
    {
    }

    public override void EnsureCollectionNotNull(RelatedEnd relatedEnd)
    {
    }

    public override object GetNavigationPropertyValue(RelatedEnd relatedEnd) => (object) null;

    public override object Entity => (object) this._entity;

    public override TEntity TypedEntity => this._entity;

    public override void SetCurrentValue(
      EntityEntry entry,
      StateManagerMemberMetadata member,
      int ordinal,
      object target,
      object value)
    {
      member.SetValue(target, value);
    }

    public override void UpdateCurrentValueRecord(object value, EntityEntry entry) => entry.UpdateRecordWithoutSetModified(value, (DbUpdatableDataRecord) entry.CurrentValues);

    public override bool RequiresRelationshipChangeTracking => false;
  }
}
