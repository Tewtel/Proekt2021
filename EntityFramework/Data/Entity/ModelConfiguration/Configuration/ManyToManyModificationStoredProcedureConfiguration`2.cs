// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ManyToManyModificationStoredProcedureConfiguration`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Utilities;
using System.Linq.Expressions;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Allows configuration to be performed for a stored procedure that is used to modify a many to many relationship.
  /// </summary>
  /// <typeparam name="TEntityType">The type of the entity that the relationship is being configured from.</typeparam>
  /// <typeparam name="TTargetEntityType">The type of the entity that the other end of the relationship targets.</typeparam>
  public class ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> : 
    ModificationStoredProcedureConfigurationBase
    where TEntityType : class
    where TTargetEntityType : class
  {
    internal ManyToManyModificationStoredProcedureConfiguration()
    {
    }

    /// <summary>Sets the name of the stored procedure.</summary>
    /// <param name="procedureName">Name of the procedure.</param>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    public ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> HasName(
      string procedureName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(procedureName, nameof (procedureName));
      this.Configuration.HasName(procedureName);
      return this;
    }

    /// <summary>Sets the name of the stored procedure.</summary>
    /// <param name="procedureName">Name of the procedure.</param>
    /// <param name="schemaName">Name of the schema.</param>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    public ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> HasName(
      string procedureName,
      string schemaName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(procedureName, nameof (procedureName));
      System.Data.Entity.Utilities.Check.NotEmpty(schemaName, nameof (schemaName));
      this.Configuration.HasName(procedureName, schemaName);
      return this;
    }

    /// <summary>Configures the parameter for the left key value(s).</summary>
    /// <typeparam name="TProperty">The type of the property to configure.</typeparam>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    public ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> LeftKeyParameter<TProperty>(
      Expression<Func<TEntityType, TProperty>> propertyExpression,
      string parameterName)
      where TProperty : struct
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, TProperty>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetSimplePropertyAccess(), parameterName);
      return this;
    }

    /// <summary>Configures the parameter for the left key value(s).</summary>
    /// <typeparam name="TProperty">The type of the property to configure.</typeparam>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    public ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> LeftKeyParameter<TProperty>(
      Expression<Func<TEntityType, TProperty?>> propertyExpression,
      string parameterName)
      where TProperty : struct
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, TProperty?>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetSimplePropertyAccess(), parameterName);
      return this;
    }

    /// <summary>Configures the parameter for the left key value(s).</summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    public ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> LeftKeyParameter(
      Expression<Func<TEntityType, string>> propertyExpression,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, string>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetSimplePropertyAccess(), parameterName);
      return this;
    }

    /// <summary>Configures the parameter for the left key value(s).</summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    public ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> LeftKeyParameter(
      Expression<Func<TEntityType, byte[]>> propertyExpression,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TEntityType, byte[]>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetSimplePropertyAccess(), parameterName);
      return this;
    }

    /// <summary>Configures the parameter for the right key value(s).</summary>
    /// <typeparam name="TProperty">The type of the property to configure.</typeparam>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    public ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> RightKeyParameter<TProperty>(
      Expression<Func<TTargetEntityType, TProperty>> propertyExpression,
      string parameterName)
      where TProperty : struct
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TTargetEntityType, TProperty>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetSimplePropertyAccess(), parameterName, rightKey: true);
      return this;
    }

    /// <summary>Configures the parameter for the right key value(s).</summary>
    /// <typeparam name="TProperty">The type of the property to configure.</typeparam>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    public ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> RightKeyParameter<TProperty>(
      Expression<Func<TTargetEntityType, TProperty?>> propertyExpression,
      string parameterName)
      where TProperty : struct
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TTargetEntityType, TProperty?>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetSimplePropertyAccess(), parameterName, rightKey: true);
      return this;
    }

    /// <summary>Configures the parameter for the right key value(s).</summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    public ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> RightKeyParameter(
      Expression<Func<TTargetEntityType, string>> propertyExpression,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TTargetEntityType, string>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetSimplePropertyAccess(), parameterName, rightKey: true);
      return this;
    }

    /// <summary>Configures the parameter for the right key value(s).</summary>
    /// <param name="propertyExpression"> A lambda expression representing the property to be configured. C#: t =&gt; t.MyProperty VB.Net: Function(t) t.MyProperty </param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    public ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> RightKeyParameter(
      Expression<Func<TTargetEntityType, byte[]>> propertyExpression,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TTargetEntityType, byte[]>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.Parameter(propertyExpression.GetSimplePropertyAccess(), parameterName, rightKey: true);
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
