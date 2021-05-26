// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.PocoPropertyAccessorStrategy
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Utilities;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal sealed class PocoPropertyAccessorStrategy : IPropertyAccessorStrategy
  {
    internal static readonly MethodInfo AddToCollectionGeneric = typeof (PocoPropertyAccessorStrategy).GetOnlyDeclaredMethod("AddToCollection");
    internal static readonly MethodInfo RemoveFromCollectionGeneric = typeof (PocoPropertyAccessorStrategy).GetOnlyDeclaredMethod("RemoveFromCollection");
    private readonly object _entity;

    public PocoPropertyAccessorStrategy(object entity) => this._entity = entity;

    public object GetNavigationPropertyValue(RelatedEnd relatedEnd)
    {
      object obj = (object) null;
      if (relatedEnd != null)
      {
        if (relatedEnd.TargetAccessor.ValueGetter == null)
        {
          Type declaringType = PocoPropertyAccessorStrategy.GetDeclaringType(relatedEnd);
          PropertyInfo topProperty = declaringType.GetTopProperty(relatedEnd.TargetAccessor.PropertyName);
          if (topProperty == (PropertyInfo) null)
            throw new EntityException(System.Data.Entity.Resources.Strings.PocoEntityWrapper_UnableToSetFieldOrProperty((object) relatedEnd.TargetAccessor.PropertyName, (object) declaringType.FullName));
          EntityProxyFactory entityProxyFactory = new EntityProxyFactory();
          relatedEnd.TargetAccessor.ValueGetter = entityProxyFactory.CreateBaseGetter(topProperty.DeclaringType, topProperty);
        }
        bool state = relatedEnd.DisableLazyLoading();
        try
        {
          obj = relatedEnd.TargetAccessor.ValueGetter(this._entity);
        }
        catch (Exception ex)
        {
          throw new EntityException(System.Data.Entity.Resources.Strings.PocoEntityWrapper_UnableToSetFieldOrProperty((object) relatedEnd.TargetAccessor.PropertyName, (object) this._entity.GetType().FullName), ex);
        }
        finally
        {
          relatedEnd.ResetLazyLoading(state);
        }
      }
      return obj;
    }

    public void SetNavigationPropertyValue(RelatedEnd relatedEnd, object value)
    {
      if (relatedEnd == null)
        return;
      if (relatedEnd.TargetAccessor.ValueSetter == null)
      {
        Type declaringType = PocoPropertyAccessorStrategy.GetDeclaringType(relatedEnd);
        PropertyInfo topProperty = declaringType.GetTopProperty(relatedEnd.TargetAccessor.PropertyName);
        if (topProperty == (PropertyInfo) null)
          throw new EntityException(System.Data.Entity.Resources.Strings.PocoEntityWrapper_UnableToSetFieldOrProperty((object) relatedEnd.TargetAccessor.PropertyName, (object) declaringType.FullName));
        EntityProxyFactory entityProxyFactory = new EntityProxyFactory();
        relatedEnd.TargetAccessor.ValueSetter = entityProxyFactory.CreateBaseSetter(topProperty.DeclaringType, topProperty);
      }
      try
      {
        relatedEnd.TargetAccessor.ValueSetter(this._entity, value);
      }
      catch (Exception ex)
      {
        throw new EntityException(System.Data.Entity.Resources.Strings.PocoEntityWrapper_UnableToSetFieldOrProperty((object) relatedEnd.TargetAccessor.PropertyName, (object) this._entity.GetType().FullName), ex);
      }
    }

    private static Type GetDeclaringType(RelatedEnd relatedEnd) => relatedEnd.NavigationProperty != null ? Util.GetObjectMapping((EdmType) relatedEnd.NavigationProperty.DeclaringType, relatedEnd.WrappedOwner.Context.MetadataWorkspace).ClrType.ClrType : relatedEnd.WrappedOwner.IdentityType;

    private static Type GetNavigationPropertyType(Type entityType, string propertyName)
    {
      PropertyInfo topProperty = entityType.GetTopProperty(propertyName);
      if (topProperty != (PropertyInfo) null)
        return topProperty.PropertyType;
      FieldInfo field = entityType.GetField(propertyName);
      return field != (FieldInfo) null ? field.FieldType : throw new EntityException(System.Data.Entity.Resources.Strings.PocoEntityWrapper_UnableToSetFieldOrProperty((object) propertyName, (object) entityType.FullName));
    }

    public void CollectionAdd(RelatedEnd relatedEnd, object value)
    {
      object entity = this._entity;
      try
      {
        object navigationPropertyValue = this.GetNavigationPropertyValue(relatedEnd);
        if (navigationPropertyValue == null)
        {
          navigationPropertyValue = this.CollectionCreate(relatedEnd);
          this.SetNavigationPropertyValue(relatedEnd, navigationPropertyValue);
        }
        if (navigationPropertyValue == relatedEnd)
          return;
        if (relatedEnd.TargetAccessor.CollectionAdd == null)
          relatedEnd.TargetAccessor.CollectionAdd = PocoPropertyAccessorStrategy.CreateCollectionAddFunction(PocoPropertyAccessorStrategy.GetDeclaringType(relatedEnd), relatedEnd.TargetAccessor.PropertyName);
        relatedEnd.TargetAccessor.CollectionAdd(navigationPropertyValue, value);
      }
      catch (Exception ex)
      {
        throw new EntityException(System.Data.Entity.Resources.Strings.PocoEntityWrapper_UnableToSetFieldOrProperty((object) relatedEnd.TargetAccessor.PropertyName, (object) entity.GetType().FullName), ex);
      }
    }

    private static Action<object, object> CreateCollectionAddFunction(
      Type type,
      string propertyName)
    {
      Type collectionElementType = EntityUtil.GetCollectionElementType(PocoPropertyAccessorStrategy.GetNavigationPropertyType(type, propertyName));
      return (Action<object, object>) PocoPropertyAccessorStrategy.AddToCollectionGeneric.MakeGenericMethod(collectionElementType).Invoke((object) null, (object[]) null);
    }

    private static Action<object, object> AddToCollection<T>() => (Action<object, object>) ((collectionArg, item) =>
    {
      ICollection<T> objs = (ICollection<T>) collectionArg;
      if (objs is Array array2 && array2.IsFixedSize)
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.RelatedEnd_CannotAddToFixedSizeArray((object) array2.GetType()));
      objs.Add((T) item);
    });

    public bool CollectionRemove(RelatedEnd relatedEnd, object value)
    {
      object entity = this._entity;
      try
      {
        object navigationPropertyValue = this.GetNavigationPropertyValue(relatedEnd);
        if (navigationPropertyValue != null)
        {
          if (navigationPropertyValue == relatedEnd)
            return true;
          if (relatedEnd.TargetAccessor.CollectionRemove == null)
            relatedEnd.TargetAccessor.CollectionRemove = PocoPropertyAccessorStrategy.CreateCollectionRemoveFunction(PocoPropertyAccessorStrategy.GetDeclaringType(relatedEnd), relatedEnd.TargetAccessor.PropertyName);
          return relatedEnd.TargetAccessor.CollectionRemove(navigationPropertyValue, value);
        }
      }
      catch (Exception ex)
      {
        throw new EntityException(System.Data.Entity.Resources.Strings.PocoEntityWrapper_UnableToSetFieldOrProperty((object) relatedEnd.TargetAccessor.PropertyName, (object) entity.GetType().FullName), ex);
      }
      return false;
    }

    private static Func<object, object, bool> CreateCollectionRemoveFunction(
      Type type,
      string propertyName)
    {
      Type collectionElementType = EntityUtil.GetCollectionElementType(PocoPropertyAccessorStrategy.GetNavigationPropertyType(type, propertyName));
      return (Func<object, object, bool>) PocoPropertyAccessorStrategy.RemoveFromCollectionGeneric.MakeGenericMethod(collectionElementType).Invoke((object) null, (object[]) null);
    }

    private static Func<object, object, bool> RemoveFromCollection<T>() => (Func<object, object, bool>) ((collectionArg, item) =>
    {
      ICollection<T> objs = (ICollection<T>) collectionArg;
      if (objs is Array array2 && array2.IsFixedSize)
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.RelatedEnd_CannotRemoveFromFixedSizeArray((object) array2.GetType()));
      return objs.Remove((T) item);
    });

    public object CollectionCreate(RelatedEnd relatedEnd)
    {
      if (this._entity is IEntityWithRelationships)
        return (object) relatedEnd;
      if (relatedEnd.TargetAccessor.CollectionCreate == null)
      {
        Type declaringType = PocoPropertyAccessorStrategy.GetDeclaringType(relatedEnd);
        string propertyName1 = relatedEnd.TargetAccessor.PropertyName;
        string propertyName2 = propertyName1;
        Type navigationPropertyType = PocoPropertyAccessorStrategy.GetNavigationPropertyType(declaringType, propertyName2);
        relatedEnd.TargetAccessor.CollectionCreate = PocoPropertyAccessorStrategy.CreateCollectionCreateDelegate(navigationPropertyType, propertyName1);
      }
      return relatedEnd.TargetAccessor.CollectionCreate();
    }

    private static Func<object> CreateCollectionCreateDelegate(
      Type navigationPropertyType,
      string propName)
    {
      Type collectionType = EntityUtil.DetermineCollectionType(navigationPropertyType);
      return !(collectionType == (Type) null) ? Expression.Lambda<Func<object>>((Expression) DelegateFactory.GetNewExpressionForCollectionType(collectionType)).Compile() : throw new EntityException(System.Data.Entity.Resources.Strings.PocoEntityWrapper_UnableToMaterializeArbitaryNavPropType((object) propName, (object) navigationPropertyType));
    }
  }
}
