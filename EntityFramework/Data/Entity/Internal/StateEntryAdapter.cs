// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.StateEntryAdapter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Internal
{
  internal class StateEntryAdapter : IEntityStateEntry
  {
    private readonly ObjectStateEntry _stateEntry;

    public StateEntryAdapter(ObjectStateEntry stateEntry) => this._stateEntry = stateEntry;

    public object Entity => this._stateEntry.Entity;

    public EntityState State => this._stateEntry.State;

    public void ChangeState(EntityState state) => this._stateEntry.ChangeState(state);

    public DbUpdatableDataRecord CurrentValues => (DbUpdatableDataRecord) this._stateEntry.CurrentValues;

    public DbUpdatableDataRecord GetUpdatableOriginalValues() => (DbUpdatableDataRecord) this._stateEntry.GetUpdatableOriginalValues();

    public EntitySetBase EntitySet => this._stateEntry.EntitySet;

    public EntityKey EntityKey => this._stateEntry.EntityKey;

    public IEnumerable<string> GetModifiedProperties() => this._stateEntry.GetModifiedProperties();

    public void SetModifiedProperty(string propertyName) => this._stateEntry.SetModifiedProperty(propertyName);

    public void RejectPropertyChanges(string propertyName) => this._stateEntry.RejectPropertyChanges(propertyName);

    public bool IsPropertyChanged(string propertyName) => this._stateEntry.IsPropertyChanged(propertyName);
  }
}
