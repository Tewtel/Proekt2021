// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.MediaTypeHeaderValueExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
  internal static class MediaTypeHeaderValueExtensions
  {
    public static bool IsSubsetOf(
      this MediaTypeHeaderValue mediaType1,
      MediaTypeHeaderValue mediaType2)
    {
      return mediaType1.IsSubsetOf(mediaType2, out MediaTypeHeaderValueRange _);
    }

    public static bool IsSubsetOf(
      this MediaTypeHeaderValue mediaType1,
      MediaTypeHeaderValue mediaType2,
      out MediaTypeHeaderValueRange mediaType2Range)
    {
      if (mediaType2 == null)
      {
        mediaType2Range = MediaTypeHeaderValueRange.None;
        return false;
      }
      ParsedMediaTypeHeaderValue mediaTypeHeaderValue = new ParsedMediaTypeHeaderValue(mediaType1);
      ParsedMediaTypeHeaderValue other = new ParsedMediaTypeHeaderValue(mediaType2);
      mediaType2Range = other.IsAllMediaRange ? MediaTypeHeaderValueRange.AllMediaRange : (other.IsSubtypeMediaRange ? MediaTypeHeaderValueRange.SubtypeMediaRange : MediaTypeHeaderValueRange.None);
      if (!mediaTypeHeaderValue.TypesEqual(ref other))
      {
        if (mediaType2Range != MediaTypeHeaderValueRange.AllMediaRange)
          return false;
      }
      else if (!mediaTypeHeaderValue.SubTypesEqual(ref other) && mediaType2Range != MediaTypeHeaderValueRange.SubtypeMediaRange)
        return false;
      Collection<NameValueHeaderValue> collection1 = mediaType1.Parameters.AsCollection<NameValueHeaderValue>();
      int count1 = collection1.Count;
      Collection<NameValueHeaderValue> collection2 = mediaType2.Parameters.AsCollection<NameValueHeaderValue>();
      int count2 = collection2.Count;
      for (int index1 = 0; index1 < count1; ++index1)
      {
        NameValueHeaderValue valueHeaderValue1 = collection1[index1];
        bool flag = false;
        for (int index2 = 0; index2 < count2; ++index2)
        {
          NameValueHeaderValue valueHeaderValue2 = collection2[index2];
          if (valueHeaderValue1.Equals((object) valueHeaderValue2))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      return true;
    }
  }
}
