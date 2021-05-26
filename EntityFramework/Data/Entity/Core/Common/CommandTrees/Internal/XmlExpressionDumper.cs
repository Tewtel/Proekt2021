﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.Internal.XmlExpressionDumper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Data.Entity.Core.Common.CommandTrees.Internal
{
  internal class XmlExpressionDumper : ExpressionDumper
  {
    private readonly XmlWriter _writer;

    internal static Encoding DefaultEncoding => Encoding.UTF8;

    internal XmlExpressionDumper(Stream stream)
      : this(stream, XmlExpressionDumper.DefaultEncoding)
    {
    }

    internal XmlExpressionDumper(Stream stream, Encoding encoding)
    {
      this._writer = XmlWriter.Create(stream, new XmlWriterSettings()
      {
        CheckCharacters = false,
        Indent = true,
        Encoding = encoding
      });
      this._writer.WriteStartDocument(true);
    }

    internal void Close()
    {
      this._writer.WriteEndDocument();
      this._writer.Flush();
      this._writer.Close();
    }

    internal override void Begin(string name, Dictionary<string, object> attrs)
    {
      this._writer.WriteStartElement(name);
      if (attrs == null)
        return;
      foreach (KeyValuePair<string, object> attr in attrs)
        this._writer.WriteAttributeString(attr.Key, attr.Value == null ? "" : attr.Value.ToString());
    }

    internal override void End(string name) => this._writer.WriteEndElement();
  }
}
