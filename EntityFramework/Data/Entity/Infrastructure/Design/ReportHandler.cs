// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.ReportHandler
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Design
{
  /// <summary>Used to handle reported design-time activity.</summary>
  public class ReportHandler : HandlerBase, IReportHandler
  {
    private readonly Action<string> _errorHandler;
    private readonly Action<string> _warningHandler;
    private readonly Action<string> _informationHandler;
    private readonly Action<string> _verboseHandler;

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.Data.Entity.Infrastructure.Design.ReportHandler" /> class.
    /// </summary>
    /// <param name="errorHandler"> A callback for <see cref="M:System.Data.Entity.Infrastructure.Design.ReportHandler.OnError(System.String)" />. </param>
    /// <param name="warningHandler"> A callback for <see cref="M:System.Data.Entity.Infrastructure.Design.ReportHandler.OnWarning(System.String)" />. </param>
    /// <param name="informationHandler"> A callback for <see cref="M:System.Data.Entity.Infrastructure.Design.ReportHandler.OnInformation(System.String)" />. </param>
    /// <param name="verboseHandler"> A callback for <see cref="M:System.Data.Entity.Infrastructure.Design.ReportHandler.OnVerbose(System.String)" />. </param>
    public ReportHandler(
      Action<string> errorHandler,
      Action<string> warningHandler,
      Action<string> informationHandler,
      Action<string> verboseHandler)
    {
      this._errorHandler = errorHandler;
      this._warningHandler = warningHandler;
      this._informationHandler = informationHandler;
      this._verboseHandler = verboseHandler;
    }

    /// <summary>Invoked when an error is reported.</summary>
    /// <param name="message">The message.</param>
    public virtual void OnError(string message)
    {
      Action<string> errorHandler = this._errorHandler;
      if (errorHandler == null)
        return;
      errorHandler(message);
    }

    /// <summary>Invoked when a warning is reported.</summary>
    /// <param name="message">The message.</param>
    public virtual void OnWarning(string message)
    {
      Action<string> warningHandler = this._warningHandler;
      if (warningHandler == null)
        return;
      warningHandler(message);
    }

    /// <summary>Invoked when information is reported.</summary>
    /// <param name="message">The message.</param>
    public virtual void OnInformation(string message)
    {
      Action<string> informationHandler = this._informationHandler;
      if (informationHandler == null)
        return;
      informationHandler(message);
    }

    /// <summary>Invoked when verbose information is reported.</summary>
    /// <param name="message">The message.</param>
    public virtual void OnVerbose(string message)
    {
      Action<string> verboseHandler = this._verboseHandler;
      if (verboseHandler == null)
        return;
      verboseHandler(message);
    }
  }
}
