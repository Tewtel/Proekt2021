﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.ModelValidationException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Runtime.Serialization;

namespace System.Data.Entity.ModelConfiguration
{
  /// <summary>
  /// Exception thrown by <see cref="T:System.Data.Entity.DbModelBuilder" /> during model creation when an invalid model is generated.
  /// </summary>
  [Serializable]
  public class ModelValidationException : Exception
  {
    /// <summary>
    /// Initializes a new instance of ModelValidationException
    /// </summary>
    public ModelValidationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of ModelValidationException
    /// </summary>
    /// <param name="message"> The exception message. </param>
    public ModelValidationException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of ModelValidationException
    /// </summary>
    /// <param name="message"> The exception message. </param>
    /// <param name="innerException"> The inner exception. </param>
    public ModelValidationException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    internal ModelValidationException(
      IEnumerable<DataModelErrorEventArgs> validationErrors)
      : base(validationErrors.ToErrorMessage())
    {
    }

    /// <summary>Initializes a new instance of <see cref="T:System.Data.Entity.ModelConfiguration.ModelValidationException" /> class serialization info and streaming context.</summary>
    /// <param name="info">The serialization info.</param>
    /// <param name="context">The streaming context.</param>
    protected ModelValidationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
