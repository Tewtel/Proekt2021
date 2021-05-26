// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.IDbAsyncEnumerableExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure
{
  internal static class IDbAsyncEnumerableExtensions
  {
    internal static async Task ForEachAsync(
      this IDbAsyncEnumerable source,
      Action<object> action,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      using (IDbAsyncEnumerator enumerator = source.GetAsyncEnumerator())
      {
        if (await enumerator.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
        {
          Task<bool> task;
          do
          {
            cancellationToken.ThrowIfCancellationRequested();
            object current = enumerator.Current;
            task = enumerator.MoveNextAsync(cancellationToken);
            action(current);
          }
          while (await task.WithCurrentCulture<bool>());
        }
      }
    }

    internal static Task ForEachAsync<T>(
      this IDbAsyncEnumerable<T> source,
      Action<T> action,
      CancellationToken cancellationToken)
    {
      return IDbAsyncEnumerableExtensions.ForEachAsync<T>(source.GetAsyncEnumerator(), action, cancellationToken);
    }

    private static async Task ForEachAsync<T>(
      IDbAsyncEnumerator<T> enumerator,
      Action<T> action,
      CancellationToken cancellationToken)
    {
      using (enumerator)
      {
        cancellationToken.ThrowIfCancellationRequested();
        System.Data.Entity.Utilities.TaskExtensions.CultureAwaiter<bool> cultureAwaiter = enumerator.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>();
        if (await cultureAwaiter)
        {
          do
          {
            cancellationToken.ThrowIfCancellationRequested();
            T current = enumerator.Current;
            Task<bool> task = enumerator.MoveNextAsync(cancellationToken);
            action(current);
            cultureAwaiter = task.WithCurrentCulture<bool>();
          }
          while (await cultureAwaiter);
        }
      }
    }

    internal static Task<List<T>> ToListAsync<T>(this IDbAsyncEnumerable source) => source.ToListAsync<T>(CancellationToken.None);

    internal static async Task<List<T>> ToListAsync<T>(
      this IDbAsyncEnumerable source,
      CancellationToken cancellationToken)
    {
      List<T> list = new List<T>();
      await source.ForEachAsync((Action<object>) (e => list.Add((T) e)), cancellationToken).WithCurrentCulture();
      return list;
    }

    internal static Task<List<T>> ToListAsync<T>(this IDbAsyncEnumerable<T> source) => IDbAsyncEnumerableExtensions.ToListAsync<T>(source, CancellationToken.None);

    internal static Task<List<T>> ToListAsync<T>(
      this IDbAsyncEnumerable<T> source,
      CancellationToken cancellationToken)
    {
      TaskCompletionSource<List<T>> tcs = new TaskCompletionSource<List<T>>();
      List<T> list = new List<T>();
      source.ForEachAsync<T>(new Action<T>(list.Add), cancellationToken).ContinueWith((Action<Task>) (t =>
      {
        if (t.IsFaulted)
          tcs.TrySetException((IEnumerable<Exception>) t.Exception.InnerExceptions);
        else if (t.IsCanceled)
          tcs.TrySetCanceled();
        else
          tcs.TrySetResult(list);
      }), TaskContinuationOptions.ExecuteSynchronously);
      return tcs.Task;
    }

    internal static Task<T[]> ToArrayAsync<T>(this IDbAsyncEnumerable<T> source) => source.ToArrayAsync<T>(CancellationToken.None);

    internal static async Task<T[]> ToArrayAsync<T>(
      this IDbAsyncEnumerable<T> source,
      CancellationToken cancellationToken)
    {
      return (await IDbAsyncEnumerableExtensions.ToListAsync<T>(source, cancellationToken).WithCurrentCulture<List<T>>()).ToArray();
    }

    internal static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      return source.ToDictionaryAsync<TSource, TKey, TSource>(keySelector, IDbAsyncEnumerableExtensions.IdentityFunction<TSource>.Instance, (IEqualityComparer<TKey>) null, CancellationToken.None);
    }

    internal static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      CancellationToken cancellationToken)
    {
      return source.ToDictionaryAsync<TSource, TKey, TSource>(keySelector, IDbAsyncEnumerableExtensions.IdentityFunction<TSource>.Instance, (IEqualityComparer<TKey>) null, cancellationToken);
    }

    internal static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return source.ToDictionaryAsync<TSource, TKey, TSource>(keySelector, IDbAsyncEnumerableExtensions.IdentityFunction<TSource>.Instance, comparer, CancellationToken.None);
    }

    internal static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      IEqualityComparer<TKey> comparer,
      CancellationToken cancellationToken)
    {
      return source.ToDictionaryAsync<TSource, TKey, TSource>(keySelector, IDbAsyncEnumerableExtensions.IdentityFunction<TSource>.Instance, comparer, cancellationToken);
    }

    internal static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector)
    {
      return source.ToDictionaryAsync<TSource, TKey, TElement>(keySelector, elementSelector, (IEqualityComparer<TKey>) null, CancellationToken.None);
    }

    internal static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      CancellationToken cancellationToken)
    {
      return source.ToDictionaryAsync<TSource, TKey, TElement>(keySelector, elementSelector, (IEqualityComparer<TKey>) null, cancellationToken);
    }

    internal static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      return source.ToDictionaryAsync<TSource, TKey, TElement>(keySelector, elementSelector, comparer, CancellationToken.None);
    }

    internal static async Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, TKey> keySelector,
      Func<TSource, TElement> elementSelector,
      IEqualityComparer<TKey> comparer,
      CancellationToken cancellationToken)
    {
      Dictionary<TKey, TElement> d = new Dictionary<TKey, TElement>(comparer);
      await source.ForEachAsync<TSource>((Action<TSource>) (element => d.Add(keySelector(element), elementSelector(element))), cancellationToken).WithCurrentCulture();
      return d;
    }

    internal static IDbAsyncEnumerable<TResult> Cast<TResult>(
      this IDbAsyncEnumerable source)
    {
      return (IDbAsyncEnumerable<TResult>) new IDbAsyncEnumerableExtensions.CastDbAsyncEnumerable<TResult>(source);
    }

    internal static Task<TSource> FirstAsync<TSource>(this IDbAsyncEnumerable<TSource> source) => source.FirstAsync<TSource>(CancellationToken.None);

    internal static Task<TSource> FirstAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source.FirstAsync<TSource>(predicate, CancellationToken.None);
    }

    internal static async Task<TSource> FirstAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      TSource current;
      using (IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator())
      {
        if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
        {
          current = e.Current;
          goto label_8;
        }
      }
      throw Error.EmptySequence();
