// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ThrowingMonitor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;
using System.Threading;

namespace System.Data.Entity.Internal
{
  internal class ThrowingMonitor
  {
    private int _isInCriticalSection;

    public void Enter()
    {
      if (Interlocked.CompareExchange(ref this._isInCriticalSection, 1, 0) != 0)
        throw new NotSupportedException(Strings.ConcurrentMethodInvocation);
    }

    public void Exit() => Interlocked.Exchange(ref this._isInCriticalSection, 0);

    public void EnsureNotEntered()
    {
      Thread.MemoryBarrier();
      if (this._isInCriticalSection != 0)
        throw new NotSupportedException(Strings.ConcurrentMethodInvocation);
    }
  }
}
