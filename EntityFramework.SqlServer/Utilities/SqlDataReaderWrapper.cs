// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.SqlDataReaderWrapper
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace System.Data.Entity.SqlServer.Utilities
{
  internal class SqlDataReaderWrapper : MarshalByRefObject
  {
    private readonly SqlDataReader _sqlDataReader;

    protected SqlDataReaderWrapper()
    {
    }

    public SqlDataReaderWrapper(SqlDataReader sqlDataReader) => this._sqlDataReader = sqlDataReader;

    public virtual IDataReader GetData(int i) => ((IDataRecord) this._sqlDataReader).GetData(i);

    public virtual void Dispose() => this._sqlDataReader.Dispose();

    public virtual Task<T> GetFieldValueAsync<T>(int ordinal) => this._sqlDataReader.GetFieldValueAsync<T>(ordinal);

    public virtual Task<bool> IsDBNullAsync(int ordinal) => this._sqlDataReader.IsDBNullAsync(ordinal);

    public virtual Task<bool> ReadAsync() => this._sqlDataReader.ReadAsync();

    public virtual Task<bool> NextResultAsync() => this._sqlDataReader.NextResultAsync();

    public virtual void Close() => this._sqlDataReader.Close();

    public virtual string GetDataTypeName(int i) => this._sqlDataReader.GetDataTypeName(i);

    public virtual IEnumerator GetEnumerator() => this._sqlDataReader.GetEnumerator();

    public virtual Type GetFieldType(int i) => this._sqlDataReader.GetFieldType(i);

    public virtual string GetName(int i) => this._sqlDataReader.GetName(i);

    public virtual Type GetProviderSpecificFieldType(int i) => this._sqlDataReader.GetProviderSpecificFieldType(i);

    public virtual int GetOrdinal(string name) => this._sqlDataReader.GetOrdinal(name);

    public virtual object GetProviderSpecificValue(int i) => this._sqlDataReader.GetProviderSpecificValue(i);

    public virtual int GetProviderSpecificValues(object[] values) => this._sqlDataReader.GetProviderSpecificValues(values);

    public virtual DataTable GetSchemaTable() => this._sqlDataReader.GetSchemaTable();

    public virtual bool GetBoolean(int i) => this._sqlDataReader.GetBoolean(i);

    public virtual XmlReader GetXmlReader(int i) => this._sqlDataReader.GetXmlReader(i);

    public virtual Stream GetStream(int i) => this._sqlDataReader.GetStream(i);

    public virtual byte GetByte(int i) => this._sqlDataReader.GetByte(i);

    public virtual long GetBytes(
      int i,
      long dataIndex,
      byte[] buffer,
      int bufferIndex,
      int length)
    {
      return this._sqlDataReader.GetBytes(i, dataIndex, buffer, bufferIndex, length);
    }

    public virtual TextReader GetTextReader(int i) => this._sqlDataReader.GetTextReader(i);

    public virtual char GetChar(int i) => this._sqlDataReader.GetChar(i);

    public virtual long GetChars(
      int i,
      long dataIndex,
      char[] buffer,
      int bufferIndex,
      int length)
    {
      return this._sqlDataReader.GetChars(i, dataIndex, buffer, bufferIndex, length);
    }

    public virtual DateTime GetDateTime(int i) => this._sqlDataReader.GetDateTime(i);

    public virtual Decimal GetDecimal(int i) => this._sqlDataReader.GetDecimal(i);

    public virtual double GetDouble(int i) => this._sqlDataReader.GetDouble(i);

    public virtual float GetFloat(int i) => this._sqlDataReader.GetFloat(i);

    public virtual Guid GetGuid(int i) => this._sqlDataReader.GetGuid(i);

    public virtual short GetInt16(int i) => this._sqlDataReader.GetInt16(i);

    public virtual int GetInt32(int i) => this._sqlDataReader.GetInt32(i);

    public virtual long GetInt64(int i) => this._sqlDataReader.GetInt64(i);

    public virtual SqlBoolean GetSqlBoolean(int i) => this._sqlDataReader.GetSqlBoolean(i);

    public virtual SqlBinary GetSqlBinary(int i) => this._sqlDataReader.GetSqlBinary(i);

    public virtual SqlByte GetSqlByte(int i) => this._sqlDataReader.GetSqlByte(i);

    public virtual SqlBytes GetSqlBytes(int i) => this._sqlDataReader.GetSqlBytes(i);

    public virtual SqlChars GetSqlChars(int i) => this._sqlDataReader.GetSqlChars(i);

    public virtual SqlDateTime GetSqlDateTime(int i) => this._sqlDataReader.GetSqlDateTime(i);

    public virtual SqlDecimal GetSqlDecimal(int i) => this._sqlDataReader.GetSqlDecimal(i);

    public virtual SqlGuid GetSqlGuid(int i) => this._sqlDataReader.GetSqlGuid(i);

    public virtual SqlDouble GetSqlDouble(int i) => this._sqlDataReader.GetSqlDouble(i);

    public virtual SqlInt16 GetSqlInt16(int i) => this._sqlDataReader.GetSqlInt16(i);

    public virtual SqlInt32 GetSqlInt32(int i) => this._sqlDataReader.GetSqlInt32(i);

    public virtual SqlInt64 GetSqlInt64(int i) => this._sqlDataReader.GetSqlInt64(i);

    public virtual SqlMoney GetSqlMoney(int i) => this._sqlDataReader.GetSqlMoney(i);

    public virtual SqlSingle GetSqlSingle(int i) => this._sqlDataReader.GetSqlSingle(i);

    public virtual SqlString GetSqlString(int i) => this._sqlDataReader.GetSqlString(i);

    public virtual SqlXml GetSqlXml(int i) => this._sqlDataReader.GetSqlXml(i);

    public virtual object GetSqlValue(int i) => this._sqlDataReader.GetSqlValue(i);

    public virtual int GetSqlValues(object[] values) => this._sqlDataReader.GetSqlValues(values);

    public virtual string GetString(int i) => this._sqlDataReader.GetString(i);

    public virtual T GetFieldValue<T>(int i) => this._sqlDataReader.GetFieldValue<T>(i);

    public virtual object GetValue(int i) => this._sqlDataReader.GetValue(i);

    public virtual TimeSpan GetTimeSpan(int i) => this._sqlDataReader.GetTimeSpan(i);

    public virtual DateTimeOffset GetDateTimeOffset(int i) => this._sqlDataReader.GetDateTimeOffset(i);

    public virtual int GetValues(object[] values) => this._sqlDataReader.GetValues(values);

    public virtual bool IsDBNull(int i) => this._sqlDataReader.IsDBNull(i);

    public virtual bool NextResult() => this._sqlDataReader.NextResult();

    public virtual bool Read() => this._sqlDataReader.Read();

    public virtual Task<bool> NextResultAsync(CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return this._sqlDataReader.NextResultAsync(cancellationToken);
    }

    public virtual Task<bool> ReadAsync(CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return this._sqlDataReader.ReadAsync(cancellationToken);
    }

    public virtual Task<bool> IsDBNullAsync(int i, CancellationToken cancellationToken) => this._sqlDataReader.IsDBNullAsync(i, cancellationToken);

    public virtual Task<T> GetFieldValueAsync<T>(int i, CancellationToken cancellationToken) => this._sqlDataReader.GetFieldValueAsync<T>(i, cancellationToken);

    public virtual int Depth => this._sqlDataReader.Depth;

    public virtual int FieldCount => this._sqlDataReader.FieldCount;

    public virtual bool HasRows => this._sqlDataReader.HasRows;

    public virtual bool IsClosed => this._sqlDataReader.IsClosed;

    public virtual int RecordsAffected => this._sqlDataReader.RecordsAffected;

    public virtual int VisibleFieldCount => this._sqlDataReader.VisibleFieldCount;

    public virtual object this[int i] => this._sqlDataReader[i];

    public virtual object this[string name] => this._sqlDataReader[name];
  }
}
