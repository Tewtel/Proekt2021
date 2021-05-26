// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Validation.DbEntityValidationException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace System.Data.Entity.Validation
{
  /// <summary>
  /// Exception thrown from <see cref="M:System.Data.Entity.DbContext.SaveChanges" /> when validating entities fails.
  /// </summary>
  [Serializable]
  public class DbEntityValidationException : DataException
  {
    private IList<DbEntityValidationResult> _entityValidationResults;

    /// <summary>
    /// Initializes a new instance of DbEntityValidationException.
    /// </summary>
    public DbEntityValidationException()
      : this(System.Data.Entity.Resources.Strings.DbEntityValidationException_ValidationFailed)
    {
    }

    /// <summary>
    /// Initializes a new instance of DbEntityValidationException.
    /// </summary>
    /// <param name="message"> The exception message. </param>
    public DbEntityValidationException(string message)
      : this(message, Enumerable.Empty<DbEntityValidationResult>())
    {
    }

    /// <summary>
    /// Initializes a new instance of DbEntityValidationException.
    /// </summary>
    /// <param name="message"> The exception message. </param>
    /// <param name="entityValidationResults"> Validation results. </param>
    public DbEntityValidationException(
      string message,
      IEnumerable<DbEntityValidationResult> entityValidationResults)
      : base(message)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<DbEntityValidationResult>>(entityValidationResults, nameof (entityValidationResults));
      this.InititializeValidationResults(entityValidationResults);
    }

    /// <summary>
    /// Initializes a new instance of DbEntityValidationException.
    /// </summary>
    /// <param name="message"> The exception message. </param>
    /// <param name="innerException"> The inner exception. </param>
    public DbEntityValidationException(string message, Exception innerException)
      : this(message, Enumerable.Empty<DbEntityValidationResult>(), innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of DbEntityValidationException.
    /// </summary>
    /// <param name="message"> The exception message. </param>
    /// <param name="entityValidationResults"> Validation results. </param>
    /// <param name="innerException"> The inner exception. </param>
    public DbEntityValidationException(
      string message,
      IEnumerable<DbEntityValidationResult> entityValidationResults,
      Exception innerException)
      : base(message, innerException)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<DbEntityValidationResult>>(entityValidationResults, nameof (entityValidationResults));
      this.InititializeValidationResults(entityValidationResults);
    }

    /// <summary>
    /// Initializes a new instance of the DbEntityValidationException class with the specified serialization information and context.
    /// </summary>
    /// <param name="info"> The data necessary to serialize or deserialize an object. </param>
    /// <param name="context"> Description of the source and destination of the specified serialized stream. </param>
    protected DbEntityValidationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._entityValidationResults = (IList<DbEntityValidationResult>) info.GetValue(nameof (EntityValidationErrors), typeof (List<DbEntityValidationResult>));
    }

    /// <summary>Validation results.</summary>
    public IEnumerable<DbEntityValidationResult> EntityValidationErrors => (IEnumerable<DbEntityValidationResult>) this._entityValidationResults;

    /// <summary>
    /// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
    /// </summary>
    /// <param name="info"> The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
    /// <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("EntityValidationErrors", (object) this._entityValidationResults);
    }

    private void InititializeValidationResults(
      IEnumerable<DbEntityValidationResult> entityValidationResults)
    {
      this._entityValidationResults = entityValidationResults == null ? (IList<DbEntityValidationResult>) new List<DbEntityValidationResult>() : (IList<DbEntityValidationResult>) entityValidationResults.ToList<DbEntityValidationResult>();
    }
  }
}
