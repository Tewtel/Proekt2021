﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.RetryLimitExceededException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core;
using System.Runtime.Serialization;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// The exception that is thrown when the action failed again after being retried the configured number of times.
  /// </summary>
  [Serializable]
  public sealed class RetryLimitExceededException : EntityException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Infrastructure.RetryLimitExceededException" /> class with no error message.
    /// </summary>
    public RetryLimitExceededException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Infrastructure.RetryLimitExceededException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public RetryLimitExceededException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Infrastructure.RetryLimitExceededException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public RetryLimitExceededException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    private RetryLimitExceededException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
