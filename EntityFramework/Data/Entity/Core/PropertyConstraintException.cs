// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.PropertyConstraintException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Runtime.Serialization;

namespace System.Data.Entity.Core
{
  /// <summary>
  /// Property constraint exception class. Note that this class has state - so if you change even
  /// its internals, it can be a breaking change
  /// </summary>
  [Serializable]
  public sealed class PropertyConstraintException : ConstraintException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.PropertyConstraintException" /> class with default message.
    /// </summary>
    public PropertyConstraintException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.PropertyConstraintException" /> class with supplied message.
    /// </summary>
    /// <param name="message">A localized error message.</param>
    public PropertyConstraintException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.PropertyConstraintException" /> class with supplied message and inner exception.
    /// </summary>
    /// <param name="message">A localized error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public PropertyConstraintException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.PropertyConstraintException" /> class.
    /// </summary>
    /// <param name="message">A localized error message.</param>
    /// <param name="propertyName">The name of the property.</param>
    public PropertyConstraintException(string message, string propertyName)
      : base(message)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      this.PropertyName = propertyName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.PropertyConstraintException" /> class.
    /// </summary>
    /// <param name="message">A localized error message.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="innerException">The inner exception.</param>
    public PropertyConstraintException(
      string message,
      string propertyName,
      Exception innerException)
      : base(message, innerException)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      this.PropertyName = propertyName;
    }

    private PropertyConstraintException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.PropertyName = info.GetString(nameof (PropertyName));
    }

    /// <summary>Gets the name of the property that violated the constraint.</summary>
    /// <returns>The name of the property that violated the constraint.</returns>
    public string PropertyName { get; }

    /// <summary>
    /// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
    /// </summary>
    /// <param name="info"> The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
    /// <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("PropertyName", (object) this.PropertyName);
    }
  }
}
