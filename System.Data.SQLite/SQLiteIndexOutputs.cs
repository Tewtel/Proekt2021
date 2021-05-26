// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndexOutputs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the various outputs provided to the SQLite core
  /// library by the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" /> method.
  /// </summary>
  public sealed class SQLiteIndexOutputs
  {
    private SQLiteIndexConstraintUsage[] constraintUsages;
    private int indexNumber;
    private string indexString;
    private int needToFreeIndexString;
    private int orderByConsumed;
    private double? estimatedCost;
    private long? estimatedRows;
    private SQLiteIndexFlags? indexFlags;
    private long? columnsUsed;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="nConstraint">
    /// The number of <see cref="T:System.Data.SQLite.SQLiteIndexConstraintUsage" /> instances
    /// to pre-allocate space for.
    /// </param>
    internal SQLiteIndexOutputs(int nConstraint)
    {
      this.constraintUsages = new SQLiteIndexConstraintUsage[nConstraint];
      for (int index = 0; index < nConstraint; ++index)
        this.constraintUsages[index] = new SQLiteIndexConstraintUsage();
    }

    /// <summary>
    /// Determines if the native estimatedRows field can be used, based on
    /// the available version of the SQLite core library.
    /// </summary>
    /// <returns>
    /// Non-zero if the <see cref="P:System.Data.SQLite.SQLiteIndexOutputs.EstimatedRows" /> property is supported
    /// by the SQLite core library.
    /// </returns>
    public bool CanUseEstimatedRows() => UnsafeNativeMethods.sqlite3_libversion_number() >= 3008002;

    /// <summary>
    /// Determines if the native flags field can be used, based on the
    /// available version of the SQLite core library.
    /// </summary>
    /// <returns>
    /// Non-zero if the <see cref="P:System.Data.SQLite.SQLiteIndexOutputs.IndexFlags" /> property is supported by
    /// the SQLite core library.
    /// </returns>
    public bool CanUseIndexFlags() => UnsafeNativeMethods.sqlite3_libversion_number() >= 3009000;

    /// <summary>
    /// Determines if the native flags field can be used, based on the
    /// available version of the SQLite core library.
    /// </summary>
    /// <returns>
    /// Non-zero if the <see cref="P:System.Data.SQLite.SQLiteIndexOutputs.ColumnsUsed" /> property is supported by
    /// the SQLite core library.
    /// </returns>
    public bool CanUseColumnsUsed() => UnsafeNativeMethods.sqlite3_libversion_number() >= 3010000;

    /// <summary>
    /// An array of <see cref="T:System.Data.SQLite.SQLiteIndexConstraintUsage" /> object
    /// instances, each containing information to be supplied to the SQLite
    /// core library.
    /// </summary>
    public SQLiteIndexConstraintUsage[] ConstraintUsages => this.constraintUsages;

    /// <summary>
    /// Number used to help identify the selected index.  This value will
    /// later be provided to the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Filter(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" />
    /// method.
    /// </summary>
    public int IndexNumber
    {
      get => this.indexNumber;
      set => this.indexNumber = value;
    }

    /// <summary>
    /// String used to help identify the selected index.  This value will
    /// later be provided to the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Filter(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" />
    /// method.
    /// </summary>
    public string IndexString
    {
      get => this.indexString;
      set => this.indexString = value;
    }

    /// <summary>
    /// Non-zero if the index string must be freed by the SQLite core
    /// library.
    /// </summary>
    public int NeedToFreeIndexString
    {
      get => this.needToFreeIndexString;
      set => this.needToFreeIndexString = value;
    }

    /// <summary>True if output is already ordered.</summary>
    public int OrderByConsumed
    {
      get => this.orderByConsumed;
      set => this.orderByConsumed = value;
    }

    /// <summary>
    /// Estimated cost of using this index.  Using a null value here
    /// indicates that a default estimated cost value should be used.
    /// </summary>
    public double? EstimatedCost
    {
      get => this.estimatedCost;
      set => this.estimatedCost = value;
    }

    /// <summary>
    /// Estimated number of rows returned.  Using a null value here
    /// indicates that a default estimated rows value should be used.
    /// This property has no effect if the SQLite core library is not at
    /// least version 3.8.2.
    /// </summary>
    public long? EstimatedRows
    {
      get => this.estimatedRows;
      set => this.estimatedRows = value;
    }

    /// <summary>
    /// The flags that should be used with this index.  Using a null value
    /// here indicates that a default flags value should be used.  This
    /// property has no effect if the SQLite core library is not at least
    /// version 3.9.0.
    /// </summary>
    public SQLiteIndexFlags? IndexFlags
    {
      get => this.indexFlags;
      set => this.indexFlags = value;
    }

    /// <summary>
    /// <para>
    /// Indicates which columns of the virtual table may be required by the
    /// current scan.  Virtual table columns are numbered from zero in the
    /// order in which they appear within the CREATE TABLE statement passed
    /// to sqlite3_declare_vtab().  For the first 63 columns (columns 0-62),
    /// the corresponding bit is set within the bit mask if the column may
    /// be required by SQLite.  If the table has at least 64 columns and
    /// any column to the right of the first 63 is required, then bit 63 of
    /// colUsed is also set.  In other words, column iCol may be required
    /// if the expression
    /// </para>
    /// <para><code>
    /// (colUsed &amp; ((sqlite3_uint64)1 &lt;&lt; (iCol&gt;=63 ? 63 : iCol)))
    /// </code></para>
    /// <para>
    /// evaluates to non-zero.  Using a null value here indicates that a
    /// default flags value should be used.  This property has no effect if
    /// the SQLite core library is not at least version 3.10.0.
    /// </para>
    /// </summary>
    public long? ColumnsUsed
    {
      get => this.columnsUsed;
      set => this.columnsUsed = value;
    }
  }
}