label_8:
      return current;
    }

    internal static async Task<TSource> FirstAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      TSource current;
      using (IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator())
      {
        if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
        {
          if (predicate(e.Current))
          {
            current = e.Current;
            goto label_9;
          }
        }
      }
      throw Error.NoMatch();
label_9:
      return current;
    }

    internal static Task<TSource> FirstOrDefaultAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source)
    {
      return source.FirstOrDefaultAsync<TSource>(CancellationToken.None);
    }

    internal static Task<TSource> FirstOrDefaultAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source.FirstOrDefaultAsync<TSource>(predicate, CancellationToken.None);
    }

    internal static async Task<TSource> FirstOrDefaultAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      using (IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator())
      {
        if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          return e.Current;
      }
      return default (TSource);
    }

    internal static async Task<TSource> FirstOrDefaultAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      using (IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator())
      {
        if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
        {
          if (predicate(e.Current))
            return e.Current;
        }
      }
      return default (TSource);
    }

    internal static Task<TSource> SingleAsync<TSource>(this IDbAsyncEnumerable<TSource> source) => source.SingleAsync<TSource>(CancellationToken.None);

    internal static async Task<TSource> SingleAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      TSource source1;
      using (IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator())
      {
        if (!await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          throw Error.EmptySequence();
        cancellationToken.ThrowIfCancellationRequested();
        TSource result = e.Current;
        if (!await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
        {
          source1 = result;
          goto label_12;
        }
        else
          result = default (TSource);
      }
      throw Error.MoreThanOneElement();
label_12:
      return source1;
    }

    internal static Task<TSource> SingleAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source.SingleAsync<TSource>(predicate, CancellationToken.None);
    }

    internal static async Task<TSource> SingleAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      TSource result = default (TSource);
      long count = 0;
      IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
              cancellationToken.ThrowIfCancellationRequested();
            else
              goto label_9;
          }
          while (!predicate(e.Current));
          result = e.Current;
          checked { ++count; }
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<TSource>) null;
      switch (count)
      {
        case 0:
          throw Error.NoMatch();
        case 1:
          return result;
        default:
          throw Error.MoreThanOneMatch();
      }
    }

    internal static Task<TSource> SingleOrDefaultAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source)
    {
      return source.SingleOrDefaultAsync<TSource>(CancellationToken.None);
    }

    internal static async Task<TSource> SingleOrDefaultAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      using (IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator())
      {
        if (!await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          return default (TSource);
        cancellationToken.ThrowIfCancellationRequested();
        TSource result = e.Current;
        if (!await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          return result;
        result = default (TSource);
      }
      throw Error.MoreThanOneElement();
    }

    internal static Task<TSource> SingleOrDefaultAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source.SingleOrDefaultAsync<TSource>(predicate, CancellationToken.None);
    }

    internal static async Task<TSource> SingleOrDefaultAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      TSource result = default (TSource);
      long count = 0;
      IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
              cancellationToken.ThrowIfCancellationRequested();
            else
              goto label_9;
          }
          while (!predicate(e.Current));
          result = e.Current;
          checked { ++count; }
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<TSource>) null;
      if (count < 2L)
        return result;
      throw Error.MoreThanOneMatch();
    }

    internal static Task<bool> ContainsAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      TSource value)
    {
      return source.ContainsAsync<TSource>(value, CancellationToken.None);
    }

    internal static async Task<bool> ContainsAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      TSource value,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            if (!EqualityComparer<TSource>.Default.Equals(e.Current, value))
              cancellationToken.ThrowIfCancellationRequested();
            else
              break;
          }
          else
            goto label_10;
        }
        return true;
      }
      finally
      {
        e?.Dispose();
      }
