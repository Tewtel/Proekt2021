// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ObjectViewListener
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.ComponentModel;
using System.Data.Entity.Core.Objects.DataClasses;

namespace System.Data.Entity.Core.Objects
{
  internal sealed class ObjectViewListener
  {
    private readonly WeakReference _viewWeak;
    private readonly object _dataSource;
    private readonly IList _list;

    internal ObjectViewListener(IObjectView view, IList list, object dataSource)
    {
      this._viewWeak = new WeakReference((object) view);
      this._dataSource = dataSource;
      this._list = list;
      this.RegisterCollectionEvents();
      this.RegisterEntityEvents();
    }

    private void CleanUpListener()
    {
      this.UnregisterCollectionEvents();
      this.UnregisterEntityEvents();
    }

    private void RegisterCollectionEvents()
    {
      if (this._dataSource is ObjectStateManager dataSource)
      {
        dataSource.EntityDeleted += new CollectionChangeEventHandler(this.CollectionChanged);
      }
      else
      {
        if (this._dataSource == null)
          return;
        ((RelatedEnd) this._dataSource).AssociationChangedForObjectView += new CollectionChangeEventHandler(this.CollectionChanged);
      }
    }

    private void UnregisterCollectionEvents()
    {
      if (this._dataSource is ObjectStateManager dataSource)
      {
        dataSource.EntityDeleted -= new CollectionChangeEventHandler(this.CollectionChanged);
      }
      else
      {
        if (this._dataSource == null)
          return;
        ((RelatedEnd) this._dataSource).AssociationChangedForObjectView -= new CollectionChangeEventHandler(this.CollectionChanged);
      }
    }

    internal void RegisterEntityEvents(object entity)
    {
      if (!(entity is INotifyPropertyChanged notifyPropertyChanged))
        return;
      notifyPropertyChanged.PropertyChanged += new PropertyChangedEventHandler(this.EntityPropertyChanged);
    }

    private void RegisterEntityEvents()
    {
      if (this._list == null)
        return;
      foreach (object obj in (IEnumerable) this._list)
      {
        if (obj is INotifyPropertyChanged notifyPropertyChanged1)
          notifyPropertyChanged1.PropertyChanged += new PropertyChangedEventHandler(this.EntityPropertyChanged);
      }
    }

    internal void UnregisterEntityEvents(object entity)
    {
      if (!(entity is INotifyPropertyChanged notifyPropertyChanged))
        return;
      notifyPropertyChanged.PropertyChanged -= new PropertyChangedEventHandler(this.EntityPropertyChanged);
    }

    private void UnregisterEntityEvents()
    {
      if (this._list == null)
        return;
      foreach (object obj in (IEnumerable) this._list)
      {
        if (obj is INotifyPropertyChanged notifyPropertyChanged1)
          notifyPropertyChanged1.PropertyChanged -= new PropertyChangedEventHandler(this.EntityPropertyChanged);
      }
    }

    private void EntityPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      IObjectView target = (IObjectView) this._viewWeak.Target;
      if (target != null)
        target.EntityPropertyChanged(sender, e);
      else
        this.CleanUpListener();
    }

    private void CollectionChanged(object sender, CollectionChangeEventArgs e)
    {
      IObjectView target = (IObjectView) this._viewWeak.Target;
      if (target != null)
        target.CollectionChanged(sender, e);
      else
        this.CleanUpListener();
    }
  }
}
