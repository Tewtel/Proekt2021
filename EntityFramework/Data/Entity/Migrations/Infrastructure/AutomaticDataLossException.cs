// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Infrastructure.AutomaticDataLossException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Runtime.Serialization;

namespace System.Data.Entity.Migrations.Infrastructure
{
  /// <summary>
  /// Represents an error that occurs when an automatic migration would result in data loss.
  /// </summary>
  [Serializable]
  public sealed class AutomaticDataLossException : MigrationsException
  {
    /// <summary>
    /// Initializes a new instance of the AutomaticDataLossException class.
    /// </summary>
    public AutomaticDataLossException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the AutomaticDataLossException class.
    /// </summary>
    /// <param name="message"> The message that describes the error. </param>
    public AutomaticDataLossException(string message)
      : base(message)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(message, nameof (message));
    }

    /// <summary>
    /// Initializes a new instance of the MigrationsException class.
    /// </summary>
    /// <param name="message"> The message that describes the error. </param>
    /// <param name="innerException"> The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. </param>
    public AutomaticDataLossException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    private AutomaticDataLossException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
