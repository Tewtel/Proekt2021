// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.EncodingFoundException
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;
using System.Text;

namespace HtmlAgilityPack
{
  internal class EncodingFoundException : Exception
  {
    private Encoding _encoding;

    internal EncodingFoundException(Encoding encoding) => this._encoding = encoding;

    internal Encoding Encoding => this._encoding;
  }
}
