// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteTransaction
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Threading;

namespace System.Data.SQLite
{
  /// <summary>
  /// SQLite implementation of DbTransaction that does not support nested transactions.
  /// </summary>
  public class SQLiteTransaction : SQLiteTransactionBase
  {
    private bool disposed;

    /// <summary>
    /// Constructs the transaction object, binding it to the supplied connection
    /// </summary>
    /// <param name="connection">The connection to open a transaction on</param>
    /// <param name="deferredLock">TRUE to defer the writelock, or FALSE to lock immediately</param>
    internal SQLiteTransaction(SQLiteConnection connection, bool deferredLock)
      : base(connection, deferredLock)
    {
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteTransaction).Name);
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
      if (this._cnn._transactionLevel - 1 == 0)
      {
        using (SQLiteCommand command = this._cnn.CreateCommand())
        {
          command.CommandText = "COMMIT;";
          command.ExecuteNonQuery();
        }
      }
      --this._cnn._transactionLevel;
      this._cnn = (SQLiteConnection) null;
    }

    /// <summary>
    /// Attempts to start a transaction.  An exception will be thrown if the transaction cannot
    /// be started for any reason.
    /// </summary>
    /// <param name="deferredLock">TRUE to defer the writelock, or FALSE to lock immediately</param>
    protected override void Begin(bool deferredLock)
    {
      if (this._cnn._transactionLevel++ != 0)
        return;
      try
      {
        using (SQLiteCommand command = this._cnn.CreateCommand())
        {
          if (!deferredLock)
            command.CommandText = "BEGIN IMMEDIATE;";
          else
            command.CommandText = "BEGIN;";
          command.ExecuteNonQuery();
        }
      }
      catch (SQLiteException ex)
      {
        --this._cnn._transactionLevel;
        this._cnn = (SQLiteConnection) null;
        throw;
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
      try
      {
        using (SQLiteCommand command = sqLiteConnection.CreateCommand())
        {
          command.CommandText = "ROLLBACK;";
          command.ExecuteNonQuery();
        }
      }
      catch
      {
        if (throwError)
          throw;
      }
      sqLiteConnection._transactionLevel = 0;
    }
  }
}
