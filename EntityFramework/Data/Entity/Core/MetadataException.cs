﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.MetadataException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;
using System.Runtime.Serialization;

namespace System.Data.Entity.Core
{
  /// <summary>metadata exception class</summary>
  [Serializable]
  public sealed class MetadataException : EntityException
  {
    private const int HResultMetadata = -2146232007;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.MetadataException" /> class with a default message.
    /// </summary>
    public MetadataException()
      : base(Strings.Metadata_General_Error)
    {
      this.HResult = -2146232007;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.MetadataException" /> class with the specified message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public MetadataException(string message)
      : base(message)
    {
      this.HResult = -2146232007;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.MetadataException" /> class with the specified message and inner exception.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">
    /// The exception that is the cause of this <see cref="T:System.Data.Entity.Core.MetadataException" />.
    /// </param>
    public MetadataException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.HResult = -2146232007;
    }

    private MetadataException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
