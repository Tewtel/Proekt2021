// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.StringWithQualityHeaderValueComparer
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
  internal class StringWithQualityHeaderValueComparer : IComparer<StringWithQualityHeaderValue>
  {
    private static readonly StringWithQualityHeaderValueComparer _qualityComparer = new StringWithQualityHeaderValueComparer();

    private StringWithQualityHeaderValueComparer()
    {
    }

    public static StringWithQualityHeaderValueComparer QualityComparer => StringWithQualityHeaderValueComparer._qualityComparer;

    public int Compare(
      StringWithQualityHeaderValue stringWithQuality1,
      StringWithQualityHeaderValue stringWithQuality2)
    {
      double? quality = stringWithQuality1.Quality;
      double num1 = quality ?? 1.0;
      quality = stringWithQuality2.Quality;
      double num2 = quality ?? 1.0;
      double num3 = num1 - num2;
      if (num3 < 0.0)
        return -1;
      if (num3 > 0.0)
        return 1;
      if (!string.Equals(stringWithQuality1.Value, stringWithQuality2.Value, StringComparison.OrdinalIgnoreCase))
      {
        if (string.Equals(stringWithQuality1.Value, "*", StringComparison.OrdinalIgnoreCase))
          return -1;
        if (string.Equals(stringWithQuality2.Value, "*", StringComparison.OrdinalIgnoreCase))
          return 1;
      }
      return 0;
    }
  }
}
