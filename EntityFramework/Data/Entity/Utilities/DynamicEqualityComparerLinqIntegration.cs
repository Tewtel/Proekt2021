// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.DynamicEqualityComparerLinqIntegration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Utilities
{
  internal static class DynamicEqualityComparerLinqIntegration
  {
    public static IEnumerable<T> Distinct<T>(
      this IEnumerable<T> source,
      Func<T, T, bool> func)
      where T : class
    {
      return source.Distinct<T>((IEqualityComparer<T>) new DynamicEqualityComparer<T>(func));
    }

    public static IEnumerable<IGrouping<TSource, TSource>> GroupBy<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, TSource, bool> func)
      where TSource : class
    {
      return source.GroupBy<TSource, TSource>((Func<TSource, TSource>) (t => t), (IEqualityComparer<TSource>) new DynamicEqualityComparer<TSource>(func));
    }

    public static IEnumerable<T> Intersect<T>(
      this IEnumerable<T> first,
      IEnumerable<T> second,
      Func<T, T, bool> func)
      where T : class
    {
      return first.Intersect<T>(second, (IEqualityComparer<T>) new DynamicEqualityComparer<T>(func));
    }

    public static IEnumerable<T> Except<T>(
      this IEnumerable<T> first,
      IEnumerable<T> second,
      Func<T, T, bool> func)
      where T : class
    {
      return first.Except<T>(second, (IEqualityComparer<T>) new DynamicEqualityComparer<T>(func));
    }

    public static bool Contains<T>(this IEnumerable<T> source, T value, Func<T, T, bool> func) where T : class => source.Contains<T>(value, (IEqualityComparer<T>) new DynamicEqualityComparer<T>(func));

    public static bool SequenceEqual<TSource>(
      this IEnumerable<TSource> source,
      IEnumerable<TSource> other,
      Func<TSource, TSource, bool> func)
      where TSource : class
    {
      return source.SequenceEqual<TSource>(other, (IEqualityComparer<TSource>) new DynamicEqualityComparer<TSource>(func));
    }
  }
}
