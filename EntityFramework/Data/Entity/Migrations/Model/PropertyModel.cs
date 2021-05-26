// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.PropertyModel
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents information about a property of an entity.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public abstract class PropertyModel
  {
    private readonly PrimitiveTypeKind _type;
    private TypeUsage _typeUsage;

    /// <summary>
    /// Initializes a new instance of the PropertyModel class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="type"> The data type for this property model. </param>
    /// <param name="typeUsage"> Additional details about the data type. This includes details such as maximum length, nullability etc. </param>
    protected PropertyModel(PrimitiveTypeKind type, TypeUsage typeUsage)
    {
      this._type = type;
      this._typeUsage = typeUsage;
    }

    /// <summary>Gets the data type for this property model.</summary>
    public virtual PrimitiveTypeKind Type => this._type;

    /// <summary>
    /// Gets additional details about the data type of this property model.
    /// This includes details such as maximum length, nullability etc.
    /// </summary>
    public TypeUsage TypeUsage => this._typeUsage ?? (this._typeUsage = this.BuildTypeUsage());

    /// <summary>
    /// Gets or sets the name of the property model.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets or sets a provider specific data type to use for this property model.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    public virtual string StoreType { get; set; }

    /// <summary>
    /// Gets or sets the maximum length for this property model.
    /// Only valid for array data types.
    /// </summary>
    public virtual int? MaxLength { get; set; }

    /// <summary>
    /// Gets or sets the precision for this property model.
    /// Only valid for decimal data types.
    /// </summary>
    public virtual byte? Precision { get; set; }

    /// <summary>
    /// Gets or sets the scale for this property model.
    /// Only valid for decimal data types.
    /// </summary>
    public virtual byte? Scale { get; set; }

    /// <summary>
    /// Gets or sets a constant value to use as the default value for this property model.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    public virtual object DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets a SQL expression used as the default value for this property model.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    public virtual string DefaultValueSql { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if this property model is fixed length.
    /// Only valid for array data types.
    /// </summary>
    public virtual bool? IsFixedLength { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if this property model supports Unicode characters.
    /// Only valid for textual data types.
    /// </summary>
    public virtual bool? IsUnicode { get; set; }

    private TypeUsage BuildTypeUsage()
    {
      PrimitiveType edmPrimitiveType = PrimitiveType.GetEdmPrimitiveType(this.Type);
      if (this.Type == PrimitiveTypeKind.Binary)
      {
        int? maxLength1 = this.MaxLength;
        if (!maxLength1.HasValue)
          return TypeUsage.CreateBinaryTypeUsage(edmPrimitiveType, this.IsFixedLength.GetValueOrDefault());
        PrimitiveType primitiveType = edmPrimitiveType;
        int num = this.IsFixedLength.GetValueOrDefault() ? 1 : 0;
        maxLength1 = this.MaxLength;
        int maxLength2 = maxLength1.Value;
        return TypeUsage.CreateBinaryTypeUsage(primitiveType, num != 0, maxLength2);
      }
      if (this.Type == PrimitiveTypeKind.String)
      {
        int? maxLength1 = this.MaxLength;
        if (maxLength1.HasValue)
        {
          PrimitiveType primitiveType = edmPrimitiveType;
          bool? nullable = this.IsUnicode;
          int num1 = (int) nullable ?? 1;
          nullable = this.IsFixedLength;
          int num2 = nullable.GetValueOrDefault() ? 1 : 0;
          maxLength1 = this.MaxLength;
          int maxLength2 = maxLength1.Value;
          return TypeUsage.CreateStringTypeUsage(primitiveType, num1 != 0, num2 != 0, maxLength2);
        }
        PrimitiveType primitiveType1 = edmPrimitiveType;
        bool? nullable1 = this.IsUnicode;
        int num3 = (int) nullable1 ?? 1;
        nullable1 = this.IsFixedLength;
        int num4 = nullable1.GetValueOrDefault() ? 1 : 0;
        return TypeUsage.CreateStringTypeUsage(primitiveType1, num3 != 0, num4 != 0);
      }
      if (this.Type == PrimitiveTypeKind.DateTime)
        return TypeUsage.CreateDateTimeTypeUsage(edmPrimitiveType, this.Precision);
      if (this.Type == PrimitiveTypeKind.DateTimeOffset)
        return TypeUsage.CreateDateTimeOffsetTypeUsage(edmPrimitiveType, this.Precision);
      if (this.Type == PrimitiveTypeKind.Decimal)
      {
        byte? nullable = this.Precision;
        if (!nullable.HasValue)
        {
          nullable = this.Scale;
          if (!nullable.HasValue)
            return TypeUsage.CreateDecimalTypeUsage(edmPrimitiveType);
        }
        PrimitiveType primitiveType = edmPrimitiveType;
        nullable = this.Precision;
        int num = (int) nullable ?? 18;
        nullable = this.Scale;
        int valueOrDefault = (int) nullable.GetValueOrDefault();
        return TypeUsage.CreateDecimalTypeUsage(primitiveType, (byte) num, (byte) valueOrDefault);
      }
      return this.Type != PrimitiveTypeKind.Time ? TypeUsage.CreateDefaultTypeUsage((EdmType) edmPrimitiveType) : TypeUsage.CreateTimeTypeUsage(edmPrimitiveType, this.Precision);
    }

    internal virtual FacetValues ToFacetValues()
    {
      FacetValues facetValues1 = new FacetValues();
      if (this.DefaultValue != null)
        facetValues1.DefaultValue = this.DefaultValue;
      bool? nullable1;
      if (this.IsFixedLength.HasValue)
      {
        FacetValues facetValues2 = facetValues1;
        nullable1 = this.IsFixedLength;
        FacetValueContainer<bool?> facetValueContainer = (FacetValueContainer<bool?>) new bool?(nullable1.Value);
        facetValues2.FixedLength = facetValueContainer;
      }
      nullable1 = this.IsUnicode;
      if (nullable1.HasValue)
      {
        FacetValues facetValues2 = facetValues1;
        nullable1 = this.IsUnicode;
        FacetValueContainer<bool?> facetValueContainer = (FacetValueContainer<bool?>) new bool?(nullable1.Value);
        facetValues2.Unicode = facetValueContainer;
      }
      if (this.MaxLength.HasValue)
        facetValues1.MaxLength = (FacetValueContainer<int?>) new int?(this.MaxLength.Value);
      byte? nullable2;
      if (this.Precision.HasValue)
      {
        FacetValues facetValues2 = facetValues1;
        nullable2 = this.Precision;
        FacetValueContainer<byte?> facetValueContainer = (FacetValueContainer<byte?>) new byte?(nullable2.Value);
        facetValues2.Precision = facetValueContainer;
      }
      nullable2 = this.Scale;
      if (nullable2.HasValue)
      {
        FacetValues facetValues2 = facetValues1;
        nullable2 = this.Scale;
        FacetValueContainer<byte?> facetValueContainer = (FacetValueContainer<byte?>) new byte?(nullable2.Value);
        facetValues2.Scale = facetValueContainer;
      }
      return facetValues1;
    }
  }
}
