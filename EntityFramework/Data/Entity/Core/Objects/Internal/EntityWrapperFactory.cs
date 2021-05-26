// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.EntityWrapperFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;
using System.Reflection;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal class EntityWrapperFactory
  {
    private static readonly Memoizer<Type, Func<object, IEntityWrapper>> _delegateCache = new Memoizer<Type, Func<object, IEntityWrapper>>(new Func<Type, Func<object, IEntityWrapper>>(EntityWrapperFactory.CreateWrapperDelegate), (IEqualityComparer<Type>) null);
    internal static readonly MethodInfo CreateWrapperDelegateTypedLightweightMethod = typeof (EntityWrapperFactory).GetOnlyDeclaredMethod("CreateWrapperDelegateTypedLightweight");
    internal static readonly MethodInfo CreateWrapperDelegateTypedWithRelationshipsMethod = typeof (EntityWrapperFactory).GetOnlyDeclaredMethod("CreateWrapperDelegateTypedWithRelationships");
    internal static readonly MethodInfo CreateWrapperDelegateTypedWithoutRelationshipsMethod = typeof (EntityWrapperFactory).GetOnlyDeclaredMethod("CreateWrapperDelegateTypedWithoutRelationships");

    internal static IEntityWrapper CreateNewWrapper(object entity, EntityKey key)
    {
      if (entity == null)
        return NullEntityWrapper.NullWrapper;
      IEntityWrapper entityWrapper = EntityWrapperFactory._delegateCache.Evaluate(entity.GetType())(entity);
      entityWrapper.RelationshipManager.SetWrappedOwner(entityWrapper, entity);
      if ((object) key != null && (object) entityWrapper.EntityKey == null)
        entityWrapper.EntityKey = key;
      EntityProxyTypeInfo proxyTypeInfo;
      if (EntityProxyFactory.TryGetProxyType(entity.GetType(), out proxyTypeInfo))
        proxyTypeInfo.SetEntityWrapper(entityWrapper);
      return entityWrapper;
    }

    private static Func<object, IEntityWrapper> CreateWrapperDelegate(
      Type entityType)
    {
      bool flag1 = typeof (IEntityWithRelationships).IsAssignableFrom(entityType);
      bool flag2 = typeof (IEntityWithChangeTracker).IsAssignableFrom(entityType);
      bool flag3 = typeof (IEntityWithKey).IsAssignableFrom(entityType);
      bool flag4 = EntityProxyFactory.IsProxyType(entityType);
      return (Func<object, IEntityWrapper>) (!(flag1 & flag2 & flag3) || flag4 ? (!flag1 ? EntityWrapperFactory.CreateWrapperDelegateTypedWithoutRelationshipsMethod : EntityWrapperFactory.CreateWrapperDelegateTypedWithRelationshipsMethod) : EntityWrapperFactory.CreateWrapperDelegateTypedLightweightMethod).MakeGenericMethod(entityType).Invoke((object) null, new object[0]);
    }

    private static Func<object, IEntityWrapper> CreateWrapperDelegateTypedLightweight<TEntity>() where TEntity : class, IEntityWithRelationships, IEntityWithKey, IEntityWithChangeTracker
    {
      bool overridesEquals = typeof (TEntity).OverridesEqualsOrGetHashCode();
      return (Func<object, IEntityWrapper>) (entity => (IEntityWrapper) new LightweightEntityWrapper<TEntity>((TEntity) entity, overridesEquals));
    }

    private static Func<object, IEntityWrapper> CreateWrapperDelegateTypedWithRelationships<TEntity>() where TEntity : class, IEntityWithRelationships
    {
      bool overridesEquals = typeof (TEntity).OverridesEqualsOrGetHashCode();
      Func<object, IPropertyAccessorStrategy> propertyAccessorStrategy;
      Func<object, IChangeTrackingStrategy> changeTrackingStrategy;
      Func<object, IEntityKeyStrategy> keyStrategy;
      EntityWrapperFactory.CreateStrategies<TEntity>(out propertyAccessorStrategy, out changeTrackingStrategy, out keyStrategy);
      return (Func<object, IEntityWrapper>) (entity => (IEntityWrapper) new EntityWrapperWithRelationships<TEntity>((TEntity) entity, propertyAccessorStrategy, changeTrackingStrategy, keyStrategy, overridesEquals));
    }

    private static Func<object, IEntityWrapper> CreateWrapperDelegateTypedWithoutRelationships<TEntity>() where TEntity : class
    {
      bool overridesEquals = typeof (TEntity).OverridesEqualsOrGetHashCode();
      Func<object, IPropertyAccessorStrategy> propertyAccessorStrategy;
      Func<object, IChangeTrackingStrategy> changeTrackingStrategy;
      Func<object, IEntityKeyStrategy> keyStrategy;
      EntityWrapperFactory.CreateStrategies<TEntity>(out propertyAccessorStrategy, out changeTrackingStrategy, out keyStrategy);
      return (Func<object, IEntityWrapper>) (entity => (IEntityWrapper) new EntityWrapperWithoutRelationships<TEntity>((TEntity) entity, propertyAccessorStrategy, changeTrackingStrategy, keyStrategy, overridesEquals));
    }

    private static void CreateStrategies<TEntity>(
      out Func<object, IPropertyAccessorStrategy> createPropertyAccessorStrategy,
      out Func<object, IChangeTrackingStrategy> createChangeTrackingStrategy,
      out Func<object, IEntityKeyStrategy> createKeyStrategy)
    {
      Type type = typeof (TEntity);
      int num = typeof (IEntityWithRelationships).IsAssignableFrom(type) ? 1 : 0;
      bool flag1 = typeof (IEntityWithChangeTracker).IsAssignableFrom(type);
      bool flag2 = typeof (IEntityWithKey).IsAssignableFrom(type);
      bool flag3 = EntityProxyFactory.IsProxyType(type);
      createPropertyAccessorStrategy = !(num == 0 | flag3) ? EntityWrapperFactory.GetNullPropertyAccessorStrategyFunc() : EntityWrapperFactory.GetPocoPropertyAccessorStrategyFunc();
      createChangeTrackingStrategy = !flag1 ? EntityWrapperFactory.GetSnapshotChangeTrackingStrategyFunc() : EntityWrapperFactory.GetEntityWithChangeTrackerStrategyFunc();
      if (flag2)
        createKeyStrategy = EntityWrapperFactory.GetEntityWithKeyStrategyStrategyFunc();
      else
        createKeyStrategy = EntityWrapperFactory.GetPocoEntityKeyStrategyFunc();
    }

    internal IEntityWrapper WrapEntityUsingContext(
      object entity,
      ObjectContext context)
    {
      return this.WrapEntityUsingStateManagerGettingEntry(entity, context == null ? (ObjectStateManager) null : context.ObjectStateManager, out EntityEntry _);
    }

    internal IEntityWrapper WrapEntityUsingContextGettingEntry(
      object entity,
      ObjectContext context,
      out EntityEntry existingEntry)
    {
      return this.WrapEntityUsingStateManagerGettingEntry(entity, context == null ? (ObjectStateManager) null : context.ObjectStateManager, out existingEntry);
    }

    internal IEntityWrapper WrapEntityUsingStateManager(
      object entity,
      ObjectStateManager stateManager)
    {
      return this.WrapEntityUsingStateManagerGettingEntry(entity, stateManager, out EntityEntry _);
    }

    internal virtual IEntityWrapper WrapEntityUsingStateManagerGettingEntry(
      object entity,
      ObjectStateManager stateManager,
      out EntityEntry existingEntry)
    {
      IEntityWrapper wrapper = (IEntityWrapper) null;
      existingEntry = (EntityEntry) null;
      if (entity == null)
        return NullEntityWrapper.NullWrapper;
      if (stateManager != null)
      {
        existingEntry = stateManager.FindEntityEntry(entity);
        if (existingEntry != null)
          return existingEntry.WrappedEntity;
        if (stateManager.TransactionManager.TrackProcessedEntities && stateManager.TransactionManager.WrappedEntities.TryGetValue(entity, out wrapper))
          return wrapper;
      }
      if (entity is IEntityWithRelationships withRelationships)
      {
        IEntityWrapper wrappedOwner = (withRelationships.RelationshipManager ?? throw new InvalidOperationException(Strings.RelationshipManager_UnexpectedNull)).WrappedOwner;
        if (wrappedOwner.Entity == entity)
          return wrappedOwner;
        throw new InvalidOperationException(Strings.RelationshipManager_InvalidRelationshipManagerOwner);
      }
      EntityProxyFactory.TryGetProxyWrapper(entity, out wrapper);
      if (wrapper == null)
      {
        IEntityWithKey entityWithKey = entity as IEntityWithKey;
        wrapper = EntityWrapperFactory.CreateNewWrapper(entity, entityWithKey == null ? (EntityKey) null : entityWithKey.EntityKey);
      }
      if (stateManager != null && stateManager.TransactionManager.TrackProcessedEntities)
        stateManager.TransactionManager.WrappedEntities.Add(entity, wrapper);
      return wrapper;
    }

    internal virtual void UpdateNoTrackingWrapper(
      IEntityWrapper wrapper,
      ObjectContext context,
      EntitySet entitySet)
    {
      if (wrapper.EntityKey == (EntityKey) null)
        wrapper.EntityKey = context.ObjectStateManager.CreateEntityKey(entitySet, wrapper.Entity);
      if (wrapper.Context != null)
        return;
      wrapper.AttachContext(context, entitySet, MergeOption.NoTracking);
    }

    internal static Func<object, IPropertyAccessorStrategy> GetPocoPropertyAccessorStrategyFunc() => (Func<object, IPropertyAccessorStrategy>) (entity => (IPropertyAccessorStrategy) new PocoPropertyAccessorStrategy(entity));

    internal static Func<object, IPropertyAccessorStrategy> GetNullPropertyAccessorStrategyFunc() => (Func<object, IPropertyAccessorStrategy>) (entity => (IPropertyAccessorStrategy) null);

    internal static Func<object, IChangeTrackingStrategy> GetEntityWithChangeTrackerStrategyFunc() => (Func<object, IChangeTrackingStrategy>) (entity => (IChangeTrackingStrategy) new EntityWithChangeTrackerStrategy((IEntityWithChangeTracker) entity));

    internal static Func<object, IChangeTrackingStrategy> GetSnapshotChangeTrackingStrategyFunc() => (Func<object, IChangeTrackingStrategy>) (entity => (IChangeTrackingStrategy) SnapshotChangeTrackingStrategy.Instance);

    internal static Func<object, IEntityKeyStrategy> GetEntityWithKeyStrategyStrategyFunc() => (Func<object, IEntityKeyStrategy>) (entity => (IEntityKeyStrategy) new EntityWithKeyStrategy((IEntityWithKey) entity));

    internal static Func<object, IEntityKeyStrategy> GetPocoEntityKeyStrategyFunc() => (Func<object, IEntityKeyStrategy>) (entity => (IEntityKeyStrategy) new PocoEntityKeyStrategy());
  }
}
