// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteFactory
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.Data.SQLite
{
  /// <summary>
  /// SQLite implementation of <see cref="T:System.Data.Common.DbProviderFactory" />.
  /// </summary>
  /// <summary>
  /// SQLite implementation of <see cref="T:System.IServiceProvider" />.
  /// </summary>
  public sealed class SQLiteFactory : DbProviderFactory, IDisposable, IServiceProvider
  {
    private bool disposed;
    /// <summary>
    /// Static instance member which returns an instanced <see cref="T:System.Data.SQLite.SQLiteFactory" /> class.
    /// </summary>
    public static readonly SQLiteFactory Instance = new SQLiteFactory();
    private static readonly string DefaultTypeName = "System.Data.SQLite.Linq.SQLiteProviderServices, System.Data.SQLite.Linq, Version={0}, Culture=neutral, PublicKeyToken=db937bc2d44ff139";
    private static readonly BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
    private static Type _dbProviderServicesType;
    private static object _sqliteServices;

    /// <summary>
    /// Cleans up resources (native and managed) associated with the current instance.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteFactory).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
    }

    /// <summary>
    /// Cleans up resources associated with the current instance.
    /// </summary>
    ~SQLiteFactory() => this.Dispose(false);

    /// <summary>
    /// This event is raised whenever SQLite raises a logging event.
    /// Note that this should be set as one of the first things in the
    /// application.  This event is provided for backward compatibility only.
    /// New code should use the <see cref="T:System.Data.SQLite.SQLiteLog" /> class instead.
    /// </summary>
    public event SQLiteLogEventHandler Log
    {
      add
      {
        this.CheckDisposed();
        SQLiteLog.Log += value;
      }
      remove
      {
        this.CheckDisposed();
        SQLiteLog.Log -= value;
      }
    }

    /// <summary>
    /// Creates and returns a new <see cref="T:System.Data.SQLite.SQLiteCommand" /> object.
    /// </summary>
    /// <returns>The new object.</returns>
    public override DbCommand CreateCommand()
    {
      this.CheckDisposed();
      return (DbCommand) new SQLiteCommand();
    }

    /// <summary>
    /// Creates and returns a new <see cref="T:System.Data.SQLite.SQLiteCommandBuilder" /> object.
    /// </summary>
    /// <returns>The new object.</returns>
    public override DbCommandBuilder CreateCommandBuilder()
    {
      this.CheckDisposed();
      return (DbCommandBuilder) new SQLiteCommandBuilder();
    }

    /// <summary>
    /// Creates and returns a new <see cref="T:System.Data.SQLite.SQLiteConnection" /> object.
    /// </summary>
    /// <returns>The new object.</returns>
    public override DbConnection CreateConnection()
    {
      this.CheckDisposed();
      return (DbConnection) new SQLiteConnection();
    }

    /// <summary>
    /// Creates and returns a new <see cref="T:System.Data.SQLite.SQLiteConnectionStringBuilder" /> object.
    /// </summary>
    /// <returns>The new object.</returns>
    public override DbConnectionStringBuilder CreateConnectionStringBuilder()
    {
      this.CheckDisposed();
      return (DbConnectionStringBuilder) new SQLiteConnectionStringBuilder();
    }

    /// <summary>
    /// Creates and returns a new <see cref="T:System.Data.SQLite.SQLiteDataAdapter" /> object.
    /// </summary>
    /// <returns>The new object.</returns>
    public override DbDataAdapter CreateDataAdapter()
    {
      this.CheckDisposed();
      return (DbDataAdapter) new SQLiteDataAdapter();
    }

    /// <summary>
    /// Creates and returns a new <see cref="T:System.Data.SQLite.SQLiteParameter" /> object.
    /// </summary>
    /// <returns>The new object.</returns>
    public override DbParameter CreateParameter()
    {
      this.CheckDisposed();
      return (DbParameter) new SQLiteParameter();
    }

    static SQLiteFactory()
    {
      UnsafeNativeMethods.Initialize();
      SQLiteLog.Initialize(typeof (SQLiteFactory).Name);
      SQLiteFactory._dbProviderServicesType = Type.GetType(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "System.Data.Common.DbProviderServices, System.Data.Entity, Version={0}, Culture=neutral, PublicKeyToken=b77a5c561934e089", (object) "4.0.0.0"), false);
    }

    /// <summary>
    /// Will provide a <see cref="T:System.IServiceProvider" /> object in .NET 3.5.
    /// </summary>
    /// <param name="serviceType">The class or interface type to query for.</param>
    /// <returns></returns>
    object IServiceProvider.GetService(Type serviceType) => serviceType == typeof (ISQLiteSchemaExtensions) || SQLiteFactory._dbProviderServicesType != (Type) null && serviceType == SQLiteFactory._dbProviderServicesType ? this.GetSQLiteProviderServicesInstance() : (object) null;

    [ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
    private object GetSQLiteProviderServicesInstance()
    {
      if (SQLiteFactory._sqliteServices == null)
      {
        string settingValue = UnsafeNativeMethods.GetSettingValue("TypeName_SQLiteProviderServices", (string) null);
        Version version = this.GetType().Assembly.GetName().Version;
        string typeName;
        if (settingValue != null)
          typeName = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, settingValue, (object) version);
        else
          typeName = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, SQLiteFactory.DefaultTypeName, (object) version);
        Type type = Type.GetType(typeName, false);
        if (type != (Type) null)
        {
          FieldInfo field = type.GetField("Instance", SQLiteFactory.DefaultBindingFlags);
          if (field != (FieldInfo) null)
            SQLiteFactory._sqliteServices = field.GetValue((object) null);
        }
      }
      return SQLiteFactory._sqliteServices;
    }
  }
}
