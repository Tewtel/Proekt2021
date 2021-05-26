// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.NavigationPropertyAccessor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Threading;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class NavigationPropertyAccessor
  {
    private Func<object, object> _memberGetter;
    private Action<object, object> _memberSetter;
    private Action<object, object> _collectionAdd;
    private Func<object, object, bool> _collectionRemove;
    private Func<object> _collectionCreate;
    private readonly string _propertyName;

    public NavigationPropertyAccessor(string propertyName) => this._propertyName = propertyName;

    public bool HasProperty => this._propertyName != null;

    public string PropertyName => this._propertyName;

    public Func<object, object> ValueGetter
    {
      get => this._memberGetter;
      set => Interlocked.CompareExchange<Func<object, object>>(ref this._memberGetter, value, (Func<object, object>) null);
    }

    public Action<object, object> ValueSetter
    {
      get => this._memberSetter;
      set => Interlocked.CompareExchange<Action<object, object>>(ref this._memberSetter, value, (Action<object, object>) null);
    }

    public Action<object, object> CollectionAdd
    {
      get => this._collectionAdd;
      set => Interlocked.CompareExchange<Action<object, object>>(ref this._collectionAdd, value, (Action<object, object>) null);
    }

    public Func<object, object, bool> CollectionRemove
    {
      get => this._collectionRemove;
      set => Interlocked.CompareExchange<Func<object, object, bool>>(ref this._collectionRemove, value, (Func<object, object, bool>) null);
    }

    public Func<object> CollectionCreate
    {
      get => this._collectionCreate;
      set => Interlocked.CompareExchange<Func<object>>(ref this._collectionCreate, value, (Func<object>) null);
    }

    public static NavigationPropertyAccessor NoNavigationProperty => new NavigationPropertyAccessor((string) null);
  }
}
