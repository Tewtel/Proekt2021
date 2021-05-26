// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.IObjectSet`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Core.Objects
{
  /// <summary>
  /// Defines behavior for implementations of IQueryable that allow modifications to the membership of the resulting set.
  /// </summary>
  /// <typeparam name="TEntity"> Type of entities returned from the queryable. </typeparam>
  public interface IObjectSet<TEntity> : 
    IQueryable<TEntity>,
    IEnumerable<TEntity>,
    IEnumerable,
    IQueryable
    where TEntity : class
  {
    /// <summary>Notifies the set that an object that represents a new entity must be added to the set.</summary>
    /// <remarks>
    /// Depending on the implementation, the change to the set may not be visible in an enumeration of the set
    /// until changes to that set have been persisted in some manner.
    /// </remarks>
    /// <param name="entity">The new object to add to the set.</param>
    void AddObject(TEntity entity);

    /// <summary>Notifies the set that an object that represents an existing entity must be added to the set.</summary>
    /// <remarks>
    /// Depending on the implementation, the change to the set may not be visible in an enumeration of the set
    /// until changes to that set have been persisted in some manner.
    /// </remarks>
    /// <param name="entity">The existing object to add to the set.</param>
    void Attach(TEntity entity);

    /// <summary>Notifies the set that an object that represents an existing entity must be deleted from the set. </summary>
    /// <remarks>
    /// Depending on the implementation, the change to the set may not be visible in an enumeration of the set
    /// until changes to that set have been persisted in some manner.
    /// </remarks>
    /// <param name="entity">The existing object to delete from the set.</param>
    void DeleteObject(TEntity entity);

    /// <summary>Notifies the set that an object that represents an existing entity must be detached from the set.</summary>
    /// <remarks>
    /// Depending on the implementation, the change to the set may not be visible in an enumeration of the set
    /// until changes to that set have been persisted in some manner.
    /// </remarks>
    /// <param name="entity">The object to detach from the set.</param>
    void Detach(TEntity entity);
  }
}
