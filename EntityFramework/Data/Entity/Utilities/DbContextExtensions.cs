// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.DbContextExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace System.Data.Entity.Utilities
{
  internal static class DbContextExtensions
  {
    public static XDocument GetModel(this DbContext context) => DbContextExtensions.GetModel((Action<XmlWriter>) (w => EdmxWriter.WriteEdmx(context, w)));

    public static XDocument GetModel(Action<XmlWriter> writeXml)
    {
      using (MemoryStream memoryStream1 = new MemoryStream())
      {
        MemoryStream memoryStream2 = memoryStream1;
        using (XmlWriter xmlWriter = XmlWriter.Create((Stream) memoryStream2, new XmlWriterSettings()
        {
          Indent = true
        }))
          writeXml(xmlWriter);
        memoryStream1.Position = 0L;
        return XDocument.Load((Stream) memoryStream1);
      }
    }
  }
}
