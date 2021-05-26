// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.Reporter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Design
{
  internal class Reporter
  {
    private readonly IReportHandler _handler;

    public Reporter(IReportHandler handler) => this._handler = handler;

    public void WriteError(string message) => this._handler?.OnError(message);

    public void WriteWarning(string message) => this._handler?.OnWarning(message);

    public void WriteInformation(string message) => this._handler?.OnInformation(message);

    public void WriteVerbose(string message) => this._handler?.OnVerbose(message);
  }
}
