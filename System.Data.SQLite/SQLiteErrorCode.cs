// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteErrorCode
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// SQLite error codes.  Actually, this enumeration represents a return code,
  /// which may also indicate success in one of several ways (e.g. SQLITE_OK,
  /// SQLITE_ROW, and SQLITE_DONE).  Therefore, the name of this enumeration is
  /// something of a misnomer.
  /// </summary>
  public enum SQLiteErrorCode
  {
    /// <summary>
    /// The error code is unknown.  This error code
    /// is only used by the managed wrapper itself.
    /// </summary>
    Unknown = -1, // 0xFFFFFFFF
    /// <summary>Successful result</summary>
    Ok = 0,
    /// <summary>SQL error or missing database</summary>
    Error = 1,
    /// <summary>Internal logic error in SQLite</summary>
    Internal = 2,
    /// <summary>Access permission denied</summary>
    Perm = 3,
    /// <summary>Callback routine requested an abort</summary>
    Abort = 4,
    /// <summary>The database file is locked</summary>
    Busy = 5,
    /// <summary>A table in the database is locked</summary>
    Locked = 6,
    /// <summary>A malloc() failed</summary>
    NoMem = 7,
    /// <summary>Attempt to write a readonly database</summary>
    ReadOnly = 8,
    /// <summary>Operation terminated by sqlite3_interrupt()</summary>
    Interrupt = 9,
    /// <summary>Some kind of disk I/O error occurred</summary>
    IoErr = 10, // 0x0000000A
    /// <summary>The database disk image is malformed</summary>
    Corrupt = 11, // 0x0000000B
    /// <summary>Unknown opcode in sqlite3_file_control()</summary>
    NotFound = 12, // 0x0000000C
    /// <summary>Insertion failed because database is full</summary>
    Full = 13, // 0x0000000D
    /// <summary>Unable to open the database file</summary>
    CantOpen = 14, // 0x0000000E
    /// <summary>Database lock protocol error</summary>
    Protocol = 15, // 0x0000000F
    /// <summary>Database is empty</summary>
    Empty = 16, // 0x00000010
    /// <summary>The database schema changed</summary>
    Schema = 17, // 0x00000011
    /// <summary>String or BLOB exceeds size limit</summary>
    TooBig = 18, // 0x00000012
    /// <summary>Abort due to constraint violation</summary>
    Constraint = 19, // 0x00000013
    /// <summary>Data type mismatch</summary>
    Mismatch = 20, // 0x00000014
    /// <summary>Library used incorrectly</summary>
    Misuse = 21, // 0x00000015
    /// <summary>Uses OS features not supported on host</summary>
    NoLfs = 22, // 0x00000016
    /// <summary>Authorization denied</summary>
    Auth = 23, // 0x00000017
    /// <summary>Auxiliary database format error</summary>
    Format = 24, // 0x00000018
    /// <summary>2nd parameter to sqlite3_bind out of range</summary>
    Range = 25, // 0x00000019
    /// <summary>File opened that is not a database file</summary>
    NotADb = 26, // 0x0000001A
    /// <summary>Notifications from sqlite3_log()</summary>
    Notice = 27, // 0x0000001B
    /// <summary>Warnings from sqlite3_log()</summary>
    Warning = 28, // 0x0000001C
    /// <summary>sqlite3_step() has another row ready</summary>
    Row = 100, // 0x00000064
    /// <summary>sqlite3_step() has finished executing</summary>
    Done = 101, // 0x00000065
    /// <summary>Used to mask off extended result codes</summary>
    NonExtendedMask = 255, // 0x000000FF
    /// <summary>
    /// Success.  Prevents the extension from unloading until the process
    /// terminates.
    /// </summary>
    Ok_Load_Permanently = 256, // 0x00000100
    /// <summary>
    /// A collation sequence was referenced by a schema and it cannot be
    /// found.
    /// </summary>
    Error_Missing_CollSeq = 257, // 0x00000101
    /// <summary>
    /// A database file is locked due to a recovery operation.
    /// </summary>
    Busy_Recovery = 261, // 0x00000105
    /// <summary>A database table is locked in shared-cache mode.</summary>
    Locked_SharedCache = 262, // 0x00000106
    /// <summary>
    /// A database file is read-only due to a recovery operation.
    /// </summary>
    ReadOnly_Recovery = 264, // 0x00000108
    /// <summary>A file read operation failed.</summary>
    IoErr_Read = 266, // 0x0000010A
    /// <summary>A virtual table is malformed.</summary>
    Corrupt_Vtab = 267, // 0x0000010B
    /// <summary>
    /// A database file cannot be opened because no temporary directory is available.
    /// </summary>
    CantOpen_NoTempDir = 270, // 0x0000010E
    /// <summary>A CHECK constraint failed.</summary>
    Constraint_Check = 275, // 0x00000113
    /// <summary>User authentication failed.</summary>
    Auth_User = 279, // 0x00000117
    /// <summary>Frames were recovered from the WAL log file.</summary>
    Notice_Recover_Wal = 283, // 0x0000011B
    /// <summary>An automatic index was created to process a query.</summary>
    Warning_AutoIndex = 284, // 0x0000011C
    /// <summary>
    /// An internal operation failed and it may succeed if retried.
    /// </summary>
    Error_Retry = 513, // 0x00000201
    /// <summary>
    /// An operation is being aborted due to rollback processing.
    /// </summary>
    Abort_Rollback = 516, // 0x00000204
    /// <summary>A database file is locked due to snapshot semantics.</summary>
    Busy_Snapshot = 517, // 0x00000205
    /// <summary>A virtual table in the database is locked.</summary>
    Locked_Vtab = 518, // 0x00000206
    /// <summary>
    /// A database file is read-only because a lock could not be obtained.
    /// </summary>
    ReadOnly_CantLock = 520, // 0x00000208
    /// <summary>
    /// A file read operation returned less data than requested.
    /// </summary>
    IoErr_Short_Read = 522, // 0x0000020A
    /// <summary>A required sequence table is missing or corrupt.</summary>
    Corrupt_Sequence = 523, // 0x0000020B
    /// <summary>
    /// A database file cannot be opened because its path represents a directory.
    /// </summary>
    CantOpen_IsDir = 526, // 0x0000020E
    /// <summary>A commit hook produced a unsuccessful return code.</summary>
    Constraint_CommitHook = 531, // 0x00000213
    /// <summary>Pages were recovered from the journal file.</summary>
    Notice_Recover_Rollback = 539, // 0x0000021B
    /// <summary>
    /// A database file is read-only because it needs rollback processing.
    /// </summary>
    ReadOnly_Rollback = 776, // 0x00000308
    /// <summary>A file write operation failed.</summary>
    IoErr_Write = 778, // 0x0000030A
    /// <summary>
    /// A database file cannot be opened because its full path could not be obtained.
    /// </summary>
    CantOpen_FullPath = 782, // 0x0000030E
    /// <summary>A FOREIGN KEY constraint failed.</summary>
    Constraint_ForeignKey = 787, // 0x00000313
    /// <summary>
    /// A database file is read-only because it was moved while open.
    /// </summary>
    ReadOnly_DbMoved = 1032, // 0x00000408
    /// <summary>A file synchronization operation failed.</summary>
    IoErr_Fsync = 1034, // 0x0000040A
    /// <summary>
    /// A database file cannot be opened because a path string conversion operation failed.
    /// </summary>
    CantOpen_ConvPath = 1038, // 0x0000040E
    /// <summary>Not currently used.</summary>
    Constraint_Function = 1043, // 0x00000413
    /// <summary>
    /// The shared-memory file is read-only and it should be read-write.
    /// </summary>
    ReadOnly_CantInit = 1288, // 0x00000508
    /// <summary>A directory synchronization operation failed.</summary>
    IoErr_Dir_Fsync = 1290, // 0x0000050A
    /// <summary>A NOT NULL constraint failed.</summary>
    Constraint_NotNull = 1299, // 0x00000513
    /// <summary>
    /// Unable to create journal file because the directory is read-only.
    /// </summary>
    ReadOnly_Directory = 1544, // 0x00000608
    /// <summary>A file truncate operation failed.</summary>
    IoErr_Truncate = 1546, // 0x0000060A
    /// <summary>A PRIMARY KEY constraint failed.</summary>
    Constraint_PrimaryKey = 1555, // 0x00000613
    /// <summary>A file metadata operation failed.</summary>
    IoErr_Fstat = 1802, // 0x0000070A
    /// <summary>The RAISE function was used by a trigger-program.</summary>
    Constraint_Trigger = 1811, // 0x00000713
    /// <summary>A file unlock operation failed.</summary>
    IoErr_Unlock = 2058, // 0x0000080A
    /// <summary>A UNIQUE constraint failed.</summary>
    Constraint_Unique = 2067, // 0x00000813
    /// <summary>A file lock operation failed.</summary>
    IoErr_RdLock = 2314, // 0x0000090A
    /// <summary>Not currently used.</summary>
    Constraint_Vtab = 2323, // 0x00000913
    /// <summary>A file delete operation failed.</summary>
    IoErr_Delete = 2570, // 0x00000A0A
    /// <summary>A ROWID constraint failed.</summary>
    Constraint_RowId = 2579, // 0x00000A13
    /// <summary>Not currently used.</summary>
    IoErr_Blocked = 2826, // 0x00000B0A
    /// <summary>Out-of-memory during a file operation.</summary>
    IoErr_NoMem = 3082, // 0x00000C0A
    /// <summary>A file existence/status operation failed.</summary>
    IoErr_Access = 3338, // 0x00000D0A
    /// <summary>A check for a reserved lock failed.</summary>
    IoErr_CheckReservedLock = 3594, // 0x00000E0A
    /// <summary>A file lock operation failed.</summary>
    IoErr_Lock = 3850, // 0x00000F0A
    /// <summary>A file close operation failed.</summary>
    IoErr_Close = 4106, // 0x0000100A
    /// <summary>A directory close operation failed.</summary>
    IoErr_Dir_Close = 4362, // 0x0000110A
    /// <summary>A shared memory open operation failed.</summary>
    IoErr_ShmOpen = 4618, // 0x0000120A
    /// <summary>A shared memory size operation failed.</summary>
    IoErr_ShmSize = 4874, // 0x0000130A
    /// <summary>A shared memory lock operation failed.</summary>
    IoErr_ShmLock = 5130, // 0x0000140A
    /// <summary>A shared memory map operation failed.</summary>
    IoErr_ShmMap = 5386, // 0x0000150A
    /// <summary>A file seek operation failed.</summary>
    IoErr_Seek = 5642, // 0x0000160A
    /// <summary>
    /// A file delete operation failed because it does not exist.
    /// </summary>
    IoErr_Delete_NoEnt = 5898, // 0x0000170A
    /// <summary>A file memory mapping operation failed.</summary>
    IoErr_Mmap = 6154, // 0x0000180A
    /// <summary>The temporary directory path could not be obtained.</summary>
    IoErr_GetTempPath = 6410, // 0x0000190A
    /// <summary>A path string conversion operation failed.</summary>
    IoErr_ConvPath = 6666, // 0x00001A0A
    /// <summary>Reserved.</summary>
    IoErr_VNode = 6922, // 0x00001B0A
    /// <summary>An attempt to authenticate failed.</summary>
    IoErr_Auth = 7178, // 0x00001C0A
    /// <summary>An attempt to begin a file system transaction failed.</summary>
    IoErr_Begin_Atomic = 7434, // 0x00001D0A
    /// <summary>
    /// An attempt to commit a file system transaction failed.
    /// </summary>
    IoErr_Commit_Atomic = 7690, // 0x00001E0A
    /// <summary>
    /// An attempt to rollback a file system transaction failed.
    /// </summary>
    IoErr_Rollback_Atomic = 7946, // 0x00001F0A
  }
}
