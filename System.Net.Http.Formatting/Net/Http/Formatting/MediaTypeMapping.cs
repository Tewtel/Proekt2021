// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.MediaTypeMapping
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Headers;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary> An abstract base class used to create an association between <see cref="T:System.Net.Http.HttpRequestMessage" /> or  <see cref="T:System.Net.Http.HttpResponseMessage" /> instances that have certain characteristics  and a specific <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" />.  </summary>
  public abstract class MediaTypeMapping
  {
    /// <summary> Initializes a new instance of a <see cref="T:System.Net.Http.Formatting.MediaTypeMapping" /> with the given mediaType value. </summary>
    /// <param name="mediaType"> The <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> that is associated with <see cref="T:System.Net.Http.HttpRequestMessage" /> or  <see cref="T:System.Net.Http.HttpResponseMessage" /> instances that have the given characteristics of the  <see cref="T:System.Net.Http.Formatting.MediaTypeMapping" />. </param>
    protected MediaTypeMapping(MediaTypeHeaderValue mediaType) => this.MediaType = mediaType != null ? mediaType : throw Error.ArgumentNull(nameof (mediaType));

    /// <summary> Initializes a new instance of a <see cref="T:System.Net.Http.Formatting.MediaTypeMapping" /> with the given mediaType value. </summary>
    /// <param name="mediaType"> The <see cref="T:System.String" /> that is associated with <see cref="T:System.Net.Http.HttpRequestMessage" /> or  <see cref="T:System.Net.Http.HttpResponseMessage" /> instances that have the given characteristics of the  <see cref="T:System.Net.Http.Formatting.MediaTypeMapping" />. </param>
    protected MediaTypeMapping(string mediaType) => this.MediaType = !string.IsNullOrWhiteSpace(mediaType) ? new MediaTypeHeaderValue(mediaType) : throw Error.ArgumentNull(nameof (mediaType));

    /// <summary> Gets the <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> that is associated with <see cref="T:System.Net.Http.HttpRequestMessage" /> or  <see cref="T:System.Net.Http.HttpResponseMessage" /> instances that have the given characteristics of the  <see cref="T:System.Net.Http.Formatting.MediaTypeMapping" />. </summary>
    public MediaTypeHeaderValue MediaType { get; private set; }

    /// <summary> Returns the quality of the match of the <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> associated with request. </summary>
    /// <returns> The quality of the match. It must be between 0.0 and 1.0. A value of 0.0 signifies no match. A value of 1.0 signifies a complete match. </returns>
    /// <param name="request"> The <see cref="T:System.Net.Http.HttpRequestMessage" /> to evaluate for the characteristics  associated with the <see cref="T:System.Net.Http.Headers.MediaTypeHeaderValue" /> of the <see cref="T:System.Net.Http.Formatting.MediaTypeMapping" />. </param>
    public abstract double TryMatchMediaType(HttpRequestMessage request);
  }
}
