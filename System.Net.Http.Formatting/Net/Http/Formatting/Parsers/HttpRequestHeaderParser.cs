// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.Parsers.HttpRequestHeaderParser
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Web.Http;

namespace System.Net.Http.Formatting.Parsers
{
  internal class HttpRequestHeaderParser
  {
    internal const int DefaultMaxRequestLineSize = 2048;
    internal const int DefaultMaxHeaderSize = 16384;
    private HttpUnsortedRequest _httpRequest;
    private HttpRequestHeaderParser.HttpRequestState _requestStatus;
    private HttpRequestLineParser _requestLineParser;
    private InternetMessageFormatHeaderParser _headerParser;

    public HttpRequestHeaderParser(HttpUnsortedRequest httpRequest)
      : this(httpRequest, 2048, 16384)
    {
    }

    public HttpRequestHeaderParser(
      HttpUnsortedRequest httpRequest,
      int maxRequestLineSize,
      int maxHeaderSize)
    {
      this._httpRequest = httpRequest != null ? httpRequest : throw Error.ArgumentNull(nameof (httpRequest));
      this._requestLineParser = new HttpRequestLineParser(this._httpRequest, maxRequestLineSize);
      this._headerParser = new InternetMessageFormatHeaderParser(this._httpRequest.HttpHeaders, maxHeaderSize);
    }

    public ParserState ParseBuffer(byte[] buffer, int bytesReady, ref int bytesConsumed)
    {
      if (buffer == null)
        throw Error.ArgumentNull(nameof (buffer));
      ParserState parserState1 = ParserState.NeedMoreData;
      ParserState parserState2 = ParserState.NeedMoreData;
      switch (this._requestStatus)
      {
        case HttpRequestHeaderParser.HttpRequestState.RequestLine:
          ParserState parserState3;
          try
          {
            parserState3 = this._requestLineParser.ParseBuffer(buffer, bytesReady, ref bytesConsumed);
          }
          catch (Exception ex)
          {
            parserState3 = ParserState.Invalid;
          }
          switch (parserState3)
          {
            case ParserState.NeedMoreData:
              break;
            case ParserState.Done:
              this._requestStatus = HttpRequestHeaderParser.HttpRequestState.RequestHeaders;
              parserState2 = ParserState.NeedMoreData;
              goto label_8;
            default:
              parserState1 = parserState3;
              break;
          }
          break;
        case HttpRequestHeaderParser.HttpRequestState.RequestHeaders:
label_8:
          if (bytesConsumed < bytesReady)
          {
            ParserState parserState4;
            try
            {
              parserState4 = this._headerParser.ParseBuffer(buffer, bytesReady, ref bytesConsumed);
            }
            catch (Exception ex)
            {
              parserState4 = ParserState.Invalid;
            }
            switch (parserState4)
            {
              case ParserState.NeedMoreData:
                break;
              case ParserState.Done:
                parserState1 = parserState4;
                break;
              default:
                parserState1 = parserState4;
                break;
            }
          }
          else
            break;
          break;
      }
      return parserState1;
    }

    private enum HttpRequestState
    {
      RequestLine,
      RequestHeaders,
    }
  }
}
