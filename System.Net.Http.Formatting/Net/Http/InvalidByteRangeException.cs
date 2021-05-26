// Decompiled with JetBrains decompiler
// Type: System.Net.Http.InvalidByteRangeException
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Web.Http;

namespace System.Net.Http
{
  /// <summary> An exception thrown by <see cref="T:System.Net.Http.ByteRangeStreamContent" /> in case none of the requested ranges  overlap with the current extend of the selected resource. The current extend of the resource is indicated in the ContentRange property. </summary>
  public class InvalidByteRangeException : Exception
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.InvalidByteRangeException" /> class.</summary>
    public InvalidByteRangeException(ContentRangeHeaderValue contentRange) => this.Initialize(contentRange);

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.InvalidByteRangeException" /> class.</summary>
    public InvalidByteRangeException(ContentRangeHeaderValue contentRange, string message)
      : base(message)
    {
      this.Initialize(contentRange);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.InvalidByteRangeException" /> class.</summary>
    public InvalidByteRangeException(
      ContentRangeHeaderValue contentRange,
      string message,
      Exception innerException)
      : base(message, innerException)
    {
      this.Initialize(contentRange);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.InvalidByteRangeException" /> class.</summary>
    public InvalidByteRangeException(
      ContentRangeHeaderValue contentRange,
      SerializationInfo info,
      StreamingContext context)
      : base(info, context)
    {
      this.Initialize(contentRange);
    }

    /// <summary> The current extend of the resource indicated in terms of a ContentRange header field. </summary>
    public ContentRangeHeaderValue ContentRange { get; private set; }

    private void Initialize(ContentRangeHeaderValue contentRange) => this.ContentRange = contentRange != null ? contentRange : throw Error.ArgumentNull(nameof (contentRange));
  }
}
