// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Linq.IInternalSet
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Internal.Linq
{
  internal interface IInternalSet : IInternalQuery
  {
    void Attach(object entity);

    void Add(object entity);

    void AddRange(IEnumerable entities);

    void RemoveRange(IEnumerable entities);

    void Remove(object entity);

    void Initialize();

    void TryInitialize();

    IEnumerator ExecuteSqlQuery(
      string sql,
      bool asNoTracking,
      bool? streaming,
      object[] parameters);

    IDbAsyncEnumerator ExecuteSqlQueryAsync(
      string sql,
      bool asNoTracking,
      bool? streaming,
      object[] parameters);
  }
}
