// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.Parsers.FormUrlEncodedParser
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.Generic;
using System.Text;
using System.Web.Http;

namespace System.Net.Http.Formatting.Parsers
{
  internal class FormUrlEncodedParser
  {
    private const int MinMessageSize = 1;
    private long _totalBytesConsumed;
    private long _maxMessageSize;
    private FormUrlEncodedParser.NameValueState _nameValueState;
    private ICollection<KeyValuePair<string, string>> _nameValuePairs;
    private readonly FormUrlEncodedParser.CurrentNameValuePair _currentNameValuePair;

    public FormUrlEncodedParser(
      ICollection<KeyValuePair<string, string>> nameValuePairs,
      long maxMessageSize)
    {
      if (maxMessageSize < 1L)
        throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (maxMessageSize), (object) maxMessageSize, (object) 1);
      this._nameValuePairs = nameValuePairs != null ? nameValuePairs : throw Error.ArgumentNull(nameof (nameValuePairs));
      this._maxMessageSize = maxMessageSize;
      this._currentNameValuePair = new FormUrlEncodedParser.CurrentNameValuePair();
    }

    public ParserState ParseBuffer(
      byte[] buffer,
      int bytesReady,
      ref int bytesConsumed,
      bool isFinal)
    {
      if (buffer == null)
        throw Error.ArgumentNull(nameof (buffer));
      ParserState parseState1 = ParserState.NeedMoreData;
      if (bytesConsumed >= bytesReady)
      {
        if (isFinal)
          parseState1 = this.CopyCurrent(parseState1);
        return parseState1;
      }
      ParserState parseState2;
      try
      {
        parseState2 = FormUrlEncodedParser.ParseNameValuePairs(buffer, bytesReady, ref bytesConsumed, ref this._nameValueState, this._maxMessageSize, ref this._totalBytesConsumed, this._currentNameValuePair, this._nameValuePairs);
        if (isFinal)
          parseState2 = this.CopyCurrent(parseState2);
      }
      catch (Exception ex)
      {
        parseState2 = ParserState.Invalid;
      }
      return parseState2;
    }

    private static ParserState ParseNameValuePairs(
      byte[] buffer,
      int bytesReady,
      ref int bytesConsumed,
      ref FormUrlEncodedParser.NameValueState nameValueState,
      long maximumLength,
      ref long totalBytesConsumed,
      FormUrlEncodedParser.CurrentNameValuePair currentNameValuePair,
      ICollection<KeyValuePair<string, string>> nameValuePairs)
    {
      int num1 = bytesConsumed;
      ParserState parserState = ParserState.DataTooBig;
      long num2 = maximumLength <= 0L ? long.MaxValue : maximumLength - totalBytesConsumed + (long) num1;
      if ((long) bytesReady < num2)
      {
        parserState = ParserState.NeedMoreData;
        num2 = (long) bytesReady;
      }
      switch (nameValueState)
      {
        case FormUrlEncodedParser.NameValueState.Name:
          do
          {
            int index = bytesConsumed;
            while (buffer[bytesConsumed] != (byte) 61 && buffer[bytesConsumed] != (byte) 38)
            {
              if ((long) ++bytesConsumed == num2)
              {
                string str = Encoding.UTF8.GetString(buffer, index, bytesConsumed - index);
                currentNameValuePair.Name.Append(str);
                goto label_19;
              }
            }
            if (bytesConsumed > index)
            {
              string str = Encoding.UTF8.GetString(buffer, index, bytesConsumed - index);
              currentNameValuePair.Name.Append(str);
            }
            if (buffer[bytesConsumed] == (byte) 61)
            {
              nameValueState = FormUrlEncodedParser.NameValueState.Value;
              if ((long) ++bytesConsumed != num2)
                goto case FormUrlEncodedParser.NameValueState.Value;
              else
                break;
            }
            else
              currentNameValuePair.CopyNameOnlyTo(nameValuePairs);
          }
          while ((long) ++bytesConsumed != num2);
          break;
        case FormUrlEncodedParser.NameValueState.Value:
          int index1 = bytesConsumed;
          while (buffer[bytesConsumed] != (byte) 38)
          {
            if ((long) ++bytesConsumed == num2)
            {
              string str = Encoding.UTF8.GetString(buffer, index1, bytesConsumed - index1);
              currentNameValuePair.Value.Append(str);
              goto label_19;
            }
          }
          if (bytesConsumed > index1)
          {
            string str = Encoding.UTF8.GetString(buffer, index1, bytesConsumed - index1);
            currentNameValuePair.Value.Append(str);
          }
          currentNameValuePair.CopyTo(nameValuePairs);
          nameValueState = FormUrlEncodedParser.NameValueState.Name;
          if ((long) ++bytesConsumed != num2)
            goto case FormUrlEncodedParser.NameValueState.Name;
          else
            break;
      }
label_19:
      totalBytesConsumed += (long) (bytesConsumed - num1);
      return parserState;
    }

    private ParserState CopyCurrent(ParserState parseState)
    {
      if (this._nameValueState == FormUrlEncodedParser.NameValueState.Name)
      {
        if (this._totalBytesConsumed > 0L)
          this._currentNameValuePair.CopyNameOnlyTo(this._nameValuePairs);
      }
      else
        this._currentNameValuePair.CopyTo(this._nameValuePairs);
      return parseState != ParserState.NeedMoreData ? parseState : ParserState.Done;
    }

    private enum NameValueState
    {
      Name,
      Value,
    }

    private class CurrentNameValuePair
    {
      private const int DefaultNameAllocation = 128;
      private const int DefaultValueAllocation = 2048;
      private readonly StringBuilder _name = new StringBuilder(128);
      private readonly StringBuilder _value = new StringBuilder(2048);

      public StringBuilder Name => this._name;

      public StringBuilder Value => this._value;

      public void CopyTo(
        ICollection<KeyValuePair<string, string>> nameValuePairs)
      {
        string key = UriQueryUtility.UrlDecode(this._name.ToString());
        string str = UriQueryUtility.UrlDecode(this._value.ToString());
        nameValuePairs.Add(new KeyValuePair<string, string>(key, str));
        this.Clear();
      }

      public void CopyNameOnlyTo(
        ICollection<KeyValuePair<string, string>> nameValuePairs)
      {
        string key = UriQueryUtility.UrlDecode(this._name.ToString());
        string empty = string.Empty;
        nameValuePairs.Add(new KeyValuePair<string, string>(key, empty));
        this.Clear();
      }

      private void Clear()
      {
        this._name.Clear();
        this._value.Clear();
      }
    }
  }
}
