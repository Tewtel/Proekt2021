// Decompiled with JetBrains decompiler
// Type: System.Web.Http.UriQueryUtility
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Text;

namespace System.Web.Http
{
  internal static class UriQueryUtility
  {
    public static string UrlEncode(string str)
    {
      if (str == null)
        return (string) null;
      byte[] bytes = Encoding.UTF8.GetBytes(str);
      return Encoding.ASCII.GetString(UriQueryUtility.UrlEncode(bytes, 0, bytes.Length, false));
    }

    public static string UrlDecode(string str) => str == null ? (string) null : UriQueryUtility.UrlDecodeInternal(str, Encoding.UTF8);

    private static byte[] UrlEncode(
      byte[] bytes,
      int offset,
      int count,
      bool alwaysCreateNewReturnValue)
    {
      byte[] numArray = UriQueryUtility.UrlEncode(bytes, offset, count);
      return !alwaysCreateNewReturnValue || numArray == null || numArray != bytes ? numArray : (byte[]) numArray.Clone();
    }

    private static byte[] UrlEncode(byte[] bytes, int offset, int count)
    {
      if (!UriQueryUtility.ValidateUrlEncodingParameters(bytes, offset, count))
        return (byte[]) null;
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < count; ++index)
      {
        char ch = (char) bytes[offset + index];
        if (ch == ' ')
          ++num1;
        else if (!UriQueryUtility.IsUrlSafeChar(ch))
          ++num2;
      }
      if (num1 == 0 && num2 == 0)
        return bytes;
      byte[] numArray1 = new byte[count + num2 * 2];
      int num3 = 0;
      for (int index1 = 0; index1 < count; ++index1)
      {
        byte num4 = bytes[offset + index1];
        char ch = (char) num4;
        if (UriQueryUtility.IsUrlSafeChar(ch))
          numArray1[num3++] = num4;
        else if (ch == ' ')
        {
          numArray1[num3++] = (byte) 43;
        }
        else
        {
          byte[] numArray2 = numArray1;
          int index2 = num3;
          int num5 = index2 + 1;
          numArray2[index2] = (byte) 37;
          byte[] numArray3 = numArray1;
          int index3 = num5;
          int num6 = index3 + 1;
          int hex1 = (int) (byte) UriQueryUtility.IntToHex((int) num4 >> 4 & 15);
          numArray3[index3] = (byte) hex1;
          byte[] numArray4 = numArray1;
          int index4 = num6;
          num3 = index4 + 1;
          int hex2 = (int) (byte) UriQueryUtility.IntToHex((int) num4 & 15);
          numArray4[index4] = (byte) hex2;
        }
      }
      return numArray1;
    }

    private static string UrlDecodeInternal(string value, Encoding encoding)
    {
      if (value == null)
        return (string) null;
      int length = value.Length;
      UriQueryUtility.UrlDecoder urlDecoder = new UriQueryUtility.UrlDecoder(length, encoding);
      for (int index = 0; index < length; ++index)
      {
        char ch = value[index];
        switch (ch)
        {
          case '%':
            if (index < length - 2)
            {
              int num1 = UriQueryUtility.HexToInt(value[index + 1]);
              int num2 = UriQueryUtility.HexToInt(value[index + 2]);
              if (num1 >= 0 && num2 >= 0)
              {
                byte b = (byte) (num1 << 4 | num2);
                index += 2;
                urlDecoder.AddByte(b);
                break;
              }
              goto default;
            }
            else
              goto default;
          case '+':
            ch = ' ';
            goto default;
          default:
            if (((int) ch & 65408) == 0)
            {
              urlDecoder.AddByte((byte) ch);
              break;
            }
            urlDecoder.AddChar(ch);
            break;
        }
      }
      return urlDecoder.GetString();
    }

    private static int HexToInt(char h)
    {
      if (h >= '0' && h <= '9')
        return (int) h - 48;
      if (h >= 'a' && h <= 'f')
        return (int) h - 97 + 10;
      return h < 'A' || h > 'F' ? -1 : (int) h - 65 + 10;
    }

    private static char IntToHex(int n) => n <= 9 ? (char) (n + 48) : (char) (n - 10 + 97);

    private static bool IsUrlSafeChar(char ch)
    {
      if (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || (ch >= '0' && ch <= '9' || ch == '!'))
        return true;
      switch (ch)
      {
        case '(':
        case ')':
        case '*':
        case '-':
        case '.':
        case '_':
          return true;
        default:
          return false;
      }
    }

    private static bool ValidateUrlEncodingParameters(byte[] bytes, int offset, int count)
    {
      if (bytes == null && count == 0)
        return false;
      if (bytes == null)
        throw Error.ArgumentNull(nameof (bytes));
      if (offset < 0 || offset > bytes.Length)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0 || offset + count > bytes.Length)
        throw new ArgumentOutOfRangeException(nameof (count));
      return true;
    }

    private class UrlDecoder
    {
      private int _bufferSize;
      private int _numChars;
      private char[] _charBuffer;
      private int _numBytes;
      private byte[] _byteBuffer;
      private Encoding _encoding;

      private void FlushBytes()
      {
        if (this._numBytes <= 0)
          return;
        this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
        this._numBytes = 0;
      }

      internal UrlDecoder(int bufferSize, Encoding encoding)
      {
        this._bufferSize = bufferSize;
        this._encoding = encoding;
        this._charBuffer = new char[bufferSize];
      }

      internal void AddChar(char ch)
      {
        if (this._numBytes > 0)
          this.FlushBytes();
        this._charBuffer[this._numChars++] = ch;
      }

      internal void AddByte(byte b)
      {
        if (this._byteBuffer == null)
          this._byteBuffer = new byte[this._bufferSize];
        this._byteBuffer[this._numBytes++] = b;
      }

      internal string GetString()
      {
        if (this._numBytes > 0)
          this.FlushBytes();
        return this._numChars > 0 ? new string(this._charBuffer, 0, this._numChars) : string.Empty;
      }
    }
  }
}
