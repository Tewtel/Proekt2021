// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.Utilities
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System.Collections.Generic;

namespace HtmlAgilityPack
{
  internal static class Utilities
  {
    public static TValue GetDictionaryValueOrDefault<TKey, TValue>(
      Dictionary<TKey, TValue> dict,
      TKey key,
      TValue defaultValue = null)
      where TKey : class
    {
      TValue obj;
      return !dict.TryGetValue(key, out obj) ? defaultValue : obj;
    }
  }
}
