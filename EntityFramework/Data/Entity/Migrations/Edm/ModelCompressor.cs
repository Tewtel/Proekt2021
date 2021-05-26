// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Edm.ModelCompressor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.IO;
using System.IO.Compression;
using System.Xml.Linq;

namespace System.Data.Entity.Migrations.Edm
{
  internal class ModelCompressor
  {
    public virtual byte[] Compress(XDocument model)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Compress))
          model.Save((Stream) gzipStream);
        return memoryStream.ToArray();
      }
    }

    public virtual XDocument Decompress(byte[] bytes)
    {
      using (MemoryStream memoryStream = new MemoryStream(bytes))
      {
        using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress))
          return XDocument.Load((Stream) gzipStream);
      }
    }
  }
}
