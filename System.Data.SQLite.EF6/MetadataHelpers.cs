// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.MetadataHelpers
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.SQLite.EF6
{
  internal static class MetadataHelpers
  {
    internal const string NullableFacetName = "Nullable";
    internal static readonly int UnicodeStringMaxMaxLength = int.MaxValue;
    internal static readonly int AsciiStringMaxMaxLength = int.MaxValue;
    internal static readonly int BinaryMaxMaxLength = int.MaxValue;
    public static readonly string MaxLengthFacetName = "MaxLength";
    public static readonly string UnicodeFacetName = "Unicode";
    public static readonly string FixedLengthFacetName = "FixedLength";
    public static readonly string PreserveSecondsFacetName = "PreserveSeconds";
    public static readonly string PrecisionFacetName = "Precision";
    public static readonly string ScaleFacetName = "Scale";
    public static readonly string DefaultValueFacetName = "DefaultValue";

    internal static TEdmType GetEdmType<TEdmType>(TypeUsage typeUsage) where TEdmType : EdmType => (TEdmType) typeUsage.EdmType;

    internal static TypeUsage GetElementTypeUsage(TypeUsage type) => MetadataHelpers.IsCollectionType(type) ? ((CollectionType) type.EdmType).TypeUsage : (TypeUsage) null;

    internal static IList<EdmProperty> GetProperties(TypeUsage typeUsage) => MetadataHelpers.GetProperties(typeUsage.EdmType);

    internal static IList<EdmProperty> GetProperties(EdmType edmType)
    {
      switch (edmType.BuiltInTypeKind)
      {
        case BuiltInTypeKind.ComplexType:
          return (IList<EdmProperty>) ((ComplexType) edmType).Properties;
        case BuiltInTypeKind.EntityType:
          return (IList<EdmProperty>) ((EntityType) edmType).Properties;
        case BuiltInTypeKind.RowType:
          return (IList<EdmProperty>) ((RowType) edmType).Properties;
        default:
          return (IList<EdmProperty>) new List<EdmProperty>();
      }
    }

    internal static bool IsCollectionType(TypeUsage typeUsage) => MetadataHelpers.IsCollectionType(typeUsage.EdmType);

    internal static bool IsCollectionType(EdmType type) => BuiltInTypeKind.CollectionType == type.BuiltInTypeKind;

    internal static bool IsPrimitiveType(TypeUsage type) => MetadataHelpers.IsPrimitiveType(type.EdmType);

    internal static bool IsPrimitiveType(EdmType type) => BuiltInTypeKind.PrimitiveType == type.BuiltInTypeKind;

    internal static bool IsRowType(TypeUsage type) => MetadataHelpers.IsRowType(type.EdmType);

    internal static bool IsRowType(EdmType type) => BuiltInTypeKind.RowType == type.BuiltInTypeKind;

    internal static bool TryGetPrimitiveTypeKind(TypeUsage type, out PrimitiveTypeKind typeKind)
    {
      if (type != null && type.EdmType != null && type.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType)
      {
        typeKind = ((PrimitiveType) type.EdmType).PrimitiveTypeKind;
        return true;
      }
      typeKind = PrimitiveTypeKind.Binary;
      return false;
    }

    internal static PrimitiveTypeKind GetPrimitiveTypeKind(TypeUsage type)
    {
      PrimitiveTypeKind typeKind;
      if (!MetadataHelpers.TryGetPrimitiveTypeKind(type, out typeKind))
        throw new NotSupportedException("Cannot create parameter of non-primitive type");
      return typeKind;
    }

    internal static T TryGetValueForMetadataProperty<T>(MetadataItem item, string propertyName)
    {
      MetadataProperty metadataProperty;
      return !item.MetadataProperties.TryGetValue(propertyName, true, out metadataProperty) ? default (T) : (T) metadataProperty.Value;
    }

    internal static bool IsPrimitiveType(TypeUsage type, PrimitiveTypeKind primitiveType)
    {
      PrimitiveTypeKind typeKind;
      return MetadataHelpers.TryGetPrimitiveTypeKind(type, out typeKind) && typeKind == primitiveType;
    }

    internal static DbType GetDbType(PrimitiveTypeKind primitiveType)
    {
      switch (primitiveType)
      {
        case PrimitiveTypeKind.Binary:
          return DbType.Binary;
        case PrimitiveTypeKind.Boolean:
          return DbType.Boolean;
        case PrimitiveTypeKind.Byte:
          return DbType.Byte;
        case PrimitiveTypeKind.DateTime:
          return DbType.DateTime;
        case PrimitiveTypeKind.Decimal:
          return DbType.Decimal;
        case PrimitiveTypeKind.Double:
          return DbType.Double;
        case PrimitiveTypeKind.Guid:
          return DbType.Guid;
        case PrimitiveTypeKind.Single:
          return DbType.Single;
        case PrimitiveTypeKind.SByte:
          return DbType.SByte;
        case PrimitiveTypeKind.Int16:
          return DbType.Int16;
        case PrimitiveTypeKind.Int32:
          return DbType.Int32;
        case PrimitiveTypeKind.Int64:
          return DbType.Int64;
        case PrimitiveTypeKind.String:
          return DbType.String;
        default:
          throw new InvalidOperationException(string.Format("Unknown PrimitiveTypeKind {0}", (object) primitiveType));
      }
    }

    internal static T GetFacetValueOrDefault<T>(TypeUsage type, string facetName, T defaultValue)
    {
      Facet facet;
      return type.Facets.TryGetValue(facetName, false, out facet) && facet.Value != null && !facet.IsUnbounded ? (T) facet.Value : defaultValue;
    }

    internal static bool IsFacetValueConstant(TypeUsage type, string facetName) => MetadataHelpers.GetFacet((IEnumerable<FacetDescription>) ((PrimitiveType) type.EdmType).FacetDescriptions, facetName).IsConstant;

    private static FacetDescription GetFacet(
      IEnumerable<FacetDescription> facetCollection,
      string facetName)
    {
      foreach (FacetDescription facet in facetCollection)
      {
        if (facet.FacetName == facetName)
          return facet;
      }
      return (FacetDescription) null;
    }

    internal static bool TryGetTypeFacetDescriptionByName(
      EdmType edmType,
      string facetName,
      out FacetDescription facetDescription)
    {
      facetDescription = (FacetDescription) null;
      if (MetadataHelpers.IsPrimitiveType(edmType))
      {
        foreach (FacetDescription facetDescription1 in ((PrimitiveType) edmType).FacetDescriptions)
        {
          if (facetName.Equals(facetDescription1.FacetName, StringComparison.OrdinalIgnoreCase))
          {
            facetDescription = facetDescription1;
            return true;
          }
        }
      }
      return false;
    }

    internal static bool IsNullable(TypeUsage type)
    {
      Facet facet;
      return type.Facets.TryGetValue("Nullable", false, out facet) && (bool) facet.Value;
    }

    internal static bool TryGetMaxLength(TypeUsage type, out int maxLength)
    {
      if (MetadataHelpers.IsPrimitiveType(type, PrimitiveTypeKind.String) || MetadataHelpers.IsPrimitiveType(type, PrimitiveTypeKind.Binary))
        return MetadataHelpers.TryGetIntFacetValue(type, MetadataHelpers.MaxLengthFacetName, out maxLength);
      maxLength = 0;
      return false;
    }

    internal static bool TryGetIntFacetValue(TypeUsage type, string facetName, out int intValue)
    {
      intValue = 0;
      Facet facet;
      if (!type.Facets.TryGetValue(facetName, false, out facet) || facet.Value == null || facet.IsUnbounded)
        return false;
      intValue = (int) facet.Value;
      return true;
    }

    internal static bool TryGetIsFixedLength(TypeUsage type, out bool isFixedLength)
    {
      if (MetadataHelpers.IsPrimitiveType(type, PrimitiveTypeKind.String) || MetadataHelpers.IsPrimitiveType(type, PrimitiveTypeKind.Binary))
        return MetadataHelpers.TryGetBooleanFacetValue(type, MetadataHelpers.FixedLengthFacetName, out isFixedLength);
      isFixedLength = false;
      return false;
    }

    internal static bool TryGetBooleanFacetValue(
      TypeUsage type,
      string facetName,
      out bool boolValue)
    {
      boolValue = false;
      Facet facet;
      if (!type.Facets.TryGetValue(facetName, false, out facet) || facet.Value == null)
        return false;
      boolValue = (bool) facet.Value;
      return true;
    }

    internal static bool TryGetIsUnicode(TypeUsage type, out bool isUnicode)
    {
      if (MetadataHelpers.IsPrimitiveType(type, PrimitiveTypeKind.String))
        return MetadataHelpers.TryGetBooleanFacetValue(type, MetadataHelpers.UnicodeFacetName, out isUnicode);
      isUnicode = false;
      return false;
    }

    internal static bool IsCanonicalFunction(EdmFunction function) => function.NamespaceName == "Edm";

    internal static bool IsStoreFunction(EdmFunction function) => !MetadataHelpers.IsCanonicalFunction(function);

    internal static ParameterDirection ParameterModeToParameterDirection(
      ParameterMode mode)
    {
      switch (mode)
      {
        case ParameterMode.In:
          return ParameterDirection.Input;
        case ParameterMode.Out:
          return ParameterDirection.Output;
        case ParameterMode.InOut:
          return ParameterDirection.InputOutput;
        case ParameterMode.ReturnValue:
          return ParameterDirection.ReturnValue;
        default:
          return (ParameterDirection) 0;
      }
    }
  }
}
