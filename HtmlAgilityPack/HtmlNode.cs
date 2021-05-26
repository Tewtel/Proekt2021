// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlNode
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace HtmlAgilityPack
{
  /// <summary>Represents an HTML node.</summary>
  [DebuggerDisplay("Name: {OriginalName}")]
  public class HtmlNode : IXPathNavigable
  {
    internal const string DepthLevelExceptionMessage = "The document is too complex to parse";
    internal HtmlAttributeCollection _attributes;
    internal HtmlNodeCollection _childnodes;
    internal HtmlNode _endnode;
    private bool _changed;
    internal string _innerhtml;
    internal int _innerlength;
    internal int _innerstartindex;
    internal int _line;
    internal int _lineposition;
    private string _name;
    internal int _namelength;
    internal int _namestartindex;
    internal HtmlNode _nextnode;
    internal HtmlNodeType _nodetype;
    internal string _outerhtml;
    internal int _outerlength;
    internal int _outerstartindex;
    private string _optimizedName;
    internal HtmlDocument _ownerdocument;
    internal HtmlNode _parentnode;
    internal HtmlNode _prevnode;
    internal HtmlNode _prevwithsamename;
    internal bool _starttag;
    internal int _streamposition;
    internal bool _isImplicitEnd;
    internal bool _isHideInnerText;
    /// <summary>
    /// Gets the name of a comment node. It is actually defined as '#comment'.
    /// </summary>
    public static readonly string HtmlNodeTypeNameComment = "#comment";
    /// <summary>
    /// Gets the name of the document node. It is actually defined as '#document'.
    /// </summary>
    public static readonly string HtmlNodeTypeNameDocument = "#document";
    /// <summary>
    /// Gets the name of a text node. It is actually defined as '#text'.
    /// </summary>
    public static readonly string HtmlNodeTypeNameText = "#text";
    /// <summary>
    /// Gets a collection of flags that define specific behaviors for specific element nodes.
    /// The table contains a DictionaryEntry list with the lowercase tag name as the Key, and a combination of HtmlElementFlags as the Value.
    /// </summary>
    public static Dictionary<string, HtmlElementFlag> ElementsFlags = new Dictionary<string, HtmlElementFlag>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initialize HtmlNode. Builds a list of all tags that have special allowances
    /// </summary>
    static HtmlNode()
    {
      HtmlNode.ElementsFlags.Add("script", HtmlElementFlag.CData);
      HtmlNode.ElementsFlags.Add("style", HtmlElementFlag.CData);
      HtmlNode.ElementsFlags.Add("noxhtml", HtmlElementFlag.CData);
      HtmlNode.ElementsFlags.Add("textarea", HtmlElementFlag.CData);
      HtmlNode.ElementsFlags.Add("title", HtmlElementFlag.CData);
      HtmlNode.ElementsFlags.Add("base", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("link", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("meta", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("isindex", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("hr", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("col", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("img", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("param", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("embed", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("frame", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("wbr", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("bgsound", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("spacer", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("keygen", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("area", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("input", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("basefont", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("source", HtmlElementFlag.Empty);
      HtmlNode.ElementsFlags.Add("form", HtmlElementFlag.CanOverlap);
      HtmlNode.ElementsFlags.Add("br", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
      if (HtmlDocument.DisableBehaviorTagP)
        return;
      HtmlNode.ElementsFlags.Add("p", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
    }

    /// <summary>
    /// Initializes HtmlNode, providing type, owner and where it exists in a collection
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ownerdocument"></param>
    /// <param name="index"></param>
    public HtmlNode(HtmlNodeType type, HtmlDocument ownerdocument, int index)
    {
      this._nodetype = type;
      this._ownerdocument = ownerdocument;
      this._outerstartindex = index;
      switch (type)
      {
        case HtmlNodeType.Document:
          this.Name = HtmlNode.HtmlNodeTypeNameDocument;
          this._endnode = this;
          break;
        case HtmlNodeType.Comment:
          this.Name = HtmlNode.HtmlNodeTypeNameComment;
          this._endnode = this;
          break;
        case HtmlNodeType.Text:
          this.Name = HtmlNode.HtmlNodeTypeNameText;
          this._endnode = this;
          break;
      }
      if (this._ownerdocument.Openednodes != null && !this.Closed && -1 != index)
        this._ownerdocument.Openednodes.Add(index, this);
      if (-1 != index || type == HtmlNodeType.Comment || type == HtmlNodeType.Text)
        return;
      this.SetChanged();
    }

    /// <summary>
    /// Gets the collection of HTML attributes for this node. May not be null.
    /// </summary>
    public HtmlAttributeCollection Attributes
    {
      get
      {
        if (!this.HasAttributes)
          this._attributes = new HtmlAttributeCollection(this);
        return this._attributes;
      }
      internal set => this._attributes = value;
    }

    /// <summary>Gets all the children of the node.</summary>
    public HtmlNodeCollection ChildNodes
    {
      get => this._childnodes ?? (this._childnodes = new HtmlNodeCollection(this));
      internal set => this._childnodes = value;
    }

    /// <summary>
    /// Gets a value indicating if this node has been closed or not.
    /// </summary>
    public bool Closed => this._endnode != null;

    /// <summary>
    /// Gets the collection of HTML attributes for the closing tag. May not be null.
    /// </summary>
    public HtmlAttributeCollection ClosingAttributes => this.HasClosingAttributes ? this._endnode.Attributes : new HtmlAttributeCollection(this);

    /// <summary>
    /// Gets the closing tag of the node, null if the node is self-closing.
    /// </summary>
    public HtmlNode EndNode => this._endnode;

    /// <summary>Gets the first child of the node.</summary>
    public HtmlNode FirstChild => this.HasChildNodes ? this._childnodes[0] : (HtmlNode) null;

    /// <summary>
    /// Gets a value indicating whether the current node has any attributes.
    /// </summary>
    public bool HasAttributes => this._attributes != null && this._attributes.Count > 0;

    /// <summary>
    /// Gets a value indicating whether this node has any child nodes.
    /// </summary>
    public bool HasChildNodes => this._childnodes != null && this._childnodes.Count > 0;

    /// <summary>
    /// Gets a value indicating whether the current node has any attributes on the closing tag.
    /// </summary>
    public bool HasClosingAttributes => this._endnode != null && this._endnode != this && (this._endnode._attributes != null && this._endnode._attributes.Count > 0);

    /// <summary>
    /// Gets or sets the value of the 'id' HTML attribute. The document must have been parsed using the OptionUseIdAttribute set to true.
    /// </summary>
    public string Id
    {
      get
      {
        if (this._ownerdocument.Nodesid == null)
          throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
        return this.GetId();
      }
      set
      {
        if (this._ownerdocument.Nodesid == null)
          throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        this.SetId(value);
      }
    }

    /// <summary>
    /// Gets or Sets the HTML between the start and end tags of the object.
    /// </summary>
    public virtual string InnerHtml
    {
      get
      {
        if (this._changed)
        {
          this.UpdateHtml();
          return this._innerhtml;
        }
        if (this._innerhtml != null)
          return this._innerhtml;
        return this._innerstartindex < 0 || this._innerlength < 0 ? string.Empty : this._ownerdocument.Text.Substring(this._innerstartindex, this._innerlength);
      }
      set
      {
        HtmlDocument htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(value);
        this.RemoveAllChildren();
        this.AppendChildren(htmlDocument.DocumentNode.ChildNodes);
      }
    }

    /// <summary>
    /// Gets the text between the start and end tags of the object.
    /// </summary>
    public virtual string InnerText
    {
      get
      {
        StringBuilder sb = new StringBuilder();
        string name = this.Name;
        if (name != null)
        {
          string lowerInvariant = name.ToLowerInvariant();
          bool isDisplayScriptingText = lowerInvariant == "head" || lowerInvariant == "script" || lowerInvariant == "style";
          this.InternalInnerText(sb, isDisplayScriptingText);
        }
        else
          this.InternalInnerText(sb, false);
        return sb.ToString();
      }
    }

    internal virtual void InternalInnerText(StringBuilder sb, bool isDisplayScriptingText)
    {
      if (!this._ownerdocument.BackwardCompatibility)
      {
        if (this.HasChildNodes)
          this.AppendInnerText(sb, isDisplayScriptingText);
        else
          sb.Append(this.GetCurrentNodeText());
      }
      else if (this._nodetype == HtmlNodeType.Text)
      {
        sb.Append(((HtmlTextNode) this).Text);
      }
      else
      {
        if (this._nodetype == HtmlNodeType.Comment || !this.HasChildNodes || this._isHideInnerText && !isDisplayScriptingText)
          return;
        foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ChildNodes)
          childNode.InternalInnerText(sb, isDisplayScriptingText);
      }
    }

    /// <summary>Gets direct inner text.</summary>
    /// <returns>The direct inner text.</returns>
    public virtual string GetDirectInnerText()
    {
      if (!this._ownerdocument.BackwardCompatibility)
      {
        if (!this.HasChildNodes)
          return this.GetCurrentNodeText();
        StringBuilder sb = new StringBuilder();
        this.AppendDirectInnerText(sb);
        return sb.ToString();
      }
      if (this._nodetype == HtmlNodeType.Text)
        return ((HtmlTextNode) this).Text;
      if (this._nodetype == HtmlNodeType.Comment)
        return "";
      if (!this.HasChildNodes)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ChildNodes)
      {
        if (childNode._nodetype == HtmlNodeType.Text)
          stringBuilder.Append(((HtmlTextNode) childNode).Text);
      }
      return stringBuilder.ToString();
    }

    internal string GetCurrentNodeText()
    {
      if (this._nodetype != HtmlNodeType.Text)
        return "";
      string str = ((HtmlTextNode) this).Text;
      if (this.ParentNode.Name != "pre")
        str = str.Replace("\n", "").Replace("\r", "").Replace("\t", "");
      return str;
    }

    internal void AppendDirectInnerText(StringBuilder sb)
    {
      if (this._nodetype == HtmlNodeType.Text)
        sb.Append(this.GetCurrentNodeText());
      if (!this.HasChildNodes)
        return;
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ChildNodes)
        sb.Append(childNode.GetCurrentNodeText());
    }

    internal void AppendInnerText(StringBuilder sb, bool isShowHideInnerText)
    {
      if (this._nodetype == HtmlNodeType.Text)
        sb.Append(this.GetCurrentNodeText());
      if (!this.HasChildNodes || this._isHideInnerText && !isShowHideInnerText)
        return;
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ChildNodes)
        childNode.AppendInnerText(sb, isShowHideInnerText);
    }

    /// <summary>Gets the last child of the node.</summary>
    public HtmlNode LastChild => this.HasChildNodes ? this._childnodes[this._childnodes.Count - 1] : (HtmlNode) null;

    /// <summary>Gets the line number of this node in the document.</summary>
    public int Line
    {
      get => this._line;
      internal set => this._line = value;
    }

    /// <summary>Gets the column number of this node in the document.</summary>
    public int LinePosition
    {
      get => this._lineposition;
      internal set => this._lineposition = value;
    }

    /// <summary>
    /// Gets the stream position of the area between the opening and closing tag of the node, relative to the start of the document.
    /// </summary>
    public int InnerStartIndex => this._innerstartindex;

    /// <summary>
    /// Gets the length of the area between the opening and closing tag of the node.
    /// </summary>
    public int InnerLength => this.InnerHtml.Length;

    /// <summary>
    /// Gets the length of the entire node, opening and closing tag included.
    /// </summary>
    public int OuterLength => this.OuterHtml.Length;

    /// <summary>Gets or sets this node's name.</summary>
    public string Name
    {
      get
      {
        if (this._optimizedName == null)
        {
          if (this._name == null)
            this.Name = this._ownerdocument.Text.Substring(this._namestartindex, this._namelength);
          this._optimizedName = this._name != null ? this._name.ToLowerInvariant() : string.Empty;
        }
        return this._optimizedName;
      }
      set
      {
        this._name = value;
        this._optimizedName = (string) null;
      }
    }

    /// <summary>
    /// Gets the HTML node immediately following this element.
    /// </summary>
    public HtmlNode NextSibling
    {
      get => this._nextnode;
      internal set => this._nextnode = value;
    }

    /// <summary>Gets the type of this node.</summary>
    public HtmlNodeType NodeType
    {
      get => this._nodetype;
      internal set => this._nodetype = value;
    }

    /// <summary>The original unaltered name of the tag</summary>
    public string OriginalName => this._name;

    /// <summary>Gets or Sets the object and its content in HTML.</summary>
    public virtual string OuterHtml
    {
      get
      {
        if (this._changed)
        {
          this.UpdateHtml();
          return this._outerhtml;
        }
        if (this._outerhtml != null)
          return this._outerhtml;
        return this._outerstartindex < 0 || this._outerlength < 0 ? string.Empty : this._ownerdocument.Text.Substring(this._outerstartindex, this._outerlength);
      }
    }

    /// <summary>
    /// Gets the <see cref="T:HtmlAgilityPack.HtmlDocument" /> to which this node belongs.
    /// </summary>
    public HtmlDocument OwnerDocument
    {
      get => this._ownerdocument;
      internal set => this._ownerdocument = value;
    }

    /// <summary>
    /// Gets the parent of this node (for nodes that can have parents).
    /// </summary>
    public HtmlNode ParentNode
    {
      get => this._parentnode;
      internal set => this._parentnode = value;
    }

    /// <summary>Gets the node immediately preceding this node.</summary>
    public HtmlNode PreviousSibling
    {
      get => this._prevnode;
      internal set => this._prevnode = value;
    }

    /// <summary>
    /// Gets the stream position of this node in the document, relative to the start of the document.
    /// </summary>
    public int StreamPosition => this._streamposition;

    /// <summary>Gets a valid XPath string that points to this node</summary>
    public string XPath => (this.ParentNode == null || this.ParentNode.NodeType == HtmlNodeType.Document ? "/" : this.ParentNode.XPath + "/") + this.GetRelativeXpath();

    /// <summary>
    /// The depth of the node relative to the opening root html element. This value is used to determine if a document has to many nested html nodes which can cause stack overflows
    /// </summary>
    public int Depth { get; set; }

    /// <summary>Determines if an element node can be kept overlapped.</summary>
    /// <param name="name">The name of the element node to check. May not be <c>null</c>.</param>
    /// <returns>true if the name is the name of an element node that can be kept overlapped, <c>false</c> otherwise.</returns>
    public static bool CanOverlapElement(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      HtmlElementFlag htmlElementFlag;
      return HtmlNode.ElementsFlags.TryGetValue(name, out htmlElementFlag) && (uint) (htmlElementFlag & HtmlElementFlag.CanOverlap) > 0U;
    }

    /// <summary>
    /// Creates an HTML node from a string representing literal HTML.
    /// </summary>
    /// <param name="html">The HTML text.</param>
    /// <returns>The newly created node instance.</returns>
    public static HtmlNode CreateNode(string html)
    {
      HtmlDocument htmlDocument = new HtmlDocument();
      htmlDocument.LoadHtml(html);
      if (!htmlDocument.DocumentNode.IsSingleElementNode())
        throw new Exception("Multiple node elments can't be created.");
      for (HtmlNode htmlNode = htmlDocument.DocumentNode.FirstChild; htmlNode != null; htmlNode = htmlNode.NextSibling)
      {
        if (htmlNode.NodeType == HtmlNodeType.Element && htmlNode.OuterHtml != "\r\n")
          return htmlNode;
      }
      return htmlDocument.DocumentNode.FirstChild;
    }

    /// <summary>
    /// Determines if an element node is a CDATA element node.
    /// </summary>
    /// <param name="name">The name of the element node to check. May not be null.</param>
    /// <returns>true if the name is the name of a CDATA element node, false otherwise.</returns>
    public static bool IsCDataElement(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      HtmlElementFlag htmlElementFlag;
      return HtmlNode.ElementsFlags.TryGetValue(name, out htmlElementFlag) && (uint) (htmlElementFlag & HtmlElementFlag.CData) > 0U;
    }

    /// <summary>Determines if an element node is closed.</summary>
    /// <param name="name">The name of the element node to check. May not be null.</param>
    /// <returns>true if the name is the name of a closed element node, false otherwise.</returns>
    public static bool IsClosedElement(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      HtmlElementFlag htmlElementFlag;
      return HtmlNode.ElementsFlags.TryGetValue(name, out htmlElementFlag) && (uint) (htmlElementFlag & HtmlElementFlag.Closed) > 0U;
    }

    /// <summary>Determines if an element node is defined as empty.</summary>
    /// <param name="name">The name of the element node to check. May not be null.</param>
    /// <returns>true if the name is the name of an empty element node, false otherwise.</returns>
    public static bool IsEmptyElement(string name)
    {
      switch (name)
      {
        case "":
          return true;
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          if ('!' == name[0] || '?' == name[0])
            return true;
          HtmlElementFlag htmlElementFlag;
          return HtmlNode.ElementsFlags.TryGetValue(name, out htmlElementFlag) && (uint) (htmlElementFlag & HtmlElementFlag.Empty) > 0U;
      }
    }

    /// <summary>
    /// Determines if a text corresponds to the closing tag of an node that can be kept overlapped.
    /// </summary>
    /// <param name="text">The text to check. May not be null.</param>
    /// <returns>true or false.</returns>
    public static bool IsOverlappedClosingElement(string text)
    {
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      return text.Length > 4 && text[0] == '<' && (text[text.Length - 1] == '>' && text[1] == '/') && HtmlNode.CanOverlapElement(text.Substring(2, text.Length - 3));
    }

    /// <summary>
    /// Returns a collection of all ancestor nodes of this element.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Ancestors()
    {
      HtmlNode node = this.ParentNode;
      if (node != null)
      {
        yield return node;
        for (; node.ParentNode != null; node = node.ParentNode)
          yield return node.ParentNode;
      }
    }

    /// <summary>Get Ancestors with matching name</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Ancestors(string name)
    {
      HtmlNode n;
      for (n = this.ParentNode; n != null; n = n.ParentNode)
      {
        if (n.Name == name)
          yield return n;
      }
      n = (HtmlNode) null;
    }

    /// <summary>
    /// Returns a collection of all ancestor nodes of this element.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<HtmlNode> AncestorsAndSelf()
    {
      HtmlNode n;
      for (n = this; n != null; n = n.ParentNode)
        yield return n;
      n = (HtmlNode) null;
    }

    /// <summary>Gets all anscestor nodes and the current node</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<HtmlNode> AncestorsAndSelf(string name)
    {
      HtmlNode n;
      for (n = this; n != null; n = n.ParentNode)
      {
        if (n.Name == name)
          yield return n;
      }
      n = (HtmlNode) null;
    }

    /// <summary>
    /// Adds the specified node to the end of the list of children of this node.
    /// </summary>
    /// <param name="newChild">The node to add. May not be null.</param>
    /// <returns>The node added.</returns>
    public HtmlNode AppendChild(HtmlNode newChild)
    {
      if (newChild == null)
        throw new ArgumentNullException(nameof (newChild));
      this.ChildNodes.Append(newChild);
      this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
      this.SetChildNodesId(newChild);
      this.SetChanged();
      return newChild;
    }

    /// <summary>Sets child nodes identifier.</summary>
    /// <param name="chilNode">The chil node.</param>
    public void SetChildNodesId(HtmlNode chilNode)
    {
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) chilNode.ChildNodes)
      {
        this._ownerdocument.SetIdForNode(childNode, childNode.GetId());
        this.SetChildNodesId(childNode);
      }
    }

    /// <summary>
    /// Adds the specified node to the end of the list of children of this node.
    /// </summary>
    /// <param name="newChildren">The node list to add. May not be null.</param>
    public void AppendChildren(HtmlNodeCollection newChildren)
    {
      if (newChildren == null)
        throw new ArgumentNullException(nameof (newChildren));
      foreach (HtmlNode newChild in (IEnumerable<HtmlNode>) newChildren)
        this.AppendChild(newChild);
    }

    /// <summary>Gets all Attributes with name</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<HtmlAttribute> ChildAttributes(string name) => this.Attributes.AttributesWithName(name);

    /// <summary>Creates a duplicate of the node</summary>
    /// <returns></returns>
    public HtmlNode Clone() => this.CloneNode(true);

    /// <summary>
    /// Creates a duplicate of the node and changes its name at the same time.
    /// </summary>
    /// <param name="newName">The new name of the cloned node. May not be <c>null</c>.</param>
    /// <returns>The cloned node.</returns>
    public HtmlNode CloneNode(string newName) => this.CloneNode(newName, true);

    /// <summary>
    /// Creates a duplicate of the node and changes its name at the same time.
    /// </summary>
    /// <param name="newName">The new name of the cloned node. May not be null.</param>
    /// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself.</param>
    /// <returns>The cloned node.</returns>
    public HtmlNode CloneNode(string newName, bool deep)
    {
      if (newName == null)
        throw new ArgumentNullException(nameof (newName));
      HtmlNode htmlNode = this.CloneNode(deep);
      htmlNode.Name = newName;
      return htmlNode;
    }

    /// <summary>Creates a duplicate of the node.</summary>
    /// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself.</param>
    /// <returns>The cloned node.</returns>
    public HtmlNode CloneNode(bool deep)
    {
      HtmlNode node = this._ownerdocument.CreateNode(this._nodetype);
      node.Name = this.OriginalName;
      switch (this._nodetype)
      {
        case HtmlNodeType.Comment:
          ((HtmlCommentNode) node).Comment = ((HtmlCommentNode) this).Comment;
          return node;
        case HtmlNodeType.Text:
          ((HtmlTextNode) node).Text = ((HtmlTextNode) this).Text;
          return node;
        default:
          if (this.HasAttributes)
          {
            foreach (HtmlAttribute attribute in (IEnumerable<HtmlAttribute>) this._attributes)
            {
              HtmlAttribute newAttribute = attribute.Clone();
              node.Attributes.Append(newAttribute);
            }
          }
          if (this.HasClosingAttributes)
          {
            node._endnode = this._endnode.CloneNode(false);
            foreach (HtmlAttribute attribute in (IEnumerable<HtmlAttribute>) this._endnode._attributes)
            {
              HtmlAttribute newAttribute = attribute.Clone();
              node._endnode._attributes.Append(newAttribute);
            }
          }
          if (!deep || !this.HasChildNodes)
            return node;
          foreach (HtmlNode childnode in (IEnumerable<HtmlNode>) this._childnodes)
          {
            HtmlNode newChild = childnode.CloneNode(deep);
            node.AppendChild(newChild);
          }
          return node;
      }
    }

    /// <summary>
    /// Creates a duplicate of the node and the subtree under it.
    /// </summary>
    /// <param name="node">The node to duplicate. May not be <c>null</c>.</param>
    public void CopyFrom(HtmlNode node) => this.CopyFrom(node, true);

    /// <summary>Creates a duplicate of the node.</summary>
    /// <param name="node">The node to duplicate. May not be <c>null</c>.</param>
    /// <param name="deep">true to recursively clone the subtree under the specified node, false to clone only the node itself.</param>
    public void CopyFrom(HtmlNode node, bool deep)
    {
      if (node == null)
        throw new ArgumentNullException(nameof (node));
      this.Attributes.RemoveAll();
      if (node.HasAttributes)
      {
        foreach (HtmlAttribute attribute in (IEnumerable<HtmlAttribute>) node.Attributes)
          this.Attributes.Append(attribute.Clone());
      }
      if (!deep)
        return;
      this.RemoveAllChildren();
      if (!node.HasChildNodes)
        return;
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) node.ChildNodes)
        this.AppendChild(childNode.CloneNode(true));
    }

    /// <summary>
    /// Gets all Descendant nodes for this node and each of child nodes
    /// </summary>
    /// <param name="level">The depth level of the node to parse in the html tree</param>
    /// <returns>the current element as an HtmlNode</returns>
    [Obsolete("Use Descendants() instead, the results of this function will change in a future version")]
    public IEnumerable<HtmlNode> DescendantNodes(int level = 0)
    {
      if (level > HtmlDocument.MaxDepthLevel)
        throw new ArgumentException("The document is too complex to parse");
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ChildNodes)
      {
        HtmlNode node = childNode;
        yield return node;
        foreach (HtmlNode descendantNode in node.DescendantNodes(level + 1))
          yield return descendantNode;
        node = (HtmlNode) null;
      }
    }

    /// <summary>
    /// Returns a collection of all descendant nodes of this element, in document order
    /// </summary>
    /// <returns></returns>
    [Obsolete("Use DescendantsAndSelf() instead, the results of this function will change in a future version")]
    public IEnumerable<HtmlNode> DescendantNodesAndSelf() => this.DescendantsAndSelf();

    /// <summary>Gets all Descendant nodes in enumerated list</summary>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Descendants() => this.Descendants(0);

    /// <summary>Gets all Descendant nodes in enumerated list</summary>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Descendants(int level)
    {
      if (level > HtmlDocument.MaxDepthLevel)
        throw new ArgumentException("The document is too complex to parse");
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ChildNodes)
      {
        HtmlNode node = childNode;
        yield return node;
        foreach (HtmlNode descendant in node.Descendants(level + 1))
          yield return descendant;
        node = (HtmlNode) null;
      }
    }

    /// <summary>Get all descendant nodes with matching name</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Descendants(string name)
    {
      foreach (HtmlNode descendant in this.Descendants())
      {
        if (string.Equals(descendant.Name, name, StringComparison.OrdinalIgnoreCase))
          yield return descendant;
      }
    }

    /// <summary>
    /// Returns a collection of all descendant nodes of this element, in document order
    /// </summary>
    /// <returns></returns>
    public IEnumerable<HtmlNode> DescendantsAndSelf()
    {
      HtmlNode htmlNode = this;
      yield return htmlNode;
      foreach (HtmlNode descendant in htmlNode.Descendants())
      {
        if (descendant != null)
          yield return descendant;
      }
    }

    /// <summary>Gets all descendant nodes including this node</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<HtmlNode> DescendantsAndSelf(string name)
    {
      HtmlNode htmlNode = this;
      yield return htmlNode;
      foreach (HtmlNode descendant in htmlNode.Descendants())
      {
        if (descendant.Name == name)
          yield return descendant;
      }
    }

    /// <summary>Gets first generation child node matching name</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public HtmlNode Element(string name)
    {
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ChildNodes)
      {
        if (childNode.Name == name)
          return childNode;
      }
      return (HtmlNode) null;
    }

    /// <summary>
    /// Gets matching first generation child nodes matching name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IEnumerable<HtmlNode> Elements(string name)
    {
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ChildNodes)
      {
        if (childNode.Name == name)
          yield return childNode;
      }
    }

    /// <summary>Gets data attribute.</summary>
    /// <param name="key">The key.</param>
    /// <returns>The data attribute.</returns>
    public HtmlAttribute GetDataAttribute(string key) => this.Attributes.Hashitems.SingleOrDefault<KeyValuePair<string, HtmlAttribute>>((Func<KeyValuePair<string, HtmlAttribute>, bool>) (x => x.Key.Equals("data-" + key, StringComparison.OrdinalIgnoreCase))).Value;

    /// <summary>Gets the data attributes in this collection.</summary>
    /// <returns>
    /// An enumerator that allows foreach to be used to process the data attributes in this
    /// collection.
    /// </returns>
    public IEnumerable<HtmlAttribute> GetDataAttributes() => (IEnumerable<HtmlAttribute>) this.Attributes.Hashitems.Where<KeyValuePair<string, HtmlAttribute>>((Func<KeyValuePair<string, HtmlAttribute>, bool>) (x => x.Key.StartsWith("data-", StringComparison.OrdinalIgnoreCase))).Select<KeyValuePair<string, HtmlAttribute>, HtmlAttribute>((Func<KeyValuePair<string, HtmlAttribute>, HtmlAttribute>) (x => x.Value)).ToList<HtmlAttribute>();

    /// <summary>Gets the attributes in this collection.</summary>
    /// <returns>
    /// An enumerator that allows foreach to be used to process the attributes in this collection.
    /// </returns>
    public IEnumerable<HtmlAttribute> GetAttributes() => (IEnumerable<HtmlAttribute>) this.Attributes.items;

    /// <summary>Gets the attributes in this collection.</summary>
    /// <param name="attributeNames">A variable-length parameters list containing attribute names.</param>
    /// <returns>
    /// An enumerator that allows foreach to be used to process the attributes in this collection.
    /// </returns>
    public IEnumerable<HtmlAttribute> GetAttributes(
      params string[] attributeNames)
    {
      List<HtmlAttribute> htmlAttributeList = new List<HtmlAttribute>();
      foreach (string attributeName in attributeNames)
        htmlAttributeList.Add(this.Attributes[attributeName]);
      return (IEnumerable<HtmlAttribute>) htmlAttributeList;
    }

    /// <summary>
    /// Helper method to get the value of an attribute of this node. If the attribute is not found, the default value will be returned.
    /// </summary>
    /// <param name="name">The name of the attribute to get. May not be <c>null</c>.</param>
    /// <param name="def">The default value to return if not found.</param>
    /// <returns>The value of the attribute if found, the default value if not found.</returns>
    public string GetAttributeValue(string name, string def) => this.GetAttributeValue<string>(name, def);

    /// <summary>
    /// Helper method to get the value of an attribute of this node. If the attribute is not found, the default value will be returned.
    /// </summary>
    /// <param name="name">The name of the attribute to get. May not be <c>null</c>.</param>
    /// <param name="def">The default value to return if not found.</param>
    /// <returns>The value of the attribute if found, the default value if not found.</returns>
    public int GetAttributeValue(string name, int def) => this.GetAttributeValue<int>(name, def);

    /// <summary>
    /// Helper method to get the value of an attribute of this node. If the attribute is not found, the default value will be returned.
    /// </summary>
    /// <param name="name">The name of the attribute to get. May not be <c>null</c>.</param>
    /// <param name="def">The default value to return if not found.</param>
    /// <returns>The value of the attribute if found, the default value if not found.</returns>
    public bool GetAttributeValue(string name, bool def) => this.GetAttributeValue<bool>(name, def);

    /// <summary>
    /// Helper method to get the value of an attribute of this node. If the attribute is not found,
    /// the default value will be returned.
    /// </summary>
    /// <param name="name">The name of the attribute to get. May not be <c>null</c>.</param>
    /// <param name="def">The default value to return if not found.</param>
    /// <returns>The value of the attribute if found, the default value if not found.</returns>
    public T GetAttributeValue<T>(string name, T def)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (!this.HasAttributes)
        return def;
      HtmlAttribute attribute = this.Attributes[name];
      if (attribute == null)
        return def;
      TypeConverter converter = TypeDescriptor.GetConverter(typeof (T));
      try
      {
        return converter != null && converter.CanConvertTo(attribute.Value.GetType()) ? (T) converter.ConvertTo((object) attribute.Value, typeof (T)) : (T) attribute.Value;
      }
      catch
      {
        return def;
      }
    }

    /// <summary>
    /// Inserts the specified node immediately after the specified reference node.
    /// </summary>
    /// <param name="newChild">The node to insert. May not be <c>null</c>.</param>
    /// <param name="refChild">The node that is the reference node. The newNode is placed after the refNode.</param>
    /// <returns>The node being inserted.</returns>
    public HtmlNode InsertAfter(HtmlNode newChild, HtmlNode refChild)
    {
      if (newChild == null)
        throw new ArgumentNullException(nameof (newChild));
      if (refChild == null)
        return this.PrependChild(newChild);
      if (newChild == refChild)
        return newChild;
      int num = -1;
      if (this._childnodes != null)
        num = this._childnodes[refChild];
      if (num == -1)
        throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
      if (this._childnodes != null)
        this._childnodes.Insert(num + 1, newChild);
      this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
      this.SetChildNodesId(newChild);
      this.SetChanged();
      return newChild;
    }

    /// <summary>
    /// Inserts the specified node immediately before the specified reference node.
    /// </summary>
    /// <param name="newChild">The node to insert. May not be <c>null</c>.</param>
    /// <param name="refChild">The node that is the reference node. The newChild is placed before this node.</param>
    /// <returns>The node being inserted.</returns>
    public HtmlNode InsertBefore(HtmlNode newChild, HtmlNode refChild)
    {
      if (newChild == null)
        throw new ArgumentNullException(nameof (newChild));
      if (refChild == null)
        return this.AppendChild(newChild);
      if (newChild == refChild)
        return newChild;
      int index = -1;
      if (this._childnodes != null)
        index = this._childnodes[refChild];
      if (index == -1)
        throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
      if (this._childnodes != null)
        this._childnodes.Insert(index, newChild);
      this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
      this.SetChildNodesId(newChild);
      this.SetChanged();
      return newChild;
    }

    /// <summary>
    /// Adds the specified node to the beginning of the list of children of this node.
    /// </summary>
    /// <param name="newChild">The node to add. May not be <c>null</c>.</param>
    /// <returns>The node added.</returns>
    public HtmlNode PrependChild(HtmlNode newChild)
    {
      if (newChild == null)
        throw new ArgumentNullException(nameof (newChild));
      this.ChildNodes.Prepend(newChild);
      this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
      this.SetChildNodesId(newChild);
      this.SetChanged();
      return newChild;
    }

    /// <summary>
    /// Adds the specified node list to the beginning of the list of children of this node.
    /// </summary>
    /// <param name="newChildren">The node list to add. May not be <c>null</c>.</param>
    public void PrependChildren(HtmlNodeCollection newChildren)
    {
      if (newChildren == null)
        throw new ArgumentNullException(nameof (newChildren));
      for (int index = newChildren.Count - 1; index >= 0; --index)
        this.PrependChild(newChildren[index]);
    }

    /// <summary>Removes node from parent collection</summary>
    public void Remove()
    {
      if (this.ParentNode == null)
        return;
      this.ParentNode.ChildNodes.Remove(this);
    }

    /// <summary>
    /// Removes all the children and/or attributes of the current node.
    /// </summary>
    public void RemoveAll()
    {
      this.RemoveAllChildren();
      if (this.HasAttributes)
        this._attributes.Clear();
      if (this._endnode != null && this._endnode != this && this._endnode._attributes != null)
        this._endnode._attributes.Clear();
      this.SetChanged();
    }

    /// <summary>Removes all the children of the current node.</summary>
    public void RemoveAllChildren()
    {
      if (!this.HasChildNodes)
        return;
      if (this._ownerdocument.OptionUseIdAttribute)
      {
        foreach (HtmlNode childnode in (IEnumerable<HtmlNode>) this._childnodes)
        {
          this._ownerdocument.SetIdForNode((HtmlNode) null, childnode.GetId());
          this.RemoveAllIDforNode(childnode);
        }
      }
      this._childnodes.Clear();
      this.SetChanged();
    }

    /// <summary>Removes all id for node described by node.</summary>
    /// <param name="node">The node.</param>
    public void RemoveAllIDforNode(HtmlNode node)
    {
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) node.ChildNodes)
      {
        this._ownerdocument.SetIdForNode((HtmlNode) null, childNode.GetId());
        this.RemoveAllIDforNode(childNode);
      }
    }

    /// <summary>Removes the specified child node.</summary>
    /// <param name="oldChild">The node being removed. May not be <c>null</c>.</param>
    /// <returns>The node removed.</returns>
    public HtmlNode RemoveChild(HtmlNode oldChild)
    {
      if (oldChild == null)
        throw new ArgumentNullException(nameof (oldChild));
      int index = -1;
      if (this._childnodes != null)
        index = this._childnodes[oldChild];
      if (index == -1)
        throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
      if (this._childnodes != null)
        this._childnodes.Remove(index);
      this._ownerdocument.SetIdForNode((HtmlNode) null, oldChild.GetId());
      this.RemoveAllIDforNode(oldChild);
      this.SetChanged();
      return oldChild;
    }

    /// <summary>Removes the specified child node.</summary>
    /// <param name="oldChild">The node being removed. May not be <c>null</c>.</param>
    /// <param name="keepGrandChildren">true to keep grand children of the node, false otherwise.</param>
    /// <returns>The node removed.</returns>
    public HtmlNode RemoveChild(HtmlNode oldChild, bool keepGrandChildren)
    {
      if (oldChild == null)
        throw new ArgumentNullException(nameof (oldChild));
      if (oldChild._childnodes != null & keepGrandChildren)
      {
        HtmlNode refChild = oldChild.PreviousSibling;
        foreach (HtmlNode childnode in (IEnumerable<HtmlNode>) oldChild._childnodes)
          refChild = this.InsertAfter(childnode, refChild);
      }
      this.RemoveChild(oldChild);
      this.SetChanged();
      return oldChild;
    }

    /// <summary>Replaces the child node oldChild with newChild node.</summary>
    /// <param name="newChild">The new node to put in the child list.</param>
    /// <param name="oldChild">The node being replaced in the list.</param>
    /// <returns>The node replaced.</returns>
    public HtmlNode ReplaceChild(HtmlNode newChild, HtmlNode oldChild)
    {
      if (newChild == null)
        return this.RemoveChild(oldChild);
      if (oldChild == null)
        return this.AppendChild(newChild);
      int index = -1;
      if (this._childnodes != null)
        index = this._childnodes[oldChild];
      if (index == -1)
        throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
      if (this._childnodes != null)
        this._childnodes.Replace(index, newChild);
      this._ownerdocument.SetIdForNode((HtmlNode) null, oldChild.GetId());
      this.RemoveAllIDforNode(oldChild);
      this._ownerdocument.SetIdForNode(newChild, newChild.GetId());
      this.SetChildNodesId(newChild);
      this.SetChanged();
      return newChild;
    }

    /// <summary>
    /// Helper method to set the value of an attribute of this node. If the attribute is not found, it will be created automatically.
    /// </summary>
    /// <param name="name">The name of the attribute to set. May not be null.</param>
    /// <param name="value">The value for the attribute.</param>
    /// <returns>The corresponding attribute instance.</returns>
    public HtmlAttribute SetAttributeValue(string name, string value)
    {
      HtmlAttribute htmlAttribute = name != null ? this.Attributes[name] : throw new ArgumentNullException(nameof (name));
      if (htmlAttribute == null)
        return this.Attributes.Append(this._ownerdocument.CreateAttribute(name, value));
      htmlAttribute.Value = value;
      return htmlAttribute;
    }

    /// <summary>
    /// Saves all the children of the node to the specified TextWriter.
    /// </summary>
    /// <param name="outText">The TextWriter to which you want to save.</param>
    /// <param name="level">Identifies the level we are in starting at root with 0</param>
    public void WriteContentTo(TextWriter outText, int level = 0)
    {
      if (level > HtmlDocument.MaxDepthLevel)
        throw new ArgumentException("The document is too complex to parse");
      if (this._childnodes == null)
        return;
      foreach (HtmlNode childnode in (IEnumerable<HtmlNode>) this._childnodes)
        childnode.WriteTo(outText, level + 1);
    }

    /// <summary>Saves all the children of the node to a string.</summary>
    /// <returns>The saved string.</returns>
    public string WriteContentTo()
    {
      StringWriter stringWriter = new StringWriter();
      this.WriteContentTo((TextWriter) stringWriter);
      stringWriter.Flush();
      return stringWriter.ToString();
    }

    /// <summary>Saves the current node to the specified TextWriter.</summary>
    /// <param name="outText">The TextWriter to which you want to save.</param>
    /// <param name="level">identifies the level we are in starting at root with 0</param>
    public virtual void WriteTo(TextWriter outText, int level = 0)
    {
      switch (this._nodetype)
      {
        case HtmlNodeType.Document:
          if (this._ownerdocument.OptionOutputAsXml)
          {
            outText.Write("<?xml version=\"1.0\" encoding=\"" + this._ownerdocument.GetOutEncoding().BodyName + "\"?>");
            if (this._ownerdocument.DocumentNode.HasChildNodes)
            {
              int count = this._ownerdocument.DocumentNode._childnodes.Count;
              if (count > 0)
              {
                if (this._ownerdocument.GetXmlDeclaration() != null)
                  --count;
                if (count > 1)
                {
                  if (!this._ownerdocument.BackwardCompatibility)
                  {
                    this.WriteContentTo(outText, level);
                    break;
                  }
                  if (this._ownerdocument.OptionOutputUpperCase)
                  {
                    outText.Write("<SPAN>");
                    this.WriteContentTo(outText, level);
                    outText.Write("</SPAN>");
                    break;
                  }
                  outText.Write("<span>");
                  this.WriteContentTo(outText, level);
                  outText.Write("</span>");
                  break;
                }
              }
            }
          }
          this.WriteContentTo(outText, level);
          break;
        case HtmlNodeType.Element:
          string name = this._ownerdocument.OptionOutputUpperCase ? this.Name.ToUpperInvariant() : this.Name;
          if (this._ownerdocument.OptionOutputOriginalCase)
            name = this.OriginalName;
          if (this._ownerdocument.OptionOutputAsXml)
          {
            if (name.Length <= 0 || name[0] == '?' || name.Trim().Length == 0)
              break;
            name = HtmlDocument.GetXmlName(name, false, this._ownerdocument.OptionPreserveXmlNamespaces);
          }
          outText.Write("<" + name);
          this.WriteAttributes(outText, false);
          if (this.HasChildNodes)
          {
            outText.Write(">");
            bool flag = false;
            if (this._ownerdocument.OptionOutputAsXml && HtmlNode.IsCDataElement(this.Name))
            {
              flag = true;
              outText.Write("\r\n//<![CDATA[\r\n");
            }
            if (flag)
            {
              if (this.HasChildNodes)
                this.ChildNodes[0].WriteTo(outText, level);
              outText.Write("\r\n//]]>//\r\n");
            }
            else
              this.WriteContentTo(outText, level);
            if (!this._ownerdocument.OptionOutputAsXml && this._isImplicitEnd)
              break;
            outText.Write("</" + name);
            if (!this._ownerdocument.OptionOutputAsXml)
              this.WriteAttributes(outText, true);
            outText.Write(">");
            break;
          }
          if (HtmlNode.IsEmptyElement(this.Name))
          {
            if (this._ownerdocument.OptionWriteEmptyNodes || this._ownerdocument.OptionOutputAsXml)
            {
              outText.Write(" />");
              break;
            }
            if (this.Name.Length > 0 && this.Name[0] == '?')
              outText.Write("?");
            outText.Write(">");
            break;
          }
          if (!this._isImplicitEnd)
          {
            outText.Write("></" + name + ">");
            break;
          }
          outText.Write(">");
          break;
        case HtmlNodeType.Comment:
          string comment1 = ((HtmlCommentNode) this).Comment;
          if (this._ownerdocument.OptionOutputAsXml)
          {
            HtmlCommentNode comment2 = (HtmlCommentNode) this;
            if (!this._ownerdocument.BackwardCompatibility && comment2.Comment.ToLowerInvariant().StartsWith("<!doctype"))
            {
              outText.Write(comment2.Comment);
              break;
            }
            outText.Write("<!--" + HtmlNode.GetXmlComment(comment2) + " -->");
            break;
          }
          outText.Write(comment1);
          break;
        case HtmlNodeType.Text:
          string text = ((HtmlTextNode) this).Text;
          outText.Write(this._ownerdocument.OptionOutputAsXml ? HtmlDocument.HtmlEncodeWithCompatibility(text, this._ownerdocument.BackwardCompatibility) : text);
          break;
      }
    }

    /// <summary>Saves the current node to the specified XmlWriter.</summary>
    /// <param name="writer">The XmlWriter to which you want to save.</param>
    public void WriteTo(XmlWriter writer)
    {
      switch (this._nodetype)
      {
        case HtmlNodeType.Document:
          writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"" + this._ownerdocument.GetOutEncoding().BodyName + "\"");
          if (!this.HasChildNodes)
            break;
          using (IEnumerator<HtmlNode> enumerator = ((IEnumerable<HtmlNode>) this.ChildNodes).GetEnumerator())
          {
            while (enumerator.MoveNext())
              enumerator.Current.WriteTo(writer);
            break;
          }
        case HtmlNodeType.Element:
          string localName = this._ownerdocument.OptionOutputUpperCase ? this.Name.ToUpperInvariant() : this.Name;
          if (this._ownerdocument.OptionOutputOriginalCase)
            localName = this.OriginalName;
          writer.WriteStartElement(localName);
          HtmlNode.WriteAttributes(writer, this);
          if (this.HasChildNodes)
          {
            foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ChildNodes)
              childNode.WriteTo(writer);
          }
          writer.WriteEndElement();
          break;
        case HtmlNodeType.Comment:
          writer.WriteComment(HtmlNode.GetXmlComment((HtmlCommentNode) this));
          break;
        case HtmlNodeType.Text:
          string text = ((HtmlTextNode) this).Text;
          writer.WriteString(text);
          break;
      }
    }

    /// <summary>Saves the current node to a string.</summary>
    /// <returns>The saved string.</returns>
    public string WriteTo()
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        this.WriteTo((TextWriter) stringWriter);
        stringWriter.Flush();
        return stringWriter.ToString();
      }
    }

    /// <summary>
    /// Sets the parent Html node and properly determines the current node's depth using the parent node's depth.
    /// </summary>
    public void SetParent(HtmlNode parent)
    {
      if (parent == null)
        return;
      this.ParentNode = parent;
      if (this.OwnerDocument.OptionMaxNestedChildNodes <= 0)
        return;
      this.Depth = parent.Depth + 1;
      if (this.Depth > this.OwnerDocument.OptionMaxNestedChildNodes)
        throw new Exception(string.Format("Document has more than {0} nested tags. This is likely due to the page not closing tags properly.", (object) this.OwnerDocument.OptionMaxNestedChildNodes));
    }

    internal void SetChanged()
    {
      this._changed = true;
      if (this.ParentNode == null)
        return;
      this.ParentNode.SetChanged();
    }

    private void UpdateHtml()
    {
      this._innerhtml = this.WriteContentTo();
      this._outerhtml = this.WriteTo();
      this._changed = false;
    }

    internal static string GetXmlComment(HtmlCommentNode comment)
    {
      string comment1 = comment.Comment;
      return comment1.Substring(4, comment1.Length - 7).Replace("--", " - -");
    }

    internal static void WriteAttributes(XmlWriter writer, HtmlNode node)
    {
      if (!node.HasAttributes)
        return;
      foreach (HtmlAttribute htmlAttribute in node.Attributes.Hashitems.Values)
        writer.WriteAttributeString(htmlAttribute.XmlName, htmlAttribute.Value);
    }

    internal void UpdateLastNode()
    {
      HtmlNode htmlNode = (HtmlNode) null;
      if (this._prevwithsamename == null || !this._prevwithsamename._starttag)
      {
        if (this._ownerdocument.Openednodes != null)
        {
          foreach (KeyValuePair<int, HtmlNode> openednode in this._ownerdocument.Openednodes)
          {
            if ((openednode.Key < this._outerstartindex || openednode.Key > this._outerstartindex + this._outerlength) && openednode.Value._name == this._name)
            {
              if (htmlNode == null && openednode.Value._starttag)
                htmlNode = openednode.Value;
              else if (htmlNode != null && htmlNode.InnerStartIndex < openednode.Key && openednode.Value._starttag)
                htmlNode = openednode.Value;
            }
          }
        }
      }
      else
        htmlNode = this._prevwithsamename;
      if (htmlNode == null)
        return;
      this._ownerdocument.Lastnodes[htmlNode.Name] = htmlNode;
    }

    internal void CloseNode(HtmlNode endnode, int level = 0)
    {
      if (level > HtmlDocument.MaxDepthLevel)
        throw new ArgumentException("The document is too complex to parse");
      if (!this._ownerdocument.OptionAutoCloseOnEnd && this._childnodes != null)
      {
        foreach (HtmlNode childnode in (IEnumerable<HtmlNode>) this._childnodes)
        {
          if (!childnode.Closed)
          {
            HtmlNode endnode1 = new HtmlNode(this.NodeType, this._ownerdocument, -1);
            endnode1._endnode = endnode1;
            childnode.CloseNode(endnode1, level + 1);
          }
        }
      }
      if (this.Closed)
        return;
      this._endnode = endnode;
      if (this._ownerdocument.Openednodes != null)
        this._ownerdocument.Openednodes.Remove(this._outerstartindex);
      if (Utilities.GetDictionaryValueOrDefault<string, HtmlNode>(this._ownerdocument.Lastnodes, this.Name) == this)
      {
        this._ownerdocument.Lastnodes.Remove(this.Name);
        this._ownerdocument.UpdateLastParentNode();
        if (this._starttag && !string.IsNullOrEmpty(this.Name))
          this.UpdateLastNode();
      }
      if (endnode == this)
        return;
      this._innerstartindex = this._outerstartindex + this._outerlength;
      this._innerlength = endnode._outerstartindex - this._innerstartindex;
      this._outerlength = endnode._outerstartindex + endnode._outerlength - this._outerstartindex;
    }

    internal string GetId()
    {
      HtmlAttribute attribute = this.Attributes["id"];
      return attribute != null ? attribute.Value : string.Empty;
    }

    internal void SetId(string id)
    {
      HtmlAttribute htmlAttribute = this.Attributes[nameof (id)] ?? this._ownerdocument.CreateAttribute(nameof (id));
      htmlAttribute.Value = id;
      this._ownerdocument.SetIdForNode(this, htmlAttribute.Value);
      this.Attributes[nameof (id)] = htmlAttribute;
      this.SetChanged();
    }

    internal void WriteAttribute(TextWriter outText, HtmlAttribute att)
    {
      if (att.Value == null)
        return;
      string str1 = att.QuoteType == AttributeValueQuote.DoubleQuote ? "\"" : "'";
      if (this._ownerdocument.OptionOutputAsXml)
      {
        string str2 = this._ownerdocument.OptionOutputUpperCase ? att.XmlName.ToUpperInvariant() : att.XmlName;
        if (this._ownerdocument.OptionOutputOriginalCase)
          str2 = att.OriginalName;
        outText.Write(" " + str2 + "=" + str1 + HtmlDocument.HtmlEncodeWithCompatibility(att.XmlValue, this._ownerdocument.BackwardCompatibility) + str1);
      }
      else
      {
        string str2 = this._ownerdocument.OptionOutputUpperCase ? att.Name.ToUpperInvariant() : att.Name;
        if (this._ownerdocument.OptionOutputOriginalCase)
          str2 = att.OriginalName;
        if (att.Name.Length >= 4 && att.Name[0] == '<' && (att.Name[1] == '%' && att.Name[att.Name.Length - 1] == '>') && att.Name[att.Name.Length - 2] == '%')
        {
          outText.Write(" " + str2);
        }
        else
        {
          string str3 = att.QuoteType == AttributeValueQuote.DoubleQuote ? (!att.Value.StartsWith("@") ? att.Value.Replace("\"", "&quot;") : att.Value) : att.Value.Replace("'", "&#39;");
          if (this._ownerdocument.OptionOutputOptimizeAttributeValues)
          {
            if (att.Value.IndexOfAny(new char[4]
            {
              '\n',
              '\r',
              '\t',
              ' '
            }) < 0)
              outText.Write(" " + str2 + "=" + att.Value);
            else
              outText.Write(" " + str2 + "=" + str1 + str3 + str1);
          }
          else
            outText.Write(" " + str2 + "=" + str1 + str3 + str1);
        }
      }
    }

    internal void WriteAttributes(TextWriter outText, bool closing)
    {
      if (this._ownerdocument.OptionOutputAsXml)
      {
        if (this._attributes == null)
          return;
        foreach (HtmlAttribute att in this._attributes.Hashitems.Values)
          this.WriteAttribute(outText, att);
      }
      else if (!closing)
      {
        if (this._attributes != null)
        {
          foreach (HtmlAttribute attribute in (IEnumerable<HtmlAttribute>) this._attributes)
            this.WriteAttribute(outText, attribute);
        }
        if (!this._ownerdocument.OptionAddDebuggingAttributes)
          return;
        this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_closed", this.Closed.ToString()));
        this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_children", this.ChildNodes.Count.ToString()));
        int num = 0;
        foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ChildNodes)
        {
          this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_child_" + num.ToString(), childNode.Name));
          ++num;
        }
      }
      else
      {
        if (this._endnode == null || this._endnode._attributes == null || this._endnode == this)
          return;
        foreach (HtmlAttribute attribute in (IEnumerable<HtmlAttribute>) this._endnode._attributes)
          this.WriteAttribute(outText, attribute);
        if (!this._ownerdocument.OptionAddDebuggingAttributes)
          return;
        this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_closed", this.Closed.ToString()));
        this.WriteAttribute(outText, this._ownerdocument.CreateAttribute("_children", this.ChildNodes.Count.ToString()));
      }
    }

    private string GetRelativeXpath()
    {
      if (this.ParentNode == null)
        return this.Name;
      if (this.NodeType == HtmlNodeType.Document)
        return string.Empty;
      int num = 1;
      foreach (HtmlNode childNode in (IEnumerable<HtmlNode>) this.ParentNode.ChildNodes)
      {
        if (!(childNode.Name != this.Name))
        {
          if (childNode != this)
            ++num;
          else
            break;
        }
      }
      return this.Name + "[" + num.ToString() + "]";
    }

    private bool IsSingleElementNode()
    {
      int num = 0;
      for (HtmlNode htmlNode = this.FirstChild; htmlNode != null; htmlNode = htmlNode.NextSibling)
      {
        if (htmlNode.NodeType == HtmlNodeType.Element && htmlNode.OuterHtml != "\r\n")
          ++num;
      }
      return num <= 1;
    }

    /// <summary>Adds one or more classes to this node.</summary>
    /// <param name="name">The node list to add. May not be null.</param>
    public void AddClass(string name) => this.AddClass(name, false);

    /// <summary>Adds one or more classes to this node.</summary>
    /// <param name="name">The node list to add. May not be null.</param>
    /// <param name="throwError">true to throw Error if class name exists, false otherwise.</param>
    public void AddClass(string name, bool throwError)
    {
      IEnumerable<HtmlAttribute> htmlAttributes = this.Attributes.AttributesWithName("class");
      if (!this.IsEmpty((IEnumerable) htmlAttributes))
      {
        foreach (HtmlAttribute htmlAttribute in htmlAttributes)
        {
          if (htmlAttribute.Value != null)
          {
            if (((IEnumerable<string>) htmlAttribute.Value.Split(' ')).ToList<string>().Any<string>((Func<string, bool>) (x => x.Equals(name))))
            {
              if (throwError)
                throw new Exception(HtmlDocument.HtmlExceptionClassExists);
              continue;
            }
          }
          this.SetAttributeValue(htmlAttribute.Name, htmlAttribute.Value + " " + name);
        }
      }
      else
        this.Attributes.Append(this._ownerdocument.CreateAttribute("class", name));
    }

    /// <summary>Removes the class attribute from the node.</summary>
    public void RemoveClass() => this.RemoveClass(false);

    /// <summary>Removes the class attribute from the node.</summary>
    /// <param name="throwError">true to throw Error if class name doesn't exist, false otherwise.</param>
    public void RemoveClass(bool throwError)
    {
      IEnumerable<HtmlAttribute> htmlAttributes = this.Attributes.AttributesWithName("class");
      if (this.IsEmpty((IEnumerable) htmlAttributes) & throwError)
        throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
      foreach (HtmlAttribute attribute in htmlAttributes)
        this.Attributes.Remove(attribute);
    }

    /// <summary>Removes the specified class from the node.</summary>
    /// <param name="name">The class being removed. May not be <c>null</c>.</param>
    public void RemoveClass(string name) => this.RemoveClass(name, false);

    /// <summary>Removes the specified class from the node.</summary>
    /// <param name="name">The class being removed. May not be <c>null</c>.</param>
    /// <param name="throwError">true to throw Error if class name doesn't exist, false otherwise.</param>
    public void RemoveClass(string name, bool throwError)
    {
      IEnumerable<HtmlAttribute> htmlAttributes = this.Attributes.AttributesWithName("class");
      if (this.IsEmpty((IEnumerable) htmlAttributes) & throwError)
        throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
      foreach (HtmlAttribute attribute in htmlAttributes)
      {
        if (attribute.Value != null)
        {
          if (attribute.Value.Equals(name))
          {
            this.Attributes.Remove(attribute);
          }
          else
          {
            if (attribute.Value != null)
            {
              if (((IEnumerable<string>) attribute.Value.Split(' ')).ToList<string>().Any<string>((Func<string, bool>) (x => x.Equals(name))))
              {
                string[] strArray = attribute.Value.Split(' ');
                string str1 = "";
                foreach (string str2 in strArray)
                {
                  if (!str2.Equals(name))
                    str1 = str1 + str2 + " ";
                }
                string str3 = str1.Trim();
                this.SetAttributeValue(attribute.Name, str3);
                goto label_17;
              }
            }
            if (throwError)
              throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
          }
label_17:
          if (string.IsNullOrEmpty(attribute.Value))
            this.Attributes.Remove(attribute);
        }
      }
    }

    /// <summary>Replaces the class name oldClass with newClass name.</summary>
    /// <param name="newClass">The new class name.</param>
    /// <param name="oldClass">The class being replaced.</param>
    public void ReplaceClass(string newClass, string oldClass) => this.ReplaceClass(newClass, oldClass, false);

    /// <summary>Replaces the class name oldClass with newClass name.</summary>
    /// <param name="newClass">The new class name.</param>
    /// <param name="oldClass">The class being replaced.</param>
    /// <param name="throwError">true to throw Error if class name doesn't exist, false otherwise.</param>
    public void ReplaceClass(string newClass, string oldClass, bool throwError)
    {
      if (string.IsNullOrEmpty(newClass))
        this.RemoveClass(oldClass);
      if (string.IsNullOrEmpty(oldClass))
        this.AddClass(newClass);
      IEnumerable<HtmlAttribute> htmlAttributes = this.Attributes.AttributesWithName("class");
      if (this.IsEmpty((IEnumerable) htmlAttributes) & throwError)
        throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
      foreach (HtmlAttribute htmlAttribute in htmlAttributes)
      {
        if (htmlAttribute.Value != null)
        {
          if (htmlAttribute.Value.Equals(oldClass) || htmlAttribute.Value.Contains(oldClass))
          {
            string str = htmlAttribute.Value.Replace(oldClass, newClass);
            this.SetAttributeValue(htmlAttribute.Name, str);
          }
          else if (throwError)
            throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
        }
      }
    }

    /// <summary>Gets the CSS Class from the node.</summary>
    /// <returns>The CSS Class from the node</returns>
    public IEnumerable<string> GetClasses()
    {
      foreach (HtmlAttribute htmlAttribute in this.Attributes.AttributesWithName("class"))
      {
        string[] strArray = htmlAttribute.Value.Split((char[]) null, StringSplitOptions.RemoveEmptyEntries);
        for (int index = 0; index < strArray.Length; ++index)
          yield return strArray[index];
        strArray = (string[]) null;
      }
    }

    /// <summary>Check if the node class has the parameter class.</summary>
    /// <param name="class">The class.</param>
    /// <returns>True if node class has the parameter class, false if not.</returns>
    public bool HasClass(string className)
    {
      foreach (string str1 in this.GetClasses())
      {
        foreach (string str2 in str1.Split((char[]) null, StringSplitOptions.RemoveEmptyEntries))
        {
          if (str2 == className)
            return true;
        }
      }
      return false;
    }

    private bool IsEmpty(IEnumerable en)
    {
      IEnumerator enumerator = en.GetEnumerator();
      try
      {
        if (enumerator.MoveNext())
        {
          object current = enumerator.Current;
          return false;
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable2)
          disposable2.Dispose();
      }
      return true;
    }

    /// <summary>
    /// Fill an object and go through it's properties and fill them too.
    /// </summary>
    /// <typeparam name="T">Type of object to want to fill. It should have atleast one property that defined XPath.</typeparam>
    /// <returns>Returns an object of type T including Encapsulated data.</returns>
    /// <exception cref="T:System.ArgumentException">Why it's thrown.</exception>
    /// <exception cref="T:System.ArgumentNullException">Why it's thrown.</exception>
    /// <exception cref="T:System.MissingMethodException">Why it's thrown.</exception>
    /// <exception cref="T:HtmlAgilityPack.MissingXPathException">Why it's thrown.</exception>
    /// <exception cref="T:System.Xml.XPath.XPathException">Why it's thrown.</exception>
    /// <exception cref="T:HtmlAgilityPack.NodeNotFoundException">Why it's thrown.</exception>
    /// <exception cref="T:HtmlAgilityPack.NodeAttributeNotFoundException">Why it's thrown.</exception>
    /// <exception cref="T:System.FormatException">Why it's thrown.</exception>
    /// <exception cref="T:System.Exception">Why it's thrown.</exception>
    public T GetEncapsulatedData<T>() => (T) this.GetEncapsulatedData(typeof (T));

    /// <summary>
    /// Fill an object and go through it's properties and fill them too.
    /// </summary>
    /// <typeparam name="T">Type of object to want to fill. It should have atleast one property that defined XPath.</typeparam>
    /// <param name="htmlDocument">If htmlDocument includes data , leave this parameter null. Else pass your specific htmldocument.</param>
    /// <returns>Returns an object of type T including Encapsulated data.</returns>
    /// <exception cref="T:System.ArgumentException">Why it's thrown.</exception>
    /// <exception cref="T:System.ArgumentNullException">Why it's thrown.</exception>
    /// <exception cref="T:System.MissingMethodException">Why it's thrown.</exception>
    /// <exception cref="T:HtmlAgilityPack.MissingXPathException">Why it's thrown.</exception>
    /// <exception cref="T:System.Xml.XPath.XPathException">Why it's thrown.</exception>
    /// <exception cref="T:HtmlAgilityPack.NodeNotFoundException">Why it's thrown.</exception>
    /// <exception cref="T:HtmlAgilityPack.NodeAttributeNotFoundException">Why it's thrown.</exception>
    /// <exception cref="T:System.FormatException">Why it's thrown.</exception>
    /// <exception cref="T:System.Exception">Why it's thrown.</exception>
    public T GetEncapsulatedData<T>(HtmlDocument htmlDocument) => (T) this.GetEncapsulatedData(typeof (T), htmlDocument);

    /// <summary>
    /// Fill an object and go through it's properties and fill them too.
    /// </summary>
    /// <param name="targetType">Type of object to want to fill. It should have atleast one property that defined XPath.</param>
    /// <param name="htmlDocument">If htmlDocument includes data , leave this parameter null. Else pass your specific htmldocument.</param>
    /// <returns>Returns an object of type targetType including Encapsulated data.</returns>
    /// <exception cref="T:System.ArgumentException">Why it's thrown.</exception>
    /// <exception cref="T:System.ArgumentNullException">Why it's thrown.</exception>
    /// <exception cref="T:System.MissingMethodException">Why it's thrown.</exception>
    /// <exception cref="T:HtmlAgilityPack.MissingXPathException">Why it's thrown.</exception>
    /// <exception cref="T:System.Xml.XPath.XPathException">Why it's thrown.</exception>
    /// <exception cref="T:HtmlAgilityPack.NodeNotFoundException">Why it's thrown.</exception>
    /// <exception cref="T:HtmlAgilityPack.NodeAttributeNotFoundException">Why it's thrown.</exception>
    /// <exception cref="T:System.FormatException">Why it's thrown.</exception>
    /// <exception cref="T:System.Exception">Why it's thrown.</exception>
    public object GetEncapsulatedData(Type targetType, HtmlDocument htmlDocument = null)
    {
      if (targetType == (Type) null)
        throw new ArgumentNullException("Parameter targetType is null");
      HtmlDocument htmlDocument1 = htmlDocument != null ? htmlDocument : this.OwnerDocument;
      object obj1 = targetType.IsInstantiable() ? Activator.CreateInstance(targetType) : throw new MissingMethodException("Parameterless Constructor excpected for " + targetType.FullName);
      IEnumerable<PropertyInfo> source = targetType.IsDefinedAttribute(typeof (HasXPathAttribute)) ? targetType.GetPropertiesDefinedXPath() : throw new MissingXPathException("Type T must define HasXPath attribute and include properties with XPath attribute.");
      if (source.CountOfIEnumerable<PropertyInfo>() == 0)
        throw new MissingXPathException("Type " + targetType.FullName + " defined HasXPath Attribute but it does not have any property with XPath Attribte.");
      foreach (PropertyInfo propertyInfo in source)
      {
        XPathAttribute customAttribute = propertyInfo.GetCustomAttributes(typeof (XPathAttribute), false)[0] as XPathAttribute;
        if (!propertyInfo.IsIEnumerable())
        {
          HtmlNode htmlNode;
          try
          {
            htmlNode = htmlDocument1.DocumentNode.SelectSingleNode(customAttribute.XPath);
          }
          catch (XPathException ex)
          {
            throw new XPathException(ex.Message + " That means you have a syntax error in XPath property of this Property : " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
          }
          catch (Exception ex)
          {
            throw new NodeNotFoundException("Cannot find node with giving XPath to bind to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name, ex);
          }
          if (htmlNode == null)
          {
            if (!propertyInfo.IsDefined(typeof (SkipNodeNotFoundAttribute), false))
              throw new NodeNotFoundException("Cannot find node with giving XPath to bind to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
          }
          else if (propertyInfo.PropertyType.IsDefinedAttribute(typeof (HasXPathAttribute)))
          {
            HtmlDocument htmlDocument2 = new HtmlDocument();
            htmlDocument2.LoadHtml(htmlNode.InnerHtml);
            object encapsulatedData = this.GetEncapsulatedData(propertyInfo.PropertyType, htmlDocument2);
            propertyInfo.SetValue(obj1, encapsulatedData, (object[]) null);
          }
          else
          {
            string empty = string.Empty;
            string str = customAttribute.AttributeName != null ? htmlNode.GetAttributeValue(customAttribute.AttributeName, (string) null) : Tools.GetNodeValueBasedOnXPathReturnType<string>(htmlNode, customAttribute);
            if (str == null)
              throw new NodeAttributeNotFoundException("Can not find " + customAttribute.AttributeName + " Attribute in " + htmlNode.Name + " related to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
            object obj2;
            try
            {
              obj2 = Convert.ChangeType((object) str, propertyInfo.PropertyType);
            }
            catch (FormatException ex)
            {
              throw new FormatException("Can not convert Invalid string to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
            }
            catch (Exception ex)
            {
              throw new Exception("Unhandled Exception : " + ex.Message);
            }
            propertyInfo.SetValue(obj1, obj2, (object[]) null);
          }
        }
        else
        {
          if (!(propertyInfo.GetGenericTypes() is IList<Type> genericTypes4) || genericTypes4.Count == 0)
            throw new ArgumentException(propertyInfo.Name + " should have one generic argument.");
          if (genericTypes4.Count > 1)
            throw new ArgumentException(propertyInfo.Name + " should have one generic argument.");
          if (genericTypes4.Count == 1)
          {
            HtmlNodeCollection htmlNodeCollection;
            try
            {
              htmlNodeCollection = htmlDocument1.DocumentNode.SelectNodes(customAttribute.XPath);
            }
            catch (XPathException ex)
            {
              throw new XPathException(ex.Message + " That means you have a syntax error in XPath property of this Property : " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
            }
            catch (Exception ex)
            {
              throw new NodeNotFoundException("Cannot find node with giving XPath to bind to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name, ex);
            }
            if (htmlNodeCollection == null || htmlNodeCollection.Count == 0)
            {
              if (!propertyInfo.IsDefined(typeof (SkipNodeNotFoundAttribute), false))
                throw new NodeNotFoundException("Cannot find node with giving XPath to bind to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
            }
            else
            {
              IList list = genericTypes4[0].CreateIListOfType();
              if (genericTypes4[0].IsDefinedAttribute(typeof (HasXPathAttribute)))
              {
                foreach (HtmlNode htmlNode in (IEnumerable<HtmlNode>) htmlNodeCollection)
                {
                  HtmlDocument htmlDocument2 = new HtmlDocument();
                  htmlDocument2.LoadHtml(htmlNode.InnerHtml);
                  object encapsulatedData = this.GetEncapsulatedData(genericTypes4[0], htmlDocument2);
                  list.Add(encapsulatedData);
                }
              }
              else if (customAttribute.AttributeName == null)
              {
                try
                {
                  list = Tools.GetNodesValuesBasedOnXPathReturnType(htmlNodeCollection, customAttribute, genericTypes4[0]);
                }
                catch (FormatException ex)
                {
                  throw new FormatException("Can not convert Invalid string in node collection to " + genericTypes4[0].FullName + " " + propertyInfo.Name);
                }
                catch (Exception ex)
                {
                  throw new Exception("Unhandled Exception : " + ex.Message);
                }
              }
              else
              {
                foreach (HtmlNode htmlNode in (IEnumerable<HtmlNode>) htmlNodeCollection)
                {
                  string attributeValue = htmlNode.GetAttributeValue(customAttribute.AttributeName, (string) null);
                  if (attributeValue == null)
                    throw new NodeAttributeNotFoundException("Can not find " + customAttribute.AttributeName + " Attribute in " + htmlNode.Name + " related to " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name);
                  object obj2;
                  try
                  {
                    obj2 = Convert.ChangeType((object) attributeValue, genericTypes4[0]);
                  }
                  catch (FormatException ex)
                  {
                    throw new FormatException("Can not convert Invalid string to " + genericTypes4[0].FullName + " " + propertyInfo.Name);
                  }
                  catch (Exception ex)
                  {
                    throw new Exception("Unhandled Exception : " + ex.Message);
                  }
                  list.Add(obj2);
                }
              }
              if (list == null || list.Count == 0)
                throw new Exception("Cannot fill " + propertyInfo.PropertyType.FullName + " " + propertyInfo.Name + " because it is null.");
              propertyInfo.SetValue(obj1, (object) list, (object[]) null);
            }
          }
        }
      }
      return obj1;
    }

    /// <summary>
    /// Creates a new XPathNavigator object for navigating this HTML node.
    /// </summary>
    /// <returns>An XPathNavigator object. The XPathNavigator is positioned on the node from which the method was called. It is not positioned on the root of the document.</returns>
    public XPathNavigator CreateNavigator() => (XPathNavigator) new HtmlNodeNavigator(this.OwnerDocument, this);

    /// <summary>
    /// Creates an XPathNavigator using the root of this document.
    /// </summary>
    /// <returns></returns>
    public XPathNavigator CreateRootNavigator() => (XPathNavigator) new HtmlNodeNavigator(this.OwnerDocument, this.OwnerDocument.DocumentNode);

    /// <summary>
    /// Selects a list of nodes matching the <see cref="P:HtmlAgilityPack.HtmlNode.XPath" /> expression.
    /// </summary>
    /// <param name="xpath">The XPath expression.</param>
    /// <returns>An <see cref="T:HtmlAgilityPack.HtmlNodeCollection" /> containing a collection of nodes matching the <see cref="P:HtmlAgilityPack.HtmlNode.XPath" /> query, or <c>null</c> if no node matched the XPath expression.</returns>
    public HtmlNodeCollection SelectNodes(string xpath)
    {
      HtmlNodeCollection htmlNodeCollection = new HtmlNodeCollection((HtmlNode) null);
      XPathNodeIterator xpathNodeIterator = new HtmlNodeNavigator(this.OwnerDocument, this).Select(xpath);
      while (xpathNodeIterator.MoveNext())
      {
        HtmlNodeNavigator current = (HtmlNodeNavigator) xpathNodeIterator.Current;
        htmlNodeCollection.Add(current.CurrentNode, false);
      }
      return htmlNodeCollection.Count == 0 && !this.OwnerDocument.OptionEmptyCollection ? (HtmlNodeCollection) null : htmlNodeCollection;
    }

    /// <summary>
    /// Selects a list of nodes matching the <see cref="P:HtmlAgilityPack.HtmlNode.XPath" /> expression.
    /// </summary>
    /// <param name="xpath">The XPath expression.</param>
    /// <returns>An <see cref="T:HtmlAgilityPack.HtmlNodeCollection" /> containing a collection of nodes matching the <see cref="P:HtmlAgilityPack.HtmlNode.XPath" /> query, or <c>null</c> if no node matched the XPath expression.</returns>
    public HtmlNodeCollection SelectNodes(XPathExpression xpath)
    {
      HtmlNodeCollection htmlNodeCollection = new HtmlNodeCollection((HtmlNode) null);
      XPathNodeIterator xpathNodeIterator = new HtmlNodeNavigator(this.OwnerDocument, this).Select(xpath);
      while (xpathNodeIterator.MoveNext())
      {
        HtmlNodeNavigator current = (HtmlNodeNavigator) xpathNodeIterator.Current;
        htmlNodeCollection.Add(current.CurrentNode, false);
      }
      return htmlNodeCollection.Count == 0 && !this.OwnerDocument.OptionEmptyCollection ? (HtmlNodeCollection) null : htmlNodeCollection;
    }

    /// <summary>
    /// Selects the first XmlNode that matches the XPath expression.
    /// </summary>
    /// <param name="xpath">The XPath expression. May not be null.</param>
    /// <returns>The first <see cref="T:HtmlAgilityPack.HtmlNode" /> that matches the XPath query or a null reference if no matching node was found.</returns>
    public HtmlNode SelectSingleNode(string xpath)
    {
      XPathNodeIterator xpathNodeIterator = xpath != null ? new HtmlNodeNavigator(this.OwnerDocument, this).Select(xpath) : throw new ArgumentNullException(nameof (xpath));
      return !xpathNodeIterator.MoveNext() ? (HtmlNode) null : ((HtmlNodeNavigator) xpathNodeIterator.Current).CurrentNode;
    }

    /// <summary>
    /// Selects a list of nodes matching the <see cref="P:HtmlAgilityPack.HtmlNode.XPath" /> expression.
    /// </summary>
    /// <param name="xpath">The XPath expression.</param>
    /// <returns>An <see cref="T:HtmlAgilityPack.HtmlNodeCollection" /> containing a collection of nodes matching the <see cref="P:HtmlAgilityPack.HtmlNode.XPath" /> query, or <c>null</c> if no node matched the XPath expression.</returns>
    public HtmlNode SelectSingleNode(XPathExpression xpath)
    {
      XPathNodeIterator xpathNodeIterator = xpath != null ? new HtmlNodeNavigator(this.OwnerDocument, this).Select(xpath) : throw new ArgumentNullException(nameof (xpath));
      return !xpathNodeIterator.MoveNext() ? (HtmlNode) null : ((HtmlNodeNavigator) xpathNodeIterator.Current).CurrentNode;
    }
  }
}
