// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConfigDbOpsEnum
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// These are the supported configuration verbs for use with the native
  /// SQLite library.  They are used with the
  /// <see cref="M:System.Data.SQLite.SQLiteConnection.SetConfigurationOption(System.Data.SQLite.SQLiteConfigDbOpsEnum,System.Object)" /> method.
  /// </summary>
  public enum SQLiteConfigDbOpsEnum
  {
    /// <summary>
    /// This value represents an unknown (or invalid) option, do not use it.
    /// </summary>
    SQLITE_DBCONFIG_NONE = 0,
    /// <summary>
    /// This option is used to change the name of the "main" database
    /// schema.  The sole argument is a pointer to a constant UTF8 string
    /// which will become the new schema name in place of "main".
    /// </summary>
    SQLITE_DBCONFIG_MAINDBNAME = 1000, // 0x000003E8
    /// <summary>
    /// This option is used to configure the lookaside memory allocator.
    /// The value must be an array with three elements.  The second element
    /// must be an <see cref="T:System.Int32" /> containing the size of each buffer
    /// slot.  The third element must be an <see cref="T:System.Int32" /> containing
    /// the number of slots.  The first element must be an <see cref="T:System.IntPtr" />
    /// that points to a native memory buffer of bytes equal to or greater
    /// than the product of the second and third element values.
    /// </summary>
    SQLITE_DBCONFIG_LOOKASIDE = 1001, // 0x000003E9
    /// <summary>
    /// This option is used to enable or disable the enforcement of
    /// foreign key constraints.
    /// </summary>
    SQLITE_DBCONFIG_ENABLE_FKEY = 1002, // 0x000003EA
    /// <summary>This option is used to enable or disable triggers.</summary>
    SQLITE_DBCONFIG_ENABLE_TRIGGER = 1003, // 0x000003EB
    /// <summary>
    /// This option is used to enable or disable the two-argument version
    /// of the fts3_tokenizer() function which is part of the FTS3 full-text
    /// search engine extension.
    /// </summary>
    SQLITE_DBCONFIG_ENABLE_FTS3_TOKENIZER = 1004, // 0x000003EC
    /// <summary>
    /// This option is used to enable or disable the loading of extensions.
    /// </summary>
    SQLITE_DBCONFIG_ENABLE_LOAD_EXTENSION = 1005, // 0x000003ED
    /// <summary>
    /// This option is used to enable or disable the automatic checkpointing
    /// when a WAL database is closed.
    /// </summary>
    SQLITE_DBCONFIG_NO_CKPT_ON_CLOSE = 1006, // 0x000003EE
    /// <summary>
    /// This option is used to enable or disable the query planner stability
    /// guarantee (QPSG).
    /// </summary>
    SQLITE_DBCONFIG_ENABLE_QPSG = 1007, // 0x000003EF
    /// <summary>
    /// This option is used to enable or disable the extra EXPLAIN QUERY PLAN
    /// output for trigger programs.
    /// </summary>
    SQLITE_DBCONFIG_TRIGGER_EQP = 1008, // 0x000003F0
    /// <summary>
    /// This option is used as part of the process to reset a database back
    /// to an empty state.  Because resetting a database is destructive and
    /// irreversible, the process requires the use of this obscure flag and
    /// multiple steps to help ensure that it does not happen by accident.
    /// </summary>
    SQLITE_DBCONFIG_RESET_DATABASE = 1009, // 0x000003F1
    /// <summary>
    /// This option activates or deactivates the "defensive" flag for a
    /// database connection.  When the defensive flag is enabled, language
    /// features that allow ordinary SQL to deliberately corrupt the database
    /// file are disabled.  The disabled features include but are not limited
    /// to the following:
    /// <![CDATA[<ul>]]>
    /// <![CDATA[<li>]]>
    /// The PRAGMA writable_schema=ON statement.
    /// <![CDATA[</li>]]>
    /// <![CDATA[<li>]]>
    /// The PRAGMA journal_mode=OFF statement.
    /// <![CDATA[</li>]]>
    /// <![CDATA[<li>]]>
    /// Writes to the sqlite_dbpage virtual table.
    /// <![CDATA[</li>]]>
    /// <![CDATA[<li>]]>
    /// Direct writes to shadow tables.
    /// <![CDATA[</li>]]>
    /// <![CDATA[</ul>]]>
    /// </summary>
    SQLITE_DBCONFIG_DEFENSIVE = 1010, // 0x000003F2
    /// <summary>
    /// This option activates or deactivates the "writable_schema" flag.
    /// </summary>
    SQLITE_DBCONFIG_WRITABLE_SCHEMA = 1011, // 0x000003F3
    /// <summary>
    /// This option activates or deactivates the legacy behavior of the ALTER
    /// TABLE RENAME command such it behaves as it did prior to version 3.24.0
    /// (2018-06-04).
    /// </summary>
    SQLITE_DBCONFIG_LEGACY_ALTER_TABLE = 1012, // 0x000003F4
    /// <summary>
    /// This option activates or deactivates the legacy double-quoted string
    /// literal misfeature for DML statement only, that is DELETE, INSERT,
    /// SELECT, and UPDATE statements.
    /// </summary>
    SQLITE_DBCONFIG_DQS_DML = 1013, // 0x000003F5
    /// <summary>
    /// This option activates or deactivates the legacy double-quoted string
    /// literal misfeature for DDL statements, such as CREATE TABLE and CREATE
    /// INDEX.
    /// </summary>
    SQLITE_DBCONFIG_DQS_DDL = 1014, // 0x000003F6
    /// <summary>This option is used to enable or disable CREATE VIEW.</summary>
    SQLITE_DBCONFIG_ENABLE_VIEW = 1015, // 0x000003F7
  }
}
