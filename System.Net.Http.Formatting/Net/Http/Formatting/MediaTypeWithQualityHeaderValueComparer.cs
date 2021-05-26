// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.MediaTypeWithQualityHeaderValueComparer
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
  internal class MediaTypeWithQualityHeaderValueComparer : IComparer<MediaTypeWithQualityHeaderValue>
  {
    private static readonly MediaTypeWithQualityHeaderValueComparer _mediaTypeComparer = new MediaTypeWithQualityHeaderValueComparer();

    private MediaTypeWithQualityHeaderValueComparer()
    {
    }

    public static MediaTypeWithQualityHeaderValueComparer QualityComparer => MediaTypeWithQualityHeaderValueComparer._mediaTypeComparer;

    public int Compare(
      MediaTypeWithQualityHeaderValue mediaType1,
      MediaTypeWithQualityHeaderValue mediaType2)
    {
      if (mediaType1 == mediaType2)
        return 0;
      int num = MediaTypeWithQualityHeaderValueComparer.CompareBasedOnQualityFactor(mediaType1, mediaType2);
      if (num == 0)
      {
        ParsedMediaTypeHeaderValue mediaTypeHeaderValue = new ParsedMediaTypeHeaderValue((MediaTypeHeaderValue) mediaType1);
        ParsedMediaTypeHeaderValue other = new ParsedMediaTypeHeaderValue((MediaTypeHeaderValue) mediaType2);
        if (!mediaTypeHeaderValue.TypesEqual(ref other))
        {
          if (mediaTypeHeaderValue.IsAllMediaRange)
            return -1;
          if (other.IsAllMediaRange)
            return 1;
          if (mediaTypeHeaderValue.IsSubtypeMediaRange && !other.IsSubtypeMediaRange)
            return -1;
          if (!mediaTypeHeaderValue.IsSubtypeMediaRange && other.IsSubtypeMediaRange)
            return 1;
        }
        else if (!mediaTypeHeaderValue.SubTypesEqual(ref other))
        {
          if (mediaTypeHeaderValue.IsSubtypeMediaRange)
            return -1;
          if (other.IsSubtypeMediaRange)
            return 1;
        }
      }
      return num;
    }

    private static int CompareBasedOnQualityFactor(
      MediaTypeWithQualityHeaderValue mediaType1,
      MediaTypeWithQualityHeaderValue mediaType2)
    {
      double? quality = mediaType1.Quality;
      double num1 = quality ?? 1.0;
      quality = mediaType2.Quality;
      double num2 = quality ?? 1.0;
      double num3 = num1 - num2;
      if (num3 < 0.0)
        return -1;
      return num3 > 0.0 ? 1 : 0;
    }
  }
}
