// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ObjectView`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.ComponentModel;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Core.Objects
{
  internal class ObjectView<TElement> : 
    IBindingList,
    IList,
    ICollection,
    IEnumerable,
    ICancelAddNew,
    IObjectView
  {
    private bool _suspendEvent;
    private ListChangedEventHandler onListChanged;
    private readonly ObjectViewListener _listener;
    private int _addNewIndex = -1;
    private readonly IObjectViewData<TElement> _viewData;

    internal ObjectView(IObjectViewData<TElement> viewData, object eventDataSource)
    {
      this._viewData = viewData;
      this._listener = new ObjectViewListener((IObjectView) this, (IList) this._viewData.List, eventDataSource);
    }

    private void EnsureWritableList()
    {
      if (((IList) this).IsReadOnly)
        throw new InvalidOperationException(Strings.ObjectView_WriteOperationNotAllowedOnReadOnlyBindingList);
    }

    private static bool IsElementTypeAbstract => typeof (TElement).IsAbstract();

    void ICancelAddNew.CancelNew(int itemIndex)
    {
      if (this._addNewIndex < 0 || itemIndex != this._addNewIndex)
        return;
      TElement element = this._viewData.List[this._addNewIndex];
      this._listener.UnregisterEntityEvents((object) element);
      int addNewIndex = this._addNewIndex;
      this._addNewIndex = -1;
      try
      {
        this._suspendEvent = true;
        this._viewData.Remove(element, true);
      }
      finally
      {
        this._suspendEvent = false;
      }
      this.OnListChanged(ListChangedType.ItemDeleted, addNewIndex, -1);
    }

    void ICancelAddNew.EndNew(int itemIndex)
    {
      if (this._addNewIndex < 0 || itemIndex != this._addNewIndex)
        return;
      this._viewData.CommitItemAt(this._addNewIndex);
      this._addNewIndex = -1;
    }

    bool IBindingList.AllowNew => this._viewData.AllowNew && !ObjectView<TElement>.IsElementTypeAbstract;

    bool IBindingList.AllowEdit => this._viewData.AllowEdit;

    object IBindingList.AddNew()
    {
      this.EnsureWritableList();
      if (ObjectView<TElement>.IsElementTypeAbstract)
        throw new InvalidOperationException(Strings.ObjectView_AddNewOperationNotAllowedOnAbstractBindingList);
      this._viewData.EnsureCanAddNew();
      ((ICancelAddNew) this).EndNew(this._addNewIndex);
      TElement instance = (TElement) Activator.CreateInstance(typeof (TElement));
      this._addNewIndex = this._viewData.Add(instance, true);
      this._listener.RegisterEntityEvents((object) instance);
      this.OnListChanged(ListChangedType.ItemAdded, this._addNewIndex, -1);
      return (object) instance;
    }

    bool IBindingList.AllowRemove => this._viewData.AllowRemove;

    bool IBindingList.SupportsChangeNotification => true;

    bool IBindingList.SupportsSearching => false;

    bool IBindingList.SupportsSorting => false;

    bool IBindingList.IsSorted => false;

    PropertyDescriptor IBindingList.SortProperty => throw new NotSupportedException();

    ListSortDirection IBindingList.SortDirection => throw new NotSupportedException();

    public event ListChangedEventHandler ListChanged
    {
      add => this.onListChanged += value;
      remove => this.onListChanged -= value;
    }

    void IBindingList.AddIndex(PropertyDescriptor property) => throw new NotSupportedException();

    void IBindingList.ApplySort(
      PropertyDescriptor property,
      ListSortDirection direction)
    {
      throw new NotSupportedException();
    }

    int IBindingList.Find(PropertyDescriptor property, object key) => throw new NotSupportedException();

    void IBindingList.RemoveIndex(PropertyDescriptor property) => throw new NotSupportedException();

    void IBindingList.RemoveSort() => throw new NotSupportedException();

    public TElement this[int index]
    {
      get => this._viewData.List[index];
      set => throw new InvalidOperationException(Strings.ObjectView_CannotReplacetheEntityorRow);
    }

    object IList.this[int index]
    {
      get => (object) this._viewData.List[index];
      set => throw new InvalidOperationException(Strings.ObjectView_CannotReplacetheEntityorRow);
    }

    bool IList.IsReadOnly => !this._viewData.AllowNew && !this._viewData.AllowRemove;

    bool IList.IsFixedSize => false;

    int IList.Add(object value)
    {
      System.Data.Entity.Utilities.Check.NotNull<object>(value, nameof (value));
      this.EnsureWritableList();
      if (!(value is TElement))
        throw new ArgumentException(Strings.ObjectView_IncompatibleArgument);
      ((ICancelAddNew) this).EndNew(this._addNewIndex);
      int newIndex = ((IList) this).IndexOf(value);
      if (newIndex == -1)
      {
        newIndex = this._viewData.Add((TElement) value, false);
        if (!this._viewData.FiresEventOnAdd)
        {
          this._listener.RegisterEntityEvents(value);
          this.OnListChanged(ListChangedType.ItemAdded, newIndex, -1);
        }
      }
      return newIndex;
    }

    void IList.Clear()
    {
      this.EnsureWritableList();
      ((ICancelAddNew) this).EndNew(this._addNewIndex);
      if (this._viewData.FiresEventOnClear)
      {
        this._viewData.Clear();
      }
      else
      {
        try
        {
          this._suspendEvent = true;
          this._viewData.Clear();
        }
        finally
        {
          this._suspendEvent = false;
        }
        this.OnListChanged(ListChangedType.Reset, -1, -1);
      }
    }

    bool IList.Contains(object value) => value is TElement element && this._viewData.List.Contains(element);

    int IList.IndexOf(object value) => !(value is TElement element) ? -1 : this._viewData.List.IndexOf(element);

    void IList.Insert(int index, object value) => throw new NotSupportedException(Strings.ObjectView_IndexBasedInsertIsNotSupported);

    void IList.Remove(object value)
    {
      System.Data.Entity.Utilities.Check.NotNull<object>(value, nameof (value));
      this.EnsureWritableList();
      if (!(value is TElement element))
        throw new ArgumentException(Strings.ObjectView_IncompatibleArgument);
      ((ICancelAddNew) this).EndNew(this._addNewIndex);
      int newIndex = this._viewData.List.IndexOf(element);
      if (!this._viewData.Remove(element, false) || this._viewData.FiresEventOnRemove)
        return;
      this._listener.UnregisterEntityEvents((object) element);
      this.OnListChanged(ListChangedType.ItemDeleted, newIndex, -1);
    }

    void IList.RemoveAt(int index) => ((IList) this).Remove(((IList) this)[index]);

    public int Count => this._viewData.List.Count;

    public void CopyTo(Array array, int index) => ((ICollection) this._viewData.List).CopyTo(array, index);

    object ICollection.SyncRoot => (object) this;

    bool ICollection.IsSynchronized => false;

    public IEnumerator GetEnumerator() => (IEnumerator) this._viewData.List.GetEnumerator();

    private void OnListChanged(ListChangedType listchangedType, int newIndex, int oldIndex) => this.OnListChanged(new ListChangedEventArgs(listchangedType, newIndex, oldIndex));

    private void OnListChanged(ListChangedEventArgs changeArgs)
    {
      if (this.onListChanged == null || this._suspendEvent)
        return;
      this.onListChanged((object) this, changeArgs);
    }

    void IObjectView.EntityPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      int num = ((IList) this).IndexOf((object) (TElement) sender);
      this.OnListChanged(ListChangedType.ItemChanged, num, num);
    }

    void IObjectView.CollectionChanged(object sender, CollectionChangeEventArgs e)
    {
      TElement element = default (TElement);
      if (this._addNewIndex >= 0)
        element = this[this._addNewIndex];
      ListChangedEventArgs changeArgs = this._viewData.OnCollectionChanged(sender, e, this._listener);
      if (this._addNewIndex >= 0)
      {
        if (this._addNewIndex >= this.Count)
          this._addNewIndex = ((IList) this).IndexOf((object) element);
        else if (!this[this._addNewIndex].Equals((object) element))
          this._addNewIndex = ((IList) this).IndexOf((object) element);
      }
      if (changeArgs == null)
        return;
      this.OnListChanged(changeArgs);
    }
  }
}
