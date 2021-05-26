// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.Parsers.HttpResponseHeaderParser
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Web.Http;

namespace System.Net.Http.Formatting.Parsers
{
  internal class HttpResponseHeaderParser
  {
    internal const int DefaultMaxStatusLineSize = 2048;
    internal const int DefaultMaxHeaderSize = 16384;
    private HttpUnsortedResponse _httpResponse;
    private HttpResponseHeaderParser.HttpResponseState _responseStatus;
    private HttpStatusLineParser _statusLineParser;
    private InternetMessageFormatHeaderParser _headerParser;

    public HttpResponseHeaderParser(HttpUnsortedResponse httpResponse)
      : this(httpResponse, 2048, 16384)
    {
    }

    public HttpResponseHeaderParser(
      HttpUnsortedResponse httpResponse,
      int maxResponseLineSize,
      int maxHeaderSize)
    {
      this._httpResponse = httpResponse != null ? httpResponse : throw Error.ArgumentNull(nameof (httpResponse));
      this._statusLineParser = new HttpStatusLineParser(this._httpResponse, maxResponseLineSize);
      this._headerParser = new InternetMessageFormatHeaderParser(this._httpResponse.HttpHeaders, maxHeaderSize);
    }

    public ParserState ParseBuffer(byte[] buffer, int bytesReady, ref int bytesConsumed)
    {
      if (buffer == null)
        throw Error.ArgumentNull(nameof (buffer));
      ParserState parserState1 = ParserState.NeedMoreData;
      ParserState parserState2 = ParserState.NeedMoreData;
      switch (this._responseStatus)
      {
        case HttpResponseHeaderParser.HttpResponseState.StatusLine:
          ParserState parserState3;
          try
          {
            parserState3 = this._statusLineParser.ParseBuffer(buffer, bytesReady, ref bytesConsumed);
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
              this._responseStatus = HttpResponseHeaderParser.HttpResponseState.ResponseHeaders;
              parserState2 = ParserState.NeedMoreData;
              goto label_8;
            default:
              parserState1 = parserState3;
              break;
          }
          break;
        case HttpResponseHeaderParser.HttpResponseState.ResponseHeaders:
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

    private enum HttpResponseState
    {
      StatusLine,
      ResponseHeaders,
    }
  }
}
