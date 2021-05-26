// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbCollectionEntry`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Internal;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Instances of this class are returned from the Collection method of
  /// <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> and allow operations such as loading to
  /// be performed on the an entity's collection navigation properties.
  /// </summary>
  /// <typeparam name="TEntity"> The type of the entity to which this property belongs. </typeparam>
  /// <typeparam name="TElement"> The type of the element in the collection of entities. </typeparam>
  public class DbCollectionEntry<TEntity, TElement> : DbMemberEntry<TEntity, ICollection<TElement>>
    where TEntity : class
  {
    private readonly InternalCollectionEntry _internalCollectionEntry;

    internal static DbCollectionEntry<TEntity, TElement> Create(
      InternalCollectionEntry internalCollectionEntry)
    {
      return internalCollectionEntry.CreateDbCollectionEntry<TEntity, TElement>();
    }

    internal DbCollectionEntry(InternalCollectionEntry internalCollectionEntry) => this._internalCollectionEntry = internalCollectionEntry;

    /// <summary>Gets the property name.</summary>
    /// <value> The property name. </value>
    public override string Name => this._internalCollectionEntry.Name;

    /// <summary>
    /// Gets or sets the current value of the navigation property.  The current value is
    /// the entity that the navigation property references.
    /// </summary>
    /// <value> The current value. </value>
    public override ICollection<TElement> CurrentValue
    {
      get => (ICollection<TElement>) this._internalCollectionEntry.CurrentValue;
      set => this._internalCollectionEntry.CurrentValue = (object) value;
    }

    /// <summary>
    /// Loads the collection of entities from the database.
    /// Note that entities that already exist in the context are not overwritten with values from the database.
    /// </summary>
    public void Load() => this._internalCollectionEntry.Load();

    /// <summary>
    /// Asynchronously loads the collection of entities from the database.
    /// Note that entities that already exist in the context are not overwritten with values from the database.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task LoadAsync() => this.LoadAsync(CancellationToken.None);

    /// <summary>
    /// Asynchronously loads the collection of entities from the database.
    /// Note that entities that already exist in the context are not overwritten with values from the database.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task LoadAsync(CancellationToken cancellationToken) => this._internalCollectionEntry.LoadAsync(cancellationToken);

    /// <summary>
    /// Gets or sets a value indicating whether all entities of this collection have been loaded from the database.
    /// </summary>
    /// <remarks>
    /// Loading the related entities from the database either using lazy-loading, as part of a query, or explicitly
    /// with one of the Load methods will set the IsLoaded flag to true.
    /// IsLoaded can be explicitly set to true to prevent the related entities of this collection from being lazy-loaded.
    /// This can be useful if the application has caused a subset of related entities to be loaded into this collection
    /// and wants to prevent any other entities from being loaded automatically.
    /// Note that explict loading using one of the Load methods will load all related entities from the database
    /// regardless of whether or not IsLoaded is true.
    /// When any related entity in the collection is detached the IsLoaded flag is reset to false indicating that the
    /// not all related entities are now loaded.
    /// </remarks>
    /// <value>
    /// <c>true</c> if all the related entities are loaded or the IsLoaded has been explicitly set to true; otherwise, <c>false</c>.
    /// </value>
    public bool IsLoaded
    {
      get => this._internalCollectionEntry.IsLoaded;
      set => this._internalCollectionEntry.IsLoaded = value;
    }

    /// <summary>
    /// Returns the query that would be used to load this collection from the database.
    /// The returned query can be modified using LINQ to perform filtering or operations in the database, such
    /// as counting the number of entities in the collection in the database without actually loading them.
    /// </summary>
    /// <returns> A query for the collection. </returns>
    public IQueryable<TElement> Query() => (IQueryable<TElement>) this._internalCollectionEntry.Query();

    /// <summary>
    /// Returns a new instance of the non-generic <see cref="T:System.Data.Entity.Infrastructure.DbCollectionEntry" /> class for
    /// the navigation property represented by this object.
    /// </summary>
    /// <param name="entry">The object representing the navigation property.</param>
    /// <returns> A non-generic version. </returns>
    public static implicit operator DbCollectionEntry(
      DbCollectionEntry<TEntity, TElement> entry)
    {
      return DbCollectionEntry.Create(entry._internalCollectionEntry);
    }

    internal override InternalMemberEntry InternalMemberEntry => (InternalMemberEntry) this._internalCollectionEntry;

    /// <summary>
    /// The <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> to which this navigation property belongs.
    /// </summary>
    /// <value> An entry for the entity that owns this navigation property. </value>
    public override DbEntityEntry<TEntity> EntityEntry => new DbEntityEntry<TEntity>(this._internalCollectionEntry.InternalEntityEntry);
  }
}
