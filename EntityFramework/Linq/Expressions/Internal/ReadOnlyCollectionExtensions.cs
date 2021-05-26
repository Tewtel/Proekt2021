// Decompiled with JetBrains decompiler
// Type: System.Linq.Expressions.Internal.ReadOnlyCollectionExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq.Expressions.Internal
{
  internal static class ReadOnlyCollectionExtensions
  {
    internal static ReadOnlyCollection<T> ToReadOnlyCollection<T>(
      this IEnumerable<T> sequence)
    {
      if (sequence == null)
        return ReadOnlyCollectionExtensions.DefaultReadOnlyCollection<T>.Empty;
      return sequence is ReadOnlyCollection<T> readOnlyCollection ? readOnlyCollection : new ReadOnlyCollection<T>((IList<T>) sequence.ToArray<T>());
    }

    private static class DefaultReadOnlyCollection<T>
    {
      private static ReadOnlyCollection<T> _defaultCollection;

      internal static ReadOnlyCollection<T> Empty
      {
        get
        {
          if (ReadOnlyCollectionExtensions.DefaultReadOnlyCollection<T>._defaultCollection == null)
            ReadOnlyCollectionExtensions.DefaultReadOnlyCollection<T>._defaultCollection = new ReadOnlyCollection<T>((IList<T>) new T[0]);
          return ReadOnlyCollectionExtensions.DefaultReadOnlyCollection<T>._defaultCollection;
        }
      }
    }
  }
}
