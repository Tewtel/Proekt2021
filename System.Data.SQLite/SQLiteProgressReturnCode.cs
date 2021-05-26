// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteProgressReturnCode
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>The possible return codes for the progress callback.</summary>
  public enum SQLiteProgressReturnCode
  {
    /// <summary>The operation should continue.</summary>
    Continue,
    /// <summary>The operation should be interrupted.</summary>
    Interrupt,
  }
}