label_10:
      e = (IDbAsyncEnumerator<TSource>) null;
      return false;
    }

    internal static Task<bool> AnyAsync<TSource>(this IDbAsyncEnumerable<TSource> source) => source.AnyAsync<TSource>(CancellationToken.None);

    internal static async Task<bool> AnyAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      using (IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator())
      {
        if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          return true;
      }
      return false;
    }

    internal static Task<bool> AnyAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source.AnyAsync<TSource>(predicate, CancellationToken.None);
    }

    internal static async Task<bool> AnyAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            if (!predicate(e.Current))
              cancellationToken.ThrowIfCancellationRequested();
            else
              break;
          }
          else
            goto label_10;
        }
        return true;
      }
      finally
      {
        e?.Dispose();
      }
label_10:
      e = (IDbAsyncEnumerator<TSource>) null;
      return false;
    }

    internal static Task<bool> AllAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source.AllAsync<TSource>(predicate, CancellationToken.None);
    }

    internal static async Task<bool> AllAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            if (predicate(e.Current))
              cancellationToken.ThrowIfCancellationRequested();
            else
              break;
          }
          else
            goto label_10;
        }
        return false;
      }
      finally
      {
        e?.Dispose();
      }
label_10:
      e = (IDbAsyncEnumerator<TSource>) null;
      return true;
    }

    internal static Task<int> CountAsync<TSource>(this IDbAsyncEnumerable<TSource> source) => source.CountAsync<TSource>(CancellationToken.None);

    internal static async Task<int> CountAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      int count = 0;
      using (IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            checked { ++count; }
          }
          else
            break;
        }
      }
      return count;
    }

    internal static Task<int> CountAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source.CountAsync<TSource>(predicate, CancellationToken.None);
    }

    internal static async Task<int> CountAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      int count = 0;
      IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
              cancellationToken.ThrowIfCancellationRequested();
            else
              goto label_9;
          }
          while (!predicate(e.Current));
          checked { ++count; }
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<TSource>) null;
      return count;
    }

    internal static Task<long> LongCountAsync<TSource>(this IDbAsyncEnumerable<TSource> source) => source.LongCountAsync<TSource>(CancellationToken.None);

    internal static async Task<long> LongCountAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      long count = 0;
      using (IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            checked { ++count; }
          }
          else
            break;
        }
      }
      return count;
    }

    internal static Task<long> LongCountAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source.LongCountAsync<TSource>(predicate, CancellationToken.None);
    }

    internal static async Task<long> LongCountAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      Func<TSource, bool> predicate,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      long count = 0;
      IDbAsyncEnumerator<TSource> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
              cancellationToken.ThrowIfCancellationRequested();
            else
              goto label_9;
          }
          while (!predicate(e.Current));
          checked { ++count; }
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<TSource>) null;
      return count;
    }

    internal static Task<TSource> MinAsync<TSource>(this IDbAsyncEnumerable<TSource> source) => source.MinAsync<TSource>(CancellationToken.None);

    internal static async Task<TSource> MinAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      Comparer<TSource> comparer = Comparer<TSource>.Default;
      TSource value = default (TSource);
      IDbAsyncEnumerator<TSource> e;
      if ((object) value == null)
      {
        e = source.GetAsyncEnumerator();
        try
        {
          while (true)
          {
            do
            {
              if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
                cancellationToken.ThrowIfCancellationRequested();
              else
                goto label_10;
            }
            while ((object) e.Current == null || (object) value != null && comparer.Compare(e.Current, value) >= 0);
            value = e.Current;
          }
        }
        finally
        {
          e?.Dispose();
        }
label_10:
        e = (IDbAsyncEnumerator<TSource>) null;
        return value;
      }
      bool hasValue = false;
      e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              if (!hasValue)
                goto label_16;
            }
            else
              goto label_22;
          }
          while (comparer.Compare(e.Current, value) >= 0);
          value = e.Current;
          continue;
