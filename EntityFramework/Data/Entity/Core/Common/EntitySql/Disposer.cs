// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.Disposer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal class Disposer : IDisposable
  {
    private readonly Action _action;

    internal Disposer(Action action) => this._action = action;

    public void Dispose()
    {
      this._action();
      GC.SuppressFinalize((object) this);
    }
  }
}
