// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.WrappedResultHandler
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Design
{
  internal class WrappedResultHandler : IResultHandler
  {
    private readonly IResultHandler _handler;
    private readonly IResultHandler2 _handler2;

    public WrappedResultHandler(object handler)
    {
      if (!(handler is HandlerBase handlerBase1))
        handlerBase1 = new ForwardingProxy<HandlerBase>(handler).GetTransparentProxy();
      HandlerBase handlerBase2 = handlerBase1;
      if (!(handler is IResultHandler resultHandler))
        resultHandler = handlerBase2.ImplementsContract(typeof (IResultHandler).FullName) ? new ForwardingProxy<IResultHandler>(handler).GetTransparentProxy() : (IResultHandler) null;
      this._handler = resultHandler;
      if (!(handler is IResultHandler2 resultHandler2))
        resultHandler2 = handlerBase2.ImplementsContract(typeof (IResultHandler2).FullName) ? new ForwardingProxy<IResultHandler2>(handler).GetTransparentProxy() : (IResultHandler2) null;
      this._handler2 = resultHandler2;
    }

    public void SetResult(object value)
    {
      if (this._handler == null)
        return;
      this._handler.SetResult(value);
    }

    public bool SetError(string type, string message, string stackTrace)
    {
      if (this._handler2 == null)
        return false;
      this._handler2.SetError(type, message, stackTrace);
      return true;
    }
  }
}
