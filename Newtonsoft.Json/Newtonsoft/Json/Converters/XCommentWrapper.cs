﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XCommentWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System.Xml.Linq;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XCommentWrapper : XObjectWrapper
  {
    private XComment Text => (XComment) this.WrappedNode;

    public XCommentWrapper(XComment text)
      : base((XObject) text)
    {
    }

    public override string? Value
    {
      get => this.Text.Value;
      set => this.Text.Value = value;
    }

    public override IXmlNode? ParentNode => this.Text.Parent == null ? (IXmlNode) null : XContainerWrapper.WrapNode((XObject) this.Text.Parent);
  }
}
