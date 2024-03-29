﻿// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlNodeNavigator
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace HtmlAgilityPack
{
  /// <summary>
  /// Represents an HTML navigator on an HTML document seen as a data store.
  /// </summary>
  public class HtmlNodeNavigator : XPathNavigator
  {
    private int _attindex;
    private HtmlNode _currentnode;
    private readonly HtmlDocument _doc;
    private readonly HtmlNameTable _nametable;
    internal bool Trace;

    internal HtmlNodeNavigator()
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this.Reset();
    }

    internal HtmlNodeNavigator(HtmlDocument doc, HtmlNode currentNode)
    {
      if (currentNode == null)
        throw new ArgumentNullException(nameof (currentNode));
      if (currentNode.OwnerDocument != doc)
        throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
      this._doc = doc != null ? doc : throw new Exception("Oops! The HtmlDocument cannot be null.");
      this._nametable = new HtmlNameTable();
      this.Reset();
      this._currentnode = currentNode;
    }

    private HtmlNodeNavigator(HtmlNodeNavigator nav)
    {
      this._doc = nav != null ? nav._doc : throw new ArgumentNullException(nameof (nav));
      this._currentnode = nav._currentnode;
      this._attindex = nav._attindex;
      this._nametable = nav._nametable;
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    public HtmlNodeNavigator(Stream stream)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(stream);
      this.Reset();
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
    public HtmlNodeNavigator(Stream stream, bool detectEncodingFromByteOrderMarks)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(stream, detectEncodingFromByteOrderMarks);
      this.Reset();
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    public HtmlNodeNavigator(Stream stream, Encoding encoding)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(stream, encoding);
      this.Reset();
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
    public HtmlNodeNavigator(
      Stream stream,
      Encoding encoding,
      bool detectEncodingFromByteOrderMarks)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(stream, encoding, detectEncodingFromByteOrderMarks);
      this.Reset();
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a stream.
    /// </summary>
    /// <param name="stream">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the stream.</param>
    /// <param name="buffersize">The minimum buffer size.</param>
    public HtmlNodeNavigator(
      Stream stream,
      Encoding encoding,
      bool detectEncodingFromByteOrderMarks,
      int buffersize)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(stream, encoding, detectEncodingFromByteOrderMarks, buffersize);
      this.Reset();
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a TextReader.
    /// </summary>
    /// <param name="reader">The TextReader used to feed the HTML data into the document.</param>
    public HtmlNodeNavigator(TextReader reader)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(reader);
      this.Reset();
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a file.
    /// </summary>
    /// <param name="path">The complete file path to be read.</param>
    public HtmlNodeNavigator(string path)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(path);
      this.Reset();
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a file.
    /// </summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    public HtmlNodeNavigator(string path, bool detectEncodingFromByteOrderMarks)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(path, detectEncodingFromByteOrderMarks);
      this.Reset();
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a file.
    /// </summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    public HtmlNodeNavigator(string path, Encoding encoding)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(path, encoding);
      this.Reset();
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a file.
    /// </summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    public HtmlNodeNavigator(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(path, encoding, detectEncodingFromByteOrderMarks);
      this.Reset();
    }

    /// <summary>
    /// Initializes a new instance of the HtmlNavigator and loads an HTML document from a file.
    /// </summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <param name="buffersize">The minimum buffer size.</param>
    public HtmlNodeNavigator(
      string path,
      Encoding encoding,
      bool detectEncodingFromByteOrderMarks,
      int buffersize)
    {
      this._doc = new HtmlDocument();
      this._nametable = new HtmlNameTable();
      this._doc.Load(path, encoding, detectEncodingFromByteOrderMarks, buffersize);
      this.Reset();
    }

    /// <summary>
    /// Gets the base URI for the current node.
    /// Always returns string.Empty in the case of HtmlNavigator implementation.
    /// </summary>
    public override string BaseURI => this._nametable.GetOrAdd(string.Empty);

    /// <summary>Gets the current HTML document.</summary>
    public HtmlDocument CurrentDocument => this._doc;

    /// <summary>Gets the current HTML node.</summary>
    public HtmlNode CurrentNode => this._currentnode;

    /// <summary>
    /// Gets a value indicating whether the current node has child nodes.
    /// </summary>
    public override bool HasAttributes => this._currentnode.Attributes.Count > 0;

    /// <summary>
    /// Gets a value indicating whether the current node has child nodes.
    /// </summary>
    public override bool HasChildren => this._currentnode.ChildNodes.Count > 0;

    /// <summary>
    /// Gets a value indicating whether the current node is an empty element.
    /// </summary>
    public override bool IsEmptyElement => !this.HasChildren;

    /// <summary>
    /// Gets the name of the current HTML node without the namespace prefix.
    /// </summary>
    public override string LocalName => this._attindex != -1 ? this._nametable.GetOrAdd(this._currentnode.Attributes[this._attindex].Name) : this._nametable.GetOrAdd(this._currentnode.Name);

    /// <summary>Gets the qualified name of the current node.</summary>
    public override string Name => this._nametable.GetOrAdd(this._currentnode.Name);

    /// <summary>
    /// Gets the namespace URI (as defined in the W3C Namespace Specification) of the current node.
    /// Always returns string.Empty in the case of HtmlNavigator implementation.
    /// </summary>
    public override string NamespaceURI => this._nametable.GetOrAdd(string.Empty);

    /// <summary>
    /// Gets the <see cref="T:System.Xml.XmlNameTable" /> associated with this implementation.
    /// </summary>
    public override XmlNameTable NameTable => (XmlNameTable) this._nametable;

    /// <summary>Gets the type of the current node.</summary>
    public override XPathNodeType NodeType
    {
      get
      {
        switch (this._currentnode.NodeType)
        {
          case HtmlNodeType.Document:
            return XPathNodeType.Root;
          case HtmlNodeType.Element:
            return this._attindex != -1 ? XPathNodeType.Attribute : XPathNodeType.Element;
          case HtmlNodeType.Comment:
            return XPathNodeType.Comment;
          case HtmlNodeType.Text:
            return XPathNodeType.Text;
          default:
            throw new NotImplementedException("Internal error: Unhandled HtmlNodeType: " + this._currentnode.NodeType.ToString());
        }
      }
    }

    /// <summary>
    /// Gets the prefix associated with the current node.
    /// Always returns string.Empty in the case of HtmlNavigator implementation.
    /// </summary>
    public override string Prefix => this._nametable.GetOrAdd(string.Empty);

    /// <summary>Gets the text value of the current node.</summary>
    public override string Value
    {
      get
      {
        switch (this._currentnode.NodeType)
        {
          case HtmlNodeType.Document:
            return "";
          case HtmlNodeType.Element:
            return this._attindex != -1 ? this._currentnode.Attributes[this._attindex].Value : this._currentnode.InnerText;
          case HtmlNodeType.Comment:
            return ((HtmlCommentNode) this._currentnode).Comment;
          case HtmlNodeType.Text:
            return ((HtmlTextNode) this._currentnode).Text;
          default:
            throw new NotImplementedException("Internal error: Unhandled HtmlNodeType: " + this._currentnode.NodeType.ToString());
        }
      }
    }

    /// <summary>
    /// Gets the xml:lang scope for the current node.
    /// Always returns string.Empty in the case of HtmlNavigator implementation.
    /// </summary>
    public override string XmlLang => this._nametable.GetOrAdd(string.Empty);

    /// <summary>
    /// Creates a new HtmlNavigator positioned at the same node as this HtmlNavigator.
    /// </summary>
    /// <returns>A new HtmlNavigator object positioned at the same node as the original HtmlNavigator.</returns>
    public override XPathNavigator Clone() => (XPathNavigator) new HtmlNodeNavigator(this);

    /// <summary>
    /// Gets the value of the HTML attribute with the specified LocalName and NamespaceURI.
    /// </summary>
    /// <param name="localName">The local name of the HTML attribute.</param>
    /// <param name="namespaceURI">The namespace URI of the attribute. Unsupported with the HtmlNavigator implementation.</param>
    /// <returns>The value of the specified HTML attribute. String.Empty or null if a matching attribute is not found or if the navigator is not positioned on an element node.</returns>
    public override string GetAttribute(string localName, string namespaceURI) => this._currentnode.Attributes[localName]?.Value;

    /// <summary>
    /// Returns the value of the namespace node corresponding to the specified local name.
    /// Always returns string.Empty for the HtmlNavigator implementation.
    /// </summary>
    /// <param name="name">The local name of the namespace node.</param>
    /// <returns>Always returns string.Empty for the HtmlNavigator implementation.</returns>
    public override string GetNamespace(string name) => string.Empty;

    /// <summary>
    /// Determines whether the current HtmlNavigator is at the same position as the specified HtmlNavigator.
    /// </summary>
    /// <param name="other">The HtmlNavigator that you want to compare against.</param>
    /// <returns>true if the two navigators have the same position, otherwise, false.</returns>
    public override bool IsSamePosition(XPathNavigator other) => other is HtmlNodeNavigator htmlNodeNavigator && htmlNodeNavigator._currentnode == this._currentnode;

    /// <summary>
    /// Moves to the same position as the specified HtmlNavigator.
    /// </summary>
    /// <param name="other">The HtmlNavigator positioned on the node that you want to move to.</param>
    /// <returns>true if successful, otherwise false. If false, the position of the navigator is unchanged.</returns>
    public override bool MoveTo(XPathNavigator other)
    {
      if (!(other is HtmlNodeNavigator htmlNodeNavigator) || htmlNodeNavigator._doc != this._doc)
        return false;
      this._currentnode = htmlNodeNavigator._currentnode;
      this._attindex = htmlNodeNavigator._attindex;
      return true;
    }

    /// <summary>
    /// Moves to the HTML attribute with matching LocalName and NamespaceURI.
    /// </summary>
    /// <param name="localName">The local name of the HTML attribute.</param>
    /// <param name="namespaceURI">The namespace URI of the attribute. Unsupported with the HtmlNavigator implementation.</param>
    /// <returns>true if the HTML attribute is found, otherwise, false. If false, the position of the navigator does not change.</returns>
    public override bool MoveToAttribute(string localName, string namespaceURI)
    {
      int attributeIndex = this._currentnode.Attributes.GetAttributeIndex(localName);
      if (attributeIndex == -1)
        return false;
      this._attindex = attributeIndex;
      return true;
    }

    /// <summary>Moves to the first sibling of the current node.</summary>
    /// <returns>true if the navigator is successful moving to the first sibling node, false if there is no first sibling or if the navigator is currently positioned on an attribute node.</returns>
    public override bool MoveToFirst()
    {
      if (this._currentnode.ParentNode == null || this._currentnode.ParentNode.FirstChild == null)
        return false;
      this._currentnode = this._currentnode.ParentNode.FirstChild;
      return true;
    }

    /// <summary>Moves to the first HTML attribute.</summary>
    /// <returns>true if the navigator is successful moving to the first HTML attribute, otherwise, false.</returns>
    public override bool MoveToFirstAttribute()
    {
      if (!this.HasAttributes)
        return false;
      this._attindex = 0;
      return true;
    }

    /// <summary>Moves to the first child of the current node.</summary>
    /// <returns>true if there is a first child node, otherwise false.</returns>
    public override bool MoveToFirstChild()
    {
      if (!this._currentnode.HasChildNodes)
        return false;
      this._currentnode = this._currentnode.ChildNodes[0];
      return true;
    }

    /// <summary>
    /// Moves the XPathNavigator to the first namespace node of the current element.
    /// Always returns false for the HtmlNavigator implementation.
    /// </summary>
    /// <param name="scope">An XPathNamespaceScope value describing the namespace scope.</param>
    /// <returns>Always returns false for the HtmlNavigator implementation.</returns>
    public override bool MoveToFirstNamespace(XPathNamespaceScope scope) => false;

    /// <summary>
    /// Moves to the node that has an attribute of type ID whose value matches the specified string.
    /// </summary>
    /// <param name="id">A string representing the ID value of the node to which you want to move. This argument does not need to be atomized.</param>
    /// <returns>true if the move was successful, otherwise false. If false, the position of the navigator is unchanged.</returns>
    public override bool MoveToId(string id)
    {
      HtmlNode elementbyId = this._doc.GetElementbyId(id);
      if (elementbyId == null)
        return false;
      this._currentnode = elementbyId;
      return true;
    }

    /// <summary>
    /// Moves the XPathNavigator to the namespace node with the specified local name.
    /// Always returns false for the HtmlNavigator implementation.
    /// </summary>
    /// <param name="name">The local name of the namespace node.</param>
    /// <returns>Always returns false for the HtmlNavigator implementation.</returns>
    public override bool MoveToNamespace(string name) => false;

    /// <summary>Moves to the next sibling of the current node.</summary>
    /// <returns>true if the navigator is successful moving to the next sibling node, false if there are no more siblings or if the navigator is currently positioned on an attribute node. If false, the position of the navigator is unchanged.</returns>
    public override bool MoveToNext()
    {
      if (this._currentnode.NextSibling == null)
        return false;
      this._currentnode = this._currentnode.NextSibling;
      return true;
    }

    /// <summary>Moves to the next HTML attribute.</summary>
    /// <returns></returns>
    public override bool MoveToNextAttribute()
    {
      if (this._attindex >= this._currentnode.Attributes.Count - 1)
        return false;
      ++this._attindex;
      return true;
    }

    /// <summary>
    /// Moves the XPathNavigator to the next namespace node.
    /// Always returns falsefor the HtmlNavigator implementation.
    /// </summary>
    /// <param name="scope">An XPathNamespaceScope value describing the namespace scope.</param>
    /// <returns>Always returns false for the HtmlNavigator implementation.</returns>
    public override bool MoveToNextNamespace(XPathNamespaceScope scope) => false;

    /// <summary>Moves to the parent of the current node.</summary>
    /// <returns>true if there is a parent node, otherwise false.</returns>
    public override bool MoveToParent()
    {
      if (this._currentnode.ParentNode == null)
        return false;
      this._currentnode = this._currentnode.ParentNode;
      return true;
    }

    /// <summary>Moves to the previous sibling of the current node.</summary>
    /// <returns>true if the navigator is successful moving to the previous sibling node, false if there is no previous sibling or if the navigator is currently positioned on an attribute node.</returns>
    public override bool MoveToPrevious()
    {
      if (this._currentnode.PreviousSibling == null)
        return false;
      this._currentnode = this._currentnode.PreviousSibling;
      return true;
    }

    /// <summary>
    /// Moves to the root node to which the current node belongs.
    /// </summary>
    public override void MoveToRoot() => this._currentnode = this._doc.DocumentNode;

    private void Reset()
    {
      this._currentnode = this._doc.DocumentNode;
      this._attindex = -1;
    }
  }
}
