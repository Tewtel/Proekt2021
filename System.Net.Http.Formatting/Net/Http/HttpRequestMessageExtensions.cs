// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpRequestMessageExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.ComponentModel;
using System.Web.Http;

namespace System.Net.Http
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class HttpRequestMessageExtensions
  {
    public static HttpResponseMessage CreateResponse(
      this HttpRequestMessage request,
      HttpStatusCode statusCode)
    {
      return request != null ? new HttpResponseMessage()
      {
        StatusCode = statusCode,
        RequestMessage = request
      } : throw Error.ArgumentNull(nameof (request));
    }

    public static HttpResponseMessage CreateResponse(
      this HttpRequestMessage request)
    {
      return request != null ? new HttpResponseMessage()
      {
        RequestMessage = request
      } : throw Error.ArgumentNull(nameof (request));
    }
  }
}
