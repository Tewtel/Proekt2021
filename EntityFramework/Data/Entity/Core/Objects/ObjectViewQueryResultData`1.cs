// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ObjectViewQueryResultData`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Objects
{
  internal sealed class ObjectViewQueryResultData<TElement> : IObjectViewData<TElement>
  {
    private readonly System.Collections.Generic.List<TElement> _bindingList;
    private readonly ObjectContext _objectContext;
    private readonly EntitySet _entitySet;
    private readonly bool _canEditItems;
    private readonly bool _canModifyList;

    internal ObjectViewQueryResultData(
      IEnumerable queryResults,
      ObjectContext objectContext,
      bool forceReadOnlyList,
      EntitySet entitySet)
    {
      bool flag = ObjectViewQueryResultData<TElement>.IsEditable(typeof (TElement));
      this._objectContext = objectContext;
      this._entitySet = entitySet;
      this._canEditItems = flag;
      this._canModifyList = !forceReadOnlyList & flag && this._objectContext != null;
      this._bindingList = new System.Collections.Generic.List<TElement>();
      foreach (TElement queryResult in queryResults)
        this._bindingList.Add(queryResult);
    }

    private static bool IsEditable(Type elementType)
    {
      if (elementType == typeof (DbDataRecord))
        return false;
      return !(elementType != typeof (DbDataRecord)) || !elementType.IsSubclassOf(typeof (DbDataRecord));
    }

    private void EnsureEntitySet()
    {
      if (this._entitySet == null)
        throw new InvalidOperationException(Strings.ObjectView_CannotResolveTheEntitySet((object) typeof (TElement).FullName));
    }

    public IList<TElement> List => (IList<TElement>) this._bindingList;

    public bool AllowNew => this._canModifyList && this._entitySet != null;

    public bool AllowEdit => this._canEditItems;

    public bool AllowRemove => this._canModifyList;

    public bool FiresEventOnAdd => false;

    public bool FiresEventOnRemove => true;

    public bool FiresEventOnClear => false;

    public void EnsureCanAddNew() => this.EnsureEntitySet();

    public int Add(TElement item, bool isAddNew)
    {
      this.EnsureEntitySet();
      if (!isAddNew)
        this._objectContext.AddObject(TypeHelpers.GetFullName(this._entitySet.EntityContainer.Name, this._entitySet.Name), (object) item);
      this._bindingList.Add(item);
      return this._bindingList.Count - 1;
    }

    public void CommitItemAt(int index)
    {
      this.EnsureEntitySet();
      TElement binding = this._bindingList[index];
      this._objectContext.AddObject(TypeHelpers.GetFullName(this._entitySet.EntityContainer.Name, this._entitySet.Name), (object) binding);
    }

    public void Clear()
    {
      while (0 < this._bindingList.Count)
        this.Remove(this._bindingList[this._bindingList.Count - 1], false);
    }

    public bool Remove(TElement item, bool isCancelNew)
    {
      bool flag;
      if (isCancelNew)
      {
        flag = this._bindingList.Remove(item);
      }
      else
      {
        EntityEntry entityEntry = this._objectContext.ObjectStateManager.FindEntityEntry((object) item);
        if (entityEntry != null)
        {
          entityEntry.Delete();
          flag = true;
        }
        else
          flag = false;
      }
      return flag;
    }

    public ListChangedEventArgs OnCollectionChanged(
      object sender,
      CollectionChangeEventArgs e,
      ObjectViewListener listener)
    {
      ListChangedEventArgs changedEventArgs = (ListChangedEventArgs) null;
      if (e.Element.GetType().IsAssignableFrom(typeof (TElement)) && this._bindingList.Contains((TElement) e.Element))
      {
        TElement element = (TElement) e.Element;
        int newIndex = this._bindingList.IndexOf(element);
        if (newIndex >= 0 && e.Action == CollectionChangeAction.Remove)
        {
          this._bindingList.Remove(element);
          listener.UnregisterEntityEvents((object) element);
          changedEventArgs = new ListChangedEventArgs(ListChangedType.ItemDeleted, newIndex, -1);
        }
      }
      return changedEventArgs;
    }
  }
}
