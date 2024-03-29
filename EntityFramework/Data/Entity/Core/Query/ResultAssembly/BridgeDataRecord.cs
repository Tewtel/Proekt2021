﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.ResultAssembly.BridgeDataRecord
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.Internal.Materialization;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Core.Query.ResultAssembly
{
  internal sealed class BridgeDataRecord : DbDataRecord, IExtendedDataRecord, IDataRecord
  {
    internal readonly int Depth;
    private readonly Shaper<RecordState> _shaper;
    private RecordState _source;
    private BridgeDataRecord.Status _status;
    private int _lastColumnRead;
    private long _lastDataOffsetRead;
    private int _lastOrdinalCheckedForNull;
    private object _lastValueCheckedForNull;
    private BridgeDataReader _currentNestedReader;
    private BridgeDataRecord _currentNestedRecord;

    internal BridgeDataRecord(Shaper<RecordState> shaper, int depth)
    {
      this._shaper = shaper;
      this.Depth = depth;
    }

    internal void CloseExplicitly() => this.Close<object>(BridgeDataRecord.Status.ClosedExplicitly, new Func<object>(this.CloseNestedObjectImplicitly));

    internal Task CloseExplicitlyAsync(CancellationToken cancellationToken) => this.Close<Task>(BridgeDataRecord.Status.ClosedExplicitly, (Func<Task>) (() => this.CloseNestedObjectImplicitlyAsync(cancellationToken)));

    internal void CloseImplicitly() => this.Close<object>(BridgeDataRecord.Status.ClosedImplicitly, new Func<object>(this.CloseNestedObjectImplicitly));

    internal Task CloseImplicitlyAsync(CancellationToken cancellationToken) => this.Close<Task>(BridgeDataRecord.Status.ClosedImplicitly, (Func<Task>) (() => this.CloseNestedObjectImplicitlyAsync(cancellationToken)));

    private T Close<T>(BridgeDataRecord.Status status, Func<T> close)
    {
      this._status = status;
      this._source = (RecordState) null;
      return close();
    }

    private object CloseNestedObjectImplicitly()
    {
      BridgeDataRecord currentNestedRecord = this._currentNestedRecord;
      if (currentNestedRecord != null)
      {
        this._currentNestedRecord = (BridgeDataRecord) null;
        currentNestedRecord.CloseImplicitly();
      }
      BridgeDataReader currentNestedReader = this._currentNestedReader;
      if (currentNestedReader != null)
      {
        this._currentNestedReader = (BridgeDataReader) null;
        currentNestedReader.CloseImplicitly();
      }
      return (object) null;
    }

    private async Task CloseNestedObjectImplicitlyAsync(CancellationToken cancellationToken)
    {
      BridgeDataRecord currentNestedRecord = this._currentNestedRecord;
      if (currentNestedRecord != null)
      {
        this._currentNestedRecord = (BridgeDataRecord) null;
        await currentNestedRecord.CloseImplicitlyAsync(cancellationToken).WithCurrentCulture();
      }
      BridgeDataReader currentNestedReader = this._currentNestedReader;
      if (currentNestedReader == null)
        return;
      this._currentNestedReader = (BridgeDataReader) null;
      await currentNestedReader.CloseImplicitlyAsync(cancellationToken).WithCurrentCulture();
    }

    internal void SetRecordSource(RecordState newSource, bool hasData)
    {
      this._source = !hasData ? (RecordState) null : newSource;
      this._status = BridgeDataRecord.Status.Open;
      this._lastColumnRead = -1;
      this._lastDataOffsetRead = -1L;
      this._lastOrdinalCheckedForNull = -1;
      this._lastValueCheckedForNull = (object) null;
    }

    private void AssertReaderIsOpen()
    {
      if (this.IsExplicitlyClosed)
        throw Error.ADP_ClosedDataReaderError();
      if (this.IsImplicitlyClosed)
        throw Error.ADP_ImplicitlyClosedDataReaderError();
    }

    private void AssertReaderIsOpenWithData()
    {
      this.AssertReaderIsOpen();
      if (!this.HasData)
        throw Error.ADP_NoData();
    }

    private void AssertSequentialAccess(int ordinal)
    {
      if (ordinal < 0 || ordinal >= this._source.ColumnCount)
        throw new ArgumentOutOfRangeException(nameof (ordinal));
      this._lastColumnRead = this._lastColumnRead < ordinal ? ordinal : throw new InvalidOperationException(Strings.ADP_NonSequentialColumnAccess((object) ordinal.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) (this._lastColumnRead + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      this._lastDataOffsetRead = long.MaxValue;
    }

    private void AssertSequentialAccess(int ordinal, long dataOffset, string methodName)
    {
      if (ordinal < 0 || ordinal >= this._source.ColumnCount)
        throw new ArgumentOutOfRangeException(nameof (ordinal));
      if (this._lastColumnRead > ordinal || this._lastColumnRead == ordinal && this._lastDataOffsetRead == long.MaxValue)
        throw new InvalidOperationException(Strings.ADP_NonSequentialColumnAccess((object) ordinal.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) (this._lastColumnRead + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      if (this._lastColumnRead == ordinal)
      {
        if (this._lastDataOffsetRead >= dataOffset)
          throw new InvalidOperationException(Strings.ADP_NonSequentialChunkAccess((object) dataOffset.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) (this._lastDataOffsetRead + 1L).ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) methodName));
      }
      else
      {
        this._lastColumnRead = ordinal;
        this._lastDataOffsetRead = -1L;
      }
    }

    internal bool HasData => this._source != null;

    internal bool IsClosed => (uint) this._status > 0U;

    internal bool IsExplicitlyClosed => this._status == BridgeDataRecord.Status.ClosedExplicitly;

    internal bool IsImplicitlyClosed => this._status == BridgeDataRecord.Status.ClosedImplicitly;

    public DataRecordInfo DataRecordInfo
    {
      get
      {
        this.AssertReaderIsOpen();
        return this._source.DataRecordInfo;
      }
    }

    public override int FieldCount
    {
      get
      {
        this.AssertReaderIsOpen();
        return this._source.ColumnCount;
      }
    }

    private TypeUsage GetTypeUsage(int ordinal)
    {
      if (ordinal < 0 || ordinal >= this._source.ColumnCount)
        throw new ArgumentOutOfRangeException(nameof (ordinal));
      return !(this._source.CurrentColumnValues[ordinal] is RecordState currentColumnValue) ? this._source.GetTypeUsage(ordinal) : currentColumnValue.DataRecordInfo.RecordType;
    }

    public override string GetDataTypeName(int ordinal)
    {
      this.AssertReaderIsOpenWithData();
      return this.GetTypeUsage(ordinal).ToString();
    }

    public override Type GetFieldType(int ordinal)
    {
      this.AssertReaderIsOpenWithData();
      return BridgeDataReader.GetClrTypeFromTypeMetadata(this.GetTypeUsage(ordinal));
    }

    public override string GetName(int ordinal)
    {
      this.AssertReaderIsOpen();
      return this._source.GetName(ordinal);
    }

    public override int GetOrdinal(string name)
    {
      this.AssertReaderIsOpen();
      return this._source.GetOrdinal(name);
    }

    public override object this[int ordinal] => this.GetValue(ordinal);

    public override object this[string name] => this.GetValue(this.GetOrdinal(name));

    public override object GetValue(int ordinal)
    {
      this.AssertReaderIsOpenWithData();
      this.AssertSequentialAccess(ordinal);
      object result;
      if (ordinal == this._lastOrdinalCheckedForNull)
      {
        result = this._lastValueCheckedForNull;
      }
      else
      {
        this._lastOrdinalCheckedForNull = -1;
        this._lastValueCheckedForNull = (object) null;
        this.CloseNestedObjectImplicitly();
        result = this._source.CurrentColumnValues[ordinal];
        if (this._source.IsNestedObject(ordinal))
          result = this.GetNestedObjectValue(result);
      }
      return result;
    }

    private object GetNestedObjectValue(object result)
    {
      if (result != DBNull.Value)
      {
        switch (result)
        {
          case RecordState newSource2:
            if (newSource2.IsNull)
            {
              result = (object) DBNull.Value;
              break;
            }
            BridgeDataRecord bridgeDataRecord = new BridgeDataRecord(this._shaper, this.Depth + 1);
            bridgeDataRecord.SetRecordSource(newSource2, true);
            result = (object) bridgeDataRecord;
            this._currentNestedRecord = bridgeDataRecord;
            this._currentNestedReader = (BridgeDataReader) null;
            break;
          case Coordinator<RecordState> coordinator2:
            BridgeDataReader bridgeDataReader = new BridgeDataReader(this._shaper, coordinator2.TypedCoordinatorFactory, this.Depth + 1, (IEnumerator<KeyValuePair<Shaper<RecordState>, CoordinatorFactory<RecordState>>>) null);
            result = (object) bridgeDataReader;
            this._currentNestedRecord = (BridgeDataRecord) null;
            this._currentNestedReader = bridgeDataReader;
            break;
        }
      }
      return result;
    }

    public override int GetValues(object[] values)
    {
      System.Data.Entity.Utilities.Check.NotNull<object[]>(values, nameof (values));
      int num = Math.Min(values.Length, this.FieldCount);
      for (int i = 0; i < num; ++i)
        values[i] = this.GetValue(i);
      return num;
    }

    public override bool GetBoolean(int ordinal) => (bool) this.GetValue(ordinal);

    public override byte GetByte(int ordinal) => (byte) this.GetValue(ordinal);

    public override char GetChar(int ordinal) => (char) this.GetValue(ordinal);

    public override DateTime GetDateTime(int ordinal) => (DateTime) this.GetValue(ordinal);

    public override Decimal GetDecimal(int ordinal) => (Decimal) this.GetValue(ordinal);

    public override double GetDouble(int ordinal) => (double) this.GetValue(ordinal);

    public override float GetFloat(int ordinal) => (float) this.GetValue(ordinal);

    public override Guid GetGuid(int ordinal) => (Guid) this.GetValue(ordinal);

    public override short GetInt16(int ordinal) => (short) this.GetValue(ordinal);

    public override int GetInt32(int ordinal) => (int) this.GetValue(ordinal);

    public override long GetInt64(int ordinal) => (long) this.GetValue(ordinal);

    public override string GetString(int ordinal) => (string) this.GetValue(ordinal);

    public override bool IsDBNull(int ordinal)
    {
      object obj = this.GetValue(ordinal);
      --this._lastColumnRead;
      this._lastDataOffsetRead = -1L;
      this._lastValueCheckedForNull = obj;
      this._lastOrdinalCheckedForNull = ordinal;
      return DBNull.Value == obj;
    }

    public override long GetBytes(
      int ordinal,
      long dataOffset,
      byte[] buffer,
      int bufferOffset,
      int length)
    {
      this.AssertReaderIsOpenWithData();
      this.AssertSequentialAccess(ordinal, dataOffset, nameof (GetBytes));
      long bytes = this._source.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length);
      if (buffer != null)
        this._lastDataOffsetRead = dataOffset + bytes - 1L;
      return bytes;
    }

    public override long GetChars(
      int ordinal,
      long dataOffset,
      char[] buffer,
      int bufferOffset,
      int length)
    {
      this.AssertReaderIsOpenWithData();
      this.AssertSequentialAccess(ordinal, dataOffset, nameof (GetChars));
      long chars = this._source.GetChars(ordinal, dataOffset, buffer, bufferOffset, length);
      if (buffer != null)
        this._lastDataOffsetRead = dataOffset + chars - 1L;
      return chars;
    }

    protected override DbDataReader GetDbDataReader(int ordinal) => (DbDataReader) this.GetValue(ordinal);

    public DbDataRecord GetDataRecord(int ordinal) => (DbDataRecord) this.GetValue(ordinal);

    public DbDataReader GetDataReader(int ordinal) => this.GetDbDataReader(ordinal);

    private enum Status
    {
      Open,
      ClosedImplicitly,
      ClosedExplicitly,
    }
  }
}
