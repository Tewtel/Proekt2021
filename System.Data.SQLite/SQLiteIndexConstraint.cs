// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexConstraint
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the native sqlite3_index_constraint structure
  /// from the SQLite core library.
  /// </summary>
  public sealed class SQLiteIndexConstraint
  {
    /// <summary>Column on left-hand side of constraint.</summary>
    public int iColumn;
    /// <summary>
    /// Constraint operator (<see cref="T:System.Data.SQLite.SQLiteIndexConstraintOp" />).
    /// </summary>
    public SQLiteIndexConstraintOp op;
    /// <summary>True if this constraint is usable.</summary>
    public byte usable;
    /// <summary>
    /// Used internally - <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" />
    /// should ignore.
    /// </summary>
    public int iTermOffset;

    /// <summary>
    /// Constructs an instance of this class using the specified native
    /// sqlite3_index_constraint structure.
    /// </summary>
    /// <param name="constraint">
    /// The native sqlite3_index_constraint structure to use.
    /// </param>
    internal SQLiteIndexConstraint(
      UnsafeNativeMethods.sqlite3_index_constraint constraint)
      : this(constraint.iColumn, constraint.op, constraint.usable, constraint.iTermOffset)
    {
    }

    /// <summary>
    /// Constructs an instance of this class using the specified field
    /// values.
    /// </summary>
    /// <param name="iColumn">Column on left-hand side of constraint.</param>
    /// <param name="op">
    /// Constraint operator (<see cref="T:System.Data.SQLite.SQLiteIndexConstraintOp" />).
    /// </param>
    /// <param name="usable">True if this constraint is usable.</param>
    /// <param name="iTermOffset">
    /// Used internally - <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" />
    /// should ignore.
    /// </param>
    private SQLiteIndexConstraint(
      int iColumn,
      SQLiteIndexConstraintOp op,
      byte usable,
      int iTermOffset)
    {
      this.iColumn = iColumn;
      this.op = op;
      this.usable = usable;
      this.iTermOffset = iTermOffset;
    }
  }
}
