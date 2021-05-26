// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.DbDataRecordPropertyValuesItem
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Internal
{
  internal class DbDataRecordPropertyValuesItem : IPropertyValuesItem
  {
    private readonly DbUpdatableDataRecord _dataRecord;
    private readonly int _ordinal;
    private object _value;

    public DbDataRecordPropertyValuesItem(
      DbUpdatableDataRecord dataRecord,
      int ordinal,
      object value)
    {
      this._dataRecord = dataRecord;
      this._ordinal = ordinal;
      this._value = value;
    }

    public object Value
    {
      get => this._value;
      set
      {
        this._dataRecord.SetValue(this._ordinal, value);
        this._value = value;
      }
    }

    public string Name => this._dataRecord.GetName(this._ordinal);

    public bool IsComplex => this._dataRecord.DataRecordInfo.FieldMetadata[this._ordinal].FieldType.TypeUsage.EdmType.BuiltInTypeKind == BuiltInTypeKind.ComplexType;

    public Type Type => this._dataRecord.GetFieldType(this._ordinal);
  }
}
