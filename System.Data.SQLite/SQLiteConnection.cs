// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnection
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Transactions;

namespace System.Data.SQLite
{
  /// <summary>SQLite implentation of DbConnection.</summary>
  /// <remarks>
  /// The <see cref="P:System.Data.SQLite.SQLiteConnection.ConnectionString" /> property can contain the following parameter(s), delimited with a semi-colon:
  /// <list type="table">
  /// <listheader>
  /// <term>Parameter</term>
  /// <term>Values</term>
  /// <term>Required</term>
  /// <term>Default</term>
  /// </listheader>
  /// <item>
  /// <description>Data Source</description>
  /// <description>
  /// This may be a file name, the string ":memory:", or any supported URI (starting with SQLite 3.7.7).
  /// Starting with release 1.0.86.0, in order to use more than one consecutive backslash (e.g. for a
  /// UNC path), each of the adjoining backslash characters must be doubled (e.g. "\\Network\Share\test.db"
  /// would become "\\\\Network\Share\test.db").
  /// </description>
  /// <description>Y</description>
  /// <description></description>
  /// </item>
  /// <item>
  /// <description>Uri</description>
  /// <description>
  /// If specified, this must be a file name that starts with "file://", "file:", or "/".  Any leading
  /// "file://" or "file:" prefix will be stripped off and the resulting file name will be used to open
  /// the database.
  /// </description>
  /// <description>N</description>
  /// <description>null</description>
  /// </item>
  /// <item>
  /// <description>FullUri</description>
  /// <description>
  /// If specified, this must be a URI in a format recognized by the SQLite core library (starting with
  /// SQLite 3.7.7).  It will be passed verbatim to the SQLite core library.
  /// </description>
  /// <description>N</description>
  /// <description>null</description>
  /// </item>
  /// <item>
  /// <description>Version</description>
  /// <description>3</description>
  /// <description>N</description>
  /// <description>3</description>
  /// </item>
  /// <item>
  /// <description>UseUTF16Encoding</description>
  /// <description>
  /// <b>True</b> - The UTF-16 encoding should be used.
  /// <br />
  /// <b>False</b> - The UTF-8 encoding should be used.
  /// </description>
  /// <description>N</description>
  /// <description>False</description>
  /// </item>
  /// <item>
  /// <description>DefaultDbType</description>
  /// <description>
  /// This is the default <see cref="T:System.Data.DbType" /> to use when one cannot be determined based on the
  /// column metadata and the configured type mappings.
  /// </description>
  /// <description>N</description>
  /// <description>null</description>
  /// </item>
  /// <item>
  /// <description>DefaultTypeName</description>
  /// <description>
  /// This is the default type name to use when one cannot be determined based on the column metadata
  /// and the configured type mappings.
  /// </description>
  /// <description>N</description>
  /// <description>null</description>
  /// </item>
  /// <item>
  /// <description>NoDefaultFlags</description>
  /// <description>
  /// <b>True</b> - Do not combine the specified (or existing) connection flags with the value of the
  /// <see cref="P:System.Data.SQLite.SQLiteConnection.DefaultFlags" /> property.
  /// <br />
  /// <b>False</b> - Combine the specified (or existing) connection flags with the value of the
  /// <see cref="P:System.Data.SQLite.SQLiteConnection.DefaultFlags" /> property.
  /// </description>
  /// <description>N</description>
  /// <description>False</description>
  /// </item>
  /// <item>
  /// <description>NoSharedFlags</description>
  /// <description>
  /// <b>True</b> - Do not combine the specified (or existing) connection flags with the value of the
  /// <see cref="P:System.Data.SQLite.SQLiteConnection.SharedFlags" /> property.
  /// <br />
  /// <b>False</b> - Combine the specified (or existing) connection flags with the value of the
  /// <see cref="P:System.Data.SQLite.SQLiteConnection.SharedFlags" /> property.
  /// </description>
  /// <description>N</description>
  /// <description>False</description>
  /// </item>
  /// <item>
  /// <description>VfsName</description>
  /// <description>
  /// The name of the VFS to use when opening the database connection.
  /// If this is not specified, the default VFS will be used.
  /// </description>
  /// <description>N</description>
  /// <description>null</description>
  /// </item>
  /// <item>
  /// <description>ZipVfsVersion</description>
  /// <description>
  /// If non-null, this is the "version" of ZipVFS to use.  This requires
  /// the System.Data.SQLite interop assembly -AND- primary managed assembly
  /// to be compiled with the INTEROP_INCLUDE_ZIPVFS option; otherwise, this
  /// property does nothing.  The valid values are "v2" and "v3".  Using
  /// anyother value will cause an exception to be thrown.  Please see the
  /// ZipVFS documentation for more information on how to use this parameter.
  /// </description>
  /// <description>N</description>
  /// <description>null</description>
  /// </item>
  /// <item>
  /// <description>DateTimeFormat</description>
  /// <description>
  /// <b>Ticks</b> - Use the value of DateTime.Ticks.<br />
  /// <b>ISO8601</b> - Use the ISO-8601 format.  Uses the "yyyy-MM-dd HH:mm:ss.FFFFFFFK" format for UTC
  /// DateTime values and "yyyy-MM-dd HH:mm:ss.FFFFFFF" format for local DateTime values).<br />
  /// <b>JulianDay</b> - The interval of time in days and fractions of a day since January 1, 4713 BC.<br />
  /// <b>UnixEpoch</b> - The whole number of seconds since the Unix epoch (January 1, 1970).<br />
  /// <b>InvariantCulture</b> - Any culture-independent string value that the .NET Framework can interpret as a valid DateTime.<br />
  /// <b>CurrentCulture</b> - Any string value that the .NET Framework can interpret as a valid DateTime using the current culture.</description>
  /// <description>N</description>
  /// <description>ISO8601</description>
  /// </item>
  /// <item>
  /// <description>DateTimeKind</description>
  /// <description>
  /// <b>Unspecified</b> - Not specified as either UTC or local time.
  /// <br />
  /// <b>Utc</b> - The time represented is UTC.
  /// <br />
  /// <b>Local</b> - The time represented is local time.
  /// </description>
  /// <description>N</description>
  /// <description>Unspecified</description>
  /// </item>
  /// <item>
  /// <description>DateTimeFormatString</description>
  /// <description>
  /// The exact DateTime format string to use for all formatting and parsing of all DateTime
  /// values for this connection.
  /// </description>
  /// <description>N</description>
  /// <description>null</description>
  /// </item>
  /// <item>
  /// <description>BaseSchemaName</description>
  /// <description>
  /// Some base data classes in the framework (e.g. those that build SQL queries dynamically)
  /// assume that an ADO.NET provider cannot support an alternate catalog (i.e. database) without supporting
  /// alternate schemas as well; however, SQLite does not fit into this model.  Therefore, this value is used
  /// as a placeholder and removed prior to preparing any SQL statements that may contain it.
  /// </description>
  /// <description>N</description>
  /// <description>sqlite_default_schema</description>
  /// </item>
  /// <item>
  /// <description>BinaryGUID</description>
  /// <description>
  /// <b>True</b> - Store GUID columns in binary form
  /// <br />
  /// <b>False</b> - Store GUID columns as text
  /// </description>
  /// <description>N</description>
  /// <description>True</description>
  /// </item>
  /// <item>
  /// <description>Cache Size</description>
  /// <description>
  /// If the argument N is positive then the suggested cache size is set to N.
  /// If the argument N is negative, then the number of cache pages is adjusted
  /// to use approximately abs(N*4096) bytes of memory. Backwards compatibility
  /// note: The behavior of cache_size with a negative N was different in SQLite
  /// versions prior to 3.7.10. In version 3.7.9 and earlier, the number of
  /// pages in the cache was set to the absolute value of N.
  /// </description>
  /// <description>N</description>
  /// <description>-2000</description>
  /// </item>
  /// <item>
  /// <description>Synchronous</description>
  /// <description>
  /// <b>Normal</b> - Normal file flushing behavior
  /// <br />
  /// <b>Full</b> - Full flushing after all writes
  /// <br />
  /// <b>Off</b> - Underlying OS flushes I/O's
  /// </description>
  /// <description>N</description>
  /// <description>Full</description>
  /// </item>
  /// <item>
  /// <description>Page Size</description>
  /// <description>{size in bytes}</description>
  /// <description>N</description>
  /// <description>4096</description>
  /// </item>
  /// <item>
  /// <description>Password</description>
  /// <description>
  /// {password} - Using this parameter requires that the legacy CryptoAPI based
  /// codec (or the SQLite Encryption Extension) be enabled at compile-time for
  /// both the native interop assembly and the core managed assemblies; otherwise,
  /// using this parameter may result in an exception being thrown when attempting
  /// to open the connection.
  /// </description>
  /// <description>N</description>
  /// <description></description>
  /// </item>
  /// <item>
  /// <description>HexPassword</description>
  /// <description>
  /// {hexPassword} - Must contain a sequence of zero or more hexadecimal encoded
  /// byte values without a leading "0x" prefix.  Using this parameter requires
  /// that the legacy CryptoAPI based codec (or the SQLite Encryption Extension)
  /// be enabled at compile-time for both the native interop assembly and the
  /// core managed assemblies; otherwise, using this parameter may result in an
  /// exception being thrown when attempting to open the connection.
  /// </description>
  /// <description>N</description>
  /// <description></description>
  /// </item>
  /// <item>
  /// <description>Enlist</description>
  /// <description>
  /// <b>Y</b> - Automatically enlist in distributed transactions
  /// <br />
  /// <b>N</b> - No automatic enlistment
  /// </description>
  /// <description>N</description>
  /// <description>Y</description>
  /// </item>
  /// <item>
  /// <description>Pooling</description>
  /// <description>
  /// <b>True</b> - Use connection pooling.<br />
  /// <b>False</b> - Do not use connection pooling.<br /><br />
  /// <b>WARNING:</b> When using the default connection pool implementation,
  /// setting this property to True should be avoided by applications that make
  /// use of COM (either directly or indirectly) due to possible deadlocks that
  /// can occur during the finalization of some COM objects.
  /// </description>
  /// <description>N</description>
  /// <description>False</description>
  /// </item>
  /// <item>
  /// <description>FailIfMissing</description>
  /// <description>
  /// <b>True</b> - Don't create the database if it does not exist, throw an error instead
  /// <br />
  /// <b>False</b> - Automatically create the database if it does not exist
  /// </description>
  /// <description>N</description>
  /// <description>False</description>
  /// </item>
  /// <item>
  /// <description>Max Page Count</description>
  /// <description>{size in pages} - Limits the maximum number of pages (limits the size) of the database</description>
  /// <description>N</description>
  /// <description>0</description>
  /// </item>
  /// <item>
  /// <description>Legacy Format</description>
  /// <description>
  /// <b>True</b> - Use the more compatible legacy 3.x database format
  /// <br />
  /// <b>False</b> - Use the newer 3.3x database format which compresses numbers more effectively
  /// </description>
  /// <description>N</description>
  /// <description>False</description>
  /// </item>
  /// <item>
  /// <description>Default Timeout</description>
  /// <description>{time in seconds}<br />The default command timeout</description>
  /// <description>N</description>
  /// <description>30</description>
  /// </item>
  /// <item>
  /// <description>BusyTimeout</description>
  /// <description>{time in milliseconds}<br />Sets the busy timeout for the core library.</description>
  /// <description>N</description>
  /// <description>0</description>
  /// </item>
  /// <item>
  /// <description>WaitTimeout</description>
  /// <description>{time in milliseconds}<br />
  /// <b>EXPERIMENTAL</b> -- The wait timeout to use with
  /// <see cref="M:System.Data.SQLite.SQLiteConnection.WaitForEnlistmentReset(System.Int32,System.Nullable{System.Boolean})" /> method.  This is only used when
  /// waiting for the enlistment to be reset prior to enlisting in a transaction,
  /// and then only when the appropriate connection flag is set.</description>
  /// <description>N</description>
  /// <description>30000</description>
  /// </item>
  /// <item>
  /// <description>Journal Mode</description>
  /// <description>
  /// <b>Delete</b> - Delete the journal file after a commit.
  /// <br />
  /// <b>Persist</b> - Zero out and leave the journal file on disk after a
  /// commit.
  /// <br />
  /// <b>Off</b> - Disable the rollback journal entirely.  This saves disk I/O
  /// but at the expense of database safety and integrity.  If the application
  /// using SQLite crashes in the middle of a transaction when this journaling
  /// mode is set, then the database file will very likely go corrupt.
  /// <br />
  /// <b>Truncate</b> - Truncate the journal file to zero-length instead of
  /// deleting it.
  /// <br />
  /// <b>Memory</b> - Store the journal in volatile RAM.  This saves disk I/O
  /// but at the expense of database safety and integrity.  If the application
  /// using SQLite crashes in the middle of a transaction when this journaling
  /// mode is set, then the database file will very likely go corrupt.
  /// <br />
  /// <b>Wal</b> - Use a write-ahead log instead of a rollback journal.
  /// </description>
  /// <description>N</description>
  /// <description>Delete</description>
  /// </item>
  /// <item>
  /// <description>Read Only</description>
  /// <description>
  /// <b>True</b> - Open the database for read only access
  /// <br />
  /// <b>False</b> - Open the database for normal read/write access
  /// </description>
  /// <description>N</description>
  /// <description>False</description>
  /// </item>
  /// <item>
  /// <description>Max Pool Size</description>
  /// <description>The maximum number of connections for the given connection string that can be in the connection pool</description>
  /// <description>N</description>
  /// <description>100</description>
  /// </item>
  /// <item>
  /// <description>Default IsolationLevel</description>
  /// <description>The default transaciton isolation level</description>
  /// <description>N</description>
  /// <description>Serializable</description>
  /// </item>
  /// <item>
  /// <description>Foreign Keys</description>
  /// <description>Enable foreign key constraints</description>
  /// <description>N</description>
  /// <description>False</description>
  /// </item>
  /// <item>
  /// <description>Flags</description>
  /// <description>Extra behavioral flags for the connection.  See the <see cref="T:System.Data.SQLite.SQLiteConnectionFlags" /> enumeration for possible values.</description>
  /// <description>N</description>
  /// <description>Default</description>
  /// </item>
  /// <item>
  /// <description>SetDefaults</description>
  /// <description>
  /// <b>True</b> - Apply the default connection settings to the opened database.<br />
  /// <b>False</b> - Skip applying the default connection settings to the opened database.
  /// </description>
  /// <description>N</description>
  /// <description>True</description>
  /// </item>
  /// <item>
  /// <description>ToFullPath</description>
  /// <description>
  /// <b>True</b> - Attempt to expand the data source file name to a fully qualified path before opening.
  /// <br />
  /// <b>False</b> - Skip attempting to expand the data source file name to a fully qualified path before opening.
  /// </description>
  /// <description>N</description>
  /// <description>True</description>
  /// </item>
  /// <item>
  /// <description>PrepareRetries</description>
  /// <description>
  /// The maximum number of retries when preparing SQL to be executed.  This
  /// normally only applies to preparation errors resulting from the database
  /// schema being changed.
  /// </description>
  /// <description>N</description>
  /// <description>3</description>
  /// </item>
  /// <item>
  /// <description>ProgressOps</description>
  /// <description>
  /// The approximate number of virtual machine instructions between progress
  /// events.  In order for progress events to actually fire, the event handler
  /// must be added to the <see cref="E:System.Data.SQLite.SQLiteConnection.Progress" /> event as well.
  /// </description>
  /// <description>N</description>
  /// <description>0</description>
  /// </item>
  /// <item>
  /// <description>Recursive Triggers</description>
  /// <description>
  /// <b>True</b> - Enable the recursive trigger capability.
  /// <b>False</b> - Disable the recursive trigger capability.
  /// </description>
  /// <description>N</description>
  /// <description>False</description>
  /// </item>
  /// </list>
  /// </remarks>
  public sealed class SQLiteConnection : DbConnection, ICloneable, IDisposable
  {
    /// <summary>
    /// The "invalid value" for the <see cref="T:System.Data.DbType" /> enumeration used
    /// by the <see cref="P:System.Data.SQLite.SQLiteConnection.DefaultDbType" /> property.  This constant is shared
    /// by this class and the SQLiteConnectionStringBuilder class.
    /// </summary>
    internal const DbType BadDbType = ~DbType.AnsiString;
    /// <summary>
    /// The default "stub" (i.e. placeholder) base schema name to use when
    /// returning column schema information.  Used as the initial value of
    /// the BaseSchemaName property.  This should start with "sqlite_*"
    /// because those names are reserved for use by SQLite (i.e. they cannot
    /// be confused with the names of user objects).
    /// </summary>
    internal const string DefaultBaseSchemaName = "sqlite_default_schema";
    private const string MemoryFileName = ":memory:";
    internal const System.Data.IsolationLevel DeferredIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
    internal const System.Data.IsolationLevel ImmediateIsolationLevel = System.Data.IsolationLevel.Serializable;
    private const SQLiteConnectionFlags FallbackDefaultFlags = SQLiteConnectionFlags.Default;
    private const SQLiteSynchronousEnum DefaultSynchronous = SQLiteSynchronousEnum.Default;
    private const SQLiteJournalModeEnum DefaultJournalMode = SQLiteJournalModeEnum.Default;
    private const System.Data.IsolationLevel DefaultIsolationLevel = System.Data.IsolationLevel.Serializable;
    internal const SQLiteDateFormats DefaultDateTimeFormat = SQLiteDateFormats.ISO8601;
    internal const DateTimeKind DefaultDateTimeKind = DateTimeKind.Unspecified;
    internal const string DefaultDateTimeFormatString = null;
    private const string DefaultDataSource = null;
    private const string DefaultUri = null;
    private const string DefaultFullUri = null;
    private const string DefaultHexPassword = null;
    private const string DefaultPassword = null;
    private const int DefaultVersion = 3;
    private const int DefaultPageSize = 4096;
    private const int DefaultMaxPageCount = 0;
    private const int DefaultCacheSize = -2000;
    private const int DefaultMaxPoolSize = 100;
    private const int DefaultConnectionTimeout = 30;
    private const int DefaultBusyTimeout = 0;
    private const int DefaultWaitTimeout = 30000;
    private const bool DefaultNoDefaultFlags = false;
    private const bool DefaultNoSharedFlags = false;
    private const bool DefaultFailIfMissing = false;
    private const bool DefaultReadOnly = false;
    internal const bool DefaultBinaryGUID = true;
    private const bool DefaultUseUTF16Encoding = false;
    private const bool DefaultToFullPath = true;
    private const bool DefaultPooling = false;
    private const bool DefaultLegacyFormat = false;
    private const bool DefaultForeignKeys = false;
    private const bool DefaultRecursiveTriggers = false;
    private const bool DefaultEnlist = true;
    private const bool DefaultSetDefaults = true;
    internal const int DefaultPrepareRetries = 3;
    private const string DefaultVfsName = null;
    private const int DefaultProgressOps = 0;
    private const int SQLITE_FCNTL_CHUNK_SIZE = 6;
    private const int SQLITE_FCNTL_WIN32_AV_RETRY = 9;
    private const string _dataDirectory = "|DataDirectory|";
    private static string _defaultCatalogName = "main";
    private static string _defaultMasterTableName = "sqlite_master";
    private static string _temporaryCatalogName = "temp";
    private static string _temporaryMasterTableName = "sqlite_temp_master";
    /// <summary>The managed assembly containing this type.</summary>
    private static readonly Assembly _assembly = typeof (SQLiteConnection).Assembly;
    /// <summary>
    /// Object used to synchronize access to the static instance data
    /// for this class.
    /// </summary>
    private static readonly object _syncRoot = new object();
    /// <summary>
    /// The extra connection flags to be used for all opened connections.
    /// </summary>
    private static SQLiteConnectionFlags _sharedFlags;
    /// <summary>
    /// The <see cref="T:System.Data.SQLite.SQLiteConnection" /> instance (for this thread) that
    /// had the most recent call to <see cref="M:System.Data.SQLite.SQLiteConnection.Open" />.
    /// </summary>
    [ThreadStatic]
    private static SQLiteConnection _lastConnectionInOpen;
    /// <summary>State of the current connection</summary>
    private ConnectionState _connectionState;
    /// <summary>The connection string</summary>
    private string _connectionString;
    /// <summary>
    /// Nesting level of the transactions open on the connection
    /// </summary>
    internal int _transactionLevel;
    /// <summary>
    /// Transaction counter for the connection.  Currently, this is only used
    /// to build SAVEPOINT names.
    /// </summary>
    internal int _transactionSequence;
    /// <summary>
    /// If this flag is non-zero, the <see cref="M:System.Data.SQLite.SQLiteConnection.Dispose" /> method will have
    /// no effect; however, the <see cref="M:System.Data.SQLite.SQLiteConnection.Close" /> method will continue to
    /// behave as normal.
    /// </summary>
    internal bool _noDispose;
    /// <summary>
    /// If set, then the connection is currently being disposed.
    /// </summary>
    private bool _disposing;
    /// <summary>The default isolation level for new transactions</summary>
    private System.Data.IsolationLevel _defaultIsolation;
    /// <summary>
    /// This object is used with lock statements to synchronize access to the
    /// <see cref="F:System.Data.SQLite.SQLiteConnection._enlistment" /> field, below.
    /// </summary>
    internal readonly object _enlistmentSyncRoot = new object();
    /// <summary>
    /// Whether or not the connection is enlisted in a distrubuted transaction
    /// </summary>
    internal SQLiteEnlistment _enlistment;
    /// <summary>
    /// The per-connection mappings between type names and <see cref="T:System.Data.DbType" />
    /// values.  These mappings override the corresponding global mappings.
    /// </summary>
    internal SQLiteDbTypeMap _typeNames;
    /// <summary>
    /// The per-connection mappings between type names and optional callbacks
    /// for parameter binding and value reading.
    /// </summary>
    private SQLiteTypeCallbacksMap _typeCallbacks;
    /// <summary>The base SQLite object to interop with</summary>
    internal SQLiteBase _sql;
    /// <summary>The database filename minus path and extension</summary>
    private string _dataSource;
    /// <summary>
    /// Temporary password storage, emptied after the database has been opened
    /// </summary>
    private byte[] _password;
    /// <summary>
    /// The "stub" (i.e. placeholder) base schema name to use when returning
    /// column schema information.
    /// </summary>
    internal string _baseSchemaName;
    /// <summary>
    /// The extra behavioral flags for this connection, if any.  See the
    /// <see cref="T:System.Data.SQLite.SQLiteConnectionFlags" /> enumeration for a list of
    /// possible values.
    /// </summary>
    private SQLiteConnectionFlags _flags;
    /// <summary>
    /// The cached values for all settings that have been fetched on behalf
    /// of this connection.  This cache may be cleared by calling the
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.ClearCachedSettings" /> method.
    /// </summary>
    private Dictionary<string, object> _cachedSettings;
    /// <summary>
    /// The default databse type for this connection.  This value will only
    /// be used if the <see cref="F:System.Data.SQLite.SQLiteConnectionFlags.UseConnectionTypes" />
    /// flag is set.
    /// </summary>
    private DbType? _defaultDbType;
    /// <summary>
    /// The default databse type name for this connection.  This value will only
    /// be used if the <see cref="F:System.Data.SQLite.SQLiteConnectionFlags.UseConnectionTypes" />
    /// flag is set.
    /// </summary>
    private string _defaultTypeName;
    /// <summary>
    /// The name of the VFS to be used when opening the database connection.
    /// </summary>
    private string _vfsName;
    /// <summary>Default command timeout</summary>
    private int _defaultTimeout = 30;
    /// <summary>
    /// The default busy timeout to use with the SQLite core library.  This is
    /// only used when opening a connection.
    /// </summary>
    private int _busyTimeout;
    /// <summary>
    /// The default wait timeout to use with <see cref="M:System.Data.SQLite.SQLiteConnection.WaitForEnlistmentReset(System.Int32,System.Nullable{System.Boolean})" />
    /// method.  This is only used when waiting for the enlistment to be reset
    /// prior to enlisting in a transaction, and then only when the appropriate
    /// connection flag is set.
    /// </summary>
    private int _waitTimeout = 30000;
    /// <summary>
    /// The maximum number of retries when preparing SQL to be executed.  This
    /// normally only applies to preparation errors resulting from the database
    /// schema being changed.
    /// </summary>
    internal int _prepareRetries = 3;
    /// <summary>
    /// The approximate number of virtual machine instructions between progress
    /// events.  In order for progress events to actually fire, the event handler
    /// must be added to the <see cref="E:System.Data.SQLite.SQLiteConnection.Progress" /> event as
    /// well.  This value will only be used when opening the database.
    /// </summary>
    private int _progressOps;
    /// <summary>
    /// Non-zero if the built-in (i.e. framework provided) connection string
    /// parser should be used when opening the connection.
    /// </summary>
    private bool _parseViaFramework;
    internal bool _binaryGuid;
    internal int _version;
    private SQLiteProgressCallback _progressCallback;
    private SQLiteAuthorizerCallback _authorizerCallback;
    private SQLiteUpdateCallback _updateCallback;
    private SQLiteCommitCallback _commitCallback;
    private SQLiteTraceCallback _traceCallback;
    private SQLiteRollbackCallback _rollbackCallback;
    private bool disposed;

