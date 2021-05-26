// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.PropertyInfoExtensions
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.SqlServer.Utilities
{
  internal static class PropertyInfoExtensions
  {
    public static bool IsSameAs(this PropertyInfo propertyInfo, PropertyInfo otherPropertyInfo)
    {
      if (propertyInfo == otherPropertyInfo)
        return true;
      if (!(propertyInfo.Name == otherPropertyInfo.Name))
        return false;
      return propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType || propertyInfo.DeclaringType.IsSubclassOf(otherPropertyInfo.DeclaringType) || (otherPropertyInfo.DeclaringType.IsSubclassOf(propertyInfo.DeclaringType) || ((IEnumerable<Type>) propertyInfo.DeclaringType.GetInterfaces()).Contains<Type>(otherPropertyInfo.DeclaringType)) || ((IEnumerable<Type>) otherPropertyInfo.DeclaringType.GetInterfaces()).Contains<Type>(propertyInfo.DeclaringType);
    }

    public static bool ContainsSame(
      this IEnumerable<PropertyInfo> enumerable,
      PropertyInfo propertyInfo)
    {
      return enumerable.Any<PropertyInfo>(new Func<PropertyInfo, bool>(((PropertyInfoExtensions) propertyInfo).IsSameAs));
    }

    public static bool IsValidStructuralProperty(this PropertyInfo propertyInfo) => propertyInfo.IsValidInterfaceStructuralProperty() && !propertyInfo.Getter().IsAbstract;

    public static bool IsValidInterfaceStructuralProperty(this PropertyInfo propertyInfo) => propertyInfo.CanRead && (propertyInfo.CanWriteExtended() || propertyInfo.PropertyType.IsCollection()) && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.PropertyType.IsValidStructuralPropertyType();

    public static bool IsValidEdmScalarProperty(this PropertyInfo propertyInfo) => propertyInfo.IsValidInterfaceStructuralProperty() && propertyInfo.PropertyType.IsValidEdmScalarType();

    public static bool IsValidEdmNavigationProperty(this PropertyInfo propertyInfo)
    {
      if (!propertyInfo.IsValidInterfaceStructuralProperty())
        return false;
      Type elementType;
      return propertyInfo.PropertyType.IsCollection(out elementType) && elementType.IsValidStructuralType() || propertyInfo.PropertyType.IsValidStructuralType();
    }

    public static EdmProperty AsEdmPrimitiveProperty(this PropertyInfo propertyInfo)
    {
      Type underlyingType = propertyInfo.PropertyType;
      bool flag = underlyingType.TryUnwrapNullableType(out underlyingType) || !underlyingType.IsValueType();
      PrimitiveType primitiveType;
      if (!underlyingType.IsPrimitiveType(out primitiveType))
        return (EdmProperty) null;
      EdmProperty primitive = EdmProperty.CreatePrimitive(propertyInfo.Name, primitiveType);
      primitive.Nullable = flag;
      return primitive;
    }

    public static bool CanWriteExtended(this PropertyInfo propertyInfo)
    {
      if (propertyInfo.CanWrite)
        return true;
      PropertyInfo declaredProperty = PropertyInfoExtensions.GetDeclaredProperty(propertyInfo);
      return declaredProperty != (PropertyInfo) null && declaredProperty.CanWrite;
    }

    public static PropertyInfo GetPropertyInfoForSet(this PropertyInfo propertyInfo)
    {
      if (propertyInfo.CanWrite)
        return propertyInfo;
      PropertyInfo declaredProperty = PropertyInfoExtensions.GetDeclaredProperty(propertyInfo);
      return (object) declaredProperty != null ? declaredProperty : propertyInfo;
    }

    private static PropertyInfo GetDeclaredProperty(PropertyInfo propertyInfo) => !(propertyInfo.DeclaringType == propertyInfo.ReflectedType) ? propertyInfo.DeclaringType.GetInstanceProperties().SingleOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.Name == propertyInfo.Name && p.DeclaringType == propertyInfo.DeclaringType && !((IEnumerable<ParameterInfo>) p.GetIndexParameters()).Any<ParameterInfo>() && p.PropertyType == propertyInfo.PropertyType)) : propertyInfo;

    public static IEnumerable<PropertyInfo> GetPropertiesInHierarchy(
      this PropertyInfo property)
    {
      List<PropertyInfo> source = new List<PropertyInfo>()
      {
        property
      };
      PropertyInfoExtensions.CollectProperties(property, (IList<PropertyInfo>) source);
      return source.Distinct<PropertyInfo>();
    }

    private static void CollectProperties(PropertyInfo property, IList<PropertyInfo> collection)
    {
      PropertyInfoExtensions.FindNextProperty(property, collection, true);
      PropertyInfoExtensions.FindNextProperty(property, collection, false);
    }

    private static void FindNextProperty(
      PropertyInfo property,
      IList<PropertyInfo> collection,
      bool getter)
    {
      MethodInfo methodInfo = getter ? property.Getter() : property.Setter();
      if (!(methodInfo != (MethodInfo) null))
        return;
      Type type = methodInfo.DeclaringType.BaseType();
      if (!(type != (Type) null) || !(type != typeof (object)))
        return;
      MethodInfo baseMethod = methodInfo.GetBaseDefinition();
      PropertyInfo property1 = type.GetInstanceProperties().Select(p => new
      {
        p = p,
        candidateMethod = getter ? p.Getter() : p.Setter()
      }).Where(_param1 => _param1.candidateMethod != (MethodInfo) null && _param1.candidateMethod.GetBaseDefinition() == baseMethod).Select(_param1 => _param1.p).FirstOrDefault<PropertyInfo>();
      if (!(property1 != (PropertyInfo) null))
        return;
      collection.Add(property1);
      PropertyInfoExtensions.CollectProperties(property1, collection);
    }

    public static MethodInfo Getter(this PropertyInfo property) => property.GetMethod;

    public static MethodInfo Setter(this PropertyInfo property) => property.SetMethod;

    public static bool IsStatic(this PropertyInfo property)
    {
      MethodInfo methodInfo = property.Getter();
      if ((object) methodInfo == null)
        methodInfo = property.Setter();
      return methodInfo.IsStatic;
    }

    public static bool IsPublic(this PropertyInfo property)
    {
      MethodInfo methodInfo1 = property.Getter();
      MethodAttributes methodAttributes1 = methodInfo1 == (MethodInfo) null ? MethodAttributes.Private : methodInfo1.Attributes & MethodAttributes.MemberAccessMask;
      MethodInfo methodInfo2 = property.Setter();
      MethodAttributes methodAttributes2 = methodInfo2 == (MethodInfo) null ? MethodAttributes.Private : methodInfo2.Attributes & MethodAttributes.MemberAccessMask;
      return (methodAttributes1 > methodAttributes2 ? (int) methodAttributes1 : (int) methodAttributes2) == 6;
    }
  }
}
