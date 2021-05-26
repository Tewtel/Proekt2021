// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlDocument
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace HtmlAgilityPack
{
  /// <summary>Represents a complete HTML document.</summary>
  public class HtmlDocument : IXPathNavigable
  {
    internal static bool _disableBehaviorTagP = true;
    /// <summary>
    /// Defines the max level we would go deep into the html document
    /// </summary>
    private static int _maxDepthLevel = int.MaxValue;
    private int _c;
    private Crc32 _crc32;
    private HtmlAttribute _currentattribute;
    private HtmlNode _currentnode;
    private Encoding _declaredencoding;
    private HtmlNode _documentnode;
    private bool _fullcomment;
    private int _index;
    internal Dictionary<string, HtmlNode> Lastnodes = new Dictionary<string, HtmlNode>();
    private HtmlNode _lastparentnode;
    private int _line;
    private int _lineposition;
    private int _maxlineposition;
    internal Dictionary<string, HtmlNode> Nodesid;
    private HtmlDocument.ParseState _oldstate;
    private bool _onlyDetectEncoding;
    internal Dictionary<int, HtmlNode> Openednodes;
    private List<HtmlParseError> _parseerrors = new List<HtmlParseError>();
    private string _remainder;
    private int _remainderOffset;
    private HtmlDocument.ParseState _state;
    private Encoding _streamencoding;
    private bool _useHtmlEncodingForStream;
    /// <summary>The HtmlDocument Text. Careful if you modify it.</summary>
    public string Text;
    /// <summary>True to stay backward compatible with previous version of HAP. This option does not guarantee 100% compatibility.</summary>
    public bool BackwardCompatibility = true;
    /// <summary>Adds Debugging attributes to node. Default is false.</summary>
    public bool OptionAddDebuggingAttributes;
    /// <summary>
    /// Defines if closing for non closed nodes must be done at the end or directly in the document.
    /// Setting this to true can actually change how browsers render the page. Default is false.
    /// </summary>
    public bool OptionAutoCloseOnEnd;
    /// <summary>
    /// Defines if non closed nodes will be checked at the end of parsing. Default is true.
    /// </summary>
    public bool OptionCheckSyntax = true;
    /// <summary>
    /// Defines if a checksum must be computed for the document while parsing. Default is false.
    /// </summary>
    public bool OptionComputeChecksum;
    /// <summary>
    /// Defines if SelectNodes method will return null or empty collection when no node matched the XPath expression.
    /// Setting this to true will return empty collection and false will return null. Default is false.
    /// </summary>
    public bool OptionEmptyCollection;
    /// <summary>True to disable, false to enable the server side code.</summary>
    public bool DisableServerSideCode;
    /// <summary>
    /// Defines the default stream encoding to use. Default is System.Text.Encoding.Default.
    /// </summary>
    public Encoding OptionDefaultStreamEncoding;
    /// <summary>
    /// Defines if source text must be extracted while parsing errors.
    /// If the document has a lot of errors, or cascading errors, parsing performance can be dramatically affected if set to true.
    /// Default is false.
    /// </summary>
    public bool OptionExtractErrorSourceText;
    /// <summary>
    /// Defines the maximum length of source text or parse errors. Default is 100.
    /// </summary>
    public int OptionExtractErrorSourceTextMaxLength = 100;
    /// <summary>
    /// Defines if LI, TR, TH, TD tags must be partially fixed when nesting errors are detected. Default is false.
    /// </summary>
    public bool OptionFixNestedTags;
    /// <summary>
    /// Defines if output must conform to XML, instead of HTML. Default is false.
    /// </summary>
    public bool OptionOutputAsXml;
    /// <summary>
    /// If used together with <see cref="F:HtmlAgilityPack.HtmlDocument.OptionOutputAsXml" /> and enabled, Xml namespaces in element names are preserved. Default is false.
    /// </summary>
    public bool OptionPreserveXmlNamespaces;
    /// <summary>
    /// Defines if attribute value output must be optimized (not bound with double quotes if it is possible). Default is false.
    /// </summary>
    public bool OptionOutputOptimizeAttributeValues;
    /// <summary>
    /// Defines if name must be output with it's original case. Useful for asp.net tags and attributes. Default is false.
    /// </summary>
    public bool OptionOutputOriginalCase;
    /// <summary>
    /// Defines if name must be output in uppercase. Default is false.
    /// </summary>
    public bool OptionOutputUpperCase;
    /// <summary>
    /// Defines if declared encoding must be read from the document.
    /// Declared encoding is determined using the meta http-equiv="content-type" content="text/html;charset=XXXXX" html node.
    /// Default is true.
    /// </summary>
    public bool OptionReadEncoding = true;
    /// <summary>
    /// Defines the name of a node that will throw the StopperNodeException when found as an end node. Default is null.
    /// </summary>
    public string OptionStopperNodeName;
    /// <summary>
    /// Defines if the 'id' attribute must be specifically used. Default is true.
    /// </summary>
    public bool OptionUseIdAttribute = true;
    /// <summary>
    /// Defines if empty nodes must be written as closed during output. Default is false.
    /// </summary>
    public bool OptionWriteEmptyNodes;
    /// <summary>
    /// The max number of nested child nodes.
    /// Added to prevent stackoverflow problem when a page has tens of thousands of opening html tags with no closing tags
    /// </summary>
    public int OptionMaxNestedChildNodes;
    internal static readonly string HtmlExceptionRefNotChild = "Reference node must be a child of this node";
    internal static readonly string HtmlExceptionUseIdAttributeFalse = "You need to set UseIdAttribute property to true to enable this feature";
    internal static readonly string HtmlExceptionClassDoesNotExist = "Class name doesn't exist";
    internal static readonly string HtmlExceptionClassExists = "Class name already exists";
    internal static readonly Dictionary<string, string[]> HtmlResetters = new Dictionary<string, string[]>()
    {
      {
        "li",
        new string[2]{ "ul", "ol" }
      },
      {
        "tr",
        new string[1]{ "table" }
      },
      {
        "th",
        new string[2]{ "tr", "table" }
      },
      {
        "td",
        new string[2]{ "tr", "table" }
      }
    };
    private static List<string> BlockAttributes = new List<string>()
    {
      "\"",
      "'"
    };

    /// <summary>True to disable, false to enable the behavior tag p.</summary>
    public static bool DisableBehaviorTagP
    {
      get => HtmlDocument._disableBehaviorTagP;
      set
      {
        if (value)
        {
          if (HtmlNode.ElementsFlags.ContainsKey("p"))
            HtmlNode.ElementsFlags.Remove("p");
        }
        else if (!HtmlNode.ElementsFlags.ContainsKey("p"))
          HtmlNode.ElementsFlags.Add("p", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
        HtmlDocument._disableBehaviorTagP = value;
      }
    }

    /// <summary>Default builder to use in the HtmlDocument constructor</summary>
    public static Action<HtmlDocument> DefaultBuilder { get; set; }

    /// <summary>Action to execute before the Parse is executed</summary>
    public Action<HtmlDocument> ParseExecuting { get; set; }

    /// <summary>Creates an instance of an HTML document.</summary>
    public HtmlDocument()
    {
      if (HtmlDocument.DefaultBuilder != null)
        HtmlDocument.DefaultBuilder(this);
      this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
      this.OptionDefaultStreamEncoding = Encoding.Default;
    }

    /// <summary>Gets the parsed text.</summary>
    /// <value>The parsed text.</value>
    public string ParsedText => this.Text;

    /// <summary>
    /// Defines the max level we would go deep into the html document. If this depth level is exceeded, and exception is
    /// thrown.
    /// </summary>
    public static int MaxDepthLevel
    {
      get => HtmlDocument._maxDepthLevel;
      set => HtmlDocument._maxDepthLevel = value;
    }

    /// <summary>
    /// Gets the document CRC32 checksum if OptionComputeChecksum was set to true before parsing, 0 otherwise.
    /// </summary>
    public int CheckSum => this._crc32 != null ? (int) this._crc32.CheckSum : 0;

    /// <summary>
    /// Gets the document's declared encoding.
    /// Declared encoding is determined using the meta http-equiv="content-type" content="text/html;charset=XXXXX" html node (pre-HTML5) or the meta charset="XXXXX" html node (HTML5).
    /// </summary>
    public Encoding DeclaredEncoding => this._declaredencoding;

    /// <summary>Gets the root node of the document.</summary>
    public HtmlNode DocumentNode => this._documentnode;

    /// <summary>Gets the document's output encoding.</summary>
    public Encoding Encoding => this.GetOutEncoding();

    /// <summary>Gets a list of parse errors found in the document.</summary>
    public IEnumerable<HtmlParseError> ParseErrors => (IEnumerable<HtmlParseError>) this._parseerrors;

    /// <summary>
    /// Gets the remaining text.
    /// Will always be null if OptionStopperNodeName is null.
    /// </summary>
    public string Remainder => this._remainder;

    /// <summary>
    /// Gets the offset of Remainder in the original Html text.
    /// If OptionStopperNodeName is null, this will return the length of the original Html text.
    /// </summary>
    public int RemainderOffset => this._remainderOffset;

    /// <summary>Gets the document's stream encoding.</summary>
    public Encoding StreamEncoding => this._streamencoding;

    /// <summary>Gets a valid XML name.</summary>
    /// <param name="name">Any text.</param>
    /// <returns>A string that is a valid XML name.</returns>
    public static string GetXmlName(string name) => HtmlDocument.GetXmlName(name, false, false);

    public void UseAttributeOriginalName(string tagName)
    {
      foreach (HtmlNode selectNode in (IEnumerable<HtmlNode>) this.DocumentNode.SelectNodes("//" + tagName))
      {
        foreach (HtmlAttribute attribute in (IEnumerable<HtmlAttribute>) selectNode.Attributes)
          attribute.UseOriginalName = true;
      }
    }

    public static string GetXmlName(string name, bool isAttribute, bool preserveXmlNamespaces)
    {
      string empty = string.Empty;
      bool flag = true;
      for (int index = 0; index < name.Length; ++index)
      {
        if (name[index] >= 'a' && name[index] <= 'z' || name[index] >= 'A' && name[index] <= 'Z' || (name[index] >= '0' && name[index] <= '9' || isAttribute | preserveXmlNamespaces && name[index] == ':') || (name[index] == '_' || name[index] == '-' || name[index] == '.'))
        {
          empty += name[index].ToString();
        }
        else
        {
          flag = false;
          Encoding utF8 = Encoding.UTF8;
          char[] chars = new char[1]{ name[index] };
          foreach (byte num in utF8.GetBytes(chars))
            empty += num.ToString("x2");
          empty += "_";
        }
      }
      return flag ? empty : "_" + empty;
    }

    /// <summary>Applies HTML encoding to a specified string.</summary>
    /// <param name="html">The input string to encode. May not be null.</param>
    /// <returns>The encoded string.</returns>
    public static string HtmlEncode(string html) => HtmlDocument.HtmlEncodeWithCompatibility(html);

    internal static string HtmlEncodeWithCompatibility(string html, bool backwardCompatibility = true)
    {
      if (html == null)
        throw new ArgumentNullException(nameof (html));
      return (backwardCompatibility ? new Regex("&(?!(amp;)|(lt;)|(gt;)|(quot;))", RegexOptions.IgnoreCase) : new Regex("&(?!(amp;)|(lt;)|(gt;)|(quot;)|(nbsp;)|(reg;))", RegexOptions.IgnoreCase)).Replace(html, "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
    }

    /// <summary>
    /// Determines if the specified character is considered as a whitespace character.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <returns>true if if the specified character is considered as a whitespace character.</returns>
    public static bool IsWhiteSpace(int c) => c == 10 || c == 13 || (c == 32 || c == 9);

    /// <summary>Creates an HTML attribute with the specified name.</summary>
    /// <param name="name">The name of the attribute. May not be null.</param>
    /// <returns>The new HTML attribute.</returns>
    public HtmlAttribute CreateAttribute(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      HtmlAttribute attribute = this.CreateAttribute();
      attribute.Name = name;
      return attribute;
    }

    /// <summary>Creates an HTML attribute with the specified name.</summary>
    /// <param name="name">The name of the attribute. May not be null.</param>
    /// <param name="value">The value of the attribute.</param>
    /// <returns>The new HTML attribute.</returns>
    public HtmlAttribute CreateAttribute(string name, string value)
    {
      HtmlAttribute htmlAttribute = name != null ? this.CreateAttribute(name) : throw new ArgumentNullException(nameof (name));
      htmlAttribute.Value = value;
      return htmlAttribute;
    }

    /// <summary>Creates an HTML comment node.</summary>
    /// <returns>The new HTML comment node.</returns>
    public HtmlCommentNode CreateComment() => (HtmlCommentNode) this.CreateNode(HtmlNodeType.Comment);

    /// <summary>
    /// Creates an HTML comment node with the specified comment text.
    /// </summary>
    /// <param name="comment">The comment text. May not be null.</param>
    /// <returns>The new HTML comment node.</returns>
    public HtmlCommentNode CreateComment(string comment)
    {
      if (comment == null)
        throw new ArgumentNullException(nameof (comment));
      HtmlCommentNode comment1 = this.CreateComment();
      comment1.Comment = comment;
      return comment1;
    }

    /// <summary>Creates an HTML element node with the specified name.</summary>
    /// <param name="name">The qualified name of the element. May not be null.</param>
    /// <returns>The new HTML node.</returns>
    public HtmlNode CreateElement(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      HtmlNode node = this.CreateNode(HtmlNodeType.Element);
      node.Name = name;
      return node;
    }

    /// <summary>Creates an HTML text node.</summary>
    /// <returns>The new HTML text node.</returns>
    public HtmlTextNode CreateTextNode() => (HtmlTextNode) this.CreateNode(HtmlNodeType.Text);

    /// <summary>Creates an HTML text node with the specified text.</summary>
    /// <param name="text">The text of the node. May not be null.</param>
    /// <returns>The new HTML text node.</returns>
    public HtmlTextNode CreateTextNode(string text)
    {
      if (text == null)
        throw new ArgumentNullException(nameof (text));
      HtmlTextNode textNode = this.CreateTextNode();
      textNode.Text = text;
      return textNode;
    }

    /// <summary>Detects the encoding of an HTML stream.</summary>
    /// <param name="stream">The input stream. May not be null.</param>
    /// <returns>The detected encoding.</returns>
    public Encoding DetectEncoding(Stream stream) => this.DetectEncoding(stream, false);

    /// <summary>Detects the encoding of an HTML stream.</summary>
    /// <param name="stream">The input stream. May not be null.</param>
    /// <param name="checkHtml">The html is checked.</param>
    /// <returns>The detected encoding.</returns>
    public Encoding DetectEncoding(Stream stream, bool checkHtml)
    {
      this._useHtmlEncodingForStream = checkHtml;
      return stream != null ? this.DetectEncoding((TextReader) new StreamReader(stream)) : throw new ArgumentNullException(nameof (stream));
    }

    /// <summary>
    /// Detects the encoding of an HTML text provided on a TextReader.
    /// </summary>
    /// <param name="reader">The TextReader used to feed the HTML. May not be null.</param>
    /// <returns>The detected encoding.</returns>
    public Encoding DetectEncoding(TextReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      this._onlyDetectEncoding = true;
      this.Openednodes = !this.OptionCheckSyntax ? (Dictionary<int, HtmlNode>) null : new Dictionary<int, HtmlNode>();
      this.Nodesid = !this.OptionUseIdAttribute ? (Dictionary<string, HtmlNode>) null : new Dictionary<string, HtmlNode>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      if (reader is StreamReader streamReader && !this._useHtmlEncodingForStream)
      {
        this.Text = streamReader.ReadToEnd();
        this._streamencoding = streamReader.CurrentEncoding;
        return this._streamencoding;
      }
      this._streamencoding = (Encoding) null;
      this._declaredencoding = (Encoding) null;
      this.Text = reader.ReadToEnd();
      this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
      try
      {
        this.Parse();
      }
      catch (EncodingFoundException ex)
      {
        return ex.Encoding;
      }
      return this._streamencoding;
    }

    /// <summary>Detects the encoding of an HTML text.</summary>
    /// <param name="html">The input html text. May not be null.</param>
    /// <returns>The detected encoding.</returns>
    public Encoding DetectEncodingHtml(string html)
    {
      if (html == null)
        throw new ArgumentNullException(nameof (html));
      using (StringReader stringReader = new StringReader(html))
        return this.DetectEncoding((TextReader) stringReader);
    }

    /// <summary>
    /// Gets the HTML node with the specified 'id' attribute value.
    /// </summary>
    /// <param name="id">The attribute id to match. May not be null.</param>
    /// <returns>The HTML node with the matching id or null if not found.</returns>
    public HtmlNode GetElementbyId(string id)
    {
      if (id == null)
        throw new ArgumentNullException(nameof (id));
      if (this.Nodesid == null)
        throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
      return !this.Nodesid.ContainsKey(id) ? (HtmlNode) null : this.Nodesid[id];
    }

    /// <summary>Loads an HTML document from a stream.</summary>
    /// <param name="stream">The input stream.</param>
    public void Load(Stream stream) => this.Load((TextReader) new StreamReader(stream, this.OptionDefaultStreamEncoding));

    /// <summary>Loads an HTML document from a stream.</summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
    public void Load(Stream stream, bool detectEncodingFromByteOrderMarks) => this.Load((TextReader) new StreamReader(stream, detectEncodingFromByteOrderMarks));

    /// <summary>Loads an HTML document from a stream.</summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    public void Load(Stream stream, Encoding encoding) => this.Load((TextReader) new StreamReader(stream, encoding));

    /// <summary>Loads an HTML document from a stream.</summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
    public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks) => this.Load((TextReader) new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));

    /// <summary>Loads an HTML document from a stream.</summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
    /// <param name="buffersize">The minimum buffer size.</param>
    public void Load(
      Stream stream,
      Encoding encoding,
      bool detectEncodingFromByteOrderMarks,
      int buffersize)
    {
      this.Load((TextReader) new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
    }

    /// <summary>
    /// Loads the HTML document from the specified TextReader.
    /// </summary>
    /// <param name="reader">The TextReader used to feed the HTML data into the document. May not be null.</param>
    public void Load(TextReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      this._onlyDetectEncoding = false;
      this.Openednodes = !this.OptionCheckSyntax ? (Dictionary<int, HtmlNode>) null : new Dictionary<int, HtmlNode>();
      this.Nodesid = !this.OptionUseIdAttribute ? (Dictionary<string, HtmlNode>) null : new Dictionary<string, HtmlNode>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      if (reader is StreamReader streamReader)
      {
        try
        {
          streamReader.Peek();
        }
        catch (Exception ex)
        {
        }
        this._streamencoding = streamReader.CurrentEncoding;
      }
      else
        this._streamencoding = (Encoding) null;
      this._declaredencoding = (Encoding) null;
      this.Text = reader.ReadToEnd();
      this._documentnode = this.CreateNode(HtmlNodeType.Document, 0);
      this.Parse();
      if (!this.OptionCheckSyntax || this.Openednodes == null)
        return;
      foreach (HtmlNode htmlNode in this.Openednodes.Values)
      {
        if (htmlNode._starttag)
        {
          string sourceText;
          if (this.OptionExtractErrorSourceText)
          {
            sourceText = htmlNode.OuterHtml;
            if (sourceText.Length > this.OptionExtractErrorSourceTextMaxLength)
              sourceText = sourceText.Substring(0, this.OptionExtractErrorSourceTextMaxLength);
          }
          else
            sourceText = string.Empty;
          this.AddError(HtmlParseErrorCode.TagNotClosed, htmlNode._line, htmlNode._lineposition, htmlNode._streamposition, sourceText, "End tag </" + htmlNode.Name + "> was not found");
        }
      }
      this.Openednodes.Clear();
    }

    /// <summary>Loads the HTML document from the specified string.</summary>
    /// <param name="html">String containing the HTML document to load. May not be null.</param>
    public void LoadHtml(string html)
    {
      if (html == null)
        throw new ArgumentNullException(nameof (html));
      using (StringReader stringReader = new StringReader(html))
        this.Load((TextReader) stringReader);
    }

    /// <summary>Saves the HTML document to the specified stream.</summary>
    /// <param name="outStream">The stream to which you want to save.</param>
    public void Save(Stream outStream) => this.Save(new StreamWriter(outStream, this.GetOutEncoding()));

    /// <summary>Saves the HTML document to the specified stream.</summary>
    /// <param name="outStream">The stream to which you want to save. May not be null.</param>
    /// <param name="encoding">The character encoding to use. May not be null.</param>
    public void Save(Stream outStream, Encoding encoding)
    {
      if (outStream == null)
        throw new ArgumentNullException(nameof (outStream));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      this.Save(new StreamWriter(outStream, encoding));
    }

    /// <summary>
    /// Saves the HTML document to the specified StreamWriter.
    /// </summary>
    /// <param name="writer">The StreamWriter to which you want to save.</param>
    public void Save(StreamWriter writer) => this.Save((TextWriter) writer);

    /// <summary>Saves the HTML document to the specified TextWriter.</summary>
    /// <param name="writer">The TextWriter to which you want to save. May not be null.</param>
    public void Save(TextWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      this.DocumentNode.WriteTo(writer);
      writer.Flush();
    }

    /// <summary>Saves the HTML document to the specified XmlWriter.</summary>
    /// <param name="writer">The XmlWriter to which you want to save.</param>
    public void Save(XmlWriter writer)
    {
      this.DocumentNode.WriteTo(writer);
      writer.Flush();
    }

    internal HtmlAttribute CreateAttribute() => new HtmlAttribute(this);

    internal HtmlNode CreateNode(HtmlNodeType type) => this.CreateNode(type, -1);

    internal HtmlNode CreateNode(HtmlNodeType type, int index)
    {
      if (type == HtmlNodeType.Comment)
        return (HtmlNode) new HtmlCommentNode(this, index);
      return type == HtmlNodeType.Text ? (HtmlNode) new HtmlTextNode(this, index) : new HtmlNode(type, this, index);
    }

    internal Encoding GetOutEncoding() => this._declaredencoding ?? this._streamencoding ?? this.OptionDefaultStreamEncoding;

    internal HtmlNode GetXmlDeclaration()
    {
      if (!this._documentnode.HasChildNodes)
        return (HtmlNode) null;
      foreach (HtmlNode childnode in (IEnumerable<HtmlNode>) this._documentnode._childnodes)
      {
        if (childnode.Name == "?xml")
          return childnode;
      }
      return (HtmlNode) null;
    }

    internal void SetIdForNode(HtmlNode node, string id)
    {
      if (!this.OptionUseIdAttribute || this.Nodesid == null || id == null)
        return;
      if (node == null)
        this.Nodesid.Remove(id);
      else
        this.Nodesid[id] = node;
    }

    internal void UpdateLastParentNode()
    {
      do
      {
        if (this._lastparentnode.Closed)
          this._lastparentnode = this._lastparentnode.ParentNode;
      }
      while (this._lastparentnode != null && this._lastparentnode.Closed);
      if (this._lastparentnode != null)
        return;
      this._lastparentnode = this._documentnode;
    }

    private void AddError(
      HtmlParseErrorCode code,
      int line,
      int linePosition,
      int streamPosition,
      string sourceText,
      string reason)
    {
      this._parseerrors.Add(new HtmlParseError(code, line, linePosition, streamPosition, sourceText, reason));
    }

    private void CloseCurrentNode()
    {
      if (this._currentnode.Closed)
        return;
      bool flag = false;
      HtmlNode dictionaryValueOrDefault = Utilities.GetDictionaryValueOrDefault<string, HtmlNode>(this.Lastnodes, this._currentnode.Name);
      if (dictionaryValueOrDefault == null)
      {
        if (HtmlNode.IsClosedElement(this._currentnode.Name))
        {
          this._currentnode.CloseNode(this._currentnode);
          if (this._lastparentnode != null)
          {
            HtmlNode htmlNode1 = (HtmlNode) null;
            Stack<HtmlNode> htmlNodeStack = new Stack<HtmlNode>();
            for (HtmlNode htmlNode2 = this._lastparentnode.LastChild; htmlNode2 != null; htmlNode2 = htmlNode2.PreviousSibling)
            {
              if (htmlNode2.Name == this._currentnode.Name && !htmlNode2.HasChildNodes)
              {
                htmlNode1 = htmlNode2;
                break;
              }
              htmlNodeStack.Push(htmlNode2);
            }
            if (htmlNode1 != null)
            {
              while (htmlNodeStack.Count != 0)
              {
                HtmlNode htmlNode2 = htmlNodeStack.Pop();
                this._lastparentnode.RemoveChild(htmlNode2);
                htmlNode1.AppendChild(htmlNode2);
              }
            }
            else
              this._lastparentnode.AppendChild(this._currentnode);
          }
        }
        else if (HtmlNode.CanOverlapElement(this._currentnode.Name))
        {
          HtmlNode node = this.CreateNode(HtmlNodeType.Text, this._currentnode._outerstartindex);
          node._outerlength = this._currentnode._outerlength;
          ((HtmlTextNode) node).Text = ((HtmlTextNode) node).Text.ToLowerInvariant();
          if (this._lastparentnode != null)
            this._lastparentnode.AppendChild(node);
        }
        else if (HtmlNode.IsEmptyElement(this._currentnode.Name))
        {
          this.AddError(HtmlParseErrorCode.EndTagNotRequired, this._currentnode._line, this._currentnode._lineposition, this._currentnode._streamposition, this._currentnode.OuterHtml, "End tag </" + this._currentnode.Name + "> is not required");
        }
        else
        {
          this.AddError(HtmlParseErrorCode.TagNotOpened, this._currentnode._line, this._currentnode._lineposition, this._currentnode._streamposition, this._currentnode.OuterHtml, "Start tag <" + this._currentnode.Name + "> was not found");
          flag = true;
        }
      }
      else
      {
        if (this.OptionFixNestedTags && this.FindResetterNodes(dictionaryValueOrDefault, this.GetResetters(this._currentnode.Name)))
        {
          this.AddError(HtmlParseErrorCode.EndTagInvalidHere, this._currentnode._line, this._currentnode._lineposition, this._currentnode._streamposition, this._currentnode.OuterHtml, "End tag </" + this._currentnode.Name + "> invalid here");
          flag = true;
        }
        if (!flag)
        {
          this.Lastnodes[this._currentnode.Name] = dictionaryValueOrDefault._prevwithsamename;
          dictionaryValueOrDefault.CloseNode(this._currentnode);
        }
      }
      if (flag || this._lastparentnode == null || HtmlNode.IsClosedElement(this._currentnode.Name) && !this._currentnode._starttag)
        return;
      this.UpdateLastParentNode();
    }

    private string CurrentNodeName() => this.Text.Substring(this._currentnode._namestartindex, this._currentnode._namelength);

    private void DecrementPosition()
    {
      --this._index;
      if (this._lineposition == 0)
      {
        this._lineposition = this._maxlineposition;
        --this._line;
      }
      else
        --this._lineposition;
    }

    private HtmlNode FindResetterNode(HtmlNode node, string name)
    {
      HtmlNode dictionaryValueOrDefault = Utilities.GetDictionaryValueOrDefault<string, HtmlNode>(this.Lastnodes, name);
      if (dictionaryValueOrDefault == null)
        return (HtmlNode) null;
      if (dictionaryValueOrDefault.Closed)
        return (HtmlNode) null;
      return dictionaryValueOrDefault._streamposition < node._streamposition ? (HtmlNode) null : dictionaryValueOrDefault;
    }

    private bool FindResetterNodes(HtmlNode node, string[] names)
    {
      if (names == null)
        return false;
      for (int index = 0; index < names.Length; ++index)
      {
        if (this.FindResetterNode(node, names[index]) != null)
          return true;
      }
      return false;
    }

    private void FixNestedTag(string name, string[] resetters)
    {
      if (resetters == null)
        return;
      HtmlNode dictionaryValueOrDefault = Utilities.GetDictionaryValueOrDefault<string, HtmlNode>(this.Lastnodes, this._currentnode.Name);
      if (dictionaryValueOrDefault == null || this.Lastnodes[name].Closed || this.FindResetterNodes(dictionaryValueOrDefault, resetters))
        return;
      HtmlNode endnode = new HtmlNode(dictionaryValueOrDefault.NodeType, this, -1);
      endnode._endnode = endnode;
      dictionaryValueOrDefault.CloseNode(endnode);
    }

    private void FixNestedTags()
    {
      if (!this._currentnode._starttag)
        return;
      string name = this.CurrentNodeName();
      this.FixNestedTag(name, this.GetResetters(name));
    }

    private string[] GetResetters(string name)
    {
      string[] strArray;
      return !HtmlDocument.HtmlResetters.TryGetValue(name, out strArray) ? (string[]) null : strArray;
    }

    private void IncrementPosition()
    {
      if (this._crc32 != null)
      {
        int crC32 = (int) this._crc32.AddToCRC32(this._c);
      }
      ++this._index;
      this._maxlineposition = this._lineposition;
      if (this._c == 10)
      {
        this._lineposition = 0;
        ++this._line;
      }
      else
        ++this._lineposition;
    }

    private bool IsValidTag()
    {
      if (this._c != 60 || this._index >= this.Text.Length)
        return false;
      return char.IsLetter(this.Text[this._index]) || this.Text[this._index] == '/' || (this.Text[this._index] == '?' || this.Text[this._index] == '!') || this.Text[this._index] == '%';
    }

    private bool NewCheck()
    {
      if (this._c != 60 || !this.IsValidTag())
        return false;
      if (this._index < this.Text.Length && this.Text[this._index] == '%')
      {
        if (this.DisableServerSideCode)
          return false;
        switch (this._state)
        {
          case HtmlDocument.ParseState.WhichTag:
            this.PushNodeNameStart(true, this._index - 1);
            this._state = HtmlDocument.ParseState.Tag;
            break;
          case HtmlDocument.ParseState.BetweenAttributes:
            this.PushAttributeNameStart(this._index - 1, this._lineposition - 1);
            break;
          case HtmlDocument.ParseState.AttributeAfterEquals:
            this.PushAttributeValueStart(this._index - 1);
            break;
        }
        this._oldstate = this._state;
        this._state = HtmlDocument.ParseState.ServerSideCode;
        return true;
      }
      if (!this.PushNodeEnd(this._index - 1, true))
      {
        this._index = this.Text.Length;
        return true;
      }
      this._state = HtmlDocument.ParseState.WhichTag;
      if (this._index - 1 <= this.Text.Length - 2 && (this.Text[this._index] == '!' || this.Text[this._index] == '?'))
      {
        this.PushNodeStart(HtmlNodeType.Comment, this._index - 1, this._lineposition - 1);
        this.PushNodeNameStart(true, this._index);
        this.PushNodeNameEnd(this._index + 1);
        this._state = HtmlDocument.ParseState.Comment;
        if (this._index < this.Text.Length - 2)
          this._fullcomment = this.Text[this._index + 1] == '-' && this.Text[this._index + 2] == '-';
        return true;
      }
      this.PushNodeStart(HtmlNodeType.Element, this._index - 1, this._lineposition - 1);
      return true;
    }

    private void Parse()
    {
      if (this.ParseExecuting != null)
        this.ParseExecuting(this);
      int num = 0;
      if (this.OptionComputeChecksum)
        this._crc32 = new Crc32();
      this.Lastnodes = new Dictionary<string, HtmlNode>();
      this._c = 0;
      this._fullcomment = false;
      this._parseerrors = new List<HtmlParseError>();
      this._line = 1;
      this._lineposition = 0;
      this._maxlineposition = 0;
      this._state = HtmlDocument.ParseState.Text;
      this._oldstate = this._state;
      this._documentnode._innerlength = this.Text.Length;
      this._documentnode._outerlength = this.Text.Length;
      this._remainderOffset = this.Text.Length;
      this._lastparentnode = this._documentnode;
      this._currentnode = this.CreateNode(HtmlNodeType.Text, 0);
      this._currentattribute = (HtmlAttribute) null;
      this._index = 0;
      this.PushNodeStart(HtmlNodeType.Text, 0, this._lineposition);
      while (this._index < this.Text.Length)
      {
        this._c = (int) this.Text[this._index];
        this.IncrementPosition();
        switch (this._state)
        {
          case HtmlDocument.ParseState.Text:
            if (!this.NewCheck())
              continue;
            continue;
          case HtmlDocument.ParseState.WhichTag:
            if (!this.NewCheck())
            {
              if (this._c == 47)
              {
                this.PushNodeNameStart(false, this._index);
              }
              else
              {
                this.PushNodeNameStart(true, this._index - 1);
                this.DecrementPosition();
              }
              this._state = HtmlDocument.ParseState.Tag;
              continue;
            }
            continue;
          case HtmlDocument.ParseState.Tag:
            if (!this.NewCheck())
            {
              if (HtmlDocument.IsWhiteSpace(this._c))
              {
                this.CloseParentImplicitExplicitNode();
                this.PushNodeNameEnd(this._index - 1);
                if (this._state == HtmlDocument.ParseState.Tag)
                {
                  this._state = HtmlDocument.ParseState.BetweenAttributes;
                  continue;
                }
                continue;
              }
              if (this._c == 47)
              {
                this.CloseParentImplicitExplicitNode();
                this.PushNodeNameEnd(this._index - 1);
                if (this._state == HtmlDocument.ParseState.Tag)
                {
                  this._state = HtmlDocument.ParseState.EmptyTag;
                  continue;
                }
                continue;
              }
              if (this._c == 62)
              {
                this.CloseParentImplicitExplicitNode();
                this.PushNodeNameEnd(this._index - 1);
                if (this._state == HtmlDocument.ParseState.Tag)
                {
                  if (!this.PushNodeEnd(this._index, false))
                  {
                    this._index = this.Text.Length;
                    continue;
                  }
                  if (this._state == HtmlDocument.ParseState.Tag)
                  {
                    this._state = HtmlDocument.ParseState.Text;
                    this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
                    continue;
                  }
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          case HtmlDocument.ParseState.BetweenAttributes:
            if (!this.NewCheck() && !HtmlDocument.IsWhiteSpace(this._c))
            {
              if (this._c == 47 || this._c == 63)
              {
                this._state = HtmlDocument.ParseState.EmptyTag;
                continue;
              }
              if (this._c == 62)
              {
                if (!this.PushNodeEnd(this._index, false))
                {
                  this._index = this.Text.Length;
                  continue;
                }
                if (this._state == HtmlDocument.ParseState.BetweenAttributes)
                {
                  this._state = HtmlDocument.ParseState.Text;
                  this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
                  continue;
                }
                continue;
              }
              this.PushAttributeNameStart(this._index - 1, this._lineposition - 1);
              this._state = HtmlDocument.ParseState.AttributeName;
              continue;
            }
            continue;
          case HtmlDocument.ParseState.EmptyTag:
            if (!this.NewCheck())
            {
              if (this._c == 62)
              {
                if (!this.PushNodeEnd(this._index, true))
                {
                  this._index = this.Text.Length;
                  continue;
                }
                if (this._state == HtmlDocument.ParseState.EmptyTag)
                {
                  this._state = HtmlDocument.ParseState.Text;
                  this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
                  continue;
                }
                continue;
              }
              if (!HtmlDocument.IsWhiteSpace(this._c))
              {
                this.DecrementPosition();
                this._state = HtmlDocument.ParseState.BetweenAttributes;
                continue;
              }
              this._state = HtmlDocument.ParseState.BetweenAttributes;
              continue;
            }
            continue;
          case HtmlDocument.ParseState.AttributeName:
            if (!this.NewCheck())
            {
              if (HtmlDocument.IsWhiteSpace(this._c))
              {
                this.PushAttributeNameEnd(this._index - 1);
                this._state = HtmlDocument.ParseState.AttributeBeforeEquals;
                continue;
              }
              if (this._c == 61)
              {
                this.PushAttributeNameEnd(this._index - 1);
                this._state = HtmlDocument.ParseState.AttributeAfterEquals;
                continue;
              }
              if (this._c == 62)
              {
                this.PushAttributeNameEnd(this._index - 1);
                if (!this.PushNodeEnd(this._index, false))
                {
                  this._index = this.Text.Length;
                  continue;
                }
                if (this._state == HtmlDocument.ParseState.AttributeName)
                {
                  this._state = HtmlDocument.ParseState.Text;
                  this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          case HtmlDocument.ParseState.AttributeBeforeEquals:
            if (!this.NewCheck() && !HtmlDocument.IsWhiteSpace(this._c))
            {
              if (this._c == 62)
              {
                if (!this.PushNodeEnd(this._index, false))
                {
                  this._index = this.Text.Length;
                  continue;
                }
                if (this._state == HtmlDocument.ParseState.AttributeBeforeEquals)
                {
                  this._state = HtmlDocument.ParseState.Text;
                  this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
                  continue;
                }
                continue;
              }
              if (this._c == 61)
              {
                this._state = HtmlDocument.ParseState.AttributeAfterEquals;
                continue;
              }
              this._state = HtmlDocument.ParseState.BetweenAttributes;
              this.DecrementPosition();
              continue;
            }
            continue;
          case HtmlDocument.ParseState.AttributeAfterEquals:
            if (!this.NewCheck() && !HtmlDocument.IsWhiteSpace(this._c))
            {
              if (this._c == 39 || this._c == 34)
              {
                this._state = HtmlDocument.ParseState.QuotedAttributeValue;
                this.PushAttributeValueStart(this._index, this._c);
                num = this._c;
                continue;
              }
              if (this._c == 62)
              {
                if (!this.PushNodeEnd(this._index, false))
                {
                  this._index = this.Text.Length;
                  continue;
                }
                if (this._state == HtmlDocument.ParseState.AttributeAfterEquals)
                {
                  this._state = HtmlDocument.ParseState.Text;
                  this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
                  continue;
                }
                continue;
              }
              this.PushAttributeValueStart(this._index - 1);
              this._state = HtmlDocument.ParseState.AttributeValue;
              continue;
            }
            continue;
          case HtmlDocument.ParseState.AttributeValue:
            if (!this.NewCheck())
            {
              if (HtmlDocument.IsWhiteSpace(this._c))
              {
                this.PushAttributeValueEnd(this._index - 1);
                this._state = HtmlDocument.ParseState.BetweenAttributes;
                continue;
              }
              if (this._c == 62)
              {
                this.PushAttributeValueEnd(this._index - 1);
                if (!this.PushNodeEnd(this._index, false))
                {
                  this._index = this.Text.Length;
                  continue;
                }
                if (this._state == HtmlDocument.ParseState.AttributeValue)
                {
                  this._state = HtmlDocument.ParseState.Text;
                  this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          case HtmlDocument.ParseState.Comment:
            if (this._c == 62 && (!this._fullcomment || this.Text[this._index - 2] == '-' && this.Text[this._index - 3] == '-' || this.Text[this._index - 2] == '!' && this.Text[this._index - 3] == '-' && this.Text[this._index - 4] == '-'))
            {
              if (!this.PushNodeEnd(this._index, false))
              {
                this._index = this.Text.Length;
                continue;
              }
              this._state = HtmlDocument.ParseState.Text;
              this.PushNodeStart(HtmlNodeType.Text, this._index, this._lineposition);
              continue;
            }
            continue;
          case HtmlDocument.ParseState.QuotedAttributeValue:
            if (this._c == num)
            {
              this.PushAttributeValueEnd(this._index - 1);
              this._state = HtmlDocument.ParseState.BetweenAttributes;
              continue;
            }
            if (this._c == 60 && this._index < this.Text.Length && this.Text[this._index] == '%')
            {
              this._oldstate = this._state;
              this._state = HtmlDocument.ParseState.ServerSideCode;
              continue;
            }
            continue;
          case HtmlDocument.ParseState.ServerSideCode:
            if (this._c == 37)
            {
              if (this._index < this.Text.Length && this.Text[this._index] == '>')
              {
                switch (this._oldstate)
                {
                  case HtmlDocument.ParseState.BetweenAttributes:
                    this.PushAttributeNameEnd(this._index + 1);
                    this._state = HtmlDocument.ParseState.BetweenAttributes;
                    break;
                  case HtmlDocument.ParseState.AttributeAfterEquals:
                    this._state = HtmlDocument.ParseState.AttributeValue;
                    break;
                  default:
                    this._state = this._oldstate;
                    break;
                }
                this.IncrementPosition();
                continue;
              }
              continue;
            }
            if (this._oldstate == HtmlDocument.ParseState.QuotedAttributeValue && this._c == num)
            {
              this._state = this._oldstate;
              this.DecrementPosition();
              continue;
            }
            continue;
          case HtmlDocument.ParseState.PcData:
            if (this._currentnode._namelength + 3 <= this.Text.Length - (this._index - 1) && string.Compare(this.Text.Substring(this._index - 1, this._currentnode._namelength + 2), "</" + this._currentnode.Name, StringComparison.OrdinalIgnoreCase) == 0)
            {
              int c = (int) this.Text[this._index - 1 + 2 + this._currentnode.Name.Length];
              if (c == 62 || HtmlDocument.IsWhiteSpace(c))
              {
                HtmlNode node = this.CreateNode(HtmlNodeType.Text, this._currentnode._outerstartindex + this._currentnode._outerlength);
                node._outerlength = this._index - 1 - node._outerstartindex;
                node._streamposition = node._outerstartindex;
                node._line = this._currentnode.Line;
                node._lineposition = this._currentnode.LinePosition + this._currentnode._namelength + 2;
                this._currentnode.AppendChild(node);
                if (this._currentnode.Name.ToLowerInvariant().Equals("script") || this._currentnode.Name.ToLowerInvariant().Equals("style"))
                  this._currentnode._isHideInnerText = true;
                this.PushNodeStart(HtmlNodeType.Element, this._index - 1, this._lineposition - 1);
                this.PushNodeNameStart(false, this._index - 1 + 2);
                this._state = HtmlDocument.ParseState.Tag;
                this.IncrementPosition();
                continue;
              }
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      if (this._currentnode._namestartindex > 0)
        this.PushNodeNameEnd(this._index);
      this.PushNodeEnd(this._index, false);
      this.Lastnodes.Clear();
    }

    private void PushAttributeNameEnd(int index)
    {
      this._currentattribute._namelength = index - this._currentattribute._namestartindex;
      if (this._currentattribute.Name == null || HtmlDocument.BlockAttributes.Contains(this._currentattribute.Name))
        return;
      this._currentnode.Attributes.Append(this._currentattribute);
    }

    private void PushAttributeNameStart(int index, int lineposition)
    {
      this._currentattribute = this.CreateAttribute();
      this._currentattribute._namestartindex = index;
      this._currentattribute.Line = this._line;
      this._currentattribute._lineposition = lineposition;
      this._currentattribute._streamposition = index;
    }

    private void PushAttributeValueEnd(int index) => this._currentattribute._valuelength = index - this._currentattribute._valuestartindex;

    private void PushAttributeValueStart(int index) => this.PushAttributeValueStart(index, 0);

    private void CloseParentImplicitExplicitNode()
    {
      bool flag1 = true;
      while (flag1 && !this._lastparentnode.Closed)
      {
        flag1 = false;
        bool flag2 = false;
        if (this.IsParentImplicitEnd())
        {
          if (this.OptionOutputAsXml)
          {
            flag2 = true;
          }
          else
          {
            this.CloseParentImplicitEnd();
            flag1 = true;
          }
        }
        if (flag2 || this.IsParentExplicitEnd())
        {
          this.CloseParentExplicitEnd();
          flag1 = true;
        }
      }
    }

    private bool IsParentImplicitEnd()
    {
      if (!this._currentnode._starttag)
        return false;
      bool flag = false;
      string name = this._lastparentnode.Name;
      string lowerInvariant = this.Text.Substring(this._currentnode._namestartindex, this._index - this._currentnode._namestartindex - 1).ToLowerInvariant();
      if (name != null)
      {
        if (!(name == "a"))
        {
          if (!(name == "dd"))
          {
            if (!(name == "dt"))
            {
              if (!(name == "li"))
              {
                if (!(name == "p"))
                {
                  if (name == "option")
                    flag = lowerInvariant == "option";
                }
                else
                  flag = !HtmlDocument.DisableBehaviorTagP ? lowerInvariant == "p" : lowerInvariant == "address" || lowerInvariant == "article" || (lowerInvariant == "aside" || lowerInvariant == "blockquote") || (lowerInvariant == "dir" || lowerInvariant == "div" || (lowerInvariant == "dl" || lowerInvariant == "fieldset")) || (lowerInvariant == "footer" || lowerInvariant == "form" || (lowerInvariant == "h1" || lowerInvariant == "h2") || (lowerInvariant == "h3" || lowerInvariant == "h4" || (lowerInvariant == "h5" || lowerInvariant == "h6"))) || (lowerInvariant == "header" || lowerInvariant == "hr" || (lowerInvariant == "menu" || lowerInvariant == "nav") || (lowerInvariant == "ol" || lowerInvariant == "p" || (lowerInvariant == "pre" || lowerInvariant == "section")) || lowerInvariant == "table") || lowerInvariant == "ul";
              }
              else
                flag = lowerInvariant == "li";
            }
            else
              flag = lowerInvariant == "dt" || lowerInvariant == "dd";
          }
          else
            flag = lowerInvariant == "dt" || lowerInvariant == "dd";
        }
        else
          flag = lowerInvariant == "a";
      }
      return flag;
    }

    private bool IsParentExplicitEnd()
    {
      if (!this._currentnode._starttag)
        return false;
      bool flag = false;
      string name = this._lastparentnode.Name;
      string lowerInvariant = this.Text.Substring(this._currentnode._namestartindex, this._index - this._currentnode._namestartindex - 1).ToLowerInvariant();
      switch (name)
      {
        case "h1":
          flag = lowerInvariant == "h2" || lowerInvariant == "h3" || lowerInvariant == "h4" || lowerInvariant == "h5";
          break;
        case "h2":
          flag = lowerInvariant == "h1" || lowerInvariant == "h3" || lowerInvariant == "h4" || lowerInvariant == "h5";
          break;
        case "h3":
          flag = lowerInvariant == "h1" || lowerInvariant == "h2" || lowerInvariant == "h4" || lowerInvariant == "h5";
          break;
        case "h4":
          flag = lowerInvariant == "h1" || lowerInvariant == "h2" || lowerInvariant == "h3" || lowerInvariant == "h5";
          break;
        case "h5":
          flag = lowerInvariant == "h1" || lowerInvariant == "h2" || lowerInvariant == "h3" || lowerInvariant == "h4";
          break;
        case "p":
          flag = lowerInvariant == "div";
          break;
        case "table":
          flag = lowerInvariant == "table";
          break;
        case "td":
          flag = lowerInvariant == "td" || lowerInvariant == "th" || lowerInvariant == "tr";
          break;
        case "th":
          flag = lowerInvariant == "td" || lowerInvariant == "th" || lowerInvariant == "tr";
          break;
        case "title":
          flag = lowerInvariant == "title";
          break;
        case "tr":
          flag = lowerInvariant == "tr";
          break;
      }
      return flag;
    }

    private void CloseParentImplicitEnd()
    {
      HtmlNode endnode = new HtmlNode(this._lastparentnode.NodeType, this, -1);
      endnode._endnode = endnode;
      endnode._isImplicitEnd = true;
      this._lastparentnode._isImplicitEnd = true;
      this._lastparentnode.CloseNode(endnode);
    }

    private void CloseParentExplicitEnd()
    {
      HtmlNode endnode = new HtmlNode(this._lastparentnode.NodeType, this, -1);
      endnode._endnode = endnode;
      this._lastparentnode.CloseNode(endnode);
    }

    private void PushAttributeValueStart(int index, int quote)
    {
      this._currentattribute._valuestartindex = index;
      if (quote != 39)
        return;
      this._currentattribute.QuoteType = AttributeValueQuote.SingleQuote;
    }

    private bool PushNodeEnd(int index, bool close)
    {
      this._currentnode._outerlength = index - this._currentnode._outerstartindex;
      if (this._currentnode._nodetype == HtmlNodeType.Text || this._currentnode._nodetype == HtmlNodeType.Comment)
      {
        if (this._currentnode._outerlength > 0)
        {
          this._currentnode._innerlength = this._currentnode._outerlength;
          this._currentnode._innerstartindex = this._currentnode._outerstartindex;
          if (this._lastparentnode != null)
            this._lastparentnode.AppendChild(this._currentnode);
        }
      }
      else if (this._currentnode._starttag && this._lastparentnode != this._currentnode)
      {
        if (this._lastparentnode != null)
          this._lastparentnode.AppendChild(this._currentnode);
        this.ReadDocumentEncoding(this._currentnode);
        this._currentnode._prevwithsamename = Utilities.GetDictionaryValueOrDefault<string, HtmlNode>(this.Lastnodes, this._currentnode.Name);
        this.Lastnodes[this._currentnode.Name] = this._currentnode;
        if (this._currentnode.NodeType == HtmlNodeType.Document || this._currentnode.NodeType == HtmlNodeType.Element)
          this._lastparentnode = this._currentnode;
        if (HtmlNode.IsCDataElement(this.CurrentNodeName()))
        {
          this._state = HtmlDocument.ParseState.PcData;
          return true;
        }
        if (HtmlNode.IsClosedElement(this._currentnode.Name) || HtmlNode.IsEmptyElement(this._currentnode.Name))
          close = true;
      }
      if (close || !this._currentnode._starttag)
      {
        if (this.OptionStopperNodeName != null && this._remainder == null && string.Compare(this._currentnode.Name, this.OptionStopperNodeName, StringComparison.OrdinalIgnoreCase) == 0)
        {
          this._remainderOffset = index;
          this._remainder = this.Text.Substring(this._remainderOffset);
          this.CloseCurrentNode();
          return false;
        }
        this.CloseCurrentNode();
      }
      return true;
    }

    private void PushNodeNameEnd(int index)
    {
      this._currentnode._namelength = index - this._currentnode._namestartindex;
      if (!this.OptionFixNestedTags)
        return;
      this.FixNestedTags();
    }

    private void PushNodeNameStart(bool starttag, int index)
    {
      this._currentnode._starttag = starttag;
      this._currentnode._namestartindex = index;
    }

    private void PushNodeStart(HtmlNodeType type, int index, int lineposition)
    {
      this._currentnode = this.CreateNode(type, index);
      this._currentnode._line = this._line;
      this._currentnode._lineposition = lineposition;
      this._currentnode._streamposition = index;
    }

    private void ReadDocumentEncoding(HtmlNode node)
    {
      if (!this.OptionReadEncoding || node._namelength != 4 || node.Name != "meta")
        return;
      string str = (string) null;
      HtmlAttribute attribute1 = node.Attributes["http-equiv"];
      if (attribute1 != null)
      {
        if (string.Compare(attribute1.Value, "content-type", StringComparison.OrdinalIgnoreCase) != 0)
          return;
        HtmlAttribute attribute2 = node.Attributes["content"];
        if (attribute2 != null)
          str = NameValuePairList.GetNameValuePairsValue(attribute2.Value, "charset");
      }
      else
      {
        HtmlAttribute attribute2 = node.Attributes["charset"];
        if (attribute2 != null)
          str = attribute2.Value;
      }
      if (string.IsNullOrEmpty(str))
        return;
      if (string.Equals(str, "utf8", StringComparison.OrdinalIgnoreCase))
        str = "utf-8";
      try
      {
        this._declaredencoding = Encoding.GetEncoding(str);
      }
      catch (ArgumentException ex)
      {
        this._declaredencoding = (Encoding) null;
      }
      if (this._onlyDetectEncoding)
        throw new EncodingFoundException(this._declaredencoding);
      if (this._streamencoding == null || this._declaredencoding == null || this._declaredencoding.CodePage == this._streamencoding.CodePage)
        return;
      this.AddError(HtmlParseErrorCode.CharsetMismatch, this._line, this._lineposition, this._index, node.OuterHtml, "Encoding mismatch between StreamEncoding: " + this._streamencoding.WebName + " and DeclaredEncoding: " + this._declaredencoding.WebName);
    }

    /// <summary>
    /// Detects the encoding of an HTML document from a file first, and then loads the file.
    /// </summary>
    /// <param name="path">The complete file path to be read.</param>
    public void DetectEncodingAndLoad(string path) => this.DetectEncodingAndLoad(path, true);

    /// <summary>
    /// Detects the encoding of an HTML document from a file first, and then loads the file.
    /// </summary>
    /// <param name="path">The complete file path to be read. May not be null.</param>
    /// <param name="detectEncoding">true to detect encoding, false otherwise.</param>
    public void DetectEncodingAndLoad(string path, bool detectEncoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      Encoding encoding = !detectEncoding ? (Encoding) null : this.DetectEncoding(path);
      if (encoding == null)
        this.Load(path);
      else
        this.Load(path, encoding);
    }

    /// <summary>Detects the encoding of an HTML file.</summary>
    /// <param name="path">Path for the file containing the HTML document to detect. May not be null.</param>
    /// <returns>The detected encoding.</returns>
    public Encoding DetectEncoding(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      using (StreamReader streamReader = new StreamReader(path, this.OptionDefaultStreamEncoding))
        return this.DetectEncoding((TextReader) streamReader);
    }

    /// <summary>Loads an HTML document from a file.</summary>
    /// <param name="path">The complete file path to be read. May not be null.</param>
    public void Load(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      using (StreamReader streamReader = new StreamReader(path, this.OptionDefaultStreamEncoding))
        this.Load((TextReader) streamReader);
    }

    /// <summary>Loads an HTML document from a file.</summary>
    /// <param name="path">The complete file path to be read. May not be null.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    public void Load(string path, bool detectEncodingFromByteOrderMarks)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      using (StreamReader streamReader = new StreamReader(path, detectEncodingFromByteOrderMarks))
        this.Load((TextReader) streamReader);
    }

    /// <summary>Loads an HTML document from a file.</summary>
    /// <param name="path">The complete file path to be read. May not be null.</param>
    /// <param name="encoding">The character encoding to use. May not be null.</param>
    public void Load(string path, Encoding encoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      using (StreamReader streamReader = new StreamReader(path, encoding))
        this.Load((TextReader) streamReader);
    }

    /// <summary>Loads an HTML document from a file.</summary>
    /// <param name="path">The complete file path to be read. May not be null.</param>
    /// <param name="encoding">The character encoding to use. May not be null.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      using (StreamReader streamReader = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks))
        this.Load((TextReader) streamReader);
    }

    /// <summary>Loads an HTML document from a file.</summary>
    /// <param name="path">The complete file path to be read. May not be null.</param>
    /// <param name="encoding">The character encoding to use. May not be null.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <param name="buffersize">The minimum buffer size.</param>
    public void Load(
      string path,
      Encoding encoding,
      bool detectEncodingFromByteOrderMarks,
      int buffersize)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      using (StreamReader streamReader = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks, buffersize))
        this.Load((TextReader) streamReader);
    }

    /// <summary>Saves the mixed document to the specified file.</summary>
    /// <param name="filename">The location of the file where you want to save the document.</param>
    public void Save(string filename)
    {
      using (StreamWriter writer = new StreamWriter(filename, false, this.GetOutEncoding()))
        this.Save(writer);
    }

    /// <summary>Saves the mixed document to the specified file.</summary>
    /// <param name="filename">The location of the file where you want to save the document. May not be null.</param>
    /// <param name="encoding">The character encoding to use. May not be null.</param>
    public void Save(string filename, Encoding encoding)
    {
      if (filename == null)
        throw new ArgumentNullException(nameof (filename));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      using (StreamWriter writer = new StreamWriter(filename, false, encoding))
        this.Save(writer);
    }

    /// <summary>
    /// Creates a new XPathNavigator object for navigating this HTML document.
    /// </summary>
    /// <returns>An XPathNavigator object. The XPathNavigator is positioned on the root of the document.</returns>
    public XPathNavigator CreateNavigator() => (XPathNavigator) new HtmlNodeNavigator(this, this._documentnode);

    private enum ParseState
    {
      Text,
      WhichTag,
      Tag,
      BetweenAttributes,
      EmptyTag,
      AttributeName,
      AttributeBeforeEquals,
      AttributeAfterEquals,
      AttributeValue,
      Comment,
      QuotedAttributeValue,
      ServerSideCode,
      PcData,
    }
  }
}
