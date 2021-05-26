// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.ParsedMediaTypeHeaderValue
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
  internal struct ParsedMediaTypeHeaderValue
  {
    private const char MediaRangeAsterisk = '*';
    private const char MediaTypeSubtypeDelimiter = '/';
    private readonly string _mediaType;
    private readonly int _delimiterIndex;
    private readonly bool _isAllMediaRange;
    private readonly bool _isSubtypeMediaRange;

    public ParsedMediaTypeHeaderValue(MediaTypeHeaderValue mediaTypeHeaderValue)
    {
      string str = this._mediaType = mediaTypeHeaderValue.MediaType;
      this._delimiterIndex = str.IndexOf('/');
      this._isAllMediaRange = false;
      this._isSubtypeMediaRange = false;
      int length = str.Length;
      if (this._delimiterIndex != length - 2 || str[length - 1] != '*')
        return;
      this._isSubtypeMediaRange = true;
      if (this._delimiterIndex != 1 || str[0] != '*')
        return;
      this._isAllMediaRange = true;
    }

    public bool IsAllMediaRange => this._isAllMediaRange;

    public bool IsSubtypeMediaRange => this._isSubtypeMediaRange;

    public bool TypesEqual(ref ParsedMediaTypeHeaderValue other) => this._delimiterIndex == other._delimiterIndex && string.Compare(this._mediaType, 0, other._mediaType, 0, this._delimiterIndex, StringComparison.OrdinalIgnoreCase) == 0;

    public bool SubTypesEqual(ref ParsedMediaTypeHeaderValue other)
    {
      int length = this._mediaType.Length - this._delimiterIndex - 1;
      return length == other._mediaType.Length - other._delimiterIndex - 1 && string.Compare(this._mediaType, this._delimiterIndex + 1, other._mediaType, other._delimiterIndex + 1, length, StringComparison.OrdinalIgnoreCase) == 0;
    }
  }
}
