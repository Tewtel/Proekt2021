// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.CancelableEntityConnectionDispatcher
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.EntityClient;

namespace System.Data.Entity.Infrastructure.Interception
{
  internal class CancelableEntityConnectionDispatcher
  {
    private readonly System.Data.Entity.Infrastructure.Interception.InternalDispatcher<ICancelableEntityConnectionInterceptor> _internalDispatcher = new System.Data.Entity.Infrastructure.Interception.InternalDispatcher<ICancelableEntityConnectionInterceptor>();

    public System.Data.Entity.Infrastructure.Interception.InternalDispatcher<ICancelableEntityConnectionInterceptor> InternalDispatcher => this._internalDispatcher;

    public virtual bool Opening(
      EntityConnection entityConnection,
      DbInterceptionContext interceptionContext)
    {
      return this._internalDispatcher.Dispatch<bool>(true, (Func<bool, ICancelableEntityConnectionInterceptor, bool>) ((b, i) => i.ConnectionOpening(entityConnection, interceptionContext) & b));
    }
  }
}
