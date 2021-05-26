// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteEnlistment
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Globalization;
using System.Transactions;

namespace System.Data.SQLite
{
  internal sealed class SQLiteEnlistment : IDisposable, IEnlistmentNotification
  {
    internal SQLiteTransaction _transaction;
    internal Transaction _scope;
    internal bool _disposeConnection;
    private bool disposed;

    internal SQLiteEnlistment(
      SQLiteConnection cnn,
      Transaction scope,
      System.Data.IsolationLevel defaultIsolationLevel,
      bool throwOnUnavailable,
      bool throwOnUnsupported)
    {
      this._transaction = cnn.BeginTransaction(this.GetSystemDataIsolationLevel(cnn, scope, defaultIsolationLevel, throwOnUnavailable, throwOnUnsupported));
      this._scope = scope;
      this._scope.EnlistVolatile((IEnlistmentNotification) this, EnlistmentOptions.None);
    }

    private System.Data.IsolationLevel GetSystemDataIsolationLevel(
      SQLiteConnection connection,
      Transaction transaction,
      System.Data.IsolationLevel defaultIsolationLevel,
      bool throwOnUnavailable,
      bool throwOnUnsupported)
    {
      if (transaction == (Transaction) null)
      {
        if (connection != null)
          return connection.GetDefaultIsolationLevel();
        if (throwOnUnavailable)
          throw new InvalidOperationException("isolation level is unavailable");
        return defaultIsolationLevel;
      }
      System.Transactions.IsolationLevel isolationLevel = transaction.IsolationLevel;
      switch (isolationLevel)
      {
        case System.Transactions.IsolationLevel.Serializable:
          return System.Data.IsolationLevel.Serializable;
        case System.Transactions.IsolationLevel.RepeatableRead:
          return System.Data.IsolationLevel.RepeatableRead;
        case System.Transactions.IsolationLevel.ReadCommitted:
          return System.Data.IsolationLevel.ReadCommitted;
        case System.Transactions.IsolationLevel.ReadUncommitted:
          return System.Data.IsolationLevel.ReadUncommitted;
        case System.Transactions.IsolationLevel.Snapshot:
          return System.Data.IsolationLevel.Snapshot;
        case System.Transactions.IsolationLevel.Chaos:
          return System.Data.IsolationLevel.Chaos;
        case System.Transactions.IsolationLevel.Unspecified:
          return System.Data.IsolationLevel.Unspecified;
        default:
          if (throwOnUnsupported)
            throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "unsupported isolation level {0}", (object) isolationLevel));
          return defaultIsolationLevel;
      }
    }

    private void Cleanup(SQLiteConnection cnn)
    {
      if (this._disposeConnection && cnn != null)
        cnn.Dispose();
      this._transaction = (SQLiteTransaction) null;
      this._scope = (Transaction) null;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteEnlistment).Name);
    }

    private void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        if (this._transaction != null)
        {
          this._transaction.Dispose();
          this._transaction = (SQLiteTransaction) null;
        }
        if (this._scope != (Transaction) null)
          this._scope = (Transaction) null;
      }
      this.disposed = true;
    }

    ~SQLiteEnlistment() => this.Dispose(false);

    public void Commit(Enlistment enlistment)
    {
      this.CheckDisposed();
      SQLiteConnection cnn = (SQLiteConnection) null;
label_1:
      try
      {
        cnn = this._transaction.Connection;
        if (cnn != null)
        {
          lock (cnn._enlistmentSyncRoot)
          {
            if (object.ReferenceEquals((object) cnn, (object) this._transaction.Connection))
            {
              cnn._enlistment = (SQLiteEnlistment) null;
              this._transaction.IsValid(true);
              cnn._transactionLevel = 1;
              this._transaction.Commit();
            }
            else
              goto label_1;
          }
        }
        enlistment.Done();
      }
      finally
      {
        this.Cleanup(cnn);
      }
    }

    public void InDoubt(Enlistment enlistment)
    {
      this.CheckDisposed();
      enlistment.Done();
    }

    public void Prepare(PreparingEnlistment preparingEnlistment)
    {
      this.CheckDisposed();
      if (!this._transaction.IsValid(false))
        preparingEnlistment.ForceRollback();
      else
        preparingEnlistment.Prepared();
    }

    public void Rollback(Enlistment enlistment)
    {
      this.CheckDisposed();
      SQLiteConnection cnn = (SQLiteConnection) null;
label_1:
      try
      {
        cnn = this._transaction.Connection;
        if (cnn != null)
        {
          lock (cnn._enlistmentSyncRoot)
          {
            if (object.ReferenceEquals((object) cnn, (object) this._transaction.Connection))
            {
              cnn._enlistment = (SQLiteEnlistment) null;
              this._transaction.Rollback();
            }
            else
              goto label_1;
          }
        }
        enlistment.Done();
      }
      finally
      {
        this.Cleanup(cnn);
      }
    }
  }
}
