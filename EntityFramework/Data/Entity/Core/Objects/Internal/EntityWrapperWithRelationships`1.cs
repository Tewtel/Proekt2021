// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.EntityWrapperWithRelationships`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal sealed class EntityWrapperWithRelationships<TEntity> : EntityWrapper<TEntity>
    where TEntity : class, IEntityWithRelationships
  {
    internal EntityWrapperWithRelationships(
      TEntity entity,
      EntityKey key,
      EntitySet entitySet,
      ObjectContext context,
      MergeOption mergeOption,
      Type identityType,
      Func<object, IPropertyAccessorStrategy> propertyStrategy,
      Func<object, IChangeTrackingStrategy> changeTrackingStrategy,
      Func<object, IEntityKeyStrategy> keyStrategy,
      bool overridesEquals)
      : base(entity, entity.RelationshipManager, key, entitySet, context, mergeOption, identityType, propertyStrategy, changeTrackingStrategy, keyStrategy, overridesEquals)
    {
    }

    internal EntityWrapperWithRelationships(
      TEntity entity,
      Func<object, IPropertyAccessorStrategy> propertyStrategy,
      Func<object, IChangeTrackingStrategy> changeTrackingStrategy,
      Func<object, IEntityKeyStrategy> keyStrategy,
      bool overridesEquals)
      : base(entity, entity.RelationshipManager, propertyStrategy, changeTrackingStrategy, keyStrategy, overridesEquals)
    {
    }

    public override bool OwnsRelationshipManager => true;

    public override void TakeSnapshotOfRelationships(EntityEntry entry)
    {
    }

    public override bool RequiresRelationshipChangeTracking => false;
  }
}
