// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.Parsers.HttpRequestLineParser
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Properties;
using System.Text;
using System.Web.Http;

namespace System.Net.Http.Formatting.Parsers
{
  internal class HttpRequestLineParser
  {
    internal const int MinRequestLineSize = 14;
    private const int DefaultTokenAllocation = 2048;
    private int _totalBytesConsumed;
    private int _maximumHeaderLength;
    private HttpRequestLineParser.HttpRequestLineState _requestLineState;
    private HttpUnsortedRequest _httpRequest;
    private StringBuilder _currentToken = new StringBuilder(2048);

    public HttpRequestLineParser(HttpUnsortedRequest httpRequest, int maxRequestLineSize)
    {
      if (maxRequestLineSize < 14)
        throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (maxRequestLineSize), (object) maxRequestLineSize, (object) 14);
      this._httpRequest = httpRequest != null ? httpRequest : throw Error.ArgumentNull(nameof (httpRequest));
      this._maximumHeaderLength = maxRequestLineSize;
    }

    public ParserState ParseBuffer(byte[] buffer, int bytesReady, ref int bytesConsumed)
    {
      if (buffer == null)
        throw Error.ArgumentNull(nameof (buffer));
      ParserState parserState = ParserState.NeedMoreData;
      if (bytesConsumed >= bytesReady)
        return parserState;
      try
      {
        return HttpRequestLineParser.ParseRequestLine(buffer, bytesReady, ref bytesConsumed, ref this._requestLineState, this._maximumHeaderLength, ref this._totalBytesConsumed, this._currentToken, this._httpRequest);
      }
      catch (Exception ex)
      {
        return ParserState.Invalid;
      }
    }

