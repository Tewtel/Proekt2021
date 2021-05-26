// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpHeaderExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Net.Http.Headers;

namespace System.Net.Http
{
  internal static class HttpHeaderExtensions
  {
    public static void CopyTo(this HttpContentHeaders fromHeaders, HttpContentHeaders toHeaders)
    {
      foreach (KeyValuePair<string, IEnumerable<string>> fromHeader in (HttpHeaders) fromHeaders)
        toHeaders.TryAddWithoutValidation(fromHeader.Key, fromHeader.Value);
    }
  }
}
