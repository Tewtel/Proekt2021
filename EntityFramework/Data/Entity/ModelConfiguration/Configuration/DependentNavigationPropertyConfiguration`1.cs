﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.DependentNavigationPropertyConfiguration`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation;
using System.Data.Entity.ModelConfiguration.Utilities;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Configures a relationship that can support foreign key properties that are exposed in the object model.
  /// This configuration functionality is available via the Code First Fluent API, see <see cref="T:System.Data.Entity.DbModelBuilder" />.
  /// </summary>
  /// <typeparam name="TDependentEntityType"> The dependent entity type. </typeparam>
  public class DependentNavigationPropertyConfiguration<TDependentEntityType> : 
    ForeignKeyNavigationPropertyConfiguration
    where TDependentEntityType : class
  {
    internal DependentNavigationPropertyConfiguration(
      NavigationPropertyConfiguration navigationPropertyConfiguration)
      : base(navigationPropertyConfiguration)
    {
    }

    /// <summary>
    /// Configures the relationship to use foreign key property(s) that are exposed in the object model.
    /// If the foreign key property(s) are not exposed in the object model then use the Map method.
    /// </summary>
    /// <typeparam name="TKey"> The type of the key. </typeparam>
    /// <param name="foreignKeyExpression"> A lambda expression representing the property to be used as the foreign key. If the foreign key is made up of multiple properties then specify an anonymous type including the properties. When using multiple foreign key properties, the properties must be specified in the same order that the primary key properties were configured for the principal entity type. </param>
    /// <returns> A configuration object that can be used to further configure the relationship. </returns>
    public CascadableNavigationPropertyConfiguration HasForeignKey<TKey>(
      Expression<Func<TDependentEntityType, TKey>> foreignKeyExpression)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TDependentEntityType, TKey>>>(foreignKeyExpression, nameof (foreignKeyExpression));
      this.NavigationPropertyConfiguration.Constraint = (ConstraintConfiguration) new ForeignKeyConstraintConfiguration(foreignKeyExpression.GetSimplePropertyAccessList().Select<PropertyPath, PropertyInfo>((Func<PropertyPath, PropertyInfo>) (p => p.Single<PropertyInfo>())));
      return (CascadableNavigationPropertyConfiguration) this;
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
