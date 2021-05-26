// Decompiled with JetBrains decompiler
// Type: System.Net.Http.RemoteStreamInfo
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.IO;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>Represents the result for <see cref="M:System.Net.Http.MultipartFormDataRemoteStreamProvider.GetRemoteStream(System.Net.Http.HttpContent,System.Net.Http.Headers.HttpContentHeaders)" />.</summary>
  public class RemoteStreamInfo
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.RemoteStreamInfo" /> class.</summary>
    /// <param name="remoteStream">The remote stream instance where the file will be written to.</param>
    /// <param name="location">The remote file's location.</param>
    /// <param name="fileName">The remote file's name.</param>
    public RemoteStreamInfo(Stream remoteStream, string location, string fileName)
    {
      if (remoteStream == null)
        throw Error.ArgumentNull(nameof (remoteStream));
      if (location == null)
        throw Error.ArgumentNull(nameof (location));
      this.FileName = fileName != null ? fileName : throw Error.ArgumentNull(nameof (fileName));
      this.RemoteStream = remoteStream;
      this.Location = location;
    }

    /// <summary>Gets the remote file's location.</summary>
    public string FileName { get; private set; }

    /// <summary>Gets the remote file's location.</summary>
    public string Location { get; private set; }

    /// <summary>Gets the remote stream instance where the file will be written to.</summary>
    public Stream RemoteStream { get; private set; }
  }
}
