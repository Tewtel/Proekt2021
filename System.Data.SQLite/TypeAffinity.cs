// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.TypeAffinity
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// SQLite has very limited types, and is inherently text-based.  The first 5 types below represent the sum of all types SQLite
  /// understands.  The DateTime extension to the spec is for internal use only.
  /// </summary>
  public enum TypeAffinity
  {
    /// <summary>Not used</summary>
    Uninitialized = 0,
    /// <summary>All integers in SQLite default to Int64</summary>
    Int64 = 1,
    /// <summary>
    /// All floating point numbers in SQLite default to double
    /// </summary>
    Double = 2,
    /// <summary>The default data type of SQLite is text</summary>
    Text = 3,
    /// <summary>
    /// Typically blob types are only seen when returned from a function
    /// </summary>
    Blob = 4,
    /// <summary>Null types can be returned from functions</summary>
    Null = 5,
    /// <summary>Used internally by this provider</summary>
    DateTime = 10, // 0x0000000A
    /// <summary>Used internally by this provider</summary>
    None = 11, // 0x0000000B
  }
}
