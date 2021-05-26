// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteTransactionBase
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Data.Common;

namespace System.Data.SQLite
{
  /// <summary>
  /// Base class used by to implement DbTransaction for SQLite.
  /// </summary>
  public abstract class SQLiteTransactionBase : DbTransaction
  {
    /// <summary>The connection to which this transaction is bound.</summary>
    internal SQLiteConnection _cnn;
    /// <summary>Matches the version of the connection.</summary>
    internal int _version;
    /// <summary>The isolation level for this transaction.</summary>
    private IsolationLevel _level;
    private bool disposed;

    /// <summary>
    /// Constructs the transaction object, binding it to the supplied connection
    /// </summary>
    /// <param name="connection">The connection to open a transaction on</param>
    /// <param name="deferredLock">TRUE to defer the writelock, or FALSE to lock immediately</param>
    internal SQLiteTransactionBase(SQLiteConnection connection, bool deferredLock)
    {
      this._cnn = connection;
      this._version = this._cnn._version;
      this._level = deferredLock ? IsolationLevel.ReadCommitted : IsolationLevel.Serializable;
      this.Begin(deferredLock);
    }

    /// <summary>
    /// Gets the isolation level of the transaction.  SQLite only supports Serializable transactions.
    /// </summary>
    public override IsolationLevel IsolationLevel
    {
      get
      {
        this.CheckDisposed();
        return this._level;
      }
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteTransactionBase).Name);
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

    /// <summary>
    /// Returns the underlying connection to which this transaction applies.
    /// </summary>
    public SQLiteConnection Connection
    {
      get
      {
        this.CheckDisposed();
        return this._cnn;
      }
    }

    /// <summary>Forwards to the local Connection property</summary>
    protected override DbConnection DbConnection => (DbConnection) this.Connection;

    /// <summary>Rolls back the active transaction.</summary>
    public override void Rollback()
    {
      this.CheckDisposed();
      this.IsValid(true);
      this.IssueRollback(true);
    }

    /// <summary>
    /// Attempts to start a transaction.  An exception will be thrown if the transaction cannot
    /// be started for any reason.
    /// </summary>
    /// <param name="deferredLock">TRUE to defer the writelock, or FALSE to lock immediately</param>
    protected abstract void Begin(bool deferredLock);

    /// <summary>
    /// Issue a ROLLBACK command against the database connection,
    /// optionally re-throwing any caught exception.
    /// </summary>
    /// <param name="throwError">Non-zero to re-throw caught exceptions.</param>
    protected abstract void IssueRollback(bool throwError);

    /// <summary>
    /// Checks the state of this transaction, optionally throwing an exception if a state
    /// inconsistency is found.
    /// </summary>
    /// <param name="throwError">
    /// Non-zero to throw an exception if a state inconsistency is found.
    /// </param>
    /// <returns>
    /// Non-zero if this transaction is valid; otherwise, false.
    /// </returns>
    internal bool IsValid(bool throwError)
    {
      if (this._cnn == null)
      {
        if (throwError)
          throw new ArgumentNullException("No connection associated with this transaction");
        return false;
      }
      if (this._cnn._version != this._version)
      {
        if (throwError)
          throw new SQLiteException("The connection was closed and re-opened, changes were already rolled back");
        return false;
      }
      if (this._cnn.State != ConnectionState.Open)
      {
        if (throwError)
          throw new SQLiteException("Connection was closed");
        return false;
      }
      if (this._cnn._transactionLevel != 0 && !this._cnn._sql.AutoCommit)
        return true;
      this._cnn._transactionLevel = 0;
      if (throwError)
        throw new SQLiteException("No transaction is active on this connection");
      return false;
    }
  }
}
