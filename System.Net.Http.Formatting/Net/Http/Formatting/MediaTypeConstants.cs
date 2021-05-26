// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.MediaTypeConstants
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
  internal static class MediaTypeConstants
  {
    private static readonly MediaTypeHeaderValue _defaultApplicationXmlMediaType = new MediaTypeHeaderValue("application/xml");
    private static readonly MediaTypeHeaderValue _defaultTextXmlMediaType = new MediaTypeHeaderValue("text/xml");
    private static readonly MediaTypeHeaderValue _defaultApplicationJsonMediaType = new MediaTypeHeaderValue("application/json");
    private static readonly MediaTypeHeaderValue _defaultTextJsonMediaType = new MediaTypeHeaderValue("text/json");
    private static readonly MediaTypeHeaderValue _defaultApplicationOctetStreamMediaType = new MediaTypeHeaderValue("application/octet-stream");
    private static readonly MediaTypeHeaderValue _defaultApplicationFormUrlEncodedMediaType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
    private static readonly MediaTypeHeaderValue _defaultApplicationBsonMediaType = new MediaTypeHeaderValue("application/bson");

    public static MediaTypeHeaderValue ApplicationOctetStreamMediaType => MediaTypeConstants._defaultApplicationOctetStreamMediaType.Clone<MediaTypeHeaderValue>();

    public static MediaTypeHeaderValue ApplicationXmlMediaType => MediaTypeConstants._defaultApplicationXmlMediaType.Clone<MediaTypeHeaderValue>();

    public static MediaTypeHeaderValue ApplicationJsonMediaType => MediaTypeConstants._defaultApplicationJsonMediaType.Clone<MediaTypeHeaderValue>();

    public static MediaTypeHeaderValue TextXmlMediaType => MediaTypeConstants._defaultTextXmlMediaType.Clone<MediaTypeHeaderValue>();

    public static MediaTypeHeaderValue TextJsonMediaType => MediaTypeConstants._defaultTextJsonMediaType.Clone<MediaTypeHeaderValue>();

    public static MediaTypeHeaderValue ApplicationFormUrlEncodedMediaType => MediaTypeConstants._defaultApplicationFormUrlEncodedMediaType.Clone<MediaTypeHeaderValue>();

    public static MediaTypeHeaderValue ApplicationBsonMediaType => MediaTypeConstants._defaultApplicationBsonMediaType.Clone<MediaTypeHeaderValue>();
  }
}
