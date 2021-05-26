// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteChangeSetStartFlags
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This enumerated type represents possible flags that may be passed
  /// to the appropriate overloads of various change set creation methods.
  /// </summary>
  public enum SQLiteChangeSetStartFlags
  {
    /// <summary>No special handling.</summary>
    None = 0,
    /// <summary>
    /// Invert the change set while iterating through it.
    /// This is equivalent to inverting a change set using
    /// <see cref="M:System.Data.SQLite.ISQLiteChangeSet.Invert" /> before
    /// applying it. It is an error to specify this flag
    /// with a patch set.
    /// </summary>
    Invert = 2,
  }
}
