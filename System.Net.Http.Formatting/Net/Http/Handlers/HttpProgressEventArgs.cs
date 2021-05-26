// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Handlers.HttpProgressEventArgs
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.ComponentModel;

namespace System.Net.Http.Handlers
{
  /// <summary>Represents the event arguments for the HTTP progress.</summary>
  public class HttpProgressEventArgs : ProgressChangedEventArgs
  {
    /// <summary> Initializes a new instance of the <see cref="T:System.Net.Http.Handlers.HttpProgressEventArgs" /> class. </summary>
    /// <param name="progressPercentage">The percentage of the progress.</param>
    /// <param name="userToken">The user token.</param>
    /// <param name="bytesTransferred">The number of bytes transferred.</param>
    /// <param name="totalBytes">The total number of bytes transferred.</param>
    public HttpProgressEventArgs(
      int progressPercentage,
      object userToken,
      long bytesTransferred,
      long? totalBytes)
      : base(progressPercentage, userToken)
    {
      this.BytesTransferred = bytesTransferred;
      this.TotalBytes = totalBytes;
    }

    public long BytesTransferred { get; private set; }

    public long? TotalBytes { get; private set; }
  }
}
