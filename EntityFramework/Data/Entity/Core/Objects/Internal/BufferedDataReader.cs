// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.BufferedDataReader
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Resources;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal class BufferedDataReader : DbDataReader
  {
    private DbDataReader _underlyingReader;
    private List<BufferedDataRecord> _bufferedDataRecords = new List<BufferedDataRecord>();
    private BufferedDataRecord _currentResultSet;
    private int _currentResultSetNumber;
    private int _recordsAffected;
    private bool _disposed;
    private bool _isClosed;

    public BufferedDataReader(DbDataReader reader) => this._underlyingReader = reader;

    public override int RecordsAffected => this._recordsAffected;

    public override object this[string name] => throw new NotSupportedException();

    public override object this[int ordinal] => throw new NotSupportedException();

    public override int Depth => throw new NotSupportedException();

    public override int FieldCount
    {
      get
      {
        this.AssertReaderIsOpen();
        return this._currentResultSet.FieldCount;
      }
    }

    public override bool HasRows
    {
      get
      {
        this.AssertReaderIsOpen();
        return this._currentResultSet.HasRows;
      }
    }

    public override bool IsClosed => this._isClosed;

    private void AssertReaderIsOpen()
    {
      if (this._isClosed)
        throw Error.ADP_ClosedDataReaderError();
    }

    private void AssertReaderIsOpenWithData()
    {
      if (this._isClosed)
        throw Error.ADP_ClosedDataReaderError();
      if (!this._currentResultSet.IsDataReady)
        throw Error.ADP_NoData();
    }

    [Conditional("DEBUG")]
    private void AssertFieldIsReady(int ordinal)
    {
      if (this._isClosed)
        throw Error.ADP_ClosedDataReaderError();
      if (!this._currentResultSet.IsDataReady)
        throw Error.ADP_NoData();
      if (0 > ordinal || ordinal > this._currentResultSet.FieldCount)
        throw new IndexOutOfRangeException();
    }

    internal void Initialize(
      string providerManifestToken,
      DbProviderServices providerServices,
      Type[] columnTypes,
      bool[] nullableColumns)
    {
      DbDataReader underlyingReader = this._underlyingReader;
      if (underlyingReader == null)
        return;
      this._underlyingReader = (DbDataReader) null;
      try
      {
        if (columnTypes != null && underlyingReader.GetType().Name != "SqlDataReader")
          this._bufferedDataRecords.Add(ShapedBufferedDataRecord.Initialize(providerManifestToken, providerServices, underlyingReader, columnTypes, nullableColumns));
        else
          this._bufferedDataRecords.Add((BufferedDataRecord) ShapelessBufferedDataRecord.Initialize(providerManifestToken, providerServices, underlyingReader));
        while (underlyingReader.NextResult())
          this._bufferedDataRecords.Add((BufferedDataRecord) ShapelessBufferedDataRecord.Initialize(providerManifestToken, providerServices, underlyingReader));
        this._recordsAffected = underlyingReader.RecordsAffected;
        this._currentResultSet = this._bufferedDataRecords[this._currentResultSetNumber];
      }
      finally
      {
        underlyingReader.Dispose();
      }
    }

    internal async Task InitializeAsync(
      string providerManifestToken,
      DbProviderServices providerServices,
      Type[] columnTypes,
      bool[] nullableColumns,
      CancellationToken cancellationToken)
    {
      if (this._underlyingReader == null)
        return;
      cancellationToken.ThrowIfCancellationRequested();
      DbDataReader reader = this._underlyingReader;
      this._underlyingReader = (DbDataReader) null;
      try
      {
        List<BufferedDataRecord> bufferedDataRecordList;
        System.Data.Entity.Utilities.TaskExtensions.CultureAwaiter<ShapelessBufferedDataRecord> cultureAwaiter;
        if (columnTypes != null && reader.GetType().Name != "SqlDataReader")
        {
          bufferedDataRecordList = this._bufferedDataRecords;
          bufferedDataRecordList.Add(await ShapedBufferedDataRecord.InitializeAsync(providerManifestToken, providerServices, reader, columnTypes, nullableColumns, cancellationToken).WithCurrentCulture<BufferedDataRecord>());
          bufferedDataRecordList = (List<BufferedDataRecord>) null;
        }
        else
        {
          bufferedDataRecordList = this._bufferedDataRecords;
          cultureAwaiter = ShapelessBufferedDataRecord.InitializeAsync(providerManifestToken, providerServices, reader, cancellationToken).WithCurrentCulture<ShapelessBufferedDataRecord>();
          bufferedDataRecordList.Add((BufferedDataRecord) await cultureAwaiter);
          bufferedDataRecordList = (List<BufferedDataRecord>) null;
        }
        while (true)
        {
          if (await reader.NextResultAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            bufferedDataRecordList = this._bufferedDataRecords;
            cultureAwaiter = ShapelessBufferedDataRecord.InitializeAsync(providerManifestToken, providerServices, reader, cancellationToken).WithCurrentCulture<ShapelessBufferedDataRecord>();
            bufferedDataRecordList.Add((BufferedDataRecord) await cultureAwaiter);
            bufferedDataRecordList = (List<BufferedDataRecord>) null;
          }
          else
            break;
        }
        this._recordsAffected = reader.RecordsAffected;
        this._currentResultSet = this._bufferedDataRecords[this._currentResultSetNumber];
      }
      finally
      {
        reader.Dispose();
      }
    }

    public override void Close()
    {
      this._bufferedDataRecords = (List<BufferedDataRecord>) null;
      this._isClosed = true;
      DbDataReader underlyingReader = this._underlyingReader;
      if (underlyingReader == null)
        return;
      this._underlyingReader = (DbDataReader) null;
      underlyingReader.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      if (!this._disposed & disposing && !this.IsClosed)
        this.Close();
      this._disposed = true;
      base.Dispose(disposing);
    }

    public override bool GetBoolean(int ordinal) => this._currentResultSet.GetBoolean(ordinal);

    public override byte GetByte(int ordinal) => this._currentResultSet.GetByte(ordinal);

    public override long GetBytes(
      int ordinal,
      long dataOffset,
      byte[] buffer,
      int bufferOffset,
      int length)
    {
      throw new NotSupportedException();
    }

    public override char GetChar(int ordinal) => this._currentResultSet.GetChar(ordinal);

    public override long GetChars(
      int ordinal,
      long dataOffset,
      char[] buffer,
      int bufferOffset,
      int length)
    {
      throw new NotSupportedException();
    }

    public override DateTime GetDateTime(int ordinal) => this._currentResultSet.GetDateTime(ordinal);

    public override Decimal GetDecimal(int ordinal) => this._currentResultSet.GetDecimal(ordinal);

    public override double GetDouble(int ordinal) => this._currentResultSet.GetDouble(ordinal);

    public override float GetFloat(int ordinal) => this._currentResultSet.GetFloat(ordinal);

    public override Guid GetGuid(int ordinal) => this._currentResultSet.GetGuid(ordinal);

    public override short GetInt16(int ordinal) => this._currentResultSet.GetInt16(ordinal);

    public override int GetInt32(int ordinal) => this._currentResultSet.GetInt32(ordinal);

    public override long GetInt64(int ordinal) => this._currentResultSet.GetInt64(ordinal);

    public override string GetString(int ordinal) => this._currentResultSet.GetString(ordinal);

    public override T GetFieldValue<T>(int ordinal) => this._currentResultSet.GetFieldValue<T>(ordinal);

    public override Task<T> GetFieldValueAsync<T>(
      int ordinal,
      CancellationToken cancellationToken)
    {
      return this._currentResultSet.GetFieldValueAsync<T>(ordinal, cancellationToken);
    }

    public override object GetValue(int ordinal) => this._currentResultSet.GetValue(ordinal);

    public override int GetValues(object[] values)
    {
      System.Data.Entity.Utilities.Check.NotNull<object[]>(values, nameof (values));
      this.AssertReaderIsOpenWithData();
      return this._currentResultSet.GetValues(values);
    }

    public override string GetDataTypeName(int ordinal)
    {
      this.AssertReaderIsOpen();
      return this._currentResultSet.GetDataTypeName(ordinal);
    }

    public override Type GetFieldType(int ordinal)
    {
      this.AssertReaderIsOpen();
      return this._currentResultSet.GetFieldType(ordinal);
    }

    public override string GetName(int ordinal)
    {
      this.AssertReaderIsOpen();
      return this._currentResultSet.GetName(ordinal);
    }

    public override int GetOrdinal(string name)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(name, nameof (name));
      this.AssertReaderIsOpen();
      return this._currentResultSet.GetOrdinal(name);
    }

    public override bool IsDBNull(int ordinal) => this._currentResultSet.IsDBNull(ordinal);

    public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken) => this._currentResultSet.IsDBNullAsync(ordinal, cancellationToken);

    public override IEnumerator GetEnumerator() => (IEnumerator) new DbEnumerator((IDataReader) this);

    public override DataTable GetSchemaTable() => throw new NotSupportedException();

    public override bool NextResult()
    {
      this.AssertReaderIsOpen();
      if (++this._currentResultSetNumber < this._bufferedDataRecords.Count)
      {
        this._currentResultSet = this._bufferedDataRecords[this._currentResultSetNumber];
        return true;
      }
      this._currentResultSet = (BufferedDataRecord) null;
      return false;
    }

    public override Task<bool> NextResultAsync(CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return Task.FromResult<bool>(this.NextResult());
    }

    public override bool Read()
    {
      this.AssertReaderIsOpen();
      return this._currentResultSet.Read();
    }

    public override Task<bool> ReadAsync(CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      this.AssertReaderIsOpen();
      return this._currentResultSet.ReadAsync(cancellationToken);
    }
  }
}
