// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpUnsortedResponse
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Headers;

namespace System.Net.Http
{
  internal class HttpUnsortedResponse
  {
    public HttpUnsortedResponse() => this.HttpHeaders = (HttpHeaders) new HttpUnsortedHeaders();

    public Version Version { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public string ReasonPhrase { get; set; }

    public HttpHeaders HttpHeaders { get; private set; }
  }
}
