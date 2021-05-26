// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexConstraintUsage
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the native sqlite3_index_constraint_usage
  /// structure from the SQLite core library.
  /// </summary>
  public sealed class SQLiteIndexConstraintUsage
  {
    /// <summary>
    /// If greater than 0, constraint is part of argv to xFilter.
    /// </summary>
    public int argvIndex;
    /// <summary>Do not code a test for this constraint.</summary>
    public byte omit;

    /// <summary>Constructs a default instance of this class.</summary>
    internal SQLiteIndexConstraintUsage()
    {
    }

    /// <summary>
    /// Constructs an instance of this class using the specified native
    /// sqlite3_index_constraint_usage structure.
    /// </summary>
    /// <param name="constraintUsage">
    /// The native sqlite3_index_constraint_usage structure to use.
    /// </param>
    internal SQLiteIndexConstraintUsage(
      UnsafeNativeMethods.sqlite3_index_constraint_usage constraintUsage)
      : this(constraintUsage.argvIndex, constraintUsage.omit)
    {
    }

    /// <summary>
    /// Constructs an instance of this class using the specified field
    /// values.
    /// </summary>
    /// <param name="argvIndex">
    /// If greater than 0, constraint is part of argv to xFilter.
    /// </param>
    /// <param name="omit">Do not code a test for this constraint.</param>
    private SQLiteIndexConstraintUsage(int argvIndex, byte omit)
    {
      this.argvIndex = argvIndex;
      this.omit = omit;
    }
  }
}
