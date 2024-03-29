﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.DbDataRecordPropertyValues
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Internal
{
  internal class DbDataRecordPropertyValues : InternalPropertyValues
  {
    private readonly DbUpdatableDataRecord _dataRecord;
    private ISet<string> _names;

    internal DbDataRecordPropertyValues(
      InternalContext internalContext,
      Type type,
      DbUpdatableDataRecord dataRecord,
      bool isEntity)
      : base(internalContext, type, isEntity)
    {
      this._dataRecord = dataRecord;
    }

    protected override IPropertyValuesItem GetItemImpl(string propertyName)
    {
      int ordinal = this._dataRecord.GetOrdinal(propertyName);
      object obj = this._dataRecord[ordinal];
      if (obj is DbUpdatableDataRecord dataRecord)
        obj = (object) new DbDataRecordPropertyValues(this.InternalContext, this._dataRecord.GetFieldType(ordinal), dataRecord, false);
      else if (obj == DBNull.Value)
        obj = (object) null;
      return (IPropertyValuesItem) new DbDataRecordPropertyValuesItem(this._dataRecord, ordinal, obj);
    }

    public override ISet<string> PropertyNames
    {
      get
      {
        if (this._names == null)
        {
          HashSet<string> stringSet = new HashSet<string>();
          for (int i = 0; i < this._dataRecord.FieldCount; ++i)
            stringSet.Add(this._dataRecord.GetName(i));
          this._names = (ISet<string>) new ReadOnlySet<string>((ISet<string>) stringSet);
        }
        return this._names;
      }
    }
  }
}
