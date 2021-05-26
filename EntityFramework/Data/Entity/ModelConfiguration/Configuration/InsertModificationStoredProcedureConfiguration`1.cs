// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.InsertModificationStoredProcedureConfiguration`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Spatial;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Allows configuration to be performed for a stored procedure that is used to insert entities.
  /// </summary>
  /// <typeparam name="TEntityType">The type of the entity that the stored procedure can be used to insert.</typeparam>
  public class InsertModificationStoredProcedureConfiguration<TEntityType> : 
    ModificationStoredProcedureConfigurationBase
    where TEntityType : class
  {
    internal InsertModificationStoredProcedureConfiguration()
    {
    }

    /// <summary> Configures the name of the stored procedure. </summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="procedureName"> The stored procedure name. </param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> HasName(
      string procedureName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(procedureName, nameof (procedureName));
      this.Configuration.HasName(procedureName);
      return this;
    }

    /// <summary>Configures the name of the stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="procedureName">The stored procedure name.</param>
    /// <param name="schemaName">The schema name.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> HasName(
      string procedureName,
      string schemaName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(procedureName, nameof (procedureName));
      System.Data.Entity.Utilities.Check.NotEmpty(schemaName, nameof (schemaName));
      this.Configuration.HasName(procedureName, schemaName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <typeparam name="TProperty">The type of the property to configure.</typeparam>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">The name of the parameter.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Parameter<TProperty>(
      Expression<Func<TEntityType, TProperty>> propertyExpression,
      string parameterName)
      where TProperty : struct
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, TProperty>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <typeparam name="TProperty">The type of the property to configure.</typeparam>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">The name of the parameter.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Parameter<TProperty>(
      Expression<Func<TEntityType, TProperty?>> propertyExpression,
      string parameterName)
      where TProperty : struct
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, TProperty?>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">The name of the parameter.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Parameter(
      Expression<Func<TEntityType, string>> propertyExpression,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, string>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">The name of the parameter.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Parameter(
      Expression<Func<TEntityType, byte[]>> propertyExpression,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, byte[]>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">The name of the parameter.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Parameter(
      Expression<Func<TEntityType, DbGeography>> propertyExpression,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, DbGeography>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the parameter for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">The name of the parameter.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Parameter(
      Expression<Func<TEntityType, DbGeometry>> propertyExpression,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, DbGeometry>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetComplexPropertyAccess(), parameterName);
      return this;
    }

    /// <summary>
    /// Configures a column of the result for this stored procedure to map to a property.
    /// This is used for database generated columns.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property to configure.</typeparam>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the result for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="columnName">The name of the result column.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Result<TProperty>(
      Expression<Func<TEntityType, TProperty>> propertyExpression,
      string columnName)
      where TProperty : struct
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, TProperty>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(columnName, nameof (columnName));
      this.Configuration.Result(propertyExpression.GetSimplePropertyAccess(), columnName);
      return this;
    }

    /// <summary>
    /// Configures a column of the result for this stored procedure to map to a property.
    /// This is used for database generated columns.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property to configure.</typeparam>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the result for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="columnName">The name of the result column.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Result<TProperty>(
      Expression<Func<TEntityType, TProperty?>> propertyExpression,
      string columnName)
      where TProperty : struct
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, TProperty?>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(columnName, nameof (columnName));
      this.Configuration.Result(propertyExpression.GetSimplePropertyAccess(), columnName);
      return this;
    }

    /// <summary>
    /// Configures a column of the result for this stored procedure to map to a property.
    /// This is used for database generated columns.
    /// </summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the result for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="columnName">The name of the result column.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Result(
      Expression<Func<TEntityType, string>> propertyExpression,
      string columnName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, string>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(columnName, nameof (columnName));
      this.Configuration.Result(propertyExpression.GetSimplePropertyAccess(), columnName);
      return this;
    }

    /// <summary>
    /// Configures a column of the result for this stored procedure to map to a property.
    /// This is used for database generated columns.
    /// </summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the result for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="columnName">The name of the result column.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Result(
      Expression<Func<TEntityType, byte[]>> propertyExpression,
      string columnName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, byte[]>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(columnName, nameof (columnName));
      this.Configuration.Result(propertyExpression.GetSimplePropertyAccess(), columnName);
      return this;
    }

    /// <summary>
    /// Configures a column of the result for this stored procedure to map to a property.
    /// This is used for database generated columns.
    /// </summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the result for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="columnName">The name of the result column.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Result(
      Expression<Func<TEntityType, DbGeography>> propertyExpression,
      string columnName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, DbGeography>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(columnName, nameof (columnName));
      this.Configuration.Result(propertyExpression.GetSimplePropertyAccess(), columnName);
      return this;
    }

    /// <summary>
    /// Configures a column of the result for this stored procedure to map to a property.
    /// This is used for database generated columns.
    /// </summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyExpression"> A lambda expression representing the property to configure the result for. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="columnName">The name of the result column.</param>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Result(
      Expression<Func<TEntityType, DbGeometry>> propertyExpression,
      string columnName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, DbGeometry>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(columnName, nameof (columnName));
      this.Configuration.Result(propertyExpression.GetSimplePropertyAccess(), columnName);
      return this;
    }

    /// <summary>Configures parameters for a relationship where the foreign key property is not included in the class.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="navigationPropertyExpression"> A lambda expression representing the navigation property for the relationship. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="associationModificationStoredProcedureConfigurationAction">A lambda expression that performs the configuration.</param>
    /// <typeparam name="TPrincipalEntityType">The type of the principal entity in the relationship.</typeparam>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Navigation<TPrincipalEntityType>(
      Expression<Func<TPrincipalEntityType, TEntityType>> navigationPropertyExpression,
      Action<AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType>> associationModificationStoredProcedureConfigurationAction)
      where TPrincipalEntityType : class
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TPrincipalEntityType, TEntityType>>>(navigationPropertyExpression, nameof (navigationPropertyExpression));
      System.Data.Entity.Utilities.Check.NotNull<Action<AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType>>>(associationModificationStoredProcedureConfigurationAction, nameof (associationModificationStoredProcedureConfigurationAction));
      AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType> procedureConfiguration = new AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType>(navigationPropertyExpression.GetSimplePropertyAccess().Single<PropertyInfo>(), this.Configuration);
      associationModificationStoredProcedureConfigurationAction(procedureConfiguration);
      return this;
    }

    /// <summary>Configures parameters for a relationship where the foreign key property is not included in the class.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="navigationPropertyExpression"> A lambda expression representing the navigation property for the relationship. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="associationModificationStoredProcedureConfigurationAction">A lambda expression that performs the configuration.</param>
    /// <typeparam name="TPrincipalEntityType">The type of the principal entity in the relationship.</typeparam>
    public InsertModificationStoredProcedureConfiguration<TEntityType> Navigation<TPrincipalEntityType>(
      Expression<Func<TPrincipalEntityType, ICollection<TEntityType>>> navigationPropertyExpression,
      Action<AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType>> associationModificationStoredProcedureConfigurationAction)
      where TPrincipalEntityType : class
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TPrincipalEntityType, ICollection<TEntityType>>>>(navigationPropertyExpression, nameof (navigationPropertyExpression));
      System.Data.Entity.Utilities.Check.NotNull<Action<AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType>>>(associationModificationStoredProcedureConfigurationAction, nameof (associationModificationStoredProcedureConfigurationAction));
      AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType> procedureConfiguration = new AssociationModificationStoredProcedureConfiguration<TPrincipalEntityType>(navigationPropertyExpression.GetSimplePropertyAccess().Single<PropertyInfo>(), this.Configuration);
      associationModificationStoredProcedureConfigurationAction(procedureConfiguration);
      return this;
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
