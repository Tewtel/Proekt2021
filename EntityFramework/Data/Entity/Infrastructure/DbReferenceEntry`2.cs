// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbReferenceEntry`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Internal;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Instances of this class are returned from the Reference method of
  /// <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> and allow operations such as loading to
  /// be performed on the an entity's reference navigation properties.
  /// </summary>
  /// <typeparam name="TEntity"> The type of the entity to which this property belongs. </typeparam>
  /// <typeparam name="TProperty"> The type of the property. </typeparam>
  public class DbReferenceEntry<TEntity, TProperty> : DbMemberEntry<TEntity, TProperty> where TEntity : class
  {
    private readonly InternalReferenceEntry _internalReferenceEntry;

    internal static DbReferenceEntry<TEntity, TProperty> Create(
      InternalReferenceEntry internalReferenceEntry)
    {
      return (DbReferenceEntry<TEntity, TProperty>) internalReferenceEntry.CreateDbMemberEntry<TEntity, TProperty>();
    }

    internal DbReferenceEntry(InternalReferenceEntry internalReferenceEntry) => this._internalReferenceEntry = internalReferenceEntry;

    /// <summary>Gets the property name.</summary>
    /// <value> The property name. </value>
    public override string Name => this._internalReferenceEntry.Name;

    /// <summary>
    /// Gets or sets the current value of the navigation property.  The current value is
    /// the entity that the navigation property references.
    /// </summary>
    /// <value> The current value. </value>
    public override TProperty CurrentValue
    {
      get => (TProperty) this._internalReferenceEntry.CurrentValue;
      set => this._internalReferenceEntry.CurrentValue = (object) value;
    }

    /// <summary>
    /// Loads the entity from the database.
    /// Note that if the entity already exists in the context, then it will not overwritten with values from the database.
    /// </summary>
    public void Load() => this._internalReferenceEntry.Load();

    /// <summary>
    /// Asynchronously loads the entity from the database.
    /// Note that if the entity already exists in the context, then it will not overwritten with values from the database.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task LoadAsync() => this.LoadAsync(CancellationToken.None);

    /// <summary>
    /// Asynchronously loads the entity from the database.
    /// Note that if the entity already exists in the context, then it will not overwritten with values from the database.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task LoadAsync(CancellationToken cancellationToken) => this._internalReferenceEntry.LoadAsync(cancellationToken);

    /// <summary>
    /// Gets or sets a value indicating whether the entity has been loaded from the database.
    /// </summary>
    /// <remarks>
    /// Loading the related entity from the database either using lazy-loading, as part of a query, or explicitly
    /// with one of the Load methods will set the IsLoaded flag to true.
    /// IsLoaded can be explicitly set to true to prevent the related entity from being lazy-loaded.
    /// Note that explict loading using one of the Load methods will load the related entity from the database
    /// regardless of whether or not IsLoaded is true.
    /// When a related entity is detached the IsLoaded flag is reset to false indicating that the related entity is
    /// no longer loaded.
    /// </remarks>
    /// <value>
    /// <c>true</c> if the entity is loaded or the IsLoaded has been explicitly set to true; otherwise, <c>false</c>.
    /// </value>
    public bool IsLoaded
    {
      get => this._internalReferenceEntry.IsLoaded;
      set => this._internalReferenceEntry.IsLoaded = value;
    }

    /// <summary>
    /// Returns the query that would be used to load this entity from the database.
    /// The returned query can be modified using LINQ to perform filtering or operations in the database.
    /// </summary>
    /// <returns> A query for the entity. </returns>
    public IQueryable<TProperty> Query() => (IQueryable<TProperty>) this._internalReferenceEntry.Query();

    /// <summary>
    /// Returns a new instance of the non-generic <see cref="T:System.Data.Entity.Infrastructure.DbReferenceEntry" /> class for
    /// the navigation property represented by this object.
    /// </summary>
    /// <param name="entry">The object representing the navigation property.</param>
    /// <returns> A non-generic version. </returns>
    public static implicit operator DbReferenceEntry(
      DbReferenceEntry<TEntity, TProperty> entry)
    {
      return DbReferenceEntry.Create(entry._internalReferenceEntry);
    }

    internal override InternalMemberEntry InternalMemberEntry => (InternalMemberEntry) this._internalReferenceEntry;

    /// <summary>
    /// The <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> to which this navigation property belongs.
    /// </summary>
    /// <value> An entry for the entity that owns this navigation property. </value>
    public override DbEntityEntry<TEntity> EntityEntry => new DbEntityEntry<TEntity>(this._internalReferenceEntry.InternalEntityEntry);
  }
}
