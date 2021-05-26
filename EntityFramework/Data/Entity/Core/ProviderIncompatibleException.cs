﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.ProviderIncompatibleException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Runtime.Serialization;

namespace System.Data.Entity.Core
{
  /// <summary>
  /// This exception is thrown when the store provider exhibits a behavior incompatible with the entity client provider
  /// </summary>
  [Serializable]
  public sealed class ProviderIncompatibleException : EntityException
  {
    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.ProviderIncompatibleException" />.
    /// </summary>
    public ProviderIncompatibleException()
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.ProviderIncompatibleException" /> with a specialized error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ProviderIncompatibleException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Core.ProviderIncompatibleException" /> that uses a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    public ProviderIncompatibleException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    private ProviderIncompatibleException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
