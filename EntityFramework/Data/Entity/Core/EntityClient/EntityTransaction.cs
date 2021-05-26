// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntityClient.EntityTransaction
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.Core.EntityClient
{
  /// <summary>
  /// Class representing a transaction for the conceptual layer
  /// </summary>
  public class EntityTransaction : DbTransaction
  {
    private readonly EntityConnection _connection;
    private readonly DbTransaction _storeTransaction;

    internal EntityTransaction()
    {
    }

    internal EntityTransaction(EntityConnection connection, DbTransaction storeTransaction)
    {
      this._connection = connection;
      this._storeTransaction = storeTransaction;
    }

    /// <summary>
    /// Gets <see cref="T:System.Data.Entity.Core.EntityClient.EntityConnection" /> for this
    /// <see cref="T:System.Data.Entity.Core.EntityClient.EntityTransaction" />
    /// .
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.EntityClient.EntityConnection" /> to the underlying data source.
    /// </returns>
    public virtual EntityConnection Connection => (EntityConnection) this.DbConnection;

    /// <summary>The connection object owning this transaction object</summary>
    protected override DbConnection DbConnection => (this._storeTransaction != null ? DbInterception.Dispatch.Transaction.GetConnection(this._storeTransaction, this.InterceptionContext) : (DbConnection) null) == null ? (DbConnection) null : (DbConnection) this._connection;

    /// <summary>
    /// Gets the isolation level of this <see cref="T:System.Data.Entity.Core.EntityClient.EntityTransaction" />.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.IsolationLevel" /> enumeration value that represents the isolation level of the underlying transaction.
    /// </returns>
    public override IsolationLevel IsolationLevel => this._storeTransaction == null ? ~IsolationLevel.Unspecified : DbInterception.Dispatch.Transaction.GetIsolationLevel(this._storeTransaction, this.InterceptionContext);

    /// <summary>
    /// Gets the DbTransaction for the underlying provider transaction.
    /// </summary>
    public virtual DbTransaction StoreTransaction => this._storeTransaction;

    private DbInterceptionContext InterceptionContext => DbInterceptionContext.Combine(this._connection.AssociatedContexts.Select<ObjectContext, DbInterceptionContext>((Func<ObjectContext, DbInterceptionContext>) (c => c.InterceptionContext)));

    /// <summary>Commits the underlying transaction.</summary>
    public override void Commit()
    {
      try
      {
        if (this._storeTransaction != null)
          DbInterception.Dispatch.Transaction.Commit(this._storeTransaction, this.InterceptionContext);
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType() && !(ex is CommitFailedException))
          throw new EntityException(System.Data.Entity.Resources.Strings.EntityClient_ProviderSpecificError((object) nameof (Commit)), ex);
        throw;
      }
      this.ClearCurrentTransaction();
    }

    /// <summary>Rolls back the underlying transaction.</summary>
    public override void Rollback()
    {
      try
      {
        if (this._storeTransaction != null)
          DbInterception.Dispatch.Transaction.Rollback(this._storeTransaction, this.InterceptionContext);
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          throw new EntityException(System.Data.Entity.Resources.Strings.EntityClient_ProviderSpecificError((object) nameof (Rollback)), ex);
        throw;
      }
      this.ClearCurrentTransaction();
    }

    /// <summary>Cleans up this transaction object</summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to release only unmanaged resources </param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        this.ClearCurrentTransaction();
        if (this._storeTransaction != null)
          DbInterception.Dispatch.Transaction.Dispose(this._storeTransaction, this.InterceptionContext);
      }
      base.Dispose(disposing);
    }

    private void ClearCurrentTransaction()
    {
      if (this._connection == null || this._connection.CurrentTransaction != this)
        return;
      this._connection.ClearCurrentTransaction();
    }
  }
}
