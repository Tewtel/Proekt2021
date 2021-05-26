// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.LogEventArgs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>Event data for logging event handlers.</summary>
  public class LogEventArgs : EventArgs
  {
    /// <summary>
    /// The error code.  The type of this object value should be
    /// <see cref="T:System.Int32" /> or <see cref="T:System.Data.SQLite.SQLiteErrorCode" />.
    /// </summary>
    public readonly object ErrorCode;
    /// <summary>
    /// SQL statement text as the statement first begins executing
    /// </summary>
    public readonly string Message;
    /// <summary>Extra data associated with this event, if any.</summary>
    public readonly object Data;

    /// <summary>Constructs the object.</summary>
    /// <param name="pUserData">Should be null.</param>
    /// <param name="errorCode">
    /// The error code.  The type of this object value should be
    /// <see cref="T:System.Int32" /> or <see cref="T:System.Data.SQLite.SQLiteErrorCode" />.
    /// </param>
    /// <param name="message">The error message, if any.</param>
    /// <param name="data">The extra data, if any.</param>
    internal LogEventArgs(IntPtr pUserData, object errorCode, string message, object data)
    {
      this.ErrorCode = errorCode;
      this.Message = message;
      this.Data = data;
    }
  }
}
