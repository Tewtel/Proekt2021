// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexInputs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the various inputs provided by the SQLite core
  /// library to the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" /> method.
  /// </summary>
  public sealed class SQLiteIndexInputs
  {
    private SQLiteIndexConstraint[] constraints;
    private SQLiteIndexOrderBy[] orderBys;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="nConstraint">
    /// The number of <see cref="T:System.Data.SQLite.SQLiteIndexConstraint" /> instances to
    /// pre-allocate space for.
    /// </param>
    /// <param name="nOrderBy">
    /// The number of <see cref="T:System.Data.SQLite.SQLiteIndexOrderBy" /> instances to
    /// pre-allocate space for.
    /// </param>
    internal SQLiteIndexInputs(int nConstraint, int nOrderBy)
    {
      this.constraints = new SQLiteIndexConstraint[nConstraint];
      this.orderBys = new SQLiteIndexOrderBy[nOrderBy];
    }

    /// <summary>
    /// An array of <see cref="T:System.Data.SQLite.SQLiteIndexConstraint" /> object instances,
    /// each containing information supplied by the SQLite core library.
    /// </summary>
    public SQLiteIndexConstraint[] Constraints => this.constraints;

    /// <summary>
    /// An array of <see cref="T:System.Data.SQLite.SQLiteIndexOrderBy" /> object instances,
    /// each containing information supplied by the SQLite core library.
    /// </summary>
    public SQLiteIndexOrderBy[] OrderBys => this.orderBys;
  }
}
