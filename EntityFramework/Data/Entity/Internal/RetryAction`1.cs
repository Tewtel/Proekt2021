// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.RetryAction`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Diagnostics;

namespace System.Data.Entity.Internal
{
  internal class RetryAction<TInput>
  {
    private readonly object _lock = new object();
    private Action<TInput> _action;

    public RetryAction(Action<TInput> action) => this._action = action;

    [DebuggerStepThrough]
    public void PerformAction(TInput input)
    {
      lock (this._lock)
      {
        if (this._action == null)
          return;
        Action<TInput> action = this._action;
        this._action = (Action<TInput>) null;
        try
        {
          action(input);
        }
        catch (Exception ex)
        {
          this._action = action;
          throw;
        }
      }
    }
  }
}
