// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System.Xml;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
  {
    private readonly XmlDeclaration _declaration;

    public XmlDeclarationWrapper(XmlDeclaration declaration)
      : base((XmlNode) declaration)
    {
      this._declaration = declaration;
    }

    public string Version => this._declaration.Version;

    public string Encoding
    {
      get => this._declaration.Encoding;
      set => this._declaration.Encoding = value;
    }

    public string Standalone
    {
      get => this._declaration.Standalone;
      set => this._declaration.Standalone = value;
    }
  }
}
