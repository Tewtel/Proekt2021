// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ObjectStateEntryDbDataRecord
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Resources;
using System.Globalization;

namespace System.Data.Entity.Core.Objects
{
  internal sealed class ObjectStateEntryDbDataRecord : DbDataRecord, IExtendedDataRecord, IDataRecord
  {
    private readonly StateManagerTypeMetadata _metadata;
    private readonly ObjectStateEntry _cacheEntry;
    private readonly object _userObject;
    private DataRecordInfo _recordInfo;

    internal ObjectStateEntryDbDataRecord(
      EntityEntry cacheEntry,
      StateManagerTypeMetadata metadata,
      object userObject)
    {
      switch (cacheEntry.State)
      {
        case EntityState.Unchanged:
        case EntityState.Deleted:
        case EntityState.Modified:
          this._cacheEntry = (ObjectStateEntry) cacheEntry;
          this._userObject = userObject;
          this._metadata = metadata;
          break;
      }
    }

    internal ObjectStateEntryDbDataRecord(RelationshipEntry cacheEntry)
    {
      switch (cacheEntry.State)
      {
        case EntityState.Unchanged:
        case EntityState.Deleted:
        case EntityState.Modified:
          this._cacheEntry = (ObjectStateEntry) cacheEntry;
          break;
      }
    }

    public override int FieldCount => this._cacheEntry.GetFieldCount(this._metadata);

    public override object this[int ordinal] => this.GetValue(ordinal);

    public override object this[string name] => this.GetValue(this.GetOrdinal(name));

    public override bool GetBoolean(int ordinal) => (bool) this.GetValue(ordinal);

    public override byte GetByte(int ordinal) => (byte) this.GetValue(ordinal);

    public override long GetBytes(
      int ordinal,
      long dataIndex,
      byte[] buffer,
      int bufferIndex,
      int length)
    {
      byte[] numArray = (byte[]) this.GetValue(ordinal);
      if (buffer == null)
        return (long) numArray.Length;
      int num1 = (int) dataIndex;
      int num2 = Math.Min(numArray.Length - num1, length);
      if (num1 < 0)
        throw new ArgumentOutOfRangeException(nameof (dataIndex), Strings.ADP_InvalidSourceBufferIndex((object) numArray.Length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) ((long) num1).ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      if (bufferIndex < 0 || bufferIndex > 0 && bufferIndex >= buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (bufferIndex), Strings.ADP_InvalidDestinationBufferIndex((object) buffer.Length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) bufferIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      if (0 < num2)
      {
        Array.Copy((Array) numArray, dataIndex, (Array) buffer, (long) bufferIndex, (long) num2);
      }
      else
      {
        if (length < 0)
          throw new IndexOutOfRangeException(Strings.ADP_InvalidDataLength((object) ((long) length).ToString((IFormatProvider) CultureInfo.InvariantCulture)));
        num2 = 0;
      }
      return (long) num2;
    }

    public override char GetChar(int ordinal) => (char) this.GetValue(ordinal);

    public override long GetChars(
      int ordinal,
      long dataIndex,
      char[] buffer,
      int bufferIndex,
      int length)
    {
      char[] chArray = (char[]) this.GetValue(ordinal);
      if (buffer == null)
        return (long) chArray.Length;
      int num1 = (int) dataIndex;
      int num2 = Math.Min(chArray.Length - num1, length);
      if (num1 < 0)
        throw new ArgumentOutOfRangeException(nameof (bufferIndex), Strings.ADP_InvalidSourceBufferIndex((object) buffer.Length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) ((long) bufferIndex).ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      if (bufferIndex < 0 || bufferIndex > 0 && bufferIndex >= buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (bufferIndex), Strings.ADP_InvalidDestinationBufferIndex((object) buffer.Length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) bufferIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      if (0 < num2)
      {
        Array.Copy((Array) chArray, dataIndex, (Array) buffer, (long) bufferIndex, (long) num2);
      }
      else
      {
        if (length < 0)
          throw new IndexOutOfRangeException(Strings.ADP_InvalidDataLength((object) ((long) length).ToString((IFormatProvider) CultureInfo.InvariantCulture)));
        num2 = 0;
      }
      return (long) num2;
    }

    protected override DbDataReader GetDbDataReader(int ordinal) => throw new NotSupportedException();

    public override string GetDataTypeName(int ordinal) => this.GetFieldType(ordinal).Name;

    public override DateTime GetDateTime(int ordinal) => (DateTime) this.GetValue(ordinal);

    public override Decimal GetDecimal(int ordinal) => (Decimal) this.GetValue(ordinal);

    public override double GetDouble(int ordinal) => (double) this.GetValue(ordinal);

    public override Type GetFieldType(int ordinal) => this._cacheEntry.GetFieldType(ordinal, this._metadata);

    public override float GetFloat(int ordinal) => (float) this.GetValue(ordinal);

    public override Guid GetGuid(int ordinal) => (Guid) this.GetValue(ordinal);

    public override short GetInt16(int ordinal) => (short) this.GetValue(ordinal);

    public override int GetInt32(int ordinal) => (int) this.GetValue(ordinal);

    public override long GetInt64(int ordinal) => (long) this.GetValue(ordinal);

    public override string GetName(int ordinal) => this._cacheEntry.GetCLayerName(ordinal, this._metadata);

    public override int GetOrdinal(string name)
    {
      int ordinalforClayerName = this._cacheEntry.GetOrdinalforCLayerName(name, this._metadata);
      return ordinalforClayerName != -1 ? ordinalforClayerName : throw new ArgumentOutOfRangeException(nameof (name));
    }

    public override string GetString(int ordinal) => (string) this.GetValue(ordinal);

    public override object GetValue(int ordinal) => this._cacheEntry.IsRelationship ? (this._cacheEntry as RelationshipEntry).GetOriginalRelationValue(ordinal) : (this._cacheEntry as EntityEntry).GetOriginalEntityValue(this._metadata, ordinal, this._userObject, ObjectStateValueRecord.OriginalReadonly);

    public override int GetValues(object[] values)
    {
      System.Data.Entity.Utilities.Check.NotNull<object[]>(values, nameof (values));
      int num = Math.Min(values.Length, this.FieldCount);
      for (int i = 0; i < num; ++i)
        values[i] = this.GetValue(i);
      return num;
    }

    public override bool IsDBNull(int ordinal) => this.GetValue(ordinal) == DBNull.Value;

    public DataRecordInfo DataRecordInfo
    {
      get
      {
        if (this._recordInfo == null)
          this._recordInfo = this._cacheEntry.GetDataRecordInfo(this._metadata, this._userObject);
        return this._recordInfo;
      }
    }

    public DbDataRecord GetDataRecord(int ordinal) => (DbDataRecord) this.GetValue(ordinal);

    public DbDataReader GetDataReader(int i) => this.GetDbDataReader(i);
  }
}
