// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ConventionUpdateModificationStoredProcedureConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.ModelConfiguration.Utilities;
using System.Data.Entity.Utilities;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Creates a convention that configures stored procedures to be used to update entities in the database.
  /// </summary>
  public class ConventionUpdateModificationStoredProcedureConfiguration : 
    ConventionModificationStoredProcedureConfiguration
  {
    private readonly Type _type;

    internal ConventionUpdateModificationStoredProcedureConfiguration(Type type) => this._type = type;

    /// <summary> Configures the name of the stored procedure. </summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="procedureName"> The stored procedure name. </param>
    public ConventionUpdateModificationStoredProcedureConfiguration HasName(
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
    public ConventionUpdateModificationStoredProcedureConfiguration HasName(
      string procedureName,
      string schemaName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(procedureName, nameof (procedureName));
      System.Data.Entity.Utilities.Check.NotEmpty(schemaName, nameof (schemaName));
      this.Configuration.HasName(procedureName, schemaName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyName"> The name of the property to configure the parameter for. </param>
    /// <param name="parameterName"> The name of the parameter. </param>
    public ConventionUpdateModificationStoredProcedureConfiguration Parameter(
      string propertyName,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      return this.Parameter(this._type.GetAnyProperty(propertyName), parameterName);
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyInfo"> The property to configure the parameter for. </param>
    /// <param name="parameterName"> The name of the parameter. </param>
    public ConventionUpdateModificationStoredProcedureConfiguration Parameter(
      PropertyInfo propertyInfo,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      if (propertyInfo != (PropertyInfo) null)
        this.Configuration.Parameter(new PropertyPath(propertyInfo), parameterName);
      return this;
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyName"> The name of the property to configure the parameter for. </param>
    /// <param name="currentValueParameterName">The current value parameter name.</param>
    /// <param name="originalValueParameterName">The original value parameter name.</param>
    public ConventionUpdateModificationStoredProcedureConfiguration Parameter(
      string propertyName,
      string currentValueParameterName,
      string originalValueParameterName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      System.Data.Entity.Utilities.Check.NotEmpty(currentValueParameterName, nameof (currentValueParameterName));
      System.Data.Entity.Utilities.Check.NotEmpty(originalValueParameterName, nameof (originalValueParameterName));
      return this.Parameter(this._type.GetAnyProperty(propertyName), currentValueParameterName, originalValueParameterName);
    }

    /// <summary>Configures a parameter for this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyInfo"> The property to configure the parameter for. </param>
    /// <param name="currentValueParameterName">The current value parameter name.</param>
    /// <param name="originalValueParameterName">The original value parameter name.</param>
    public ConventionUpdateModificationStoredProcedureConfiguration Parameter(
      PropertyInfo propertyInfo,
      string currentValueParameterName,
      string originalValueParameterName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(currentValueParameterName, nameof (currentValueParameterName));
      System.Data.Entity.Utilities.Check.NotEmpty(originalValueParameterName, nameof (originalValueParameterName));
      if (propertyInfo != (PropertyInfo) null)
        this.Configuration.Parameter(new PropertyPath(propertyInfo), currentValueParameterName, originalValueParameterName);
      return this;
    }

    /// <summary>
    /// Configures a column of the result for this stored procedure to map to a property.
    /// This is used for database generated columns.
    /// </summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyName"> The name of the property to configure the result for. </param>
    /// <param name="columnName">The name of the result column.</param>
    public ConventionUpdateModificationStoredProcedureConfiguration Result(
      string propertyName,
      string columnName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      System.Data.Entity.Utilities.Check.NotEmpty(columnName, nameof (columnName));
      this.Configuration.Result(new PropertyPath(this._type.GetAnyProperty(propertyName)), columnName);
      return this;
    }

    /// <summary>
    /// Configures a column of the result for this stored procedure to map to a property.
    /// This is used for database generated columns.
    /// </summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="propertyInfo"> The property to configure the result for. </param>
    /// <param name="columnName">The name of the result column.</param>
    public ConventionUpdateModificationStoredProcedureConfiguration Result(
      PropertyInfo propertyInfo,
      string columnName)
    {
      System.Data.Entity.Utilities.Check.NotNull<PropertyInfo>(propertyInfo, nameof (propertyInfo));
      System.Data.Entity.Utilities.Check.NotEmpty(columnName, nameof (columnName));
      this.Configuration.Result(new PropertyPath(propertyInfo), columnName);
      return this;
    }

    /// <summary>Configures the output parameter that returns the rows affected by this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="parameterName">The name of the parameter.</param>
    public ConventionUpdateModificationStoredProcedureConfiguration RowsAffectedParameter(
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      this.Configuration.RowsAffectedParameter(parameterName);
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
