// Decompiled with JetBrains decompiler
// Type: System.Net.Http.UnsupportedMediaTypeException
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Headers;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary> Defines an exception type for signalling that a request's media type was not supported. </summary>
  public class UnsupportedMediaTypeException : Exception
  {
    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.UnsupportedMediaTypeException" /> class. </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="mediaType">The unsupported media type.</param>
    public UnsupportedMediaTypeException(string message, MediaTypeHeaderValue mediaType)
      : base(message)
    {
      this.MediaType = mediaType != null ? mediaType : throw Error.ArgumentNull(nameof (mediaType));
    }

    /// <summary>Gets or sets the media type.</summary>
    /// <returns>The media type.</returns>
    public MediaTypeHeaderValue MediaType { get; private set; }
  }
}
