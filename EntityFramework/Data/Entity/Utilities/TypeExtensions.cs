// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.TypeExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Hierarchy;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Utilities
{
  internal static class TypeExtensions
  {
    private static readonly Dictionary<Type, PrimitiveType> _primitiveTypesMap = new Dictionary<Type, PrimitiveType>();

    static TypeExtensions()
    {
      foreach (PrimitiveType edmPrimitiveType in PrimitiveType.GetEdmPrimitiveTypes())
      {
        if (!TypeExtensions._primitiveTypesMap.ContainsKey(edmPrimitiveType.ClrEquivalentType))
          TypeExtensions._primitiveTypesMap.Add(edmPrimitiveType.ClrEquivalentType, edmPrimitiveType);
      }
    }

    public static bool IsCollection(this Type type) => type.IsCollection(out type);

    public static bool IsCollection(this Type type, out Type elementType)
    {
      elementType = type.TryGetElementType(typeof (ICollection<>));
      if (!(elementType == (Type) null) && !type.IsArray)
        return true;
      elementType = type;
      return false;
    }

    public static IEnumerable<PropertyInfo> GetNonIndexerProperties(
      this Type type)
    {
      return type.GetRuntimeProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.IsPublic() && !((IEnumerable<ParameterInfo>) p.GetIndexParameters()).Any<ParameterInfo>()));
    }

    public static Type TryGetElementType(this Type type, Type interfaceOrBaseType)
    {
      if (type.IsGenericTypeDefinition())
        return (Type) null;
      List<Type> list = type.GetGenericTypeImplementations(interfaceOrBaseType).ToList<Type>();
      return list.Count != 1 ? (Type) null : ((IEnumerable<Type>) list[0].GetGenericArguments()).FirstOrDefault<Type>();
    }

    public static IEnumerable<Type> GetGenericTypeImplementations(
      this Type type,
      Type interfaceOrBaseType)
    {
      if (type.IsGenericTypeDefinition())
        return Enumerable.Empty<Type>();
      return (interfaceOrBaseType.IsInterface() ? (IEnumerable<Type>) type.GetInterfaces() : type.GetBaseTypes()).Union<Type>((IEnumerable<Type>) new Type[1]
      {
        type
      }).Where<Type>((Func<Type, bool>) (t => t.IsGenericType() && t.GetGenericTypeDefinition() == interfaceOrBaseType));
    }

    public static IEnumerable<Type> GetBaseTypes(this Type type)
    {
      for (type = type.BaseType(); type != (Type) null; type = type.BaseType())
        yield return type;
    }

    public static Type GetTargetType(this Type type)
    {
      Type elementType;
      if (!type.IsCollection(out elementType))
        elementType = type;
      return elementType;
    }

    public static bool TryUnwrapNullableType(this Type type, out Type underlyingType)
    {
      ref Type local = ref underlyingType;
      Type type1 = Nullable.GetUnderlyingType(type);
      if ((object) type1 == null)
        type1 = type;
      local = type1;
      return underlyingType != type;
    }

    public static bool IsNullable(this Type type) => !type.IsValueType() || Nullable.GetUnderlyingType(type) != (Type) null;

    public static bool IsValidStructuralType(this Type type) => !type.IsGenericType() && !type.IsValueType() && (!type.IsPrimitive() && !type.IsInterface()) && (!type.IsArray && !(type == typeof (string)) && (!(type == typeof (DbGeography)) && !(type == typeof (DbGeometry)))) && !(type == typeof (HierarchyId)) && type.IsValidStructuralPropertyType();

    public static bool IsValidStructuralPropertyType(this Type type) => !type.IsGenericTypeDefinition() && !type.IsPointer && (!(type == typeof (object)) && !typeof (ComplexObject).IsAssignableFrom(type)) && (!typeof (EntityObject).IsAssignableFrom(type) && !typeof (StructuralObject).IsAssignableFrom(type) && !typeof (EntityKey).IsAssignableFrom(type)) && !typeof (EntityReference).IsAssignableFrom(type);

    public static bool IsPrimitiveType(this Type type, out PrimitiveType primitiveType) => TypeExtensions._primitiveTypesMap.TryGetValue(type, out primitiveType);

    public static T CreateInstance<T>(
      this Type type,
      Func<string, string, string> typeMessageFactory,
      Func<string, Exception> exceptionFactory = null)
    {
      exceptionFactory = exceptionFactory ?? (Func<string, Exception>) (s => (Exception) new InvalidOperationException(s));
      return typeof (T).IsAssignableFrom(type) ? type.CreateInstance<T>(exceptionFactory) : throw exceptionFactory(typeMessageFactory(type.ToString(), typeof (T).ToString()));
    }

    public static T CreateInstance<T>(this Type type, Func<string, Exception> exceptionFactory = null)
    {
      exceptionFactory = exceptionFactory ?? (Func<string, Exception>) (s => (Exception) new InvalidOperationException(s));
      if (type.GetDeclaredConstructor() == (ConstructorInfo) null)
        throw exceptionFactory(System.Data.Entity.Resources.Strings.CreateInstance_NoParameterlessConstructor((object) type));
      if (type.IsAbstract())
        throw exceptionFactory(System.Data.Entity.Resources.Strings.CreateInstance_AbstractType((object) type));
      return !type.IsGenericType() ? (T) Activator.CreateInstance(type, true) : throw exceptionFactory(System.Data.Entity.Resources.Strings.CreateInstance_GenericType((object) type));
    }

    public static bool IsValidEdmScalarType(this Type type)
    {
      type.TryUnwrapNullableType(out type);
      return type.IsPrimitiveType(out PrimitiveType _) || type.IsEnum();
    }

    public static string NestingNamespace(this Type type)
    {
      if (!type.IsNested)
        return type.Namespace;
      string fullName = type.FullName;
      return fullName.Substring(0, fullName.Length - type.Name.Length - 1).Replace('+', '.');
    }

    public static string FullNameWithNesting(this Type type) => !type.IsNested ? type.FullName : type.FullName.Replace('+', '.');

    public static bool OverridesEqualsOrGetHashCode(this Type type)
    {
      for (; type != typeof (object); type = type.BaseType())
      {
        if (type.GetDeclaredMethods().Any<MethodInfo>((Func<MethodInfo, bool>) (m => (m.Name == "Equals" || m.Name == "GetHashCode") && m.DeclaringType != typeof (object) && m.GetBaseDefinition().DeclaringType == typeof (object))))
          return true;
      }
      return false;
    }

    public static bool IsPublic(this Type type)
    {
      TypeInfo typeInfo = type.GetTypeInfo();
      if (typeInfo.IsPublic)
        return true;
      return typeInfo.IsNestedPublic && type.DeclaringType.IsPublic();
    }

    public static bool IsNotPublic(this Type type) => !type.IsPublic();

    public static MethodInfo GetOnlyDeclaredMethod(this Type type, string name) => type.GetDeclaredMethods(name).SingleOrDefault<MethodInfo>();

    public static MethodInfo GetDeclaredMethod(
      this Type type,
      string name,
      params Type[] parameterTypes)
    {
      return type.GetDeclaredMethods(name).SingleOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => ((IEnumerable<ParameterInfo>) m.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)).SequenceEqual<Type>((IEnumerable<Type>) parameterTypes)));
    }

    public static MethodInfo GetPublicInstanceMethod(
      this Type type,
      string name,
      params Type[] parameterTypes)
    {
      return type.GetRuntimeMethod(name, (Func<MethodInfo, bool>) (m => m.IsPublic && !m.IsStatic), parameterTypes);
    }

    public static MethodInfo GetRuntimeMethod(
      this Type type,
      string name,
      Func<MethodInfo, bool> predicate,
      params Type[][] parameterTypes)
    {
      return ((IEnumerable<Type[]>) parameterTypes).Select<Type[], MethodInfo>((Func<Type[], MethodInfo>) (t => type.GetRuntimeMethod(name, predicate, t))).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => m != (MethodInfo) null));
    }

    private static MethodInfo GetRuntimeMethod(
      this Type type,
      string name,
      Func<MethodInfo, bool> predicate,
      Type[] parameterTypes)
    {
      MethodInfo[] methods = type.GetRuntimeMethods().Where<MethodInfo>((Func<MethodInfo, bool>) (m => name == m.Name && predicate(m) && ((IEnumerable<ParameterInfo>) m.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)).SequenceEqual<Type>((IEnumerable<Type>) parameterTypes))).ToArray<MethodInfo>();
      return methods.Length == 1 ? methods[0] : ((IEnumerable<MethodInfo>) methods).SingleOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => !((IEnumerable<MethodInfo>) methods).Any<MethodInfo>((Func<MethodInfo, bool>) (m2 => m2.DeclaringType.IsSubclassOf(m.DeclaringType)))));
    }

    public static IEnumerable<MethodInfo> GetDeclaredMethods(this Type type) => type.GetTypeInfo().DeclaredMethods;

    public static IEnumerable<MethodInfo> GetDeclaredMethods(
      this Type type,
      string name)
    {
      return type.GetTypeInfo().GetDeclaredMethods(name);
    }

    public static PropertyInfo GetDeclaredProperty(this Type type, string name) => type.GetTypeInfo().GetDeclaredProperty(name);

    public static IEnumerable<PropertyInfo> GetDeclaredProperties(
      this Type type)
    {
      return type.GetTypeInfo().DeclaredProperties;
    }

    public static IEnumerable<PropertyInfo> GetInstanceProperties(
      this Type type)
    {
      return type.GetRuntimeProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => !p.IsStatic()));
    }

    public static IEnumerable<PropertyInfo> GetNonHiddenProperties(
      this Type type)
    {
      return type.GetRuntimeProperties().GroupBy<PropertyInfo, string>((Func<PropertyInfo, string>) (property => property.Name)).Select<IGrouping<string, PropertyInfo>, PropertyInfo>((Func<IGrouping<string, PropertyInfo>, PropertyInfo>) (propertyGroup => TypeExtensions.MostDerived((IEnumerable<PropertyInfo>) propertyGroup)));
    }

    private static PropertyInfo MostDerived(IEnumerable<PropertyInfo> properties)
    {
      PropertyInfo propertyInfo = (PropertyInfo) null;
      foreach (PropertyInfo property in properties)
      {
        if (propertyInfo == (PropertyInfo) null || propertyInfo.DeclaringType != (Type) null && propertyInfo.DeclaringType.IsAssignableFrom(property.DeclaringType))
          propertyInfo = property;
      }
      return propertyInfo;
    }

    public static PropertyInfo GetAnyProperty(this Type type, string name)
    {
      List<PropertyInfo> list = type.GetRuntimeProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.Name == name)).ToList<PropertyInfo>();
      return list.Count<PropertyInfo>() <= 1 ? list.SingleOrDefault<PropertyInfo>() : throw new AmbiguousMatchException();
    }

    public static PropertyInfo GetInstanceProperty(this Type type, string name)
    {
      List<PropertyInfo> list = type.GetRuntimeProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.Name == name && !p.IsStatic())).ToList<PropertyInfo>();
      return list.Count<PropertyInfo>() <= 1 ? list.SingleOrDefault<PropertyInfo>() : throw new AmbiguousMatchException();
    }

    public static PropertyInfo GetStaticProperty(this Type type, string name)
    {
      List<PropertyInfo> list = type.GetRuntimeProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.Name == name && p.IsStatic())).ToList<PropertyInfo>();
      return list.Count<PropertyInfo>() <= 1 ? list.SingleOrDefault<PropertyInfo>() : throw new AmbiguousMatchException();
    }

    public static PropertyInfo GetTopProperty(this Type type, string name)
    {
      do
      {
        TypeInfo typeInfo = type.GetTypeInfo();
        PropertyInfo declaredProperty = typeInfo.GetDeclaredProperty(name);
        if (declaredProperty != (PropertyInfo) null)
        {
          MethodInfo methodInfo = declaredProperty.GetMethod;
          if ((object) methodInfo == null)
            methodInfo = declaredProperty.SetMethod;
          if (!methodInfo.IsStatic)
            return declaredProperty;
        }
        type = typeInfo.BaseType;
      }
      while (type != (Type) null);
      return (PropertyInfo) null;
    }

    public static System.Reflection.Assembly Assembly(this Type type) => type.GetTypeInfo().Assembly;

    public static Type BaseType(this Type type) => type.GetTypeInfo().BaseType;

    public static bool IsGenericType(this Type type) => type.GetTypeInfo().IsGenericType;

    public static bool IsGenericTypeDefinition(this Type type) => type.GetTypeInfo().IsGenericTypeDefinition;

    public static TypeAttributes Attributes(this Type type) => type.GetTypeInfo().Attributes;

    public static bool IsClass(this Type type) => type.GetTypeInfo().IsClass;

    public static bool IsInterface(this Type type) => type.GetTypeInfo().IsInterface;

    public static bool IsValueType(this Type type) => type.GetTypeInfo().IsValueType;

    public static bool IsAbstract(this Type type) => type.GetTypeInfo().IsAbstract;

    public static bool IsSealed(this Type type) => type.GetTypeInfo().IsSealed;

    public static bool IsEnum(this Type type) => type.GetTypeInfo().IsEnum;

    public static bool IsSerializable(this Type type) => type.GetTypeInfo().IsSerializable;

    public static bool IsGenericParameter(this Type type) => type.GetTypeInfo().IsGenericParameter;

    public static bool ContainsGenericParameters(this Type type) => type.GetTypeInfo().ContainsGenericParameters;

    public static bool IsPrimitive(this Type type) => type.GetTypeInfo().IsPrimitive;

    public static IEnumerable<ConstructorInfo> GetDeclaredConstructors(
      this Type type)
    {
      return type.GetTypeInfo().DeclaredConstructors;
    }

    public static ConstructorInfo GetDeclaredConstructor(
      this Type type,
      params Type[] parameterTypes)
    {
      return type.GetDeclaredConstructors().SingleOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => !c.IsStatic && ((IEnumerable<ParameterInfo>) c.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)).SequenceEqual<Type>((IEnumerable<Type>) parameterTypes)));
    }

    public static ConstructorInfo GetPublicConstructor(
      this Type type,
      params Type[] parameterTypes)
    {
      ConstructorInfo declaredConstructor = type.GetDeclaredConstructor(parameterTypes);
      return !(declaredConstructor != (ConstructorInfo) null) || !declaredConstructor.IsPublic ? (ConstructorInfo) null : declaredConstructor;
    }

    public static ConstructorInfo GetDeclaredConstructor(
      this Type type,
      Func<ConstructorInfo, bool> predicate,
      params Type[][] parameterTypes)
    {
      return ((IEnumerable<Type[]>) parameterTypes).Select<Type[], ConstructorInfo>((Func<Type[], ConstructorInfo>) (p => type.GetDeclaredConstructor(p))).FirstOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => c != (ConstructorInfo) null && predicate(c)));
    }

    public static bool IsSubclassOf(this Type type, Type otherType) => type.GetTypeInfo().IsSubclassOf(otherType);
  }
}
