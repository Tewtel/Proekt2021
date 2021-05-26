// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteReadValueEventArgs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the parameters and return values for the
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBlob(System.Int32,System.Boolean)" />,
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
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetString(System.Int32)" />, and
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetValue(System.Int32)" /> methods.
  /// </summary>
  public class SQLiteReadValueEventArgs : SQLiteReadEventArgs
  {
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteReadValueEventArgs.MethodName" /> property.
    /// </summary>
    private string methodName;
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteReadValueEventArgs.ExtraEventArgs" /> property.
    /// </summary>
    private SQLiteReadEventArgs extraEventArgs;
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteReadValueEventArgs.Value" /> property.
    /// </summary>
    private SQLiteDataReaderValue value;

    /// <summary>
    /// Constructs a new instance of this class.  Depending on the method
    /// being called, the <paramref name="extraEventArgs" /> and/or
    /// <paramref name="value" /> parameters may be null.
    /// </summary>
    /// <param name="methodName">
    /// The name of the <see cref="T:System.Data.SQLite.SQLiteDataReader" /> method that was
    /// responsible for invoking this callback.
    /// </param>
    /// <param name="extraEventArgs">
    /// If the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> method is being called,
    /// this object will contain the array related parameters for that
    /// method.  If the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBlob(System.Int32,System.Boolean)" /> method is
    /// being called, this object will contain the blob related parameters
    /// for that method.
    /// </param>
    /// <param name="value">
    /// This may be used by the callback to set the return value for the
    /// called <see cref="T:System.Data.SQLite.SQLiteDataReader" /> method.
    /// </param>
    internal SQLiteReadValueEventArgs(
      string methodName,
      SQLiteReadEventArgs extraEventArgs,
      SQLiteDataReaderValue value)
    {
      this.methodName = methodName;
      this.extraEventArgs = extraEventArgs;
      this.value = value;
    }

    /// <summary>
    /// The name of the <see cref="T:System.Data.SQLite.SQLiteDataReader" /> method that was
    /// responsible for invoking this callback.
    /// </summary>
    public string MethodName => this.methodName;

    /// <summary>
    /// If the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> method is being called,
    /// this object will contain the array related parameters for that
    /// method.  If the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBlob(System.Int32,System.Boolean)" /> method is
    /// being called, this object will contain the blob related parameters
    /// for that method.
    /// </summary>
    public SQLiteReadEventArgs ExtraEventArgs => this.extraEventArgs;

    /// <summary>
    /// This may be used by the callback to set the return value for the
    /// called <see cref="T:System.Data.SQLite.SQLiteDataReader" /> method.
    /// </summary>
    public SQLiteDataReaderValue Value => this.value;
  }
}
