// Decompiled with JetBrains decompiler
// Type: System.Net.Http.ByteRangeStreamContent
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Internal;
using System.Net.Http.Properties;
using System.Threading.Tasks;

namespace System.Net.Http
{
  /// <summary>
  /// <see cref="T:System.Net.Http.HttpContent" /> implementation which provides a byte range view over a stream used to generate HTTP 206 (Partial Content) byte range responses. The <see cref="T:System.Net.Http.ByteRangeStreamContent" /> supports one or more  byte ranges regardless of whether the ranges are consecutive or not. If there is only one range then a  single partial response body containing a Content-Range header is generated. If there are more than one ranges then a multipart/byteranges response is generated where each body part contains a range indicated by the associated Content-Range header field. </summary>
  public class ByteRangeStreamContent : HttpContent
  {
    private const string SupportedRangeUnit = "bytes";
    private const string ByteRangesContentSubtype = "byteranges";
    private const int DefaultBufferSize = 4096;
    private const int MinBufferSize = 1;
    private readonly Stream _content;
    private readonly long _start;
    private readonly HttpContent _byteRangeContent;
    private bool _disposed;

    /// <summary>
    /// <see cref="T:System.Net.Http.HttpContent" /> implementation which provides a byte range view over a stream used to generate HTTP 206 (Partial Content) byte range responses. If none of the requested ranges overlap with the current extend  of the selected resource represented by the content parameter then an  <see cref="T:System.Net.Http.InvalidByteRangeException" /> is thrown indicating the valid Content-Range of the content.  </summary>
    /// <param name="content">The stream over which to generate a byte range view.</param>
    /// <param name="range">The range or ranges, typically obtained from the Range HTTP request header field.</param>
    /// <param name="mediaType">The media type of the content stream.</param>
    public ByteRangeStreamContent(Stream content, RangeHeaderValue range, string mediaType)
      : this(content, range, new MediaTypeHeaderValue(mediaType), 4096)
    {
    }

    /// <summary>
    /// <see cref="T:System.Net.Http.HttpContent" /> implementation which provides a byte range view over a stream used to generate HTTP 206 (Partial Content) byte range responses. If none of the requested ranges overlap with the current extend  of the selected resource represented by the content parameter then an  <see cref="T:System.Net.Http.InvalidByteRangeException" /> is thrown indicating the valid Content-Range of the content.  </summary>
    /// <param name="content">The stream over which to generate a byte range view.</param>
    /// <param name="range">The range or ranges, typically obtained from the Range HTTP request header field.</param>
    /// <param name="mediaType">The media type of the content stream.</param>
    /// <param name="bufferSize">The buffer size used when copying the content stream.</param>
    public ByteRangeStreamContent(
      Stream content,
      RangeHeaderValue range,
      string mediaType,
      int bufferSize)
      : this(content, range, new MediaTypeHeaderValue(mediaType), bufferSize)
    {
    }

    /// <summary>
    /// <see cref="T:System.Net.Http.HttpContent" /> implementation which provides a byte range view over a stream used to generate HTTP 206 (Partial Content) byte range responses. If none of the requested ranges overlap with the current extend  of the selected resource represented by the content parameter then an  <see cref="T:System.Net.Http.InvalidByteRangeException" /> is thrown indicating the valid Content-Range of the content.  </summary>
    /// <param name="content">The stream over which to generate a byte range view.</param>
    /// <param name="range">The range or ranges, typically obtained from the Range HTTP request header field.</param>
    /// <param name="mediaType">The media type of the content stream.</param>
    public ByteRangeStreamContent(
      Stream content,
      RangeHeaderValue range,
      MediaTypeHeaderValue mediaType)
      : this(content, range, mediaType, 4096)
    {
    }

