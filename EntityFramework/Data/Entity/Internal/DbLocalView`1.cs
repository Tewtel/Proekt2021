// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.DbLocalView`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Internal
{
  internal class DbLocalView<TEntity> : 
    ObservableCollection<TEntity>,
    ICollection<TEntity>,
    IEnumerable<TEntity>,
    IEnumerable,
    IList,
    ICollection
    where TEntity : class
  {
    private readonly InternalContext _internalContext;
    private bool _inStateManagerChanged;
    private ObservableBackedBindingList<TEntity> _bindingList;

    public DbLocalView()
    {
    }

    public DbLocalView(IEnumerable<TEntity> collection)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<TEntity>>(collection, nameof (collection));
      collection.Each<TEntity>(new Action<TEntity>(((Collection<TEntity>) this).Add));
    }

    internal DbLocalView(InternalContext internalContext)
    {
      this._internalContext = internalContext;
      try
      {
        this._inStateManagerChanged = true;
        foreach (TEntity localEntity in this._internalContext.GetLocalEntities<TEntity>())
          this.Add(localEntity);
      }
      finally
      {
        this._inStateManagerChanged = false;
      }
      this._internalContext.RegisterObjectStateManagerChangedEvent(new CollectionChangeEventHandler(this.StateManagerChangedHandler));
    }

    internal ObservableBackedBindingList<TEntity> BindingList => this._bindingList ?? (this._bindingList = new ObservableBackedBindingList<TEntity>((ObservableCollection<TEntity>) this));

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (!this._inStateManagerChanged && this._internalContext != null)
      {
        if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
        {
          foreach (TEntity oldItem in (IEnumerable) e.OldItems)
            this._internalContext.Set<TEntity>().Remove(oldItem);
        }
        if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
        {
          foreach (TEntity newItem in (IEnumerable) e.NewItems)
          {
            if (!this._internalContext.EntityInContextAndNotDeleted((object) newItem))
              this._internalContext.Set<TEntity>().Add(newItem);
          }
        }
      }
      base.OnCollectionChanged(e);
    }

    private void StateManagerChangedHandler(object sender, CollectionChangeEventArgs e)
    {
      try
      {
        this._inStateManagerChanged = true;
        if (!(e.Element is TEntity element2))
          return;
        if (e.Action == CollectionChangeAction.Remove && this.Contains(element2))
        {
          this.Remove(element2);
        }
        else
        {
          if (e.Action != CollectionChangeAction.Add || this.Contains(element2))
            return;
          this.Add(element2);
        }
      }
      finally
      {
        this._inStateManagerChanged = false;
      }
    }

    protected override void ClearItems() => new List<TEntity>((IEnumerable<TEntity>) this).Each<TEntity, bool>((Func<TEntity, bool>) (t => this.Remove(t)));

    protected override void InsertItem(int index, TEntity item)
    {
      if (this.Contains(item))
        return;
      base.InsertItem(index, item);
    }

    public new virtual bool Contains(TEntity item)
    {
      IEqualityComparer<TEntity> equalityComparer = (IEqualityComparer<TEntity>) ObjectReferenceEqualityComparer.Default;
      foreach (TEntity x in (IEnumerable<TEntity>) this.Items)
      {
        if (equalityComparer.Equals(x, item))
          return true;
      }
      return false;
    }

    public new virtual bool Remove(TEntity item)
    {
      IEqualityComparer<TEntity> equalityComparer = (IEqualityComparer<TEntity>) ObjectReferenceEqualityComparer.Default;
      int index = 0;
      while (index < this.Count && !equalityComparer.Equals(this.Items[index], item))
        ++index;
      if (index == this.Count)
        return false;
      this.RemoveItem(index);
      return true;
    }

    bool ICollection<TEntity>.Contains(TEntity item) => this.Contains(item);

    bool ICollection<TEntity>.Remove(TEntity item) => this.Remove(item);

    bool IList.Contains(object value) => DbLocalView<TEntity>.IsCompatibleObject(value) && this.Contains((TEntity) value);

    void IList.Remove(object value)
    {
      if (!DbLocalView<TEntity>.IsCompatibleObject(value))
        return;
      this.Remove((TEntity) value);
    }

    private static bool IsCompatibleObject(object value) => value is TEntity || value == null;
  }
}
