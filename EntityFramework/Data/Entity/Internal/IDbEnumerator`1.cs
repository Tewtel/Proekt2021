// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.IDbEnumerator`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Internal
{
  internal interface IDbEnumerator<out T> : 
    IEnumerator<T>,
    IDisposable,
    IEnumerator,
    IDbAsyncEnumerator<T>,
    IDbAsyncEnumerator
  {
    new T Current { get; }
  }
}
