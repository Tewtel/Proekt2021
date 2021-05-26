// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.WrappedReportHandler
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Design
{
  internal class WrappedReportHandler : IReportHandler
  {
    private readonly IReportHandler _handler;

    public WrappedReportHandler(object handler)
    {
      if (handler == null)
        return;
      if (!(handler is HandlerBase handlerBase1))
        handlerBase1 = new ForwardingProxy<HandlerBase>(handler).GetTransparentProxy();
      HandlerBase handlerBase2 = handlerBase1;
      if (!(handler is IReportHandler reportHandler))
        reportHandler = handlerBase2.ImplementsContract(typeof (IReportHandler).FullName) ? new ForwardingProxy<IReportHandler>(handler).GetTransparentProxy() : (IReportHandler) null;
      this._handler = reportHandler;
    }

    public void OnError(string message) => this._handler?.OnError(message);

    public void OnInformation(string message) => this._handler?.OnInformation(message);

    public void OnVerbose(string message) => this._handler?.OnVerbose(message);

    public void OnWarning(string message) => this._handler?.OnWarning(message);
  }
}
