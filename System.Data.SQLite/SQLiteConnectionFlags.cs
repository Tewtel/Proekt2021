// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionFlags
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// The extra behavioral flags that can be applied to a connection.
  /// </summary>
  [Flags]
  public enum SQLiteConnectionFlags : long
  {
    /// <summary>No extra flags.</summary>
    None = 0,
    /// <summary>Enable logging of all SQL statements to be prepared.</summary>
    LogPrepare = 1,
    /// <summary>
    /// Enable logging of all bound parameter types and raw values.
    /// </summary>
    LogPreBind = 2,
    /// <summary>
    /// Enable logging of all bound parameter strongly typed values.
    /// </summary>
    LogBind = 4,
    /// <summary>
    /// Enable logging of all exceptions caught from user-provided
    /// managed code called from native code via delegates.
    /// </summary>
    LogCallbackException = 8,
    /// <summary>Enable logging of backup API errors.</summary>
    LogBackup = 16, // 0x0000000000000010
    /// <summary>
    /// Skip adding the extension functions provided by the native
    /// interop assembly.
    /// </summary>
    NoExtensionFunctions = 32, // 0x0000000000000020
    /// <summary>
    /// When binding parameter values with the <see cref="T:System.UInt32" />
    /// type, use the interop method that accepts an <see cref="T:System.Int64" />
    /// value.
    /// </summary>
    BindUInt32AsInt64 = 64, // 0x0000000000000040
    /// <summary>
    /// When binding parameter values, always bind them as though they were
    /// plain text (i.e. no numeric, date/time, or other conversions should
    /// be attempted).
    /// </summary>
    BindAllAsText = 128, // 0x0000000000000080
    /// <summary>
    /// When returning column values, always return them as though they were
    /// plain text (i.e. no numeric, date/time, or other conversions should
    /// be attempted).
    /// </summary>
    GetAllAsText = 256, // 0x0000000000000100
    /// <summary>
    /// Prevent this <see cref="T:System.Data.SQLite.SQLiteConnection" /> object instance from
    /// loading extensions.
    /// </summary>
    NoLoadExtension = 512, // 0x0000000000000200
    /// <summary>
    /// Prevent this <see cref="T:System.Data.SQLite.SQLiteConnection" /> object instance from
    /// creating virtual table modules.
    /// </summary>
    NoCreateModule = 1024, // 0x0000000000000400
    /// <summary>
    /// Skip binding any functions provided by other managed assemblies when
    /// opening the connection.
    /// </summary>
    NoBindFunctions = 2048, // 0x0000000000000800
    /// <summary>
    /// Skip setting the logging related properties of the
    /// <see cref="T:System.Data.SQLite.SQLiteModule" /> object instance that was passed to
    /// the <see cref="M:System.Data.SQLite.SQLiteConnection.CreateModule(System.Data.SQLite.SQLiteModule)" /> method.
    /// </summary>
    NoLogModule = 4096, // 0x0000000000001000
    /// <summary>
    /// Enable logging of all virtual table module errors seen by the
    /// <see cref="M:System.Data.SQLite.SQLiteModule.SetTableError(System.IntPtr,System.String)" /> method.
    /// </summary>
    LogModuleError = 8192, // 0x0000000000002000
    /// <summary>
    /// Enable logging of certain virtual table module exceptions that cannot
    /// be easily discovered via other means.
    /// </summary>
    LogModuleException = 16384, // 0x0000000000004000
    /// <summary>
    /// Enable tracing of potentially important [non-fatal] error conditions
    /// that cannot be easily reported through other means.
    /// </summary>
    TraceWarning = 32768, // 0x0000000000008000
    /// <summary>
    /// When binding parameter values, always use the invariant culture when
    /// converting their values from strings.
    /// </summary>
    ConvertInvariantText = 65536, // 0x0000000000010000
    /// <summary>
    /// When binding parameter values, always use the invariant culture when
    /// converting their values to strings.
    /// </summary>
    BindInvariantText = 131072, // 0x0000000000020000
    /// <summary>
    /// Disable using the connection pool by default.  If the "Pooling"
    /// connection string property is specified, its value will override
    /// this flag.  The precise outcome of combining this flag with the
    /// <see cref="F:System.Data.SQLite.SQLiteConnectionFlags.UseConnectionPool" /> flag is unspecified; however,
    /// one of the flags will be in effect.
    /// </summary>
    NoConnectionPool = 262144, // 0x0000000000040000
    /// <summary>
    /// Enable using the connection pool by default.  If the "Pooling"
    /// connection string property is specified, its value will override
    /// this flag.  The precise outcome of combining this flag with the
    /// <see cref="F:System.Data.SQLite.SQLiteConnectionFlags.NoConnectionPool" /> flag is unspecified; however,
    /// one of the flags will be in effect.
    /// </summary>
    UseConnectionPool = 524288, // 0x0000000000080000
    /// <summary>
    /// Enable using per-connection mappings between type names and
    /// <see cref="T:System.Data.DbType" /> values.  Also see the
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.ClearTypeMappings" />,
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.GetTypeMappings" />, and
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.AddTypeMapping(System.String,System.Data.DbType,System.Boolean)" /> methods.  These
    /// per-connection mappings, when present, override the corresponding
    /// global mappings.
    /// </summary>
    UseConnectionTypes = 1048576, // 0x0000000000100000
    /// <summary>
    /// Disable using global mappings between type names and
    /// <see cref="T:System.Data.DbType" /> values.  This may be useful in some very narrow
    /// cases; however, if there are no per-connection type mappings, the
    /// fallback defaults will be used for both type names and their
    /// associated <see cref="T:System.Data.DbType" /> values.  Therefore, use of this flag
    /// is not recommended.
    /// </summary>
    NoGlobalTypes = 2097152, // 0x0000000000200000
    /// <summary>
    /// When the <see cref="P:System.Data.SQLite.SQLiteDataReader.HasRows" /> property is used, it
    /// should return non-zero if there were ever any rows in the associated
    /// result sets.
    /// </summary>
    StickyHasRows = 4194304, // 0x0000000000400000
    /// <summary>
    /// Enable "strict" transaction enlistment semantics.  Setting this flag
    /// will cause an exception to be thrown if an attempt is made to enlist
    /// in a transaction with an unavailable or unsupported isolation level.
    /// In the future, more extensive checks may be enabled by this flag as
    /// well.
    /// </summary>
    StrictEnlistment = 8388608, // 0x0000000000800000
    /// <summary>
    /// Enable mapping of unsupported transaction isolation levels to the
    /// closest supported transaction isolation level.
    /// </summary>
    MapIsolationLevels = 16777216, // 0x0000000001000000
    /// <summary>
    /// When returning column values, attempt to detect the affinity of
    /// textual values by checking if they fully conform to those of the
    /// <see cref="F:System.Data.SQLite.TypeAffinity.Null" />,
    /// <see cref="F:System.Data.SQLite.TypeAffinity.Int64" />,
    /// <see cref="F:System.Data.SQLite.TypeAffinity.Double" />,
    /// or <see cref="F:System.Data.SQLite.TypeAffinity.DateTime" /> types.
    /// </summary>
    DetectTextAffinity = 33554432, // 0x0000000002000000
    /// <summary>
    /// When returning column values, attempt to detect the type of
    /// string values by checking if they fully conform to those of
    /// the <see cref="F:System.Data.SQLite.TypeAffinity.Null" />,
    /// <see cref="F:System.Data.SQLite.TypeAffinity.Int64" />,
    /// <see cref="F:System.Data.SQLite.TypeAffinity.Double" />,
    /// or <see cref="F:System.Data.SQLite.TypeAffinity.DateTime" /> types.
    /// </summary>
    DetectStringType = 67108864, // 0x0000000004000000
    /// <summary>
    /// Skip querying runtime configuration settings for use by the
    /// <see cref="T:System.Data.SQLite.SQLiteConvert" /> class, including the default
    /// <see cref="T:System.Data.DbType" /> value and default database type name.
    /// <b>NOTE: If the <see cref="P:System.Data.SQLite.SQLiteConnection.DefaultDbType" />
    /// and/or <see cref="P:System.Data.SQLite.SQLiteConnection.DefaultTypeName" />
    /// properties are not set explicitly nor set via their connection
    /// string properties and repeated calls to determine these runtime
    /// configuration settings are seen to be a problem, this flag
    /// should be set.</b>
    /// </summary>
    NoConvertSettings = 134217728, // 0x0000000008000000
    /// <summary>
    /// When binding parameter values with the <see cref="T:System.DateTime" />
    /// type, take their <see cref="T:System.DateTimeKind" /> into account as
    /// well as that of the associated <see cref="T:System.Data.SQLite.SQLiteConnection" />.
    /// </summary>
    BindDateTimeWithKind = 268435456, // 0x0000000010000000
    /// <summary>
    /// If an exception is caught when raising the
    /// <see cref="E:System.Data.SQLite.SQLiteConnection.Commit" /> event, the transaction
    /// should be rolled back.  If this is not specified, the transaction
    /// will continue the commit process instead.
    /// </summary>
    RollbackOnException = 536870912, // 0x0000000020000000
    /// <summary>
    /// If an exception is caught when raising the
    /// <see cref="E:System.Data.SQLite.SQLiteConnection.Authorize" /> event, the action should
    /// should be denied.  If this is not specified, the action will be
    /// allowed instead.
    /// </summary>
    DenyOnException = 1073741824, // 0x0000000040000000
    /// <summary>
    /// If an exception is caught when raising the
    /// <see cref="E:System.Data.SQLite.SQLiteConnection.Progress" /> event, the operation
    /// should be interrupted.  If this is not specified, the operation
    /// will simply continue.
    /// </summary>
    InterruptOnException = 2147483648, // 0x0000000080000000
    /// <summary>
    /// Attempt to unbind all functions provided by other managed assemblies
    /// when closing the connection.
    /// </summary>
    UnbindFunctionsOnClose = 4294967296, // 0x0000000100000000
    /// <summary>
    /// When returning column values as a <see cref="T:System.String" />, skip
    /// verifying their affinity.
    /// </summary>
    NoVerifyTextAffinity = 8589934592, // 0x0000000200000000
    /// <summary>
    /// Enable using per-connection mappings between type names and
    /// <see cref="T:System.Data.SQLite.SQLiteBindValueCallback" /> values.  Also see the
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.ClearTypeCallbacks" />,
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.TryGetTypeCallbacks(System.String,System.Data.SQLite.SQLiteTypeCallbacks@)" />, and
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.SetTypeCallbacks(System.String,System.Data.SQLite.SQLiteTypeCallbacks)" /> methods.
    /// </summary>
    UseConnectionBindValueCallbacks = 17179869184, // 0x0000000400000000
    /// <summary>
    /// Enable using per-connection mappings between type names and
    /// <see cref="T:System.Data.SQLite.SQLiteReadValueCallback" /> values.  Also see the
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.ClearTypeCallbacks" />,
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.TryGetTypeCallbacks(System.String,System.Data.SQLite.SQLiteTypeCallbacks@)" />, and
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.SetTypeCallbacks(System.String,System.Data.SQLite.SQLiteTypeCallbacks)" /> methods.
    /// </summary>
    UseConnectionReadValueCallbacks = 34359738368, // 0x0000000800000000
    /// <summary>
    /// If the database type name has not been explicitly set for the
    /// parameter specified, fallback to using the parameter name.
    /// </summary>
    UseParameterNameForTypeName = 68719476736, // 0x0000001000000000
    /// <summary>
    /// If the database type name has not been explicitly set for the
    /// parameter specified, fallback to using the database type name
    /// associated with the <see cref="T:System.Data.DbType" /> value.
    /// </summary>
    UseParameterDbTypeForTypeName = 137438953472, // 0x0000002000000000
    /// <summary>
    /// When returning column values, skip verifying their affinity.
    /// </summary>
    NoVerifyTypeAffinity = 274877906944, // 0x0000004000000000
    /// <summary>
    /// Allow transactions to be nested.  The outermost transaction still
    /// controls whether or not any changes are ultimately committed or
    /// rolled back.  All non-outermost transactions are implemented using
    /// the SAVEPOINT construct.
    /// </summary>
    AllowNestedTransactions = 549755813888, // 0x0000008000000000
    /// <summary>
    /// When binding parameter values, always bind <see cref="T:System.Decimal" />
    /// values as though they were plain text (i.e. not <see cref="T:System.Decimal" />,
    /// which is the legacy behavior).
    /// </summary>
    BindDecimalAsText = 1099511627776, // 0x0000010000000000
    /// <summary>
    /// When returning column values, always return <see cref="T:System.Decimal" />
    /// values as though they were plain text (i.e. not <see cref="T:System.Double" />,
    /// which is the legacy behavior).
    /// </summary>
    GetDecimalAsText = 2199023255552, // 0x0000020000000000
    /// <summary>
    /// When binding <see cref="T:System.Decimal" /> parameter values, always use
    /// the invariant culture when converting their values to strings.
    /// </summary>
    BindInvariantDecimal = 4398046511104, // 0x0000040000000000
    /// <summary>
    /// When returning <see cref="T:System.Decimal" /> column values, always use
    /// the invariant culture when converting their values from strings.
    /// </summary>
    GetInvariantDecimal = 8796093022208, // 0x0000080000000000
    /// <summary>
    /// <b>EXPERIMENTAL</b> --
    /// Enable waiting for the enlistment to be reset prior to attempting
    /// to create a new enlistment.  This may be necessary due to the
    /// semantics used by distributed transactions, which complete
    /// asynchronously.
    /// </summary>
    WaitForEnlistmentReset = 17592186044416, // 0x0000100000000000
    /// <summary>
    /// When returning <see cref="T:System.Int64" /> column values, always use
    /// the invariant culture when converting their values from strings.
    /// </summary>
    GetInvariantInt64 = 35184372088832, // 0x0000200000000000
    /// <summary>
    /// When returning <see cref="T:System.Double" /> column values, always use
    /// the invariant culture when converting their values from strings.
    /// </summary>
    GetInvariantDouble = 70368744177664, // 0x0000400000000000
    /// <summary>
    /// <b>EXPERIMENTAL</b> --
    /// Enable strict conformance to the ADO.NET standard, e.g. use of
    /// thrown exceptions to indicate common error conditions.
    /// </summary>
    StrictConformance = 140737488355328, // 0x0000800000000000
    /// <summary>
    /// <b>EXPERIMENTAL</b> --
    /// When opening a connection, attempt to hide the password from the
    /// connection string, etc.  Given the memory architecture of the CLR,
    /// (and P/Invoke) this is not 100% reliable and should not be relied
    /// upon for security critical uses or applications.
    /// </summary>
    HidePassword = 281474976710656, // 0x0001000000000000
    /// <summary>
    /// When binding parameter values or returning column values, always
    /// treat them as though they were plain text (i.e. no numeric,
    /// date/time, or other conversions should be attempted).
    /// </summary>
    BindAndGetAllAsText = GetAllAsText | BindAllAsText, // 0x0000000000000180
    /// <summary>
    /// When binding parameter values, always use the invariant culture when
    /// converting their values to strings or from strings.
    /// </summary>
    ConvertAndBindInvariantText = BindInvariantText | ConvertInvariantText, // 0x0000000000030000
    /// <summary>
    /// When binding parameter values or returning column values, always
    /// treat them as though they were plain text (i.e. no numeric,
    /// date/time, or other conversions should be attempted) and always
    /// use the invariant culture when converting their values to strings.
    /// </summary>
    BindAndGetAllAsInvariantText = BindAndGetAllAsText | BindInvariantText, // 0x0000000000020180
    /// <summary>
    /// When binding parameter values or returning column values, always
    /// treat them as though they were plain text (i.e. no numeric,
    /// date/time, or other conversions should be attempted) and always
    /// use the invariant culture when converting their values to strings
    /// or from strings.
    /// </summary>
    ConvertAndBindAndGetAllAsInvariantText = BindAndGetAllAsInvariantText | ConvertInvariantText, // 0x0000000000030180
    /// <summary>
    /// Enables use of all per-connection value handling callbacks.
    /// </summary>
    UseConnectionAllValueCallbacks = UseConnectionReadValueCallbacks | UseConnectionBindValueCallbacks, // 0x0000000C00000000
    /// <summary>
    /// Enables use of all applicable <see cref="T:System.Data.SQLite.SQLiteParameter" />
    /// properties as fallbacks for the database type name.
    /// </summary>
    UseParameterAnythingForTypeName = UseParameterDbTypeForTypeName | UseParameterNameForTypeName, // 0x0000003000000000
    /// <summary>Enable all logging.</summary>
    LogAll = LogModuleException | LogModuleError | LogBackup | LogCallbackException | LogBind | LogPreBind | LogPrepare, // 0x000000000000601F
    /// <summary>
    /// The default logging related flags for new connections.
    /// </summary>
    LogDefault = LogModuleException | LogCallbackException, // 0x0000000000004008
    /// <summary>The default extra flags for new connections.</summary>
    Default = LogDefault | GetInvariantDecimal | BindInvariantDecimal, // 0x00000C0000004008
    /// <summary>
    /// The default extra flags for new connections with all logging enabled.
    /// </summary>
    DefaultAndLogAll = Default | LogModuleError | LogBackup | LogBind | LogPreBind | LogPrepare, // 0x00000C000000601F
  }
}
