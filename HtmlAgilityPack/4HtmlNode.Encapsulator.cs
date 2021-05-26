// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.MissingXPathException
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;

namespace HtmlAgilityPack
{
  /// <summary>
  /// Exception that often occures when there is no property that assigned with XPath Property in Class.
  /// </summary>
  public class MissingXPathException : Exception
  {
    /// <summary>
    /// 
    /// </summary>
    public MissingXPathException()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public MissingXPathException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public MissingXPathException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
