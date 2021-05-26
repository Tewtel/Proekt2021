// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.IDictionaryExtensions
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;

namespace System.Data.Entity.SqlServer.Utilities
{
  internal static class IDictionaryExtensions
  {
    internal static void Add<TKey, TValue>(
      this IDictionary<TKey, IList<TValue>> map,
      TKey key,
      TValue value)
    {
      IList<TValue> objList;
      if (!map.TryGetValue(key, out objList))
      {
        objList = (IList<TValue>) new List<TValue>();
        map[key] = objList;
      }
      objList.Add(value);
    }
  }
}
