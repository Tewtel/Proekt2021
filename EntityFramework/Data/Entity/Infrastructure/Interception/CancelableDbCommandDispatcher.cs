// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.CancelableDbCommandDispatcher
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;

namespace System.Data.Entity.Infrastructure.Interception
{
  internal class CancelableDbCommandDispatcher
  {
    private readonly System.Data.Entity.Infrastructure.Interception.InternalDispatcher<ICancelableDbCommandInterceptor> _internalDispatcher = new System.Data.Entity.Infrastructure.Interception.InternalDispatcher<ICancelableDbCommandInterceptor>();

    public System.Data.Entity.Infrastructure.Interception.InternalDispatcher<ICancelableDbCommandInterceptor> InternalDispatcher => this._internalDispatcher;

    public virtual bool Executing(DbCommand command, DbInterceptionContext interceptionContext) => this._internalDispatcher.Dispatch<bool>(true, (Func<bool, ICancelableDbCommandInterceptor, bool>) ((b, i) => i.CommandExecuting(command, interceptionContext) & b));
  }
}
