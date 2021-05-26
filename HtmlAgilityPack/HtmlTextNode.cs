// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlTextNode
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

namespace HtmlAgilityPack
{
  /// <summary>Represents an HTML text node.</summary>
  public class HtmlTextNode : HtmlNode
  {
    private string _text;

    internal HtmlTextNode(HtmlDocument ownerdocument, int index)
      : base(HtmlNodeType.Text, ownerdocument, index)
    {
    }

    /// <summary>
    /// Gets or Sets the HTML between the start and end tags of the object. In the case of a text node, it is equals to OuterHtml.
    /// </summary>
    public override string InnerHtml
    {
      get => this.OuterHtml;
      set => this._text = value;
    }

    /// <summary>Gets or Sets the object and its content in HTML.</summary>
    public override string OuterHtml => this._text == null ? base.OuterHtml : this._text;

    /// <summary>Gets or Sets the text of the node.</summary>
    public string Text
    {
      get => this._text == null ? base.OuterHtml : this._text;
      set
      {
        this._text = value;
        this.SetChanged();
      }
    }
  }
}
