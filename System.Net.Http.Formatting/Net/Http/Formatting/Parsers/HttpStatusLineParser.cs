// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.Parsers.HttpStatusLineParser
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Globalization;
using System.Net.Http.Properties;
using System.Text;
using System.Web.Http;

namespace System.Net.Http.Formatting.Parsers
{
  internal class HttpStatusLineParser
  {
    internal const int MinStatusLineSize = 15;
    private const int DefaultTokenAllocation = 2048;
    private const int MaxStatusCode = 1000;
    private int _totalBytesConsumed;
    private int _maximumHeaderLength;
    private HttpStatusLineParser.HttpStatusLineState _statusLineState;
    private HttpUnsortedResponse _httpResponse;
    private StringBuilder _currentToken = new StringBuilder(2048);

    public HttpStatusLineParser(HttpUnsortedResponse httpResponse, int maxStatusLineSize)
    {
      if (maxStatusLineSize < 15)
        throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (maxStatusLineSize), (object) maxStatusLineSize, (object) 15);
      this._httpResponse = httpResponse != null ? httpResponse : throw Error.ArgumentNull(nameof (httpResponse));
      this._maximumHeaderLength = maxStatusLineSize;
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
        return HttpStatusLineParser.ParseStatusLine(buffer, bytesReady, ref bytesConsumed, ref this._statusLineState, this._maximumHeaderLength, ref this._totalBytesConsumed, this._currentToken, this._httpResponse);
      }
      catch (Exception ex)
      {
        return ParserState.Invalid;
      }
    }

    private static ParserState ParseStatusLine(
      byte[] buffer,
      int bytesReady,
      ref int bytesConsumed,
      ref HttpStatusLineParser.HttpStatusLineState statusLineState,
      int maximumHeaderLength,
      ref int totalBytesConsumed,
      StringBuilder currentToken,
      HttpUnsortedResponse httpResponse)
    {
      int num1 = bytesConsumed;
      ParserState parserState = ParserState.DataTooBig;
      int num2 = maximumHeaderLength <= 0 ? int.MaxValue : maximumHeaderLength - totalBytesConsumed + bytesConsumed;
      if (bytesReady < num2)
      {
        parserState = ParserState.NeedMoreData;
        num2 = bytesReady;
      }
      switch (statusLineState)
      {
        case HttpStatusLineParser.HttpStatusLineState.BeforeVersionNumbers:
          int index1 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 47)
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
          string strB = currentToken.ToString();
          if (string.CompareOrdinal("HTTP", strB) != 0)
            throw new FormatException(Error.Format(Resources.HttpInvalidVersion, (object) strB, (object) "HTTP"));
          currentToken.Clear();
          statusLineState = HttpStatusLineParser.HttpStatusLineState.MajorVersionNumber;
          if (++bytesConsumed == num2)
            break;
          goto case HttpStatusLineParser.HttpStatusLineState.MajorVersionNumber;
        case HttpStatusLineParser.HttpStatusLineState.MajorVersionNumber:
          int index2 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 46)
          {
            if (buffer[bytesConsumed] < (byte) 48 || buffer[bytesConsumed] > (byte) 57)
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
          currentToken.Append('.');
          statusLineState = HttpStatusLineParser.HttpStatusLineState.MinorVersionNumber;
          if (++bytesConsumed == num2)
            break;
          goto case HttpStatusLineParser.HttpStatusLineState.MinorVersionNumber;
        case HttpStatusLineParser.HttpStatusLineState.MinorVersionNumber:
          int index3 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 32)
          {
            if (buffer[bytesConsumed] < (byte) 48 || buffer[bytesConsumed] > (byte) 57)
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
          httpResponse.Version = Version.Parse(currentToken.ToString());
          currentToken.Clear();
          statusLineState = HttpStatusLineParser.HttpStatusLineState.StatusCode;
          if (++bytesConsumed == num2)
            break;
          goto case HttpStatusLineParser.HttpStatusLineState.StatusCode;
        case HttpStatusLineParser.HttpStatusLineState.StatusCode:
          int index4 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 32)
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
          int num3 = int.Parse(currentToken.ToString(), (IFormatProvider) CultureInfo.InvariantCulture);
          httpResponse.StatusCode = num3 >= 100 && num3 <= 1000 ? (HttpStatusCode) num3 : throw new FormatException(Error.Format(Resources.HttpInvalidStatusCode, (object) num3, (object) 100, (object) 1000));
          currentToken.Clear();
          statusLineState = HttpStatusLineParser.HttpStatusLineState.ReasonPhrase;
          if (++bytesConsumed == num2)
            break;
          goto case HttpStatusLineParser.HttpStatusLineState.ReasonPhrase;
        case HttpStatusLineParser.HttpStatusLineState.ReasonPhrase:
          int index5 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 13)
          {
            if (buffer[bytesConsumed] < (byte) 32 || buffer[bytesConsumed] > (byte) 122)
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
          httpResponse.ReasonPhrase = currentToken.ToString();
          currentToken.Clear();
          statusLineState = HttpStatusLineParser.HttpStatusLineState.AfterCarriageReturn;
          if (++bytesConsumed == num2)
            break;
          goto case HttpStatusLineParser.HttpStatusLineState.AfterCarriageReturn;
        case HttpStatusLineParser.HttpStatusLineState.AfterCarriageReturn:
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

    private enum HttpStatusLineState
    {
      BeforeVersionNumbers,
      MajorVersionNumber,
      MinorVersionNumber,
      StatusCode,
      ReasonPhrase,
      AfterCarriageReturn,
    }
  }
}
