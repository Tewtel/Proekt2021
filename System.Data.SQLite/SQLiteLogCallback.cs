// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteLogCallback
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>
  /// This is the method signature for the SQLite core library logging callback
  /// function for use with sqlite3_log() and the SQLITE_CONFIG_LOG.
  /// 
  /// WARNING: This delegate is used more-or-less directly by native code, do
  ///          not modify its type signature.
  /// </summary>
  /// <param name="pUserData">
  /// The extra data associated with this message, if any.
  /// </param>
  /// <param name="errorCode">
  /// The error code associated with this message.
  /// </param>
  /// <param name="pMessage">The message string to be logged.</param>
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  internal delegate void SQLiteLogCallback(IntPtr pUserData, int errorCode, IntPtr pMessage);
}