label_16:
          value = e.Current;
          hasValue = true;
        }
      }
      finally
      {
        e?.Dispose();
      }
label_22:
      e = (IDbAsyncEnumerator<TSource>) null;
      if (hasValue)
        return value;
      throw Error.EmptySequence();
    }

    internal static Task<TSource> MaxAsync<TSource>(this IDbAsyncEnumerable<TSource> source) => source.MaxAsync<TSource>(CancellationToken.None);

    internal static async Task<TSource> MaxAsync<TSource>(
      this IDbAsyncEnumerable<TSource> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      Comparer<TSource> comparer = Comparer<TSource>.Default;
      TSource value = default (TSource);
      IDbAsyncEnumerator<TSource> e;
      if ((object) value == null)
      {
        e = source.GetAsyncEnumerator();
        try
        {
          while (true)
          {
            do
            {
              if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
                cancellationToken.ThrowIfCancellationRequested();
              else
                goto label_10;
            }
            while ((object) e.Current == null || (object) value != null && comparer.Compare(e.Current, value) <= 0);
            value = e.Current;
          }
        }
        finally
        {
          e?.Dispose();
        }
label_10:
        e = (IDbAsyncEnumerator<TSource>) null;
        return value;
      }
      bool hasValue = false;
      e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              if (!hasValue)
                goto label_16;
            }
            else
              goto label_22;
          }
          while (comparer.Compare(e.Current, value) <= 0);
          value = e.Current;
          continue;
label_16:
          value = e.Current;
          hasValue = true;
        }
      }
      finally
      {
        e?.Dispose();
      }
