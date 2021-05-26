// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexConstraintOp
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// These are the allowed values for the operators that are part of a
  /// constraint term in the WHERE clause of a query that uses a virtual
  /// table.
  /// </summary>
  public enum SQLiteIndexConstraintOp : byte
  {
    /// <summary>This value represents the equality operator.</summary>
    EqualTo = 2,
    /// <summary>This value represents the greater than operator.</summary>
    GreaterThan = 4,
    /// <summary>
    /// This value represents the less than or equal to operator.
    /// </summary>
    LessThanOrEqualTo = 8,
    /// <summary>This value represents the less than operator.</summary>
    LessThan = 16, // 0x10
    /// <summary>
    /// This value represents the greater than or equal to operator.
    /// </summary>
    GreaterThanOrEqualTo = 32, // 0x20
    /// <summary>This value represents the MATCH operator.</summary>
    Match = 64, // 0x40
    /// <summary>This value represents the LIKE operator.</summary>
    Like = 65, // 0x41
    /// <summary>This value represents the GLOB operator.</summary>
    Glob = 66, // 0x42
    /// <summary>This value represents the REGEXP operator.</summary>
    Regexp = 67, // 0x43
    /// <summary>This value represents the inequality operator.</summary>
    NotEqualTo = 68, // 0x44
    /// <summary>This value represents the IS NOT operator.</summary>
    IsNot = 69, // 0x45
    /// <summary>This value represents the IS NOT NULL operator.</summary>
    IsNotNull = 70, // 0x46
    /// <summary>This value represents the IS NULL operator.</summary>
    IsNull = 71, // 0x47
    /// <summary>This value represents the IS operator.</summary>
    Is = 72, // 0x48
  }
}
