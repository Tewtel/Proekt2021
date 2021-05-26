// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpMessageContent
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary> Derived <see cref="T:System.Net.Http.HttpContent" /> class which can encapsulate an <see cref="P:System.Net.Http.HttpMessageContent.HttpResponseMessage" /> or an <see cref="P:System.Net.Http.HttpMessageContent.HttpRequestMessage" /> as an entity with media type "application/http". </summary>
  public class HttpMessageContent : HttpContent
  {
    private const string SP = " ";
    private const string ColonSP = ": ";
    private const string CRLF = "\r\n";
    private const string CommaSeparator = ", ";
    private const int DefaultHeaderAllocation = 2048;
    private const string DefaultMediaType = "application/http";
    private const string MsgTypeParameter = "msgtype";
    private const string DefaultRequestMsgType = "request";
    private const string DefaultResponseMsgType = "response";
    private const string DefaultRequestMediaType = "application/http; msgtype=request";
    private const string DefaultResponseMediaType = "application/http; msgtype=response";
    private static readonly HashSet<string> _singleValueHeaderFields = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "Cookie",
      "Set-Cookie",
      "X-Powered-By"
    };
    private static readonly HashSet<string> _spaceSeparatedValueHeaderFields = new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "User-Agent"
    };
    private bool _contentConsumed;
    private Lazy<Task<Stream>> _streamTask;

    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.HttpMessageContent" /> class encapsulating an <see cref="P:System.Net.Http.HttpMessageContent.HttpRequestMessage" />. </summary>
    /// <param name="httpRequest">The <see cref="P:System.Net.Http.HttpMessageContent.HttpResponseMessage" /> instance to encapsulate.</param>
    public HttpMessageContent(HttpRequestMessage httpRequest)
    {
      this.HttpRequestMessage = httpRequest != null ? httpRequest : throw Error.ArgumentNull(nameof (httpRequest));
      this.Headers.ContentType = new MediaTypeHeaderValue("application/http");
      this.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("msgtype", "request"));
      this.InitializeStreamTask();
    }

    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.HttpMessageContent" /> class encapsulating an <see cref="P:System.Net.Http.HttpMessageContent.HttpResponseMessage" />. </summary>
    /// <param name="httpResponse">The <see cref="P:System.Net.Http.HttpMessageContent.HttpResponseMessage" /> instance to encapsulate.</param>
    public HttpMessageContent(HttpResponseMessage httpResponse)
    {
      this.HttpResponseMessage = httpResponse != null ? httpResponse : throw Error.ArgumentNull(nameof (httpResponse));
      this.Headers.ContentType = new MediaTypeHeaderValue("application/http");
      this.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("msgtype", "response"));
      this.InitializeStreamTask();
    }

    private HttpContent Content => this.HttpRequestMessage == null ? this.HttpResponseMessage.Content : this.HttpRequestMessage.Content;

    /// <summary> Gets the HTTP request message. </summary>
    public HttpRequestMessage HttpRequestMessage { get; private set; }

    /// <summary> Gets the HTTP response message. </summary>
    public HttpResponseMessage HttpResponseMessage { get; private set; }

    private void InitializeStreamTask() => this._streamTask = new Lazy<Task<Stream>>((Func<Task<Stream>>) (() => this.Content != null ? this.Content.ReadAsStreamAsync() : (Task<Stream>) null));

    internal static bool ValidateHttpMessageContent(
      HttpContent content,
      bool isRequest,
      bool throwOnError)
    {
      if (content == null)
        throw Error.ArgumentNull(nameof (content));
      MediaTypeHeaderValue contentType = content.Headers.ContentType;
      if (contentType != null)
      {
        if (!contentType.MediaType.Equals("application/http", StringComparison.OrdinalIgnoreCase))
        {
          if (throwOnError)
            throw Error.Argument(nameof (content), Resources.HttpMessageInvalidMediaType, (object) FormattingUtilities.HttpContentType.Name, isRequest ? (object) "application/http; msgtype=request" : (object) "application/http; msgtype=response");
          return false;
        }
        foreach (NameValueHeaderValue parameter in (IEnumerable<NameValueHeaderValue>) contentType.Parameters)
        {
          if (parameter.Name.Equals("msgtype", StringComparison.OrdinalIgnoreCase))
          {
            if (FormattingUtilities.UnquoteToken(parameter.Value).Equals(isRequest ? "request" : "response", StringComparison.OrdinalIgnoreCase))
              return true;
            if (throwOnError)
              throw Error.Argument(nameof (content), Resources.HttpMessageInvalidMediaType, (object) FormattingUtilities.HttpContentType.Name, isRequest ? (object) "application/http; msgtype=request" : (object) "application/http; msgtype=response");
            return false;
          }
        }
      }
      if (throwOnError)
        throw Error.Argument(nameof (content), Resources.HttpMessageInvalidMediaType, (object) FormattingUtilities.HttpContentType.Name, isRequest ? (object) "application/http; msgtype=request" : (object) "application/http; msgtype=response");
      return false;
    }

    /// <summary> Asynchronously serializes the object's content to the given stream. </summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> instance that is asynchronously serializing the object's content.</returns>
    /// <param name="stream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
    /// <param name="context">The associated <see cref="T:System.Net.TransportContext" />.</param>
    protected override async Task SerializeToStreamAsync(
      Stream stream,
      TransportContext context)
    {
      if (stream == null)
        throw Error.ArgumentNull(nameof (stream));
      byte[] buffer = this.SerializeHeader();
      await stream.WriteAsync(buffer, 0, buffer.Length);
      if (this.Content == null)
        return;
      this.ValidateStreamForReading(await this._streamTask.Value);
      await this.Content.CopyToAsync(stream);
    }

    /// <summary> Computes the length of the stream if possible. </summary>
    /// <returns>true if the length has been computed; otherwise false.</returns>
    /// <param name="length">The computed length of the stream.</param>
    protected override bool TryComputeLength(out long length)
    {
      int num = this._streamTask.Value != null ? 1 : 0;
      length = 0L;
      if (num != 0)
      {
        Stream result;
        if (!this._streamTask.Value.TryGetResult<Stream>(out result) || result == null || !result.CanSeek)
        {
          length = -1L;
          return false;
        }
        length = result.Length;
      }
      byte[] numArray = this.SerializeHeader();
      length += (long) numArray.Length;
      return true;
    }

    /// <summary> Releases unmanaged and - optionally - managed resources </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (this.HttpRequestMessage != null)
        {
          this.HttpRequestMessage.Dispose();
          this.HttpRequestMessage = (HttpRequestMessage) null;
        }
        if (this.HttpResponseMessage != null)
        {
          this.HttpResponseMessage.Dispose();
          this.HttpResponseMessage = (HttpResponseMessage) null;
        }
      }
      base.Dispose(disposing);
    }

    private static void SerializeRequestLine(StringBuilder message, HttpRequestMessage httpRequest)
    {
      message.Append(httpRequest.Method.ToString() + " ");
      message.Append(httpRequest.RequestUri.PathAndQuery + " ");
      message.Append("HTTP/" + (httpRequest.Version != (Version) null ? httpRequest.Version.ToString(2) : "1.1") + "\r\n");
      if (httpRequest.Headers.Host != null)
        return;
      message.Append("Host: " + httpRequest.RequestUri.Authority + "\r\n");
    }

    private static void SerializeStatusLine(StringBuilder message, HttpResponseMessage httpResponse)
    {
      message.Append("HTTP/" + (httpResponse.Version != (Version) null ? httpResponse.Version.ToString(2) : "1.1") + " ");
      message.Append(((int) httpResponse.StatusCode).ToString() + " ");
      message.Append(httpResponse.ReasonPhrase + "\r\n");
    }

    private static void SerializeHeaderFields(StringBuilder message, HttpHeaders headers)
    {
      if (headers == null)
        return;
      foreach (KeyValuePair<string, IEnumerable<string>> header in headers)
      {
        if (HttpMessageContent._singleValueHeaderFields.Contains(header.Key))
        {
          foreach (string str in header.Value)
            message.Append(header.Key + ": " + str + "\r\n");
        }
        else if (HttpMessageContent._spaceSeparatedValueHeaderFields.Contains(header.Key))
          message.Append(header.Key + ": " + string.Join(" ", header.Value) + "\r\n");
        else
          message.Append(header.Key + ": " + string.Join(", ", header.Value) + "\r\n");
      }
    }

    private byte[] SerializeHeader()
    {
      StringBuilder message = new StringBuilder(2048);
      HttpHeaders headers;
      HttpContent content;
      if (this.HttpRequestMessage != null)
      {
        HttpMessageContent.SerializeRequestLine(message, this.HttpRequestMessage);
        headers = (HttpHeaders) this.HttpRequestMessage.Headers;
        content = this.HttpRequestMessage.Content;
      }
      else
      {
        HttpMessageContent.SerializeStatusLine(message, this.HttpResponseMessage);
        headers = (HttpHeaders) this.HttpResponseMessage.Headers;
        content = this.HttpResponseMessage.Content;
      }
      HttpMessageContent.SerializeHeaderFields(message, headers);
      if (content != null)
        HttpMessageContent.SerializeHeaderFields(message, (HttpHeaders) content.Headers);
      message.Append("\r\n");
      return Encoding.UTF8.GetBytes(message.ToString());
    }

    private void ValidateStreamForReading(Stream stream)
    {
      if (this._contentConsumed)
      {
        if (stream != null && stream.CanRead)
          stream.Position = 0L;
        else
          throw Error.InvalidOperation(Resources.HttpMessageContentAlreadyRead, (object) FormattingUtilities.HttpContentType.Name, this.HttpRequestMessage != null ? (object) FormattingUtilities.HttpRequestMessageType.Name : (object) FormattingUtilities.HttpResponseMessageType.Name);
      }
      this._contentConsumed = true;
    }
  }
}
