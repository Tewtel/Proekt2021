// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.UpdateEventType
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Whenever an update event is triggered on a connection, this enum will indicate
  /// exactly what type of operation is being performed.
  /// </summary>
  public enum UpdateEventType
  {
    /// <summary>
    /// A row is being deleted from the given database and table
    /// </summary>
    Delete = 9,
    /// <summary>A row is being inserted into the table.</summary>
    Insert = 18, // 0x00000012
    /// <summary>A row is being updated in the table.</summary>
    Update = 23, // 0x00000017
  }
}
