// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ConventionDeleteModificationStoredProcedureConfiguration
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
  /// Creates a convention that configures stored procedures to be used to delete entities in the database.
  /// </summary>
  public class ConventionDeleteModificationStoredProcedureConfiguration : 
    ConventionModificationStoredProcedureConfiguration
  {
    private readonly Type _type;

    internal ConventionDeleteModificationStoredProcedureConfiguration(Type type) => this._type = type;

    /// <summary> Configures the name of the stored procedure. </summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="procedureName"> The stored procedure name. </param>
    public ConventionDeleteModificationStoredProcedureConfiguration HasName(
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
    public ConventionDeleteModificationStoredProcedureConfiguration HasName(
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
    /// <param name="parameterName">The name of the parameter.</param>
    public ConventionDeleteModificationStoredProcedureConfiguration Parameter(
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
    /// <param name="parameterName">The name of the parameter.</param>
    public ConventionDeleteModificationStoredProcedureConfiguration Parameter(
      PropertyInfo propertyInfo,
      string parameterName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(parameterName, nameof (parameterName));
      if (propertyInfo != (PropertyInfo) null)
        this.Configuration.Parameter(new PropertyPath(propertyInfo), parameterName);
      return this;
    }

    /// <summary>Configures the output parameter that returns the rows affected by this stored procedure.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="parameterName">The name of the parameter.</param>
    public ConventionDeleteModificationStoredProcedureConfiguration RowsAffectedParameter(
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
