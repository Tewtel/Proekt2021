// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbEntityEntry`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Internal;
using System.Data.Entity.Validation;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Instances of this class provide access to information about and control of entities that
  /// are being tracked by the <see cref="T:System.Data.Entity.DbContext" />.  Use the Entity or Entities methods of
  /// the context to obtain objects of this type.
  /// </summary>
  /// <typeparam name="TEntity"> The type of the entity. </typeparam>
  public class DbEntityEntry<TEntity> where TEntity : class
  {
    private readonly InternalEntityEntry _internalEntityEntry;

    internal DbEntityEntry(InternalEntityEntry internalEntityEntry) => this._internalEntityEntry = internalEntityEntry;

    /// <summary>Gets the entity.</summary>
    /// <value> The entity. </value>
    public TEntity Entity => (TEntity) this._internalEntityEntry.Entity;

    /// <summary>Gets or sets the state of the entity.</summary>
    /// <value> The state. </value>
    public EntityState State
    {
      get => this._internalEntityEntry.State;
      set => this._internalEntityEntry.State = value;
    }

    /// <summary>
    /// Gets the current property values for the tracked entity represented by this object.
    /// </summary>
    /// <value> The current values. </value>
    public DbPropertyValues CurrentValues => new DbPropertyValues(this._internalEntityEntry.CurrentValues);

    /// <summary>
    /// Gets the original property values for the tracked entity represented by this object.
    /// The original values are usually the entity's property values as they were when last queried from
    /// the database.
    /// </summary>
    /// <value> The original values. </value>
    public DbPropertyValues OriginalValues => new DbPropertyValues(this._internalEntityEntry.OriginalValues);

    /// <summary>
    /// Queries the database for copies of the values of the tracked entity as they currently exist in the database.
    /// Note that changing the values in the returned dictionary will not update the values in the database.
    /// If the entity is not found in the database then null is returned.
    /// </summary>
    /// <returns> The store values. </returns>
    public DbPropertyValues GetDatabaseValues()
    {
      InternalPropertyValues databaseValues = this._internalEntityEntry.GetDatabaseValues();
      return databaseValues != null ? new DbPropertyValues(databaseValues) : (DbPropertyValues) null;
    }

    /// <summary>
    /// Asynchronously queries the database for copies of the values of the tracked entity as they currently exist in the database.
    /// Note that changing the values in the returned dictionary will not update the values in the database.
    /// If the entity is not found in the database then null is returned.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the store values.
    /// </returns>
    public Task<DbPropertyValues> GetDatabaseValuesAsync() => this.GetDatabaseValuesAsync(CancellationToken.None);

    /// <summary>
    /// Asynchronously queries the database for copies of the values of the tracked entity as they currently exist in the database.
    /// Note that changing the values in the returned dictionary will not update the values in the database.
    /// If the entity is not found in the database then null is returned.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the store values.
    /// </returns>
    public async Task<DbPropertyValues> GetDatabaseValuesAsync(
      CancellationToken cancellationToken)
    {
      InternalPropertyValues internalValues = await this._internalEntityEntry.GetDatabaseValuesAsync(cancellationToken).WithCurrentCulture<InternalPropertyValues>();
      return internalValues == null ? (DbPropertyValues) null : new DbPropertyValues(internalValues);
    }

    /// <summary>
    /// Reloads the entity from the database overwriting any property values with values from the database.
    /// The entity will be in the Unchanged state after calling this method.
    /// </summary>
    public void Reload() => this._internalEntityEntry.Reload();

    /// <summary>
    /// Asynchronously reloads the entity from the database overwriting any property values with values from the database.
    /// The entity will be in the Unchanged state after calling this method.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task ReloadAsync() => this._internalEntityEntry.ReloadAsync(CancellationToken.None);

    /// <summary>
    /// Asynchronously reloads the entity from the database overwriting any property values with values from the database.
    /// The entity will be in the Unchanged state after calling this method.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task ReloadAsync(CancellationToken cancellationToken) => this._internalEntityEntry.ReloadAsync(cancellationToken);

    /// <summary>
    /// Gets an object that represents the reference (i.e. non-collection) navigation property from this
    /// entity to another entity.
    /// </summary>
    /// <param name="navigationProperty"> The name of the navigation property. </param>
    /// <returns> An object representing the navigation property. </returns>
    public DbReferenceEntry Reference(string navigationProperty)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(navigationProperty, nameof (navigationProperty));
      return DbReferenceEntry.Create(this._internalEntityEntry.Reference(navigationProperty));
    }

    /// <summary>
    /// Gets an object that represents the reference (i.e. non-collection) navigation property from this
    /// entity to another entity.
    /// </summary>
    /// <typeparam name="TProperty"> The type of the property. </typeparam>
    /// <param name="navigationProperty"> The name of the navigation property. </param>
    /// <returns> An object representing the navigation property. </returns>
    public DbReferenceEntry<TEntity, TProperty> Reference<TProperty>(
      string navigationProperty)
      where TProperty : class
    {
      System.Data.Entity.Utilities.Check.NotEmpty(navigationProperty, nameof (navigationProperty));
      return DbReferenceEntry<TEntity, TProperty>.Create(this._internalEntityEntry.Reference(navigationProperty, typeof (TProperty)));
    }

    /// <summary>
    /// Gets an object that represents the reference (i.e. non-collection) navigation property from this
    /// entity to another entity.
    /// </summary>
    /// <typeparam name="TProperty"> The type of the property. </typeparam>
    /// <param name="navigationProperty"> An expression representing the navigation property. </param>
    /// <returns> An object representing the navigation property. </returns>
    public DbReferenceEntry<TEntity, TProperty> Reference<TProperty>(
      Expression<Func<TEntity, TProperty>> navigationProperty)
      where TProperty : class
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntity, TProperty>>>(navigationProperty, nameof (navigationProperty));
      return DbReferenceEntry<TEntity, TProperty>.Create(this._internalEntityEntry.Reference(DbHelpers.ParsePropertySelector<TEntity, TProperty>(navigationProperty, nameof (Reference), nameof (navigationProperty)), typeof (TProperty)));
    }

    /// <summary>
    /// Gets an object that represents the collection navigation property from this
    /// entity to a collection of related entities.
    /// </summary>
    /// <param name="navigationProperty"> The name of the navigation property. </param>
    /// <returns> An object representing the navigation property. </returns>
    public DbCollectionEntry Collection(string navigationProperty)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(navigationProperty, nameof (navigationProperty));
      return DbCollectionEntry.Create(this._internalEntityEntry.Collection(navigationProperty));
    }

    /// <summary>
    /// Gets an object that represents the collection navigation property from this
    /// entity to a collection of related entities.
    /// </summary>
    /// <typeparam name="TElement"> The type of elements in the collection. </typeparam>
    /// <param name="navigationProperty"> The name of the navigation property. </param>
    /// <returns> An object representing the navigation property. </returns>
    public DbCollectionEntry<TEntity, TElement> Collection<TElement>(
      string navigationProperty)
      where TElement : class
    {
      System.Data.Entity.Utilities.Check.NotEmpty(navigationProperty, nameof (navigationProperty));
      return DbCollectionEntry<TEntity, TElement>.Create(this._internalEntityEntry.Collection(navigationProperty, typeof (TElement)));
    }

    /// <summary>
    /// Gets an object that represents the collection navigation property from this
    /// entity to a collection of related entities.
    /// </summary>
    /// <typeparam name="TElement"> The type of elements in the collection. </typeparam>
    /// <param name="navigationProperty"> An expression representing the navigation property. </param>
    /// <returns> An object representing the navigation property. </returns>
    public DbCollectionEntry<TEntity, TElement> Collection<TElement>(
      Expression<Func<TEntity, ICollection<TElement>>> navigationProperty)
      where TElement : class
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntity, ICollection<TElement>>>>(navigationProperty, nameof (navigationProperty));
      return this.Collection<TElement>(DbHelpers.ParsePropertySelector<TEntity, ICollection<TElement>>(navigationProperty, nameof (Collection), nameof (navigationProperty)));
    }

    /// <summary>
    /// Gets an object that represents a scalar or complex property of this entity.
    /// </summary>
    /// <param name="propertyName"> The name of the property. </param>
    /// <returns> An object representing the property. </returns>
    public DbPropertyEntry Property(string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbPropertyEntry.Create(this._internalEntityEntry.Property(propertyName));
    }

    /// <summary>
    /// Gets an object that represents a scalar or complex property of this entity.
    /// </summary>
    /// <typeparam name="TProperty"> The type of the property. </typeparam>
    /// <param name="propertyName"> The name of the property. </param>
    /// <returns> An object representing the property. </returns>
    public DbPropertyEntry<TEntity, TProperty> Property<TProperty>(
      string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbPropertyEntry<TEntity, TProperty>.Create(this._internalEntityEntry.Property(propertyName, typeof (TProperty)));
    }

    /// <summary>
    /// Gets an object that represents a scalar or complex property of this entity.
    /// </summary>
    /// <typeparam name="TProperty"> The type of the property. </typeparam>
    /// <param name="property"> An expression representing the property. </param>
    /// <returns> An object representing the property. </returns>
    public DbPropertyEntry<TEntity, TProperty> Property<TProperty>(
      Expression<Func<TEntity, TProperty>> property)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntity, TProperty>>>(property, nameof (property));
      return this.Property<TProperty>(DbHelpers.ParsePropertySelector<TEntity, TProperty>(property, nameof (Property), nameof (property)));
    }

    /// <summary>
    /// Gets an object that represents a complex property of this entity.
    /// </summary>
    /// <param name="propertyName"> The name of the complex property. </param>
    /// <returns> An object representing the complex property. </returns>
    public DbComplexPropertyEntry ComplexProperty(string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbComplexPropertyEntry.Create(this._internalEntityEntry.Property(propertyName, requireComplex: true));
    }

    /// <summary>
    /// Gets an object that represents a complex property of this entity.
    /// </summary>
    /// <typeparam name="TComplexProperty"> The type of the complex property. </typeparam>
    /// <param name="propertyName"> The name of the complex property. </param>
    /// <returns> An object representing the complex property. </returns>
    public DbComplexPropertyEntry<TEntity, TComplexProperty> ComplexProperty<TComplexProperty>(
      string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbComplexPropertyEntry<TEntity, TComplexProperty>.Create(this._internalEntityEntry.Property(propertyName, typeof (TComplexProperty), true));
    }

    /// <summary>
    /// Gets an object that represents a complex property of this entity.
    /// </summary>
    /// <typeparam name="TComplexProperty"> The type of the complex property. </typeparam>
    /// <param name="property"> An expression representing the complex property. </param>
    /// <returns> An object representing the complex property. </returns>
    public DbComplexPropertyEntry<TEntity, TComplexProperty> ComplexProperty<TComplexProperty>(
      Expression<Func<TEntity, TComplexProperty>> property)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntity, TComplexProperty>>>(property, nameof (property));
      return this.ComplexProperty<TComplexProperty>(DbHelpers.ParsePropertySelector<TEntity, TComplexProperty>(property, "Property", nameof (property)));
    }

    /// <summary>
    /// Gets an object that represents a member of the entity.  The runtime type of the returned object will
    /// vary depending on what kind of member is asked for.  The currently supported member types and their return
    /// types are:
    /// Reference navigation property: <see cref="T:System.Data.Entity.Infrastructure.DbReferenceEntry" />.
    /// Collection navigation property: <see cref="T:System.Data.Entity.Infrastructure.DbCollectionEntry" />.
    /// Primitive/scalar property: <see cref="T:System.Data.Entity.Infrastructure.DbPropertyEntry" />.
    /// Complex property: <see cref="T:System.Data.Entity.Infrastructure.DbComplexPropertyEntry" />.
    /// </summary>
    /// <param name="propertyName"> The name of the member. </param>
    /// <returns> An object representing the member. </returns>
    public DbMemberEntry Member(string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbMemberEntry.Create(this._internalEntityEntry.Member(propertyName));
    }

    /// <summary>
    /// Gets an object that represents a member of the entity.  The runtime type of the returned object will
    /// vary depending on what kind of member is asked for.  The currently supported member types and their return
    /// types are:
    /// Reference navigation property: <see cref="T:System.Data.Entity.Infrastructure.DbReferenceEntry`2" />.
    /// Collection navigation property: <see cref="T:System.Data.Entity.Infrastructure.DbCollectionEntry`2" />.
    /// Primitive/scalar property: <see cref="T:System.Data.Entity.Infrastructure.DbPropertyEntry`2" />.
    /// Complex property: <see cref="T:System.Data.Entity.Infrastructure.DbComplexPropertyEntry`2" />.
    /// </summary>
    /// <typeparam name="TMember"> The type of the member. </typeparam>
    /// <param name="propertyName"> The name of the member. </param>
    /// <returns> An object representing the member. </returns>
    public DbMemberEntry<TEntity, TMember> Member<TMember>(string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return this._internalEntityEntry.Member(propertyName, typeof (TMember)).CreateDbMemberEntry<TEntity, TMember>();
    }

    /// <summary>
    /// Returns a new instance of the non-generic <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry" /> class for
    /// the tracked entity represented by this object.
    /// </summary>
    /// <param name="entry">The object representing the tracked entity.</param>
    /// <returns> A non-generic version. </returns>
    public static implicit operator DbEntityEntry(DbEntityEntry<TEntity> entry) => new DbEntityEntry(entry._internalEntityEntry);

    /// <summary>
    /// Validates this <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> instance and returns validation result.
    /// </summary>
    /// <returns>
    /// Entity validation result. Possibly null if
    /// DbContext.ValidateEntity(DbEntityEntry, IDictionary{object, object})
    /// method is overridden.
    /// </returns>
    public DbEntityValidationResult GetValidationResult() => this._internalEntityEntry.InternalContext.Owner.CallValidateEntity((DbEntityEntry) this);

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object" /> is equal to this instance.
    /// Two <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> instances are considered equal if they are both entries for
    /// the same entity on the same <see cref="T:System.Data.Entity.DbContext" />.
    /// </summary>
    /// <param name="obj">
    /// The <see cref="T:System.Object" /> to compare with this instance.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <c>false</c> .
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => obj != null && !(obj.GetType() != typeof (DbEntityEntry<TEntity>)) && this.Equals((DbEntityEntry<TEntity>) obj);

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> is equal to this instance.
    /// Two <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> instances are considered equal if they are both entries for
    /// the same entity on the same <see cref="T:System.Data.Entity.DbContext" />.
    /// </summary>
    /// <param name="other">
    /// The <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> to compare with this instance.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> is equal to this instance; otherwise, <c>false</c> .
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool Equals(DbEntityEntry<TEntity> other)
    {
      if (this == other)
        return true;
      return other != null && this._internalEntityEntry.Equals(other._internalEntityEntry);
    }

    /// <summary>Returns a hash code for this instance.</summary>
    /// <returns> A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => this._internalEntityEntry.GetHashCode();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the current instance.
    /// </summary>
    /// <returns>The exact runtime type of the current instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
