// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.Extensions
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;


#nullable enable
namespace Newtonsoft.Json.Linq
{
  /// <summary>Contains the LINQ to JSON extension methods.</summary>
  public static class Extensions
  {
    /// <summary>
    /// Returns a collection of tokens that contains the ancestors of every token in the source collection.
    /// </summary>
    /// <typeparam name="T">The type of the objects in source, constrained to <see cref="T:Newtonsoft.Json.Linq.JToken" />.</typeparam>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the ancestors of every token in the source collection.</returns>
    public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.Ancestors())).AsJEnumerable();
    }

    /// <summary>
    /// Returns a collection of tokens that contains every token in the source collection, and the ancestors of every token in the source collection.
    /// </summary>
    /// <typeparam name="T">The type of the objects in source, constrained to <see cref="T:Newtonsoft.Json.Linq.JToken" />.</typeparam>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains every token in the source collection, the ancestors of every token in the source collection.</returns>
    public static IJEnumerable<JToken> AncestorsAndSelf<T>(
      this IEnumerable<T> source)
      where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.AncestorsAndSelf())).AsJEnumerable();
    }

    /// <summary>
    /// Returns a collection of tokens that contains the descendants of every token in the source collection.
    /// </summary>
    /// <typeparam name="T">The type of the objects in source, constrained to <see cref="T:Newtonsoft.Json.Linq.JContainer" />.</typeparam>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the descendants of every token in the source collection.</returns>
    public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source) where T : JContainer
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.Descendants())).AsJEnumerable();
    }

    /// <summary>
    /// Returns a collection of tokens that contains every token in the source collection, and the descendants of every token in the source collection.
    /// </summary>
    /// <typeparam name="T">The type of the objects in source, constrained to <see cref="T:Newtonsoft.Json.Linq.JContainer" />.</typeparam>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains every token in the source collection, and the descendants of every token in the source collection.</returns>
    public static IJEnumerable<JToken> DescendantsAndSelf<T>(
      this IEnumerable<T> source)
      where T : JContainer
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (j => j.DescendantsAndSelf())).AsJEnumerable();
    }

    /// <summary>
    /// Returns a collection of child properties of every object in the source collection.
    /// </summary>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JObject" /> that contains the source collection.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JProperty" /> that contains the properties of every object in the source collection.</returns>
    public static IJEnumerable<JProperty> Properties(
      this IEnumerable<JObject> source)
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<JObject, JProperty>((Func<JObject, IEnumerable<JProperty>>) (d => d.Properties())).AsJEnumerable<JProperty>();
    }

    /// <summary>
    /// Returns a collection of child values of every object in the source collection with the given key.
    /// </summary>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <param name="key">The token key.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the values of every token in the source collection with the given key.</returns>
    public static IJEnumerable<JToken> Values(
      this IEnumerable<JToken> source,
      object? key)
    {
      return source.Values<JToken, JToken>(key).AsJEnumerable();
    }

    /// <summary>
    /// Returns a collection of child values of every object in the source collection.
    /// </summary>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the values of every token in the source collection.</returns>
    public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source) => source.Values((object) null);

    /// <summary>
    /// Returns a collection of converted child values of every object in the source collection with the given key.
    /// </summary>
    /// <typeparam name="U">The type to convert the values to.</typeparam>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <param name="key">The token key.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the converted values of every token in the source collection with the given key.</returns>
    public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source, object key) => source.Values<JToken, U>(key);

    /// <summary>
    /// Returns a collection of converted child values of every object in the source collection.
    /// </summary>
    /// <typeparam name="U">The type to convert the values to.</typeparam>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the converted values of every token in the source collection.</returns>
    public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source) => source.Values<JToken, U>((object) null);

    /// <summary>Converts the value.</summary>
    /// <typeparam name="U">The type to convert the value to.</typeparam>
    /// <param name="value">A <see cref="T:Newtonsoft.Json.Linq.JToken" /> cast as a <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" />.</param>
    /// <returns>A converted value.</returns>
    public static U Value<U>(this IEnumerable<JToken> value) => value.Value<JToken, U>();

    /// <summary>Converts the value.</summary>
    /// <typeparam name="T">The source collection type.</typeparam>
    /// <typeparam name="U">The type to convert the value to.</typeparam>
    /// <param name="value">A <see cref="T:Newtonsoft.Json.Linq.JToken" /> cast as a <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" />.</param>
    /// <returns>A converted value.</returns>
    public static U Value<T, U>(this IEnumerable<T> value) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) value, nameof (value));
      return value is JToken token ? token.Convert<JToken, U>() : throw new ArgumentException("Source value must be a JToken.");
    }

    internal static IEnumerable<U> Values<T, U>(this IEnumerable<T> source, object? key) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      if (key == null)
      {
        foreach (T obj in source)
        {
          if (obj is JValue token3)
          {
            yield return token3.Convert<JValue, U>();
          }
          else
          {
            foreach (JToken child in obj.Children())
              yield return child.Convert<JToken, U>();
          }
        }
      }
      else
      {
        foreach (T obj in source)
        {
          JToken token2 = obj[key];
          if (token2 != null)
            yield return token2.Convert<JToken, U>();
        }
      }
    }

    /// <summary>
    /// Returns a collection of child tokens of every array in the source collection.
    /// </summary>
    /// <typeparam name="T">The source collection type.</typeparam>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the values of every token in the source collection.</returns>
    public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source) where T : JToken => source.Children<T, JToken>().AsJEnumerable();

    /// <summary>
    /// Returns a collection of converted child tokens of every array in the source collection.
    /// </summary>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <typeparam name="U">The type to convert the values to.</typeparam>
    /// <typeparam name="T">The source collection type.</typeparam>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> that contains the converted values of every token in the source collection.</returns>
    public static IEnumerable<U> Children<T, U>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      return source.SelectMany<T, JToken>((Func<T, IEnumerable<JToken>>) (c => (IEnumerable<JToken>) c.Children())).Convert<JToken, U>();
    }

    internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source) where T : JToken
    {
      ValidationUtils.ArgumentNotNull((object) source, nameof (source));
      foreach (T token in source)
        yield return token.Convert<JToken, U>();
    }

    [return: MaybeNull]
    internal static U Convert<T, U>(this T token) where T : JToken?
    {
      if ((object) token == null)
        return default (U);
      switch (token)
      {
        case U u2 when typeof (U) != typeof (IComparable) && typeof (U) != typeof (IFormattable):
          return u2;
        case JValue jvalue:
          if (jvalue.Value is U u3)
            return u3;
          Type type = typeof (U);
          if (ReflectionUtils.IsNullableType(type))
          {
            if (jvalue.Value == null)
              return default (U);
            type = Nullable.GetUnderlyingType(type);
          }
          return (U) System.Convert.ChangeType(jvalue.Value, type, (IFormatProvider) CultureInfo.InvariantCulture);
        default:
          throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) token.GetType(), (object) typeof (T)));
      }
    }

    /// <summary>
    /// Returns the input typed as <see cref="T:Newtonsoft.Json.Linq.IJEnumerable`1" />.
    /// </summary>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <returns>The input typed as <see cref="T:Newtonsoft.Json.Linq.IJEnumerable`1" />.</returns>
    public static IJEnumerable<JToken> AsJEnumerable(
      this IEnumerable<JToken> source)
    {
      return source.AsJEnumerable<JToken>();
    }

    /// <summary>
    /// Returns the input typed as <see cref="T:Newtonsoft.Json.Linq.IJEnumerable`1" />.
    /// </summary>
    /// <typeparam name="T">The source collection type.</typeparam>
    /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> that contains the source collection.</param>
    /// <returns>The input typed as <see cref="T:Newtonsoft.Json.Linq.IJEnumerable`1" />.</returns>
    public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source) where T : JToken
    {
      if (source == null)
        return (IJEnumerable<T>) null;
      return source is IJEnumerable<T> jenumerable ? jenumerable : (IJEnumerable<T>) new JEnumerable<T>(source);
    }
  }
}
