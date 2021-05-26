// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartFileData
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Headers;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>Represents a multipart file data.</summary>
  public class MultipartFileData
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.MultipartFileData" /> class.</summary>
    /// <param name="headers">The headers of the multipart file data.</param>
    /// <param name="localFileName">The name of the local file for the multipart file data.</param>
    public MultipartFileData(HttpContentHeaders headers, string localFileName)
    {
      if (headers == null)
        throw Error.ArgumentNull(nameof (headers));
      if (localFileName == null)
        throw Error.ArgumentNull(nameof (localFileName));
      this.Headers = headers;
      this.LocalFileName = localFileName;
    }

    /// <summary>Gets or sets the headers of the multipart file data.</summary>
    /// <returns>The headers of the multipart file data.</returns>
    public HttpContentHeaders Headers { get; private set; }

    /// <summary>Gets or sets the name of the local file for the multipart file data.</summary>
    /// <returns>The name of the local file for the multipart file data.</returns>
    public string LocalFileName { get; private set; }
  }
}
