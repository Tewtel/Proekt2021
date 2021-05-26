// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlCommentNode
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

namespace HtmlAgilityPack
{
  /// <summary>Represents an HTML comment.</summary>
  public class HtmlCommentNode : HtmlNode
  {
    private string _comment;

    internal HtmlCommentNode(HtmlDocument ownerdocument, int index)
      : base(HtmlNodeType.Comment, ownerdocument, index)
    {
    }

    /// <summary>Gets or Sets the comment text of the node.</summary>
    public string Comment
    {
      get => this._comment == null ? base.InnerHtml : this._comment;
      set => this._comment = value;
    }

    /// <summary>
    /// Gets or Sets the HTML between the start and end tags of the object. In the case of a text node, it is equals to OuterHtml.
    /// </summary>
    public override string InnerHtml
    {
      get => this._comment == null ? base.InnerHtml : this._comment;
      set => this._comment = value;
    }

    /// <summary>Gets or Sets the object and its content in HTML.</summary>
    public override string OuterHtml => this._comment == null ? base.OuterHtml : "<!--" + this._comment + "-->";
  }
}
