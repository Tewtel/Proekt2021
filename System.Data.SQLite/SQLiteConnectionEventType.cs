// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionEventType
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// These are the event types associated with the
  /// <see cref="T:System.Data.SQLite.SQLiteConnectionEventHandler" />
  /// delegate (and its corresponding event) and the
  /// <see cref="T:System.Data.SQLite.ConnectionEventArgs" /> class.
  /// </summary>
  public enum SQLiteConnectionEventType
  {
    /// <summary>Not used.</summary>
    Invalid = -1, // 0xFFFFFFFF
    /// <summary>Not used.</summary>
    Unknown = 0,
    /// <summary>The connection is being opened.</summary>
    Opening = 1,
    /// <summary>The connection string has been parsed.</summary>
    ConnectionString = 2,
    /// <summary>The connection was opened.</summary>
    Opened = 3,
    /// <summary>
    /// The <see cref="F:System.Data.SQLite.SQLiteConnectionEventType.ChangeDatabase" /> method was called on the
    /// connection.
    /// </summary>
    ChangeDatabase = 4,
    /// <summary>A transaction was created using the connection.</summary>
    NewTransaction = 5,
    /// <summary>The connection was enlisted into a transaction.</summary>
    EnlistTransaction = 6,
    /// <summary>A command was created using the connection.</summary>
    NewCommand = 7,
    /// <summary>A data reader was created using the connection.</summary>
    NewDataReader = 8,
    /// <summary>
    /// An instance of a <see cref="T:System.Runtime.InteropServices.CriticalHandle" /> derived class has
    /// been created to wrap a native resource.
    /// </summary>
    NewCriticalHandle = 9,
    /// <summary>The connection is being closed.</summary>
    Closing = 10, // 0x0000000A
    /// <summary>The connection was closed.</summary>
    Closed = 11, // 0x0000000B
    /// <summary>A command is being disposed.</summary>
    DisposingCommand = 12, // 0x0000000C
    /// <summary>A data reader is being disposed.</summary>
    DisposingDataReader = 13, // 0x0000000D
    /// <summary>A data reader is being closed.</summary>
    ClosingDataReader = 14, // 0x0000000E
    /// <summary>
    /// A native resource was opened (i.e. obtained) from the pool.
    /// </summary>
    OpenedFromPool = 15, // 0x0000000F
    /// <summary>
    /// A native resource was closed (i.e. released) to the pool.
    /// </summary>
    ClosedToPool = 16, // 0x00000010
  }
}
