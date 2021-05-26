// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteOpenFlagsEnum
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  [Flags]
  internal enum SQLiteOpenFlagsEnum
  {
    None = 0,
    ReadOnly = 1,
    ReadWrite = 2,
    Create = 4,
    Uri = 64, // 0x00000040
    Memory = 128, // 0x00000080
    Default = Create | ReadWrite, // 0x00000006
  }
}
