// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Pluralization.PluralizationServiceUtil
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.Data.Entity.Infrastructure.Pluralization
{
  internal static class PluralizationServiceUtil
  {
    internal static bool DoesWordContainSuffix(
      string word,
      IEnumerable<string> suffixes,
      CultureInfo culture)
    {
      return suffixes.Any<string>((Func<string, bool>) (s => word.EndsWith(s, true, culture)));
    }

    internal static bool TryGetMatchedSuffixForWord(
      string word,
      IEnumerable<string> suffixes,
      CultureInfo culture,
      out string matchedSuffix)
    {
      matchedSuffix = (string) null;
      if (!PluralizationServiceUtil.DoesWordContainSuffix(word, suffixes, culture))
        return false;
      matchedSuffix = suffixes.First<string>((Func<string, bool>) (s => word.EndsWith(s, true, culture)));
      return true;
    }

    internal static bool TryInflectOnSuffixInWord(
      string word,
      IEnumerable<string> suffixes,
      Func<string, string> operationOnWord,
      CultureInfo culture,
      out string newWord)
    {
      newWord = (string) null;
      if (!PluralizationServiceUtil.TryGetMatchedSuffixForWord(word, suffixes, culture, out string _))
        return false;
      newWord = operationOnWord(word);
      return true;
    }
  }
}
