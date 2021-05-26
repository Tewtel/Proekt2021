// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBindValueCallback
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This represents a method that will be called in response to a request to
  /// bind a parameter to a command.  If an exception is thrown, it will cause
  /// the parameter binding operation to fail -AND- it will continue to unwind
  /// the call stack.
  /// </summary>
  /// <param name="convert">
  /// The <see cref="T:System.Data.SQLite.SQLiteConvert" /> instance in use.
  /// </param>
  /// <param name="command">
  /// The <see cref="T:System.Data.SQLite.SQLiteCommand" /> instance in use.
  /// </param>
  /// <param name="flags">
  /// The flags associated with the <see cref="T:System.Data.SQLite.SQLiteConnection" /> instance
  /// in use.
  /// </param>
  /// <param name="parameter">
  /// The <see cref="T:System.Data.SQLite.SQLiteParameter" /> instance being bound to the command.
  /// </param>
  /// <param name="typeName">
  /// The database type name associated with this callback.
  /// </param>
  /// <param name="index">
  /// The ordinal of the parameter being bound to the command.
  /// </param>
  /// <param name="userData">
  /// The data originally used when registering this callback.
  /// </param>
  /// <param name="complete">
  /// Non-zero if the default handling for the parameter binding call should
  /// be skipped (i.e. the parameter should not be bound at all).  Great care
  /// should be used when setting this to non-zero.
  /// </param>
  public delegate void SQLiteBindValueCallback(
    SQLiteConvert convert,
    SQLiteCommand command,
    SQLiteConnectionFlags flags,
    SQLiteParameter parameter,
    string typeName,
    int index,
    object userData,
    out bool complete);
}
