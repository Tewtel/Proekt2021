﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.IEnumerableExtensions
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System.Data.Entity.SqlServer.Utilities
{
  [DebuggerStepThrough]
  internal static class IEnumerableExtensions
  {
    public static string Uniquify(this IEnumerable<string> inputStrings, string targetString)
    {
      string uniqueString = targetString;
      int num = 0;
      while (inputStrings.Any<string>((Func<string, bool>) (n => string.Equals(n, uniqueString, StringComparison.Ordinal))))
        uniqueString = targetString + (++num).ToString();
      return uniqueString;
    }

    public static void Each<T>(this IEnumerable<T> ts, Action<T, int> action)
    {
      int num = 0;
      foreach (T t in ts)
        action(t, num++);
    }

    public static void Each<T>(this IEnumerable<T> ts, Action<T> action)
    {
      foreach (T t in ts)
        action(t);
    }

    public static void Each<T, S>(this IEnumerable<T> ts, Func<T, S> action)
    {
      foreach (T t in ts)
      {
        S s = action(t);
      }
    }

    public static string Join<T>(
      this IEnumerable<T> ts,
      Func<T, string> selector = null,
      string separator = ", ")
    {
      selector = selector ?? (Func<T, string>) (t => t.ToString());
      return string.Join(separator, ts.Where<T>((Func<T, bool>) (t => (object) t != null)).Select<T, string>(selector));
    }

    public static IEnumerable<TSource> Prepend<TSource>(
      this IEnumerable<TSource> source,
      TSource value)
    {
      yield return value;
      foreach (TSource source1 in source)
        yield return source1;
    }

    public static IEnumerable<TSource> Append<TSource>(
      this IEnumerable<TSource> source,
      TSource value)
    {
      foreach (TSource source1 in source)
        yield return source1;
      yield return value;
    }
  }
}
