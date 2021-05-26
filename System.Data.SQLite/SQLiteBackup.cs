// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBackup
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>Represents a single SQL backup in SQLite.</summary>
  internal sealed class SQLiteBackup : IDisposable
  {
    /// <summary>The underlying SQLite object this backup is bound to.</summary>
    internal SQLiteBase _sql;
    /// <summary>The actual backup handle.</summary>
    internal SQLiteBackupHandle _sqlite_backup;
    /// <summary>The destination database for the backup.</summary>
    internal IntPtr _destDb;
    /// <summary>The destination database name for the backup.</summary>
    internal byte[] _zDestName;
    /// <summary>The source database for the backup.</summary>
    internal IntPtr _sourceDb;
    /// <summary>The source database name for the backup.</summary>
    internal byte[] _zSourceName;
    /// <summary>
    /// The last result from the StepBackup method of the SQLite3 class.
    /// This is used to determine if the call to the FinishBackup method of
    /// the SQLite3 class should throw an exception when it receives a non-Ok
    /// return code from the core SQLite library.
    /// </summary>
    internal SQLiteErrorCode _stepResult;
    private bool disposed;

    /// <summary>Initializes the backup.</summary>
    /// <param name="sqlbase">The base SQLite object.</param>
    /// <param name="backup">The backup handle.</param>
    /// <param name="destDb">The destination database for the backup.</param>
    /// <param name="zDestName">The destination database name for the backup.</param>
    /// <param name="sourceDb">The source database for the backup.</param>
    /// <param name="zSourceName">The source database name for the backup.</param>
    internal SQLiteBackup(
      SQLiteBase sqlbase,
      SQLiteBackupHandle backup,
      IntPtr destDb,
      byte[] zDestName,
      IntPtr sourceDb,
      byte[] zSourceName)
    {
      this._sql = sqlbase;
      this._sqlite_backup = backup;
      this._destDb = destDb;
      this._zDestName = zDestName;
      this._sourceDb = sourceDb;
      this._zSourceName = zSourceName;
    }

    /// <summary>Disposes and finalizes the backup.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteBackup).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this._sqlite_backup != null)
        {
          this._sqlite_backup.Dispose();
          this._sqlite_backup = (SQLiteBackupHandle) null;
        }
        this._zSourceName = (byte[]) null;
        this._sourceDb = IntPtr.Zero;
        this._zDestName = (byte[]) null;
        this._destDb = IntPtr.Zero;
        this._sql = (SQLiteBase) null;
      }
      this.disposed = true;
    }

    ~SQLiteBackup() => this.Dispose(false);
  }
}
