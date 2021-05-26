// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ClonedObjectContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Internal.MockingProxies;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Internal
{
  internal class ClonedObjectContext : IDisposable
  {
    private ObjectContextProxy _objectContext;
    private readonly bool _connectionCloned;
    private readonly EntityConnectionProxy _clonedEntityConnection;

    protected ClonedObjectContext()
    {
    }

    public ClonedObjectContext(
      ObjectContextProxy objectContext,
      DbConnection connection,
      string connectionString,
      bool transferLoadedAssemblies = true)
    {
      if (connection == null || connection.State != ConnectionState.Open)
      {
        connection = connection ?? objectContext.Connection.StoreConnection;
        connection = DbProviderServices.GetProviderServices(connection).CloneDbConnection(connection);
        DbInterception.Dispatch.Connection.SetConnectionString(connection, new DbConnectionPropertyInterceptionContext<string>().WithValue(connectionString));
        this._connectionCloned = true;
      }
      this._clonedEntityConnection = objectContext.Connection.CreateNew(connection);
      this._objectContext = objectContext.CreateNew(this._clonedEntityConnection);
      this._objectContext.CopyContextOptions(objectContext);
      if (!string.IsNullOrWhiteSpace(objectContext.DefaultContainerName))
        this._objectContext.DefaultContainerName = objectContext.DefaultContainerName;
      if (!transferLoadedAssemblies)
        return;
      this.TransferLoadedAssemblies(objectContext);
    }

    public virtual ObjectContextProxy ObjectContext => this._objectContext;

    public virtual DbConnection Connection => this._objectContext.Connection.StoreConnection;

    private void TransferLoadedAssemblies(ObjectContextProxy source)
    {
      IEnumerable<GlobalItem> objectItemCollection = source.GetObjectItemCollection();
      foreach (Assembly assembly in objectItemCollection.Where<GlobalItem>((Func<GlobalItem, bool>) (i => i is EntityType || i is ComplexType)).Select<GlobalItem, Assembly>((Func<GlobalItem, Assembly>) (i => source.GetClrType((StructuralType) i).Assembly())).Union<Assembly>(objectItemCollection.OfType<EnumType>().Select<EnumType, Assembly>((Func<EnumType, Assembly>) (i => source.GetClrType(i).Assembly()))).Distinct<Assembly>())
        this._objectContext.LoadFromAssembly(assembly);
    }

    public void Dispose()
    {
      if (this._objectContext == null)
        return;
      ObjectContextProxy objectContext = this._objectContext;
      DbConnection connection = this.Connection;
      this._objectContext = (ObjectContextProxy) null;
      objectContext.Dispose();
      this._clonedEntityConnection.Dispose();
      if (!this._connectionCloned)
        return;
      DbInterception.Dispatch.Connection.Dispose(connection, new DbInterceptionContext());
    }
  }
}
