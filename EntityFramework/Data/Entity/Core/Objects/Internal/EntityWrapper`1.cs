// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.EntityWrapper`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal abstract class EntityWrapper<TEntity> : BaseEntityWrapper<TEntity> where TEntity : class
  {
    private readonly TEntity _entity;
    private readonly IPropertyAccessorStrategy _propertyStrategy;
    private readonly IChangeTrackingStrategy _changeTrackingStrategy;
    private readonly IEntityKeyStrategy _keyStrategy;

    protected EntityWrapper(
      TEntity entity,
      RelationshipManager relationshipManager,
      Func<object, IPropertyAccessorStrategy> propertyStrategy,
      Func<object, IChangeTrackingStrategy> changeTrackingStrategy,
      Func<object, IEntityKeyStrategy> keyStrategy,
      bool overridesEquals)
      : base(entity, relationshipManager, overridesEquals)
    {
      if (relationshipManager == null)
        throw new InvalidOperationException(Strings.RelationshipManager_UnexpectedNull);
      this._entity = entity;
      this._propertyStrategy = propertyStrategy((object) entity);
      this._changeTrackingStrategy = changeTrackingStrategy((object) entity);
      this._keyStrategy = keyStrategy((object) entity);
    }

    protected EntityWrapper(
      TEntity entity,
      RelationshipManager relationshipManager,
      EntityKey key,
      EntitySet set,
      ObjectContext context,
      MergeOption mergeOption,
      Type identityType,
      Func<object, IPropertyAccessorStrategy> propertyStrategy,
      Func<object, IChangeTrackingStrategy> changeTrackingStrategy,
      Func<object, IEntityKeyStrategy> keyStrategy,
      bool overridesEquals)
      : base(entity, relationshipManager, set, context, mergeOption, identityType, overridesEquals)
    {
      if (relationshipManager == null)
        throw new InvalidOperationException(Strings.RelationshipManager_UnexpectedNull);
      this._entity = entity;
      this._propertyStrategy = propertyStrategy((object) entity);
      this._changeTrackingStrategy = changeTrackingStrategy((object) entity);
      this._keyStrategy = keyStrategy((object) entity);
      this._keyStrategy.SetEntityKey(key);
    }

    public override void SetChangeTracker(IEntityChangeTracker changeTracker) => this._changeTrackingStrategy.SetChangeTracker(changeTracker);

    public override void TakeSnapshot(EntityEntry entry) => this._changeTrackingStrategy.TakeSnapshot(entry);

    public override EntityKey EntityKey
    {
      get => this._keyStrategy.GetEntityKey();
      set => this._keyStrategy.SetEntityKey(value);
    }

    public override EntityKey GetEntityKeyFromEntity() => this._keyStrategy.GetEntityKeyFromEntity();

    public override void CollectionAdd(RelatedEnd relatedEnd, object value)
    {
      if (this._propertyStrategy == null)
        return;
      this._propertyStrategy.CollectionAdd(relatedEnd, value);
    }

    public override bool CollectionRemove(RelatedEnd relatedEnd, object value) => this._propertyStrategy != null && this._propertyStrategy.CollectionRemove(relatedEnd, value);

    public override void EnsureCollectionNotNull(RelatedEnd relatedEnd)
    {
      if (this._propertyStrategy == null || this._propertyStrategy.GetNavigationPropertyValue(relatedEnd) != null)
        return;
      object obj = this._propertyStrategy.CollectionCreate(relatedEnd);
      this._propertyStrategy.SetNavigationPropertyValue(relatedEnd, obj);
    }

    public override object GetNavigationPropertyValue(RelatedEnd relatedEnd) => this._propertyStrategy == null ? (object) null : this._propertyStrategy.GetNavigationPropertyValue(relatedEnd);

    public override void SetNavigationPropertyValue(RelatedEnd relatedEnd, object value)
    {
      if (this._propertyStrategy == null)
        return;
      this._propertyStrategy.SetNavigationPropertyValue(relatedEnd, value);
    }

    public override void RemoveNavigationPropertyValue(RelatedEnd relatedEnd, object value)
    {
      if (this._propertyStrategy == null || this._propertyStrategy.GetNavigationPropertyValue(relatedEnd) != value)
        return;
      this._propertyStrategy.SetNavigationPropertyValue(relatedEnd, (object) null);
    }

    public override object Entity => (object) this._entity;

    public override TEntity TypedEntity => this._entity;

    public override void SetCurrentValue(
      EntityEntry entry,
      StateManagerMemberMetadata member,
      int ordinal,
      object target,
      object value)
    {
      this._changeTrackingStrategy.SetCurrentValue(entry, member, ordinal, target, value);
    }

    public override void UpdateCurrentValueRecord(object value, EntityEntry entry) => this._changeTrackingStrategy.UpdateCurrentValueRecord(value, entry);
  }
}
