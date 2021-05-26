// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.ContentNegotiationResult
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Headers;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary> Represents the result of content negotiation performed using &lt;see cref="M:System.Net.Http.Formatting.IContentNegotiator.Negotiate(System.Type,System.Net.Http.HttpRequestMessage,System.Collections.Generic.IEnumerable{System.Net.Http.Formatting.MediaTypeFormatter})" /&gt;</summary>
  public class ContentNegotiationResult
  {
    private MediaTypeFormatter _formatter;

    /// <summary> Create the content negotiation result object. </summary>
    /// <param name="formatter">The formatter.</param>
    /// <param name="mediaType">The preferred media type. Can be null.</param>
    public ContentNegotiationResult(MediaTypeFormatter formatter, MediaTypeHeaderValue mediaType)
    {
      this._formatter = formatter != null ? formatter : throw Error.ArgumentNull(nameof (formatter));
      this.MediaType = mediaType;
    }

    /// <summary> The formatter chosen for serialization. </summary>
    public MediaTypeFormatter Formatter
    {
      get => this._formatter;
      set => this._formatter = value != null ? value : throw Error.ArgumentNull(nameof (value));
    }

    /// <summary> The media type that is associated with the formatter chosen for serialization. Can be null. </summary>
    public MediaTypeHeaderValue MediaType { get; set; }
  }
}