label_22:
      e = (IDbAsyncEnumerator<TSource>) null;
      if (hasValue)
        return value;
      throw Error.EmptySequence();
    }

    internal static Task<int> SumAsync(this IDbAsyncEnumerable<int> source) => source.SumAsync(CancellationToken.None);

    internal static async Task<int> SumAsync(
      this IDbAsyncEnumerable<int> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      long sum = 0;
      using (IDbAsyncEnumerator<int> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            checked { sum += (long) e.Current; }
          }
          else
            break;
        }
      }
      return (int) sum;
    }

    internal static Task<int?> SumAsync(this IDbAsyncEnumerable<int?> source) => source.SumAsync(CancellationToken.None);

    internal static async Task<int?> SumAsync(
      this IDbAsyncEnumerable<int?> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      long sum = 0;
      IDbAsyncEnumerator<int?> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          int? current;
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              current = e.Current;
            }
            else
              goto label_9;
          }
          while (!current.HasValue);
          long num = sum;
          current = e.Current;
          long valueOrDefault = (long) current.GetValueOrDefault();
          sum = checked (num + valueOrDefault);
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<int?>) null;
      return new int?((int) sum);
    }

    internal static Task<long> SumAsync(this IDbAsyncEnumerable<long> source) => source.SumAsync(CancellationToken.None);

    internal static async Task<long> SumAsync(
      this IDbAsyncEnumerable<long> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      long sum = 0;
      using (IDbAsyncEnumerator<long> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            checked { sum += e.Current; }
          }
          else
            break;
        }
      }
      return sum;
    }

    internal static Task<long?> SumAsync(this IDbAsyncEnumerable<long?> source) => source.SumAsync(CancellationToken.None);

    internal static async Task<long?> SumAsync(
      this IDbAsyncEnumerable<long?> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      long sum = 0;
      IDbAsyncEnumerator<long?> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          long? current;
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              current = e.Current;
            }
            else
              goto label_9;
          }
          while (!current.HasValue);
          long num = sum;
          current = e.Current;
          long valueOrDefault = current.GetValueOrDefault();
          sum = checked (num + valueOrDefault);
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<long?>) null;
      return new long?(sum);
    }

    internal static Task<float> SumAsync(this IDbAsyncEnumerable<float> source) => source.SumAsync(CancellationToken.None);

    internal static async Task<float> SumAsync(
      this IDbAsyncEnumerable<float> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      double sum = 0.0;
      using (IDbAsyncEnumerator<float> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            sum += (double) e.Current;
          }
          else
            break;
        }
      }
      return (float) sum;
    }

    internal static Task<float?> SumAsync(this IDbAsyncEnumerable<float?> source) => source.SumAsync(CancellationToken.None);

    internal static async Task<float?> SumAsync(
      this IDbAsyncEnumerable<float?> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      double sum = 0.0;
      IDbAsyncEnumerator<float?> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          float? current;
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              current = e.Current;
            }
            else
              goto label_9;
          }
          while (!current.HasValue);
          double num = sum;
          current = e.Current;
          double valueOrDefault = (double) current.GetValueOrDefault();
          sum = num + valueOrDefault;
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<float?>) null;
      return new float?((float) sum);
    }

    internal static Task<double> SumAsync(this IDbAsyncEnumerable<double> source) => source.SumAsync(CancellationToken.None);

    internal static async Task<double> SumAsync(
      this IDbAsyncEnumerable<double> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      double sum = 0.0;
      using (IDbAsyncEnumerator<double> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            sum += e.Current;
          }
          else
            break;
        }
      }
      return sum;
    }

    internal static Task<double?> SumAsync(this IDbAsyncEnumerable<double?> source) => source.SumAsync(CancellationToken.None);

    internal static async Task<double?> SumAsync(
      this IDbAsyncEnumerable<double?> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      double sum = 0.0;
      IDbAsyncEnumerator<double?> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          double? current;
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              current = e.Current;
            }
            else
              goto label_9;
          }
          while (!current.HasValue);
          double num = sum;
          current = e.Current;
          double valueOrDefault = current.GetValueOrDefault();
          sum = num + valueOrDefault;
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<double?>) null;
      return new double?(sum);
    }

    internal static Task<Decimal> SumAsync(this IDbAsyncEnumerable<Decimal> source) => source.SumAsync(CancellationToken.None);

    internal static async Task<Decimal> SumAsync(
      this IDbAsyncEnumerable<Decimal> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      Decimal sum = 0M;
      using (IDbAsyncEnumerator<Decimal> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            sum += e.Current;
          }
          else
            break;
        }
      }
      return sum;
    }

    internal static Task<Decimal?> SumAsync(this IDbAsyncEnumerable<Decimal?> source) => source.SumAsync(CancellationToken.None);

    internal static async Task<Decimal?> SumAsync(
      this IDbAsyncEnumerable<Decimal?> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      Decimal sum = 0M;
      IDbAsyncEnumerator<Decimal?> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          Decimal? current;
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              current = e.Current;
            }
            else
              goto label_9;
          }
          while (!current.HasValue);
          Decimal num = sum;
          current = e.Current;
          Decimal valueOrDefault = current.GetValueOrDefault();
          sum = num + valueOrDefault;
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<Decimal?>) null;
      return new Decimal?(sum);
    }

    internal static Task<double> AverageAsync(this IDbAsyncEnumerable<int> source) => source.AverageAsync(CancellationToken.None);

    internal static async Task<double> AverageAsync(
      this IDbAsyncEnumerable<int> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      long sum = 0;
      long count = 0;
      using (IDbAsyncEnumerator<int> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            checked { sum += (long) e.Current; }
            checked { ++count; }
          }
          else
            break;
        }
      }
      if (count > 0L)
        return (double) sum / (double) count;
      throw Error.EmptySequence();
    }

    internal static Task<double?> AverageAsync(this IDbAsyncEnumerable<int?> source) => source.AverageAsync(CancellationToken.None);

    internal static async Task<double?> AverageAsync(
      this IDbAsyncEnumerable<int?> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      long sum = 0;
      long count = 0;
      IDbAsyncEnumerator<int?> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          int? current;
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              current = e.Current;
            }
            else
              goto label_9;
          }
          while (!current.HasValue);
          long num = sum;
          current = e.Current;
          long valueOrDefault = (long) current.GetValueOrDefault();
          sum = checked (num + valueOrDefault);
          checked { ++count; }
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<int?>) null;
      return count > 0L ? new double?((double) sum / (double) count) : throw Error.EmptySequence();
    }

    internal static Task<double> AverageAsync(this IDbAsyncEnumerable<long> source) => source.AverageAsync(CancellationToken.None);

    internal static async Task<double> AverageAsync(
      this IDbAsyncEnumerable<long> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      long sum = 0;
      long count = 0;
      using (IDbAsyncEnumerator<long> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            checked { sum += e.Current; }
            checked { ++count; }
          }
          else
            break;
        }
      }
      if (count > 0L)
        return (double) sum / (double) count;
      throw Error.EmptySequence();
    }

    internal static Task<double?> AverageAsync(this IDbAsyncEnumerable<long?> source) => source.AverageAsync(CancellationToken.None);

    internal static async Task<double?> AverageAsync(
      this IDbAsyncEnumerable<long?> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      long sum = 0;
      long count = 0;
      IDbAsyncEnumerator<long?> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          long? current;
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              current = e.Current;
            }
            else
              goto label_9;
          }
          while (!current.HasValue);
          long num = sum;
          current = e.Current;
          long valueOrDefault = current.GetValueOrDefault();
          sum = checked (num + valueOrDefault);
          checked { ++count; }
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<long?>) null;
      return count > 0L ? new double?((double) sum / (double) count) : throw Error.EmptySequence();
    }

    internal static Task<float> AverageAsync(this IDbAsyncEnumerable<float> source) => source.AverageAsync(CancellationToken.None);

    internal static async Task<float> AverageAsync(
      this IDbAsyncEnumerable<float> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      double sum = 0.0;
      long count = 0;
      using (IDbAsyncEnumerator<float> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            sum += (double) e.Current;
            checked { ++count; }
          }
          else
            break;
        }
      }
      if (count > 0L)
        return (float) sum / (float) count;
      throw Error.EmptySequence();
    }

    internal static Task<float?> AverageAsync(this IDbAsyncEnumerable<float?> source) => source.AverageAsync(CancellationToken.None);

    internal static async Task<float?> AverageAsync(
      this IDbAsyncEnumerable<float?> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      double sum = 0.0;
      long count = 0;
      IDbAsyncEnumerator<float?> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          float? current;
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              current = e.Current;
            }
            else
              goto label_9;
          }
          while (!current.HasValue);
          double num = sum;
          current = e.Current;
          double valueOrDefault = (double) current.GetValueOrDefault();
          sum = num + valueOrDefault;
          checked { ++count; }
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<float?>) null;
      return count > 0L ? new float?((float) sum / (float) count) : throw Error.EmptySequence();
    }

    internal static Task<double> AverageAsync(this IDbAsyncEnumerable<double> source) => source.AverageAsync(CancellationToken.None);

    internal static async Task<double> AverageAsync(
      this IDbAsyncEnumerable<double> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      double sum = 0.0;
      long count = 0;
      using (IDbAsyncEnumerator<double> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            sum += e.Current;
            checked { ++count; }
          }
          else
            break;
        }
      }
      if (count > 0L)
        return (double) ((float) sum / (float) count);
      throw Error.EmptySequence();
    }

    internal static Task<double?> AverageAsync(this IDbAsyncEnumerable<double?> source) => source.AverageAsync(CancellationToken.None);

    internal static async Task<double?> AverageAsync(
      this IDbAsyncEnumerable<double?> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      double sum = 0.0;
      long count = 0;
      IDbAsyncEnumerator<double?> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          double? current;
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              current = e.Current;
            }
            else
              goto label_9;
          }
          while (!current.HasValue);
          double num = sum;
          current = e.Current;
          double valueOrDefault = current.GetValueOrDefault();
          sum = num + valueOrDefault;
          checked { ++count; }
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<double?>) null;
      return count > 0L ? new double?((double) ((float) sum / (float) count)) : throw Error.EmptySequence();
    }

    internal static Task<Decimal> AverageAsync(this IDbAsyncEnumerable<Decimal> source) => source.AverageAsync(CancellationToken.None);

    internal static async Task<Decimal> AverageAsync(
      this IDbAsyncEnumerable<Decimal> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      Decimal sum = 0M;
      long count = 0;
      using (IDbAsyncEnumerator<Decimal> e = source.GetAsyncEnumerator())
      {
        while (true)
        {
          if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
          {
            cancellationToken.ThrowIfCancellationRequested();
            sum += e.Current;
            checked { ++count; }
          }
          else
            break;
        }
      }
      if (count > 0L)
        return sum / (Decimal) count;
      throw Error.EmptySequence();
    }

    internal static Task<Decimal?> AverageAsync(this IDbAsyncEnumerable<Decimal?> source) => source.AverageAsync(CancellationToken.None);

    internal static async Task<Decimal?> AverageAsync(
      this IDbAsyncEnumerable<Decimal?> source,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      Decimal sum = 0M;
      long count = 0;
      IDbAsyncEnumerator<Decimal?> e = source.GetAsyncEnumerator();
      try
      {
        while (true)
        {
          Decimal? current;
          do
          {
            if (await e.MoveNextAsync(cancellationToken).WithCurrentCulture<bool>())
            {
              cancellationToken.ThrowIfCancellationRequested();
              current = e.Current;
            }
            else
              goto label_9;
          }
          while (!current.HasValue);
          Decimal num = sum;
          current = e.Current;
          Decimal valueOrDefault = current.GetValueOrDefault();
          sum = num + valueOrDefault;
          checked { ++count; }
        }
      }
      finally
      {
        e?.Dispose();
      }
label_9:
      e = (IDbAsyncEnumerator<Decimal?>) null;
      return count > 0L ? new Decimal?(sum / (Decimal) count) : throw Error.EmptySequence();
    }

    private class CastDbAsyncEnumerable<TResult> : IDbAsyncEnumerable<TResult>, IDbAsyncEnumerable
    {
      private readonly IDbAsyncEnumerable _underlyingEnumerable;

      public CastDbAsyncEnumerable(IDbAsyncEnumerable sourceEnumerable) => this._underlyingEnumerable = sourceEnumerable;

      public IDbAsyncEnumerator<TResult> GetAsyncEnumerator() => this._underlyingEnumerable.GetAsyncEnumerator().Cast<TResult>();

      IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() => this._underlyingEnumerable.GetAsyncEnumerator();
    }

    private static class IdentityFunction<TElement>
    {
      internal static Func<TElement, TElement> Instance => (Func<TElement, TElement>) (x => x);
    }
  }
}
