﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.PrimitivePropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Used to configure a primitive property of an entity type or complex type.
  /// This configuration functionality is available via the Code First Fluent API, see <see cref="T:System.Data.Entity.DbModelBuilder" />.
  /// </summary>
  public class PrimitivePropertyConfiguration
  {
    private readonly System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration _configuration;

    internal PrimitivePropertyConfiguration(System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration configuration) => this._configuration = configuration;

    internal System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration Configuration => this._configuration;

    /// <summary>
    /// Configures the property to be optional.
    /// The database column used to store this property will be nullable.
    /// </summary>
    /// <returns> The same PrimitivePropertyConfiguration instance so that multiple calls can be chained. </returns>
    public PrimitivePropertyConfiguration IsOptional()
    {
      this.Configuration.IsNullable = new bool?(true);
      return this;
    }

    /// <summary>
    /// Configures the property to be required.
    /// The database column used to store this property will be non-nullable.
    /// </summary>
    /// <returns> The same PrimitivePropertyConfiguration instance so that multiple calls can be chained. </returns>
    public PrimitivePropertyConfiguration IsRequired()
    {
      this.Configuration.IsNullable = new bool?(false);
      return this;
    }

    /// <summary>
    /// Configures how values for the property are generated by the database.
    /// </summary>
    /// <param name="databaseGeneratedOption">
    /// The pattern used to generate values for the property in the database.
    /// Setting 'null' will cause the default option to be used, which may be 'None', 'Identity', or 'Computed' depending
    /// on the type of the property, its semantics in the model (e.g. primary keys are treated differently), and which
    /// set of conventions are being used.
    /// </param>
    /// <returns> The same PrimitivePropertyConfiguration instance so that multiple calls can be chained. </returns>
    public PrimitivePropertyConfiguration HasDatabaseGeneratedOption(
      DatabaseGeneratedOption? databaseGeneratedOption)
    {
      this.Configuration.DatabaseGeneratedOption = !databaseGeneratedOption.HasValue || Enum.IsDefined(typeof (DatabaseGeneratedOption), (object) databaseGeneratedOption) ? databaseGeneratedOption : throw new ArgumentOutOfRangeException(nameof (databaseGeneratedOption));
      return this;
    }

    /// <summary>
    /// Configures the property to be used as an optimistic concurrency token.
    /// </summary>
    /// <returns> The same PrimitivePropertyConfiguration instance so that multiple calls can be chained. </returns>
    public PrimitivePropertyConfiguration IsConcurrencyToken()
    {
      this.IsConcurrencyToken(new bool?(true));
      return this;
    }

    /// <summary>
    /// Configures whether or not the property is to be used as an optimistic concurrency token.
    /// </summary>
    /// <param name="concurrencyToken"> Value indicating if the property is a concurrency token or not. Specifying 'null' will remove the concurrency token facet from the property. Specifying 'null' will cause the same runtime behavior as specifying 'false'. </param>
    /// <returns> The same PrimitivePropertyConfiguration instance so that multiple calls can be chained. </returns>
    public PrimitivePropertyConfiguration IsConcurrencyToken(
      bool? concurrencyToken)
    {
      this.Configuration.ConcurrencyMode = !concurrencyToken.HasValue ? new ConcurrencyMode?() : new ConcurrencyMode?(concurrencyToken.Value ? ConcurrencyMode.Fixed : ConcurrencyMode.None);
      return this;
    }

    /// <summary>
    /// Configures the data type of the database column used to store the property.
    /// </summary>
    /// <param name="columnType"> Name of the database provider specific data type. </param>
    /// <returns> The same PrimitivePropertyConfiguration instance so that multiple calls can be chained. </returns>
    public PrimitivePropertyConfiguration HasColumnType(
      string columnType)
    {
      this.Configuration.ColumnType = columnType;
      return this;
    }

    /// <summary>
    /// Configures the name of the database column used to store the property.
    /// </summary>
    /// <param name="columnName"> The name of the column. </param>
    /// <returns> The same PrimitivePropertyConfiguration instance so that multiple calls can be chained. </returns>
    public PrimitivePropertyConfiguration HasColumnName(
      string columnName)
    {
      this.Configuration.ColumnName = columnName;
      return this;
    }

    /// <summary>
    /// Sets an annotation in the model for the database column used to store the property. The annotation
    /// value can later be used when processing the column such as when creating migrations.
    /// </summary>
    /// <remarks>
    /// It will likely be necessary to register a <see cref="T:System.Data.Entity.Infrastructure.IMetadataAnnotationSerializer" /> if the type of
    /// the annotation value is anything other than a string. Passing a null value clears any annotation with
    /// the given name on the column that had been previously set.
    /// </remarks>
    /// <param name="name">The annotation name, which must be a valid C#/EDM identifier.</param>
    /// <param name="value">The annotation value, which may be a string or some other type that
    /// can be serialized with an <see cref="T:System.Data.Entity.Infrastructure.IMetadataAnnotationSerializer" /></param>
    /// .
    ///             <returns>The same PrimitivePropertyConfiguration instance so that multiple calls can be chained.</returns>
    public PrimitivePropertyConfiguration HasColumnAnnotation(
      string name,
      object value)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      this.Configuration.SetAnnotation(name, value);
      return this;
    }

    /// <summary>
    /// Configures the name of the parameter used in stored procedures for this property.
    /// </summary>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <returns> The same PrimitivePropertyConfiguration instance so that multiple calls can be chained. </returns>
    public PrimitivePropertyConfiguration HasParameterName(
      string parameterName)
    {
      this.Configuration.ParameterName = parameterName;
      return this;
    }

    /// <summary>
    /// Configures the order of the database column used to store the property.
    /// This method is also used to specify key ordering when an entity type has a composite key.
    /// </summary>
    /// <param name="columnOrder"> The order that this column should appear in the database table. </param>
    /// <returns> The same PrimitivePropertyConfiguration instance so that multiple calls can be chained. </returns>
    public PrimitivePropertyConfiguration HasColumnOrder(
      int? columnOrder)
    {
      if (columnOrder.HasValue && columnOrder.Value < 0)
        throw new ArgumentOutOfRangeException(nameof (columnOrder));
      this.Configuration.ColumnOrder = columnOrder;
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

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the current instance.
    /// </summary>
    /// <returns>The exact runtime type of the current instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
