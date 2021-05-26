// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SynchronizationModes
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// The I/O file cache flushing behavior for the connection
  /// </summary>
  public enum SynchronizationModes
  {
    /// <summary>Normal file flushing at critical sections of the code</summary>
    Normal,
    /// <summary>Full file flushing after every write operation</summary>
    Full,
    /// <summary>
    /// Use the default operating system's file flushing, SQLite does not explicitly flush the file buffers after writing
    /// </summary>
    Off,
  }
}
