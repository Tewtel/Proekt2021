// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ObjectStateEntryOriginalDbUpdatableDataRecord_Internal
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects
{
  internal class ObjectStateEntryOriginalDbUpdatableDataRecord_Internal : OriginalValueRecord
  {
    internal ObjectStateEntryOriginalDbUpdatableDataRecord_Internal(
      EntityEntry cacheEntry,
      StateManagerTypeMetadata metadata,
      object userObject)
      : base((ObjectStateEntry) cacheEntry, metadata, userObject)
    {
      switch (cacheEntry.State)
      {
      }
    }

    protected override object GetRecordValue(int ordinal) => (this._cacheEntry as EntityEntry).GetOriginalEntityValue(this._metadata, ordinal, this._userObject, ObjectStateValueRecord.OriginalUpdatableInternal);

    protected override void SetRecordValue(int ordinal, object value) => (this._cacheEntry as EntityEntry).SetOriginalEntityValue(this._metadata, ordinal, this._userObject, value);
  }
}
