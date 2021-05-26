// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteTransaction2
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Threading;

namespace System.Data.SQLite
{
  /// <summary>
  /// SQLite implementation of DbTransaction that does support nested transactions.
  /// </summary>
  public sealed class SQLiteTransaction2 : SQLiteTransaction
  {
    /// <summary>
    /// The original transaction level for the associated connection
    /// when this transaction was created (i.e. begun).
    /// </summary>
    private int _beginLevel;
    /// <summary>
    /// The SAVEPOINT name for this transaction, if any.  This will
    /// only be non-null if this transaction is a nested one.
    /// </summary>
    private string _savePointName;
    private bool disposed;

    /// <summary>
    /// Constructs the transaction object, binding it to the supplied connection
    /// </summary>
    /// <param name="connection">The connection to open a transaction on</param>
    /// <param name="deferredLock">TRUE to defer the writelock, or FALSE to lock immediately</param>
    internal SQLiteTransaction2(SQLiteConnection connection, bool deferredLock)
      : base(connection, deferredLock)
    {
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteTransaction2).Name);
    }

    /// <summary>
    /// Disposes the transaction.  If it is currently active, any changes are rolled back.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed || !disposing || !this.IsValid(false))
          return;
        this.IssueRollback(false);
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }

    /// <summary>Commits the current transaction.</summary>
    public override void Commit()
    {
      this.CheckDisposed();
      this.IsValid(true);
      if (this._beginLevel == 0)
      {
        using (SQLiteCommand command = this._cnn.CreateCommand())
        {
          command.CommandText = "COMMIT;";
          command.ExecuteNonQuery();
        }
        this._cnn._transactionLevel = 0;
        this._cnn = (SQLiteConnection) null;
      }
      else
      {
        using (SQLiteCommand command = this._cnn.CreateCommand())
        {
          command.CommandText = !string.IsNullOrEmpty(this._savePointName) ? string.Format("RELEASE {0};", (object) this._savePointName) : throw new SQLiteException("Cannot commit, unknown SAVEPOINT");
          command.ExecuteNonQuery();
        }
        --this._cnn._transactionLevel;
        this._cnn = (SQLiteConnection) null;
      }
    }

    /// <summary>
    /// Attempts to start a transaction.  An exception will be thrown if the transaction cannot
    /// be started for any reason.
    /// </summary>
    /// <param name="deferredLock">TRUE to defer the writelock, or FALSE to lock immediately</param>
    protected override void Begin(bool deferredLock)
    {
      int num1 = this._cnn._transactionLevel++;
      int num2;
      if ((num2 = num1) == 0)
      {
        try
        {
          using (SQLiteCommand command = this._cnn.CreateCommand())
          {
            if (!deferredLock)
              command.CommandText = "BEGIN IMMEDIATE;";
            else
              command.CommandText = "BEGIN;";
            command.ExecuteNonQuery();
            this._beginLevel = num2;
          }
        }
        catch (SQLiteException ex)
        {
          --this._cnn._transactionLevel;
          this._cnn = (SQLiteConnection) null;
          throw;
        }
      }
      else
      {
        try
        {
          using (SQLiteCommand command = this._cnn.CreateCommand())
          {
            this._savePointName = this.GetSavePointName();
            command.CommandText = string.Format("SAVEPOINT {0};", (object) this._savePointName);
            command.ExecuteNonQuery();
            this._beginLevel = num2;
          }
        }
        catch (SQLiteException ex)
        {
          --this._cnn._transactionLevel;
          this._cnn = (SQLiteConnection) null;
          throw;
        }
      }
    }

    /// <summary>
    /// Issue a ROLLBACK command against the database connection,
    /// optionally re-throwing any caught exception.
    /// </summary>
    /// <param name="throwError">Non-zero to re-throw caught exceptions.</param>
    protected override void IssueRollback(bool throwError)
    {
      SQLiteConnection sqLiteConnection = Interlocked.Exchange<SQLiteConnection>(ref this._cnn, (SQLiteConnection) null);
      if (sqLiteConnection == null)
        return;
      if (this._beginLevel == 0)
      {
        try
        {
          using (SQLiteCommand command = sqLiteConnection.CreateCommand())
          {
            command.CommandText = "ROLLBACK;";
            command.ExecuteNonQuery();
          }
          sqLiteConnection._transactionLevel = 0;
        }
        catch
        {
          if (!throwError)
            return;
          throw;
        }
      }
      else
      {
        try
        {
          using (SQLiteCommand command = sqLiteConnection.CreateCommand())
          {
            command.CommandText = !string.IsNullOrEmpty(this._savePointName) ? string.Format("ROLLBACK TO {0};", (object) this._savePointName) : throw new SQLiteException("Cannot rollback, unknown SAVEPOINT");
            command.ExecuteNonQuery();
          }
          --sqLiteConnection._transactionLevel;
        }
        catch
        {
          if (!throwError)
            return;
          throw;
        }
      }
    }

    /// <summary>
    /// Constructs the name of a new savepoint for this transaction.  It
    /// should only be called from the constructor of this class.
    /// </summary>
    /// <returns>
    /// The name of the new savepoint -OR- null if it cannot be constructed.
    /// </returns>
    private string GetSavePointName() => string.Format("sqlite_dotnet_savepoint_{0}", (object) ++this._cnn._transactionSequence);
  }
}
