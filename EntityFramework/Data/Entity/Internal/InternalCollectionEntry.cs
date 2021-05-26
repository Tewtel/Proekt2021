// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InternalCollectionEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Internal
{
  internal class InternalCollectionEntry : InternalNavigationEntry
  {
    private static readonly ConcurrentDictionary<Type, Func<InternalCollectionEntry, object>> _entryFactories = new ConcurrentDictionary<Type, Func<InternalCollectionEntry, object>>();

    public InternalCollectionEntry(
      InternalEntityEntry internalEntityEntry,
      NavigationEntryMetadata navigationMetadata)
      : base(internalEntityEntry, navigationMetadata)
    {
    }

    protected override object GetNavigationPropertyFromRelatedEnd(object entity) => (object) this.RelatedEnd;

    public override object CurrentValue
    {
      get => base.CurrentValue;
      set
      {
        if (this.Setter != null)
          this.Setter(this.InternalEntityEntry.Entity, value);
        else if (this.InternalEntityEntry.IsDetached || this.RelatedEnd != value)
          throw Error.DbCollectionEntry_CannotSetCollectionProp((object) this.Name, (object) this.InternalEntityEntry.Entity.GetType().ToString());
      }
    }

    public override DbMemberEntry CreateDbMemberEntry() => (DbMemberEntry) new DbCollectionEntry(this);

    public override DbMemberEntry<TEntity, TProperty> CreateDbMemberEntry<TEntity, TProperty>() => this.CreateDbCollectionEntry<TEntity, TProperty>(this.EntryMetadata.ElementType);

    public virtual DbCollectionEntry<TEntity, TElement> CreateDbCollectionEntry<TEntity, TElement>() where TEntity : class => new DbCollectionEntry<TEntity, TElement>(this);

    private DbMemberEntry<TEntity, TProperty> CreateDbCollectionEntry<TEntity, TProperty>(
      Type elementType)
      where TEntity : class
    {
      Type key = typeof (DbMemberEntry<TEntity, TProperty>);
      Func<InternalCollectionEntry, object> func;
      if (!InternalCollectionEntry._entryFactories.TryGetValue(key, out func))
      {
        Type type = typeof (DbCollectionEntry<,>).MakeGenericType(typeof (TEntity), elementType);
        func = key.IsAssignableFrom(type) ? (Func<InternalCollectionEntry, object>) Delegate.CreateDelegate(typeof (Func<InternalCollectionEntry, object>), type.GetDeclaredMethod("Create", typeof (InternalCollectionEntry))) : throw Error.DbEntityEntry_WrongGenericForCollectionNavProp((object) typeof (TProperty), (object) this.Name, (object) this.EntryMetadata.DeclaringType, (object) typeof (ICollection<>).MakeGenericType(elementType));
        InternalCollectionEntry._entryFactories.TryAdd(key, func);
      }
      return (DbMemberEntry<TEntity, TProperty>) func(this);
    }
  }
}
