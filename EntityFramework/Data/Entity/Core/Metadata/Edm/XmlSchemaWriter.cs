// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.XmlSchemaWriter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Text;
using System.Xml;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal abstract class XmlSchemaWriter
  {
    protected XmlWriter _xmlWriter;
    protected double _version;

    internal void WriteComment(string comment)
    {
      if (string.IsNullOrEmpty(comment))
        return;
      this._xmlWriter.WriteComment(comment);
    }

    internal virtual void WriteEndElement() => this._xmlWriter.WriteEndElement();

    protected static string GetQualifiedTypeName(string prefix, string typeName) => new StringBuilder().Append(prefix).Append(".").Append(typeName).ToString();

    internal static string GetLowerCaseStringFromBoolValue(bool value) => !value ? "false" : "true";
  }
}
