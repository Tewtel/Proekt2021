// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Linq.IInternalQuery`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Internal.Linq
{
  internal interface IInternalQuery<out TElement> : IInternalQuery
  {
    IInternalQuery<TElement> Include(string path);

    IInternalQuery<TElement> AsNoTracking();

    IInternalQuery<TElement> AsStreaming();

    IInternalQuery<TElement> WithExecutionStrategy(
      IDbExecutionStrategy executionStrategy);

    IDbAsyncEnumerator<TElement> GetAsyncEnumerator();

    IEnumerator<TElement> GetEnumerator();
  }
}
