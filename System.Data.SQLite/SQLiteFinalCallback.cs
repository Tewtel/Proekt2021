﻿// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteFinalCallback
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>An internal final callback delegate declaration.</summary>
  /// <param name="context">Raw context pointer for the user function</param>
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  internal delegate void SQLiteFinalCallback(IntPtr context);
}
