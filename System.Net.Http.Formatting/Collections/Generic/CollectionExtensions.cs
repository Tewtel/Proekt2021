// Decompiled with JetBrains decompiler
// Type: System.Collections.Generic.CollectionExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.ObjectModel;
using System.Linq;

namespace System.Collections.Generic
{
  internal static class CollectionExtensions
  {
    public static T[] AppendAndReallocate<T>(this T[] array, T value)
    {
      int length = array.Length;
      T[] objArray = new T[length + 1];
      array.CopyTo((Array) objArray, 0);
      objArray[length] = value;
      return objArray;
    }

    public static T[] AsArray<T>(this IEnumerable<T> values)
    {
      if (!(values is T[] objArray))
        objArray = values.ToArray<T>();
      return objArray;
    }

    public static Collection<T> AsCollection<T>(this IEnumerable<T> enumerable)
    {
      switch (enumerable)
      {
        case Collection<T> collection:
          return collection;
        case IList<T> list:
label_3:
          return new Collection<T>(list);
        default:
          list = (IList<T>) new List<T>(enumerable);
          goto label_3;
      }
    }

    public static IList<T> AsIList<T>(this IEnumerable<T> enumerable) => enumerable is IList<T> objList ? objList : (IList<T>) new List<T>(enumerable);

    public static List<T> AsList<T>(this IEnumerable<T> enumerable)
    {
      switch (enumerable)
      {
        case List<T> objList:
          return objList;
        case ListWrapperCollection<T> wrapperCollection:
          return wrapperCollection.ItemsList;
        default:
          return new List<T>(enumerable);
      }
    }

    public static void RemoveFrom<T>(this List<T> list, int start) => list.RemoveRange(start, list.Count - start);

    public static T SingleDefaultOrError<T, TArg1>(
      this IList<T> list,
      Action<TArg1> errorAction,
      TArg1 errorArg1)
    {
      switch (list.Count)
      {
        case 0:
          return default (T);
        case 1:
          return list[0];
        default:
          errorAction(errorArg1);
          return default (T);
      }
    }

    public static TMatch SingleOfTypeDefaultOrError<TInput, TMatch, TArg1>(
      this IList<TInput> list,
      Action<TArg1> errorAction,
      TArg1 errorArg1)
      where TMatch : class
    {
      TMatch match1 = default (TMatch);
      for (int index = 0; index < list.Count; ++index)
      {
        if (list[index] is TMatch match3)
        {
          if ((object) match1 == null)
          {
            match1 = match3;
          }
          else
          {
            errorAction(errorArg1);
            return default (TMatch);
          }
        }
      }
      return match1;
    }

    public static T[] ToArrayWithoutNulls<T>(this ICollection<T> collection) where T : class
    {
      T[] objArray1 = new T[collection.Count];
      int length = 0;
      foreach (T obj in (IEnumerable<T>) collection)
      {
        if ((object) obj != null)
        {
          objArray1[length] = obj;
          ++length;
        }
      }
      if (length == collection.Count)
        return objArray1;
      T[] objArray2 = new T[length];
      Array.Copy((Array) objArray1, (Array) objArray2, length);
      return objArray2;
    }

    public static Dictionary<TKey, TValue> ToDictionaryFast<TKey, TValue>(
      this TValue[] array,
      Func<TValue, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(array.Length, comparer);
      for (int index = 0; index < array.Length; ++index)
      {
        TValue obj = array[index];
        dictionary.Add(keySelector(obj), obj);
      }
      return dictionary;
    }

    public static Dictionary<TKey, TValue> ToDictionaryFast<TKey, TValue>(
      this IList<TValue> list,
      Func<TValue, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      return list is TValue[] array ? array.ToDictionaryFast<TKey, TValue>(keySelector, comparer) : CollectionExtensions.ToDictionaryFastNoCheck<TKey, TValue>(list, keySelector, comparer);
    }

    public static Dictionary<TKey, TValue> ToDictionaryFast<TKey, TValue>(
      this IEnumerable<TValue> enumerable,
      Func<TValue, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      switch (enumerable)
      {
        case TValue[] array:
          return array.ToDictionaryFast<TKey, TValue>(keySelector, comparer);
        case IList<TValue> list:
          return CollectionExtensions.ToDictionaryFastNoCheck<TKey, TValue>(list, keySelector, comparer);
        default:
          Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(comparer);
          foreach (TValue obj in enumerable)
            dictionary.Add(keySelector(obj), obj);
          return dictionary;
      }
    }

    private static Dictionary<TKey, TValue> ToDictionaryFastNoCheck<TKey, TValue>(
      IList<TValue> list,
      Func<TValue, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      int count = list.Count;
      Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(count, comparer);
      for (int index = 0; index < count; ++index)
      {
        TValue obj = list[index];
        dictionary.Add(keySelector(obj), obj);
      }
      return dictionary;
    }
  }
}
