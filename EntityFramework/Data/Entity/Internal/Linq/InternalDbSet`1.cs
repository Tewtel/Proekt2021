// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Linq.InternalDbSet`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Internal.Linq
{
  internal class InternalDbSet<TEntity> : 
    DbSet,
    IQueryable<TEntity>,
    IEnumerable<TEntity>,
    IEnumerable,
    IQueryable,
    IDbAsyncEnumerable<TEntity>,
    IDbAsyncEnumerable
    where TEntity : class
  {
    private readonly IInternalSet<TEntity> _internalSet;

    public InternalDbSet(IInternalSet<TEntity> internalSet) => this._internalSet = internalSet;

    public static InternalDbSet<TEntity> Create(
      InternalContext internalContext,
      IInternalSet internalSet)
    {
      return new InternalDbSet<TEntity>((IInternalSet<TEntity>) internalSet ?? (IInternalSet<TEntity>) new System.Data.Entity.Internal.Linq.InternalSet<TEntity>(internalContext));
    }

    internal override IInternalQuery InternalQuery => (IInternalQuery) this._internalSet;

    internal override IInternalSet InternalSet => (IInternalSet) this._internalSet;

    public override DbQuery Include(string path)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(path, nameof (path));
      return (DbQuery) new InternalDbQuery<TEntity>(this._internalSet.Include(path));
    }

    public override DbQuery AsNoTracking() => (DbQuery) new InternalDbQuery<TEntity>(this._internalSet.AsNoTracking());

    [Obsolete("Queries are now streaming by default unless a retrying ExecutionStrategy is used. Calling this method will have no effect.")]
    public override DbQuery AsStreaming() => (DbQuery) new InternalDbQuery<TEntity>(this._internalSet.AsStreaming());

    internal override DbQuery WithExecutionStrategy(IDbExecutionStrategy executionStrategy) => (DbQuery) new InternalDbQuery<TEntity>(this._internalSet.WithExecutionStrategy(executionStrategy));

    public override object Find(params object[] keyValues) => (object) this._internalSet.Find(keyValues);

    internal override IInternalQuery GetInternalQueryWithCheck(string memberName) => (IInternalQuery) this._internalSet;

    internal override IInternalSet GetInternalSetWithCheck(string memberName) => (IInternalSet) this._internalSet;

    public override async Task<object> FindAsync(
      CancellationToken cancellationToken,
      params object[] keyValues)
    {
      return (object) await this._internalSet.FindAsync(cancellationToken, keyValues).WithCurrentCulture<TEntity>();
    }

    public override IList Local => (IList) this._internalSet.Local;

    public override object Create() => (object) this._internalSet.Create();

    public override object Create(Type derivedEntityType)
    {
      System.Data.Entity.Utilities.Check.NotNull<Type>(derivedEntityType, nameof (derivedEntityType));
      return (object) this._internalSet.Create(derivedEntityType);
    }

    public IEnumerator<TEntity> GetEnumerator() => this._internalSet.GetEnumerator();

    public IDbAsyncEnumerator<TEntity> GetAsyncEnumerator() => this._internalSet.GetAsyncEnumerator();
  }
}