    /// <summary>
    /// <see cref="T:System.Net.Http.HttpContent" /> implementation which provides a byte range view over a stream used to generate HTTP 206 (Partial Content) byte range responses. If none of the requested ranges overlap with the current extend  of the selected resource represented by the content parameter then an  <see cref="T:System.Net.Http.InvalidByteRangeException" /> is thrown indicating the valid Content-Range of the content.  </summary>
    /// <param name="content">The stream over which to generate a byte range view.</param>
    /// <param name="range">The range or ranges, typically obtained from the Range HTTP request header field.</param>
    /// <param name="mediaType">The media type of the content stream.</param>
    /// <param name="bufferSize">The buffer size used when copying the content stream.</param>
    public ByteRangeStreamContent(
      Stream content,
      RangeHeaderValue range,
      MediaTypeHeaderValue mediaType,
      int bufferSize)
    {
      if (content == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (content));
      if (!content.CanSeek)
        throw System.Web.Http.Error.Argument(nameof (content), Resources.ByteRangeStreamNotSeekable, (object) typeof (ByteRangeStreamContent).Name);
      if (range == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (range));
      if (mediaType == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (mediaType));
      if (bufferSize < 1)
        throw System.Web.Http.Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (bufferSize), (object) bufferSize, (object) 1);
      if (!range.Unit.Equals("bytes", StringComparison.OrdinalIgnoreCase))
        throw System.Web.Http.Error.Argument(nameof (range), Resources.ByteRangeStreamContentNotBytesRange, (object) range.Unit, (object) "bytes");
      try
      {
        if (range.Ranges.Count > 1)
        {
          MultipartContent source = new MultipartContent("byteranges");
          this._byteRangeContent = (HttpContent) source;
          foreach (RangeItemHeaderValue range1 in (IEnumerable<RangeItemHeaderValue>) range.Ranges)
          {
            try
            {
              ByteRangeStream byteRangeStream = new ByteRangeStream(content, range1);
              HttpContent content1 = (HttpContent) new StreamContent((Stream) byteRangeStream, bufferSize);
              content1.Headers.ContentType = mediaType;
              content1.Headers.ContentRange = byteRangeStream.ContentRange;
              source.Add(content1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
            }
          }
          if (!source.Any<HttpContent>())
            throw new InvalidByteRangeException(new ContentRangeHeaderValue(content.Length), System.Web.Http.Error.Format(Resources.ByteRangeStreamNoneOverlap, (object) range.ToString()));
        }
        else
        {
          if (range.Ranges.Count != 1)
            throw System.Web.Http.Error.Argument(nameof (range), Resources.ByteRangeStreamContentNoRanges);
          try
          {
            ByteRangeStream byteRangeStream = new ByteRangeStream(content, range.Ranges.First<RangeItemHeaderValue>());
            this._byteRangeContent = (HttpContent) new StreamContent((Stream) byteRangeStream, bufferSize);
            this._byteRangeContent.Headers.ContentType = mediaType;
            this._byteRangeContent.Headers.ContentRange = byteRangeStream.ContentRange;
          }
          catch (ArgumentOutOfRangeException ex)
          {
            throw new InvalidByteRangeException(new ContentRangeHeaderValue(content.Length), System.Web.Http.Error.Format(Resources.ByteRangeStreamNoOverlap, (object) range.ToString()));
          }
        }
        this._byteRangeContent.Headers.CopyTo(this.Headers);
        this._content = content;
        this._start = content.Position;
      }
      catch
      {
        if (this._byteRangeContent != null)
          this._byteRangeContent.Dispose();
        throw;
      }
    }

    /// <summary>Asynchronously serialize and write the byte range to an HTTP content stream.</summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <param name="stream">The target stream.</param>
    /// <param name="context">Information about the transport.</param>
    protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
    {
      this._content.Position = this._start;
      return this._byteRangeContent.CopyToAsync(stream);
    }

    /// <summary>Determines whether a byte array has a valid length in bytes.</summary>
    /// <returns>true if length is a valid length; otherwise, false.</returns>
    /// <param name="length">The length in bytes of the byte array.</param>
    protected override bool TryComputeLength(out long length)
    {
      long? contentLength = this._byteRangeContent.Headers.ContentLength;
      if (contentLength.HasValue)
      {
        length = contentLength.Value;
        return true;
      }
      length = -1L;
      return false;
    }

    /// <summary>Releases the resources used by the current instance of the <see cref="T:System.Net.Http.ByteRangeStreamContent" /> class.</summary>
    /// <param name="disposing">true to release managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && !this._disposed)
      {
        this._byteRangeContent.Dispose();
        this._content.Dispose();
        this._disposed = true;
      }
      base.Dispose(disposing);
    }
  }
}
