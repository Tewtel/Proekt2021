// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ProgressEventArgs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// The event data associated with progress reporting events.
  /// </summary>
  public class ProgressEventArgs : EventArgs
  {
    /// <summary>
    /// The user-defined native data associated with this event.  Currently,
    /// this will always contain the value of <see cref="F:System.IntPtr.Zero" />.
    /// </summary>
    public readonly IntPtr UserData;
    /// <summary>
    /// The return code for the current call into the progress callback.
    /// </summary>
    public SQLiteProgressReturnCode ReturnCode;

    /// <summary>
    /// Constructs an instance of this class with default property values.
    /// </summary>
    private ProgressEventArgs()
    {
      this.UserData = IntPtr.Zero;
      this.ReturnCode = SQLiteProgressReturnCode.Continue;
    }

    /// <summary>
    /// Constructs an instance of this class with specific property values.
    /// </summary>
    /// <param name="pUserData">
    /// The user-defined native data associated with this event.
    /// </param>
    /// <param name="returnCode">The progress return code.</param>
    internal ProgressEventArgs(IntPtr pUserData, SQLiteProgressReturnCode returnCode)
      : this()
    {
      this.UserData = pUserData;
      this.ReturnCode = returnCode;
    }
  }
}
