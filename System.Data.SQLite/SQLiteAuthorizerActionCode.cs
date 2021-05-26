// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteAuthorizerActionCode
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// The action code responsible for the current call into the authorizer.
  /// </summary>
  public enum SQLiteAuthorizerActionCode
  {
    /// <summary>
    /// No action is being performed.  This value should not be used from
    /// external code.
    /// </summary>
    None = -1, // 0xFFFFFFFF
    /// <summary>No longer used.</summary>
    Copy = 0,
    /// <summary>
    /// An index will be created.  The action-specific arguments are the
    /// index name and the table name.
    /// 
    /// </summary>
    CreateIndex = 1,
    /// <summary>
    /// A table will be created.  The action-specific arguments are the
    /// table name and a null value.
    /// </summary>
    CreateTable = 2,
    /// <summary>
    /// A temporary index will be created.  The action-specific arguments
    /// are the index name and the table name.
    /// </summary>
    CreateTempIndex = 3,
    /// <summary>
    /// A temporary table will be created.  The action-specific arguments
    /// are the table name and a null value.
    /// </summary>
    CreateTempTable = 4,
    /// <summary>
    /// A temporary trigger will be created.  The action-specific arguments
    /// are the trigger name and the table name.
    /// </summary>
    CreateTempTrigger = 5,
    /// <summary>
    /// A temporary view will be created.  The action-specific arguments are
    /// the view name and a null value.
    /// </summary>
    CreateTempView = 6,
    /// <summary>
    /// A trigger will be created.  The action-specific arguments are the
    /// trigger name and the table name.
    /// </summary>
    CreateTrigger = 7,
    /// <summary>
    /// A view will be created.  The action-specific arguments are the view
    /// name and a null value.
    /// </summary>
    CreateView = 8,
    /// <summary>
    /// A DELETE statement will be executed.  The action-specific arguments
    /// are the table name and a null value.
    /// </summary>
    Delete = 9,
    /// <summary>
    /// An index will be dropped.  The action-specific arguments are the
    /// index name and the table name.
    /// </summary>
    DropIndex = 10, // 0x0000000A
    /// <summary>
    /// A table will be dropped.  The action-specific arguments are the tables
    /// name and a null value.
    /// </summary>
    DropTable = 11, // 0x0000000B
    /// <summary>
    /// A temporary index will be dropped.  The action-specific arguments are
    /// the index name and the table name.
    /// </summary>
    DropTempIndex = 12, // 0x0000000C
    /// <summary>
    /// A temporary table will be dropped.  The action-specific arguments are
    /// the table name and a null value.
    /// </summary>
    DropTempTable = 13, // 0x0000000D
    /// <summary>
    /// A temporary trigger will be dropped.  The action-specific arguments
    /// are the trigger name and the table name.
    /// </summary>
    DropTempTrigger = 14, // 0x0000000E
    /// <summary>
    /// A temporary view will be dropped.  The action-specific arguments are
    /// the view name and a null value.
    /// </summary>
    DropTempView = 15, // 0x0000000F
    /// <summary>
    /// A trigger will be dropped.  The action-specific arguments are the
    /// trigger name and the table name.
    /// </summary>
    DropTrigger = 16, // 0x00000010
    /// <summary>
    /// A view will be dropped.  The action-specific arguments are the view
    /// name and a null value.
    /// </summary>
    DropView = 17, // 0x00000011
    /// <summary>
    /// An INSERT statement will be executed.  The action-specific arguments
    /// are the table name and a null value.
    /// </summary>
    Insert = 18, // 0x00000012
    /// <summary>
    /// A PRAGMA statement will be executed.  The action-specific arguments
    /// are the name of the PRAGMA and the new value or a null value.
    /// </summary>
    Pragma = 19, // 0x00000013
    /// <summary>
    /// A table column will be read.  The action-specific arguments are the
    /// table name and the column name.
    /// </summary>
    Read = 20, // 0x00000014
    /// <summary>
    /// A SELECT statement will be executed.  The action-specific arguments
    /// are both null values.
    /// </summary>
    Select = 21, // 0x00000015
    /// <summary>
    /// A transaction will be started, committed, or rolled back.  The
    /// action-specific arguments are the name of the operation (BEGIN,
    /// COMMIT, or ROLLBACK) and a null value.
    /// </summary>
    Transaction = 22, // 0x00000016
    /// <summary>
    /// An UPDATE statement will be executed.  The action-specific arguments
    /// are the table name and the column name.
    /// </summary>
    Update = 23, // 0x00000017
    /// <summary>
    /// A database will be attached to the connection.  The action-specific
    /// arguments are the database file name and a null value.
    /// </summary>
    Attach = 24, // 0x00000018
    /// <summary>
    /// A database will be detached from the connection.  The action-specific
    /// arguments are the database name and a null value.
    /// </summary>
    Detach = 25, // 0x00000019
    /// <summary>
    /// The schema of a table will be altered.  The action-specific arguments
    /// are the database name and the table name.
    /// </summary>
    AlterTable = 26, // 0x0000001A
    /// <summary>
    /// An index will be deleted and then recreated.  The action-specific
    /// arguments are the index name and a null value.
    /// </summary>
    Reindex = 27, // 0x0000001B
    /// <summary>
    /// A table will be analyzed to gathers statistics about it.  The
    /// action-specific arguments are the table name and a null value.
    /// </summary>
    Analyze = 28, // 0x0000001C
    /// <summary>
    /// A virtual table will be created.  The action-specific arguments are
    /// the table name and the module name.
    /// </summary>
    CreateVtable = 29, // 0x0000001D
    /// <summary>
    /// A virtual table will be dropped.  The action-specific arguments are
    /// the table name and the module name.
    /// </summary>
    DropVtable = 30, // 0x0000001E
    /// <summary>
    /// A SQL function will be called.  The action-specific arguments are a
    /// null value and the function name.
    /// </summary>
    Function = 31, // 0x0000001F
    /// <summary>
    /// A savepoint will be created, released, or rolled back.  The
    /// action-specific arguments are the name of the operation (BEGIN,
    /// RELEASE, or ROLLBACK) and the savepoint name.
    /// </summary>
    Savepoint = 32, // 0x00000020
    /// <summary>
    /// A recursive query will be executed.  The action-specific arguments
    /// are two null values.
    /// </summary>
    Recursive = 33, // 0x00000021
  }
}
