// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartRemoteFileData
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Headers;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>Represents a multipart file data for remote storage.</summary>
  public class MultipartRemoteFileData
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.MultipartRemoteFileData" /> class.</summary>
    /// <param name="headers">The headers of the multipart file data.</param>
    /// <param name="location">The remote file's location.</param>
    /// <param name="fileName">The remote file's name.</param>
    public MultipartRemoteFileData(HttpContentHeaders headers, string location, string fileName)
    {
      if (headers == null)
        throw Error.ArgumentNull(nameof (headers));
      if (location == null)
        throw Error.ArgumentNull(nameof (location));
      this.FileName = fileName != null ? fileName : throw Error.ArgumentNull(nameof (fileName));
      this.Headers = headers;
      this.Location = location;
    }

    /// <summary>Gets the remote file's name.</summary>
    public string FileName { get; private set; }

    /// <summary>Gets the headers of the multipart file data.</summary>
    public HttpContentHeaders Headers { get; private set; }

    /// <summary>Gets the remote file's location.</summary>
    public string Location { get; private set; }
  }
}
