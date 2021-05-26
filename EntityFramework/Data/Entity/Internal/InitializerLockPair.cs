// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InitializerLockPair
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Internal
{
  internal class InitializerLockPair : Tuple<Action<DbContext>, bool>
  {
    public InitializerLockPair(Action<DbContext> initializerDelegate, bool isLocked)
      : base(initializerDelegate, isLocked)
    {
    }

    public Action<DbContext> InitializerDelegate => this.Item1;

    public bool IsLocked => this.Item2;
  }
}