    /// <summary>
    /// Static variable to store the connection event handlers to call.
    /// </summary>
    private static event SQLiteConnectionEventHandler _handlers;

    private event SQLiteProgressEventHandler _progressHandler;

    private event SQLiteAuthorizerEventHandler _authorizerHandler;

    private event SQLiteUpdateEventHandler _updateHandler;

    private event SQLiteCommitHandler _commitHandler;

    private event SQLiteTraceEventHandler _traceHandler;

    private event EventHandler _rollbackHandler;

    private static string GetDefaultCatalogName() => SQLiteConnection._defaultCatalogName;

    private static bool IsDefaultCatalogName(string catalogName) => string.Compare(catalogName, SQLiteConnection.GetDefaultCatalogName(), StringComparison.OrdinalIgnoreCase) == 0;

    private static string GetTemporaryCatalogName() => SQLiteConnection._temporaryCatalogName;

    private static bool IsTemporaryCatalogName(string catalogName) => string.Compare(catalogName, SQLiteConnection.GetTemporaryCatalogName(), StringComparison.OrdinalIgnoreCase) == 0;

    private static string GetMasterTableName(bool temporary) => !temporary ? SQLiteConnection._defaultMasterTableName : SQLiteConnection._temporaryMasterTableName;

    /// <summary>
    /// This event is raised whenever the database is opened or closed.
    /// </summary>
    public override event StateChangeEventHandler StateChange;

    /// <overloads>Constructs a new SQLiteConnection object</overloads>
    /// <summary>Default constructor</summary>
    public SQLiteConnection()
      : this((string) null)
    {
    }

    /// <summary>
    /// Initializes the connection with the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to use.</param>
    public SQLiteConnection(string connectionString)
      : this(connectionString, false)
    {
    }

    /// <summary>
    /// Initializes the connection with a pre-existing native connection handle.
    /// This constructor overload is intended to be used only by the private
    /// <see cref="M:System.Data.SQLite.SQLiteModule.CreateOrConnect(System.Boolean,System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.IntPtr@,System.IntPtr@)" /> method.
    /// </summary>
    /// <param name="db">The native connection handle to use.</param>
    /// <param name="fileName">
    /// The file name corresponding to the native connection handle.
    /// </param>
    /// <param name="ownHandle">
    /// Non-zero if this instance owns the native connection handle and
    /// should dispose of it when it is no longer needed.
    /// </param>
    internal SQLiteConnection(IntPtr db, string fileName, bool ownHandle)
      : this()
    {
      this._sql = (SQLiteBase) new SQLite3(SQLiteDateFormats.ISO8601, DateTimeKind.Unspecified, (string) null, db, fileName, ownHandle);
      this._flags = SQLiteConnectionFlags.None;
      this._connectionState = db != IntPtr.Zero ? ConnectionState.Open : ConnectionState.Closed;
      this._connectionString = (string) null;
    }

    /// <summary>
    /// Initializes the connection with the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to use.</param>
    /// <param name="parseViaFramework">
    /// Non-zero to parse the connection string using the built-in (i.e.
    /// framework provided) parser when opening the connection.
    /// </param>
    public SQLiteConnection(string connectionString, bool parseViaFramework)
    {
      this._noDispose = false;
      UnsafeNativeMethods.Initialize();
      SQLiteLog.Initialize(typeof (SQLiteConnection).Name);
      this._cachedSettings = new Dictionary<string, object>((IEqualityComparer<string>) new TypeNameStringComparer());
      this._typeNames = new SQLiteDbTypeMap();
      this._typeCallbacks = new SQLiteTypeCallbacksMap();
      this._parseViaFramework = parseViaFramework;
      this._flags = SQLiteConnectionFlags.None;
      this._defaultDbType = new DbType?();
      this._defaultTypeName = (string) null;
      this._vfsName = (string) null;
      this._connectionState = ConnectionState.Closed;
      this._connectionString = (string) null;
      if (connectionString == null)
        return;
      this.ConnectionString = connectionString;
    }

    /// <summary>
    /// Clones the settings and connection string from an existing connection.  If the existing connection is already open, this
    /// function will open its own connection, enumerate any attached databases of the original connection, and automatically
    /// attach to them.
    /// </summary>
    /// <param name="connection">The connection to copy the settings from.</param>
    public SQLiteConnection(SQLiteConnection connection)
      : this(connection.ConnectionString, connection.ParseViaFramework)
    {
      if (connection.State != ConnectionState.Open)
        return;
      this.Open();
      using (DataTable schema = connection.GetSchema("Catalogs"))
      {
        foreach (DataRow row in (InternalDataCollectionBase) schema.Rows)
        {
          string catalogName = row[0].ToString();
          if (!SQLiteConnection.IsDefaultCatalogName(catalogName) && !SQLiteConnection.IsTemporaryCatalogName(catalogName))
          {
            using (SQLiteCommand command = this.CreateCommand())
            {
              command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "ATTACH DATABASE '{0}' AS [{1}]", row[1], row[0]);
              command.ExecuteNonQuery();
            }
          }
        }
      }
    }

