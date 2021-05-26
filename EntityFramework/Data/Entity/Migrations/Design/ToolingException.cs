// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Design.ToolingException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Runtime.Serialization;

namespace System.Data.Entity.Migrations.Design
{
  /// <summary>
  ///     Represents an exception that occurred while running an operation in another AppDomain in the
  ///     <see cref="T:System.Data.Entity.Migrations.Design.ToolingFacade" />.
  /// </summary>
  [Obsolete("Use System.Data.Entity.Infrastructure.Design.IErrorHandler instead.")]
  [Serializable]
  public class ToolingException : Exception
  {
    [NonSerialized]
    private ToolingException.ToolingExceptionState _state;

    /// <summary>
    ///     Initializes a new instance of the ToolingException class.
    /// </summary>
    public ToolingException() => this.SubscribeToSerializeObjectState();

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.
    /// </summary>
    /// <param name="message"> The message that describes the error. </param>
    public ToolingException(string message)
      : base(message)
    {
      this.SubscribeToSerializeObjectState();
    }

    /// <summary>
    ///     Initializes a new instance of the ToolingException class.
    /// </summary>
    /// <param name="message"> Error that explains the reason for the exception. </param>
    /// <param name="innerType"> The type of the exception that was thrown. </param>
    /// <param name="innerStackTrace"> The stack trace of the exception that was thrown. </param>
    public ToolingException(string message, string innerType, string innerStackTrace)
      : base(message)
    {
      this._state.InnerType = innerType;
      this._state.InnerStackTrace = innerStackTrace;
      this.SubscribeToSerializeObjectState();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message"> The error message that explains the reason for the exception. </param>
    /// <param name="innerException"> The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. </param>
    public ToolingException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.SubscribeToSerializeObjectState();
    }

    /// <summary>Gets the type of the exception that was thrown.</summary>
    public string InnerType => this._state.InnerType;

    /// <summary>
    ///     Gets the stack trace of the exception that was thrown.
    /// </summary>
    public string InnerStackTrace => this._state.InnerStackTrace;

    private void SubscribeToSerializeObjectState() => this.SerializeObjectState += (EventHandler<SafeSerializationEventArgs>) ((_, a) => a.AddSerializedState((ISafeSerializationData) this._state));

    [Serializable]
    private struct ToolingExceptionState : ISafeSerializationData
    {
      public string InnerType { get; set; }

      public string InnerStackTrace { get; set; }

      public void CompleteDeserialization(object deserialized) => ((ToolingException) deserialized)._state = this;
    }
  }
}
