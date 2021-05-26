// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.NullEntityWrapper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal class NullEntityWrapper : IEntityWrapper
  {
    private static readonly IEntityWrapper _nullWrapper = (IEntityWrapper) new NullEntityWrapper();

    private NullEntityWrapper()
    {
    }

    internal static IEntityWrapper NullWrapper => NullEntityWrapper._nullWrapper;

    public RelationshipManager RelationshipManager => (RelationshipManager) null;

    public bool OwnsRelationshipManager => false;

    public object Entity => (object) null;

    public EntityEntry ObjectStateEntry
    {
      get => (EntityEntry) null;
      set
      {
      }
    }

    public void CollectionAdd(RelatedEnd relatedEnd, object value)
    {
    }

    public bool CollectionRemove(RelatedEnd relatedEnd, object value) => false;

    public EntityKey EntityKey
    {
      get => (EntityKey) null;
      set
      {
      }
    }

    public EntityKey GetEntityKeyFromEntity() => (EntityKey) null;

    public ObjectContext Context
    {
      get => (ObjectContext) null;
      set
      {
      }
    }

    public MergeOption MergeOption => MergeOption.NoTracking;

    public void AttachContext(ObjectContext context, EntitySet entitySet, MergeOption mergeOption)
    {
    }

    public void ResetContext(ObjectContext context, EntitySet entitySet, MergeOption mergeOption)
    {
    }

    public void DetachContext()
    {
    }

    public void SetChangeTracker(IEntityChangeTracker changeTracker)
    {
    }

    public void TakeSnapshot(EntityEntry entry)
    {
    }

    public void TakeSnapshotOfRelationships(EntityEntry entry)
    {
    }

    public Type IdentityType => (Type) null;

    public void EnsureCollectionNotNull(RelatedEnd relatedEnd)
    {
    }

    public object GetNavigationPropertyValue(RelatedEnd relatedEnd) => (object) null;

    public void SetNavigationPropertyValue(RelatedEnd relatedEnd, object value)
    {
    }

    public void RemoveNavigationPropertyValue(RelatedEnd relatedEnd, object value)
    {
    }

    public void SetCurrentValue(
      EntityEntry entry,
      StateManagerMemberMetadata member,
      int ordinal,
      object target,
      object value)
    {
    }

    public bool InitializingProxyRelatedEnds
    {
      get => false;
      set
      {
      }
    }

    public void UpdateCurrentValueRecord(object value, EntityEntry entry)
    {
    }

    public bool RequiresRelationshipChangeTracking => false;

    public bool OverridesEqualsOrGetHashCode => false;
  }
}
