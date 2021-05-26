// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteExecuteType
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// The requested command execution type.  This controls which method of the
  /// <see cref="T:System.Data.SQLite.SQLiteCommand" /> object will be called.
  /// </summary>
  public enum SQLiteExecuteType
  {
    /// <summary>Do nothing.  No method will be called.</summary>
    None = 0,
    /// <summary>
    /// Use the default command execution type.  Using this value is the same
    /// as using the <see cref="F:System.Data.SQLite.SQLiteExecuteType.NonQuery" /> value.
    /// </summary>
    Default = 1,
    /// <summary>
    /// The command is not expected to return a result -OR- the result is not
    /// needed.  The <see cref="M:System.Data.SQLite.SQLiteCommand.ExecuteNonQuery" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteCommand.ExecuteNonQuery(System.Data.CommandBehavior)" />  method
    /// will be called.
    /// </summary>
    NonQuery = 1,
    /// <summary>
    /// The command is expected to return a scalar result -OR- the result should
    /// be limited to a scalar result.  The <see cref="M:System.Data.SQLite.SQLiteCommand.ExecuteScalar" />
    /// or <see cref="M:System.Data.SQLite.SQLiteCommand.ExecuteScalar(System.Data.CommandBehavior)" /> method will
    /// be called.
    /// </summary>
    Scalar = 2,
    /// <summary>
    /// The command is expected to return <see cref="T:System.Data.SQLite.SQLiteDataReader" /> result.
    /// The <see cref="M:System.Data.SQLite.SQLiteCommand.ExecuteReader" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteCommand.ExecuteReader(System.Data.CommandBehavior)" /> method will
    /// be called.
    /// </summary>
    Reader = 3,
  }
}
