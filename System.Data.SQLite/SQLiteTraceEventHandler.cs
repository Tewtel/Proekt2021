﻿// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteTraceEventHandler
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Raised when a statement first begins executing on a given connection
  /// </summary>
  /// <param name="sender">The connection executing the statement</param>
  /// <param name="e">Event arguments of the trace</param>
  public delegate void SQLiteTraceEventHandler(object sender, TraceEventArgs e);
}
