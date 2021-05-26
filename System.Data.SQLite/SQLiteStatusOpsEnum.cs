// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteStatusOpsEnum
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// These are the supported status parameters for use with the native
  /// SQLite library.
  /// </summary>
  internal enum SQLiteStatusOpsEnum
  {
    /// <summary>
    /// This parameter returns the number of lookaside memory slots
    /// currently checked out.
    /// </summary>
    SQLITE_DBSTATUS_LOOKASIDE_USED,
    /// <summary>
    /// This parameter returns the approximate number of bytes of
    /// heap memory used by all pager caches associated with the
    /// database connection. The highwater mark associated with
    /// SQLITE_DBSTATUS_CACHE_USED is always 0.
    /// </summary>
    SQLITE_DBSTATUS_CACHE_USED,
    /// <summary>
    /// This parameter returns the approximate number of bytes of
    /// heap memory used to store the schema for all databases
    /// associated with the connection - main, temp, and any ATTACH-ed
    /// databases. The full amount of memory used by the schemas is
    /// reported, even if the schema memory is shared with other
    /// database connections due to shared cache mode being enabled.
    /// The highwater mark associated with SQLITE_DBSTATUS_SCHEMA_USED
    /// is always 0.
    /// </summary>
    SQLITE_DBSTATUS_SCHEMA_USED,
    /// <summary>
    /// This parameter returns the number malloc attempts that might
    /// have been satisfied using lookaside memory but failed due to
    /// all lookaside memory already being in use. Only the high-water
    /// value is meaningful; the current value is always zero.
    /// </summary>
    SQLITE_DBSTATUS_STMT_USED,
    /// <summary>
    /// This parameter returns the number malloc attempts that were
    /// satisfied using lookaside memory. Only the high-water value
    /// is meaningful; the current value is always zero.
    /// </summary>
    SQLITE_DBSTATUS_LOOKASIDE_HIT,
    /// <summary>
    /// This parameter returns the number malloc attempts that might
    /// have been satisfied using lookaside memory but failed due to
    /// the amount of memory requested being larger than the lookaside
    /// slot size. Only the high-water value is meaningful; the current
    /// value is always zero.
    /// </summary>
    SQLITE_DBSTATUS_LOOKASIDE_MISS_SIZE,
    /// <summary>
    /// This parameter returns the number malloc attempts that might
    /// have been satisfied using lookaside memory but failed due to
    /// the amount of memory requested being larger than the lookaside
    /// slot size. Only the high-water value is meaningful; the current
    /// value is always zero.
    /// </summary>
    SQLITE_DBSTATUS_LOOKASIDE_MISS_FULL,
    /// <summary>
    /// This parameter returns the number of pager cache hits that
    /// have occurred. The highwater mark associated with
    /// SQLITE_DBSTATUS_CACHE_HIT is always 0.
    /// </summary>
    SQLITE_DBSTATUS_CACHE_HIT,
    /// <summary>
    /// This parameter returns the number of pager cache misses that
    /// have occurred. The highwater mark associated with
    /// SQLITE_DBSTATUS_CACHE_MISS is always 0.
    /// </summary>
    SQLITE_DBSTATUS_CACHE_MISS,
    /// <summary>
    /// This parameter returns the number of dirty cache entries that
    /// have been written to disk. Specifically, the number of pages
    /// written to the wal file in wal mode databases, or the number
    /// of pages written to the database file in rollback mode
    /// databases. Any pages written as part of transaction rollback
    /// or database recovery operations are not included. If an IO or
    /// other error occurs while writing a page to disk, the effect
    /// on subsequent SQLITE_DBSTATUS_CACHE_WRITE requests is
    /// undefined. The highwater mark associated with
    /// SQLITE_DBSTATUS_CACHE_WRITE is always 0.
    /// </summary>
    SQLITE_DBSTATUS_CACHE_WRITE,
    /// <summary>
    /// This parameter returns zero for the current value if and only
    /// if all foreign key constraints (deferred or immediate) have
    /// been resolved. The highwater mark is always 0.
    /// </summary>
    SQLITE_DBSTATUS_DEFERRED_FKS,
    /// <summary>
    /// This parameter is similar to DBSTATUS_CACHE_USED, except that
    /// if a pager cache is shared between two or more connections the
    /// bytes of heap memory used by that pager cache is divided evenly
    /// between the attached connections. In other words, if none of
    /// the pager caches associated with the database connection are
    /// shared, this request returns the same value as DBSTATUS_CACHE_USED.
    /// Or, if one or more or the pager caches are shared, the value
    /// returned by this call will be smaller than that returned by
    /// DBSTATUS_CACHE_USED. The highwater mark associated with
    /// SQLITE_DBSTATUS_CACHE_USED_SHARED is always 0.
    /// </summary>
    SQLITE_DBSTATUS_CACHE_USED_SHARED,
  }
}
