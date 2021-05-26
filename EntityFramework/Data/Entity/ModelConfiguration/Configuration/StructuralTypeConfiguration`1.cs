// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.StructuralTypeConfiguration`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Hierarchy;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.Spatial;
using System.Linq.Expressions;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Allows configuration to be performed for a type in a model.
  /// </summary>
  /// <typeparam name="TStructuralType"> The type to be configured. </typeparam>
  public abstract class StructuralTypeConfiguration<TStructuralType> where TStructuralType : class
  {
    /// <summary>
    /// Configures a <see cref="T:System.struct" /> property that is defined on this type.
    /// </summary>
    /// <typeparam name="T"> The type of the property being configured. </typeparam>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public PrimitivePropertyConfiguration Property<T>(
      Expression<Func<TStructuralType, T>> propertyExpression)
      where T : struct
    {
      return new PrimitivePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.struct" /> property that is defined on this type.
    /// </summary>
    /// <typeparam name="T"> The type of the property being configured. </typeparam>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public PrimitivePropertyConfiguration Property<T>(
      Expression<Func<TStructuralType, T?>> propertyExpression)
      where T : struct
    {
      return new PrimitivePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:HierarchyId" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public PrimitivePropertyConfiguration Property(
      Expression<Func<TStructuralType, HierarchyId>> propertyExpression)
    {
      return new PrimitivePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.Data.Entity.Spatial.DbGeometry" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public PrimitivePropertyConfiguration Property(
      Expression<Func<TStructuralType, DbGeometry>> propertyExpression)
    {
      return new PrimitivePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.Data.Entity.Spatial.DbGeography" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public PrimitivePropertyConfiguration Property(
      Expression<Func<TStructuralType, DbGeography>> propertyExpression)
    {
      return new PrimitivePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.String" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public StringPropertyConfiguration Property(
      Expression<Func<TStructuralType, string>> propertyExpression)
    {
      return new StringPropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.StringPropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.Byte[]" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public BinaryPropertyConfiguration Property(
      Expression<Func<TStructuralType, byte[]>> propertyExpression)
    {
      return new BinaryPropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.BinaryPropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.Decimal" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public DecimalPropertyConfiguration Property(
      Expression<Func<TStructuralType, Decimal>> propertyExpression)
    {
      return new DecimalPropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DecimalPropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.Decimal" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public DecimalPropertyConfiguration Property(
      Expression<Func<TStructuralType, Decimal?>> propertyExpression)
    {
      return new DecimalPropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DecimalPropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.DateTime" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public DateTimePropertyConfiguration Property(
      Expression<Func<TStructuralType, DateTime>> propertyExpression)
    {
      return new DateTimePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DateTimePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.DateTime" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public DateTimePropertyConfiguration Property(
      Expression<Func<TStructuralType, DateTime?>> propertyExpression)
    {
      return new DateTimePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DateTimePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.DateTimeOffset" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public DateTimePropertyConfiguration Property(
      Expression<Func<TStructuralType, DateTimeOffset>> propertyExpression)
    {
      return new DateTimePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DateTimePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.DateTimeOffset" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public DateTimePropertyConfiguration Property(
      Expression<Func<TStructuralType, DateTimeOffset?>> propertyExpression)
    {
      return new DateTimePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DateTimePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.TimeSpan" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public DateTimePropertyConfiguration Property(
      Expression<Func<TStructuralType, TimeSpan>> propertyExpression)
    {
      return new DateTimePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DateTimePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    /// <summary>
    /// Configures a <see cref="T:System.TimeSpan" /> property that is defined on this type.
    /// </summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> A configuration object that can be used to configure the property. </returns>
    public DateTimePropertyConfiguration Property(
      Expression<Func<TStructuralType, TimeSpan?>> propertyExpression)
    {
      return new DateTimePropertyConfiguration(this.Property<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.DateTimePropertyConfiguration>((LambdaExpression) propertyExpression));
    }

    internal abstract StructuralTypeConfiguration Configuration { get; }

    internal abstract TPrimitivePropertyConfiguration Property<TPrimitivePropertyConfiguration>(
      LambdaExpression lambdaExpression)
      where TPrimitivePropertyConfiguration : System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration, new();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the current instance.
    /// </summary>
    /// <returns>The exact runtime type of the current instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