    /// <summary>
    /// Attempts to lookup the native handle associated with the connection.  An exception will
    /// be thrown if this cannot be accomplished.
    /// </summary>
    /// <param name="connection">
    /// The connection associated with the desired native handle.
    /// </param>
    /// <returns>
    /// The native handle associated with the connection or <see cref="F:System.IntPtr.Zero" /> if it
    /// cannot be determined.
    /// </returns>
    private static SQLiteConnectionHandle GetNativeHandle(
      SQLiteConnection connection)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (!(connection._sql is SQLite3 sql1))
        throw new InvalidOperationException("Connection has no wrapper");
      SQLiteConnectionHandle sql2 = sql1._sql;
      if (sql2 == null)
        throw new InvalidOperationException("Connection has an invalid handle.");
      return !((IntPtr) sql2 == IntPtr.Zero) ? sql2 : throw new InvalidOperationException("Connection has an invalid handle pointer.");
    }

    /// <summary>
    /// Raises the <see cref="E:System.Data.SQLite.SQLiteConnection.Changed" /> event.
    /// </summary>
    /// <param name="connection">
    /// The connection associated with this event.  If this parameter is not
    /// null and the specified connection cannot raise events, then the
    /// registered event handlers will not be invoked.
    /// </param>
    /// <param name="e">
    /// A <see cref="T:System.Data.SQLite.ConnectionEventArgs" /> that contains the event data.
    /// </param>
    internal static void OnChanged(SQLiteConnection connection, ConnectionEventArgs e)
    {
      if (connection != null && !connection.CanRaiseEvents)
        return;
      SQLiteConnectionEventHandler connectionEventHandler;
      lock (SQLiteConnection._syncRoot)
        connectionEventHandler = SQLiteConnection._handlers == null ? (SQLiteConnectionEventHandler) null : SQLiteConnection._handlers.Clone() as SQLiteConnectionEventHandler;
      if (connectionEventHandler == null)
        return;
      connectionEventHandler((object) connection, e);
    }

    /// <summary>
    /// This event is raised when events related to the lifecycle of a
    /// SQLiteConnection object occur.
    /// </summary>
    public static event SQLiteConnectionEventHandler Changed
    {
      add
      {
        lock (SQLiteConnection._syncRoot)
        {
          SQLiteConnection._handlers -= value;
          SQLiteConnection._handlers += value;
        }
      }
      remove
      {
        lock (SQLiteConnection._syncRoot)
          SQLiteConnection._handlers -= value;
      }
    }

    /// <summary>
    /// This property is used to obtain or set the custom connection pool
    /// implementation to use, if any.  Setting this property to null will
    /// cause the default connection pool implementation to be used.
    /// </summary>
    public static ISQLiteConnectionPool ConnectionPool
    {
      get => SQLiteConnectionPool.GetConnectionPool();
      set => SQLiteConnectionPool.SetConnectionPool(value);
    }

    /// <summary>
    /// Creates and returns a new managed database connection handle.  This
    /// method is intended to be used by implementations of the
    /// <see cref="T:System.Data.SQLite.ISQLiteConnectionPool" /> interface only.  In theory, it
    /// could be used by other classes; however, that usage is not supported.
    /// </summary>
    /// <param name="nativeHandle">
    /// This must be a native database connection handle returned by the
    /// SQLite core library and it must remain valid and open during the
    /// entire duration of the calling method.
    /// </param>
    /// <returns>
    /// The new managed database connection handle or null if it cannot be
    /// created.
    /// </returns>
    public static object CreateHandle(IntPtr nativeHandle)
    {
      SQLiteConnectionHandle connectionHandle;
      try
      {
      }
      finally
      {
        connectionHandle = nativeHandle != IntPtr.Zero ? new SQLiteConnectionHandle(nativeHandle, true) : (SQLiteConnectionHandle) null;
      }
      if (connectionHandle != null)
        SQLiteConnection.OnChanged((SQLiteConnection) null, new ConnectionEventArgs(SQLiteConnectionEventType.NewCriticalHandle, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) connectionHandle, (string) null, (object) new object[2]
        {
          (object) typeof (SQLiteConnection),
          (object) nativeHandle
        }));
      return (object) connectionHandle;
    }

    /// <summary>
    /// Backs up the database, using the specified database connection as the
    /// destination.
    /// </summary>
    /// <param name="destination">The destination database connection.</param>
    /// <param name="destinationName">The destination database name.</param>
    /// <param name="sourceName">The source database name.</param>
    /// <param name="pages">
    /// The number of pages to copy at a time -OR- a negative value to copy all
    /// pages.  When a negative value is used, the <paramref name="callback" />
    /// may never be invoked.
    /// </param>
    /// <param name="callback">
    /// The method to invoke between each step of the backup process.  This
    /// parameter may be null (i.e. no callbacks will be performed).  If the
    /// callback returns false -OR- throws an exception, the backup is canceled.
    /// </param>
    /// <param name="retryMilliseconds">
    /// The number of milliseconds to sleep after encountering a locking error
    /// during the backup process.  A value less than zero means that no sleep
    /// should be performed.
    /// </param>
    public void BackupDatabase(
      SQLiteConnection destination,
      string destinationName,
      string sourceName,
      int pages,
      SQLiteBackupCallback callback,
      int retryMilliseconds)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException("Source database is not open.");
      if (destination == null)
        throw new ArgumentNullException(nameof (destination));
      if (destination._connectionState != ConnectionState.Open)
        throw new ArgumentException("Destination database is not open.", nameof (destination));
      if (destinationName == null)
        throw new ArgumentNullException(nameof (destinationName));
      if (sourceName == null)
        throw new ArgumentNullException(nameof (sourceName));
      SQLiteBase sql = this._sql;
      if (sql == null)
        throw new InvalidOperationException("Connection object has an invalid handle.");
      SQLiteBackup backup = (SQLiteBackup) null;
      try
      {
        backup = sql.InitializeBackup(destination, destinationName, sourceName);
        bool retry = false;
        while (sql.StepBackup(backup, pages, ref retry) && (callback == null || callback(this, sourceName, destination, destinationName, pages, sql.RemainingBackup(backup), sql.PageCountBackup(backup), retry)))
        {
          if (retry && retryMilliseconds >= 0)
            Thread.Sleep(retryMilliseconds);
          if (pages == 0)
            break;
        }
      }
      catch (Exception ex)
      {
        if (HelperMethods.LogBackup(this._flags))
          SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception while backing up database: {0}", (object) ex));
        throw;
      }
      finally
      {
        if (backup != null)
          sql.FinishBackup(backup);
      }
    }

    /// <summary>Clears the per-connection cached settings.</summary>
    /// <returns>The total number of per-connection settings cleared.</returns>
    public int ClearCachedSettings()
    {
      this.CheckDisposed();
      int num = -1;
      if (this._cachedSettings != null)
      {
        num = this._cachedSettings.Count;
        this._cachedSettings.Clear();
      }
      return num;
    }

    /// <summary>
    /// Queries and returns the value of the specified setting, using the
    /// cached setting names and values for this connection, when available.
    /// </summary>
    /// <param name="name">The name of the setting.</param>
    /// <param name="default">
    /// The value to be returned if the setting has not been set explicitly
    /// or cannot be determined.
    /// </param>
    /// <param name="value">
    /// The value of the cached setting is stored here if found; otherwise,
    /// the value of <paramref name="default" /> is stored here.
    /// </param>
    /// <returns>
    /// Non-zero if the cached setting was found; otherwise, zero.
    /// </returns>
    internal bool TryGetCachedSetting(string name, object @default, out object value)
    {
      if (name != null && this._cachedSettings != null)
        return this._cachedSettings.TryGetValue(name, out value);
      value = @default;
      return false;
    }

    /// <summary>
    /// Adds or sets the cached setting specified by <paramref name="name" />
    /// to the value specified by <paramref name="value" />.
    /// </summary>
    /// <param name="name">
    /// The name of the cached setting to add or replace.
    /// </param>
    /// <param name="value">The new value of the cached setting.</param>
    internal void SetCachedSetting(string name, object value)
    {
      if (name == null || this._cachedSettings == null)
        return;
      this._cachedSettings[name] = value;
    }

    /// <summary>Clears the per-connection type mappings.</summary>
    /// <returns>
    /// The total number of per-connection type mappings cleared.
    /// </returns>
    public int ClearTypeMappings()
    {
      this.CheckDisposed();
      int num = -1;
      if (this._typeNames != null)
        num = this._typeNames.Clear();
      return num;
    }

    /// <summary>Returns the per-connection type mappings.</summary>
    /// <returns>
    /// The per-connection type mappings -OR- null if they are unavailable.
    /// </returns>
    public Dictionary<string, object> GetTypeMappings()
    {
      this.CheckDisposed();
      Dictionary<string, object> dictionary = (Dictionary<string, object>) null;
      if (this._typeNames != null)
      {
        dictionary = new Dictionary<string, object>(this._typeNames.Count, this._typeNames.Comparer);
        foreach (KeyValuePair<string, SQLiteDbTypeMapping> typeName in (Dictionary<string, SQLiteDbTypeMapping>) this._typeNames)
        {
          SQLiteDbTypeMapping liteDbTypeMapping = typeName.Value;
          object obj1 = (object) null;
          object obj2 = (object) null;
          object obj3 = (object) null;
          if (liteDbTypeMapping != null)
          {
            obj1 = (object) liteDbTypeMapping.typeName;
            obj2 = (object) liteDbTypeMapping.dataType;
            obj3 = (object) liteDbTypeMapping.primary;
          }
          dictionary.Add(typeName.Key, (object) new object[3]
          {
            obj1,
            obj2,
            obj3
          });
        }
      }
      return dictionary;
    }

    /// <summary>
    /// Adds a per-connection type mapping, possibly replacing one or more
    /// that already exist.
    /// </summary>
    /// <param name="typeName">
    /// The case-insensitive database type name (e.g. "MYDATE").  The value
    /// of this parameter cannot be null.  Using an empty string value (or
    /// a string value consisting entirely of whitespace) for this parameter
    /// is not recommended.
    /// </param>
    /// <param name="dataType">
    /// The <see cref="T:System.Data.DbType" /> value that should be associated with the
    /// specified type name.
    /// </param>
    /// <param name="primary">
    /// Non-zero if this mapping should be considered to be the primary one
    /// for the specified <see cref="T:System.Data.DbType" />.
    /// </param>
    /// <returns>
    /// A negative value if nothing was done.  Zero if no per-connection type
    /// mappings were replaced (i.e. it was a pure add operation).  More than
    /// zero if some per-connection type mappings were replaced.
    /// </returns>
    public int AddTypeMapping(string typeName, DbType dataType, bool primary)
    {
      this.CheckDisposed();
      if (typeName == null)
        throw new ArgumentNullException(nameof (typeName));
      int num = -1;
      if (this._typeNames != null)
      {
        num = 0;
        if (primary && this._typeNames.ContainsKey(dataType))
          num += this._typeNames.Remove(dataType) ? 1 : 0;
        if (this._typeNames.ContainsKey(typeName))
          num += this._typeNames.Remove(typeName) ? 1 : 0;
        this._typeNames.Add(new SQLiteDbTypeMapping(typeName, dataType, primary));
      }
      return num;
    }

    /// <summary>Clears the per-connection type callbacks.</summary>
    /// <returns>
    /// The total number of per-connection type callbacks cleared.
    /// </returns>
    public int ClearTypeCallbacks()
    {
      this.CheckDisposed();
      int num = -1;
      if (this._typeCallbacks != null)
      {
        num = this._typeCallbacks.Count;
        this._typeCallbacks.Clear();
      }
      return num;
    }

    /// <summary>
    /// Attempts to get the per-connection type callbacks for the specified
    /// database type name.
    /// </summary>
    /// <param name="typeName">The database type name.</param>
    /// <param name="callbacks">
    /// Upon success, this parameter will contain the object holding the
    /// callbacks for the database type name.  Upon failure, this parameter
    /// will be null.
    /// </param>
    /// <returns>Non-zero upon success; otherwise, zero.</returns>
    public bool TryGetTypeCallbacks(string typeName, out SQLiteTypeCallbacks callbacks)
    {
      this.CheckDisposed();
      if (typeName == null)
        throw new ArgumentNullException(nameof (typeName));
      if (this._typeCallbacks != null)
        return this._typeCallbacks.TryGetValue(typeName, out callbacks);
      callbacks = (SQLiteTypeCallbacks) null;
      return false;
    }

    /// <summary>
    /// Sets, resets, or clears the per-connection type callbacks for the
    /// specified database type name.
    /// </summary>
    /// <param name="typeName">The database type name.</param>
    /// <param name="callbacks">
    /// The object holding the callbacks for the database type name.  If
    /// this parameter is null, any callbacks for the database type name
    /// will be removed if they are present.
    /// </param>
    /// <returns>
    /// Non-zero if callbacks were set or removed; otherwise, zero.
    /// </returns>
    public bool SetTypeCallbacks(string typeName, SQLiteTypeCallbacks callbacks)
    {
      this.CheckDisposed();
      if (typeName == null)
        throw new ArgumentNullException(nameof (typeName));
      if (this._typeCallbacks == null)
        return false;
      if (callbacks == null)
        return this._typeCallbacks.Remove(typeName);
      callbacks.TypeName = typeName;
      this._typeCallbacks[typeName] = callbacks;
      return true;
    }

    /// <summary>
    /// Attempts to bind the specified <see cref="T:System.Data.SQLite.SQLiteFunction" /> object
    /// instance to this connection.
    /// </summary>
    /// <param name="functionAttribute">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" /> object instance containing
    /// the metadata for the function to be bound.
    /// </param>
    /// <param name="function">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunction" /> object instance that implements the
    /// function to be bound.
    /// </param>
    public void BindFunction(SQLiteFunctionAttribute functionAttribute, SQLiteFunction function)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for binding functions.");
      this._sql.BindFunction(functionAttribute, function, this._flags);
    }

    /// <summary>
    /// Attempts to bind the specified <see cref="T:System.Data.SQLite.SQLiteFunction" /> object
    /// instance to this connection.
    /// </summary>
    /// <param name="functionAttribute">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" /> object instance containing
    /// the metadata for the function to be bound.
    /// </param>
    /// <param name="callback1">
    /// A <see cref="T:System.Delegate" /> object instance that helps implement the
    /// function to be bound.  For scalar functions, this corresponds to the
    /// <see cref="T:System.Data.SQLite.SQLiteInvokeDelegate" /> type.  For aggregate functions,
    /// this corresponds to the <see cref="T:System.Data.SQLite.SQLiteStepDelegate" /> type.  For
    /// collation functions, this corresponds to the
    /// <see cref="T:System.Data.SQLite.SQLiteCompareDelegate" /> type.
    /// </param>
    /// <param name="callback2">
    /// A <see cref="T:System.Delegate" /> object instance that helps implement the
    /// function to be bound.  For aggregate functions, this corresponds to the
    /// <see cref="T:System.Data.SQLite.SQLiteFinalDelegate" /> type.  For other callback types, it
    /// is not used and must be null.
    /// </param>
    public void BindFunction(
      SQLiteFunctionAttribute functionAttribute,
      Delegate callback1,
      Delegate callback2)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for binding functions.");
      this._sql.BindFunction(functionAttribute, (SQLiteFunction) new SQLiteDelegateFunction(callback1, callback2), this._flags);
    }

    /// <summary>
    /// Attempts to unbind the specified <see cref="T:System.Data.SQLite.SQLiteFunction" /> object
    /// instance to this connection.
    /// </summary>
    /// <param name="functionAttribute">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" /> object instance containing
    /// the metadata for the function to be unbound.
    /// </param>
    /// <returns>Non-zero if the function was unbound.</returns>
    public bool UnbindFunction(SQLiteFunctionAttribute functionAttribute)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for unbinding functions.");
      return this._sql.UnbindFunction(functionAttribute, this._flags);
    }

    /// <summary>
    /// This method unbinds all registered (known) functions -OR- all previously
    /// bound user-defined functions from this connection.
    /// </summary>
    /// <param name="registered">
    /// Non-zero to unbind all registered (known) functions -OR- zero to unbind
    /// all functions currently bound to the connection.
    /// </param>
    /// <returns>
    /// Non-zero if all the specified user-defined functions were unbound.
    /// </returns>
    public bool UnbindAllFunctions(bool registered)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for unbinding functions.");
      return SQLiteFunction.UnbindAllFunctions(this._sql, this._flags, registered);
    }

    [Conditional("CHECK_STATE")]
    internal static void Check(SQLiteConnection connection)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      connection.CheckDisposed();
      if (connection._connectionState != ConnectionState.Open)
        throw new InvalidOperationException("The connection is not open.");
      if (!(connection._sql is SQLite3 sql1))
        throw new InvalidOperationException("The connection handle wrapper is null.");
      SQLiteConnectionHandle sql2 = sql1._sql;
      if (sql2 == null)
        throw new InvalidOperationException("The connection handle is null.");
      if (sql2.IsInvalid)
        throw new InvalidOperationException("The connection handle is invalid.");
      if (sql2.IsClosed)
        throw new InvalidOperationException("The connection handle is closed.");
    }

    /// <summary>
    /// Parses a connection string into component parts using the custom
    /// connection string parser.  An exception may be thrown if the syntax
    /// of the connection string is incorrect.
    /// </summary>
    /// <param name="connectionString">The connection string to parse.</param>
    /// <param name="parseViaFramework">
    /// Non-zero to parse the connection string using the algorithm provided
    /// by the framework itself.  This is not applicable when running on the
    /// .NET Compact Framework.
    /// </param>
    /// <param name="allowNameOnly">
    /// Non-zero if names are allowed without values.
    /// </param>
    /// <returns>
    /// The list of key/value pairs corresponding to the parameters specified
    /// within the connection string.
    /// </returns>
    internal static SortedList<string, string> ParseConnectionString(
      string connectionString,
      bool parseViaFramework,
      bool allowNameOnly)
    {
      return SQLiteConnection.ParseConnectionString((SQLiteConnection) null, connectionString, parseViaFramework, allowNameOnly);
    }

    /// <summary>
    /// Parses a connection string into component parts using the custom
    /// connection string parser.  An exception may be thrown if the syntax
    /// of the connection string is incorrect.
    /// </summary>
    /// <param name="connection">
    /// The connection that will be using the parsed connection string.
    /// </param>
    /// <param name="connectionString">The connection string to parse.</param>
    /// <param name="parseViaFramework">
    /// Non-zero to parse the connection string using the algorithm provided
    /// by the framework itself.  This is not applicable when running on the
    /// .NET Compact Framework.
    /// </param>
    /// <param name="allowNameOnly">
    /// Non-zero if names are allowed without values.
    /// </param>
    /// <returns>
    /// The list of key/value pairs corresponding to the parameters specified
    /// within the connection string.
    /// </returns>
    private static SortedList<string, string> ParseConnectionString(
      SQLiteConnection connection,
      string connectionString,
      bool parseViaFramework,
      bool allowNameOnly)
    {
      return !parseViaFramework ? SQLiteConnection.ParseConnectionString(connection, connectionString, allowNameOnly) : SQLiteConnection.ParseConnectionStringViaFramework(connection, connectionString, false);
    }

    /// <summary>
    /// Attempts to escape the specified connection string property name or
    /// value in a way that is compatible with the connection string parser.
    /// </summary>
    /// <param name="value">
    /// The connection string property name or value to escape.
    /// </param>
    /// <param name="allowEquals">
    /// Non-zero if the equals sign is permitted in the string.  If this is
    /// zero and the string contains an equals sign, an exception will be
    /// thrown.
    /// </param>
    /// <returns>
    /// The original string, with all special characters escaped.  If the
    /// original string contains equals signs, they will not be escaped.
    /// Instead, they will be preserved verbatim.
    /// </returns>
    private static string EscapeForConnectionString(string value, bool allowEquals)
    {
      if (string.IsNullOrEmpty(value) || value.IndexOfAny(SQLiteConvert.SpecialChars) == -1)
        return value;
      int length = value.Length;
      StringBuilder stringBuilder = new StringBuilder(length);
      for (int index = 0; index < length; ++index)
      {
        char ch = value[index];
        switch (ch)
        {
          case '"':
          case '\'':
          case ';':
          case '\\':
            stringBuilder.Append('\\');
            stringBuilder.Append(ch);
            break;
          case '=':
            if (!allowEquals)
              throw new ArgumentException("equals sign character is not allowed here");
            stringBuilder.Append(ch);
            break;
          default:
            stringBuilder.Append(ch);
            break;
        }
      }
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Builds a connection string from a list of key/value pairs.
    /// </summary>
    /// <param name="opts">
    /// The list of key/value pairs corresponding to the parameters to be
    /// specified within the connection string.
    /// </param>
    /// <returns>
    /// The connection string.  Depending on how the connection string was
    /// originally parsed, the returned connection string value may not be
    /// usable in a subsequent call to the <see cref="M:System.Data.SQLite.SQLiteConnection.Open" /> method.
    /// </returns>
    private static string BuildConnectionString(SortedList<string, string> opts)
    {
      if (opts == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> opt in opts)
        stringBuilder.AppendFormat("{0}{1}{2}{3}", (object) SQLiteConnection.EscapeForConnectionString(opt.Key, false), (object) '=', (object) SQLiteConnection.EscapeForConnectionString(opt.Value, true), (object) ';');
      return stringBuilder.ToString();
    }

    private void SetupSQLiteBase(SortedList<string, string> opts)
    {
      SQLiteDateFormats fmt = SQLiteConnection.TryParseEnum(typeof (SQLiteDateFormats), SQLiteConnection.FindKey(opts, "DateTimeFormat", SQLiteDateFormats.ISO8601.ToString()), true) is SQLiteDateFormats sqLiteDateFormats ? sqLiteDateFormats : SQLiteDateFormats.ISO8601;
      DateTimeKind kind = SQLiteConnection.TryParseEnum(typeof (DateTimeKind), SQLiteConnection.FindKey(opts, "DateTimeKind", DateTimeKind.Unspecified.ToString()), true) is DateTimeKind dateTimeKind ? dateTimeKind : DateTimeKind.Unspecified;
      string key = SQLiteConnection.FindKey(opts, "DateTimeFormatString", (string) null);
      if (SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(opts, "UseUTF16Encoding", false.ToString())))
        this._sql = (SQLiteBase) new SQLite3_UTF16(fmt, kind, key, IntPtr.Zero, (string) null, false);
      else
        this._sql = (SQLiteBase) new SQLite3(fmt, kind, key, IntPtr.Zero, (string) null, false);
    }

    /// <summary>Disposes and finalizes the connection, if applicable.</summary>
    public new void Dispose()
    {
      if (this._noDispose)
        return;
      base.Dispose();
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteConnection).Name);
    }

    /// <summary>
    /// Cleans up resources (native and managed) associated with the current instance.
    /// </summary>
    /// <param name="disposing">
    /// Zero when being disposed via garbage collection; otherwise, non-zero.
    /// </param>
    protected override void Dispose(bool disposing)
    {
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.TraceWarning) && this._noDispose)
        System.Diagnostics.Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "WARNING: Disposing of connection \"{0}\" with the no-dispose flag set.", (object) this._connectionString));
      this._disposing = true;
      try
      {
        if (this.disposed)
          return;
        this.Close();
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }

    /// <summary>
    /// Creates a clone of the connection.  All attached databases and user-defined functions are cloned.  If the existing connection is open, the cloned connection
    /// will also be opened.
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
      this.CheckDisposed();
      return (object) new SQLiteConnection(this);
    }

    /// <summary>
    /// Creates a database file.  This just creates a zero-byte file which SQLite
    /// will turn into a database when the file is opened properly.
    /// </summary>
    /// <param name="databaseFileName">The file to create</param>
    public static void CreateFile(string databaseFileName) => File.Create(databaseFileName).Close();

    /// <summary>
    /// Raises the state change event when the state of the connection changes
    /// </summary>
    /// <param name="newState">The new connection state.  If this is different
    /// from the previous state, the <see cref="E:System.Data.SQLite.SQLiteConnection.StateChange" /> event is
    /// raised.</param>
    /// <param name="eventArgs">The event data created for the raised event, if
    /// it was actually raised.</param>
    internal void OnStateChange(ConnectionState newState, ref StateChangeEventArgs eventArgs)
    {
      ConnectionState connectionState = this._connectionState;
      this._connectionState = newState;
      if (this.StateChange == null || newState == connectionState)
        return;
      StateChangeEventArgs e = new StateChangeEventArgs(connectionState, newState);
      this.StateChange((object) this, e);
      eventArgs = e;
    }

    /// <summary>
    /// Determines and returns the fallback default isolation level when one cannot be
    /// obtained from an existing connection instance.
    /// </summary>
    /// <returns>
    /// The fallback default isolation level for this connection instance -OR-
    /// <see cref="F:System.Data.IsolationLevel.Unspecified" /> if it cannot be determined.
    /// </returns>
    private static System.Data.IsolationLevel GetFallbackDefaultIsolationLevel() => System.Data.IsolationLevel.Serializable;

    /// <summary>
    /// Determines and returns the default isolation level for this connection instance.
    /// </summary>
    /// <returns>
    /// The default isolation level for this connection instance -OR-
    /// <see cref="F:System.Data.IsolationLevel.Unspecified" /> if it cannot be determined.
    /// </returns>
    internal System.Data.IsolationLevel GetDefaultIsolationLevel() => this._defaultIsolation;

    /// <summary>
    /// OBSOLETE.  Creates a new SQLiteTransaction if one isn't already active on the connection.
    /// </summary>
    /// <param name="isolationLevel">This parameter is ignored.</param>
    /// <param name="deferredLock">When TRUE, SQLite defers obtaining a write lock until a write operation is requested.
    /// When FALSE, a writelock is obtained immediately.  The default is TRUE, but in a multi-threaded multi-writer
    /// environment, one may instead choose to lock the database immediately to avoid any possible writer deadlock.</param>
    /// <returns>Returns a SQLiteTransaction object.</returns>
    [Obsolete("Use one of the standard BeginTransaction methods, this one will be removed soon")]
    public SQLiteTransaction BeginTransaction(
      System.Data.IsolationLevel isolationLevel,
      bool deferredLock)
    {
      this.CheckDisposed();
      return (SQLiteTransaction) this.BeginDbTransaction(!deferredLock ? System.Data.IsolationLevel.Serializable : System.Data.IsolationLevel.ReadCommitted);
    }

    /// <summary>
    /// OBSOLETE.  Creates a new SQLiteTransaction if one isn't already active on the connection.
    /// </summary>
    /// <param name="deferredLock">When TRUE, SQLite defers obtaining a write lock until a write operation is requested.
    /// When FALSE, a writelock is obtained immediately.  The default is false, but in a multi-threaded multi-writer
    /// environment, one may instead choose to lock the database immediately to avoid any possible writer deadlock.</param>
    /// <returns>Returns a SQLiteTransaction object.</returns>
    [Obsolete("Use one of the standard BeginTransaction methods, this one will be removed soon")]
    public SQLiteTransaction BeginTransaction(bool deferredLock)
    {
      this.CheckDisposed();
      return (SQLiteTransaction) this.BeginDbTransaction(!deferredLock ? System.Data.IsolationLevel.Serializable : System.Data.IsolationLevel.ReadCommitted);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.SQLite.SQLiteTransaction" /> if one isn't already active on the connection.
    /// </summary>
    /// <param name="isolationLevel">Supported isolation levels are Serializable, ReadCommitted and Unspecified.</param>
    /// <remarks>
    /// Unspecified will use the default isolation level specified in the connection string.  If no isolation level is specified in the
    /// connection string, Serializable is used.
    /// Serializable transactions are the default.  In this mode, the engine gets an immediate lock on the database, and no other threads
    /// may begin a transaction.  Other threads may read from the database, but not write.
    /// With a ReadCommitted isolation level, locks are deferred and elevated as needed.  It is possible for multiple threads to start
    /// a transaction in ReadCommitted mode, but if a thread attempts to commit a transaction while another thread
    /// has a ReadCommitted lock, it may timeout or cause a deadlock on both threads until both threads' CommandTimeout's are reached.
    /// </remarks>
    /// <returns>Returns a SQLiteTransaction object.</returns>
    public SQLiteTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
    {
      this.CheckDisposed();
      return (SQLiteTransaction) this.BeginDbTransaction(isolationLevel);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.SQLite.SQLiteTransaction" /> if one isn't already
    /// active on the connection.
    /// </summary>
    /// <returns>Returns the new transaction object.</returns>
    public SQLiteTransaction BeginTransaction()
    {
      this.CheckDisposed();
      return (SQLiteTransaction) this.BeginDbTransaction(this._defaultIsolation);
    }

    /// <summary>
    /// Forwards to the local <see cref="M:System.Data.SQLite.SQLiteConnection.BeginTransaction(System.Data.IsolationLevel)" /> function
    /// </summary>
    /// <param name="isolationLevel">Supported isolation levels are Unspecified, Serializable, and ReadCommitted</param>
    /// <returns></returns>
    protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
    {
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException();
      if (isolationLevel == System.Data.IsolationLevel.Unspecified)
        isolationLevel = this._defaultIsolation;
      isolationLevel = this.GetEffectiveIsolationLevel(isolationLevel);
      if (isolationLevel != System.Data.IsolationLevel.Serializable && isolationLevel != System.Data.IsolationLevel.ReadCommitted)
        throw new ArgumentException(nameof (isolationLevel));
      SQLiteTransaction sqLiteTransaction = !HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.AllowNestedTransactions) ? new SQLiteTransaction(this, isolationLevel != System.Data.IsolationLevel.Serializable) : (SQLiteTransaction) new SQLiteTransaction2(this, isolationLevel != System.Data.IsolationLevel.Serializable);
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.NewTransaction, (StateChangeEventArgs) null, (IDbTransaction) sqLiteTransaction, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
      return (DbTransaction) sqLiteTransaction;
    }

    /// <summary>
    /// This method is not implemented; however, the <see cref="E:System.Data.SQLite.SQLiteConnection.Changed" />
    /// event will still be raised.
    /// </summary>
    /// <param name="databaseName"></param>
    public override void ChangeDatabase(string databaseName)
    {
      this.CheckDisposed();
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.ChangeDatabase, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, databaseName, (object) null));
      throw new NotImplementedException();
    }

    /// <summary>
    /// When the database connection is closed, all commands linked to this connection are automatically reset.
    /// </summary>
    public override void Close()
    {
      this.CheckDisposed();
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Closing, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
      if (this._sql != null)
      {
        lock (this._enlistmentSyncRoot)
        {
          SQLiteEnlistment enlistment = this._enlistment;
          this._enlistment = (SQLiteEnlistment) null;
          if (enlistment != null)
          {
            SQLiteConnection sqLiteConnection = new SQLiteConnection();
            sqLiteConnection._sql = this._sql;
            sqLiteConnection._transactionLevel = this._transactionLevel;
            sqLiteConnection._transactionSequence = this._transactionSequence;
            sqLiteConnection._enlistment = enlistment;
            sqLiteConnection._connectionState = this._connectionState;
            sqLiteConnection._version = this._version;
            SQLiteTransaction transaction = enlistment._transaction;
            if (transaction != null)
              transaction._cnn = sqLiteConnection;
            enlistment._disposeConnection = true;
            this._sql = (SQLiteBase) null;
          }
        }
        if (this._sql != null)
        {
          this._sql.Close(this._disposing);
          this._sql = (SQLiteBase) null;
        }
        this._transactionLevel = 0;
        this._transactionSequence = 0;
      }
      StateChangeEventArgs eventArgs = (StateChangeEventArgs) null;
      this.OnStateChange(ConnectionState.Closed, ref eventArgs);
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Closed, eventArgs, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
    }

    /// <summary>
    /// Returns the number of pool entries for the file name associated with this connection.
    /// </summary>
    public int PoolCount => this._sql == null ? 0 : this._sql.CountPool();

    /// <summary>
    /// Clears the connection pool associated with the connection.  Any other active connections using the same database file
    /// will be discarded instead of returned to the pool when they are closed.
    /// </summary>
    /// <param name="connection"></param>
    public static void ClearPool(SQLiteConnection connection)
    {
      if (connection._sql == null)
        return;
      connection._sql.ClearPool();
    }

    /// <summary>
    /// Clears all connection pools.  Any active connections will be discarded instead of sent to the pool when they are closed.
    /// </summary>
    public static void ClearAllPools() => SQLiteConnectionPool.ClearAllPools();

    /// <summary>
    /// The connection string containing the parameters for the connection
    /// </summary>
    /// <remarks>
    /// For the complete list of supported connection string properties,
    /// please see <see cref="T:System.Data.SQLite.SQLiteConnection" />.
    /// </remarks>
    [DefaultValue("")]
    [Editor("SQLite.Designer.SQLiteConnectionStringEditor, SQLite.Designer, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [RefreshProperties(RefreshProperties.All)]
    public override string ConnectionString
    {
      get
      {
        this.CheckDisposed();
        return this._connectionString;
      }
      set
      {
        this.CheckDisposed();
        if (value == null)
          throw new ArgumentNullException();
        if (this._connectionState != ConnectionState.Closed)
          throw new InvalidOperationException();
        this._connectionString = value;
      }
    }

    /// <summary>
    /// Create a new <see cref="T:System.Data.SQLite.SQLiteCommand" /> and associate it with this connection.
    /// </summary>
    /// <returns>Returns a new command object already assigned to this connection.</returns>
    public SQLiteCommand CreateCommand()
    {
      this.CheckDisposed();
      return new SQLiteCommand(this);
    }

    /// <summary>
    /// Forwards to the local <see cref="M:System.Data.SQLite.SQLiteConnection.CreateCommand" /> function.
    /// </summary>
    /// <returns></returns>
    protected override DbCommand CreateDbCommand() => (DbCommand) this.CreateCommand();

    /// <summary>
    /// Attempts to create a new <see cref="T:System.Data.SQLite.ISQLiteSession" /> object instance
    /// using this connection and the specified database name.
    /// </summary>
    /// <param name="databaseName">
    /// The name of the database for the newly created session.
    /// </param>
    /// <returns>
    /// The newly created session -OR- null if it cannot be created.
    /// </returns>
    public ISQLiteSession CreateSession(string databaseName)
    {
      this.CheckDisposed();
      return (ISQLiteSession) new SQLiteSession(SQLiteConnection.GetNativeHandle(this), this._flags, databaseName);
    }

    /// <summary>
    /// Attempts to create a new <see cref="T:System.Data.SQLite.ISQLiteChangeSet" /> object instance
    /// using this connection and the specified raw data.
    /// </summary>
    /// <param name="rawData">
    /// The raw data that contains a change set (or patch set).
    /// </param>
    /// <returns>
    /// The newly created change set -OR- null if it cannot be created.
    /// </returns>
    public ISQLiteChangeSet CreateChangeSet(byte[] rawData)
    {
      this.CheckDisposed();
      return (ISQLiteChangeSet) new SQLiteMemoryChangeSet(rawData, SQLiteConnection.GetNativeHandle(this), this._flags);
    }

    /// <summary>
    /// Attempts to create a new <see cref="T:System.Data.SQLite.ISQLiteChangeSet" /> object instance
    /// using this connection and the specified raw data.
    /// </summary>
    /// <param name="rawData">
    /// The raw data that contains a change set (or patch set).
    /// </param>
    /// <param name="flags">
    /// The flags used to create the change set iterator.
    /// </param>
    /// <returns>
    /// The newly created change set -OR- null if it cannot be created.
    /// </returns>
    public ISQLiteChangeSet CreateChangeSet(
      byte[] rawData,
      SQLiteChangeSetStartFlags flags)
    {
      this.CheckDisposed();
      return (ISQLiteChangeSet) new SQLiteMemoryChangeSet(rawData, SQLiteConnection.GetNativeHandle(this), this._flags, flags);
    }

    /// <summary>
    /// Attempts to create a new <see cref="T:System.Data.SQLite.ISQLiteChangeSet" /> object instance
    /// using this connection and the specified stream.
    /// </summary>
    /// <param name="inputStream">
    /// The stream where the raw data that contains a change set (or patch set)
    /// may be read.
    /// </param>
    /// <param name="outputStream">
    /// The stream where the raw data that contains a change set (or patch set)
    /// may be written.
    /// </param>
    /// <returns>
    /// The newly created change set -OR- null if it cannot be created.
    /// </returns>
    public ISQLiteChangeSet CreateChangeSet(Stream inputStream, Stream outputStream)
    {
      this.CheckDisposed();
      return (ISQLiteChangeSet) new SQLiteStreamChangeSet(inputStream, outputStream, SQLiteConnection.GetNativeHandle(this), this._flags);
    }

    /// <summary>
    /// Attempts to create a new <see cref="T:System.Data.SQLite.ISQLiteChangeSet" /> object instance
    /// using this connection and the specified stream.
    /// </summary>
    /// <param name="inputStream">
    /// The stream where the raw data that contains a change set (or patch set)
    /// may be read.
    /// </param>
    /// <param name="outputStream">
    /// The stream where the raw data that contains a change set (or patch set)
    /// may be written.
    /// </param>
    /// <param name="flags">
    /// The flags used to create the change set iterator.
    /// </param>
    /// <returns>
    /// The newly created change set -OR- null if it cannot be created.
    /// </returns>
    public ISQLiteChangeSet CreateChangeSet(
      Stream inputStream,
      Stream outputStream,
      SQLiteChangeSetStartFlags flags)
    {
      this.CheckDisposed();
      return (ISQLiteChangeSet) new SQLiteStreamChangeSet(inputStream, outputStream, SQLiteConnection.GetNativeHandle(this), this._flags, flags);
    }

    /// <summary>
    /// Attempts to create a new <see cref="T:System.Data.SQLite.ISQLiteChangeGroup" /> object
    /// instance using this connection.
    /// </summary>
    /// <returns>
    /// The newly created change group -OR- null if it cannot be created.
    /// </returns>
    public ISQLiteChangeGroup CreateChangeGroup()
    {
      this.CheckDisposed();
      return (ISQLiteChangeGroup) new SQLiteChangeGroup(this._flags);
    }

    /// <summary>
    /// Returns the data source file name without extension or path.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string DataSource
    {
      get
      {
        this.CheckDisposed();
        return this._dataSource;
      }
    }

    /// <summary>
    /// Returns the fully qualified path and file name for the currently open
    /// database, if any.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string FileName
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.GetFileName(SQLiteConnection.GetDefaultCatalogName()) : throw new InvalidOperationException("Database connection not valid for getting file name.");
      }
    }

    /// <summary>Returns the string "main".</summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override string Database
    {
      get
      {
        this.CheckDisposed();
        return SQLiteConnection.GetDefaultCatalogName();
      }
    }

    internal static string MapUriPath(string path)
    {
      if (path.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
        return path.Substring(7);
      if (path.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
        return path.Substring(5);
      return path.StartsWith("/", StringComparison.OrdinalIgnoreCase) ? path : throw new InvalidOperationException("Invalid connection string: invalid URI");
    }

    /// <summary>
    /// Determines if the legacy connection string parser should be used.
    /// </summary>
    /// <param name="connection">
    /// The connection that will be using the parsed connection string.
    /// </param>
    /// <returns>
    /// Non-zero if the legacy connection string parser should be used.
    /// </returns>
    private static bool ShouldUseLegacyConnectionStringParser(SQLiteConnection connection)
    {
      string name = "No_SQLiteConnectionNewParser";
      object settingValue;
      if (connection != null && connection.TryGetCachedSetting(name, (object) null, out settingValue) || connection == null && SQLiteConnection.TryGetLastCachedSetting(name, (object) null, out settingValue))
        return settingValue != null;
      settingValue = (object) UnsafeNativeMethods.GetSettingValue(name, (string) null);
      if (connection != null)
        connection.SetCachedSetting(name, settingValue);
      else
        SQLiteConnection.SetLastCachedSetting(name, settingValue);
      return settingValue != null;
    }

    /// <summary>
    /// Parses a connection string into component parts using the custom
    /// connection string parser.  An exception may be thrown if the syntax
    /// of the connection string is incorrect.
    /// </summary>
    /// <param name="connectionString">The connection string to parse.</param>
    /// <param name="allowNameOnly">
    /// Non-zero if names are allowed without values.
    /// </param>
    /// <returns>
    /// The list of key/value pairs corresponding to the parameters specified
    /// within the connection string.
    /// </returns>
    private static SortedList<string, string> ParseConnectionString(
      string connectionString,
      bool allowNameOnly)
    {
      return SQLiteConnection.ParseConnectionString((SQLiteConnection) null, connectionString, allowNameOnly);
    }

    /// <summary>
    /// Parses a connection string into component parts using the custom
    /// connection string parser.  An exception may be thrown if the syntax
    /// of the connection string is incorrect.
    /// </summary>
    /// <param name="connection">
    /// The connection that will be using the parsed connection string.
    /// </param>
    /// <param name="connectionString">The connection string to parse.</param>
    /// <param name="allowNameOnly">
    /// Non-zero if names are allowed without values.
    /// </param>
    /// <returns>
    /// The list of key/value pairs corresponding to the parameters specified
    /// within the connection string.
    /// </returns>
    private static SortedList<string, string> ParseConnectionString(
      SQLiteConnection connection,
      string connectionString,
      bool allowNameOnly)
    {
      string source = connectionString;
      SortedList<string, string> sortedList = new SortedList<string, string>((IComparer<string>) StringComparer.OrdinalIgnoreCase);
      string error = (string) null;
      string[] strArray = !SQLiteConnection.ShouldUseLegacyConnectionStringParser(connection) ? SQLiteConvert.NewSplit(source, ';', true, ref error) : SQLiteConvert.Split(source, ';');
      if (strArray == null)
        throw new ArgumentException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Invalid ConnectionString format, cannot parse: {0}", (object) (error ?? "could not split connection string into properties")));
      int num = strArray != null ? strArray.Length : 0;
      for (int index = 0; index < num; ++index)
      {
        if (strArray[index] != null)
        {
          strArray[index] = strArray[index].Trim();
          if (strArray[index].Length != 0)
          {
            int length = strArray[index].IndexOf('=');
            if (length != -1)
              sortedList.Add(SQLiteConnection.UnwrapString(strArray[index].Substring(0, length).Trim()), SQLiteConnection.UnwrapString(strArray[index].Substring(length + 1).Trim()));
            else if (allowNameOnly)
              sortedList.Add(SQLiteConnection.UnwrapString(strArray[index].Trim()), string.Empty);
            else
              throw new ArgumentException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Invalid ConnectionString format for part \"{0}\", no equal sign found", (object) strArray[index]));
          }
        }
      }
      return sortedList;
    }

    /// <summary>
    /// Parses a connection string using the built-in (i.e. framework provided)
    /// connection string parser class and returns the key/value pairs.  An
    /// exception may be thrown if the connection string is invalid or cannot be
    /// parsed.  When compiled for the .NET Compact Framework, the custom
    /// connection string parser is always used instead because the framework
    /// provided one is unavailable there.
    /// </summary>
    /// <param name="connection">
    /// The connection that will be using the parsed connection string.
    /// </param>
    /// <param name="connectionString">The connection string to parse.</param>
    /// <param name="strict">
    /// Non-zero to throw an exception if any connection string values are not of
    /// the <see cref="T:System.String" /> type.  This is not applicable when running on
    /// the .NET Compact Framework.
    /// </param>
    /// <returns>The list of key/value pairs.</returns>
    private static SortedList<string, string> ParseConnectionStringViaFramework(
      SQLiteConnection connection,
      string connectionString,
      bool strict)
    {
      DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
      connectionStringBuilder.ConnectionString = connectionString;
      SortedList<string, string> sortedList = new SortedList<string, string>((IComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (string key in (IEnumerable) connectionStringBuilder.Keys)
      {
        object obj = connectionStringBuilder[key];
        string str = (string) null;
        if (obj is string)
        {
          str = (string) obj;
        }
        else
        {
          if (strict)
            throw new ArgumentException("connection property value is not a string", key);
          if (obj != null)
            str = obj.ToString();
        }
        sortedList.Add(key, str);
      }
      return sortedList;
    }

    /// <summary>Manual distributed transaction enlistment support</summary>
    /// <param name="transaction">The distributed transaction to enlist in</param>
    public override void EnlistTransaction(Transaction transaction)
    {
      this.CheckDisposed();
      bool flag1;
      int waitTimeout;
      lock (this._enlistmentSyncRoot)
      {
        flag1 = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.WaitForEnlistmentReset);
        waitTimeout = this._waitTimeout;
      }
      if (flag1)
        this.WaitForEnlistmentReset(waitTimeout, new bool?());
      lock (this._enlistmentSyncRoot)
      {
        if (this._enlistment != null && transaction == this._enlistment._scope)
          return;
        if (this._enlistment != null)
          throw new ArgumentException("Already enlisted in a transaction");
        if (this._transactionLevel > 0 && transaction != (Transaction) null)
          throw new ArgumentException("Unable to enlist in transaction, a local transaction already exists");
        if (transaction == (Transaction) null)
          throw new ArgumentNullException("Unable to enlist in transaction, it is null");
        bool flag2 = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.StrictEnlistment);
        this._enlistment = new SQLiteEnlistment(this, transaction, SQLiteConnection.GetFallbackDefaultIsolationLevel(), flag2, flag2);
        SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.EnlistTransaction, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) new object[1]
        {
          (object) this._enlistment
        }));
      }
    }

    /// <summary>
    /// <b>EXPERIMENTAL</b> --
    /// Waits for the enlistment associated with this connection to be reset.
    /// This method always throws <see cref="T:System.NotImplementedException" /> when
    /// running on the .NET Compact Framework.
    /// </summary>
    /// <param name="timeoutMilliseconds">
    /// The approximate maximum number of milliseconds to wait before timing
    /// out the wait operation.
    /// </param>
    /// <param name="returnOnDisposed">
    /// The return value to use if the connection has been disposed; if this
    /// value is null, <see cref="T:System.ObjectDisposedException" /> will be raised
    /// if the connection has been disposed.
    /// </param>
    /// <returns>
    /// Non-zero if the enlistment assciated with this connection was reset;
    /// otherwise, zero.  It should be noted that this method returning a
    /// non-zero value does not necessarily guarantee that the connection
    /// can enlist in a new transaction (i.e. due to potentical race with
    /// other threads); therefore, callers should generally use try/catch
    /// when calling the <see cref="M:System.Data.SQLite.SQLiteConnection.EnlistTransaction(System.Transactions.Transaction)" /> method.
    /// </returns>
    public bool WaitForEnlistmentReset(int timeoutMilliseconds, bool? returnOnDisposed)
    {
      if (!returnOnDisposed.HasValue)
        this.CheckDisposed();
      else if (this.disposed)
        return returnOnDisposed.Value;
      if (timeoutMilliseconds < 0)
        throw new ArgumentException("timeout cannot be negative");
      int millisecondsTimeout;
      if (timeoutMilliseconds == 0)
      {
        millisecondsTimeout = 0;
      }
      else
      {
        millisecondsTimeout = Math.Min(timeoutMilliseconds / 10, 100);
        if (millisecondsTimeout == 0)
          millisecondsTimeout = 100;
      }
      DateTime utcNow = DateTime.UtcNow;
      while (true)
      {
        bool flag = Monitor.TryEnter(this._enlistmentSyncRoot);
        try
        {
          if (flag)
          {
            if (this._enlistment == null)
              return true;
          }
        }
        finally
        {
          if (flag)
            Monitor.Exit(this._enlistmentSyncRoot);
        }
        if (millisecondsTimeout != 0)
        {
          double totalMilliseconds = DateTime.UtcNow.Subtract(utcNow).TotalMilliseconds;
          if (totalMilliseconds >= 0.0 && totalMilliseconds < (double) timeoutMilliseconds)
            Thread.Sleep(millisecondsTimeout);
          else
            goto label_21;
        }
        else
          break;
      }
      return false;
label_21:
      return false;
    }

    /// <summary>
    /// Looks for a key in the array of key/values of the parameter string.  If not found, return the specified default value
    /// </summary>
    /// <param name="items">The list to look in</param>
    /// <param name="key">The key to find</param>
    /// <param name="defValue">The default value to return if the key is not found</param>
    /// <returns>The value corresponding to the specified key, or the default value if not found.</returns>
    internal static string FindKey(SortedList<string, string> items, string key, string defValue)
    {
      string str;
      return string.IsNullOrEmpty(key) || !items.TryGetValue(key, out str) && !items.TryGetValue(key.Replace(" ", string.Empty), out str) && !items.TryGetValue(key.Replace(" ", "_"), out str) ? defValue : str;
    }

    /// <summary>
    /// Attempts to convert the string value to an enumerated value of the specified type.
    /// </summary>
    /// <param name="type">The enumerated type to convert the string value to.</param>
    /// <param name="value">The string value to be converted.</param>
    /// <param name="ignoreCase">Non-zero to make the conversion case-insensitive.</param>
    /// <returns>The enumerated value upon success or null upon error.</returns>
    internal static object TryParseEnum(Type type, string value, bool ignoreCase)
    {
      if (!string.IsNullOrEmpty(value))
      {
        try
        {
          return Enum.Parse(type, value, ignoreCase);
        }
        catch
        {
        }
      }
      return (object) null;
    }

    /// <summary>
    /// Attempts to convert an input string into a byte value.
    /// </summary>
    /// <param name="value">The string value to be converted.</param>
    /// <param name="style">The number styles to use for the conversion.</param>
    /// <param name="result">
    /// Upon sucess, this will contain the parsed byte value.
    /// Upon failure, the value of this parameter is undefined.
    /// </param>
    /// <returns>Non-zero upon success; zero on failure.</returns>
    private static bool TryParseByte(string value, NumberStyles style, out byte result) => byte.TryParse(value, style, (IFormatProvider) null, out result);

    /// <summary>Change a limit value for the database.</summary>
    /// <param name="option">The database limit to change.</param>
    /// <param name="value">The new value for the specified limit.</param>
    /// <returns>
    /// The old value for the specified limit -OR- negative one if an error
    /// occurs.
    /// </returns>
    public int SetLimitOption(SQLiteLimitOpsEnum option, int value)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for changing a limit option.");
      return this._sql.SetLimitOption(option, value);
    }

    /// <summary>Change a configuration option value for the database.</summary>
    /// <param name="option">
    /// The database configuration option to change.
    /// </param>
    /// <param name="value">
    /// The new value for the specified configuration option.
    /// </param>
    public void SetConfigurationOption(SQLiteConfigDbOpsEnum option, object value)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for changing a configuration option.");
      if (option == SQLiteConfigDbOpsEnum.SQLITE_DBCONFIG_ENABLE_LOAD_EXTENSION && HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoLoadExtension))
        throw new SQLiteException("Loading extensions is disabled for this database connection.");
      SQLiteErrorCode errorCode = this._sql.SetConfigurationOption(option, value);
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, (string) null);
    }

    /// <summary>Enables or disables extension loading.</summary>
    /// <param name="enable">
    /// True to enable loading of extensions, false to disable.
    /// </param>
    public void EnableExtensions(bool enable)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Database connection not valid for {0} extensions.", enable ? (object) "enabling" : (object) "disabling"));
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoLoadExtension))
        throw new SQLiteException("Loading extensions is disabled for this database connection.");
      this._sql.SetLoadExtension(enable);
    }

    /// <summary>
    /// Loads a SQLite extension library from the named dynamic link library file.
    /// </summary>
    /// <param name="fileName">
    /// The name of the dynamic link library file containing the extension.
    /// </param>
    public void LoadExtension(string fileName)
    {
      this.CheckDisposed();
      this.LoadExtension(fileName, (string) null);
    }

    /// <summary>
    /// Loads a SQLite extension library from the named dynamic link library file.
    /// </summary>
    /// <param name="fileName">
    /// The name of the dynamic link library file containing the extension.
    /// </param>
    /// <param name="procName">
    /// The name of the exported function used to initialize the extension.
    /// If null, the default "sqlite3_extension_init" will be used.
    /// </param>
    public void LoadExtension(string fileName, string procName)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for loading extensions.");
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoLoadExtension))
        throw new SQLiteException("Loading extensions is disabled for this database connection.");
      this._sql.LoadExtension(fileName, procName);
    }

    /// <summary>
    /// Creates a disposable module containing the implementation of a virtual
    /// table.
    /// </summary>
    /// <param name="module">
    /// The module object to be used when creating the disposable module.
    /// </param>
    public void CreateModule(SQLiteModule module)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for creating modules.");
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoCreateModule))
        throw new SQLiteException("Creating modules is disabled for this database connection.");
      this._sql.CreateModule(module, this._flags);
    }

    /// <summary>
    /// Parses a string containing a sequence of zero or more hexadecimal
    /// encoded byte values and returns the resulting byte array.  The
    /// "0x" prefix is not allowed on the input string.
    /// </summary>
    /// <param name="text">
    /// The input string containing zero or more hexadecimal encoded byte
    /// values.
    /// </param>
    /// <returns>
    /// A byte array containing the parsed byte values or null if an error
    /// was encountered.
    /// </returns>
    internal static byte[] FromHexString(string text)
    {
      string error = (string) null;
      return SQLiteConnection.FromHexString(text, ref error);
    }

    /// <summary>
    /// Creates and returns a string containing the hexadecimal encoded byte
    /// values from the input array.
    /// </summary>
    /// <param name="array">The input array of bytes.</param>
    /// <returns>The resulting string or null upon failure.</returns>
    internal static string ToHexString(byte[] array)
    {
      if (array == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      int length = array.Length;
      for (int index = 0; index < length; ++index)
        stringBuilder.AppendFormat("{0:x2}", (object) array[index]);
      return stringBuilder.ToString();
    }

    /// <summary>
    /// Parses a string containing a sequence of zero or more hexadecimal
    /// encoded byte values and returns the resulting byte array.  The
    /// "0x" prefix is not allowed on the input string.
    /// </summary>
    /// <param name="text">
    /// The input string containing zero or more hexadecimal encoded byte
    /// values.
    /// </param>
    /// <param name="error">
    /// Upon failure, this will contain an appropriate error message.
    /// </param>
    /// <returns>
    /// A byte array containing the parsed byte values or null if an error
    /// was encountered.
    /// </returns>
    private static byte[] FromHexString(string text, ref string error)
    {
      if (text == null)
      {
        error = "string is null";
        return (byte[]) null;
      }
      if (text.Length % 2 != 0)
      {
        error = "string contains an odd number of characters";
        return (byte[]) null;
      }
      byte[] numArray = new byte[text.Length / 2];
      for (int startIndex = 0; startIndex < text.Length; startIndex += 2)
      {
        string str = text.Substring(startIndex, 2);
        if (!SQLiteConnection.TryParseByte(str, NumberStyles.HexNumber, out numArray[startIndex / 2]))
        {
          error = HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "string contains \"{0}\", which cannot be converted to a byte value", (object) str);
          return (byte[]) null;
        }
      }
      return numArray;
    }

    /// <summary>
    /// This method figures out what the default connection pool setting should
    /// be based on the connection flags.  When present, the "Pooling" connection
    /// string property value always overrides the value returned by this method.
    /// </summary>
    /// <returns>
    /// Non-zero if the connection pool should be enabled by default; otherwise,
    /// zero.
    /// </returns>
    private bool GetDefaultPooling()
    {
      bool flag = false;
      if (flag)
      {
        if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoConnectionPool))
          flag = false;
        if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionPool))
          flag = true;
      }
      else
      {
        if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.UseConnectionPool))
          flag = true;
        if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.NoConnectionPool))
          flag = false;
      }
      return flag;
    }

    /// <summary>
    /// Determines the transaction isolation level that should be used by
    /// the caller, primarily based upon the one specified by the caller.
    /// If mapping of transaction isolation levels is enabled, the returned
    /// transaction isolation level may be significantly different than the
    /// originally specified one.
    /// </summary>
    /// <param name="isolationLevel">
    /// The originally specified transaction isolation level.
    /// </param>
    /// <returns>The transaction isolation level that should be used.</returns>
    private System.Data.IsolationLevel GetEffectiveIsolationLevel(
      System.Data.IsolationLevel isolationLevel)
    {
      if (!HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.MapIsolationLevels))
        return isolationLevel;
      switch (isolationLevel)
      {
        case System.Data.IsolationLevel.Unspecified:
        case System.Data.IsolationLevel.Chaos:
        case System.Data.IsolationLevel.ReadUncommitted:
        case System.Data.IsolationLevel.ReadCommitted:
          return System.Data.IsolationLevel.ReadCommitted;
        case System.Data.IsolationLevel.RepeatableRead:
        case System.Data.IsolationLevel.Serializable:
        case System.Data.IsolationLevel.Snapshot:
          return System.Data.IsolationLevel.Serializable;
        default:
          return SQLiteConnection.GetFallbackDefaultIsolationLevel();
      }
    }

    /// <summary>
    /// Opens the connection using the parameters found in the <see cref="P:System.Data.SQLite.SQLiteConnection.ConnectionString" />.
    /// </summary>
    public override void Open()
    {
      this.CheckDisposed();
      SQLiteConnection._lastConnectionInOpen = this;
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Opening, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, (string) null, (object) null));
      if (this._connectionState != ConnectionState.Closed)
        throw new InvalidOperationException();
      this.Close();
      SortedList<string, string> connectionString = SQLiteConnection.ParseConnectionString(this, this._connectionString, this._parseViaFramework, false);
      object obj = SQLiteConnection.TryParseEnum(typeof (SQLiteConnectionFlags), SQLiteConnection.FindKey(connectionString, "Flags", (string) null), true);
      bool boolean1 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "NoDefaultFlags", false.ToString()));
      if (obj is SQLiteConnectionFlags liteConnectionFlags)
        this._flags |= liteConnectionFlags;
      else if (!boolean1)
        this._flags |= SQLiteConnection.DefaultFlags;
      if (!SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "NoSharedFlags", false.ToString())))
      {
        lock (SQLiteConnection._syncRoot)
          this._flags |= SQLiteConnection._sharedFlags;
      }
      bool flag1 = HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.HidePassword);
      SortedList<string, string> opts = connectionString;
      string text = this._connectionString;
      if (flag1)
      {
        opts = new SortedList<string, string>((IComparer<string>) StringComparer.OrdinalIgnoreCase);
        foreach (KeyValuePair<string, string> keyValuePair in connectionString)
        {
          if (!string.Equals(keyValuePair.Key, "Password", StringComparison.OrdinalIgnoreCase) && !string.Equals(keyValuePair.Key, "HexPassword", StringComparison.OrdinalIgnoreCase))
            opts.Add(keyValuePair.Key, keyValuePair.Value);
        }
        text = SQLiteConnection.BuildConnectionString(opts);
      }
      SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.ConnectionString, (StateChangeEventArgs) null, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, text, (object) new object[1]
      {
        (object) opts
      }));
      this._defaultDbType = SQLiteConnection.TryParseEnum(typeof (DbType), SQLiteConnection.FindKey(connectionString, "DefaultDbType", (string) null), true) is DbType dbType ? new DbType?(dbType) : new DbType?();
      if (this._defaultDbType.HasValue && this._defaultDbType.Value == ~DbType.AnsiString)
        this._defaultDbType = new DbType?();
      this._defaultTypeName = SQLiteConnection.FindKey(connectionString, "DefaultTypeName", (string) null);
      this._vfsName = SQLiteConnection.FindKey(connectionString, "VfsName", (string) null);
      bool flag2 = false;
      bool flag3 = false;
      string str = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Version", SQLiteConvert.ToString(3)), (IFormatProvider) CultureInfo.InvariantCulture) == 3 ? SQLiteConnection.FindKey(connectionString, "Data Source", (string) null) : throw new NotSupportedException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Only SQLite Version {0} is supported at this time", (object) 3));
      if (string.IsNullOrEmpty(str))
      {
        string key = SQLiteConnection.FindKey(connectionString, "Uri", (string) null);
        if (string.IsNullOrEmpty(key))
        {
          str = SQLiteConnection.FindKey(connectionString, "FullUri", (string) null);
          if (string.IsNullOrEmpty(str))
            throw new ArgumentException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Data Source cannot be empty.  Use {0} to open an in-memory database", (object) ":memory:"));
          flag3 = true;
        }
        else
        {
          str = SQLiteConnection.MapUriPath(key);
          flag2 = true;
        }
      }
      bool flag4 = string.Compare(str, ":memory:", StringComparison.OrdinalIgnoreCase) == 0;
      if (HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.TraceWarning) && !flag2 && (!flag3 && !flag4) && (!string.IsNullOrEmpty(str) && str.StartsWith("\\", StringComparison.OrdinalIgnoreCase) && !str.StartsWith("\\\\", StringComparison.OrdinalIgnoreCase)))
        System.Diagnostics.Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "WARNING: Detected a possibly malformed UNC database file name \"{0}\" that may have originally started with two backslashes; however, four leading backslashes may be required, e.g.: \"Data Source=\\\\\\{0};\"", (object) str));
      if (!flag3)
      {
        if (flag4)
        {
          str = ":memory:";
        }
        else
        {
          bool boolean2 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "ToFullPath", true.ToString()));
          str = SQLiteConnection.ExpandFileName(str, boolean2);
        }
      }
      try
      {
        bool boolean2 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Pooling", this.GetDefaultPooling().ToString()));
        int int32_1 = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Max Pool Size", SQLiteConvert.ToString(100)), (IFormatProvider) CultureInfo.InvariantCulture);
        this._defaultTimeout = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Default Timeout", SQLiteConvert.ToString(30)), (IFormatProvider) CultureInfo.InvariantCulture);
        this._busyTimeout = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "BusyTimeout", SQLiteConvert.ToString(0)), (IFormatProvider) CultureInfo.InvariantCulture);
        this._waitTimeout = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "WaitTimeout", SQLiteConvert.ToString(30000)), (IFormatProvider) CultureInfo.InvariantCulture);
        this._prepareRetries = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "PrepareRetries", SQLiteConvert.ToString(3)), (IFormatProvider) CultureInfo.InvariantCulture);
        this._progressOps = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "ProgressOps", SQLiteConvert.ToString(0)), (IFormatProvider) CultureInfo.InvariantCulture);
        this._defaultIsolation = SQLiteConnection.TryParseEnum(typeof (System.Data.IsolationLevel), SQLiteConnection.FindKey(connectionString, "Default IsolationLevel", System.Data.IsolationLevel.Serializable.ToString()), true) is System.Data.IsolationLevel isolationLevel2 ? isolationLevel2 : System.Data.IsolationLevel.Serializable;
        this._defaultIsolation = this.GetEffectiveIsolationLevel(this._defaultIsolation);
        if (this._defaultIsolation != System.Data.IsolationLevel.Serializable && this._defaultIsolation != System.Data.IsolationLevel.ReadCommitted)
          throw new NotSupportedException("Invalid Default IsolationLevel specified");
        this._baseSchemaName = SQLiteConnection.FindKey(connectionString, "BaseSchemaName", "sqlite_default_schema");
        if (this._sql == null)
          this.SetupSQLiteBase(connectionString);
        SQLiteOpenFlagsEnum liteOpenFlagsEnum = SQLiteOpenFlagsEnum.None;
        if (!SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "FailIfMissing", false.ToString())))
          liteOpenFlagsEnum |= SQLiteOpenFlagsEnum.Create;
        SQLiteOpenFlagsEnum openFlags = !SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Read Only", false.ToString())) ? liteOpenFlagsEnum | SQLiteOpenFlagsEnum.ReadWrite : (liteOpenFlagsEnum | SQLiteOpenFlagsEnum.ReadOnly) & ~SQLiteOpenFlagsEnum.Create;
        if (flag3)
          openFlags |= SQLiteOpenFlagsEnum.Uri;
        this._sql.Open(str, this._vfsName, this._flags, openFlags, int32_1, boolean2);
        this._binaryGuid = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "BinaryGUID", true.ToString()));
        string key1 = SQLiteConnection.FindKey(connectionString, "HexPassword", (string) null);
        if (key1 != null)
        {
          string error = (string) null;
          this._sql.SetPassword(SQLiteConnection.FromHexString(key1, ref error) ?? throw new FormatException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Cannot parse 'HexPassword' property value into byte values: {0}", (object) error)));
        }
        else
        {
          string key2 = SQLiteConnection.FindKey(connectionString, "Password", (string) null);
          if (key2 != null)
            this._sql.SetPassword(Encoding.UTF8.GetBytes(key2));
          else if (this._password != null)
            this._sql.SetPassword(this._password);
        }
        this._password = (byte[]) null;
        if (flag1)
        {
          if (connectionString.ContainsKey("HexPassword"))
            connectionString["HexPassword"] = string.Empty;
          if (connectionString.ContainsKey("Password"))
            connectionString["Password"] = string.Empty;
          this._connectionString = SQLiteConnection.BuildConnectionString(connectionString);
        }
        this._dataSource = flag3 ? str : Path.GetFileNameWithoutExtension(str);
        ++this._version;
        ConnectionState connectionState = this._connectionState;
        this._connectionState = ConnectionState.Open;
        try
        {
          if (SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "SetDefaults", true.ToString())))
          {
            using (SQLiteCommand command = this.CreateCommand())
            {
              if (this._busyTimeout != 0)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA busy_timeout={0}", (object) this._busyTimeout);
                command.ExecuteNonQuery();
              }
              if (!flag3 && !flag4)
              {
                int int32_2 = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Page Size", SQLiteConvert.ToString(4096)), (IFormatProvider) CultureInfo.InvariantCulture);
                if (int32_2 != 4096)
                {
                  command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA page_size={0}", (object) int32_2);
                  command.ExecuteNonQuery();
                }
              }
              int int32_3 = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Max Page Count", SQLiteConvert.ToString(0)), (IFormatProvider) CultureInfo.InvariantCulture);
              if (int32_3 != 0)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA max_page_count={0}", (object) int32_3);
                command.ExecuteNonQuery();
              }
              bool boolean3 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Legacy Format", false.ToString()));
              if (boolean3)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA legacy_file_format={0}", boolean3 ? (object) "ON" : (object) "OFF");
                command.ExecuteNonQuery();
              }
              string key2 = SQLiteConnection.FindKey(connectionString, "Synchronous", SQLiteSynchronousEnum.Default.ToString());
              if (!(SQLiteConnection.TryParseEnum(typeof (SQLiteSynchronousEnum), key2, true) is SQLiteSynchronousEnum.Default))
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA synchronous={0}", (object) key2);
                command.ExecuteNonQuery();
              }
              int int32_4 = Convert.ToInt32(SQLiteConnection.FindKey(connectionString, "Cache Size", SQLiteConvert.ToString(-2000)), (IFormatProvider) CultureInfo.InvariantCulture);
              if (int32_4 != -2000)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA cache_size={0}", (object) int32_4);
                command.ExecuteNonQuery();
              }
              string key3 = SQLiteConnection.FindKey(connectionString, "Journal Mode", SQLiteJournalModeEnum.Default.ToString());
              if (!(SQLiteConnection.TryParseEnum(typeof (SQLiteJournalModeEnum), key3, true) is SQLiteJournalModeEnum.Default))
              {
                string format = "PRAGMA journal_mode={0}";
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, format, (object) key3);
                command.ExecuteNonQuery();
              }
              bool boolean4 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Foreign Keys", false.ToString()));
              if (boolean4)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA foreign_keys={0}", boolean4 ? (object) "ON" : (object) "OFF");
                command.ExecuteNonQuery();
              }
              bool boolean5 = SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Recursive Triggers", false.ToString()));
              if (boolean5)
              {
                command.CommandText = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA recursive_triggers={0}", boolean5 ? (object) "ON" : (object) "OFF");
                command.ExecuteNonQuery();
              }
            }
          }
          if (this._progressHandler != null)
            this._sql.SetProgressHook(this._progressOps, this._progressCallback);
          if (this._authorizerHandler != null)
            this._sql.SetAuthorizerHook(this._authorizerCallback);
          if (this._commitHandler != null)
            this._sql.SetCommitHook(this._commitCallback);
          if (this._updateHandler != null)
            this._sql.SetUpdateHook(this._updateCallback);
          if (this._rollbackHandler != null)
            this._sql.SetRollbackHook(this._rollbackCallback);
          Transaction current = Transaction.Current;
          if (current != (Transaction) null && SQLiteConvert.ToBoolean(SQLiteConnection.FindKey(connectionString, "Enlist", true.ToString())))
            this.EnlistTransaction(current);
          this._connectionState = connectionState;
          StateChangeEventArgs eventArgs = (StateChangeEventArgs) null;
          this.OnStateChange(ConnectionState.Open, ref eventArgs);
          SQLiteConnection.OnChanged(this, new ConnectionEventArgs(SQLiteConnectionEventType.Opened, eventArgs, (IDbTransaction) null, (IDbCommand) null, (IDataReader) null, (CriticalHandle) null, text, (object) new object[1]
          {
            (object) opts
          }));
        }
        catch
        {
          this._connectionState = connectionState;
          throw;
        }
      }
      catch (SQLiteException ex)
      {
        this.Close();
        throw;
      }
    }

    /// <summary>
    /// Opens the connection using the parameters found in the <see cref="P:System.Data.SQLite.SQLiteConnection.ConnectionString" /> and then returns it.
    /// </summary>
    /// <returns>The current connection object.</returns>
    public SQLiteConnection OpenAndReturn()
    {
      this.CheckDisposed();
      this.Open();
      return this;
    }

    /// <summary>
    /// Gets/sets the default command timeout for newly-created commands.  This is especially useful for
    /// commands used internally such as inside a SQLiteTransaction, where setting the timeout is not possible.
    /// This can also be set in the ConnectionString with "Default Timeout"
    /// </summary>
    public int DefaultTimeout
    {
      get
      {
        this.CheckDisposed();
        return this._defaultTimeout;
      }
      set
      {
        this.CheckDisposed();
        this._defaultTimeout = value;
      }
    }

    /// <summary>
    /// Gets/sets the default busy timeout to use with the SQLite core library.  This is only used when
    /// opening a connection.
    /// </summary>
    public int BusyTimeout
    {
      get
      {
        this.CheckDisposed();
        return this._busyTimeout;
      }
      set
      {
        this.CheckDisposed();
        this._busyTimeout = value;
      }
    }

    /// <summary>
    /// <b>EXPERIMENTAL</b> --
    /// The wait timeout to use with <see cref="M:System.Data.SQLite.SQLiteConnection.WaitForEnlistmentReset(System.Int32,System.Nullable{System.Boolean})" /> method.
    /// This is only used when waiting for the enlistment to be reset prior to
    /// enlisting in a transaction, and then only when the appropriate connection
    /// flag is set.
    /// </summary>
    public int WaitTimeout
    {
      get
      {
        this.CheckDisposed();
        return this._waitTimeout;
      }
      set
      {
        this.CheckDisposed();
        this._waitTimeout = value;
      }
    }

    /// <summary>
    /// The maximum number of retries when preparing SQL to be executed.  This
    /// normally only applies to preparation errors resulting from the database
    /// schema being changed.
    /// </summary>
    public int PrepareRetries
    {
      get
      {
        this.CheckDisposed();
        return this._prepareRetries;
      }
      set
      {
        this.CheckDisposed();
        this._prepareRetries = value;
      }
    }

    /// <summary>
    /// The approximate number of virtual machine instructions between progress
    /// events.  In order for progress events to actually fire, the event handler
    /// must be added to the <see cref="E:System.Data.SQLite.SQLiteConnection.Progress" /> event as
    /// well.  This value will only be used when the underlying native progress
    /// callback needs to be changed.
    /// </summary>
    public int ProgressOps
    {
      get
      {
        this.CheckDisposed();
        return this._progressOps;
      }
      set
      {
        this.CheckDisposed();
        this._progressOps = value;
      }
    }

    /// <summary>
    /// Non-zero if the built-in (i.e. framework provided) connection string
    /// parser should be used when opening the connection.
    /// </summary>
    public bool ParseViaFramework
    {
      get
      {
        this.CheckDisposed();
        return this._parseViaFramework;
      }
      set
      {
        this.CheckDisposed();
        this._parseViaFramework = value;
      }
    }

    /// <summary>
    /// Gets/sets the extra behavioral flags for this connection.  See the
    /// <see cref="T:System.Data.SQLite.SQLiteConnectionFlags" /> enumeration for a list of
    /// possible values.
    /// </summary>
    public SQLiteConnectionFlags Flags
    {
      get
      {
        this.CheckDisposed();
        return this._flags;
      }
      set
      {
        this.CheckDisposed();
        this._flags = value;
      }
    }

    /// <summary>
    /// Gets/sets the default database type for this connection.  This value
    /// will only be used when not null.
    /// </summary>
    public DbType? DefaultDbType
    {
      get
      {
        this.CheckDisposed();
        return this._defaultDbType;
      }
      set
      {
        this.CheckDisposed();
        this._defaultDbType = value;
      }
    }

    /// <summary>
    /// Gets/sets the default database type name for this connection.  This
    /// value will only be used when not null.
    /// </summary>
    public string DefaultTypeName
    {
      get
      {
        this.CheckDisposed();
        return this._defaultTypeName;
      }
      set
      {
        this.CheckDisposed();
        this._defaultTypeName = value;
      }
    }

    /// <summary>
    /// Gets/sets the VFS name for this connection.  This value will only be
    /// used when opening the database.
    /// </summary>
    public string VfsName
    {
      get
      {
        this.CheckDisposed();
        return this._vfsName;
      }
      set
      {
        this.CheckDisposed();
        this._vfsName = value;
      }
    }

    /// <summary>
    /// Returns non-zero if the underlying native connection handle is
    /// owned by this instance.
    /// </summary>
    public bool OwnHandle
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.OwnHandle : throw new InvalidOperationException("Database connection not valid for checking handle.");
      }
    }

    /// <summary>
    /// Returns the version of the underlying SQLite database engine
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public override string ServerVersion
    {
      get
      {
        this.CheckDisposed();
        return SQLiteConnection.SQLiteVersion;
      }
    }

    /// <summary>
    /// Returns the rowid of the most recent successful INSERT into the database from this connection.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public long LastInsertRowId
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.LastInsertRowId : throw new InvalidOperationException("Database connection not valid for getting last insert rowid.");
      }
    }

    /// <summary>
    /// This method causes any pending database operation to abort and return at
    /// its earliest opportunity.  This routine is typically called in response
    /// to a user action such as pressing "Cancel" or Ctrl-C where the user wants
    /// a long query operation to halt immediately.  It is safe to call this
    /// routine from any thread.  However, it is not safe to call this routine
    /// with a database connection that is closed or might close before this method
    /// returns.
    /// </summary>
    public void Cancel()
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for query cancellation.");
      this._sql.Cancel();
    }

    /// <summary>
    /// Returns the number of rows changed by the last INSERT, UPDATE, or DELETE statement executed on
    /// this connection.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public int Changes
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.Changes : throw new InvalidOperationException("Database connection not valid for getting number of changes.");
      }
    }

    /// <summary>
    /// Checks if this connection to the specified database should be considered
    /// read-only.  An exception will be thrown if the database name specified
    /// via <paramref name="name" /> cannot be found.
    /// </summary>
    /// <param name="name">
    /// The name of a database associated with this connection -OR- null for the
    /// main database.
    /// </param>
    /// <returns>
    /// Non-zero if this connection to the specified database should be considered
    /// read-only.
    /// </returns>
    public bool IsReadOnly(string name)
    {
      this.CheckDisposed();
      return this._sql != null ? this._sql.IsReadOnly(name) : throw new InvalidOperationException("Database connection not valid for checking read-only status.");
    }

    /// <summary>
    /// Returns non-zero if the given database connection is in autocommit mode.
    /// Autocommit mode is on by default.  Autocommit mode is disabled by a BEGIN
    /// statement.  Autocommit mode is re-enabled by a COMMIT or ROLLBACK.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public bool AutoCommit
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.AutoCommit : throw new InvalidOperationException("Database connection not valid for getting autocommit mode.");
      }
    }

    /// <summary>
    /// Returns the amount of memory (in bytes) currently in use by the SQLite core library.
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public long MemoryUsed
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.MemoryUsed : throw new InvalidOperationException("Database connection not valid for getting memory used.");
      }
    }

    /// <summary>
    /// Returns the maximum amount of memory (in bytes) used by the SQLite core library since the high-water mark was last reset.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public long MemoryHighwater
    {
      get
      {
        this.CheckDisposed();
        return this._sql != null ? this._sql.MemoryHighwater : throw new InvalidOperationException("Database connection not valid for getting maximum memory used.");
      }
    }

    /// <summary>
    /// Returns various global memory statistics for the SQLite core library via
    /// a dictionary of key/value pairs.  Currently, only the "MemoryUsed" and
    /// "MemoryHighwater" keys are returned and they have values that correspond
    /// to the values that could be obtained via the <see cref="P:System.Data.SQLite.SQLiteConnection.MemoryUsed" />
    /// and <see cref="P:System.Data.SQLite.SQLiteConnection.MemoryHighwater" /> connection properties.
    /// </summary>
    /// <param name="statistics">
    /// This dictionary will be populated with the global memory statistics.  It
    /// will be created if necessary.
    /// </param>
    public static void GetMemoryStatistics(ref IDictionary<string, long> statistics)
    {
      if (statistics == null)
        statistics = (IDictionary<string, long>) new Dictionary<string, long>();
      statistics["MemoryUsed"] = SQLite3.StaticMemoryUsed;
      statistics["MemoryHighwater"] = SQLite3.StaticMemoryHighwater;
    }

    /// <summary>
    /// Attempts to free as much heap memory as possible for this database connection.
    /// </summary>
    public void ReleaseMemory()
    {
      this.CheckDisposed();
      SQLiteErrorCode errorCode = this._sql != null ? this._sql.ReleaseMemory() : throw new InvalidOperationException("Database connection not valid for releasing memory.");
      if (errorCode != SQLiteErrorCode.Ok)
        throw new SQLiteException(errorCode, this._sql.GetLastError("Could not release connection memory."));
    }

    /// <summary>
    /// Attempts to free N bytes of heap memory by deallocating non-essential memory
    /// allocations held by the database library. Memory used to cache database pages
    /// to improve performance is an example of non-essential memory.  This is a no-op
    /// returning zero if the SQLite core library was not compiled with the compile-time
    /// option SQLITE_ENABLE_MEMORY_MANAGEMENT.  Optionally, attempts to reset and/or
    /// compact the Win32 native heap, if applicable.
    /// </summary>
    /// <param name="nBytes">The requested number of bytes to free.</param>
    /// <param name="reset">Non-zero to attempt a heap reset.</param>
    /// <param name="compact">Non-zero to attempt heap compaction.</param>
    /// <param name="nFree">
    /// The number of bytes actually freed.  This value may be zero.
    /// </param>
    /// <param name="resetOk">
    /// This value will be non-zero if the heap reset was successful.
    /// </param>
    /// <param name="nLargest">
    /// The size of the largest committed free block in the heap, in bytes.
    /// This value will be zero unless heap compaction is enabled.
    /// </param>
    /// <returns>
    /// A standard SQLite return code (i.e. zero for success and non-zero
    /// for failure).
    /// </returns>
    public static SQLiteErrorCode ReleaseMemory(
      int nBytes,
      bool reset,
      bool compact,
      ref int nFree,
      ref bool resetOk,
      ref uint nLargest)
    {
      return SQLite3.StaticReleaseMemory(nBytes, reset, compact, ref nFree, ref resetOk, ref nLargest);
    }

    /// <summary>
    /// Sets the status of the memory usage tracking subsystem in the SQLite core library.  By default, this is enabled.
    /// If this is disabled, memory usage tracking will not be performed.  This is not really a per-connection value, it is
    /// global to the process.
    /// </summary>
    /// <param name="value">Non-zero to enable memory usage tracking, zero otherwise.</param>
    /// <returns>A standard SQLite return code (i.e. zero for success and non-zero for failure).</returns>
    public static SQLiteErrorCode SetMemoryStatus(bool value) => SQLite3.StaticSetMemoryStatus(value);

    /// <summary>
    /// Returns a string containing the define constants (i.e. compile-time
    /// options) used to compile the core managed assembly, delimited with
    /// spaces.
    /// </summary>
    public static string DefineConstants => SQLite3.DefineConstants;

    /// <summary>
    /// Returns the version of the underlying SQLite core library.
    /// </summary>
    public static string SQLiteVersion => SQLite3.SQLiteVersion;

    /// <summary>
    /// This method returns the string whose value is the same as the
    /// SQLITE_SOURCE_ID C preprocessor macro used when compiling the
    /// SQLite core library.
    /// </summary>
    public static string SQLiteSourceId => SQLite3.SQLiteSourceId;

    /// <summary>
    /// Returns a string containing the compile-time options used to
    /// compile the SQLite core native library, delimited with spaces.
    /// </summary>
    public static string SQLiteCompileOptions => SQLite3.SQLiteCompileOptions;

    /// <summary>
    /// This method returns the version of the interop SQLite assembly
    /// used.  If the SQLite interop assembly is not in use or the
    /// necessary information cannot be obtained for any reason, a null
    /// value may be returned.
    /// </summary>
    public static string InteropVersion => SQLite3.InteropVersion;

    /// <summary>
    /// This method returns the string whose value contains the unique
    /// identifier for the source checkout used to build the interop
    /// assembly.  If the SQLite interop assembly is not in use or the
    /// necessary information cannot be obtained for any reason, a null
    /// value may be returned.
    /// </summary>
    public static string InteropSourceId => SQLite3.InteropSourceId;

    /// <summary>
    /// Returns a string containing the compile-time options used to
    /// compile the SQLite interop assembly, delimited with spaces.
    /// </summary>
    public static string InteropCompileOptions => SQLite3.InteropCompileOptions;

    /// <summary>
    /// This method returns the version of the managed components used
    /// to interact with the SQLite core library.  If the necessary
    /// information cannot be obtained for any reason, a null value may
    /// be returned.
    /// </summary>
    public static string ProviderVersion => !(SQLiteConnection._assembly != (Assembly) null) ? (string) null : SQLiteConnection._assembly.GetName().Version.ToString();

    /// <summary>
    /// This method returns the string whose value contains the unique
    /// identifier for the source checkout used to build the managed
    /// components currently executing.  If the necessary information
    /// cannot be obtained for any reason, a null value may be returned.
    /// </summary>
    public static string ProviderSourceId
    {
      get
      {
        if (SQLiteConnection._assembly == (Assembly) null)
          return (string) null;
        string str1 = (string) null;
        if (SQLiteConnection._assembly.IsDefined(typeof (AssemblySourceIdAttribute), false))
          str1 = ((AssemblySourceIdAttribute) SQLiteConnection._assembly.GetCustomAttributes(typeof (AssemblySourceIdAttribute), false)[0]).SourceId;
        string str2 = (string) null;
        if (SQLiteConnection._assembly.IsDefined(typeof (AssemblySourceTimeStampAttribute), false))
          str2 = ((AssemblySourceTimeStampAttribute) SQLiteConnection._assembly.GetCustomAttributes(typeof (AssemblySourceTimeStampAttribute), false)[0]).SourceTimeStamp;
        if (str1 == null && str2 == null)
          return (string) null;
        if (str1 == null)
          str1 = "0000000000000000000000000000000000000000";
        if (str2 == null)
          str2 = "0000-00-00 00:00:00 UTC";
        return HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}", (object) str1, (object) str2);
      }
    }

    /// <summary>
    /// Queries and returns the value of the specified setting, using the
    /// cached setting names and values for the last connection that used
    /// the <see cref="M:System.Data.SQLite.SQLiteConnection.Open" /> method, when available.
    /// </summary>
    /// <param name="name">The name of the setting.</param>
    /// <param name="default">
    /// The value to be returned if the setting has not been set explicitly
    /// or cannot be determined.
    /// </param>
    /// <param name="value">
    /// The value of the cached setting is stored here if found; otherwise,
    /// the value of <paramref name="default" /> is stored here.
    /// </param>
    /// <returns>
    /// Non-zero if the cached setting was found; otherwise, zero.
    /// </returns>
    private static bool TryGetLastCachedSetting(string name, object @default, out object value)
    {
      if (SQLiteConnection._lastConnectionInOpen != null)
        return SQLiteConnection._lastConnectionInOpen.TryGetCachedSetting(name, @default, out value);
      value = @default;
      return false;
    }

    /// <summary>
    /// Adds or sets the cached setting specified by <paramref name="name" />
    /// to the value specified by <paramref name="value" /> using the cached
    /// setting names and values for the last connection that used the
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.Open" /> method, when available.
    /// </summary>
    /// <param name="name">
    /// The name of the cached setting to add or replace.
    /// </param>
    /// <param name="value">The new value of the cached setting.</param>
    private static void SetLastCachedSetting(string name, object value)
    {
      if (SQLiteConnection._lastConnectionInOpen == null)
        return;
      SQLiteConnection._lastConnectionInOpen.SetCachedSetting(name, value);
    }

    /// <summary>
    /// The default connection flags to be used for all opened connections
    /// when they are not present in the connection string.
    /// </summary>
    public static SQLiteConnectionFlags DefaultFlags
    {
      get
      {
        string name = "DefaultFlags_SQLiteConnection";
        object settingValue;
        if (!SQLiteConnection.TryGetLastCachedSetting(name, (object) null, out settingValue))
        {
          settingValue = (object) UnsafeNativeMethods.GetSettingValue(name, (string) null);
          SQLiteConnection.SetLastCachedSetting(name, settingValue);
        }
        return settingValue == null || !(SQLiteConnection.TryParseEnum(typeof (SQLiteConnectionFlags), settingValue.ToString(), true) is SQLiteConnectionFlags liteConnectionFlags) ? SQLiteConnectionFlags.Default : liteConnectionFlags;
      }
    }

    /// <summary>
    /// The extra connection flags to be used for all opened connections.
    /// </summary>
    public static SQLiteConnectionFlags SharedFlags
    {
      get
      {
        lock (SQLiteConnection._syncRoot)
          return SQLiteConnection._sharedFlags;
      }
      set
      {
        lock (SQLiteConnection._syncRoot)
          SQLiteConnection._sharedFlags = value;
      }
    }

    /// <summary>Returns the state of the connection.</summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override ConnectionState State
    {
      get
      {
        this.CheckDisposed();
        return this._connectionState;
      }
    }

    /// <summary>
    /// Passes a shutdown request to the SQLite core library.  Does not throw
    /// an exception if the shutdown request fails.
    /// </summary>
    /// <returns>
    /// A standard SQLite return code (i.e. zero for success and non-zero for
    /// failure).
    /// </returns>
    public SQLiteErrorCode Shutdown()
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for shutdown.");
      this._sql.Close(false);
      return this._sql.Shutdown();
    }

    /// <summary>
    /// Passes a shutdown request to the SQLite core library.  Throws an
    /// exception if the shutdown request fails and the no-throw parameter
    /// is non-zero.
    /// </summary>
    /// <param name="directories">
    /// Non-zero to reset the database and temporary directories to their
    /// default values, which should be null for both.
    /// </param>
    /// <param name="noThrow">
    /// When non-zero, throw an exception if the shutdown request fails.
    /// </param>
    public static void Shutdown(bool directories, bool noThrow)
    {
      SQLiteErrorCode errorCode = SQLite3.StaticShutdown(directories);
      if (errorCode != SQLiteErrorCode.Ok && !noThrow)
        throw new SQLiteException(errorCode, (string) null);
    }

    /// Enables or disables extended result codes returned by SQLite
    public void SetExtendedResultCodes(bool bOnOff)
    {
      this.CheckDisposed();
      if (this._sql == null)
        return;
      this._sql.SetExtendedResultCodes(bOnOff);
    }

    /// Enables or disables extended result codes returned by SQLite
    public SQLiteErrorCode ResultCode()
    {
      this.CheckDisposed();
      return this._sql != null ? this._sql.ResultCode() : throw new InvalidOperationException("Database connection not valid for getting result code.");
    }

    /// Enables or disables extended result codes returned by SQLite
    public SQLiteErrorCode ExtendedResultCode()
    {
      this.CheckDisposed();
      return this._sql != null ? this._sql.ExtendedResultCode() : throw new InvalidOperationException("Database connection not valid for getting extended result code.");
    }

    /// Add a log message via the SQLite sqlite3_log interface.
    public void LogMessage(SQLiteErrorCode iErrCode, string zMessage)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for logging message.");
      this._sql.LogMessage(iErrCode, zMessage);
    }

    /// Add a log message via the SQLite sqlite3_log interface.
    public void LogMessage(int iErrCode, string zMessage)
    {
      this.CheckDisposed();
      if (this._sql == null)
        throw new InvalidOperationException("Database connection not valid for logging message.");
      this._sql.LogMessage((SQLiteErrorCode) iErrCode, zMessage);
    }

    /// <summary>
    /// Change the password (or assign a password) to an open database.
    /// </summary>
    /// <remarks>
    /// No readers or writers may be active for this process.  The database must already be open
    /// and if it already was password protected, the existing password must already have been supplied.
    /// </remarks>
    /// <param name="newPassword">The new password to assign to the database</param>
    public void ChangePassword(string newPassword)
    {
      this.CheckDisposed();
      if (!string.IsNullOrEmpty(newPassword))
        this.ChangePassword(Encoding.UTF8.GetBytes(newPassword));
      else
        this.ChangePassword((byte[]) null);
    }

    /// <summary>
    /// Change the password (or assign a password) to an open database.
    /// </summary>
    /// <remarks>
    /// No readers or writers may be active for this process.  The database must already be open
    /// and if it already was password protected, the existing password must already have been supplied.
    /// </remarks>
    /// <param name="newPassword">The new password to assign to the database</param>
    public void ChangePassword(byte[] newPassword)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException("Database must be opened before changing the password.");
      this._sql.ChangePassword(newPassword);
    }

    /// <summary>
    /// Sets the password for a password-protected database.  A password-protected database is
    /// unusable for any operation until the password has been set.
    /// </summary>
    /// <param name="databasePassword">The password for the database</param>
    public void SetPassword(string databasePassword)
    {
      this.CheckDisposed();
      if (!string.IsNullOrEmpty(databasePassword))
        this.SetPassword(Encoding.UTF8.GetBytes(databasePassword));
      else
        this.SetPassword((byte[]) null);
    }

    /// <summary>
    /// Sets the password for a password-protected database.  A password-protected database is
    /// unusable for any operation until the password has been set.
    /// </summary>
    /// <param name="databasePassword">The password for the database</param>
    public void SetPassword(byte[] databasePassword)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Closed)
        throw new InvalidOperationException("Password can only be set before the database is opened.");
      if (databasePassword != null && databasePassword.Length == 0)
        databasePassword = (byte[]) null;
      this._password = databasePassword == null || !HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.HidePassword) ? databasePassword : throw new InvalidOperationException("With 'HidePassword' enabled, passwords can only be set via the connection string.");
    }

    /// <summary>
    /// Queries or modifies the number of retries or the retry interval (in milliseconds) for
    /// certain I/O operations that may fail due to anti-virus software.
    /// </summary>
    /// <param name="count">The number of times to retry the I/O operation.  A negative value
    /// will cause the current count to be queried and replace that negative value.</param>
    /// <param name="interval">The number of milliseconds to wait before retrying the I/O
    /// operation.  This number is multiplied by the number of retry attempts so far to come
    /// up with the final number of milliseconds to wait.  A negative value will cause the
    /// current interval to be queried and replace that negative value.</param>
    /// <returns>Zero for success, non-zero for error.</returns>
    public SQLiteErrorCode SetAvRetry(ref int count, ref int interval)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException("Database must be opened before changing the AV retry parameters.");
      IntPtr num = IntPtr.Zero;
      SQLiteErrorCode sqLiteErrorCode;
      try
      {
        num = Marshal.AllocHGlobal(8);
        Marshal.WriteInt32(num, 0, count);
        Marshal.WriteInt32(num, 4, interval);
        sqLiteErrorCode = this._sql.FileControl((string) null, 9, num);
        if (sqLiteErrorCode == SQLiteErrorCode.Ok)
        {
          count = Marshal.ReadInt32(num, 0);
          interval = Marshal.ReadInt32(num, 4);
        }
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
      return sqLiteErrorCode;
    }

    /// <summary>
    /// Sets the chunk size for the primary file associated with this database
    /// connection.
    /// </summary>
    /// <param name="size">
    /// The new chunk size for the main database, in bytes.
    /// </param>
    /// <returns>Zero for success, non-zero for error.</returns>
    public SQLiteErrorCode SetChunkSize(int size)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException("Database must be opened before changing the chunk size.");
      IntPtr num = IntPtr.Zero;
      try
      {
        num = Marshal.AllocHGlobal(4);
        Marshal.WriteInt32(num, 0, size);
        return this._sql.FileControl((string) null, 6, num);
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
    }

    /// <summary>
    /// Removes one set of surrounding single -OR- double quotes from the string
    /// value and returns the resulting string value.  If the string is null, empty,
    /// or contains quotes that are not balanced, nothing is done and the original
    /// string value will be returned.
    /// </summary>
    /// <param name="value">The string value to process.</param>
    /// <returns>
    /// The string value, modified to remove one set of surrounding single -OR-
    /// double quotes, if applicable.
    /// </returns>
    private static string UnwrapString(string value)
    {
      if (string.IsNullOrEmpty(value))
        return value;
      int length = value.Length;
      return value[0] == '"' && value[length - 1] == '"' || value[0] == '\'' && value[length - 1] == '\'' ? value.Substring(1, length - 2) : value;
    }

    /// <summary>
    /// Determines the directory to be used when dealing with the "|DataDirectory|"
    /// macro in a database file name.
    /// </summary>
    /// <returns>
    /// The directory to use in place of the "|DataDirectory|" macro -OR- null if it
    /// cannot be determined.
    /// </returns>
    private static string GetDataDirectory()
    {
      string str = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
      if (string.IsNullOrEmpty(str))
        str = AppDomain.CurrentDomain.BaseDirectory;
      return str;
    }

    /// <summary>
    /// Expand the filename of the data source, resolving the |DataDirectory|
    /// macro as appropriate.
    /// </summary>
    /// <param name="sourceFile">The database filename to expand</param>
    /// <param name="toFullPath">
    /// Non-zero if the returned file name should be converted to a full path
    /// (except when using the .NET Compact Framework).
    /// </param>
    /// <returns>The expanded path and filename of the filename</returns>
    private static string ExpandFileName(string sourceFile, bool toFullPath)
    {
      if (string.IsNullOrEmpty(sourceFile))
        return sourceFile;
      if (sourceFile.StartsWith("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
      {
        string dataDirectory = SQLiteConnection.GetDataDirectory();
        if (sourceFile.Length > "|DataDirectory|".Length && ((int) sourceFile["|DataDirectory|".Length] == (int) Path.DirectorySeparatorChar || (int) sourceFile["|DataDirectory|".Length] == (int) Path.AltDirectorySeparatorChar))
          sourceFile = sourceFile.Remove("|DataDirectory|".Length, 1);
        sourceFile = Path.Combine(dataDirectory, sourceFile.Substring("|DataDirectory|".Length));
      }
      if (toFullPath)
        sourceFile = Path.GetFullPath(sourceFile);
      return sourceFile;
    }

    /// <overloads>
    /// The following commands are used to extract schema information out of the database.  Valid schema types are:
    /// <list type="bullet">
    /// <item>
    /// <description>MetaDataCollections</description>
    /// </item>
    /// <item>
    /// <description>DataSourceInformation</description>
    /// </item>
    /// <item>
    /// <description>Catalogs</description>
    /// </item>
    /// <item>
    /// <description>Columns</description>
    /// </item>
    /// <item>
    /// <description>ForeignKeys</description>
    /// </item>
    /// <item>
    /// <description>Indexes</description>
    /// </item>
    /// <item>
    /// <description>IndexColumns</description>
    /// </item>
    /// <item>
    /// <description>Tables</description>
    /// </item>
    /// <item>
    /// <description>Views</description>
    /// </item>
    /// <item>
    /// <description>ViewColumns</description>
    /// </item>
    /// </list>
    /// </overloads>
    /// <summary>Returns the MetaDataCollections schema</summary>
    /// <returns>A DataTable of the MetaDataCollections schema</returns>
    public override DataTable GetSchema()
    {
      this.CheckDisposed();
      return this.GetSchema("MetaDataCollections", (string[]) null);
    }

    /// <summary>
    /// Returns schema information of the specified collection
    /// </summary>
    /// <param name="collectionName">The schema collection to retrieve</param>
    /// <returns>A DataTable of the specified collection</returns>
    public override DataTable GetSchema(string collectionName)
    {
      this.CheckDisposed();
      return this.GetSchema(collectionName, new string[0]);
    }

    /// <summary>
    /// Retrieves schema information using the specified constraint(s) for the specified collection
    /// </summary>
    /// <param name="collectionName">The collection to retrieve.</param>
    /// <param name="restrictionValues">
    /// The restrictions to impose.  Typically, this may include:
    /// <list type="table">
    /// <listheader>
    /// <term>restrictionValues element index</term>
    /// <term>usage</term>
    /// </listheader>
    /// <item>
    /// <description>0</description>
    /// <description>The database (or catalog) name, if applicable.</description>
    /// </item>
    /// <item>
    /// <description>1</description>
    /// <description>The schema name.  This is not used by this provider.</description>
    /// </item>
    /// <item>
    /// <description>2</description>
    /// <description>The table name, if applicable.</description>
    /// </item>
    /// <item>
    /// <description>3</description>
    /// <description>
    /// Depends on <paramref name="collectionName" />.
    /// When "IndexColumns", it is the index name; otherwise, it is the column name.
    /// </description>
    /// </item>
    /// <item>
    /// <description>4</description>
    /// <description>
    /// Depends on <paramref name="collectionName" />.
    /// When "IndexColumns", it is the column name; otherwise, it is not used.
    /// </description>
    /// </item>
    /// </list>
    /// </param>
    /// <returns>A DataTable of the specified collection</returns>
    public override DataTable GetSchema(string collectionName, string[] restrictionValues)
    {
      this.CheckDisposed();
      if (this._connectionState != ConnectionState.Open)
        throw new InvalidOperationException();
      string[] strArray = new string[5];
      if (restrictionValues == null)
        restrictionValues = new string[0];
      restrictionValues.CopyTo((Array) strArray, 0);
      switch (collectionName.ToUpper(CultureInfo.InvariantCulture))
      {
        case "METADATACOLLECTIONS":
          return SQLiteConnection.Schema_MetaDataCollections();
        case "DATASOURCEINFORMATION":
          return this.Schema_DataSourceInformation();
        case "DATATYPES":
          return this.Schema_DataTypes();
        case "COLUMNS":
        case "TABLECOLUMNS":
          return this.Schema_Columns(strArray[0], strArray[2], strArray[3]);
        case "INDEXES":
          return this.Schema_Indexes(strArray[0], strArray[2], strArray[3]);
        case "TRIGGERS":
          return this.Schema_Triggers(strArray[0], strArray[2], strArray[3]);
        case "INDEXCOLUMNS":
          return this.Schema_IndexColumns(strArray[0], strArray[2], strArray[3], strArray[4]);
        case "TABLES":
          return this.Schema_Tables(strArray[0], strArray[2], strArray[3]);
        case "VIEWS":
          return this.Schema_Views(strArray[0], strArray[2]);
        case "VIEWCOLUMNS":
          return this.Schema_ViewColumns(strArray[0], strArray[2], strArray[3]);
        case "FOREIGNKEYS":
          return this.Schema_ForeignKeys(strArray[0], strArray[2], strArray[3]);
        case "CATALOGS":
          return this.Schema_Catalogs(strArray[0]);
        case "RESERVEDWORDS":
          return SQLiteConnection.Schema_ReservedWords();
        default:
          throw new NotSupportedException();
      }
    }

    private static DataTable Schema_ReservedWords()
    {
      DataTable dataTable = new DataTable("ReservedWords");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("ReservedWord", typeof (string));
      dataTable.Columns.Add("MaximumVersion", typeof (string));
      dataTable.Columns.Add("MinimumVersion", typeof (string));
      dataTable.BeginLoadData();
      string keywords = SR.Keywords;
      char[] chArray = new char[1]{ ',' };
      foreach (string str in keywords.Split(chArray))
      {
        DataRow row = dataTable.NewRow();
        row[0] = (object) str;
        dataTable.Rows.Add(row);
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    /// <summary>Builds a MetaDataCollections schema datatable</summary>
    /// <returns>DataTable</returns>
    private static DataTable Schema_MetaDataCollections()
    {
      DataTable dataTable = new DataTable("MetaDataCollections");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("CollectionName", typeof (string));
      dataTable.Columns.Add("NumberOfRestrictions", typeof (int));
      dataTable.Columns.Add("NumberOfIdentifierParts", typeof (int));
      dataTable.BeginLoadData();
      StringReader stringReader = new StringReader(SR.MetaDataCollections);
      int num = (int) dataTable.ReadXml((TextReader) stringReader);
      stringReader.Close();
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    /// <summary>Builds a DataSourceInformation datatable</summary>
    /// <returns>DataTable</returns>
    private DataTable Schema_DataSourceInformation()
    {
      DataTable dataTable = new DataTable("DataSourceInformation");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add(DbMetaDataColumnNames.CompositeIdentifierSeparatorPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.DataSourceProductName, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.DataSourceProductVersion, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.DataSourceProductVersionNormalized, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.GroupByBehavior, typeof (int));
      dataTable.Columns.Add(DbMetaDataColumnNames.IdentifierPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.IdentifierCase, typeof (int));
      dataTable.Columns.Add(DbMetaDataColumnNames.OrderByColumnsInSelect, typeof (bool));
      dataTable.Columns.Add(DbMetaDataColumnNames.ParameterMarkerFormat, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.ParameterMarkerPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.ParameterNameMaxLength, typeof (int));
      dataTable.Columns.Add(DbMetaDataColumnNames.ParameterNamePattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.QuotedIdentifierPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.QuotedIdentifierCase, typeof (int));
      dataTable.Columns.Add(DbMetaDataColumnNames.StatementSeparatorPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.StringLiteralPattern, typeof (string));
      dataTable.Columns.Add(DbMetaDataColumnNames.SupportedJoinOperators, typeof (int));
      dataTable.BeginLoadData();
      DataRow row = dataTable.NewRow();
      row.ItemArray = new object[17]
      {
        null,
        (object) "SQLite",
        (object) this._sql.Version,
        (object) this._sql.Version,
        (object) 3,
        (object) "(^\\[\\p{Lo}\\p{Lu}\\p{Ll}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Nd}@$#_]*$)|(^\\[[^\\]\\0]|\\]\\]+\\]$)|(^\\\"[^\\\"\\0]|\\\"\\\"+\\\"$)",
        (object) 1,
        (object) false,
        (object) "{0}",
        (object) "@[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_@#\\$]*(?=\\s+|$)",
        (object) (int) byte.MaxValue,
        (object) "^[\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}_@#][\\p{Lo}\\p{Lu}\\p{Ll}\\p{Lm}\\p{Nd}\\uff3f_@#\\$]*(?=\\s+|$)",
        (object) "(([^\\[]|\\]\\])*)",
        (object) 1,
        (object) ";",
        (object) "'(([^']|'')*)'",
        (object) 15
      };
      dataTable.Rows.Add(row);
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    /// <summary>Build a Columns schema</summary>
    /// <param name="strCatalog">The catalog (attached database) to query, can be null</param>
    /// <param name="strTable">The table to retrieve schema information for, can be null</param>
    /// <param name="strColumn">The column to retrieve schema information for, can be null</param>
    /// <returns>DataTable</returns>
    private DataTable Schema_Columns(string strCatalog, string strTable, string strColumn)
    {
      DataTable dataTable = new DataTable("Columns");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("COLUMN_NAME", typeof (string));
      dataTable.Columns.Add("COLUMN_GUID", typeof (Guid));
      dataTable.Columns.Add("COLUMN_PROPID", typeof (long));
      dataTable.Columns.Add("ORDINAL_POSITION", typeof (int));
      dataTable.Columns.Add("COLUMN_HASDEFAULT", typeof (bool));
      dataTable.Columns.Add("COLUMN_DEFAULT", typeof (string));
      dataTable.Columns.Add("COLUMN_FLAGS", typeof (long));
      dataTable.Columns.Add("IS_NULLABLE", typeof (bool));
      dataTable.Columns.Add("DATA_TYPE", typeof (string));
      dataTable.Columns.Add("TYPE_GUID", typeof (Guid));
      dataTable.Columns.Add("CHARACTER_MAXIMUM_LENGTH", typeof (int));
      dataTable.Columns.Add("CHARACTER_OCTET_LENGTH", typeof (int));
      dataTable.Columns.Add("NUMERIC_PRECISION", typeof (int));
      dataTable.Columns.Add("NUMERIC_SCALE", typeof (int));
      dataTable.Columns.Add("DATETIME_PRECISION", typeof (long));
      dataTable.Columns.Add("CHARACTER_SET_CATALOG", typeof (string));
      dataTable.Columns.Add("CHARACTER_SET_SCHEMA", typeof (string));
      dataTable.Columns.Add("CHARACTER_SET_NAME", typeof (string));
      dataTable.Columns.Add("COLLATION_CATALOG", typeof (string));
      dataTable.Columns.Add("COLLATION_SCHEMA", typeof (string));
      dataTable.Columns.Add("COLLATION_NAME", typeof (string));
      dataTable.Columns.Add("DOMAIN_CATALOG", typeof (string));
      dataTable.Columns.Add("DOMAIN_NAME", typeof (string));
      dataTable.Columns.Add("DESCRIPTION", typeof (string));
      dataTable.Columns.Add("PRIMARY_KEY", typeof (bool));
      dataTable.Columns.Add("EDM_TYPE", typeof (string));
      dataTable.Columns.Add("AUTOINCREMENT", typeof (bool));
      dataTable.Columns.Add("UNIQUE", typeof (bool));
      dataTable.BeginLoadData();
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = SQLiteConnection.GetDefaultCatalogName();
      string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
      using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table' OR [type] LIKE 'view'", (object) strCatalog, (object) masterTableName), this))
      {
        using (SQLiteDataReader sqLiteDataReader1 = sqLiteCommand1.ExecuteReader())
        {
          while (sqLiteDataReader1.Read())
          {
            if (!string.IsNullOrEmpty(strTable))
            {
              if (string.Compare(strTable, sqLiteDataReader1.GetString(2), StringComparison.OrdinalIgnoreCase) != 0)
                continue;
            }
            try
            {
              using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}]", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
              {
                using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                  using (DataTable schemaTable = sqLiteDataReader2.GetSchemaTable(true, true))
                  {
                    foreach (DataRow row1 in (InternalDataCollectionBase) schemaTable.Rows)
                    {
                      if (string.Compare(row1[SchemaTableColumn.ColumnName].ToString(), strColumn, StringComparison.OrdinalIgnoreCase) == 0 || strColumn == null)
                      {
                        DataRow row2 = dataTable.NewRow();
                        row2["NUMERIC_PRECISION"] = row1[SchemaTableColumn.NumericPrecision];
                        row2["NUMERIC_SCALE"] = row1[SchemaTableColumn.NumericScale];
                        row2["TABLE_NAME"] = (object) sqLiteDataReader1.GetString(2);
                        row2["COLUMN_NAME"] = row1[SchemaTableColumn.ColumnName];
                        row2["TABLE_CATALOG"] = (object) strCatalog;
                        row2["ORDINAL_POSITION"] = row1[SchemaTableColumn.ColumnOrdinal];
                        row2["COLUMN_HASDEFAULT"] = (object) (row1[SchemaTableOptionalColumn.DefaultValue] != DBNull.Value);
                        row2["COLUMN_DEFAULT"] = row1[SchemaTableOptionalColumn.DefaultValue];
                        row2["IS_NULLABLE"] = row1[SchemaTableColumn.AllowDBNull];
                        row2["DATA_TYPE"] = (object) row1["DataTypeName"].ToString().ToLower(CultureInfo.InvariantCulture);
                        row2["EDM_TYPE"] = (object) SQLiteConvert.DbTypeToTypeName(this, (DbType) row1[SchemaTableColumn.ProviderType], this._flags).ToString().ToLower(CultureInfo.InvariantCulture);
                        row2["CHARACTER_MAXIMUM_LENGTH"] = row1[SchemaTableColumn.ColumnSize];
                        row2["TABLE_SCHEMA"] = row1[SchemaTableColumn.BaseSchemaName];
                        row2["PRIMARY_KEY"] = row1[SchemaTableColumn.IsKey];
                        row2["AUTOINCREMENT"] = row1[SchemaTableOptionalColumn.IsAutoIncrement];
                        row2["COLLATION_NAME"] = row1["CollationType"];
                        row2["UNIQUE"] = row1[SchemaTableColumn.IsUnique];
                        dataTable.Rows.Add(row2);
                      }
                    }
                  }
                }
              }
            }
            catch (SQLiteException ex)
            {
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    /// <summary>
    /// Returns index information for the given database and catalog
    /// </summary>
    /// <param name="strCatalog">The catalog (attached database) to query, can be null</param>
    /// <param name="strIndex">The name of the index to retrieve information for, can be null</param>
    /// <param name="strTable">The table to retrieve index information for, can be null</param>
    /// <returns>DataTable</returns>
    private DataTable Schema_Indexes(string strCatalog, string strTable, string strIndex)
    {
      DataTable dataTable = new DataTable("Indexes");
      List<int> intList = new List<int>();
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("INDEX_CATALOG", typeof (string));
      dataTable.Columns.Add("INDEX_SCHEMA", typeof (string));
      dataTable.Columns.Add("INDEX_NAME", typeof (string));
      dataTable.Columns.Add("PRIMARY_KEY", typeof (bool));
      dataTable.Columns.Add("UNIQUE", typeof (bool));
      dataTable.Columns.Add("CLUSTERED", typeof (bool));
      dataTable.Columns.Add("TYPE", typeof (int));
      dataTable.Columns.Add("FILL_FACTOR", typeof (int));
      dataTable.Columns.Add("INITIAL_SIZE", typeof (int));
      dataTable.Columns.Add("NULLS", typeof (int));
      dataTable.Columns.Add("SORT_BOOKMARKS", typeof (bool));
      dataTable.Columns.Add("AUTO_UPDATE", typeof (bool));
      dataTable.Columns.Add("NULL_COLLATION", typeof (int));
      dataTable.Columns.Add("ORDINAL_POSITION", typeof (int));
      dataTable.Columns.Add("COLUMN_NAME", typeof (string));
      dataTable.Columns.Add("COLUMN_GUID", typeof (Guid));
      dataTable.Columns.Add("COLUMN_PROPID", typeof (long));
      dataTable.Columns.Add("COLLATION", typeof (short));
      dataTable.Columns.Add("CARDINALITY", typeof (Decimal));
      dataTable.Columns.Add("PAGES", typeof (int));
      dataTable.Columns.Add("FILTER_CONDITION", typeof (string));
      dataTable.Columns.Add("INTEGRATED", typeof (bool));
      dataTable.Columns.Add("INDEX_DEFINITION", typeof (string));
      dataTable.BeginLoadData();
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = SQLiteConnection.GetDefaultCatalogName();
      string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
      using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table'", (object) strCatalog, (object) masterTableName), this))
      {
        using (SQLiteDataReader sqLiteDataReader1 = sqLiteCommand1.ExecuteReader())
        {
          while (sqLiteDataReader1.Read())
          {
            bool flag = false;
            intList.Clear();
            if (!string.IsNullOrEmpty(strTable))
            {
              if (string.Compare(sqLiteDataReader1.GetString(2), strTable, StringComparison.OrdinalIgnoreCase) != 0)
                continue;
            }
            try
            {
              using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].table_info([{1}])", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
              {
                using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader())
                {
                  while (sqLiteDataReader2.Read())
                  {
                    if (sqLiteDataReader2.GetInt32(5) != 0)
                    {
                      intList.Add(sqLiteDataReader2.GetInt32(0));
                      if (string.Compare(sqLiteDataReader2.GetString(2), "INTEGER", StringComparison.OrdinalIgnoreCase) == 0)
                        flag = true;
                    }
                  }
                }
              }
            }
            catch (SQLiteException ex)
            {
            }
            if (intList.Count == 1)
            {
              if (flag)
              {
                DataRow row = dataTable.NewRow();
                row["TABLE_CATALOG"] = (object) strCatalog;
                row["TABLE_NAME"] = (object) sqLiteDataReader1.GetString(2);
                row["INDEX_CATALOG"] = (object) strCatalog;
                row["PRIMARY_KEY"] = (object) true;
                row["INDEX_NAME"] = (object) HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "{1}_PK_{0}", (object) sqLiteDataReader1.GetString(2), (object) masterTableName);
                row["UNIQUE"] = (object) true;
                if (string.Compare((string) row["INDEX_NAME"], strIndex, StringComparison.OrdinalIgnoreCase) == 0 || strIndex == null)
                  dataTable.Rows.Add(row);
                intList.Clear();
              }
            }
            try
            {
              using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].index_list([{1}])", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
              {
                using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader())
                {
                  while (sqLiteDataReader2.Read())
                  {
                    if (string.Compare(sqLiteDataReader2.GetString(1), strIndex, StringComparison.OrdinalIgnoreCase) == 0 || strIndex == null)
                    {
                      DataRow row = dataTable.NewRow();
                      row["TABLE_CATALOG"] = (object) strCatalog;
                      row["TABLE_NAME"] = (object) sqLiteDataReader1.GetString(2);
                      row["INDEX_CATALOG"] = (object) strCatalog;
                      row["INDEX_NAME"] = (object) sqLiteDataReader2.GetString(1);
                      row["UNIQUE"] = (object) SQLiteConvert.ToBoolean(sqLiteDataReader2.GetValue(2), (IFormatProvider) CultureInfo.InvariantCulture, false);
                      row["PRIMARY_KEY"] = (object) false;
                      using (SQLiteCommand sqLiteCommand3 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{2}] WHERE [type] LIKE 'index' AND [name] LIKE '{1}'", (object) strCatalog, (object) sqLiteDataReader2.GetString(1).Replace("'", "''"), (object) masterTableName), this))
                      {
                        using (SQLiteDataReader sqLiteDataReader3 = sqLiteCommand3.ExecuteReader())
                        {
                          if (sqLiteDataReader3.Read())
                          {
                            if (!sqLiteDataReader3.IsDBNull(4))
                              row["INDEX_DEFINITION"] = (object) sqLiteDataReader3.GetString(4);
                          }
                        }
                      }
                      if (intList.Count > 0 && sqLiteDataReader2.GetString(1).StartsWith("sqlite_autoindex_" + sqLiteDataReader1.GetString(2), StringComparison.InvariantCultureIgnoreCase))
                      {
                        using (SQLiteCommand sqLiteCommand3 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].index_info([{1}])", (object) strCatalog, (object) sqLiteDataReader2.GetString(1)), this))
                        {
                          using (SQLiteDataReader sqLiteDataReader3 = sqLiteCommand3.ExecuteReader())
                          {
                            int num = 0;
                            while (sqLiteDataReader3.Read())
                            {
                              if (!intList.Contains(sqLiteDataReader3.GetInt32(1)))
                              {
                                num = 0;
                                break;
                              }
                              ++num;
                            }
                            if (num == intList.Count)
                            {
                              row["PRIMARY_KEY"] = (object) true;
                              intList.Clear();
                            }
                          }
                        }
                      }
                      dataTable.Rows.Add(row);
                    }
                  }
                }
              }
            }
            catch (SQLiteException ex)
            {
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_Triggers(string catalog, string table, string triggerName)
    {
      DataTable dataTable = new DataTable("Triggers");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("TRIGGER_NAME", typeof (string));
      dataTable.Columns.Add("TRIGGER_DEFINITION", typeof (string));
      dataTable.BeginLoadData();
      if (string.IsNullOrEmpty(table))
        table = (string) null;
      if (string.IsNullOrEmpty(catalog))
        catalog = SQLiteConnection.GetDefaultCatalogName();
      string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(catalog));
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT [type], [name], [tbl_name], [rootpage], [sql], [rowid] FROM [{0}].[{1}] WHERE [type] LIKE 'trigger'", (object) catalog, (object) masterTableName), this))
      {
        using (SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader())
        {
          while (sqLiteDataReader.Read())
          {
            if ((string.Compare(sqLiteDataReader.GetString(1), triggerName, StringComparison.OrdinalIgnoreCase) == 0 || triggerName == null) && (table == null || string.Compare(table, sqLiteDataReader.GetString(2), StringComparison.OrdinalIgnoreCase) == 0))
            {
              DataRow row = dataTable.NewRow();
              row["TABLE_CATALOG"] = (object) catalog;
              row["TABLE_NAME"] = (object) sqLiteDataReader.GetString(2);
              row["TRIGGER_NAME"] = (object) sqLiteDataReader.GetString(1);
              row["TRIGGER_DEFINITION"] = (object) sqLiteDataReader.GetString(4);
              dataTable.Rows.Add(row);
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    /// <summary>
    /// Retrieves table schema information for the database and catalog
    /// </summary>
    /// <param name="strCatalog">The catalog (attached database) to retrieve tables on</param>
    /// <param name="strTable">The table to retrieve, can be null</param>
    /// <param name="strType">The table type, can be null</param>
    /// <returns>DataTable</returns>
    private DataTable Schema_Tables(string strCatalog, string strTable, string strType)
    {
      DataTable dataTable = new DataTable("Tables");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("TABLE_TYPE", typeof (string));
      dataTable.Columns.Add("TABLE_ID", typeof (long));
      dataTable.Columns.Add("TABLE_ROOTPAGE", typeof (int));
      dataTable.Columns.Add("TABLE_DEFINITION", typeof (string));
      dataTable.BeginLoadData();
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = SQLiteConnection.GetDefaultCatalogName();
      string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT [type], [name], [tbl_name], [rootpage], [sql], [rowid] FROM [{0}].[{1}] WHERE [type] LIKE 'table'", (object) strCatalog, (object) masterTableName), this))
      {
        using (SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader())
        {
          while (sqLiteDataReader.Read())
          {
            string strB = sqLiteDataReader.GetString(0);
            if (string.Compare(sqLiteDataReader.GetString(2), 0, "SQLITE_", 0, 7, StringComparison.OrdinalIgnoreCase) == 0)
              strB = "SYSTEM_TABLE";
            if ((string.Compare(strType, strB, StringComparison.OrdinalIgnoreCase) == 0 || strType == null) && (string.Compare(sqLiteDataReader.GetString(2), strTable, StringComparison.OrdinalIgnoreCase) == 0 || strTable == null))
            {
              DataRow row = dataTable.NewRow();
              row["TABLE_CATALOG"] = (object) strCatalog;
              row["TABLE_NAME"] = (object) sqLiteDataReader.GetString(2);
              row["TABLE_TYPE"] = (object) strB;
              row["TABLE_ID"] = (object) sqLiteDataReader.GetInt64(5);
              row["TABLE_ROOTPAGE"] = (object) sqLiteDataReader.GetInt32(3);
              row["TABLE_DEFINITION"] = (object) sqLiteDataReader.GetString(4);
              dataTable.Rows.Add(row);
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    /// <summary>Retrieves view schema information for the database</summary>
    /// <param name="strCatalog">The catalog (attached database) to retrieve views on</param>
    /// <param name="strView">The view name, can be null</param>
    /// <returns>DataTable</returns>
    private DataTable Schema_Views(string strCatalog, string strView)
    {
      DataTable dataTable = new DataTable("Views");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("VIEW_DEFINITION", typeof (string));
      dataTable.Columns.Add("CHECK_OPTION", typeof (bool));
      dataTable.Columns.Add("IS_UPDATABLE", typeof (bool));
      dataTable.Columns.Add("DESCRIPTION", typeof (string));
      dataTable.Columns.Add("DATE_CREATED", typeof (DateTime));
      dataTable.Columns.Add("DATE_MODIFIED", typeof (DateTime));
      dataTable.BeginLoadData();
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = SQLiteConnection.GetDefaultCatalogName();
      string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'view'", (object) strCatalog, (object) masterTableName), this))
      {
        using (SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader())
        {
          while (sqLiteDataReader.Read())
          {
            if (string.Compare(sqLiteDataReader.GetString(1), strView, StringComparison.OrdinalIgnoreCase) == 0 || string.IsNullOrEmpty(strView))
            {
              string source = sqLiteDataReader.GetString(4).Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');
              int num = CultureInfo.InvariantCulture.CompareInfo.IndexOf(source, " AS ", CompareOptions.IgnoreCase);
              if (num > -1)
              {
                string str = source.Substring(num + 4).Trim();
                DataRow row = dataTable.NewRow();
                row["TABLE_CATALOG"] = (object) strCatalog;
                row["TABLE_NAME"] = (object) sqLiteDataReader.GetString(2);
                row["IS_UPDATABLE"] = (object) false;
                row["VIEW_DEFINITION"] = (object) str;
                dataTable.Rows.Add(row);
              }
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    /// <summary>
    /// Retrieves catalog (attached databases) schema information for the database
    /// </summary>
    /// <param name="strCatalog">The catalog to retrieve, can be null</param>
    /// <returns>DataTable</returns>
    private DataTable Schema_Catalogs(string strCatalog)
    {
      DataTable dataTable = new DataTable("Catalogs");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("CATALOG_NAME", typeof (string));
      dataTable.Columns.Add("DESCRIPTION", typeof (string));
      dataTable.Columns.Add("ID", typeof (long));
      dataTable.BeginLoadData();
      using (SQLiteCommand sqLiteCommand = new SQLiteCommand("PRAGMA database_list", this))
      {
        using (SQLiteDataReader sqLiteDataReader = sqLiteCommand.ExecuteReader())
        {
          while (sqLiteDataReader.Read())
          {
            if (string.Compare(sqLiteDataReader.GetString(1), strCatalog, StringComparison.OrdinalIgnoreCase) == 0 || strCatalog == null)
            {
              DataRow row = dataTable.NewRow();
              row["CATALOG_NAME"] = (object) sqLiteDataReader.GetString(1);
              row["DESCRIPTION"] = (object) sqLiteDataReader.GetString(2);
              row["ID"] = (object) sqLiteDataReader.GetInt64(0);
              dataTable.Rows.Add(row);
            }
          }
        }
      }
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    private DataTable Schema_DataTypes()
    {
      DataTable dataTable = new DataTable("DataTypes");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("TypeName", typeof (string));
      dataTable.Columns.Add("ProviderDbType", typeof (int));
      dataTable.Columns.Add("ColumnSize", typeof (long));
      dataTable.Columns.Add("CreateFormat", typeof (string));
      dataTable.Columns.Add("CreateParameters", typeof (string));
      dataTable.Columns.Add("DataType", typeof (string));
      dataTable.Columns.Add("IsAutoIncrementable", typeof (bool));
      dataTable.Columns.Add("IsBestMatch", typeof (bool));
      dataTable.Columns.Add("IsCaseSensitive", typeof (bool));
      dataTable.Columns.Add("IsFixedLength", typeof (bool));
      dataTable.Columns.Add("IsFixedPrecisionScale", typeof (bool));
      dataTable.Columns.Add("IsLong", typeof (bool));
      dataTable.Columns.Add("IsNullable", typeof (bool));
      dataTable.Columns.Add("IsSearchable", typeof (bool));
      dataTable.Columns.Add("IsSearchableWithLike", typeof (bool));
      dataTable.Columns.Add("IsLiteralSupported", typeof (bool));
      dataTable.Columns.Add("LiteralPrefix", typeof (string));
      dataTable.Columns.Add("LiteralSuffix", typeof (string));
      dataTable.Columns.Add("IsUnsigned", typeof (bool));
      dataTable.Columns.Add("MaximumScale", typeof (short));
      dataTable.Columns.Add("MinimumScale", typeof (short));
      dataTable.Columns.Add("IsConcurrencyType", typeof (bool));
      dataTable.BeginLoadData();
      StringReader stringReader = new StringReader(SR.DataTypes);
      int num = (int) dataTable.ReadXml((TextReader) stringReader);
      stringReader.Close();
      dataTable.AcceptChanges();
      dataTable.EndLoadData();
      return dataTable;
    }

    /// <summary>
    /// Returns the base column information for indexes in a database
    /// </summary>
    /// <param name="strCatalog">The catalog to retrieve indexes for (can be null)</param>
    /// <param name="strTable">The table to restrict index information by (can be null)</param>
    /// <param name="strIndex">The index to restrict index information by (can be null)</param>
    /// <param name="strColumn">The source column to restrict index information by (can be null)</param>
    /// <returns>A DataTable containing the results</returns>
    private DataTable Schema_IndexColumns(
      string strCatalog,
      string strTable,
      string strIndex,
      string strColumn)
    {
      DataTable dataTable = new DataTable("IndexColumns");
      List<KeyValuePair<int, string>> keyValuePairList = new List<KeyValuePair<int, string>>();
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("CONSTRAINT_CATALOG", typeof (string));
      dataTable.Columns.Add("CONSTRAINT_SCHEMA", typeof (string));
      dataTable.Columns.Add("CONSTRAINT_NAME", typeof (string));
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("COLUMN_NAME", typeof (string));
      dataTable.Columns.Add("ORDINAL_POSITION", typeof (int));
      dataTable.Columns.Add("INDEX_NAME", typeof (string));
      dataTable.Columns.Add("COLLATION_NAME", typeof (string));
      dataTable.Columns.Add("SORT_MODE", typeof (string));
      dataTable.Columns.Add("CONFLICT_OPTION", typeof (int));
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = SQLiteConnection.GetDefaultCatalogName();
      string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
      dataTable.BeginLoadData();
      using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table'", (object) strCatalog, (object) masterTableName), this))
      {
        using (SQLiteDataReader sqLiteDataReader1 = sqLiteCommand1.ExecuteReader())
        {
          while (sqLiteDataReader1.Read())
          {
            bool flag = false;
            keyValuePairList.Clear();
            if (!string.IsNullOrEmpty(strTable))
            {
              if (string.Compare(sqLiteDataReader1.GetString(2), strTable, StringComparison.OrdinalIgnoreCase) != 0)
                continue;
            }
            try
            {
              using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].table_info([{1}])", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
              {
                using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader())
                {
                  while (sqLiteDataReader2.Read())
                  {
                    if (sqLiteDataReader2.GetInt32(5) == 1)
                    {
                      keyValuePairList.Add(new KeyValuePair<int, string>(sqLiteDataReader2.GetInt32(0), sqLiteDataReader2.GetString(1)));
                      if (string.Compare(sqLiteDataReader2.GetString(2), "INTEGER", StringComparison.OrdinalIgnoreCase) == 0)
                        flag = true;
                    }
                  }
                }
              }
            }
            catch (SQLiteException ex)
            {
            }
            if (keyValuePairList.Count == 1 && flag)
            {
              DataRow row = dataTable.NewRow();
              row["CONSTRAINT_CATALOG"] = (object) strCatalog;
              row["CONSTRAINT_NAME"] = (object) HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "{1}_PK_{0}", (object) sqLiteDataReader1.GetString(2), (object) masterTableName);
              row["TABLE_CATALOG"] = (object) strCatalog;
              row["TABLE_NAME"] = (object) sqLiteDataReader1.GetString(2);
              row["COLUMN_NAME"] = (object) keyValuePairList[0].Value;
              row["INDEX_NAME"] = row["CONSTRAINT_NAME"];
              row["ORDINAL_POSITION"] = (object) 0;
              row["COLLATION_NAME"] = (object) "BINARY";
              row["SORT_MODE"] = (object) "ASC";
              row["CONFLICT_OPTION"] = (object) 2;
              if (string.IsNullOrEmpty(strIndex) || string.Compare(strIndex, (string) row["INDEX_NAME"], StringComparison.OrdinalIgnoreCase) == 0)
                dataTable.Rows.Add(row);
            }
            using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{2}] WHERE [type] LIKE 'index' AND [tbl_name] LIKE '{1}'", (object) strCatalog, (object) sqLiteDataReader1.GetString(2).Replace("'", "''"), (object) masterTableName), this))
            {
              using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader())
              {
                while (sqLiteDataReader2.Read())
                {
                  int num = 0;
                  if (!string.IsNullOrEmpty(strIndex))
                  {
                    if (string.Compare(strIndex, sqLiteDataReader2.GetString(1), StringComparison.OrdinalIgnoreCase) != 0)
                      continue;
                  }
                  try
                  {
                    using (SQLiteCommand sqLiteCommand3 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].index_info([{1}])", (object) strCatalog, (object) sqLiteDataReader2.GetString(1)), this))
                    {
                      using (SQLiteDataReader sqLiteDataReader3 = sqLiteCommand3.ExecuteReader())
                      {
                        while (sqLiteDataReader3.Read())
                        {
                          string str = sqLiteDataReader3.IsDBNull(2) ? (string) null : sqLiteDataReader3.GetString(2);
                          DataRow row = dataTable.NewRow();
                          row["CONSTRAINT_CATALOG"] = (object) strCatalog;
                          row["CONSTRAINT_NAME"] = (object) sqLiteDataReader2.GetString(1);
                          row["TABLE_CATALOG"] = (object) strCatalog;
                          row["TABLE_NAME"] = (object) sqLiteDataReader2.GetString(2);
                          row["COLUMN_NAME"] = (object) str;
                          row["INDEX_NAME"] = (object) sqLiteDataReader2.GetString(1);
                          row["ORDINAL_POSITION"] = (object) num;
                          string collationSequence = (string) null;
                          int sortMode = 0;
                          int onError = 0;
                          if (str != null)
                            this._sql.GetIndexColumnExtendedInfo(strCatalog, sqLiteDataReader2.GetString(1), str, ref sortMode, ref onError, ref collationSequence);
                          if (!string.IsNullOrEmpty(collationSequence))
                            row["COLLATION_NAME"] = (object) collationSequence;
                          row["SORT_MODE"] = sortMode == 0 ? (object) "ASC" : (object) "DESC";
                          row["CONFLICT_OPTION"] = (object) onError;
                          ++num;
                          if (strColumn == null || string.Compare(strColumn, str, StringComparison.OrdinalIgnoreCase) == 0)
                            dataTable.Rows.Add(row);
                        }
                      }
                    }
                  }
                  catch (SQLiteException ex)
                  {
                  }
                }
              }
            }
          }
        }
      }
      dataTable.EndLoadData();
      dataTable.AcceptChanges();
      return dataTable;
    }

    /// <summary>
    /// Returns detailed column information for a specified view
    /// </summary>
    /// <param name="strCatalog">The catalog to retrieve columns for (can be null)</param>
    /// <param name="strView">The view to restrict column information by (can be null)</param>
    /// <param name="strColumn">The source column to restrict column information by (can be null)</param>
    /// <returns>A DataTable containing the results</returns>
    private DataTable Schema_ViewColumns(
      string strCatalog,
      string strView,
      string strColumn)
    {
      DataTable dataTable = new DataTable("ViewColumns");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("VIEW_CATALOG", typeof (string));
      dataTable.Columns.Add("VIEW_SCHEMA", typeof (string));
      dataTable.Columns.Add("VIEW_NAME", typeof (string));
      dataTable.Columns.Add("VIEW_COLUMN_NAME", typeof (string));
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("COLUMN_NAME", typeof (string));
      dataTable.Columns.Add("ORDINAL_POSITION", typeof (int));
      dataTable.Columns.Add("COLUMN_HASDEFAULT", typeof (bool));
      dataTable.Columns.Add("COLUMN_DEFAULT", typeof (string));
      dataTable.Columns.Add("COLUMN_FLAGS", typeof (long));
      dataTable.Columns.Add("IS_NULLABLE", typeof (bool));
      dataTable.Columns.Add("DATA_TYPE", typeof (string));
      dataTable.Columns.Add("CHARACTER_MAXIMUM_LENGTH", typeof (int));
      dataTable.Columns.Add("NUMERIC_PRECISION", typeof (int));
      dataTable.Columns.Add("NUMERIC_SCALE", typeof (int));
      dataTable.Columns.Add("DATETIME_PRECISION", typeof (long));
      dataTable.Columns.Add("CHARACTER_SET_CATALOG", typeof (string));
      dataTable.Columns.Add("CHARACTER_SET_SCHEMA", typeof (string));
      dataTable.Columns.Add("CHARACTER_SET_NAME", typeof (string));
      dataTable.Columns.Add("COLLATION_CATALOG", typeof (string));
      dataTable.Columns.Add("COLLATION_SCHEMA", typeof (string));
      dataTable.Columns.Add("COLLATION_NAME", typeof (string));
      dataTable.Columns.Add("PRIMARY_KEY", typeof (bool));
      dataTable.Columns.Add("EDM_TYPE", typeof (string));
      dataTable.Columns.Add("AUTOINCREMENT", typeof (bool));
      dataTable.Columns.Add("UNIQUE", typeof (bool));
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = SQLiteConnection.GetDefaultCatalogName();
      string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
      dataTable.BeginLoadData();
      using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'view'", (object) strCatalog, (object) masterTableName), this))
      {
        using (SQLiteDataReader sqLiteDataReader1 = sqLiteCommand1.ExecuteReader())
        {
          while (sqLiteDataReader1.Read())
          {
            if (string.IsNullOrEmpty(strView) || string.Compare(strView, sqLiteDataReader1.GetString(2), StringComparison.OrdinalIgnoreCase) == 0)
            {
              using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}]", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
              {
                string source = sqLiteDataReader1.GetString(4).Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');
                int num = CultureInfo.InvariantCulture.CompareInfo.IndexOf(source, " AS ", CompareOptions.IgnoreCase);
                if (num >= 0)
                {
                  using (SQLiteCommand sqLiteCommand3 = new SQLiteCommand(source.Substring(num + 4), this))
                  {
                    using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                      using (SQLiteDataReader sqLiteDataReader3 = sqLiteCommand3.ExecuteReader(CommandBehavior.SchemaOnly))
                      {
                        using (DataTable schemaTable1 = sqLiteDataReader2.GetSchemaTable(false, false))
                        {
                          using (DataTable schemaTable2 = sqLiteDataReader3.GetSchemaTable(false, false))
                          {
                            for (int index = 0; index < schemaTable2.Rows.Count; ++index)
                            {
                              DataRow row1 = schemaTable1.Rows[index];
                              DataRow row2 = schemaTable2.Rows[index];
                              if (string.Compare(row1[SchemaTableColumn.ColumnName].ToString(), strColumn, StringComparison.OrdinalIgnoreCase) == 0 || strColumn == null)
                              {
                                DataRow row3 = dataTable.NewRow();
                                row3["VIEW_CATALOG"] = (object) strCatalog;
                                row3["VIEW_NAME"] = (object) sqLiteDataReader1.GetString(2);
                                row3["TABLE_CATALOG"] = (object) strCatalog;
                                row3["TABLE_SCHEMA"] = row2[SchemaTableColumn.BaseSchemaName];
                                row3["TABLE_NAME"] = row2[SchemaTableColumn.BaseTableName];
                                row3["COLUMN_NAME"] = row2[SchemaTableColumn.BaseColumnName];
                                row3["VIEW_COLUMN_NAME"] = row1[SchemaTableColumn.ColumnName];
                                row3["COLUMN_HASDEFAULT"] = (object) (row1[SchemaTableOptionalColumn.DefaultValue] != DBNull.Value);
                                row3["COLUMN_DEFAULT"] = row1[SchemaTableOptionalColumn.DefaultValue];
                                row3["ORDINAL_POSITION"] = row1[SchemaTableColumn.ColumnOrdinal];
                                row3["IS_NULLABLE"] = row1[SchemaTableColumn.AllowDBNull];
                                row3["DATA_TYPE"] = row1["DataTypeName"];
                                row3["EDM_TYPE"] = (object) SQLiteConvert.DbTypeToTypeName(this, (DbType) row1[SchemaTableColumn.ProviderType], this._flags).ToString().ToLower(CultureInfo.InvariantCulture);
                                row3["CHARACTER_MAXIMUM_LENGTH"] = row1[SchemaTableColumn.ColumnSize];
                                row3["TABLE_SCHEMA"] = row1[SchemaTableColumn.BaseSchemaName];
                                row3["PRIMARY_KEY"] = row1[SchemaTableColumn.IsKey];
                                row3["AUTOINCREMENT"] = row1[SchemaTableOptionalColumn.IsAutoIncrement];
                                row3["COLLATION_NAME"] = row1["CollationType"];
                                row3["UNIQUE"] = row1[SchemaTableColumn.IsUnique];
                                dataTable.Rows.Add(row3);
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      dataTable.EndLoadData();
      dataTable.AcceptChanges();
      return dataTable;
    }

    /// <summary>
    /// Retrieves foreign key information from the specified set of filters
    /// </summary>
    /// <param name="strCatalog">An optional catalog to restrict results on</param>
    /// <param name="strTable">An optional table to restrict results on</param>
    /// <param name="strKeyName">An optional foreign key name to restrict results on</param>
    /// <returns>A DataTable with the results of the query</returns>
    private DataTable Schema_ForeignKeys(
      string strCatalog,
      string strTable,
      string strKeyName)
    {
      DataTable dataTable = new DataTable("ForeignKeys");
      dataTable.Locale = CultureInfo.InvariantCulture;
      dataTable.Columns.Add("CONSTRAINT_CATALOG", typeof (string));
      dataTable.Columns.Add("CONSTRAINT_SCHEMA", typeof (string));
      dataTable.Columns.Add("CONSTRAINT_NAME", typeof (string));
      dataTable.Columns.Add("TABLE_CATALOG", typeof (string));
      dataTable.Columns.Add("TABLE_SCHEMA", typeof (string));
      dataTable.Columns.Add("TABLE_NAME", typeof (string));
      dataTable.Columns.Add("CONSTRAINT_TYPE", typeof (string));
      dataTable.Columns.Add("IS_DEFERRABLE", typeof (bool));
      dataTable.Columns.Add("INITIALLY_DEFERRED", typeof (bool));
      dataTable.Columns.Add("FKEY_ID", typeof (int));
      dataTable.Columns.Add("FKEY_FROM_COLUMN", typeof (string));
      dataTable.Columns.Add("FKEY_FROM_ORDINAL_POSITION", typeof (int));
      dataTable.Columns.Add("FKEY_TO_CATALOG", typeof (string));
      dataTable.Columns.Add("FKEY_TO_SCHEMA", typeof (string));
      dataTable.Columns.Add("FKEY_TO_TABLE", typeof (string));
      dataTable.Columns.Add("FKEY_TO_COLUMN", typeof (string));
      dataTable.Columns.Add("FKEY_ON_UPDATE", typeof (string));
      dataTable.Columns.Add("FKEY_ON_DELETE", typeof (string));
      dataTable.Columns.Add("FKEY_MATCH", typeof (string));
      if (string.IsNullOrEmpty(strCatalog))
        strCatalog = SQLiteConnection.GetDefaultCatalogName();
      string masterTableName = SQLiteConnection.GetMasterTableName(SQLiteConnection.IsTemporaryCatalogName(strCatalog));
      dataTable.BeginLoadData();
      using (SQLiteCommand sqLiteCommand1 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "SELECT * FROM [{0}].[{1}] WHERE [type] LIKE 'table'", (object) strCatalog, (object) masterTableName), this))
      {
        using (SQLiteDataReader sqLiteDataReader1 = sqLiteCommand1.ExecuteReader())
        {
          while (sqLiteDataReader1.Read())
          {
            if (!string.IsNullOrEmpty(strTable))
            {
              if (string.Compare(strTable, sqLiteDataReader1.GetString(2), StringComparison.OrdinalIgnoreCase) != 0)
                continue;
            }
            try
            {
              using (SQLiteCommandBuilder liteCommandBuilder = new SQLiteCommandBuilder())
              {
                using (SQLiteCommand sqLiteCommand2 = new SQLiteCommand(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "PRAGMA [{0}].foreign_key_list([{1}])", (object) strCatalog, (object) sqLiteDataReader1.GetString(2)), this))
                {
                  using (SQLiteDataReader sqLiteDataReader2 = sqLiteCommand2.ExecuteReader())
                  {
                    while (sqLiteDataReader2.Read())
                    {
                      DataRow row = dataTable.NewRow();
                      row["CONSTRAINT_CATALOG"] = (object) strCatalog;
                      row["CONSTRAINT_NAME"] = (object) HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "FK_{0}_{1}_{2}", sqLiteDataReader1[2], (object) sqLiteDataReader2.GetInt32(0), (object) sqLiteDataReader2.GetInt32(1));
                      row["TABLE_CATALOG"] = (object) strCatalog;
                      row["TABLE_NAME"] = (object) liteCommandBuilder.UnquoteIdentifier(sqLiteDataReader1.GetString(2));
                      row["CONSTRAINT_TYPE"] = (object) "FOREIGN KEY";
                      row["IS_DEFERRABLE"] = (object) false;
                      row["INITIALLY_DEFERRED"] = (object) false;
                      row["FKEY_ID"] = sqLiteDataReader2[0];
                      row["FKEY_FROM_COLUMN"] = (object) liteCommandBuilder.UnquoteIdentifier(sqLiteDataReader2[3].ToString());
                      row["FKEY_TO_CATALOG"] = (object) strCatalog;
                      row["FKEY_TO_TABLE"] = (object) liteCommandBuilder.UnquoteIdentifier(sqLiteDataReader2[2].ToString());
                      row["FKEY_TO_COLUMN"] = (object) liteCommandBuilder.UnquoteIdentifier(sqLiteDataReader2[4].ToString());
                      row["FKEY_FROM_ORDINAL_POSITION"] = sqLiteDataReader2[1];
                      row["FKEY_ON_UPDATE"] = sqLiteDataReader2.FieldCount > 5 ? sqLiteDataReader2[5] : (object) string.Empty;
                      row["FKEY_ON_DELETE"] = sqLiteDataReader2.FieldCount > 6 ? sqLiteDataReader2[6] : (object) string.Empty;
                      row["FKEY_MATCH"] = sqLiteDataReader2.FieldCount > 7 ? sqLiteDataReader2[7] : (object) string.Empty;
                      if (string.IsNullOrEmpty(strKeyName) || string.Compare(strKeyName, row["CONSTRAINT_NAME"].ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                        dataTable.Rows.Add(row);
                    }
                  }
                }
              }
            }
            catch (SQLiteException ex)
            {
            }
          }
        }
      }
      dataTable.EndLoadData();
      dataTable.AcceptChanges();
      return dataTable;
    }

    /// <summary>
    /// This event is raised periodically during long running queries.  Changing
    /// the value of the <see cref="F:System.Data.SQLite.ProgressEventArgs.ReturnCode" /> property will
    /// determine if the operation in progress will continue or be interrupted.
    /// For the entire duration of the event, the associated connection and
    /// statement objects must not be modified, either directly or indirectly, by
    /// the called code.
    /// </summary>
    public event SQLiteProgressEventHandler Progress
    {
      add
      {
        this.CheckDisposed();
        if (this._progressHandler == null)
        {
          this._progressCallback = new SQLiteProgressCallback(this.ProgressCallback);
          if (this._sql != null)
            this._sql.SetProgressHook(this._progressOps, this._progressCallback);
        }
        this._progressHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._progressHandler -= value;
        if (this._progressHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetProgressHook(0, (SQLiteProgressCallback) null);
        this._progressCallback = (SQLiteProgressCallback) null;
      }
    }

    /// <summary>
    /// This event is raised whenever SQLite encounters an action covered by the
    /// authorizer during query preparation.  Changing the value of the
    /// <see cref="F:System.Data.SQLite.AuthorizerEventArgs.ReturnCode" /> property will determine if
    /// the specific action will be allowed, ignored, or denied.  For the entire
    /// duration of the event, the associated connection and statement objects
    /// must not be modified, either directly or indirectly, by the called code.
    /// </summary>
    public event SQLiteAuthorizerEventHandler Authorize
    {
      add
      {
        this.CheckDisposed();
        if (this._authorizerHandler == null)
        {
          this._authorizerCallback = new SQLiteAuthorizerCallback(this.AuthorizerCallback);
          if (this._sql != null)
            this._sql.SetAuthorizerHook(this._authorizerCallback);
        }
        this._authorizerHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._authorizerHandler -= value;
        if (this._authorizerHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetAuthorizerHook((SQLiteAuthorizerCallback) null);
        this._authorizerCallback = (SQLiteAuthorizerCallback) null;
      }
    }

    /// <summary>
    /// This event is raised whenever SQLite makes an update/delete/insert into the database on
    /// this connection.  It only applies to the given connection.
    /// </summary>
    public event SQLiteUpdateEventHandler Update
    {
      add
      {
        this.CheckDisposed();
        if (this._updateHandler == null)
        {
          this._updateCallback = new SQLiteUpdateCallback(this.UpdateCallback);
          if (this._sql != null)
            this._sql.SetUpdateHook(this._updateCallback);
        }
        this._updateHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._updateHandler -= value;
        if (this._updateHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetUpdateHook((SQLiteUpdateCallback) null);
        this._updateCallback = (SQLiteUpdateCallback) null;
      }
    }

    private SQLiteProgressReturnCode ProgressCallback(IntPtr pUserData)
    {
      try
      {
        ProgressEventArgs e = new ProgressEventArgs(pUserData, SQLiteProgressReturnCode.Continue);
        if (this._progressHandler != null)
          this._progressHandler((object) this, e);
        return e.ReturnCode;
      }
      catch (Exception ex)
      {
        try
        {
          if (HelperMethods.LogCallbackExceptions(this._flags))
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Progress", (object) ex));
        }
        catch
        {
        }
      }
      return HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.InterruptOnException) ? SQLiteProgressReturnCode.Interrupt : SQLiteProgressReturnCode.Continue;
    }

    private SQLiteAuthorizerReturnCode AuthorizerCallback(
      IntPtr pUserData,
      SQLiteAuthorizerActionCode actionCode,
      IntPtr pArgument1,
      IntPtr pArgument2,
      IntPtr pDatabase,
      IntPtr pAuthContext)
    {
      try
      {
        AuthorizerEventArgs e = new AuthorizerEventArgs(pUserData, actionCode, SQLiteConvert.UTF8ToString(pArgument1, -1), SQLiteConvert.UTF8ToString(pArgument2, -1), SQLiteConvert.UTF8ToString(pDatabase, -1), SQLiteConvert.UTF8ToString(pAuthContext, -1), SQLiteAuthorizerReturnCode.Ok);
        if (this._authorizerHandler != null)
          this._authorizerHandler((object) this, e);
        return e.ReturnCode;
      }
      catch (Exception ex)
      {
        try
        {
          if (HelperMethods.LogCallbackExceptions(this._flags))
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Authorize", (object) ex));
        }
        catch
        {
        }
      }
      return HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.DenyOnException) ? SQLiteAuthorizerReturnCode.Deny : SQLiteAuthorizerReturnCode.Ok;
    }

    private void UpdateCallback(
      IntPtr puser,
      int type,
      IntPtr database,
      IntPtr table,
      long rowid)
    {
      try
      {
        this._updateHandler((object) this, new UpdateEventArgs(SQLiteConvert.UTF8ToString(database, -1), SQLiteConvert.UTF8ToString(table, -1), (UpdateEventType) type, rowid));
      }
      catch (Exception ex)
      {
        try
        {
          if (!HelperMethods.LogCallbackExceptions(this._flags))
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Update", (object) ex));
        }
        catch
        {
        }
      }
    }

    /// <summary>
    /// This event is raised whenever SQLite is committing a transaction.
    /// Return non-zero to trigger a rollback.
    /// </summary>
    public event SQLiteCommitHandler Commit
    {
      add
      {
        this.CheckDisposed();
        if (this._commitHandler == null)
        {
          this._commitCallback = new SQLiteCommitCallback(this.CommitCallback);
          if (this._sql != null)
            this._sql.SetCommitHook(this._commitCallback);
        }
        this._commitHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._commitHandler -= value;
        if (this._commitHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetCommitHook((SQLiteCommitCallback) null);
        this._commitCallback = (SQLiteCommitCallback) null;
      }
    }

    /// <summary>
    /// This event is raised whenever SQLite statement first begins executing on
    /// this connection.  It only applies to the given connection.
    /// </summary>
    public event SQLiteTraceEventHandler Trace
    {
      add
      {
        this.CheckDisposed();
        if (this._traceHandler == null)
        {
          this._traceCallback = new SQLiteTraceCallback(this.TraceCallback);
          if (this._sql != null)
            this._sql.SetTraceCallback(this._traceCallback);
        }
        this._traceHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._traceHandler -= value;
        if (this._traceHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetTraceCallback((SQLiteTraceCallback) null);
        this._traceCallback = (SQLiteTraceCallback) null;
      }
    }

    private void TraceCallback(IntPtr puser, IntPtr statement)
    {
      try
      {
        if (this._traceHandler == null)
          return;
        this._traceHandler((object) this, new TraceEventArgs(SQLiteConvert.UTF8ToString(statement, -1)));
      }
      catch (Exception ex)
      {
        try
        {
          if (!HelperMethods.LogCallbackExceptions(this._flags))
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Trace", (object) ex));
        }
        catch
        {
        }
      }
    }

    /// <summary>
    /// This event is raised whenever SQLite is rolling back a transaction.
    /// </summary>
    public event EventHandler RollBack
    {
      add
      {
        this.CheckDisposed();
        if (this._rollbackHandler == null)
        {
          this._rollbackCallback = new SQLiteRollbackCallback(this.RollbackCallback);
          if (this._sql != null)
            this._sql.SetRollbackHook(this._rollbackCallback);
        }
        this._rollbackHandler += value;
      }
      remove
      {
        this.CheckDisposed();
        this._rollbackHandler -= value;
        if (this._rollbackHandler != null)
          return;
        if (this._sql != null)
          this._sql.SetRollbackHook((SQLiteRollbackCallback) null);
        this._rollbackCallback = (SQLiteRollbackCallback) null;
      }
    }

    private int CommitCallback(IntPtr parg)
    {
      try
      {
        CommitEventArgs e = new CommitEventArgs();
        if (this._commitHandler != null)
          this._commitHandler((object) this, e);
        return e.AbortTransaction ? 1 : 0;
      }
      catch (Exception ex)
      {
        try
        {
          if (HelperMethods.LogCallbackExceptions(this._flags))
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Commit", (object) ex));
        }
        catch
        {
        }
      }
      return HelperMethods.HasFlags(this._flags, SQLiteConnectionFlags.RollbackOnException) ? 1 : 0;
    }

    private void RollbackCallback(IntPtr parg)
    {
      try
      {
        if (this._rollbackHandler == null)
          return;
        this._rollbackHandler((object) this, EventArgs.Empty);
      }
      catch (Exception ex)
      {
        try
        {
          if (!HelperMethods.LogCallbackExceptions(this._flags))
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Rollback", (object) ex));
        }
        catch
        {
        }
      }
    }

    /// <summary>
    /// Returns the <see cref="T:System.Data.SQLite.SQLiteFactory" /> instance.
    /// </summary>
    protected override DbProviderFactory DbProviderFactory => (DbProviderFactory) SQLiteFactory.Instance;
  }
}
