// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbComplexPropertyEntry`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Internal;
using System.Linq.Expressions;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Instances of this class are returned from the ComplexProperty method of
  /// <see cref="T:System.Data.Entity.Infrastructure.DbEntityEntry`1" /> and allow access to the state of a complex property.
  /// </summary>
  /// <typeparam name="TEntity"> The type of the entity to which this property belongs. </typeparam>
  /// <typeparam name="TComplexProperty"> The type of the property. </typeparam>
  public class DbComplexPropertyEntry<TEntity, TComplexProperty> : 
    DbPropertyEntry<TEntity, TComplexProperty>
    where TEntity : class
  {
    internal static DbComplexPropertyEntry<TEntity, TComplexProperty> Create(
      InternalPropertyEntry internalPropertyEntry)
    {
      return (DbComplexPropertyEntry<TEntity, TComplexProperty>) internalPropertyEntry.CreateDbMemberEntry<TEntity, TComplexProperty>();
    }

    internal DbComplexPropertyEntry(InternalPropertyEntry internalPropertyEntry)
      : base(internalPropertyEntry)
    {
    }

    /// <summary>
    /// Returns a new instance of the non-generic <see cref="T:System.Data.Entity.Infrastructure.DbComplexPropertyEntry" /> class for
    /// the property represented by this object.
    /// </summary>
    /// <param name="entry">The object representing the property.</param>
    /// <returns> A non-generic version. </returns>
    public static implicit operator DbComplexPropertyEntry(
      DbComplexPropertyEntry<TEntity, TComplexProperty> entry)
    {
      return DbComplexPropertyEntry.Create(entry.InternalPropertyEntry);
    }

    /// <summary>
    /// Gets an object that represents a nested property of this property.
    /// This method can be used for both scalar or complex properties.
    /// </summary>
    /// <param name="propertyName"> The name of the nested property. </param>
    /// <returns> An object representing the nested property. </returns>
    public DbPropertyEntry Property(string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbPropertyEntry.Create(this.InternalPropertyEntry.Property(propertyName));
    }

    /// <summary>
    /// Gets an object that represents a nested property of this property.
    /// This method can be used for both scalar or complex properties.
    /// </summary>
    /// <typeparam name="TNestedProperty"> The type of the nested property. </typeparam>
    /// <param name="propertyName"> The name of the nested property. </param>
    /// <returns> An object representing the nested property. </returns>
    public DbPropertyEntry<TEntity, TNestedProperty> Property<TNestedProperty>(
      string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbPropertyEntry<TEntity, TNestedProperty>.Create(this.InternalPropertyEntry.Property(propertyName, typeof (TNestedProperty)));
    }

    /// <summary>
    /// Gets an object that represents a nested property of this property.
    /// This method can be used for both scalar or complex properties.
    /// </summary>
    /// <typeparam name="TNestedProperty"> The type of the nested property. </typeparam>
    /// <param name="property"> An expression representing the nested property. </param>
    /// <returns> An object representing the nested property. </returns>
    public DbPropertyEntry<TEntity, TNestedProperty> Property<TNestedProperty>(
      Expression<Func<TComplexProperty, TNestedProperty>> property)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TComplexProperty, TNestedProperty>>>(property, nameof (property));
      return this.Property<TNestedProperty>(DbHelpers.ParsePropertySelector<TComplexProperty, TNestedProperty>(property, nameof (Property), nameof (property)));
    }

    /// <summary>
    /// Gets an object that represents a nested complex property of this property.
    /// </summary>
    /// <param name="propertyName"> The name of the nested property. </param>
    /// <returns> An object representing the nested property. </returns>
    public DbComplexPropertyEntry ComplexProperty(string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbComplexPropertyEntry.Create(this.InternalPropertyEntry.Property(propertyName, requireComplex: true));
    }

    /// <summary>
    /// Gets an object that represents a nested complex property of this property.
    /// </summary>
    /// <typeparam name="TNestedComplexProperty"> The type of the nested property. </typeparam>
    /// <param name="propertyName"> The name of the nested property. </param>
    /// <returns> An object representing the nested property. </returns>
    public DbComplexPropertyEntry<TEntity, TNestedComplexProperty> ComplexProperty<TNestedComplexProperty>(
      string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbComplexPropertyEntry<TEntity, TNestedComplexProperty>.Create(this.InternalPropertyEntry.Property(propertyName, typeof (TNestedComplexProperty), true));
    }

    /// <summary>
    /// Gets an object that represents a nested complex property of this property.
    /// </summary>
    /// <typeparam name="TNestedComplexProperty"> The type of the nested property. </typeparam>
    /// <param name="property"> An expression representing the nested property. </param>
    /// <returns> An object representing the nested property. </returns>
    public DbComplexPropertyEntry<TEntity, TNestedComplexProperty> ComplexProperty<TNestedComplexProperty>(
      Expression<Func<TComplexProperty, TNestedComplexProperty>> property)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TComplexProperty, TNestedComplexProperty>>>(property, nameof (property));
      return this.ComplexProperty<TNestedComplexProperty>(DbHelpers.ParsePropertySelector<TComplexProperty, TNestedComplexProperty>(property, "Property", nameof (property)));
    }
  }
}
