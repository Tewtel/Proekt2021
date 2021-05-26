// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.HashSetExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Utilities
{
  internal static class HashSetExtensions
  {
    public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
    {
      foreach (T obj in items)
        set.Add(obj);
    }
  }
}
