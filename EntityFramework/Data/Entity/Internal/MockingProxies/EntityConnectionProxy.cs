// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.MockingProxies.EntityConnectionProxy
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure.Interception;

namespace System.Data.Entity.Internal.MockingProxies
{
  internal class EntityConnectionProxy
  {
    private readonly EntityConnection _entityConnection;

    protected EntityConnectionProxy()
    {
    }

    public EntityConnectionProxy(EntityConnection entityConnection) => this._entityConnection = entityConnection;

    public static implicit operator EntityConnection(EntityConnectionProxy proxy) => proxy._entityConnection;

    public virtual DbConnection StoreConnection => this._entityConnection.StoreConnection;

    public virtual void Dispose() => this._entityConnection.Dispose();

    public virtual EntityConnectionProxy CreateNew(DbConnection storeConnection)
    {
      EntityConnection entityConnection = new EntityConnection(this._entityConnection.GetMetadataWorkspace(), storeConnection);
      EntityTransaction currentTransaction = this._entityConnection.CurrentTransaction;
      if (currentTransaction != null && DbInterception.Dispatch.Transaction.GetConnection(currentTransaction.StoreTransaction, this._entityConnection.InterceptionContext) == storeConnection)
        entityConnection.UseStoreTransaction(currentTransaction.StoreTransaction);
      return new EntityConnectionProxy(entityConnection);
    }
  }
}
