// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConnectionStringBuilder
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Reflection;

namespace System.Data.SQLite
{
  /// <summary>SQLite implementation of DbConnectionStringBuilder.</summary>
  [DefaultMember("Item")]
  [DefaultProperty("DataSource")]
  public sealed class SQLiteConnectionStringBuilder : DbConnectionStringBuilder
  {
    /// <summary>Properties of this class</summary>
    private Hashtable _properties;

    /// <overloads>Constructs a new instance of the class</overloads>
    /// <summary>Default constructor</summary>
    public SQLiteConnectionStringBuilder() => this.Initialize((string) null);

    /// <summary>
    /// Constructs a new instance of the class using the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to parse</param>
    public SQLiteConnectionStringBuilder(string connectionString) => this.Initialize(connectionString);

    /// <summary>
    /// Private initializer, which assigns the connection string and resets the builder
    /// </summary>
    /// <param name="cnnString">The connection string to assign</param>
    private void Initialize(string cnnString)
    {
      this._properties = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
      try
      {
        this.GetProperties(this._properties);
      }
      catch (NotImplementedException ex)
      {
        this.FallbackGetProperties(this._properties);
      }
      if (string.IsNullOrEmpty(cnnString))
        return;
      this.ConnectionString = cnnString;
    }

    /// <summary>
    /// Gets/Sets the default version of the SQLite engine to instantiate.  Currently the only valid value is 3, indicating version 3 of the sqlite library.
    /// </summary>
    [DefaultValue(3)]
    [Browsable(true)]
    public int Version
    {
      get
      {
        object obj;
        this.TryGetValue("version", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["version"] = value == 3 ? (object) value : throw new NotSupportedException();
    }

    /// <summary>
    /// Gets/Sets the synchronization mode (file flushing) of the connection string.  Default is "Normal".
    /// </summary>
    [DisplayName("Synchronous")]
    [Browsable(true)]
    [DefaultValue(SynchronizationModes.Normal)]
    public SynchronizationModes SyncMode
    {
      get
      {
        object obj;
        this.TryGetValue("synchronous", out obj);
        return obj is string ? (SynchronizationModes) TypeDescriptor.GetConverter(typeof (SynchronizationModes)).ConvertFrom(obj) : (SynchronizationModes) obj;
      }
      set => this["synchronous"] = (object) value;
    }

    /// <summary>
    /// Gets/Sets the encoding for the connection string.  The default is "False" which indicates UTF-8 encoding.
    /// </summary>
    [DisplayName("Use UTF-16 Encoding")]
    [DefaultValue(false)]
    [Browsable(true)]
    public bool UseUTF16Encoding
    {
      get
      {
        object source;
        this.TryGetValue("useutf16encoding", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["useutf16encoding"] = (object) value;
    }

    /// <summary>
    /// Gets/Sets whether or not to use connection pooling.  The default is "False"
    /// </summary>
    [Browsable(true)]
    [DefaultValue(false)]
    public bool Pooling
    {
      get
      {
        object source;
        this.TryGetValue("pooling", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["pooling"] = (object) value;
    }

    /// <summary>
    /// Gets/Sets whethor not to store GUID's in binary format.  The default is True
    /// which saves space in the database.
    /// </summary>
    [Browsable(true)]
    [DisplayName("Binary GUID")]
    [DefaultValue(true)]
    public bool BinaryGUID
    {
      get
      {
        object source;
        this.TryGetValue("binaryguid", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["binaryguid"] = (object) value;
    }

    /// <summary>
    /// Gets/Sets the filename to open on the connection string.
    /// </summary>
    [Browsable(true)]
    [DisplayName("Data Source")]
    [DefaultValue("")]
    public string DataSource
    {
      get
      {
        object obj;
        this.TryGetValue("data source", out obj);
        return obj?.ToString();
      }
      set => this["data source"] = (object) value;
    }

    /// <summary>An alternate to the data source property</summary>
    [DisplayName("URI")]
    [Browsable(true)]
    [DefaultValue(null)]
    public string Uri
    {
      get
      {
        object obj;
        this.TryGetValue("uri", out obj);
        return obj?.ToString();
      }
      set => this["uri"] = (object) value;
    }

    /// <summary>
    /// An alternate to the data source property that uses the SQLite URI syntax.
    /// </summary>
    [DisplayName("Full URI")]
    [DefaultValue(null)]
    [Browsable(true)]
    public string FullUri
    {
      get
      {
        object obj;
        this.TryGetValue("fulluri", out obj);
        return obj?.ToString();
      }
      set => this["fulluri"] = (object) value;
    }

    /// <summary>
    /// Gets/sets the default command timeout for newly-created commands.  This is especially useful for
    /// commands used internally such as inside a SQLiteTransaction, where setting the timeout is not possible.
    /// </summary>
    [DisplayName("Default Timeout")]
    [DefaultValue(30)]
    [Browsable(true)]
    public int DefaultTimeout
    {
      get
      {
        object obj;
        this.TryGetValue("default timeout", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["default timeout"] = (object) value;
    }

    /// <summary>
    /// Gets/sets the busy timeout to use with the SQLite core library.
    /// </summary>
    [DefaultValue(0)]
    [DisplayName("Busy Timeout")]
    [Browsable(true)]
    public int BusyTimeout
    {
      get
      {
        object obj;
        this.TryGetValue("busytimeout", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["busytimeout"] = (object) value;
    }

    /// <summary>
    /// <b>EXPERIMENTAL</b> --
    /// The wait timeout to use with
    /// <see cref="M:System.Data.SQLite.SQLiteConnection.WaitForEnlistmentReset(System.Int32,System.Nullable{System.Boolean})" /> method.
    /// This is only used when waiting for the enlistment to be reset
    /// prior to enlisting in a transaction, and then only when the
    /// appropriate connection flag is set.
    /// </summary>
    [DefaultValue(30000)]
    [Browsable(true)]
    [DisplayName("Wait Timeout")]
    public int WaitTimeout
    {
      get
      {
        object obj;
        this.TryGetValue("waittimeout", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["waittimeout"] = (object) value;
    }

    /// <summary>
    /// Gets/sets the maximum number of retries when preparing SQL to be executed.
    /// This normally only applies to preparation errors resulting from the database
    /// schema being changed.
    /// </summary>
    [DisplayName("Prepare Retries")]
    [Browsable(true)]
    [DefaultValue(3)]
    public int PrepareRetries
    {
      get
      {
        object obj;
        this.TryGetValue("prepareretries", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["prepareretries"] = (object) value;
    }

    /// <summary>
    /// Gets/sets the approximate number of virtual machine instructions between
    /// progress events.  In order for progress events to actually fire, the event
    /// handler must be added to the <see cref="E:System.Data.SQLite.SQLiteConnection.Progress" /> event
    /// as well.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(0)]
    [DisplayName("Progress Ops")]
    public int ProgressOps
    {
      get
      {
        object obj;
        this.TryGetValue("progressops", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["progressops"] = (object) value;
    }

    /// <summary>
    /// Determines whether or not the connection will automatically participate
    /// in the current distributed transaction (if one exists)
    /// </summary>
    [Browsable(true)]
    [DefaultValue(true)]
    public bool Enlist
    {
      get
      {
        object source;
        this.TryGetValue("enlist", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["enlist"] = (object) value;
    }

    /// <summary>
    /// If set to true, will throw an exception if the database specified in the connection
    /// string does not exist.  If false, the database will be created automatically.
    /// </summary>
    [DisplayName("Fail If Missing")]
    [Browsable(true)]
    [DefaultValue(false)]
    public bool FailIfMissing
    {
      get
      {
        object source;
        this.TryGetValue("failifmissing", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["failifmissing"] = (object) value;
    }

    /// <summary>
    /// If enabled, uses the legacy 3.xx format for maximum compatibility, but results in larger
    /// database sizes.
    /// </summary>
    [DisplayName("Legacy Format")]
    [Browsable(true)]
    [DefaultValue(false)]
    public bool LegacyFormat
    {
      get
      {
        object source;
        this.TryGetValue("legacy format", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["legacy format"] = (object) value;
    }

    /// <summary>
    /// When enabled, the database will be opened for read-only access and writing will be disabled.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(false)]
    [DisplayName("Read Only")]
    public bool ReadOnly
    {
      get
      {
        object source;
        this.TryGetValue("read only", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["read only"] = (object) value;
    }

    /// <summary>Gets/sets the database encryption password</summary>
    [PasswordPropertyText(true)]
    [Browsable(true)]
    [DefaultValue("")]
    public string Password
    {
      get
      {
        object obj;
        this.TryGetValue("password", out obj);
        return obj?.ToString();
      }
      set => this["password"] = (object) value;
    }

    /// <summary>
    /// Gets/sets the database encryption hexadecimal password
    /// </summary>
    [DefaultValue(null)]
    [PasswordPropertyText(true)]
    [Browsable(true)]
    [DisplayName("Hexadecimal Password")]
    public byte[] HexPassword
    {
      get
      {
        object obj;
        if (this.TryGetValue("hexpassword", out obj))
        {
          if (obj is string)
            return SQLiteConnection.FromHexString((string) obj);
          if (obj != null)
            return (byte[]) obj;
        }
        return (byte[]) null;
      }
      set => this["hexpassword"] = (object) SQLiteConnection.ToHexString(value);
    }

    /// <summary>Gets/Sets the page size for the connection.</summary>
    [Browsable(true)]
    [DisplayName("Page Size")]
    [DefaultValue(4096)]
    public int PageSize
    {
      get
      {
        object obj;
        this.TryGetValue("page size", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["page size"] = (object) value;
    }

    /// <summary>
    /// Gets/Sets the maximum number of pages the database may hold
    /// </summary>
    [Browsable(true)]
    [DefaultValue(0)]
    [DisplayName("Maximum Page Count")]
    public int MaxPageCount
    {
      get
      {
        object obj;
        this.TryGetValue("max page count", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["max page count"] = (object) value;
    }

    /// <summary>Gets/Sets the cache size for the connection.</summary>
    [Browsable(true)]
    [DefaultValue(-2000)]
    [DisplayName("Cache Size")]
    public int CacheSize
    {
      get
      {
        object obj;
        this.TryGetValue("cache size", out obj);
        return Convert.ToInt32(obj, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      set => this["cache size"] = (object) value;
    }

    /// <summary>Gets/Sets the DateTime format for the connection.</summary>
    [DisplayName("DateTime Format")]
    [Browsable(true)]
    [DefaultValue(SQLiteDateFormats.ISO8601)]
    public SQLiteDateFormats DateTimeFormat
    {
      get
      {
        object obj;
        if (this.TryGetValue("datetimeformat", out obj))
        {
          if (obj is SQLiteDateFormats sqLiteDateFormats2)
            return sqLiteDateFormats2;
          if (obj != null)
            return (SQLiteDateFormats) TypeDescriptor.GetConverter(typeof (SQLiteDateFormats)).ConvertFrom(obj);
        }
        return SQLiteDateFormats.ISO8601;
      }
      set => this["datetimeformat"] = (object) value;
    }

    /// <summary>Gets/Sets the DateTime kind for the connection.</summary>
    [DisplayName("DateTime Kind")]
    [Browsable(true)]
    [DefaultValue(DateTimeKind.Unspecified)]
    public DateTimeKind DateTimeKind
    {
      get
      {
        object obj;
        if (this.TryGetValue("datetimekind", out obj))
        {
          if (obj is DateTimeKind dateTimeKind2)
            return dateTimeKind2;
          if (obj != null)
            return (DateTimeKind) TypeDescriptor.GetConverter(typeof (DateTimeKind)).ConvertFrom(obj);
        }
        return DateTimeKind.Unspecified;
      }
      set => this["datetimekind"] = (object) value;
    }

    /// <summary>
    /// Gets/sets the DateTime format string used for formatting
    /// and parsing purposes.
    /// </summary>
    [DisplayName("DateTime Format String")]
    [DefaultValue(null)]
    [Browsable(true)]
    public string DateTimeFormatString
    {
      get
      {
        object obj;
        if (this.TryGetValue("datetimeformatstring", out obj))
        {
          if (obj is string)
            return (string) obj;
          if (obj != null)
            return obj.ToString();
        }
        return (string) null;
      }
      set => this["datetimeformatstring"] = (object) value;
    }

    /// <summary>
    /// Gets/Sets the placeholder base schema name used for
    /// .NET Framework compatibility purposes.
    /// </summary>
    [DefaultValue("sqlite_default_schema")]
    [DisplayName("Base Schema Name")]
    [Browsable(true)]
    public string BaseSchemaName
    {
      get
      {
        object obj;
        if (this.TryGetValue("baseschemaname", out obj))
        {
          if (obj is string)
            return (string) obj;
          if (obj != null)
            return obj.ToString();
        }
        return (string) null;
      }
      set => this["baseschemaname"] = (object) value;
    }

    /// <summary>
    /// Determines how SQLite handles the transaction journal file.
    /// </summary>
    [DefaultValue(SQLiteJournalModeEnum.Default)]
    [DisplayName("Journal Mode")]
    [Browsable(true)]
    public SQLiteJournalModeEnum JournalMode
    {
      get
      {
        object obj;
        this.TryGetValue("journal mode", out obj);
        return obj is string ? (SQLiteJournalModeEnum) TypeDescriptor.GetConverter(typeof (SQLiteJournalModeEnum)).ConvertFrom(obj) : (SQLiteJournalModeEnum) obj;
      }
      set => this["journal mode"] = (object) value;
    }

    /// <summary>
    /// Sets the default isolation level for transactions on the connection.
    /// </summary>
    [DisplayName("Default Isolation Level")]
    [Browsable(true)]
    [DefaultValue(IsolationLevel.Serializable)]
    public IsolationLevel DefaultIsolationLevel
    {
      get
      {
        object obj;
        this.TryGetValue("default isolationlevel", out obj);
        return obj is string ? (IsolationLevel) TypeDescriptor.GetConverter(typeof (IsolationLevel)).ConvertFrom(obj) : (IsolationLevel) obj;
      }
      set => this["default isolationlevel"] = (object) value;
    }

    /// <summary>
    /// Gets/sets the default database type for the connection.
    /// </summary>
    [Browsable(true)]
    [DefaultValue(~DbType.AnsiString)]
    [DisplayName("Default Database Type")]
    public DbType DefaultDbType
    {
      get
      {
        object obj;
        if (this.TryGetValue("defaultdbtype", out obj))
        {
          if (obj is string)
            return (DbType) TypeDescriptor.GetConverter(typeof (DbType)).ConvertFrom(obj);
          if (obj != null)
            return (DbType) obj;
        }
        return ~DbType.AnsiString;
      }
      set => this["defaultdbtype"] = (object) value;
    }

    /// <summary>Gets/sets the default type name for the connection.</summary>
    [Browsable(true)]
    [DefaultValue(null)]
    [DisplayName("Default Type Name")]
    public string DefaultTypeName
    {
      get
      {
        object obj;
        this.TryGetValue("defaulttypename", out obj);
        return obj?.ToString();
      }
      set => this["defaulttypename"] = (object) value;
    }

    /// <summary>Gets/sets the VFS name for the connection.</summary>
    [DefaultValue(null)]
    [DisplayName("VFS Name")]
    [Browsable(true)]
    public string VfsName
    {
      get
      {
        object obj;
        this.TryGetValue("vfsname", out obj);
        return obj?.ToString();
      }
      set => this["vfsname"] = (object) value;
    }

    /// <summary>If enabled, use foreign key constraints</summary>
    [DefaultValue(false)]
    [DisplayName("Foreign Keys")]
    [Browsable(true)]
    public bool ForeignKeys
    {
      get
      {
        object source;
        this.TryGetValue("foreign keys", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["foreign keys"] = (object) value;
    }

    /// <summary>Enable or disable the recursive trigger capability.</summary>
    [DefaultValue(false)]
    [DisplayName("Recursive Triggers")]
    [Browsable(true)]
    public bool RecursiveTriggers
    {
      get
      {
        object source;
        this.TryGetValue("recursive triggers", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["recursive triggers"] = (object) value;
    }

    /// <summary>
    /// If non-null, this is the version of ZipVFS to use.  This requires the
    /// System.Data.SQLite interop assembly -AND- primary managed assembly to
    /// be compiled with the INTEROP_INCLUDE_ZIPVFS option; otherwise, this
    /// property does nothing.
    /// </summary>
    [DisplayName("ZipVFS Version")]
    [DefaultValue(null)]
    [Browsable(true)]
    public string ZipVfsVersion
    {
      get
      {
        object obj;
        this.TryGetValue("zipvfsversion", out obj);
        return obj?.ToString();
      }
      set => this["zipvfsversion"] = (object) value;
    }

    /// <summary>Gets/Sets the extra behavioral flags.</summary>
    [DefaultValue(SQLiteConnectionFlags.Default)]
    [Browsable(true)]
    public SQLiteConnectionFlags Flags
    {
      get
      {
        object obj;
        if (this.TryGetValue("flags", out obj))
        {
          if (obj is SQLiteConnectionFlags liteConnectionFlags2)
            return liteConnectionFlags2;
          if (obj != null)
            return (SQLiteConnectionFlags) TypeDescriptor.GetConverter(typeof (SQLiteConnectionFlags)).ConvertFrom(obj);
        }
        return SQLiteConnectionFlags.Default;
      }
      set => this["flags"] = (object) value;
    }

    /// <summary>
    /// If enabled, apply the default connection settings to opened databases.
    /// </summary>
    [DefaultValue(true)]
    [Browsable(true)]
    [DisplayName("Set Defaults")]
    public bool SetDefaults
    {
      get
      {
        object source;
        this.TryGetValue("setdefaults", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["setdefaults"] = (object) value;
    }

    /// <summary>
    /// If enabled, attempt to resolve the provided data source file name to a
    /// full path before opening.
    /// </summary>
    [DefaultValue(true)]
    [Browsable(true)]
    [DisplayName("To Full Path")]
    public bool ToFullPath
    {
      get
      {
        object source;
        this.TryGetValue("tofullpath", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["tofullpath"] = (object) value;
    }

    /// <summary>
    /// If enabled, skip using the configured default connection flags.
    /// </summary>
    [DefaultValue(false)]
    [DisplayName("No Default Flags")]
    [Browsable(true)]
    public bool NoDefaultFlags
    {
      get
      {
        object source;
        this.TryGetValue("nodefaultflags", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["nodefaultflags"] = (object) value;
    }

    /// <summary>
    /// If enabled, skip using the configured shared connection flags.
    /// </summary>
    [DefaultValue(false)]
    [DisplayName("No Shared Flags")]
    [Browsable(true)]
    public bool NoSharedFlags
    {
      get
      {
        object source;
        this.TryGetValue("nosharedflags", out source);
        return SQLiteConvert.ToBoolean(source);
      }
      set => this["nosharedflags"] = (object) value;
    }

    /// <summary>
    /// Helper function for retrieving values from the connectionstring
    /// </summary>
    /// <param name="keyword">The keyword to retrieve settings for</param>
    /// <param name="value">The resulting parameter value</param>
    /// <returns>Returns true if the value was found and returned</returns>
    public override bool TryGetValue(string keyword, out object value)
    {
      bool flag = base.TryGetValue(keyword, out value);
      if (!this._properties.ContainsKey((object) keyword) || !(this._properties[(object) keyword] is PropertyDescriptor property))
        return flag;
      if (flag)
      {
        if (property.PropertyType == typeof (bool))
          value = (object) SQLiteConvert.ToBoolean(value);
        else if (property.PropertyType != typeof (byte[]))
          value = TypeDescriptor.GetConverter(property.PropertyType).ConvertFrom(value);
      }
      else if (property.Attributes[typeof (DefaultValueAttribute)] is DefaultValueAttribute attribute2)
      {
        value = attribute2.Value;
        flag = true;
      }
      return flag;
    }

    /// <summary>
    /// Fallback method for MONO, which doesn't implement DbConnectionStringBuilder.GetProperties()
    /// </summary>
    /// <param name="propertyList">The hashtable to fill with property descriptors</param>
    private void FallbackGetProperties(Hashtable propertyList)
    {
      foreach (PropertyDescriptor property in TypeDescriptor.GetProperties((object) this, true))
      {
        if (property.Name != "ConnectionString" && !propertyList.ContainsKey((object) property.DisplayName))
          propertyList.Add((object) property.DisplayName, (object) property);
      }
    }
  }
}
