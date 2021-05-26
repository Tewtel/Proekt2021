// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.TypeSemantics
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm.Provider;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal static class TypeSemantics
  {
    private static ReadOnlyCollection<PrimitiveType>[,] _commonTypeClosure;

    internal static bool IsEqual(TypeUsage type1, TypeUsage type2) => TypeSemantics.CompareTypes(type1, type2, false);

    internal static bool IsStructurallyEqual(TypeUsage fromType, TypeUsage toType) => TypeSemantics.CompareTypes(fromType, toType, true);

    internal static bool IsStructurallyEqualOrPromotableTo(TypeUsage fromType, TypeUsage toType) => TypeSemantics.IsStructurallyEqual(fromType, toType) || TypeSemantics.IsPromotableTo(fromType, toType);

    internal static bool IsStructurallyEqualOrPromotableTo(EdmType fromType, EdmType toType) => TypeSemantics.IsStructurallyEqualOrPromotableTo(TypeUsage.Create(fromType), TypeUsage.Create(toType));

    internal static bool IsSubTypeOf(TypeUsage subType, TypeUsage superType)
    {
      if (subType.EdmEquals((MetadataItem) superType))
        return true;
      return Helper.IsPrimitiveType(subType.EdmType) && Helper.IsPrimitiveType(superType.EdmType) ? TypeSemantics.IsPrimitiveTypeSubTypeOf(subType, superType) : subType.IsSubtypeOf(superType);
    }

    internal static bool IsSubTypeOf(EdmType subEdmType, EdmType superEdmType) => subEdmType.IsSubtypeOf(superEdmType);

    internal static bool IsPromotableTo(TypeUsage fromType, TypeUsage toType)
    {
      if (toType.EdmType.EdmEquals((MetadataItem) fromType.EdmType))
        return true;
      if (Helper.IsPrimitiveType(fromType.EdmType) && Helper.IsPrimitiveType(toType.EdmType))
        return TypeSemantics.IsPrimitiveTypePromotableTo(fromType, toType);
      if (Helper.IsCollectionType((GlobalItem) fromType.EdmType) && Helper.IsCollectionType((GlobalItem) toType.EdmType))
        return TypeSemantics.IsPromotableTo(TypeHelpers.GetElementTypeUsage(fromType), TypeHelpers.GetElementTypeUsage(toType));
      if (Helper.IsEntityTypeBase(fromType.EdmType) && Helper.IsEntityTypeBase(toType.EdmType))
        return fromType.EdmType.IsSubtypeOf(toType.EdmType);
      if (Helper.IsRefType((GlobalItem) fromType.EdmType) && Helper.IsRefType((GlobalItem) toType.EdmType))
        return TypeSemantics.IsPromotableTo(TypeHelpers.GetElementTypeUsage(fromType), TypeHelpers.GetElementTypeUsage(toType));
      return Helper.IsRowType((GlobalItem) fromType.EdmType) && Helper.IsRowType((GlobalItem) toType.EdmType) && TypeSemantics.IsPromotableTo((RowType) fromType.EdmType, (RowType) toType.EdmType);
    }

    internal static IEnumerable<TypeUsage> FlattenType(TypeUsage type)
    {
      Func<TypeUsage, bool> isLeaf = (Func<TypeUsage, bool>) (t => !Helper.IsTransientType(t.EdmType));
      Func<TypeUsage, IEnumerable<TypeUsage>> getImmediateSubNodes = (Func<TypeUsage, IEnumerable<TypeUsage>>) (t => Helper.IsCollectionType((GlobalItem) t.EdmType) || Helper.IsRefType((GlobalItem) t.EdmType) ? (IEnumerable<TypeUsage>) new TypeUsage[1]
      {
        TypeHelpers.GetElementTypeUsage(t)
      } : (Helper.IsRowType((GlobalItem) t.EdmType) ? ((RowType) t.EdmType).Properties.Select<EdmProperty, TypeUsage>((Func<EdmProperty, TypeUsage>) (p => p.TypeUsage)) : (IEnumerable<TypeUsage>) new TypeUsage[0]));
      return Helpers.GetLeafNodes<TypeUsage>(type, isLeaf, getImmediateSubNodes);
    }

    internal static bool IsCastAllowed(TypeUsage fromType, TypeUsage toType)
    {
      if (Helper.IsPrimitiveType(fromType.EdmType) && Helper.IsPrimitiveType(toType.EdmType) || Helper.IsPrimitiveType(fromType.EdmType) && Helper.IsEnumType(toType.EdmType) || Helper.IsEnumType(fromType.EdmType) && Helper.IsPrimitiveType(toType.EdmType))
        return true;
      return Helper.IsEnumType(fromType.EdmType) && Helper.IsEnumType(toType.EdmType) && fromType.EdmType.Equals((object) toType.EdmType);
    }

    internal static bool TryGetCommonType(
      TypeUsage type1,
      TypeUsage type2,
      out TypeUsage commonType)
    {
      commonType = (TypeUsage) null;
      if (type1.EdmEquals((MetadataItem) type2))
      {
        commonType = TypeSemantics.ForgetConstraints(type2);
        return true;
      }
      if (Helper.IsPrimitiveType(type1.EdmType) && Helper.IsPrimitiveType(type2.EdmType))
        return TypeSemantics.TryGetCommonPrimitiveType(type1, type2, out commonType);
      EdmType commonEdmType;
      if (TypeSemantics.TryGetCommonType(type1.EdmType, type2.EdmType, out commonEdmType))
      {
        commonType = TypeSemantics.ForgetConstraints(TypeUsage.Create(commonEdmType));
        return true;
      }
      commonType = (TypeUsage) null;
      return false;
    }

    internal static TypeUsage GetCommonType(TypeUsage type1, TypeUsage type2)
    {
      TypeUsage commonType = (TypeUsage) null;
      return TypeSemantics.TryGetCommonType(type1, type2, out commonType) ? commonType : (TypeUsage) null;
    }

    internal static bool IsAggregateFunction(EdmFunction function) => function.AggregateAttribute;

    internal static bool IsValidPolymorphicCast(TypeUsage fromType, TypeUsage toType)
    {
      if (!TypeSemantics.IsPolymorphicType(fromType) || !TypeSemantics.IsPolymorphicType(toType))
        return false;
      return TypeSemantics.IsStructurallyEqual(fromType, toType) || TypeSemantics.IsSubTypeOf(fromType, toType) || TypeSemantics.IsSubTypeOf(toType, fromType);
    }

    internal static bool IsValidPolymorphicCast(EdmType fromEdmType, EdmType toEdmType) => TypeSemantics.IsValidPolymorphicCast(TypeUsage.Create(fromEdmType), TypeUsage.Create(toEdmType));

    internal static bool IsNominalType(TypeUsage type) => TypeSemantics.IsEntityType(type) || TypeSemantics.IsComplexType(type);

    internal static bool IsCollectionType(TypeUsage type) => Helper.IsCollectionType((GlobalItem) type.EdmType);

    internal static bool IsComplexType(TypeUsage type) => BuiltInTypeKind.ComplexType == type.EdmType.BuiltInTypeKind;

    internal static bool IsEntityType(TypeUsage type) => Helper.IsEntityType(type.EdmType);

    internal static bool IsRelationshipType(TypeUsage type) => BuiltInTypeKind.AssociationType == type.EdmType.BuiltInTypeKind;

    internal static bool IsEnumerationType(TypeUsage type) => Helper.IsEnumType(type.EdmType);

    internal static bool IsScalarType(TypeUsage type) => TypeSemantics.IsScalarType(type.EdmType);

    internal static bool IsScalarType(EdmType type) => Helper.IsPrimitiveType(type) || Helper.IsEnumType(type);

    internal static bool IsNumericType(TypeUsage type) => TypeSemantics.IsIntegerNumericType(type) || TypeSemantics.IsFixedPointNumericType(type) || TypeSemantics.IsFloatPointNumericType(type);

    internal static bool IsIntegerNumericType(TypeUsage type)
    {
      PrimitiveTypeKind typeKind;
      if (!TypeHelpers.TryGetPrimitiveTypeKind(type, out typeKind))
        return false;
      switch (typeKind)
      {
        case PrimitiveTypeKind.Byte:
        case PrimitiveTypeKind.SByte:
        case PrimitiveTypeKind.Int16:
        case PrimitiveTypeKind.Int32:
        case PrimitiveTypeKind.Int64:
          return true;
        default:
          return false;
      }
    }

    internal static bool IsFixedPointNumericType(TypeUsage type)
    {
      PrimitiveTypeKind typeKind;
      return TypeHelpers.TryGetPrimitiveTypeKind(type, out typeKind) && typeKind == PrimitiveTypeKind.Decimal;
    }

    internal static bool IsFloatPointNumericType(TypeUsage type)
    {
      PrimitiveTypeKind typeKind;
      if (!TypeHelpers.TryGetPrimitiveTypeKind(type, out typeKind))
        return false;
      return typeKind == PrimitiveTypeKind.Double || typeKind == PrimitiveTypeKind.Single;
    }

    internal static bool IsUnsignedNumericType(TypeUsage type)
    {
      PrimitiveTypeKind typeKind;
      return TypeHelpers.TryGetPrimitiveTypeKind(type, out typeKind) && typeKind == PrimitiveTypeKind.Byte;
    }

    internal static bool IsPolymorphicType(TypeUsage type) => TypeSemantics.IsEntityType(type) || TypeSemantics.IsComplexType(type);

    internal static bool IsBooleanType(TypeUsage type) => TypeSemantics.IsPrimitiveType(type, PrimitiveTypeKind.Boolean);

    internal static bool IsPrimitiveType(TypeUsage type) => Helper.IsPrimitiveType(type.EdmType);

    internal static bool IsPrimitiveType(TypeUsage type, PrimitiveTypeKind primitiveTypeKind)
    {
      PrimitiveTypeKind typeKind;
      return TypeHelpers.TryGetPrimitiveTypeKind(type, out typeKind) && typeKind == primitiveTypeKind;
    }

    internal static bool IsRowType(TypeUsage type) => Helper.IsRowType((GlobalItem) type.EdmType);

    internal static bool IsReferenceType(TypeUsage type) => Helper.IsRefType((GlobalItem) type.EdmType);

    internal static bool IsSpatialType(TypeUsage type) => Helper.IsSpatialType(type);

    internal static bool IsStrongSpatialType(TypeUsage type) => TypeSemantics.IsPrimitiveType(type) && Helper.IsStrongSpatialTypeKind(((PrimitiveType) type.EdmType).PrimitiveTypeKind);

    internal static bool IsStructuralType(TypeUsage type) => Helper.IsStructuralType(type.EdmType);

    internal static bool IsPartOfKey(EdmMember edmMember)
    {
      if (Helper.IsRelationshipEndMember(edmMember))
        return ((EntityTypeBase) edmMember.DeclaringType).KeyMembers.Contains(edmMember);
      return Helper.IsEdmProperty(edmMember) && Helper.IsEntityTypeBase((EdmType) edmMember.DeclaringType) && ((EntityTypeBase) edmMember.DeclaringType).KeyMembers.Contains(edmMember);
    }

    internal static bool IsNullable(TypeUsage type)
    {
      Facet facet;
      return !type.Facets.TryGetValue("Nullable", false, out facet) || (bool) facet.Value;
    }

    internal static bool IsNullable(EdmMember edmMember) => TypeSemantics.IsNullable(edmMember.TypeUsage);

    internal static bool IsEqualComparable(TypeUsage type) => TypeSemantics.IsEqualComparable(type.EdmType);

    internal static bool IsEqualComparableTo(TypeUsage type1, TypeUsage type2) => TypeSemantics.IsEqualComparable(type1) && TypeSemantics.IsEqualComparable(type2) && TypeSemantics.HasCommonType(type1, type2);

    internal static bool IsOrderComparable(TypeUsage type) => TypeSemantics.IsOrderComparable(type.EdmType);

    internal static bool IsOrderComparableTo(TypeUsage type1, TypeUsage type2) => TypeSemantics.IsOrderComparable(type1) && TypeSemantics.IsOrderComparable(type2) && TypeSemantics.HasCommonType(type1, type2);

    internal static TypeUsage ForgetConstraints(TypeUsage type) => Helper.IsPrimitiveType(type.EdmType) ? EdmProviderManifest.Instance.ForgetScalarConstraints(type) : type;

    [Conditional("DEBUG")]
    internal static void AssertTypeInvariant(string message, Func<bool> assertPredicate)
    {
    }

    private static bool IsPrimitiveTypeSubTypeOf(TypeUsage fromType, TypeUsage toType) => TypeSemantics.IsSubTypeOf((PrimitiveType) fromType.EdmType, (PrimitiveType) toType.EdmType);

    private static bool IsSubTypeOf(
      PrimitiveType subPrimitiveType,
      PrimitiveType superPrimitiveType)
    {
      return subPrimitiveType == superPrimitiveType || Helper.AreSameSpatialUnionType(subPrimitiveType, superPrimitiveType) || -1 != EdmProviderManifest.Instance.GetPromotionTypes(subPrimitiveType).IndexOf(superPrimitiveType);
    }

    private static bool IsPromotableTo(RowType fromRowType, RowType toRowType)
    {
      if (fromRowType.Properties.Count != toRowType.Properties.Count)
        return false;
      for (int index = 0; index < fromRowType.Properties.Count; ++index)
      {
        if (!TypeSemantics.IsPromotableTo(fromRowType.Properties[index].TypeUsage, toRowType.Properties[index].TypeUsage))
          return false;
      }
      return true;
    }

    private static bool IsPrimitiveTypePromotableTo(TypeUsage fromType, TypeUsage toType) => TypeSemantics.IsSubTypeOf((PrimitiveType) fromType.EdmType, (PrimitiveType) toType.EdmType);

    private static bool TryGetCommonType(
      EdmType edmType1,
      EdmType edmType2,
      out EdmType commonEdmType)
    {
      if (edmType2 == edmType1)
      {
        commonEdmType = edmType1;
        return true;
      }
      if (Helper.IsPrimitiveType(edmType1) && Helper.IsPrimitiveType(edmType2))
        return TypeSemantics.TryGetCommonType((PrimitiveType) edmType1, (PrimitiveType) edmType2, out commonEdmType);
      if (Helper.IsCollectionType((GlobalItem) edmType1) && Helper.IsCollectionType((GlobalItem) edmType2))
        return TypeSemantics.TryGetCommonType((CollectionType) edmType1, (CollectionType) edmType2, out commonEdmType);
      if (Helper.IsEntityTypeBase(edmType1) && Helper.IsEntityTypeBase(edmType2))
        return TypeSemantics.TryGetCommonBaseType(edmType1, edmType2, out commonEdmType);
      if (Helper.IsRefType((GlobalItem) edmType1) && Helper.IsRefType((GlobalItem) edmType2))
        return TypeSemantics.TryGetCommonType((RefType) edmType1, (RefType) edmType2, out commonEdmType);
      if (Helper.IsRowType((GlobalItem) edmType1) && Helper.IsRowType((GlobalItem) edmType2))
        return TypeSemantics.TryGetCommonType((RowType) edmType1, (RowType) edmType2, out commonEdmType);
      commonEdmType = (EdmType) null;
      return false;
    }

    private static bool TryGetCommonPrimitiveType(
      TypeUsage type1,
      TypeUsage type2,
      out TypeUsage commonType)
    {
      commonType = (TypeUsage) null;
      if (TypeSemantics.IsPromotableTo(type1, type2))
      {
        commonType = TypeSemantics.ForgetConstraints(type2);
        return true;
      }
      if (TypeSemantics.IsPromotableTo(type2, type1))
      {
        commonType = TypeSemantics.ForgetConstraints(type1);
        return true;
      }
      ReadOnlyCollection<PrimitiveType> commonSuperTypes = TypeSemantics.GetPrimitiveCommonSuperTypes((PrimitiveType) type1.EdmType, (PrimitiveType) type2.EdmType);
      if (commonSuperTypes.Count == 0)
        return false;
      commonType = TypeUsage.CreateDefaultTypeUsage((EdmType) commonSuperTypes[0]);
      return commonType != null;
    }

    private static bool TryGetCommonType(
      PrimitiveType primitiveType1,
      PrimitiveType primitiveType2,
      out EdmType commonType)
    {
      commonType = (EdmType) null;
      if (TypeSemantics.IsSubTypeOf(primitiveType1, primitiveType2))
      {
        commonType = (EdmType) primitiveType2;
        return true;
      }
      if (TypeSemantics.IsSubTypeOf(primitiveType2, primitiveType1))
      {
        commonType = (EdmType) primitiveType1;
        return true;
      }
      ReadOnlyCollection<PrimitiveType> commonSuperTypes = TypeSemantics.GetPrimitiveCommonSuperTypes(primitiveType1, primitiveType2);
      if (commonSuperTypes.Count <= 0)
        return false;
      commonType = (EdmType) commonSuperTypes[0];
      return true;
    }

    private static bool TryGetCommonType(
      CollectionType collectionType1,
      CollectionType collectionType2,
      out EdmType commonType)
    {
      TypeUsage commonType1 = (TypeUsage) null;
      if (!TypeSemantics.TryGetCommonType(collectionType1.TypeUsage, collectionType2.TypeUsage, out commonType1))
      {
        commonType = (EdmType) null;
        return false;
      }
      commonType = (EdmType) new CollectionType(commonType1);
      return true;
    }

    private static bool TryGetCommonType(
      RefType refType1,
      RefType reftype2,
      out EdmType commonType)
    {
      if (!TypeSemantics.TryGetCommonType((EdmType) refType1.ElementType, (EdmType) reftype2.ElementType, out commonType))
        return false;
      commonType = (EdmType) new RefType((EntityType) commonType);
      return true;
    }

    private static bool TryGetCommonType(
      RowType rowType1,
      RowType rowType2,
      out EdmType commonRowType)
    {
      if (rowType1.Properties.Count != rowType2.Properties.Count || rowType1.InitializerMetadata != rowType2.InitializerMetadata)
      {
        commonRowType = (EdmType) null;
        return false;
      }
      List<EdmProperty> edmPropertyList = new List<EdmProperty>();
      for (int index = 0; index < rowType1.Properties.Count; ++index)
      {
        TypeUsage commonType;
        if (!TypeSemantics.TryGetCommonType(rowType1.Properties[index].TypeUsage, rowType2.Properties[index].TypeUsage, out commonType))
        {
          commonRowType = (EdmType) null;
          return false;
        }
        edmPropertyList.Add(new EdmProperty(rowType1.Properties[index].Name, commonType));
      }
      commonRowType = (EdmType) new RowType((IEnumerable<EdmProperty>) edmPropertyList, rowType1.InitializerMetadata);
      return true;
    }

    internal static bool TryGetCommonBaseType(
      EdmType type1,
      EdmType type2,
      out EdmType commonBaseType)
    {
      Dictionary<EdmType, byte> dictionary = new Dictionary<EdmType, byte>();
      for (EdmType key = type2; key != null; key = key.BaseType)
        dictionary.Add(key, (byte) 0);
      for (EdmType key = type1; key != null; key = key.BaseType)
      {
        if (dictionary.ContainsKey(key))
        {
          commonBaseType = key;
          return true;
        }
      }
      commonBaseType = (EdmType) null;
      return false;
    }

    private static bool HasCommonType(TypeUsage type1, TypeUsage type2) => TypeHelpers.GetCommonTypeUsage(type1, type2) != null;

    private static bool IsEqualComparable(EdmType edmType)
    {
      if (Helper.IsPrimitiveType(edmType) || Helper.IsRefType((GlobalItem) edmType) || (Helper.IsEntityType(edmType) || Helper.IsEnumType(edmType)))
        return true;
      if (!Helper.IsRowType((GlobalItem) edmType))
        return false;
      foreach (EdmMember property in ((RowType) edmType).Properties)
      {
        if (!TypeSemantics.IsEqualComparable(property.TypeUsage))
          return false;
      }
      return true;
    }

    private static bool IsOrderComparable(EdmType edmType) => Helper.IsScalarType(edmType);

    private static bool CompareTypes(TypeUsage fromType, TypeUsage toType, bool equivalenceOnly)
    {
      if (fromType == toType)
        return true;
      if (fromType.EdmType.BuiltInTypeKind != toType.EdmType.BuiltInTypeKind)
        return false;
      if (fromType.EdmType.BuiltInTypeKind == BuiltInTypeKind.CollectionType)
        return TypeSemantics.CompareTypes(((CollectionType) fromType.EdmType).TypeUsage, ((CollectionType) toType.EdmType).TypeUsage, equivalenceOnly);
      if (fromType.EdmType.BuiltInTypeKind == BuiltInTypeKind.RefType)
        return ((RefType) fromType.EdmType).ElementType.EdmEquals((MetadataItem) ((RefType) toType.EdmType).ElementType);
      if (fromType.EdmType.BuiltInTypeKind != BuiltInTypeKind.RowType)
        return fromType.EdmType.EdmEquals((MetadataItem) toType.EdmType);
      RowType edmType1 = (RowType) fromType.EdmType;
      RowType edmType2 = (RowType) toType.EdmType;
      if (edmType1.Properties.Count != edmType2.Properties.Count)
        return false;
      for (int index = 0; index < edmType1.Properties.Count; ++index)
      {
        EdmProperty property1 = edmType1.Properties[index];
        EdmProperty property2 = edmType2.Properties[index];
        if (!equivalenceOnly && property1.Name != property2.Name || !TypeSemantics.CompareTypes(property1.TypeUsage, property2.TypeUsage, equivalenceOnly))
          return false;
      }
      return true;
    }

    private static void ComputeCommonTypeClosure()
    {
      if (TypeSemantics._commonTypeClosure != null)
        return;
      ReadOnlyCollection<PrimitiveType>[,] readOnlyCollectionArray = new ReadOnlyCollection<PrimitiveType>[32, 32];
      for (int index = 0; index < 32; ++index)
        readOnlyCollectionArray[index, index] = Helper.EmptyPrimitiveTypeReadOnlyCollection;
      ReadOnlyCollection<PrimitiveType> storeTypes = EdmProviderManifest.Instance.GetStoreTypes();
      for (int index1 = 0; index1 < 32; ++index1)
      {
        for (int index2 = 0; index2 < index1; ++index2)
        {
          readOnlyCollectionArray[index1, index2] = TypeSemantics.Intersect((IList<PrimitiveType>) EdmProviderManifest.Instance.GetPromotionTypes(storeTypes[index1]), (IList<PrimitiveType>) EdmProviderManifest.Instance.GetPromotionTypes(storeTypes[index2]));
          readOnlyCollectionArray[index2, index1] = readOnlyCollectionArray[index1, index2];
        }
      }
      Interlocked.CompareExchange<ReadOnlyCollection<PrimitiveType>[,]>(ref TypeSemantics._commonTypeClosure, readOnlyCollectionArray, (ReadOnlyCollection<PrimitiveType>[,]) null);
    }

    private static ReadOnlyCollection<PrimitiveType> Intersect(
      IList<PrimitiveType> types1,
      IList<PrimitiveType> types2)
    {
      List<PrimitiveType> primitiveTypeList = new List<PrimitiveType>();
      for (int index = 0; index < types1.Count; ++index)
      {
        if (types2.Contains(types1[index]))
          primitiveTypeList.Add(types1[index]);
      }
      return primitiveTypeList.Count == 0 ? Helper.EmptyPrimitiveTypeReadOnlyCollection : new ReadOnlyCollection<PrimitiveType>((IList<PrimitiveType>) primitiveTypeList);
    }

    private static ReadOnlyCollection<PrimitiveType> GetPrimitiveCommonSuperTypes(
      PrimitiveType primitiveType1,
      PrimitiveType primitiveType2)
    {
      TypeSemantics.ComputeCommonTypeClosure();
      return TypeSemantics._commonTypeClosure[(int) primitiveType1.PrimitiveTypeKind, (int) primitiveType2.PrimitiveTypeKind];
    }
  }
}
