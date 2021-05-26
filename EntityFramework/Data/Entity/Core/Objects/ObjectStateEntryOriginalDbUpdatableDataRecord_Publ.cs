// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ObjectStateEntryOriginalDbUpdatableDataRecord_Public
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Core.Objects
{
  internal sealed class ObjectStateEntryOriginalDbUpdatableDataRecord_Public : 
    ObjectStateEntryOriginalDbUpdatableDataRecord_Internal
  {
    private readonly int _parentEntityPropertyIndex;

    internal ObjectStateEntryOriginalDbUpdatableDataRecord_Public(
      EntityEntry cacheEntry,
      StateManagerTypeMetadata metadata,
      object userObject,
      int parentEntityPropertyIndex)
      : base(cacheEntry, metadata, userObject)
    {
      this._parentEntityPropertyIndex = parentEntityPropertyIndex;
    }

    protected override object GetRecordValue(int ordinal) => (this._cacheEntry as EntityEntry).GetOriginalEntityValue(this._metadata, ordinal, this._userObject, ObjectStateValueRecord.OriginalUpdatablePublic, this.GetPropertyIndex(ordinal));

    protected override void SetRecordValue(int ordinal, object value)
    {
      StateManagerMemberMetadata managerMemberMetadata = this._metadata.Member(ordinal);
      if (managerMemberMetadata.IsComplex)
        throw new InvalidOperationException(Strings.ObjectStateEntry_SetOriginalComplexProperties((object) managerMemberMetadata.CLayerName));
      object newFieldValue = value ?? (object) DBNull.Value;
      EntityEntry cacheEntry = this._cacheEntry as EntityEntry;
      EntityState state = cacheEntry.State;
      if (!cacheEntry.HasRecordValueChanged((DbDataRecord) this, ordinal, newFieldValue))
        return;
      Type type = !managerMemberMetadata.IsPartOfKey ? managerMemberMetadata.ClrType : throw new InvalidOperationException(Strings.ObjectStateEntry_SetOriginalPrimaryKey((object) managerMemberMetadata.CLayerName));
      if (DBNull.Value == newFieldValue && type.IsValueType() && !managerMemberMetadata.CdmMetadata.Nullable)
        throw new InvalidOperationException(Strings.ObjectStateEntry_NullOriginalValueForNonNullableProperty((object) managerMemberMetadata.CLayerName, (object) managerMemberMetadata.ClrMetadata.Name, (object) managerMemberMetadata.ClrMetadata.DeclaringType.FullName));
      base.SetRecordValue(ordinal, value);
      if (state == EntityState.Unchanged && cacheEntry.State == EntityState.Modified)
        cacheEntry.ObjectStateManager.ChangeState(cacheEntry, state, EntityState.Modified);
      cacheEntry.SetModifiedPropertyInternal(this.GetPropertyIndex(ordinal));
    }

    private int GetPropertyIndex(int ordinal) => this._parentEntityPropertyIndex != -1 ? this._parentEntityPropertyIndex : ordinal;
  }
}
