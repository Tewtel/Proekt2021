// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteCollation
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>
  /// Internal callback delegate for implementing collating sequences
  /// </summary>
  /// <param name="puser">Not used</param>
  /// <param name="len1">Length of the string pv1</param>
  /// <param name="pv1">Pointer to the first string to compare</param>
  /// <param name="len2">Length of the string pv2</param>
  /// <param name="pv2">Pointer to the second string to compare</param>
  /// <returns>Returns -1 if the first string is less than the second.  0 if they are equal, or 1 if the first string is greater
  /// than the second.</returns>
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  internal delegate int SQLiteCollation(IntPtr puser, int len1, IntPtr pv1, int len2, IntPtr pv2);
}
