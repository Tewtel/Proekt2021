// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.SQLiteProviderFactory
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Data.Common;
using System.Globalization;

namespace System.Data.SQLite.Linq
{
  public sealed class SQLiteProviderFactory : DbProviderFactory, IServiceProvider, IDisposable
  {
    public static readonly SQLiteProviderFactory Instance = new SQLiteProviderFactory();
    private bool disposed;

    public override DbCommand CreateCommand()
    {
      this.CheckDisposed();
      return (DbCommand) new SQLiteCommand();
    }

    public override DbCommandBuilder CreateCommandBuilder()
    {
      this.CheckDisposed();
      return (DbCommandBuilder) new SQLiteCommandBuilder();
    }

    public override DbConnection CreateConnection()
    {
      this.CheckDisposed();
      return (DbConnection) new SQLiteConnection();
    }

    public override DbConnectionStringBuilder CreateConnectionStringBuilder()
    {
      this.CheckDisposed();
      return (DbConnectionStringBuilder) new SQLiteConnectionStringBuilder();
    }

    public override DbDataAdapter CreateDataAdapter()
    {
      this.CheckDisposed();
      return (DbDataAdapter) new SQLiteDataAdapter();
    }

    public override DbParameter CreateParameter()
    {
      this.CheckDisposed();
      return (DbParameter) new SQLiteParameter();
    }

    public object GetService(Type serviceType)
    {
      if (serviceType == typeof (ISQLiteSchemaExtensions) || serviceType == typeof (DbProviderServices))
      {
        SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "IServiceProvider.GetService for type \"{0}\" (success).", (object) serviceType));
        return (object) SQLiteProviderServices.Instance;
      }
      SQLiteLog.LogMessage(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "IServiceProvider.GetService for type \"{0}\" (failure).", (object) serviceType));
      return (object) null;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteProviderFactory).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
    }

    ~SQLiteProviderFactory() => this.Dispose(false);
  }
}
