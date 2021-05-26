// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Linq.IInternalQuery
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.ELinq;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace System.Data.Entity.Internal.Linq
{
  internal interface IInternalQuery
  {
    void ResetQuery();

    InternalContext InternalContext { get; }

    ObjectQuery ObjectQuery { get; }

    Type ElementType { get; }

    Expression Expression { get; }

    ObjectQueryProvider ObjectQueryProvider { get; }

    string ToTraceString();

    IDbAsyncEnumerator GetAsyncEnumerator();

    IEnumerator GetEnumerator();
  }
}
