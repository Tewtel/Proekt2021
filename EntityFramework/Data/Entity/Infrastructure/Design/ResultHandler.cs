// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.ResultHandler
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Design
{
  /// <summary>
  ///     Used with <see cref="T:System.Data.Entity.Infrastructure.Design.Executor" /> to handle operation results.
  /// </summary>
  public class ResultHandler : HandlerBase, IResultHandler2, IResultHandler
  {
    private bool _hasResult;
    private object _result;
    private string _errorType;
    private string _errorMessage;
    private string _errorStackTrace;

    /// <summary>
    ///     Gets a value indicating whether a result is available.
    /// </summary>
    /// <value>A value indicating whether a result is available.</value>
    public virtual bool HasResult => this._hasResult;

    /// <summary>Gets the result.</summary>
    /// <value>The result.</value>
    public virtual object Result => this._result;

    /// <summary>Gets the type of the exception if any.</summary>
    /// <value>The exception type.</value>
    public virtual string ErrorType => this._errorType;

    /// <summary>Gets the error message if any.</summary>
    /// <value>The error message.</value>
    public virtual string ErrorMessage => this._errorMessage;

    /// <summary>Get the error stack trace if any.</summary>
    /// <value> The stack trace. </value>
    public virtual string ErrorStackTrace => this._errorStackTrace;

    /// <summary>Invoked when a result is available.</summary>
    /// <param name="value"> The result. </param>
    public virtual void SetResult(object value)
    {
      this._hasResult = true;
      this._result = value;
    }

    /// <summary>Invoked when an error occurs.</summary>
    /// <param name="type"> The exception type. </param>
    /// <param name="message"> The error message. </param>
    /// <param name="stackTrace"> The stack trace. </param>
    public virtual void SetError(string type, string message, string stackTrace)
    {
      this._errorType = type;
      this._errorMessage = message;
      this._errorStackTrace = stackTrace;
    }
  }
}
