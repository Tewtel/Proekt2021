// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.SortableBindingList`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Utilities;
using System.Xml.Linq;

namespace System.Data.Entity.Internal
{
  internal class SortableBindingList<T> : BindingList<T>
  {
    private bool _isSorted;
    private ListSortDirection _sortDirection;
    private PropertyDescriptor _sortProperty;

    public SortableBindingList(List<T> list)
      : base((IList<T>) list)
    {
    }

    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    {
      if (!SortableBindingList<T>.PropertyComparer.CanSort(prop.PropertyType))
        return;
      ((List<T>) this.Items).Sort((IComparer<T>) new SortableBindingList<T>.PropertyComparer(prop, direction));
      this._sortDirection = direction;
      this._sortProperty = prop;
      this._isSorted = true;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    protected override void RemoveSortCore()
    {
      this._isSorted = false;
      this._sortProperty = (PropertyDescriptor) null;
    }

    protected override bool IsSortedCore => this._isSorted;

    protected override ListSortDirection SortDirectionCore => this._sortDirection;

    protected override PropertyDescriptor SortPropertyCore => this._sortProperty;

    protected override bool SupportsSortingCore => true;

    internal class PropertyComparer : Comparer<T>
    {
      private readonly IComparer _comparer;
      private readonly ListSortDirection _direction;
      private readonly PropertyDescriptor _prop;
      private readonly bool _useToString;

      public PropertyComparer(PropertyDescriptor prop, ListSortDirection direction)
      {
        this._prop = prop.ComponentType.IsAssignableFrom(typeof (T)) ? prop : throw new MissingMemberException(typeof (T).Name, prop.Name);
        this._direction = direction;
        if (SortableBindingList<T>.PropertyComparer.CanSortWithIComparable(prop.PropertyType))
        {
          this._comparer = (IComparer) typeof (Comparer<>).MakeGenericType(prop.PropertyType).GetDeclaredProperty("Default").GetValue((object) null, (object[]) null);
          this._useToString = false;
        }
        else
        {
          this._comparer = (IComparer) StringComparer.CurrentCultureIgnoreCase;
          this._useToString = true;
        }
      }

      public override int Compare(T left, T right)
      {
        object obj1 = this._prop.GetValue((object) left);
        object obj2 = this._prop.GetValue((object) right);
        if (this._useToString)
        {
          obj1 = (object) obj1?.ToString();
          obj2 = (object) obj2?.ToString();
        }
        return this._direction != ListSortDirection.Ascending ? this._comparer.Compare(obj2, obj1) : this._comparer.Compare(obj1, obj2);
      }

      public static bool CanSort(Type type) => SortableBindingList<T>.PropertyComparer.CanSortWithToString(type) || SortableBindingList<T>.PropertyComparer.CanSortWithIComparable(type);

      private static bool CanSortWithIComparable(Type type)
      {
        if (type.GetInterface("IComparable") != (Type) null)
          return true;
        return type.IsGenericType() && type.GetGenericTypeDefinition() == typeof (Nullable<>);
      }

      private static bool CanSortWithToString(Type type) => type.Equals(typeof (XNode)) || type.IsSubclassOf(typeof (XNode));
    }
  }
}
