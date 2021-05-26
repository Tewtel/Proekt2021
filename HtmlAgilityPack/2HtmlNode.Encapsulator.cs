// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.NodeNotFoundException
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;

namespace HtmlAgilityPack
{
  /// <summary>
  /// Exception that often occures when there is no way to bind a XPath to a Html Tag.
  /// </summary>
  public class NodeNotFoundException : Exception
  {
    /// <summary>
    /// 
    /// </summary>
    public NodeNotFoundException()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public NodeNotFoundException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public NodeNotFoundException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
