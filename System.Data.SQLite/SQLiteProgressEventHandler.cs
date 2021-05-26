// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteProgressEventHandler
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Raised each time the number of virtual machine instructions is
  /// approximately equal to the value of the
  /// <see cref="P:System.Data.SQLite.SQLiteConnection.ProgressOps" /> property.
  /// </summary>
  /// <param name="sender">The connection performing the operation.</param>
  /// <param name="e">A <see cref="T:System.Data.SQLite.ProgressEventArgs" /> that contains the
  /// event data.</param>
  public delegate void SQLiteProgressEventHandler(object sender, ProgressEventArgs e);
}
