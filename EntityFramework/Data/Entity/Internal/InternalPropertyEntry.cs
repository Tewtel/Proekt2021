// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InternalPropertyEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Internal
{
  internal abstract class InternalPropertyEntry : InternalMemberEntry
  {
    private bool _getterIsCached;
    private Func<object, object> _getter;
    private bool _setterIsCached;
    private Action<object, object> _setter;

    protected InternalPropertyEntry(
      InternalEntityEntry internalEntityEntry,
      PropertyEntryMetadata propertyMetadata)
      : base(internalEntityEntry, (MemberEntryMetadata) propertyMetadata)
    {
    }

    public abstract InternalPropertyEntry ParentPropertyEntry { get; }

    public abstract InternalPropertyValues ParentCurrentValues { get; }

    public abstract InternalPropertyValues ParentOriginalValues { get; }

    protected abstract Func<object, object> CreateGetter();

    protected abstract Action<object, object> CreateSetter();

    public abstract bool EntityPropertyIsModified();

    public abstract void SetEntityPropertyModified();

    public abstract void RejectEntityPropertyChanges();

    public abstract void UpdateComplexPropertyState();

    public Func<object, object> Getter
    {
      get
      {
        if (!this._getterIsCached)
        {
          this._getter = this.CreateGetter();
          this._getterIsCached = true;
        }
        return this._getter;
      }
    }

    public Action<object, object> Setter
    {
      get
      {
        if (!this._setterIsCached)
        {
          this._setter = this.CreateSetter();
          this._setterIsCached = true;
        }
        return this._setter;
      }
    }

    public virtual object OriginalValue
    {
      get
      {
        this.ValidateNotDetachedAndInModel(nameof (OriginalValue));
        InternalPropertyValues parentOriginalValues = this.ParentOriginalValues;
        object obj = parentOriginalValues == null ? (object) null : parentOriginalValues[this.Name];
        if (obj is InternalPropertyValues internalPropertyValues)
          obj = internalPropertyValues.ToObject();
        return obj;
      }
      set
      {
        this.ValidateNotDetachedAndInModel(nameof (OriginalValue));
        this.CheckNotSettingComplexPropertyToNull(value);
        this.SetPropertyValueUsingValues(this.ParentOriginalValues ?? throw Error.DbPropertyValues_CannotSetPropertyOnNullOriginalValue((object) this.Name, (object) this.ParentPropertyEntry.Name), value);
      }
    }

    public override object CurrentValue
    {
      get
      {
        if (this.Getter != null)
          return this.Getter(this.InternalEntityEntry.Entity);
        if (this.InternalEntityEntry.IsDetached || !this.EntryMetadata.IsMapped)
          throw Error.DbPropertyEntry_CannotGetCurrentValue((object) this.Name, (object) base.EntryMetadata.DeclaringType.Name);
        InternalPropertyValues parentCurrentValues = this.ParentCurrentValues;
        object obj = parentCurrentValues == null ? (object) null : parentCurrentValues[this.Name];
        if (obj is InternalPropertyValues internalPropertyValues)
          obj = internalPropertyValues.ToObject();
        return obj;
      }
      set
      {
        this.CheckNotSettingComplexPropertyToNull(value);
        if (!this.EntryMetadata.IsMapped || this.InternalEntityEntry.IsDetached || this.InternalEntityEntry.State == EntityState.Deleted)
        {
          if (!this.SetCurrentValueOnClrObject(value))
            throw Error.DbPropertyEntry_CannotSetCurrentValue((object) this.Name, (object) base.EntryMetadata.DeclaringType.Name);
        }
        else
        {
          this.SetPropertyValueUsingValues(this.ParentCurrentValues ?? throw Error.DbPropertyValues_CannotSetPropertyOnNullCurrentValue((object) this.Name, (object) this.ParentPropertyEntry.Name), value);
          if (!this.EntryMetadata.IsComplex)
            return;
          this.SetCurrentValueOnClrObject(value);
        }
      }
    }

    private void CheckNotSettingComplexPropertyToNull(object value)
    {
      if (value == null && this.EntryMetadata.IsComplex)
        throw Error.DbPropertyValues_ComplexObjectCannotBeNull((object) this.Name, (object) base.EntryMetadata.DeclaringType.Name);
    }

    private bool SetCurrentValueOnClrObject(object value)
    {
      if (this.Setter == null)
        return false;
      if (this.Getter == null || !DbHelpers.PropertyValuesEqual(value, this.Getter(this.InternalEntityEntry.Entity)))
      {
        this.Setter(this.InternalEntityEntry.Entity, value);
        if (this.EntryMetadata.IsMapped && (this.InternalEntityEntry.State == EntityState.Modified || this.InternalEntityEntry.State == EntityState.Unchanged))
          this.IsModified = true;
      }
      return true;
    }

    private void SetPropertyValueUsingValues(InternalPropertyValues internalValues, object value)
    {
      if (internalValues[this.Name] is InternalPropertyValues internalValue)
      {
        if (!internalValue.ObjectType.IsAssignableFrom(value.GetType()))
          throw Error.DbPropertyValues_AttemptToSetValuesFromWrongObject((object) value.GetType().Name, (object) internalValue.ObjectType.Name);
        internalValue.SetValues(value);
      }
      else
        internalValues[this.Name] = value;
    }

    public virtual InternalPropertyEntry Property(
      string property,
      Type requestedType = null,
      bool requireComplex = false)
    {
      InternalEntityEntry internalEntityEntry = this.InternalEntityEntry;
      string propertyName = property;
      Type requestedType1 = requestedType;
      if ((object) requestedType1 == null)
        requestedType1 = typeof (object);
      int num = requireComplex ? 1 : 0;
      return internalEntityEntry.Property(this, propertyName, requestedType1, num != 0);
    }

    public virtual bool IsModified
    {
      get => !this.InternalEntityEntry.IsDetached && this.EntryMetadata.IsMapped && this.EntityPropertyIsModified();
      set
      {
        this.ValidateNotDetachedAndInModel(nameof (IsModified));
        if (value)
        {
          this.SetEntityPropertyModified();
        }
        else
        {
          if (!this.IsModified)
            return;
          this.RejectEntityPropertyChanges();
        }
      }
    }

    private void ValidateNotDetachedAndInModel(string method)
    {
      if (!this.EntryMetadata.IsMapped)
        throw Error.DbPropertyEntry_NotSupportedForPropertiesNotInTheModel((object) method, (object) base.EntryMetadata.MemberName, (object) this.InternalEntityEntry.EntityType.Name);
      if (this.InternalEntityEntry.IsDetached)
        throw Error.DbPropertyEntry_NotSupportedForDetached((object) method, (object) base.EntryMetadata.MemberName, (object) this.InternalEntityEntry.EntityType.Name);
    }

    public PropertyEntryMetadata EntryMetadata => (PropertyEntryMetadata) base.EntryMetadata;

    public override DbMemberEntry CreateDbMemberEntry() => !this.EntryMetadata.IsComplex ? (DbMemberEntry) new DbPropertyEntry(this) : (DbMemberEntry) new DbComplexPropertyEntry(this);

    public override DbMemberEntry<TEntity, TProperty> CreateDbMemberEntry<TEntity, TProperty>() => !this.EntryMetadata.IsComplex ? (DbMemberEntry<TEntity, TProperty>) new DbPropertyEntry<TEntity, TProperty>(this) : (DbMemberEntry<TEntity, TProperty>) new DbComplexPropertyEntry<TEntity, TProperty>(this);
  }
}
