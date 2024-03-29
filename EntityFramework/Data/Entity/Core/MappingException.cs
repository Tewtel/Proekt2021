﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.MappingException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;
using System.Runtime.Serialization;

namespace System.Data.Entity.Core
{
  /// <summary>
  /// Mapping exception class. Note that this class has state - so if you change even
  /// its internals, it can be a breaking change
  /// </summary>
  [Serializable]
  public sealed class MappingException : EntityException
  {
    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.MappingException" />.
    /// </summary>
    public MappingException()
      : base(Strings.Mapping_General_Error)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.MappingException" /> with a specialized error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public MappingException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.MappingException" /> that uses a specified error message and a reference to the inner exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public MappingException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    private MappingException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
