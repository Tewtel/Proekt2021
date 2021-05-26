// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteReadValueCallback
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This represents a method that will be called in response to a request
  /// to read a value from a data reader.  If an exception is thrown, it will
  /// cause the data reader operation to fail -AND- it will continue to unwind
  /// the call stack.
  /// </summary>
  /// <param name="convert">
  /// The <see cref="T:System.Data.SQLite.SQLiteConvert" /> instance in use.
  /// </param>
  /// <param name="dataReader">
  /// The <see cref="T:System.Data.SQLite.SQLiteDataReader" /> instance in use.
  /// </param>
  /// <param name="flags">
  /// The flags associated with the <see cref="T:System.Data.SQLite.SQLiteConnection" /> instance
  /// in use.
  /// </param>
  /// <param name="eventArgs">
  /// The parameter and return type data for the column being read from the
  /// data reader.
  /// </param>
  /// <param name="typeName">
  /// The database type name associated with this callback.
  /// </param>
  /// <param name="index">
  /// The zero based index of the column being read from the data reader.
  /// </param>
  /// <param name="userData">
  /// The data originally used when registering this callback.
  /// </param>
  /// <param name="complete">
  /// Non-zero if the default handling for the data reader call should be
  /// skipped.  If this is set to non-zero and the necessary return value
  /// is unavailable or unsuitable, an exception will be thrown.
  /// </param>
  public delegate void SQLiteReadValueCallback(
    SQLiteConvert convert,
    SQLiteDataReader dataReader,
    SQLiteConnectionFlags flags,
    SQLiteReadEventArgs eventArgs,
    string typeName,
    int index,
    object userData,
    out bool complete);
}
