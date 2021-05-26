// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionEventHandler
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Raised when an event pertaining to a connection occurs.
  /// </summary>
  /// <param name="sender">The connection involved.</param>
  /// <param name="e">Extra information about the event.</param>
  public delegate void SQLiteConnectionEventHandler(object sender, ConnectionEventArgs e);
}
