// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Linq.IInternalSet`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Internal.Linq
{
  internal interface IInternalSet<TEntity> : IInternalSet, IInternalQuery, IInternalQuery<TEntity>
    where TEntity : class
  {
    TEntity Find(params object[] keyValues);

    Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);

    TEntity Create();

    TEntity Create(Type derivedEntityType);

    ObservableCollection<TEntity> Local { get; }
  }
}
