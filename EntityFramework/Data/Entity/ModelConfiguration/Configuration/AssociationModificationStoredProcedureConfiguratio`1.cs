// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.AssociationModificationStoredProcedureConfiguration`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Utilities;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Allows configuration to be performed for a stored procedure that is used to modify a relationship.
  /// </summary>
  /// <typeparam name="TEntityType">The type of the entity that the relationship is being configured from.</typeparam>
  public class AssociationModificationStoredProcedureConfiguration<TEntityType> where TEntityType : class
  {
    private readonly PropertyInfo _navigationPropertyInfo;
    private readonly ModificationStoredProcedureConfiguration _configuration;

    internal AssociationModificationStoredProcedureConfiguration(
      PropertyInfo navigationPropertyInfo,
      ModificationStoredProcedureConfiguration configuration)
    {
      this._navigationPropertyInfo = navigationPropertyInfo;
      this._configuration = configuration;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    public AssociationModificationStoredProcedureConfiguration<TEntityType> Parameter<TProperty>(
      Expression<Func<TEntityType, TProperty>> propertyExpression,
      string parameterName)
      where TProperty : struct
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, TProperty>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this._configuration.Parameter(new PropertyPath(((IEnumerable<PropertyInfo>) new PropertyInfo[1]
      {
        this._navigationPropertyInfo
      }).Concat<PropertyInfo>((IEnumerable<PropertyInfo>) propertyExpression.GetSimplePropertyAccess())), parameterName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">The name of the parameter.</param>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    public AssociationModificationStoredProcedureConfiguration<TEntityType> Parameter<TProperty>(
      Expression<Func<TEntityType, TProperty?>> propertyExpression,
      string parameterName)
      where TProperty : struct
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, TProperty?>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this._configuration.Parameter(new PropertyPath(((IEnumerable<PropertyInfo>) new PropertyInfo[1]
      {
        this._navigationPropertyInfo
      }).Concat<PropertyInfo>((IEnumerable<PropertyInfo>) propertyExpression.GetSimplePropertyAccess())), parameterName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">The name of the parameter.</param>
    public AssociationModificationStoredProcedureConfiguration<TEntityType> Parameter(
      Expression<Func<TEntityType, string>> propertyExpression,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, string>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this._configuration.Parameter(new PropertyPath(((IEnumerable<PropertyInfo>) new PropertyInfo[1]
      {
        this._navigationPropertyInfo
      }).Concat<PropertyInfo>((IEnumerable<PropertyInfo>) propertyExpression.GetSimplePropertyAccess())), parameterName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">The name of the parameter.</param>
    public AssociationModificationStoredProcedureConfiguration<TEntityType> Parameter(
      Expression<Func<TEntityType, byte[]>> propertyExpression,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, byte[]>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this._configuration.Parameter(new PropertyPath(((IEnumerable<PropertyInfo>) new PropertyInfo[1]
      {
        this._navigationPropertyInfo
      }).Concat<PropertyInfo>((IEnumerable<PropertyInfo>) propertyExpression.GetSimplePropertyAccess())), parameterName);
      return this;
    }
  }
}
