﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XDocumentTypeWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System.Xml.Linq;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
  {
    private readonly XDocumentType _documentType;

    public XDocumentTypeWrapper(XDocumentType documentType)
      : base((XObject) documentType)
    {
      this._documentType = documentType;
    }

    public string Name => this._documentType.Name;

    public string System => this._documentType.SystemId;

    public string Public => this._documentType.PublicId;

    public string InternalSubset => this._documentType.InternalSubset;

    public override string? LocalName => "DOCTYPE";
  }
}
