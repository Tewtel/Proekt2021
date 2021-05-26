// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartMemoryStreamProvider
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Net.Http.Headers;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>Represents a multipart memory stream provider.</summary>
  public class MultipartMemoryStreamProvider : MultipartStreamProvider
  {
    /// <summary>Returns the <see cref="T:System.IO.Stream" /> for the <see cref="T:System.Net.Http.MultipartMemoryStreamProvider" />.</summary>
    /// <returns>The <see cref="T:System.IO.Stream" /> for the <see cref="T:System.Net.Http.MultipartMemoryStreamProvider" />.</returns>
    /// <param name="parent">A <see cref="T:System.Net.Http.HttpContent" /> object.</param>
    /// <param name="headers">The HTTP content headers.</param>
    public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
    {
      if (parent == null)
        throw Error.ArgumentNull(nameof (parent));
      if (headers == null)
        throw Error.ArgumentNull(nameof (headers));
      return (Stream) new MemoryStream();
    }
  }
}
