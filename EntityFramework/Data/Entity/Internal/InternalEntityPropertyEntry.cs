﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InternalEntityPropertyEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Linq;

namespace System.Data.Entity.Internal
{
  internal class InternalEntityPropertyEntry : InternalPropertyEntry
  {
    public InternalEntityPropertyEntry(
      InternalEntityEntry internalEntityEntry,
      PropertyEntryMetadata propertyMetadata)
      : base(internalEntityEntry, propertyMetadata)
    {
    }

    public override InternalPropertyEntry ParentPropertyEntry => (InternalPropertyEntry) null;

    public override InternalPropertyValues ParentCurrentValues => this.InternalEntityEntry.CurrentValues;

    public override InternalPropertyValues ParentOriginalValues => this.InternalEntityEntry.OriginalValues;

    protected override Func<object, object> CreateGetter()
    {
      Func<object, object> func;
      DbHelpers.GetPropertyGetters(this.InternalEntityEntry.EntityType).TryGetValue(this.Name, out func);
      return func;
    }

    protected override Action<object, object> CreateSetter()
    {
      Action<object, object> action;
      DbHelpers.GetPropertySetters(this.InternalEntityEntry.EntityType).TryGetValue(this.Name, out action);
      return action;
    }

    public override bool EntityPropertyIsModified() => this.InternalEntityEntry.ObjectStateEntry.GetModifiedProperties().Contains<string>(this.Name);

    public override void SetEntityPropertyModified() => this.InternalEntityEntry.ObjectStateEntry.SetModifiedProperty(this.Name);

    public override void RejectEntityPropertyChanges() => this.InternalEntityEntry.ObjectStateEntry.RejectPropertyChanges(this.Name);

    public override void UpdateComplexPropertyState()
    {
      if (this.InternalEntityEntry.ObjectStateEntry.IsPropertyChanged(this.Name))
        return;
      this.RejectEntityPropertyChanges();
    }
  }
}
