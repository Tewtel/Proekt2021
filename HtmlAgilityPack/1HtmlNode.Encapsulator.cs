// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.XPathAttribute
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;

namespace HtmlAgilityPack
{
  /// <summary>
  /// Includes XPath and <see cref="P:HtmlAgilityPack.XPathAttribute.NodeReturnType" />. XPath for finding html tags and <see cref="P:HtmlAgilityPack.XPathAttribute.NodeReturnType" /> for specify which part of <see cref="T:HtmlAgilityPack.HtmlNode" /> you want to return.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
  public sealed class XPathAttribute : Attribute
  {
    /// <summary>
    /// XPath Expression that is used to find related html node.
    /// </summary>
    public string XPath { get; }

    /// <summary>Html Attribute name</summary>
    public string AttributeName { get; set; }

    /// <summary>The methode of output</summary>
    public ReturnType NodeReturnType { get; set; }

    /// <summary>Specify Xpath to find related Html Node.</summary>
    /// <param name="xpathString"></param>
    public XPathAttribute(string xpathString)
    {
      this.XPath = xpathString;
      this.NodeReturnType = ReturnType.InnerText;
    }

    /// <summary>Specify Xpath to find related Html Node.</summary>
    /// <param name="xpathString"></param>
    /// <param name="nodeReturnType">Specify you want the output include html text too.</param>
    public XPathAttribute(string xpathString, ReturnType nodeReturnType)
    {
      this.XPath = xpathString;
      this.NodeReturnType = nodeReturnType;
    }

    /// <summary>
    /// Specify Xpath and Attribute to find related Html Node and its attribute value.
    /// </summary>
    /// <param name="xpathString"></param>
    /// <param name="attributeName"></param>
    public XPathAttribute(string xpathString, string attributeName)
    {
      this.XPath = xpathString;
      this.AttributeName = attributeName;
    }
  }
}
