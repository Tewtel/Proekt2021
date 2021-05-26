// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexOrderBy
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the native sqlite3_index_orderby structure from
  /// the SQLite core library.
  /// </summary>
  public sealed class SQLiteIndexOrderBy
  {
    /// <summary>Column number.</summary>
    public int iColumn;
    /// <summary>True for DESC.  False for ASC.</summary>
    public byte desc;

    /// <summary>
    /// Constructs an instance of this class using the specified native
    /// sqlite3_index_orderby structure.
    /// </summary>
    /// <param name="orderBy">
    /// The native sqlite3_index_orderby structure to use.
    /// </param>
    internal SQLiteIndexOrderBy(UnsafeNativeMethods.sqlite3_index_orderby orderBy)
      : this(orderBy.iColumn, orderBy.desc)
    {
    }

    /// <summary>
    /// Constructs an instance of this class using the specified field
    /// values.
    /// </summary>
    /// <param name="iColumn">Column number.</param>
    /// <param name="desc">True for DESC.  False for ASC.</param>
    private SQLiteIndexOrderBy(int iColumn, byte desc)
    {
      this.iColumn = iColumn;
      this.desc = desc;
    }
  }
}
