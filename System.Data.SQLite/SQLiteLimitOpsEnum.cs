// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteLimitOpsEnum
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// These constants are used with the sqlite3_limit() API.
  /// </summary>
  public enum SQLiteLimitOpsEnum
  {
    /// <summary>
    /// This value represents an unknown (or invalid) limit, do not use it.
    /// </summary>
    SQLITE_LIMIT_NONE = -1, // 0xFFFFFFFF
    /// <summary>
    /// The maximum size of any string or BLOB or table row, in bytes.
    /// </summary>
    SQLITE_LIMIT_LENGTH = 0,
    /// <summary>The maximum length of an SQL statement, in bytes.</summary>
    SQLITE_LIMIT_SQL_LENGTH = 1,
    /// <summary>
    /// The maximum number of columns in a table definition or in the
    /// result set of a SELECT or the maximum number of columns in an
    /// index or in an ORDER BY or GROUP BY clause.
    /// </summary>
    SQLITE_LIMIT_COLUMN = 2,
    /// <summary>
    /// The maximum depth of the parse tree on any expression.
    /// </summary>
    SQLITE_LIMIT_EXPR_DEPTH = 3,
    /// <summary>
    /// The maximum number of terms in a compound SELECT statement.
    /// </summary>
    SQLITE_LIMIT_COMPOUND_SELECT = 4,
    /// <summary>
    /// The maximum number of instructions in a virtual machine program
    /// used to implement an SQL statement. If sqlite3_prepare_v2() or
    /// the equivalent tries to allocate space for more than this many
    /// opcodes in a single prepared statement, an SQLITE_NOMEM error
    /// is returned.
    /// </summary>
    SQLITE_LIMIT_VDBE_OP = 5,
    /// <summary>The maximum number of arguments on a function.</summary>
    SQLITE_LIMIT_FUNCTION_ARG = 6,
    /// <summary>The maximum number of attached databases.</summary>
    SQLITE_LIMIT_ATTACHED = 7,
    /// <summary>
    /// The maximum length of the pattern argument to the LIKE or GLOB
    /// operators.
    /// </summary>
    SQLITE_LIMIT_LIKE_PATTERN_LENGTH = 8,
    /// <summary>
    /// The maximum index number of any parameter in an SQL statement.
    /// </summary>
    SQLITE_LIMIT_VARIABLE_NUMBER = 9,
    /// <summary>The maximum depth of recursion for triggers.</summary>
    SQLITE_LIMIT_TRIGGER_DEPTH = 10, // 0x0000000A
    /// <summary>
    /// The maximum number of auxiliary worker threads that a single
    /// prepared statement may start.
    /// </summary>
    SQLITE_LIMIT_WORKER_THREADS = 11, // 0x0000000B
  }
}
