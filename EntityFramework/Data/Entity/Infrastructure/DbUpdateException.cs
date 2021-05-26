// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbUpdateException
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Internal;
using System.Linq;
using System.Runtime.Serialization;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Exception thrown by <see cref="T:System.Data.Entity.DbContext" /> when the saving of changes to the database fails.
  /// Note that state entries referenced by this exception are not serialized due to security and accesses to the
  /// state entries after serialization will return null.
  /// </summary>
  [Serializable]
  public class DbUpdateException : DataException
  {
    [NonSerialized]
    private readonly InternalContext _internalContext;
    private readonly bool _involvesIndependentAssociations;

    internal DbUpdateException(
      InternalContext internalContext,
      UpdateException innerException,
      bool involvesIndependentAssociations)
      : base(involvesIndependentAssociations ? System.Data.Entity.Resources.Strings.DbContext_IndependentAssociationUpdateException : innerException.Message, (Exception) innerException)
    {
      this._internalContext = internalContext;
      this._involvesIndependentAssociations = involvesIndependentAssociations;
    }

    /// <summary>
    /// Gets <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry" /> objects that represents the entities that could not
    /// be saved to the database.
    /// </summary>
    /// <returns> The entries representing the entities that could not be saved. </returns>
    public IEnumerable<DbEntityEntry> Entries
    {
      get
      {
        UpdateException innerException = this.InnerException as UpdateException;
        return this._involvesIndependentAssociations || this._internalContext == null || (innerException == null || innerException.StateEntries == null) ? Enumerable.Empty<DbEntityEntry>() : innerException.StateEntries.Select<ObjectStateEntry, DbEntityEntry>((Func<ObjectStateEntry, DbEntityEntry>) (e => new DbEntityEntry(new InternalEntityEntry(this._internalContext, (System.Data.Entity.Internal.IEntityStateEntry) new StateEntryAdapter(e)))));
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Infrastructure.DbUpdateException" /> class.
    /// </summary>
    public DbUpdateException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Infrastructure.DbUpdateException" /> class.
    /// </summary>
    /// <param name="message"> The message. </param>
    public DbUpdateException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Infrastructure.DbUpdateException" /> class.
    /// </summary>
    /// <param name="message"> The message. </param>
    /// <param name="innerException"> The inner exception. </param>
    public DbUpdateException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the DbUpdateException class with the specified serialization information and context.
    /// </summary>
    /// <param name="info"> The data necessary to serialize or deserialize an object. </param>
    /// <param name="context"> Description of the source and destination of the specified serialized stream. </param>
    protected DbUpdateException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._involvesIndependentAssociations = info.GetBoolean("InvolvesIndependentAssociations");
    }

    /// <summary>
    /// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
    /// </summary>
    /// <param name="info"> The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
    /// <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("InvolvesIndependentAssociations", this._involvesIndependentAssociations);
    }
  }
}
