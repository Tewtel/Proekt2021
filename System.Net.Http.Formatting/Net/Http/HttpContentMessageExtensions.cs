// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpContentMessageExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting.Parsers;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
  /// <summary>Provides extension methods to read <see cref="T:System.Net.Http.HttpRequestMessage" /> and <see cref="T:System.Net.Http.HttpResponseMessage" /> entities from <see cref="T:System.Net.Http.HttpContent" /> instances. </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class HttpContentMessageExtensions
  {
    private const int MinBufferSize = 256;
    private const int DefaultBufferSize = 32768;

    /// <summary>Determines whether the specified content is HTTP request message content.</summary>
    /// <returns>true if the specified content is HTTP message content; otherwise, false.</returns>
    /// <param name="content">The content to check.</param>
    public static bool IsHttpRequestMessageContent(this HttpContent content)
    {
      if (content == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (content));
      try
      {
        return HttpMessageContent.ValidateHttpMessageContent(content, true, false);
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    /// <summary>Determines whether the specified content is HTTP response message content.</summary>
    /// <returns>true if the specified content is HTTP message content; otherwise, false.</returns>
    /// <param name="content">The content to check.</param>
    public static bool IsHttpResponseMessageContent(this HttpContent content)
    {
      if (content == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (content));
      try
      {
        return HttpMessageContent.ValidateHttpMessageContent(content, false, false);
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    /// <summary> Reads the <see cref="T:System.Net.Http.HttpContent" /> as an <see cref="T:System.Net.Http.HttpRequestMessage" />. </summary>
    /// <returns>The parsed <see cref="T:System.Net.Http.HttpRequestMessage" /> instance.</returns>
    /// <param name="content">The content to read.</param>
    public static Task<HttpRequestMessage> ReadAsHttpRequestMessageAsync(
      this HttpContent content)
    {
      return content.ReadAsHttpRequestMessageAsync("http", 32768);
    }

    public static Task<HttpRequestMessage> ReadAsHttpRequestMessageAsync(
      this HttpContent content,
      CancellationToken cancellationToken)
    {
      return content.ReadAsHttpRequestMessageAsync("http", 32768, cancellationToken);
    }

    /// <summary> Reads the <see cref="T:System.Net.Http.HttpContent" /> as an <see cref="T:System.Net.Http.HttpRequestMessage" />. </summary>
    /// <returns>The parsed <see cref="T:System.Net.Http.HttpRequestMessage" /> instance.</returns>
    /// <param name="content">The content to read.</param>
    /// <param name="uriScheme">The URI scheme to use for the request URI.</param>
    public static Task<HttpRequestMessage> ReadAsHttpRequestMessageAsync(
      this HttpContent content,
      string uriScheme)
    {
      return content.ReadAsHttpRequestMessageAsync(uriScheme, 32768);
    }

    public static Task<HttpRequestMessage> ReadAsHttpRequestMessageAsync(
      this HttpContent content,
      string uriScheme,
      CancellationToken cancellationToken)
    {
      return content.ReadAsHttpRequestMessageAsync(uriScheme, 32768, cancellationToken);
    }

    /// <summary> Reads the <see cref="T:System.Net.Http.HttpContent" /> as an <see cref="T:System.Net.Http.HttpRequestMessage" />. </summary>
    /// <returns>The parsed <see cref="T:System.Net.Http.HttpRequestMessage" /> instance.</returns>
    /// <param name="content">The content to read.</param>
    /// <param name="uriScheme">The URI scheme to use for the request URI.</param>
    /// <param name="bufferSize">The size of the buffer.</param>
    public static Task<HttpRequestMessage> ReadAsHttpRequestMessageAsync(
      this HttpContent content,
      string uriScheme,
      int bufferSize)
    {
      return content.ReadAsHttpRequestMessageAsync(uriScheme, bufferSize, 16384);
    }

    public static Task<HttpRequestMessage> ReadAsHttpRequestMessageAsync(
      this HttpContent content,
      string uriScheme,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      return content.ReadAsHttpRequestMessageAsync(uriScheme, bufferSize, 16384, cancellationToken);
    }

    /// <summary>Reads the <see cref="T:System.Net.Http.HttpContent" /> as an <see cref="T:System.Net.Http.HttpRequestMessage" />.</summary>
    /// <returns>The parsed <see cref="T:System.Net.Http.HttpRequestMessage" /> instance.</returns>
    /// <param name="content">The content to read.</param>
    /// <param name="uriScheme">The URI scheme to use for the request URI.</param>
    /// <param name="bufferSize">The size of the buffer.</param>
    /// <param name="maxHeaderSize">The maximum length of the HTTP header.</param>
    public static Task<HttpRequestMessage> ReadAsHttpRequestMessageAsync(
      this HttpContent content,
      string uriScheme,
      int bufferSize,
      int maxHeaderSize)
    {
      return content.ReadAsHttpRequestMessageAsync(uriScheme, bufferSize, maxHeaderSize, CancellationToken.None);
    }

    public static Task<HttpRequestMessage> ReadAsHttpRequestMessageAsync(
      this HttpContent content,
      string uriScheme,
      int bufferSize,
      int maxHeaderSize,
      CancellationToken cancellationToken)
    {
      if (content == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (content));
      if (uriScheme == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (uriScheme));
      if (!Uri.CheckSchemeName(uriScheme))
        throw System.Web.Http.Error.Argument(nameof (uriScheme), Resources.HttpMessageParserInvalidUriScheme, (object) uriScheme, (object) typeof (Uri).Name);
      if (bufferSize < 256)
        throw System.Web.Http.Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (bufferSize), (object) bufferSize, (object) 256);
      if (maxHeaderSize < 2)
        throw System.Web.Http.Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (maxHeaderSize), (object) maxHeaderSize, (object) 2);
      HttpMessageContent.ValidateHttpMessageContent(content, true, true);
      return content.ReadAsHttpRequestMessageAsyncCore(uriScheme, bufferSize, maxHeaderSize, cancellationToken);
    }

    private static async Task<HttpRequestMessage> ReadAsHttpRequestMessageAsyncCore(
      this HttpContent content,
      string uriScheme,
      int bufferSize,
      int maxHeaderSize,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      Stream stream = await content.ReadAsStreamAsync();
      HttpUnsortedRequest httpRequest = new HttpUnsortedRequest();
      HttpRequestHeaderParser parser = new HttpRequestHeaderParser(httpRequest, 2048, maxHeaderSize);
      byte[] buffer = new byte[bufferSize];
      int headerConsumed = 0;
      int bytesReady;
      do
      {
        try
        {
          bytesReady = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
        }
        catch (Exception ex)
        {
          throw new IOException(Resources.HttpMessageErrorReading, ex);
        }
        ParserState parserState;
        try
        {
          parserState = parser.ParseBuffer(buffer, bytesReady, ref headerConsumed);
        }
        catch (Exception ex)
        {
          parserState = ParserState.Invalid;
        }
        switch (parserState)
        {
          case ParserState.NeedMoreData:
            continue;
          case ParserState.Done:
            return HttpContentMessageExtensions.CreateHttpRequestMessage(uriScheme, httpRequest, stream, bytesReady - headerConsumed);
          default:
            throw System.Web.Http.Error.InvalidOperation(Resources.HttpMessageParserError, (object) headerConsumed, (object) buffer);
        }
      }
      while (bytesReady != 0);
      throw new IOException(Resources.ReadAsHttpMessageUnexpectedTermination);
    }

    /// <summary> Reads the <see cref="T:System.Net.Http.HttpContent" /> as an <see cref="T:System.Net.Http.HttpResponseMessage" />. </summary>
    /// <returns>The parsed <see cref="T:System.Net.Http.HttpResponseMessage" /> instance.</returns>
    /// <param name="content">The content to read.</param>
    public static Task<HttpResponseMessage> ReadAsHttpResponseMessageAsync(
      this HttpContent content)
    {
      return content.ReadAsHttpResponseMessageAsync(32768);
    }

    public static Task<HttpResponseMessage> ReadAsHttpResponseMessageAsync(
      this HttpContent content,
      CancellationToken cancellationToken)
    {
      return content.ReadAsHttpResponseMessageAsync(32768, cancellationToken);
    }

    /// <summary>Reads the <see cref="T:System.Net.Http.HttpContent" /> as an <see cref="T:System.Net.Http.HttpResponseMessage" />. </summary>
    /// <returns>The parsed <see cref="T:System.Net.Http.HttpResponseMessage" /> instance.</returns>
    /// <param name="content">The content to read.</param>
    /// <param name="bufferSize">The size of the buffer.</param>
    public static Task<HttpResponseMessage> ReadAsHttpResponseMessageAsync(
      this HttpContent content,
      int bufferSize)
    {
      return content.ReadAsHttpResponseMessageAsync(bufferSize, 16384);
    }

    public static Task<HttpResponseMessage> ReadAsHttpResponseMessageAsync(
      this HttpContent content,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      return content.ReadAsHttpResponseMessageAsync(bufferSize, 16384, cancellationToken);
    }

    /// <summary>Reads the <see cref="T:System.Net.Http.HttpContent" /> as an <see cref="T:System.Net.Http.HttpResponseMessage" />.</summary>
    /// <returns>The parsed <see cref="T:System.Net.Http.HttpResponseMessage" /> instance.</returns>
    /// <param name="content">The content to read.</param>
    /// <param name="bufferSize">The size of the buffer.</param>
    /// <param name="maxHeaderSize">The maximum length of the HTTP header.</param>
    public static Task<HttpResponseMessage> ReadAsHttpResponseMessageAsync(
      this HttpContent content,
      int bufferSize,
      int maxHeaderSize)
    {
      return content.ReadAsHttpResponseMessageAsync(bufferSize, maxHeaderSize, CancellationToken.None);
    }

    public static Task<HttpResponseMessage> ReadAsHttpResponseMessageAsync(
      this HttpContent content,
      int bufferSize,
      int maxHeaderSize,
      CancellationToken cancellationToken)
    {
      if (content == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (content));
      if (bufferSize < 256)
        throw System.Web.Http.Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (bufferSize), (object) bufferSize, (object) 256);
      if (maxHeaderSize < 2)
        throw System.Web.Http.Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (maxHeaderSize), (object) maxHeaderSize, (object) 2);
      HttpMessageContent.ValidateHttpMessageContent(content, false, true);
      return content.ReadAsHttpResponseMessageAsyncCore(bufferSize, maxHeaderSize, cancellationToken);
    }

    private static async Task<HttpResponseMessage> ReadAsHttpResponseMessageAsyncCore(
      this HttpContent content,
      int bufferSize,
      int maxHeaderSize,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      Stream stream = await content.ReadAsStreamAsync();
      HttpUnsortedResponse httpResponse = new HttpUnsortedResponse();
      HttpResponseHeaderParser parser = new HttpResponseHeaderParser(httpResponse, 2048, maxHeaderSize);
      byte[] buffer = new byte[bufferSize];
      int headerConsumed = 0;
      int bytesReady;
      do
      {
        try
        {
          bytesReady = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
        }
        catch (Exception ex)
        {
          throw new IOException(Resources.HttpMessageErrorReading, ex);
        }
        ParserState parserState;
        try
        {
          parserState = parser.ParseBuffer(buffer, bytesReady, ref headerConsumed);
        }
        catch (Exception ex)
        {
          parserState = ParserState.Invalid;
        }
        switch (parserState)
        {
          case ParserState.NeedMoreData:
            continue;
          case ParserState.Done:
            return HttpContentMessageExtensions.CreateHttpResponseMessage(httpResponse, stream, bytesReady - headerConsumed);
          default:
            throw System.Web.Http.Error.InvalidOperation(Resources.HttpMessageParserError, (object) headerConsumed, (object) buffer);
        }
      }
      while (bytesReady != 0);
      throw new IOException(Resources.ReadAsHttpMessageUnexpectedTermination);
    }

    private static Uri CreateRequestUri(string uriScheme, HttpUnsortedRequest httpRequest)
    {
      IEnumerable<string> values;
      if (httpRequest.HttpHeaders.TryGetValues("Host", out values))
      {
        int num = values.Count<string>();
        if (num != 1)
          throw System.Web.Http.Error.InvalidOperation(Resources.HttpMessageParserInvalidHostCount, (object) "Host", (object) num);
        return new Uri(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}://{1}{2}", (object) uriScheme, (object) values.ElementAt<string>(0), (object) httpRequest.RequestUri));
      }
      throw System.Web.Http.Error.InvalidOperation(Resources.HttpMessageParserInvalidHostCount, (object) "Host", (object) 0);
    }

    private static HttpContent CreateHeaderFields(
      HttpHeaders source,
      HttpHeaders destination,
      Stream contentStream,
      int rewind)
    {
      HttpContentHeaders fromHeaders = (HttpContentHeaders) null;
      HttpContent httpContent = (HttpContent) null;
      foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in source)
      {
        if (!destination.TryAddWithoutValidation(keyValuePair.Key, keyValuePair.Value))
        {
          if (fromHeaders == null)
            fromHeaders = FormattingUtilities.CreateEmptyContentHeaders();
          fromHeaders.TryAddWithoutValidation(keyValuePair.Key, keyValuePair.Value);
        }
      }
      if (fromHeaders != null)
      {
        if (!contentStream.CanSeek)
          throw System.Web.Http.Error.InvalidOperation(Resources.HttpMessageContentStreamMustBeSeekable, (object) "ContentReadStream", (object) FormattingUtilities.HttpResponseMessageType.Name);
        contentStream.Seek((long) -rewind, SeekOrigin.Current);
        httpContent = (HttpContent) new StreamContent(contentStream);
        fromHeaders.CopyTo(httpContent.Headers);
      }
      return httpContent;
    }

    private static HttpRequestMessage CreateHttpRequestMessage(
      string uriScheme,
      HttpUnsortedRequest httpRequest,
      Stream contentStream,
      int rewind)
    {
      HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
      {
        Method = httpRequest.Method,
        RequestUri = HttpContentMessageExtensions.CreateRequestUri(uriScheme, httpRequest),
        Version = httpRequest.Version
      };
      httpRequestMessage.Content = HttpContentMessageExtensions.CreateHeaderFields(httpRequest.HttpHeaders, (HttpHeaders) httpRequestMessage.Headers, contentStream, rewind);
      return httpRequestMessage;
    }

    private static HttpResponseMessage CreateHttpResponseMessage(
      HttpUnsortedResponse httpResponse,
      Stream contentStream,
      int rewind)
    {
      HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
      {
        Version = httpResponse.Version,
        StatusCode = httpResponse.StatusCode,
        ReasonPhrase = httpResponse.ReasonPhrase
      };
      httpResponseMessage.Content = HttpContentMessageExtensions.CreateHeaderFields(httpResponse.HttpHeaders, (HttpHeaders) httpResponseMessage.Headers, contentStream, rewind);
      return httpResponseMessage;
    }
  }
}
