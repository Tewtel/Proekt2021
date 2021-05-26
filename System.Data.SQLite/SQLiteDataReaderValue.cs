// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteDataReaderValue
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents a single value to be returned
  /// from the <see cref="T:System.Data.SQLite.SQLiteDataReader" /> class via
  /// its <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBlob(System.Int32,System.Boolean)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBoolean(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetByte(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChar(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetDateTime(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetDecimal(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetDouble(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetFloat(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetGuid(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetInt16(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetInt32(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetInt64(System.Int32)" />,
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetString(System.Int32)" />, or
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetValue(System.Int32)" /> method.  If the value of the
  /// associated public field of this class is null upon returning from the
  /// callback, the null value will only be used if the return type for the
  /// <see cref="T:System.Data.SQLite.SQLiteDataReader" /> method called is not a value type.
  /// If the value to be returned from the <see cref="T:System.Data.SQLite.SQLiteDataReader" />
  /// method is unsuitable (e.g. null with a value type), an exception will
  /// be thrown.
  /// </summary>
  public sealed class SQLiteDataReaderValue
  {
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBlob(System.Int32,System.Boolean)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public SQLiteBlob BlobValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBoolean(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public bool? BooleanValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetByte(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public byte? ByteValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> method.
    /// </summary>
    public byte[] BytesValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChar(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public char? CharValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> method.
    /// </summary>
    public char[] CharsValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetDateTime(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public DateTime? DateTimeValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetDecimal(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public Decimal? DecimalValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetDouble(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public double? DoubleValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetFloat(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public float? FloatValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetGuid(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public Guid? GuidValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetInt16(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public short? Int16Value;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetInt32(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public int? Int32Value;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetInt64(System.Int32)" /> method -OR- null to
    /// indicate an error.
    /// </summary>
    public long? Int64Value;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetString(System.Int32)" /> method.
    /// </summary>
    public string StringValue;
    /// <summary>
    /// The value to be returned from the
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetValue(System.Int32)" /> method.
    /// </summary>
    public object Value;
  }
}
