// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.Helper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal static class Helper
  {
    internal static readonly EdmMember[] EmptyArrayEdmProperty = new EdmMember[0];
    private static readonly Dictionary<PrimitiveTypeKind, long[]> _enumUnderlyingTypeRanges = new Dictionary<PrimitiveTypeKind, long[]>()
    {
      {
        PrimitiveTypeKind.Byte,
        new long[2]{ 0L, (long) byte.MaxValue }
      },
      {
        PrimitiveTypeKind.SByte,
        new long[2]{ (long) sbyte.MinValue, (long) sbyte.MaxValue }
      },
      {
        PrimitiveTypeKind.Int16,
        new long[2]{ (long) short.MinValue, (long) short.MaxValue }
      },
      {
        PrimitiveTypeKind.Int32,
        new long[2]{ (long) int.MinValue, (long) int.MaxValue }
      },
      {
        PrimitiveTypeKind.Int64,
        new long[2]{ long.MinValue, long.MaxValue }
      }
    };
    internal static readonly ReadOnlyCollection<KeyValuePair<string, object>> EmptyKeyValueStringObjectList = new ReadOnlyCollection<KeyValuePair<string, object>>((IList<KeyValuePair<string, object>>) new KeyValuePair<string, object>[0]);
    internal static readonly ReadOnlyCollection<string> EmptyStringList = new ReadOnlyCollection<string>((IList<string>) new string[0]);
    internal static readonly ReadOnlyCollection<FacetDescription> EmptyFacetDescriptionEnumerable = new ReadOnlyCollection<FacetDescription>((IList<FacetDescription>) new FacetDescription[0]);
    internal static readonly ReadOnlyCollection<EdmFunction> EmptyEdmFunctionReadOnlyCollection = new ReadOnlyCollection<EdmFunction>((IList<EdmFunction>) new EdmFunction[0]);
    internal static readonly ReadOnlyCollection<PrimitiveType> EmptyPrimitiveTypeReadOnlyCollection = new ReadOnlyCollection<PrimitiveType>((IList<PrimitiveType>) new PrimitiveType[0]);
    internal static readonly KeyValuePair<string, object>[] EmptyKeyValueStringObjectArray = new KeyValuePair<string, object>[0];
    internal const char PeriodSymbol = '.';
    internal const char CommaSymbol = ',';

    internal static string GetAttributeValue(XPathNavigator nav, string attributeName)
    {
      nav = nav.Clone();
      string str = (string) null;
      if (nav.MoveToAttribute(attributeName, string.Empty))
        str = nav.Value;
      return str;
    }

    internal static object GetTypedAttributeValue(
      XPathNavigator nav,
      string attributeName,
      Type clrType)
    {
      nav = nav.Clone();
      object obj = (object) null;
      if (nav.MoveToAttribute(attributeName, string.Empty))
        obj = nav.ValueAs(clrType);
      return obj;
    }

    internal static FacetDescription GetFacet(
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

    internal static bool IsAssignableFrom(EdmType firstType, EdmType secondType)
    {
      if (secondType == null)
        return false;
      return firstType.Equals((object) secondType) || Helper.IsSubtypeOf(secondType, firstType);
    }

    internal static bool IsSubtypeOf(EdmType firstType, EdmType secondType)
    {
      if (secondType == null)
        return false;
      for (EdmType baseType = firstType.BaseType; baseType != null; baseType = baseType.BaseType)
      {
        if (baseType == secondType)
          return true;
      }
      return false;
    }

    internal static IList GetAllStructuralMembers(EdmType edmType)
    {
      switch (edmType.BuiltInTypeKind)
      {
        case BuiltInTypeKind.AssociationType:
          return (IList) ((AssociationType) edmType).AssociationEndMembers;
        case BuiltInTypeKind.ComplexType:
          return (IList) ((ComplexType) edmType).Properties;
        case BuiltInTypeKind.EntityType:
          return (IList) ((EntityType) edmType).Properties;
        case BuiltInTypeKind.RowType:
          return (IList) ((RowType) edmType).Properties;
        default:
          return (IList) Helper.EmptyArrayEdmProperty;
      }
    }

    internal static AssociationEndMember GetEndThatShouldBeMappedToKey(
      AssociationType associationType)
    {
      if (associationType.AssociationEndMembers.Any<AssociationEndMember>((Func<AssociationEndMember, bool>) (it => it.RelationshipMultiplicity.Equals((object) RelationshipMultiplicity.One))))
        return associationType.AssociationEndMembers.SingleOrDefault<AssociationEndMember>((Func<AssociationEndMember, bool>) (it => it.RelationshipMultiplicity.Equals((object) RelationshipMultiplicity.Many) || it.RelationshipMultiplicity.Equals((object) RelationshipMultiplicity.ZeroOrOne)));
      return associationType.AssociationEndMembers.Any<AssociationEndMember>((Func<AssociationEndMember, bool>) (it => it.RelationshipMultiplicity.Equals((object) RelationshipMultiplicity.ZeroOrOne))) ? associationType.AssociationEndMembers.SingleOrDefault<AssociationEndMember>((Func<AssociationEndMember, bool>) (it => it.RelationshipMultiplicity.Equals((object) RelationshipMultiplicity.Many))) : (AssociationEndMember) null;
    }

    internal static string GetCommaDelimitedString(IEnumerable<string> stringList)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (string str in stringList)
      {
        if (!flag)
          stringBuilder.Append(", ");
        else
          flag = false;
        stringBuilder.Append(str);
      }
      return stringBuilder.ToString();
    }

    internal static IEnumerable<T> Concat<T>(params IEnumerable<T>[] sources)
    {
      IEnumerable<T>[] objsArray = sources;
      for (int index = 0; index < objsArray.Length; ++index)
      {
        IEnumerable<T> objs = objsArray[index];
        if (objs != null)
        {
          foreach (T obj in objs)
            yield return obj;
        }
      }
      objsArray = (IEnumerable<T>[]) null;
    }

    internal static void DisposeXmlReaders(IEnumerable<XmlReader> xmlReaders)
    {
      foreach (IDisposable xmlReader in xmlReaders)
        xmlReader.Dispose();
    }

    internal static bool IsStructuralType(EdmType type) => Helper.IsComplexType(type) || Helper.IsEntityType(type) || Helper.IsRelationshipType(type) || Helper.IsRowType((GlobalItem) type);

    internal static bool IsCollectionType(GlobalItem item) => BuiltInTypeKind.CollectionType == item.BuiltInTypeKind;

    internal static bool IsEntityType(EdmType type) => BuiltInTypeKind.EntityType == type.BuiltInTypeKind;

    internal static bool IsComplexType(EdmType type) => BuiltInTypeKind.ComplexType == type.BuiltInTypeKind;

    internal static bool IsPrimitiveType(EdmType type) => BuiltInTypeKind.PrimitiveType == type.BuiltInTypeKind;

    internal static bool IsRefType(GlobalItem item) => BuiltInTypeKind.RefType == item.BuiltInTypeKind;

    internal static bool IsRowType(GlobalItem item) => BuiltInTypeKind.RowType == item.BuiltInTypeKind;

    internal static bool IsAssociationType(EdmType type) => BuiltInTypeKind.AssociationType == type.BuiltInTypeKind;

    internal static bool IsRelationshipType(EdmType type) => BuiltInTypeKind.AssociationType == type.BuiltInTypeKind;

    internal static bool IsEdmProperty(EdmMember member) => BuiltInTypeKind.EdmProperty == member.BuiltInTypeKind;

    internal static bool IsRelationshipEndMember(EdmMember member) => member.BuiltInTypeKind == BuiltInTypeKind.AssociationEndMember;

    internal static bool IsAssociationEndMember(EdmMember member) => member.BuiltInTypeKind == BuiltInTypeKind.AssociationEndMember;

    internal static bool IsNavigationProperty(EdmMember member) => BuiltInTypeKind.NavigationProperty == member.BuiltInTypeKind;

    internal static bool IsEntityTypeBase(EdmType edmType) => Helper.IsEntityType(edmType) || Helper.IsRelationshipType(edmType);

    internal static bool IsTransientType(EdmType edmType) => Helper.IsCollectionType((GlobalItem) edmType) || Helper.IsRefType((GlobalItem) edmType) || Helper.IsRowType((GlobalItem) edmType);

    internal static bool IsAssociationSet(EntitySetBase entitySetBase) => BuiltInTypeKind.AssociationSet == entitySetBase.BuiltInTypeKind;

    internal static bool IsEntitySet(EntitySetBase entitySetBase) => BuiltInTypeKind.EntitySet == entitySetBase.BuiltInTypeKind;

    internal static bool IsRelationshipSet(EntitySetBase entitySetBase) => BuiltInTypeKind.AssociationSet == entitySetBase.BuiltInTypeKind;

    internal static bool IsEntityContainer(GlobalItem item) => BuiltInTypeKind.EntityContainer == item.BuiltInTypeKind;

    internal static bool IsEdmFunction(GlobalItem item) => BuiltInTypeKind.EdmFunction == item.BuiltInTypeKind;

    internal static string GetFileNameFromUri(Uri uri)
    {
      System.Data.Entity.Utilities.Check.NotNull<Uri>(uri, nameof (uri));
      if (uri.IsFile)
        return uri.LocalPath;
      return uri.IsAbsoluteUri ? uri.AbsolutePath : throw new ArgumentException(System.Data.Entity.Resources.Strings.UnacceptableUri((object) uri), nameof (uri));
    }

    internal static bool IsEnumType(EdmType edmType) => BuiltInTypeKind.EnumType == edmType.BuiltInTypeKind;

    internal static bool IsUnboundedFacetValue(Facet facet) => facet.Value == EdmConstants.UnboundedValue;

    internal static bool IsVariableFacetValue(Facet facet) => facet.Value == EdmConstants.VariableValue;

    internal static bool IsScalarType(EdmType edmType) => Helper.IsEnumType(edmType) || Helper.IsPrimitiveType(edmType);

    internal static bool IsHierarchyIdType(PrimitiveType type) => type.PrimitiveTypeKind == PrimitiveTypeKind.HierarchyId;

    internal static bool IsSpatialType(PrimitiveType type) => Helper.IsGeographicType(type) || Helper.IsGeometricType(type);

    internal static bool IsSpatialType(EdmType type, out bool isGeographic)
    {
      if (!(type is PrimitiveType type1))
      {
        isGeographic = false;
        return false;
      }
      isGeographic = Helper.IsGeographicType(type1);
      return isGeographic || Helper.IsGeometricType(type1);
    }

    internal static bool IsGeographicType(PrimitiveType type) => Helper.IsGeographicTypeKind(type.PrimitiveTypeKind);

    internal static bool AreSameSpatialUnionType(PrimitiveType firstType, PrimitiveType secondType) => Helper.IsGeographicTypeKind(firstType.PrimitiveTypeKind) && Helper.IsGeographicTypeKind(secondType.PrimitiveTypeKind) || Helper.IsGeometricTypeKind(firstType.PrimitiveTypeKind) && Helper.IsGeometricTypeKind(secondType.PrimitiveTypeKind);

    internal static bool IsGeographicTypeKind(PrimitiveTypeKind kind) => kind == PrimitiveTypeKind.Geography || Helper.IsStrongGeographicTypeKind(kind);

    internal static bool IsGeometricType(PrimitiveType type) => Helper.IsGeometricTypeKind(type.PrimitiveTypeKind);

    internal static bool IsGeometricTypeKind(PrimitiveTypeKind kind) => kind == PrimitiveTypeKind.Geometry || Helper.IsStrongGeometricTypeKind(kind);

    internal static bool IsStrongSpatialTypeKind(PrimitiveTypeKind kind) => Helper.IsStrongGeometricTypeKind(kind) || Helper.IsStrongGeographicTypeKind(kind);

    private static bool IsStrongGeometricTypeKind(PrimitiveTypeKind kind) => kind >= PrimitiveTypeKind.GeometryPoint && kind <= PrimitiveTypeKind.GeometryCollection;

    private static bool IsStrongGeographicTypeKind(PrimitiveTypeKind kind) => kind >= PrimitiveTypeKind.GeographyPoint && kind <= PrimitiveTypeKind.GeographyCollection;

    internal static bool IsHierarchyIdType(TypeUsage type) => type.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType && ((PrimitiveType) type.EdmType).PrimitiveTypeKind == PrimitiveTypeKind.HierarchyId;

    internal static bool IsSpatialType(TypeUsage type) => type.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType && Helper.IsSpatialType((PrimitiveType) type.EdmType);

    internal static bool IsSpatialType(TypeUsage type, out PrimitiveTypeKind spatialType)
    {
      if (type.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType)
      {
        PrimitiveType edmType = (PrimitiveType) type.EdmType;
        if (Helper.IsGeographicTypeKind(edmType.PrimitiveTypeKind) || Helper.IsGeometricTypeKind(edmType.PrimitiveTypeKind))
        {
          spatialType = edmType.PrimitiveTypeKind;
          return true;
        }
      }
      spatialType = PrimitiveTypeKind.Binary;
      return false;
    }

    internal static string ToString(ParameterDirection value)
    {
      switch (value)
      {
        case ParameterDirection.Input:
          return "Input";
        case ParameterDirection.Output:
          return "Output";
        case ParameterDirection.InputOutput:
          return "InputOutput";
        case ParameterDirection.ReturnValue:
          return "ReturnValue";
        default:
          return value.ToString();
      }
    }

    internal static string ToString(ParameterMode value)
    {
      switch (value)
      {
        case ParameterMode.In:
          return "In";
        case ParameterMode.Out:
          return "Out";
        case ParameterMode.InOut:
          return "InOut";
        case ParameterMode.ReturnValue:
          return "ReturnValue";
        default:
          return value.ToString();
      }
    }

    internal static bool IsSupportedEnumUnderlyingType(PrimitiveTypeKind typeKind) => typeKind == PrimitiveTypeKind.Byte || typeKind == PrimitiveTypeKind.SByte || (typeKind == PrimitiveTypeKind.Int16 || typeKind == PrimitiveTypeKind.Int32) || typeKind == PrimitiveTypeKind.Int64;

    internal static bool IsEnumMemberValueInRange(PrimitiveTypeKind underlyingTypeKind, long value) => value >= Helper._enumUnderlyingTypeRanges[underlyingTypeKind][0] && value <= Helper._enumUnderlyingTypeRanges[underlyingTypeKind][1];

    internal static PrimitiveType AsPrimitive(EdmType type) => !Helper.IsEnumType(type) ? (PrimitiveType) type : Helper.GetUnderlyingEdmTypeForEnumType(type);

    internal static PrimitiveType GetUnderlyingEdmTypeForEnumType(EdmType type) => ((EnumType) type).UnderlyingType;

    internal static PrimitiveType GetSpatialNormalizedPrimitiveType(EdmType type)
    {
      PrimitiveType type1 = (PrimitiveType) type;
      if (Helper.IsGeographicType(type1) && type1.PrimitiveTypeKind != PrimitiveTypeKind.Geography)
        return PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Geography);
      return Helper.IsGeometricType(type1) && type1.PrimitiveTypeKind != PrimitiveTypeKind.Geometry ? PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Geometry) : type1;
    }

    internal static string CombineErrorMessage(IEnumerable<EdmSchemaError> errors)
    {
      StringBuilder stringBuilder = new StringBuilder(Environment.NewLine);
      int num = 0;
      foreach (EdmSchemaError error in errors)
      {
        if (num++ != 0)
          stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append((object) error);
      }
      return stringBuilder.ToString();
    }

    internal static string CombineErrorMessage(IEnumerable<EdmItemError> errors)
    {
      StringBuilder stringBuilder = new StringBuilder(Environment.NewLine);
      int num = 0;
      foreach (EdmItemError error in errors)
      {
        if (num++ != 0)
          stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(error.Message);
      }
      return stringBuilder.ToString();
    }

    internal static IEnumerable<KeyValuePair<T, S>> PairEnumerations<T, S>(
      IBaseList<T> left,
      IEnumerable<S> right)
    {
      IEnumerator leftEnumerator = left.GetEnumerator();
      IEnumerator<S> rightEnumerator = right.GetEnumerator();
      while (leftEnumerator.MoveNext() && rightEnumerator.MoveNext())
        yield return new KeyValuePair<T, S>((T) leftEnumerator.Current, rightEnumerator.Current);
    }

    internal static TypeUsage GetModelTypeUsage(TypeUsage typeUsage) => typeUsage.ModelTypeUsage;

    internal static TypeUsage GetModelTypeUsage(EdmMember member) => Helper.GetModelTypeUsage(member.TypeUsage);

    internal static TypeUsage ValidateAndConvertTypeUsage(
      EdmProperty edmProperty,
      EdmProperty columnProperty)
    {
      return Helper.ValidateAndConvertTypeUsage(edmProperty.TypeUsage, columnProperty.TypeUsage);
    }

    internal static TypeUsage ValidateAndConvertTypeUsage(
      TypeUsage cspaceType,
      TypeUsage sspaceType)
    {
      TypeUsage storeType = sspaceType;
      if (sspaceType.EdmType.DataSpace == DataSpace.SSpace)
        storeType = sspaceType.ModelTypeUsage;
      return Helper.ValidateScalarTypesAreCompatible(cspaceType, storeType) ? storeType : (TypeUsage) null;
    }

    private static bool ValidateScalarTypesAreCompatible(TypeUsage cspaceType, TypeUsage storeType) => Helper.IsEnumType(cspaceType.EdmType) ? TypeSemantics.IsSubTypeOf(TypeUsage.Create((EdmType) Helper.GetUnderlyingEdmTypeForEnumType(cspaceType.EdmType)), storeType) : TypeSemantics.IsSubTypeOf(cspaceType, storeType);
  }
}
