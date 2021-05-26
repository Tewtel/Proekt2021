// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.ComplexTypeConfiguration`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration
{
  /// <summary>
  /// Allows configuration to be performed for an complex type in a model.
  /// A ComplexTypeConfiguration can be obtained via the ComplexType method on
  /// <see cref="T:System.Data.Entity.DbModelBuilder" /> or a custom type derived from ComplexTypeConfiguration
  /// can be registered via the Configurations property on <see cref="T:System.Data.Entity.DbModelBuilder" />.
  /// </summary>
  /// <typeparam name="TComplexType"> The complex type to be configured. </typeparam>
  public class ComplexTypeConfiguration<TComplexType> : StructuralTypeConfiguration<TComplexType>
    where TComplexType : class
  {
    private readonly ComplexTypeConfiguration _complexTypeConfiguration;

    /// <summary>
    /// Initializes a new instance of ComplexTypeConfiguration
    /// </summary>
    public ComplexTypeConfiguration()
      : this(new ComplexTypeConfiguration(typeof (TComplexType)))
    {
    }

    /// <summary>
    /// Excludes a property from the model so that it will not be mapped to the database.
    /// </summary>
    /// <typeparam name="TProperty"> The type of the property to be ignored. </typeparam>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <returns> The same ComplexTypeConfiguration instance so that multiple calls can be chained. </returns>
    public ComplexTypeConfiguration<TComplexType> Ignore<TProperty>(
      Expression<Func<TComplexType, TProperty>> propertyExpression)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TComplexType, TProperty>>>(propertyExpression, nameof (propertyExpression));
      this.Configuration.Ignore(propertyExpression.GetSimplePropertyAccess().Single<PropertyInfo>());
      return this;
    }

    internal ComplexTypeConfiguration(ComplexTypeConfiguration configuration) => this._complexTypeConfiguration = configuration;

    internal override StructuralTypeConfiguration Configuration => (StructuralTypeConfiguration) this._complexTypeConfiguration;

    internal override TPrimitivePropertyConfiguration Property<TPrimitivePropertyConfiguration>(
      LambdaExpression lambdaExpression)
    {
      return this.Configuration.Property<TPrimitivePropertyConfiguration>(lambdaExpression.GetSimplePropertyAccess(), (Func<TPrimitivePropertyConfiguration>) (() =>
      {
        return new TPrimitivePropertyConfiguration()
        {
          OverridableConfigurationParts = OverridableConfigurationParts.OverridableInSSpace
        };
      }));
    }

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
