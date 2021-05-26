// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteAuthorizerEventHandler
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Raised when authorization is required to perform an action contained
  /// within a SQL query.
  /// </summary>
  /// <param name="sender">The connection performing the action.</param>
  /// <param name="e">A <see cref="T:System.Data.SQLite.AuthorizerEventArgs" /> that contains the
  /// event data.</param>
  public delegate void SQLiteAuthorizerEventHandler(object sender, AuthorizerEventArgs e);
}
