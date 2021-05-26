// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.CollationTypeEnum
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>The type of collating sequence</summary>
  public enum CollationTypeEnum
  {
    /// <summary>A custom user-defined collating sequence</summary>
    Custom,
    /// <summary>The built-in BINARY collating sequence</summary>
    Binary,
    /// <summary>The built-in NOCASE collating sequence</summary>
    NoCase,
    /// <summary>The built-in REVERSE collating sequence</summary>
    Reverse,
  }
}
