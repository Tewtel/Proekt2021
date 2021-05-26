// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlAttribute
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HtmlAgilityPack
{
  /// <summary>Represents an HTML attribute.</summary>
  [DebuggerDisplay("Name: {OriginalName}, Value: {Value}")]
  public class HtmlAttribute : IComparable
  {
    private int _line;
    internal int _lineposition;
    internal string _name;
    internal int _namelength;
    internal int _namestartindex;
    internal HtmlDocument _ownerdocument;
    internal HtmlNode _ownernode;
    private AttributeValueQuote _quoteType = AttributeValueQuote.DoubleQuote;
    internal int _streamposition;
    internal string _value;
    internal int _valuelength;
    internal int _valuestartindex;

    internal HtmlAttribute(HtmlDocument ownerdocument) => this._ownerdocument = ownerdocument;

    /// <summary>
    /// Gets the line number of this attribute in the document.
    /// </summary>
    public int Line
    {
      get => this._line;
      internal set => this._line = value;
    }

    /// <summary>
    /// Gets the column number of this attribute in the document.
    /// </summary>
    public int LinePosition => this._lineposition;

    /// <summary>
    /// Gets the stream position of the value of this attribute in the document, relative to the start of the document.
    /// </summary>
    public int ValueStartIndex => this._valuestartindex;

    /// <summary>Gets the length of the value.</summary>
    public int ValueLength => this._valuelength;

    public bool UseOriginalName { get; set; }

    /// <summary>Gets the qualified name of the attribute.</summary>
    public string Name
    {
      get
      {
        if (this._name == null)
          this._name = this._ownerdocument.Text.Substring(this._namestartindex, this._namelength);
        return !this.UseOriginalName ? this._name.ToLowerInvariant() : this._name;
      }
      set
      {
        this._name = value != null ? value : throw new ArgumentNullException(nameof (value));
        if (this._ownernode == null)
          return;
        this._ownernode.SetChanged();
      }
    }

    /// <summary>Name of attribute with original case</summary>
    public string OriginalName => this._name;

    /// <summary>
    /// Gets the HTML document to which this attribute belongs.
    /// </summary>
    public HtmlDocument OwnerDocument => this._ownerdocument;

    /// <summary>Gets the HTML node to which this attribute belongs.</summary>
    public HtmlNode OwnerNode => this._ownernode;

    /// <summary>
    /// Specifies what type of quote the data should be wrapped in
    /// </summary>
    public AttributeValueQuote QuoteType
    {
      get => this._quoteType;
      set => this._quoteType = value;
    }

    /// <summary>
    /// Gets the stream position of this attribute in the document, relative to the start of the document.
    /// </summary>
    public int StreamPosition => this._streamposition;

    /// <summary>Gets or sets the value of the attribute.</summary>
    public string Value
    {
      get
      {
        if (this._value == null && this._ownerdocument.Text == null && (this._valuestartindex == 0 && this._valuelength == 0))
          return (string) null;
        if (this._value == null)
        {
          this._value = this._ownerdocument.Text.Substring(this._valuestartindex, this._valuelength);
          if (!this._ownerdocument.BackwardCompatibility)
            this._value = HtmlEntity.DeEntitize(this._value);
        }
        return this._value;
      }
      set
      {
        this._value = value;
        if (this._ownernode == null)
          return;
        this._ownernode.SetChanged();
      }
    }

    /// <summary>Gets the DeEntitized value of the attribute.</summary>
    public string DeEntitizeValue => HtmlEntity.DeEntitize(this.Value);

    internal string XmlName => HtmlDocument.GetXmlName(this.Name, true, this.OwnerDocument.OptionPreserveXmlNamespaces);

    internal string XmlValue => this.Value;

    /// <summary>
    /// Gets a valid XPath string that points to this Attribute
    /// </summary>
    public string XPath => (this.OwnerNode == null ? "/" : this.OwnerNode.XPath + "/") + this.GetRelativeXpath();

    /// <summary>
    /// Compares the current instance with another attribute. Comparison is based on attributes' name.
    /// </summary>
    /// <param name="obj">An attribute to compare with this instance.</param>
    /// <returns>A 32-bit signed integer that indicates the relative order of the names comparison.</returns>
    public int CompareTo(object obj) => obj is HtmlAttribute htmlAttribute ? this.Name.CompareTo(htmlAttribute.Name) : throw new ArgumentException(nameof (obj));

    /// <summary>Creates a duplicate of this attribute.</summary>
    /// <returns>The cloned attribute.</returns>
    public HtmlAttribute Clone() => new HtmlAttribute(this._ownerdocument)
    {
      Name = this.Name,
      Value = this.Value,
      QuoteType = this.QuoteType
    };

    /// <summary>Removes this attribute from it's parents collection</summary>
    public void Remove() => this._ownernode.Attributes.Remove(this);

    private string GetRelativeXpath()
    {
      if (this.OwnerNode == null)
        return this.Name;
      int num = 1;
      foreach (HtmlAttribute attribute in (IEnumerable<HtmlAttribute>) this.OwnerNode.Attributes)
      {
        if (!(attribute.Name != this.Name))
        {
          if (attribute != this)
            ++num;
          else
            break;
        }
      }
      return "@" + this.Name + "[" + num.ToString() + "]";
    }
  }
}
