// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteJournalModeEnum
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This enum determines how SQLite treats its journal file.
  /// </summary>
  /// <remarks>
  /// By default SQLite will create and delete the journal file when needed during a transaction.
  /// However, for some computers running certain filesystem monitoring tools, the rapid
  /// creation and deletion of the journal file can cause those programs to fail, or to interfere with SQLite.
  /// 
  /// If a program or virus scanner is interfering with SQLite's journal file, you may receive errors like "unable to open database file"
  /// when starting a transaction.  If this is happening, you may want to change the default journal mode to Persist.
  /// </remarks>
  public enum SQLiteJournalModeEnum
  {
    /// <summary>
    /// The default mode, this causes SQLite to use the existing journaling mode for the database.
    /// </summary>
    Default = -1, // 0xFFFFFFFF
    /// <summary>
    /// SQLite will create and destroy the journal file as-needed.
    /// </summary>
    Delete = 0,
    /// <summary>
    /// When this is set, SQLite will keep the journal file even after a transaction has completed.  It's contents will be erased,
    /// and the journal re-used as often as needed.  If it is deleted, it will be recreated the next time it is needed.
    /// </summary>
    Persist = 1,
    /// <summary>
    /// This option disables the rollback journal entirely.  Interrupted transactions or a program crash can cause database
    /// corruption in this mode!
    /// </summary>
    Off = 2,
    /// <summary>
    /// SQLite will truncate the journal file to zero-length instead of deleting it.
    /// </summary>
    Truncate = 3,
    /// <summary>
    /// SQLite will store the journal in volatile RAM.  This saves disk I/O but at the expense of database safety and integrity.
    /// If the application using SQLite crashes in the middle of a transaction when the MEMORY journaling mode is set, then the
    /// database file will very likely go corrupt.
    /// </summary>
    Memory = 4,
    /// <summary>
    /// SQLite uses a write-ahead log instead of a rollback journal to implement transactions.  The WAL journaling mode is persistent;
    /// after being set it stays in effect across multiple database connections and after closing and reopening the database. A database
    /// in WAL journaling mode can only be accessed by SQLite version 3.7.0 or later.
    /// </summary>
    Wal = 5,
  }
}
