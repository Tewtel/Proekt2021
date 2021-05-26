// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.TypeUsageBuilder
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal class TypeUsageBuilder
  {
    private readonly Dictionary<string, object> _facetValues;
    private readonly SchemaElement _element;
    private string _default;
    private object _defaultObject;
    private bool? _nullable;
    private TypeUsage _typeUsage;
    private bool _hasUserDefinedFacets;

    internal TypeUsageBuilder(SchemaElement element)
    {
      this._element = element;
      this._facetValues = new Dictionary<string, object>();
    }

    internal TypeUsage TypeUsage => this._typeUsage;

    internal bool Nullable => !this._nullable.HasValue || this._nullable.Value;

    internal string Default => this._default;

    internal object DefaultAsObject => this._defaultObject;

    internal bool HasUserDefinedFacets => this._hasUserDefinedFacets;

    private bool TryGetFacets(
      EdmType edmType,
      bool complainOnMissingFacet,
      out Dictionary<string, Facet> calculatedFacets)
    {
      bool flag = true;
      Dictionary<string, Facet> dictionary = edmType.GetAssociatedFacetDescriptions().ToDictionary<FacetDescription, string, Facet>((Func<FacetDescription, string>) (f => f.FacetName), (Func<FacetDescription, Facet>) (f => f.DefaultValueFacet));
      calculatedFacets = new Dictionary<string, Facet>();
      foreach (Facet facet in dictionary.Values)
      {
        object obj;
        if (this._facetValues.TryGetValue(facet.Name, out obj))
        {
          if (facet.Description.IsConstant)
          {
            this._element.AddError(ErrorCode.ConstantFacetSpecifiedInSchema, EdmSchemaErrorSeverity.Error, this._element, (object) System.Data.Entity.Resources.Strings.ConstantFacetSpecifiedInSchema((object) facet.Name, (object) edmType.Name));
            flag = false;
          }
          else
            calculatedFacets.Add(facet.Name, Facet.Create(facet.Description, obj));
          this._facetValues.Remove(facet.Name);
        }
        else if (complainOnMissingFacet && facet.Description.IsRequired)
        {
          this._element.AddError(ErrorCode.RequiredFacetMissing, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.RequiredFacetMissing((object) facet.Name, (object) edmType.Name));
          flag = false;
        }
        else
          calculatedFacets.Add(facet.Name, facet);
      }
      foreach (KeyValuePair<string, object> facetValue in this._facetValues)
      {
        if (facetValue.Key == "StoreGeneratedPattern")
        {
          Facet facet = Facet.Create(Converter.StoreGeneratedPatternFacet, facetValue.Value);
          calculatedFacets.Add(facet.Name, facet);
        }
        else if (facetValue.Key == "ConcurrencyMode")
        {
          Facet facet = Facet.Create(Converter.ConcurrencyModeFacet, facetValue.Value);
          calculatedFacets.Add(facet.Name, facet);
        }
        else if (edmType is PrimitiveType && ((PrimitiveType) edmType).PrimitiveTypeKind == PrimitiveTypeKind.String && facetValue.Key == "Collation")
        {
          Facet facet = Facet.Create(Converter.CollationFacet, facetValue.Value);
          calculatedFacets.Add(facet.Name, facet);
        }
        else
          this._element.AddError(ErrorCode.FacetNotAllowedByType, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.FacetNotAllowed((object) facetValue.Key, (object) edmType.Name));
      }
      return flag;
    }

    internal void ValidateAndSetTypeUsage(EdmType edmType, bool complainOnMissingFacet)
    {
      Dictionary<string, Facet> calculatedFacets;
      this.TryGetFacets(edmType, complainOnMissingFacet, out calculatedFacets);
      this._typeUsage = TypeUsage.Create(edmType, (IEnumerable<Facet>) calculatedFacets.Values);
    }

    internal void ValidateAndSetTypeUsage(ScalarType scalar, bool complainOnMissingFacet)
    {
      Trace.Assert(this._element != null);
      Trace.Assert(scalar != null);
      if (Helper.IsSpatialType(scalar.Type) && !this._facetValues.ContainsKey("IsStrict") && !this._element.Schema.UseStrongSpatialTypes)
        this._facetValues.Add("IsStrict", (object) false);
      Dictionary<string, Facet> calculatedFacets;
      if (this.TryGetFacets((EdmType) scalar.Type, complainOnMissingFacet, out calculatedFacets))
      {
        switch (scalar.TypeKind)
        {
          case PrimitiveTypeKind.Binary:
            this.ValidateAndSetBinaryFacets((EdmType) scalar.Type, calculatedFacets);
            break;
          case PrimitiveTypeKind.DateTime:
          case PrimitiveTypeKind.Time:
          case PrimitiveTypeKind.DateTimeOffset:
            this.ValidatePrecisionFacetsForDateTimeFamily((EdmType) scalar.Type, calculatedFacets);
            break;
          case PrimitiveTypeKind.Decimal:
            this.ValidateAndSetDecimalFacets((EdmType) scalar.Type, calculatedFacets);
            break;
          case PrimitiveTypeKind.String:
            this.ValidateAndSetStringFacets((EdmType) scalar.Type, calculatedFacets);
            break;
          case PrimitiveTypeKind.Geometry:
          case PrimitiveTypeKind.Geography:
          case PrimitiveTypeKind.GeometryPoint:
          case PrimitiveTypeKind.GeometryLineString:
          case PrimitiveTypeKind.GeometryPolygon:
          case PrimitiveTypeKind.GeometryMultiPoint:
          case PrimitiveTypeKind.GeometryMultiLineString:
          case PrimitiveTypeKind.GeometryMultiPolygon:
          case PrimitiveTypeKind.GeometryCollection:
          case PrimitiveTypeKind.GeographyPoint:
          case PrimitiveTypeKind.GeographyLineString:
          case PrimitiveTypeKind.GeographyPolygon:
          case PrimitiveTypeKind.GeographyMultiPoint:
          case PrimitiveTypeKind.GeographyMultiLineString:
          case PrimitiveTypeKind.GeographyMultiPolygon:
          case PrimitiveTypeKind.GeographyCollection:
            this.ValidateSpatialFacets((EdmType) scalar.Type, calculatedFacets);
            break;
        }
      }
      this._typeUsage = TypeUsage.Create((EdmType) scalar.Type, (IEnumerable<Facet>) calculatedFacets.Values);
    }

    internal void ValidateEnumFacets(SchemaEnumType schemaEnumType)
    {
      foreach (KeyValuePair<string, object> facetValue in this._facetValues)
      {
        if (facetValue.Key != "Nullable" && facetValue.Key != "StoreGeneratedPattern" && facetValue.Key != "ConcurrencyMode")
          this._element.AddError(ErrorCode.FacetNotAllowedByType, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.FacetNotAllowed((object) facetValue.Key, (object) schemaEnumType.FQName));
      }
    }

    internal bool HandleAttribute(XmlReader reader)
    {
      bool flag = this.InternalHandleAttribute(reader);
      this._hasUserDefinedFacets |= flag;
      return flag;
    }

    private bool InternalHandleAttribute(XmlReader reader)
    {
      if (SchemaElement.CanHandleAttribute(reader, "Nullable"))
      {
        this.HandleNullableAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "DefaultValue"))
      {
        this.HandleDefaultAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "Precision"))
      {
        this.HandlePrecisionAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "Scale"))
      {
        this.HandleScaleAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "StoreGeneratedPattern"))
      {
        this.HandleStoreGeneratedPatternAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "ConcurrencyMode"))
      {
        this.HandleConcurrencyModeAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "MaxLength"))
      {
        this.HandleMaxLengthAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "Unicode"))
      {
        this.HandleUnicodeAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "Collation"))
      {
        this.HandleCollationAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "FixedLength"))
      {
        this.HandleIsFixedLengthAttribute(reader);
        return true;
      }
      if (SchemaElement.CanHandleAttribute(reader, "Nullable"))
      {
        this.HandleNullableAttribute(reader);
        return true;
      }
      if (!SchemaElement.CanHandleAttribute(reader, "SRID"))
        return false;
      this.HandleSridAttribute(reader);
      return true;
    }

    private void ValidateAndSetBinaryFacets(EdmType type, Dictionary<string, Facet> facets) => this.ValidateLengthFacets(type, facets);

    private void ValidateAndSetDecimalFacets(EdmType type, Dictionary<string, Facet> facets)
    {
      PrimitiveType primitiveType = (PrimitiveType) type;
      byte? nullable1 = new byte?();
      Facet facet1;
      byte? nullable2;
      int? nullable3;
      int? nullable4;
      if (facets.TryGetValue("Precision", out facet1) && facet1.Value != null)
      {
        nullable1 = new byte?((byte) facet1.Value);
        FacetDescription facet2 = Helper.GetFacet((IEnumerable<FacetDescription>) primitiveType.FacetDescriptions, "Precision");
        nullable2 = nullable1;
        int? nullable5 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
        nullable3 = facet2.MinValue;
        int num1 = nullable3.Value;
        if (!(nullable5.GetValueOrDefault() < num1 & nullable5.HasValue))
        {
          nullable2 = nullable1;
          int? nullable6;
          if (!nullable2.HasValue)
          {
            nullable3 = new int?();
            nullable6 = nullable3;
          }
          else
            nullable6 = new int?((int) nullable2.GetValueOrDefault());
          int? nullable7 = nullable6;
          nullable3 = facet2.MaxValue;
          int num2 = nullable3.Value;
          if (!(nullable7.GetValueOrDefault() > num2 & nullable7.HasValue))
            goto label_7;
        }
        SchemaElement element = this._element;
        // ISSUE: variable of a boxed type
        __Boxed<byte?> local1 = (System.ValueType) nullable1;
        nullable4 = facet2.MinValue;
        // ISSUE: variable of a boxed type
        __Boxed<int> local2 = (System.ValueType) nullable4.Value;
        nullable4 = facet2.MaxValue;
        // ISSUE: variable of a boxed type
        __Boxed<int> local3 = (System.ValueType) nullable4.Value;
        string name = primitiveType.Name;
        string str = System.Data.Entity.Resources.Strings.PrecisionOutOfRange((object) local1, (object) local2, (object) local3, (object) name);
        element.AddError(ErrorCode.PrecisionOutOfRange, EdmSchemaErrorSeverity.Error, (object) str);
      }
label_7:
      Facet facet3;
      if (!facets.TryGetValue("Scale", out facet3) || facet3.Value == null)
        return;
      byte num3 = (byte) facet3.Value;
      FacetDescription facet4 = Helper.GetFacet((IEnumerable<FacetDescription>) primitiveType.FacetDescriptions, "Scale");
      int num4 = (int) num3;
      nullable4 = facet4.MinValue;
      int num5 = nullable4.Value;
      if (num4 >= num5)
      {
        int num1 = (int) num3;
        nullable4 = facet4.MaxValue;
        int num2 = nullable4.Value;
        if (num1 <= num2)
        {
          if (!nullable1.HasValue)
            return;
          nullable2 = nullable1;
          int? nullable5;
          if (!nullable2.HasValue)
          {
            nullable3 = new int?();
            nullable5 = nullable3;
          }
          else
            nullable5 = new int?((int) nullable2.GetValueOrDefault());
          nullable4 = nullable5;
          int num6 = (int) num3;
          if (!(nullable4.GetValueOrDefault() < num6 & nullable4.HasValue))
            return;
          this._element.AddError(ErrorCode.BadPrecisionAndScale, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.BadPrecisionAndScale((object) nullable1, (object) num3));
          return;
        }
      }
      SchemaElement element1 = this._element;
      // ISSUE: variable of a boxed type
      __Boxed<byte> local4 = (System.ValueType) num3;
      nullable4 = facet4.MinValue;
      // ISSUE: variable of a boxed type
      __Boxed<int> local5 = (System.ValueType) nullable4.Value;
      nullable4 = facet4.MaxValue;
      // ISSUE: variable of a boxed type
      __Boxed<int> local6 = (System.ValueType) nullable4.Value;
      string name1 = primitiveType.Name;
      string str1 = System.Data.Entity.Resources.Strings.ScaleOutOfRange((object) local4, (object) local5, (object) local6, (object) name1);
      element1.AddError(ErrorCode.ScaleOutOfRange, EdmSchemaErrorSeverity.Error, (object) str1);
    }

    private void ValidatePrecisionFacetsForDateTimeFamily(
      EdmType type,
      Dictionary<string, Facet> facets)
    {
      PrimitiveType primitiveType = (PrimitiveType) type;
      byte? nullable1 = new byte?();
      Facet facet1;
      if (!facets.TryGetValue("Precision", out facet1) || facet1.Value == null)
        return;
      nullable1 = new byte?((byte) facet1.Value);
      FacetDescription facet2 = Helper.GetFacet((IEnumerable<FacetDescription>) primitiveType.FacetDescriptions, "Precision");
      byte? nullable2 = nullable1;
      int? nullable3 = nullable2.HasValue ? new int?((int) nullable2.GetValueOrDefault()) : new int?();
      int? nullable4 = facet2.MinValue;
      int num1 = nullable4.Value;
      if (!(nullable3.GetValueOrDefault() < num1 & nullable3.HasValue))
      {
        nullable2 = nullable1;
        int? nullable5;
        if (!nullable2.HasValue)
        {
          nullable4 = new int?();
          nullable5 = nullable4;
        }
        else
          nullable5 = new int?((int) nullable2.GetValueOrDefault());
        int? nullable6 = nullable5;
        nullable4 = facet2.MaxValue;
        int num2 = nullable4.Value;
        if (!(nullable6.GetValueOrDefault() > num2 & nullable6.HasValue))
          return;
      }
      SchemaElement element = this._element;
      // ISSUE: variable of a boxed type
      __Boxed<byte?> local1 = (System.ValueType) nullable1;
      int? nullable7 = facet2.MinValue;
      // ISSUE: variable of a boxed type
      __Boxed<int> local2 = (System.ValueType) nullable7.Value;
      nullable7 = facet2.MaxValue;
      // ISSUE: variable of a boxed type
      __Boxed<int> local3 = (System.ValueType) nullable7.Value;
      string name = primitiveType.Name;
      string str = System.Data.Entity.Resources.Strings.PrecisionOutOfRange((object) local1, (object) local2, (object) local3, (object) name);
      element.AddError(ErrorCode.PrecisionOutOfRange, EdmSchemaErrorSeverity.Error, (object) str);
    }

    private void ValidateAndSetStringFacets(EdmType type, Dictionary<string, Facet> facets) => this.ValidateLengthFacets(type, facets);

    private void ValidateLengthFacets(EdmType type, Dictionary<string, Facet> facets)
    {
      PrimitiveType primitiveType = (PrimitiveType) type;
      Facet facet1;
      if (!facets.TryGetValue("MaxLength", out facet1) || facet1.Value == null || Helper.IsUnboundedFacetValue(facet1))
        return;
      int num1 = (int) facet1.Value;
      FacetDescription facet2 = Helper.GetFacet((IEnumerable<FacetDescription>) primitiveType.FacetDescriptions, "MaxLength");
      int? nullable = facet2.MaxValue;
      int num2 = nullable.Value;
      nullable = facet2.MinValue;
      int num3 = nullable.Value;
      if (num1 >= num3 && num1 <= num2)
        return;
      this._element.AddError(ErrorCode.InvalidSize, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidSize((object) num1, (object) num3, (object) num2, (object) primitiveType.Name));
    }

    private void ValidateSpatialFacets(EdmType type, Dictionary<string, Facet> facets)
    {
      PrimitiveType primitiveType = (PrimitiveType) type;
      if (this._facetValues.ContainsKey("ConcurrencyMode"))
        this._element.AddError(ErrorCode.FacetNotAllowedByType, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.FacetNotAllowed((object) "ConcurrencyMode", (object) type.FullName));
      Facet facet1;
      if (this._element.Schema.DataModel == SchemaDataModelOption.EntityDataModel && (!facets.TryGetValue("IsStrict", out facet1) || (bool) facet1.Value))
        this._element.AddError(ErrorCode.UnexpectedSpatialType, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.SpatialWithUseStrongSpatialTypesFalse);
      Facet facet2;
      if (!facets.TryGetValue("SRID", out facet2) || facet2.Value == null || Helper.IsVariableFacetValue(facet2))
        return;
      int num1 = (int) facet2.Value;
      FacetDescription facet3 = Helper.GetFacet((IEnumerable<FacetDescription>) primitiveType.FacetDescriptions, "SRID");
      int? nullable = facet3.MaxValue;
      int num2 = nullable.Value;
      nullable = facet3.MinValue;
      int num3 = nullable.Value;
      if (num1 >= num3 && num1 <= num2)
        return;
      this._element.AddError(ErrorCode.InvalidSystemReferenceId, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidSystemReferenceId((object) num1, (object) num3, (object) num2, (object) primitiveType.Name));
    }

    internal void HandleMaxLengthAttribute(XmlReader reader)
    {
      if (reader.Value.Trim() == "Max")
      {
        this._facetValues.Add("MaxLength", (object) EdmConstants.UnboundedValue);
      }
      else
      {
        int field = 0;
        if (!this._element.HandleIntAttribute(reader, ref field))
          return;
        this._facetValues.Add("MaxLength", (object) field);
      }
    }

    internal void HandleSridAttribute(XmlReader reader)
    {
      if (reader.Value.Trim() == "Variable")
      {
        this._facetValues.Add("SRID", (object) EdmConstants.VariableValue);
      }
      else
      {
        int field = 0;
        if (!this._element.HandleIntAttribute(reader, ref field))
          return;
        this._facetValues.Add("SRID", (object) field);
      }
    }

    private void HandleNullableAttribute(XmlReader reader)
    {
      bool field = false;
      if (!this._element.HandleBoolAttribute(reader, ref field))
        return;
      this._facetValues.Add("Nullable", (object) field);
      this._nullable = new bool?(field);
    }

    internal void HandleStoreGeneratedPatternAttribute(XmlReader reader)
    {
      string str = reader.Value;
      StoreGeneratedPattern generatedPattern;
      if (str == "None")
        generatedPattern = StoreGeneratedPattern.None;
      else if (str == "Identity")
      {
        generatedPattern = StoreGeneratedPattern.Identity;
      }
      else
      {
        if (!(str == "Computed"))
          return;
        generatedPattern = StoreGeneratedPattern.Computed;
      }
      this._facetValues.Add("StoreGeneratedPattern", (object) generatedPattern);
    }

    internal void HandleConcurrencyModeAttribute(XmlReader reader)
    {
      string str = reader.Value;
      ConcurrencyMode concurrencyMode;
      if (str == "None")
      {
        concurrencyMode = ConcurrencyMode.None;
      }
      else
      {
        if (!(str == "Fixed"))
          return;
        concurrencyMode = ConcurrencyMode.Fixed;
      }
      this._facetValues.Add("ConcurrencyMode", (object) concurrencyMode);
    }

    private void HandleDefaultAttribute(XmlReader reader) => this._default = reader.Value;

    private void HandlePrecisionAttribute(XmlReader reader)
    {
      byte field = 0;
      if (!this._element.HandleByteAttribute(reader, ref field))
        return;
      this._facetValues.Add("Precision", (object) field);
    }

    private void HandleScaleAttribute(XmlReader reader)
    {
      byte field = 0;
      if (!this._element.HandleByteAttribute(reader, ref field))
        return;
      this._facetValues.Add("Scale", (object) field);
    }

    private void HandleUnicodeAttribute(XmlReader reader)
    {
      bool field = false;
      if (!this._element.HandleBoolAttribute(reader, ref field))
        return;
      this._facetValues.Add("Unicode", (object) field);
    }

    private void HandleCollationAttribute(XmlReader reader)
    {
      if (string.IsNullOrEmpty(reader.Value))
        return;
      this._facetValues.Add("Collation", (object) reader.Value);
    }

    private void HandleIsFixedLengthAttribute(XmlReader reader)
    {
      bool field = false;
      if (!this._element.HandleBoolAttribute(reader, ref field))
        return;
      this._facetValues.Add("FixedLength", (object) field);
    }

    internal void ValidateDefaultValue(SchemaType type)
    {
      if (this._default == null)
        return;
      if (type is ScalarType scalar)
        this.ValidateScalarMemberDefaultValue(scalar);
      else
        this._element.AddError(ErrorCode.DefaultNotAllowed, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.DefaultNotAllowed);
    }

    private void ValidateScalarMemberDefaultValue(ScalarType scalar)
    {
      if (scalar == null)
        return;
      switch (scalar.TypeKind)
      {
        case PrimitiveTypeKind.Binary:
          this.ValidateBinaryDefaultValue(scalar);
          break;
        case PrimitiveTypeKind.Boolean:
          this.ValidateBooleanDefaultValue(scalar);
          break;
        case PrimitiveTypeKind.Byte:
          this.ValidateIntegralDefaultValue(scalar, 0L, (long) byte.MaxValue);
          break;
        case PrimitiveTypeKind.DateTime:
          this.ValidateDateTimeDefaultValue(scalar);
          break;
        case PrimitiveTypeKind.Decimal:
          this.ValidateDecimalDefaultValue(scalar);
          break;
        case PrimitiveTypeKind.Double:
          this.ValidateFloatingPointDefaultValue(scalar, double.MinValue, double.MaxValue);
          break;
        case PrimitiveTypeKind.Guid:
          this.ValidateGuidDefaultValue(scalar);
          break;
        case PrimitiveTypeKind.Single:
          this.ValidateFloatingPointDefaultValue(scalar, -3.40282346638529E+38, 3.40282346638529E+38);
          break;
        case PrimitiveTypeKind.Int16:
          this.ValidateIntegralDefaultValue(scalar, (long) short.MinValue, (long) short.MaxValue);
          break;
        case PrimitiveTypeKind.Int32:
          this.ValidateIntegralDefaultValue(scalar, (long) int.MinValue, (long) int.MaxValue);
          break;
        case PrimitiveTypeKind.Int64:
          this.ValidateIntegralDefaultValue(scalar, long.MinValue, long.MaxValue);
          break;
        case PrimitiveTypeKind.String:
          this._defaultObject = (object) this._default;
          break;
        case PrimitiveTypeKind.Time:
          this.ValidateTimeDefaultValue(scalar);
          break;
        case PrimitiveTypeKind.DateTimeOffset:
          this.ValidateDateTimeOffsetDefaultValue(scalar);
          break;
        default:
          this._element.AddError(ErrorCode.DefaultNotAllowed, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.DefaultNotAllowed);
          break;
      }
    }

    private void ValidateBinaryDefaultValue(ScalarType scalar)
    {
      if (scalar.TryParse(this._default, out this._defaultObject))
        return;
      this._element.AddError(ErrorCode.InvalidDefault, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidDefaultBinaryWithNoMaxLength((object) this._default));
    }

    private void ValidateBooleanDefaultValue(ScalarType scalar)
    {
      if (scalar.TryParse(this._default, out this._defaultObject))
        return;
      this._element.AddError(ErrorCode.InvalidDefault, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidDefaultBoolean((object) this._default));
    }

    private void ValidateIntegralDefaultValue(ScalarType scalar, long minValue, long maxValue)
    {
      if (scalar.TryParse(this._default, out this._defaultObject))
        return;
      this._element.AddError(ErrorCode.InvalidDefault, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidDefaultIntegral((object) this._default, (object) minValue, (object) maxValue));
    }

    private void ValidateDateTimeDefaultValue(ScalarType scalar)
    {
      if (scalar.TryParse(this._default, out this._defaultObject))
        return;
      this._element.AddError(ErrorCode.InvalidDefault, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidDefaultDateTime((object) this._default, (object) "yyyy-MM-dd HH\\:mm\\:ss.fffZ".Replace("\\", "")));
    }

    private void ValidateTimeDefaultValue(ScalarType scalar)
    {
      if (scalar.TryParse(this._default, out this._defaultObject))
        return;
      this._element.AddError(ErrorCode.InvalidDefault, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidDefaultTime((object) this._default, (object) "HH\\:mm\\:ss.fffffffZ".Replace("\\", "")));
    }

    private void ValidateDateTimeOffsetDefaultValue(ScalarType scalar)
    {
      if (scalar.TryParse(this._default, out this._defaultObject))
        return;
      this._element.AddError(ErrorCode.InvalidDefault, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidDefaultDateTimeOffset((object) this._default, (object) "yyyy-MM-dd HH\\:mm\\:ss.fffffffz".Replace("\\", "")));
    }

    private void ValidateDecimalDefaultValue(ScalarType scalar)
    {
      if (scalar.TryParse(this._default, out this._defaultObject))
        return;
      this._element.AddError(ErrorCode.InvalidDefault, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidDefaultDecimal((object) this._default, (object) 38, (object) 38));
    }

    private void ValidateFloatingPointDefaultValue(
      ScalarType scalar,
      double minValue,
      double maxValue)
    {
      if (scalar.TryParse(this._default, out this._defaultObject))
        return;
      this._element.AddError(ErrorCode.InvalidDefault, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidDefaultFloatingPoint((object) this._default, (object) minValue, (object) maxValue));
    }

    private void ValidateGuidDefaultValue(ScalarType scalar)
    {
      if (scalar.TryParse(this._default, out this._defaultObject))
        return;
      this._element.AddError(ErrorCode.InvalidDefault, EdmSchemaErrorSeverity.Error, (object) System.Data.Entity.Resources.Strings.InvalidDefaultGuid((object) this._default));
    }
  }
}
