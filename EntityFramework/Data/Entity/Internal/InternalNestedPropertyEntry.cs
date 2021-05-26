// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InternalNestedPropertyEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity.Internal
{
  internal class InternalNestedPropertyEntry : InternalPropertyEntry
  {
    private readonly InternalPropertyEntry _parentPropertyEntry;

    public InternalNestedPropertyEntry(
      InternalPropertyEntry parentPropertyEntry,
      PropertyEntryMetadata propertyMetadata)
      : base(parentPropertyEntry.InternalEntityEntry, propertyMetadata)
    {
      this._parentPropertyEntry = parentPropertyEntry;
    }

    public override InternalPropertyEntry ParentPropertyEntry => this._parentPropertyEntry;

    public override InternalPropertyValues ParentCurrentValues
    {
      get
      {
        InternalPropertyValues parentCurrentValues = this._parentPropertyEntry.ParentCurrentValues;
        return parentCurrentValues == null ? (InternalPropertyValues) (object) null : (InternalPropertyValues) parentCurrentValues[this._parentPropertyEntry.Name];
      }
    }

    public override InternalPropertyValues ParentOriginalValues
    {
      get
      {
        InternalPropertyValues parentOriginalValues = this._parentPropertyEntry.ParentOriginalValues;
        return parentOriginalValues == null ? (InternalPropertyValues) (object) null : (InternalPropertyValues) parentOriginalValues[this._parentPropertyEntry.Name];
      }
    }

    protected override Func<object, object> CreateGetter()
    {
      Func<object, object> parentGetter = this._parentPropertyEntry.Getter;
      if (parentGetter == null)
        return (Func<object, object>) null;
      Func<object, object> getter;
      return !DbHelpers.GetPropertyGetters(this.EntryMetadata.DeclaringType).TryGetValue(this.Name, out getter) ? (Func<object, object>) null : (Func<object, object>) (o =>
      {
        object obj = parentGetter(o);
        return obj != null ? getter(obj) : (object) null;
      });
    }

    protected override Action<object, object> CreateSetter()
    {
      Func<object, object> parentGetter = this._parentPropertyEntry.Getter;
      if (parentGetter == null)
        return (Action<object, object>) null;
      Action<object, object> setter;
      return !DbHelpers.GetPropertySetters(this.EntryMetadata.DeclaringType).TryGetValue(this.Name, out setter) ? (Action<object, object>) null : (Action<object, object>) ((o, v) =>
      {
        if (parentGetter(o) == null)
          throw Error.DbPropertyValues_CannotSetPropertyOnNullCurrentValue((object) this.Name, (object) this.ParentPropertyEntry.Name);
        setter(parentGetter(o), v);
      });
    }

    public override bool EntityPropertyIsModified() => this._parentPropertyEntry.EntityPropertyIsModified();

    public override void SetEntityPropertyModified() => this._parentPropertyEntry.SetEntityPropertyModified();

    public override void RejectEntityPropertyChanges()
    {
      this.CurrentValue = this.OriginalValue;
      this.UpdateComplexPropertyState();
    }

    public override void UpdateComplexPropertyState() => this._parentPropertyEntry.UpdateComplexPropertyState();
  }
}
