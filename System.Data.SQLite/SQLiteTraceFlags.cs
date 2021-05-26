// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteTraceFlags
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// These constants are used with the sqlite3_trace_v2() API and the
  /// callbacks registered by it.
  /// </summary>
  [Flags]
  internal enum SQLiteTraceFlags
  {
    SQLITE_TRACE_NONE = 0,
    SQLITE_TRACE_STMT = 1,
    SQLITE_TRACE_PROFILE = 2,
    SQLITE_TRACE_ROW = 4,
    SQLITE_TRACE_CLOSE = 8,
  }
}
