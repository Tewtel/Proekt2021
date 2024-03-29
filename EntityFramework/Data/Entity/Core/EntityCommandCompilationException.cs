﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntityCommandCompilationException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Runtime.Serialization;

namespace System.Data.Entity.Core
{
  /// <summary>
  /// Represents a failure while trying to prepare or execute a CommandCompilation
  /// This exception is intended to provide a common exception that people can catch to
  /// hold provider exceptions (SqlException, OracleException) when using the EntityCommand
  /// to execute statements.
  /// </summary>
  [Serializable]
  public sealed class EntityCommandCompilationException : EntityException
  {
    private const int HResultCommandCompilation = -2146232005;

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.EntityCommandCompilationException" />.
    /// </summary>
    public EntityCommandCompilationException() => this.HResult = -2146232005;

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.EntityCommandCompilationException" />.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EntityCommandCompilationException(string message)
      : base(message)
    {
      this.HResult = -2146232005;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.EntityCommandCompilationException" />.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that caused the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public EntityCommandCompilationException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.HResult = -2146232005;
    }

    private EntityCommandCompilationException(
      SerializationInfo serializationInfo,
      StreamingContext streamingContext)
      : base(serializationInfo, streamingContext)
    {
      this.HResult = -2146232005;
    }
  }
}
