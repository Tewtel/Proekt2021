// Decompiled with JetBrains decompiler
// Type: System.Net.Http.MultipartFileStreamProvider
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Properties;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary>Represents an <see cref="T:System.Net.Http.IMultipartStreamProvider" /> suited for writing each MIME body parts of the MIME multipart message to a file using a <see cref="T:System.IO.FileStream" />.</summary>
  public class MultipartFileStreamProvider : MultipartStreamProvider
  {
    private const int MinBufferSize = 1;
    private const int DefaultBufferSize = 4096;
    private string _rootPath;
    private int _bufferSize = 4096;
    private Collection<MultipartFileData> _fileData = new Collection<MultipartFileData>();

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.MultipartFileStreamProvider" /> class.</summary>
    /// <param name="rootPath">The root path where the content of MIME multipart body parts are written to.</param>
    public MultipartFileStreamProvider(string rootPath)
      : this(rootPath, 4096)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.MultipartFileStreamProvider" /> class.</summary>
    /// <param name="rootPath">The root path where the content of MIME multipart body parts are written to.</param>
    /// <param name="bufferSize">The number of bytes buffered for writes to the file.</param>
    public MultipartFileStreamProvider(string rootPath, int bufferSize)
    {
      if (rootPath == null)
        throw Error.ArgumentNull(nameof (rootPath));
      if (bufferSize < 1)
        throw Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (bufferSize), (object) bufferSize, (object) 1);
      this._rootPath = Path.GetFullPath(rootPath);
      this._bufferSize = bufferSize;
    }

    /// <summary>Gets or sets the multipart file data.</summary>
    /// <returns>The multipart file data.</returns>
    public Collection<MultipartFileData> FileData => this._fileData;

    /// <summary>Gets or sets the root path where the content of MIME multipart body parts are written to.</summary>
    /// <returns>The root path where the content of MIME multipart body parts are written to.</returns>
    protected string RootPath => this._rootPath;

    /// <summary>Gets or sets the number of bytes buffered for writes to the file.</summary>
    /// <returns>The number of bytes buffered for writes to the file.</returns>
    protected int BufferSize => this._bufferSize;

    /// <summary>Gets the stream instance where the message body part is written to.</summary>
    /// <returns>The <see cref="T:System.IO.Stream" /> instance where the message body part is written to.</returns>
    /// <param name="parent">The content of HTTP.</param>
    /// <param name="headers">The header fields describing the body part.</param>
    public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
    {
      if (parent == null)
        throw Error.ArgumentNull(nameof (parent));
      if (headers == null)
        throw Error.ArgumentNull(nameof (headers));
      string str;
      try
      {
        str = Path.Combine(this._rootPath, Path.GetFileName(this.GetLocalFileName(headers)));
      }
      catch (Exception ex)
      {
        string invalidLocalFileName = Resources.MultipartStreamProviderInvalidLocalFileName;
        object[] objArray = new object[0];
        throw Error.InvalidOperation(ex, invalidLocalFileName, objArray);
      }
      this._fileData.Add(new MultipartFileData(headers, str));
      return (Stream) System.IO.File.Create(str, this._bufferSize, FileOptions.Asynchronous);
    }

    /// <summary>Gets the name of the local file which will be combined with the root path to create an absolute file name where the contents of the current MIME body part will be stored.</summary>
    /// <returns>A relative filename with no path component.</returns>
    /// <param name="headers">The headers for the current MIME body part.</param>
    public virtual string GetLocalFileName(HttpContentHeaders headers)
    {
      if (headers == null)
        throw Error.ArgumentNull(nameof (headers));
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "BodyPart_{0}", (object) Guid.NewGuid());
    }
  }
}
