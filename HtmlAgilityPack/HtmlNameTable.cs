// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlNameTable
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System.Xml;

namespace HtmlAgilityPack
{
  internal class HtmlNameTable : XmlNameTable
  {
    private NameTable _nametable = new NameTable();

    public override string Add(string array) => this._nametable.Add(array);

    public override string Add(char[] array, int offset, int length) => this._nametable.Add(array, offset, length);

    public override string Get(string array) => this._nametable.Get(array);

    public override string Get(char[] array, int offset, int length) => this._nametable.Get(array, offset, length);

    internal string GetOrAdd(string array) => this.Get(array) ?? this.Add(array);
  }
}
