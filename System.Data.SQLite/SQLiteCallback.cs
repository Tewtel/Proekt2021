// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteCallback
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>An internal callback delegate declaration.</summary>
  /// <param name="context">Raw native context pointer for the user function.</param>
  /// <param name="argc">Total number of arguments to the user function.</param>
  /// <param name="argv">Raw native pointer to the array of raw native argument pointers.</param>
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void SQLiteCallback(IntPtr context, int argc, IntPtr argv);
}
