﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.MockingProxies.ObjectContextProxy
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Reflection;

namespace System.Data.Entity.Internal.MockingProxies
{
  internal class ObjectContextProxy : IDisposable
  {
    private readonly ObjectContext _objectContext;
    private ObjectItemCollection _objectItemCollection;

    protected ObjectContextProxy()
    {
    }

    public ObjectContextProxy(ObjectContext objectContext) => this._objectContext = objectContext;

    public static implicit operator ObjectContext(ObjectContextProxy proxy) => proxy?._objectContext;

    public virtual EntityConnectionProxy Connection => new EntityConnectionProxy((EntityConnection) this._objectContext.Connection);

    public virtual string DefaultContainerName
    {
      get => this._objectContext.DefaultContainerName;
      set => this._objectContext.DefaultContainerName = value;
    }

    public virtual void Dispose() => this._objectContext.Dispose();

    public virtual IEnumerable<GlobalItem> GetObjectItemCollection() => (IEnumerable<GlobalItem>) (this._objectItemCollection = (ObjectItemCollection) this._objectContext.MetadataWorkspace.GetItemCollection(DataSpace.OSpace));

    public virtual Type GetClrType(StructuralType item) => this._objectItemCollection.GetClrType(item);

    public virtual Type GetClrType(EnumType item) => this._objectItemCollection.GetClrType(item);

    public virtual void LoadFromAssembly(Assembly assembly) => this._objectContext.MetadataWorkspace.LoadFromAssembly(assembly);

    public virtual ObjectContextProxy CreateNew(
      EntityConnectionProxy entityConnection)
    {
      return new ObjectContextProxy(new ObjectContext((EntityConnection) entityConnection));
    }

    public virtual void CopyContextOptions(ObjectContextProxy source)
    {
      this._objectContext.ContextOptions.LazyLoadingEnabled = source._objectContext.ContextOptions.LazyLoadingEnabled;
      this._objectContext.ContextOptions.ProxyCreationEnabled = source._objectContext.ContextOptions.ProxyCreationEnabled;
      this._objectContext.ContextOptions.UseCSharpNullComparisonBehavior = source._objectContext.ContextOptions.UseCSharpNullComparisonBehavior;
      this._objectContext.ContextOptions.UseConsistentNullReferenceBehavior = source._objectContext.ContextOptions.UseConsistentNullReferenceBehavior;
      this._objectContext.ContextOptions.UseLegacyPreserveChangesBehavior = source._objectContext.ContextOptions.UseLegacyPreserveChangesBehavior;
      this._objectContext.CommandTimeout = source._objectContext.CommandTimeout;
      this._objectContext.ContextOptions.DisableFilterOverProjectionSimplificationForCustomFunctions = source._objectContext.ContextOptions.DisableFilterOverProjectionSimplificationForCustomFunctions;
      this._objectContext.InterceptionContext = source._objectContext.InterceptionContext.WithObjectContext(this._objectContext);
    }
  }
}
