// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteSynchronousEnum
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Possible values for the "synchronous" database setting.  This setting determines
  /// how often the database engine calls the xSync method of the VFS.
  /// </summary>
  internal enum SQLiteSynchronousEnum
  {
    /// <summary>
    /// Use the default "synchronous" database setting.  Currently, this should be
    /// the same as using the FULL mode.
    /// </summary>
    Default = -1, // 0xFFFFFFFF
    /// <summary>
    /// The database engine continues without syncing as soon as it has handed
    /// data off to the operating system.  If the application running SQLite
    /// crashes, the data will be safe, but the database might become corrupted
    /// if the operating system crashes or the computer loses power before that
    /// data has been written to the disk surface.
    /// </summary>
    Off = 0,
    /// <summary>
    /// The database engine will still sync at the most critical moments, but
    /// less often than in FULL mode.  There is a very small (though non-zero)
    /// chance that a power failure at just the wrong time could corrupt the
    /// database in NORMAL mode.
    /// </summary>
    Normal = 1,
    /// <summary>
    /// The database engine will use the xSync method of the VFS to ensure that
    /// all content is safely written to the disk surface prior to continuing.
    /// This ensures that an operating system crash or power failure will not
    /// corrupt the database.  FULL synchronous is very safe, but it is also
    /// slower.
    /// </summary>
    Full = 2,
  }
}
