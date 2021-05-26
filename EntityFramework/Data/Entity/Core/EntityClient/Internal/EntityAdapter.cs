// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntityClient.Internal.EntityAdapter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core.Mapping.Update.Internal;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Core.EntityClient.Internal
{
  internal class EntityAdapter : IEntityAdapter
  {
    private bool _acceptChangesDuringUpdate = true;
    private EntityConnection _connection;
    private readonly ObjectContext _context;
    private readonly Func<EntityAdapter, UpdateTranslator> _updateTranslatorFactory;

    public EntityAdapter(ObjectContext context)
      : this(context, (Func<EntityAdapter, UpdateTranslator>) (a => new UpdateTranslator(a)))
    {
    }

    protected EntityAdapter(
      ObjectContext context,
      Func<EntityAdapter, UpdateTranslator> updateTranslatorFactory)
    {
      this._context = context;
      this._updateTranslatorFactory = updateTranslatorFactory;
    }

    public ObjectContext Context => this._context;

    DbConnection IEntityAdapter.Connection
    {
      get => (DbConnection) this.Connection;
      set => this.Connection = (EntityConnection) value;
    }

    public EntityConnection Connection
    {
      get => this._connection;
      set => this._connection = value;
    }

    public bool AcceptChangesDuringUpdate
    {
      get => this._acceptChangesDuringUpdate;
      set => this._acceptChangesDuringUpdate = value;
    }

    public int? CommandTimeout { get; set; }

    public int Update() => this.Update<int>(0, (Func<UpdateTranslator, int>) (ut => ut.Update()));

    public Task<int> UpdateAsync(CancellationToken cancellationToken) => this.Update<Task<int>>(Task.FromResult<int>(0), (Func<UpdateTranslator, Task<int>>) (ut => ut.UpdateAsync(cancellationToken)));

    private T Update<T>(T noChangesResult, Func<UpdateTranslator, T> updateFunction)
    {
      if (!EntityAdapter.IsStateManagerDirty(this._context.ObjectStateManager))
        return noChangesResult;
      if (this._connection == null)
        throw Error.EntityClient_NoConnectionForAdapter();
      if (this._connection.StoreProviderFactory == null || this._connection.StoreConnection == null)
        throw Error.EntityClient_NoStoreConnectionForUpdate();
      if (ConnectionState.Open != this._connection.State)
        throw Error.EntityClient_ClosedConnectionForUpdate();
      UpdateTranslator updateTranslator = this._updateTranslatorFactory(this);
      return updateFunction(updateTranslator);
    }

    private static bool IsStateManagerDirty(ObjectStateManager entityCache) => entityCache.HasChanges();
  }
}
