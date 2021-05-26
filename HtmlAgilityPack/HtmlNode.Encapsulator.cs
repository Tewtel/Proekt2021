// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.Tools
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace HtmlAgilityPack
{
  /// <summary>
  /// Includes tools that GetEncapsulatedData method uses them.
  /// </summary>
  internal static class Tools
  {
    /// <summary>
    /// Determine if a type define an attribute or not , supporting both .NetStandard and .NetFramework2.0
    /// </summary>
    /// <param name="type">Type you want to test it.</param>
    /// <param name="attributeType">Attribute that type must have or not.</param>
    /// <returns>If true , The type parameter define attributeType parameter.</returns>
    internal static bool IsDefinedAttribute(this Type type, Type attributeType)
    {
      if (type == (Type) null)
        throw new ArgumentNullException("Parameter type is null when checking type defined attributeType or not.");
      return !(attributeType == (Type) null) ? type.IsDefined(attributeType, false) : throw new ArgumentNullException("Parameter attributeType is null when checking type defined attributeType or not.");
    }

    /// <summary>
    /// Retrive properties of type that defined <see cref="T:HtmlAgilityPack.XPathAttribute" />.
    /// </summary>
    /// <param name="type">Type that you want to find it's XPath-Defined properties.</param>
    /// <returns>IEnumerable of property infos of a type , that defined specific attribute.</returns>
    internal static IEnumerable<PropertyInfo> GetPropertiesDefinedXPath(
      this Type type)
    {
      return !(type == (Type) null) ? ((IEnumerable<PropertyInfo>) type.GetProperties()).HAPWhere<PropertyInfo>((Tools.HAPFunc<PropertyInfo, bool>) (x => x.IsDefined(typeof (XPathAttribute), false))) : throw new ArgumentNullException("Parameter type is null while retrieving properties defined XPathAttribute of Type type.");
    }

    /// <summary>
    /// Determine if a <see cref="T:System.Reflection.PropertyInfo" /> has implemented <see cref="T:System.Collections.IEnumerable" /> BUT <see cref="T:System.String" /> is considered as NONE-IEnumerable !
    /// </summary>
    /// <param name="propertyInfo">The property info you want to test.</param>
    /// <returns>True if property info is IEnumerable.</returns>
    internal static bool IsIEnumerable(this PropertyInfo propertyInfo)
    {
      if (propertyInfo == (PropertyInfo) null)
        throw new ArgumentNullException("Parameter propertyInfo is null while checking propertyInfo for being IEnumerable or not.");
      return !(propertyInfo.PropertyType == typeof (string)) && typeof (IEnumerable).IsAssignableFrom(propertyInfo.PropertyType);
    }

    /// <summary>
    /// Returns T type(first generic type) of <see cref="T:System.Collections.Generic.IEnumerable`1" /> or <see cref="T:System.Collections.Generic.List`1" />.
    /// </summary>
    /// <param name="propertyInfo">IEnumerable-Implemented property</param>
    /// <returns>List of generic types.</returns>
    internal static IEnumerable<Type> GetGenericTypes(this PropertyInfo propertyInfo) => !(propertyInfo == (PropertyInfo) null) ? (IEnumerable<Type>) propertyInfo.PropertyType.GetGenericArguments() : throw new ArgumentNullException("Parameter propertyInfo is null while Getting generic types of Property.");

    /// <summary>
    /// Find and Return a mehtod that defined in a class by it's name.
    /// </summary>
    /// <param name="type">Type of class include requested method.</param>
    /// <param name="methodName">Name of requested method as string.</param>
    /// <returns>Method info of requested method.</returns>
    internal static MethodInfo GetMethodByItsName(this Type type, string methodName)
    {
      if (type == (Type) null)
        throw new ArgumentNullException("Parameter type is null while Getting method from it.");
      switch (methodName)
      {
        case "":
        case null:
          throw new ArgumentNullException("Parameter methodName is null while Getting method from Type type.");
        default:
          return type.GetMethod(methodName);
      }
    }

    /// <summary>
    /// Create <see cref="T:System.Collections.IList" /> of given type.
    /// </summary>
    /// <param name="type">Type that you want to make a List of it.</param>
    /// <returns>Returns IList of given type.</returns>
    internal static IList CreateIListOfType(this Type type) => !(type == (Type) null) ? Activator.CreateInstance(typeof (List<>).MakeGenericType(type)) as IList : throw new ArgumentNullException("Parameter type is null while creating List<type>.");

    /// <summary>
    /// Returns the part of value of <see cref="T:HtmlAgilityPack.HtmlNode" /> you want as .
    /// </summary>
    /// <param name="htmlNode">A htmlNode instance.</param>
    /// <param name="xPathAttribute">Attribute that includes ReturnType</param>
    /// <returns>String that choosen from HtmlNode as result.</returns>
    internal static T GetNodeValueBasedOnXPathReturnType<T>(
      HtmlNode htmlNode,
      XPathAttribute xPathAttribute)
    {
      if (htmlNode == null)
        throw new ArgumentNullException("parameter html node is null");
      if (xPathAttribute == null)
        throw new ArgumentNullException("parameter xpathAttribute is null");
      Type conversionType = typeof (T);
      object obj;
      switch (xPathAttribute.NodeReturnType)
      {
        case ReturnType.InnerText:
          obj = Convert.ChangeType((object) htmlNode.InnerText, conversionType);
          break;
        case ReturnType.InnerHtml:
          obj = Convert.ChangeType((object) htmlNode.InnerHtml, conversionType);
          break;
        case ReturnType.OuterHtml:
          obj = Convert.ChangeType((object) htmlNode.OuterHtml, conversionType);
          break;
        default:
          throw new Exception();
      }
      return (T) obj;
    }

    /// <summary>
    /// Returns parts of values of <see cref="T:HtmlAgilityPack.HtmlNode" /> you want as <see cref="T:System.Collections.Generic.IList`1" />.
    /// </summary>
    /// <param name="htmlNodeCollection"><see cref="T:HtmlAgilityPack.HtmlNodeCollection" /> that you want to retrive each <see cref="T:HtmlAgilityPack.HtmlNode" /> value.</param>
    /// <param name="xPathAttribute">A <see cref="T:HtmlAgilityPack.XPathAttribute" /> instnce incules <see cref="T:HtmlAgilityPack.ReturnType" />.</param>
    /// <param name="listGenericType">Type of IList generic you want.</param>
    /// <returns></returns>
    internal static IList GetNodesValuesBasedOnXPathReturnType(
      HtmlNodeCollection htmlNodeCollection,
      XPathAttribute xPathAttribute,
      Type listGenericType)
    {
      if (htmlNodeCollection == null || htmlNodeCollection.Count == 0)
        throw new ArgumentNullException("parameter htmlNodeCollection is null or empty.");
      if (xPathAttribute == null)
        throw new ArgumentNullException("parameter xpathAttribute is null");
      IList ilistOfType = listGenericType.CreateIListOfType();
      switch (xPathAttribute.NodeReturnType)
      {
        case ReturnType.InnerText:
          using (IEnumerator<HtmlNode> enumerator = ((IEnumerable<HtmlNode>) htmlNodeCollection).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              HtmlNode current = enumerator.Current;
              ilistOfType.Add(Convert.ChangeType((object) current.InnerText, listGenericType));
            }
            break;
          }
        case ReturnType.InnerHtml:
          using (IEnumerator<HtmlNode> enumerator = ((IEnumerable<HtmlNode>) htmlNodeCollection).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              HtmlNode current = enumerator.Current;
              ilistOfType.Add(Convert.ChangeType((object) current.InnerHtml, listGenericType));
            }
            break;
          }
        case ReturnType.OuterHtml:
          using (IEnumerator<HtmlNode> enumerator = ((IEnumerable<HtmlNode>) htmlNodeCollection).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              HtmlNode current = enumerator.Current;
              ilistOfType.Add(Convert.ChangeType((object) current.OuterHtml, listGenericType));
            }
            break;
          }
      }
      return ilistOfType;
    }

    /// <summary>This method works like Where method in LINQ.</summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    internal static IEnumerable<TSource> HAPWhere<TSource>(
      this IEnumerable<TSource> source,
      Tools.HAPFunc<TSource, bool> predicate)
    {
      foreach (TSource source1 in source)
      {
        if (predicate(source1))
          yield return source1;
      }
    }

    /// <summary>Check if the type can instantiated.</summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static bool IsInstantiable(this Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException("type is null");
      return !(type.GetConstructor(Type.EmptyTypes) == (ConstructorInfo) null);
    }

    /// <summary>Returns count of elements stored in IEnumerable of T</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    internal static int CountOfIEnumerable<T>(this IEnumerable<T> source)
    {
      if (source == null)
        throw new ArgumentNullException("Parameter source is null while counting the IEnumerable");
      int num = 0;
      foreach (T obj in source)
        ++num;
      return num;
    }

    /// <summary>Simulate Func method to use in Lambada Expression.</summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="arg"></param>
    /// <returns></returns>
    internal delegate TResult HAPFunc<T, TResult>(T arg);
  }
}