    private static ParserState ParseRequestLine(
      byte[] buffer,
      int bytesReady,
      ref int bytesConsumed,
      ref HttpRequestLineParser.HttpRequestLineState requestLineState,
      int maximumHeaderLength,
      ref int totalBytesConsumed,
      StringBuilder currentToken,
      HttpUnsortedRequest httpRequest)
    {
      int num1 = bytesConsumed;
      ParserState parserState = ParserState.DataTooBig;
      int num2 = maximumHeaderLength <= 0 ? int.MaxValue : maximumHeaderLength - totalBytesConsumed + bytesConsumed;
      if (bytesReady < num2)
      {
        parserState = ParserState.NeedMoreData;
        num2 = bytesReady;
      }
      switch (requestLineState)
      {
        case HttpRequestLineParser.HttpRequestLineState.RequestMethod:
          int index1 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 32)
          {
            if (buffer[bytesConsumed] < (byte) 33 || buffer[bytesConsumed] > (byte) 122)
            {
              parserState = ParserState.Invalid;
              goto label_55;
            }
            else if (++bytesConsumed == num2)
            {
              string str = Encoding.UTF8.GetString(buffer, index1, bytesConsumed - index1);
              currentToken.Append(str);
              goto label_55;
            }
          }
          if (bytesConsumed > index1)
          {
            string str = Encoding.UTF8.GetString(buffer, index1, bytesConsumed - index1);
            currentToken.Append(str);
          }
          httpRequest.Method = new HttpMethod(currentToken.ToString());
          currentToken.Clear();
          requestLineState = HttpRequestLineParser.HttpRequestLineState.RequestUri;
          if (++bytesConsumed == num2)
            break;
          goto case HttpRequestLineParser.HttpRequestLineState.RequestUri;
        case HttpRequestLineParser.HttpRequestLineState.RequestUri:
          int index2 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 32)
          {
            if (buffer[bytesConsumed] == (byte) 13)
            {
              parserState = ParserState.Invalid;
              goto label_55;
            }
            else if (++bytesConsumed == num2)
            {
              string str = Encoding.UTF8.GetString(buffer, index2, bytesConsumed - index2);
              currentToken.Append(str);
              goto label_55;
            }
          }
          if (bytesConsumed > index2)
          {
            string str = Encoding.UTF8.GetString(buffer, index2, bytesConsumed - index2);
            currentToken.Append(str);
          }
          httpRequest.RequestUri = currentToken.Length != 0 ? currentToken.ToString() : throw new FormatException(Resources.HttpMessageParserEmptyUri);
          currentToken.Clear();
          requestLineState = HttpRequestLineParser.HttpRequestLineState.BeforeVersionNumbers;
          if (++bytesConsumed == num2)
            break;
          goto case HttpRequestLineParser.HttpRequestLineState.BeforeVersionNumbers;
        case HttpRequestLineParser.HttpRequestLineState.BeforeVersionNumbers:
          int index3 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 47)
          {
            if (buffer[bytesConsumed] < (byte) 33 || buffer[bytesConsumed] > (byte) 122)
            {
              parserState = ParserState.Invalid;
              goto label_55;
            }
            else if (++bytesConsumed == num2)
            {
              string str = Encoding.UTF8.GetString(buffer, index3, bytesConsumed - index3);
              currentToken.Append(str);
              goto label_55;
            }
          }
          if (bytesConsumed > index3)
          {
            string str = Encoding.UTF8.GetString(buffer, index3, bytesConsumed - index3);
            currentToken.Append(str);
          }
          string strB = currentToken.ToString();
          if (string.CompareOrdinal("HTTP", strB) != 0)
            throw new FormatException(Error.Format(Resources.HttpInvalidVersion, (object) strB, (object) "HTTP"));
          currentToken.Clear();
          requestLineState = HttpRequestLineParser.HttpRequestLineState.MajorVersionNumber;
          if (++bytesConsumed == num2)
            break;
          goto case HttpRequestLineParser.HttpRequestLineState.MajorVersionNumber;
        case HttpRequestLineParser.HttpRequestLineState.MajorVersionNumber:
          int index4 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 46)
          {
            if (buffer[bytesConsumed] < (byte) 48 || buffer[bytesConsumed] > (byte) 57)
            {
              parserState = ParserState.Invalid;
              goto label_55;
            }
            else if (++bytesConsumed == num2)
            {
              string str = Encoding.UTF8.GetString(buffer, index4, bytesConsumed - index4);
              currentToken.Append(str);
              goto label_55;
            }
          }
          if (bytesConsumed > index4)
          {
            string str = Encoding.UTF8.GetString(buffer, index4, bytesConsumed - index4);
            currentToken.Append(str);
          }
          currentToken.Append('.');
          requestLineState = HttpRequestLineParser.HttpRequestLineState.MinorVersionNumber;
          if (++bytesConsumed == num2)
            break;
          goto case HttpRequestLineParser.HttpRequestLineState.MinorVersionNumber;
        case HttpRequestLineParser.HttpRequestLineState.MinorVersionNumber:
          int index5 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 13)
          {
            if (buffer[bytesConsumed] < (byte) 48 || buffer[bytesConsumed] > (byte) 57)
            {
              parserState = ParserState.Invalid;
              goto label_55;
            }
            else if (++bytesConsumed == num2)
            {
              string str = Encoding.UTF8.GetString(buffer, index5, bytesConsumed - index5);
              currentToken.Append(str);
              goto label_55;
            }
          }
          if (bytesConsumed > index5)
          {
            string str = Encoding.UTF8.GetString(buffer, index5, bytesConsumed - index5);
            currentToken.Append(str);
          }
          httpRequest.Version = Version.Parse(currentToken.ToString());
          currentToken.Clear();
          requestLineState = HttpRequestLineParser.HttpRequestLineState.AfterCarriageReturn;
          if (++bytesConsumed == num2)
            break;
          goto case HttpRequestLineParser.HttpRequestLineState.AfterCarriageReturn;
        case HttpRequestLineParser.HttpRequestLineState.AfterCarriageReturn:
          if (buffer[bytesConsumed] != (byte) 10)
          {
            parserState = ParserState.Invalid;
            break;
          }
          parserState = ParserState.Done;
          ++bytesConsumed;
          break;
      }
label_55:
      totalBytesConsumed += bytesConsumed - num1;
      return parserState;
    }

    private enum HttpRequestLineState
    {
      RequestMethod,
      RequestUri,
      BeforeVersionNumbers,
      MajorVersionNumber,
      MinorVersionNumber,
      AfterCarriageReturn,
    }
  }
}
