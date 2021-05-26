// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteVirtualTableCursor
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents a managed virtual table cursor implementation.
  /// It is not sealed and should be used as the base class for any
  /// user-defined virtual table cursor classes implemented in managed code.
  /// </summary>
  public class SQLiteVirtualTableCursor : ISQLiteNativeHandle, IDisposable
  {
    /// <summary>
    /// This value represents an invalid integer row sequence number.
    /// </summary>
    protected static readonly int InvalidRowIndex;
    /// <summary>
    /// The field holds the integer row sequence number for the current row
    /// pointed to by this cursor object instance.
    /// </summary>
    private int rowIndex;
    private SQLiteVirtualTable table;
    private int indexNumber;
    private string indexString;
    private SQLiteValue[] values;
    private IntPtr nativeHandle;
    private bool disposed;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this object instance.
    /// </param>
    public SQLiteVirtualTableCursor(SQLiteVirtualTable table)
      : this()
    {
      this.table = table;
    }

    /// <summary>Constructs an instance of this class.</summary>
    private SQLiteVirtualTableCursor() => this.rowIndex = SQLiteVirtualTableCursor.InvalidRowIndex;

    /// <summary>
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this object instance.
    /// </summary>
    public virtual SQLiteVirtualTable Table
    {
      get
      {
        this.CheckDisposed();
        return this.table;
      }
    }

    /// <summary>
    /// Number used to help identify the selected index.  This value will
    /// be set via the <see cref="M:System.Data.SQLite.SQLiteVirtualTableCursor.Filter(System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" /> method.
    /// </summary>
    public virtual int IndexNumber
    {
      get
      {
        this.CheckDisposed();
        return this.indexNumber;
      }
    }

    /// <summary>
    /// String used to help identify the selected index.  This value will
    /// be set via the <see cref="M:System.Data.SQLite.SQLiteVirtualTableCursor.Filter(System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" /> method.
    /// </summary>
    public virtual string IndexString
    {
      get
      {
        this.CheckDisposed();
        return this.indexString;
      }
    }

    /// <summary>
    /// The values used to filter the rows returned via this cursor object
    /// instance.  This value will be set via the <see cref="M:System.Data.SQLite.SQLiteVirtualTableCursor.Filter(System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" />
    /// method.
    /// </summary>
    public virtual SQLiteValue[] Values
    {
      get
      {
        this.CheckDisposed();
        return this.values;
      }
    }

    /// <summary>
    /// Attempts to persist the specified <see cref="T:System.Data.SQLite.SQLiteValue" /> object
    /// instances in order to make them available after the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Filter(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" /> method returns.
    /// </summary>
    /// <param name="values">
    /// The array of <see cref="T:System.Data.SQLite.SQLiteValue" /> object instances to be
    /// persisted.
    /// </param>
    /// <returns>
    /// The number of <see cref="T:System.Data.SQLite.SQLiteValue" /> object instances that were
    /// successfully persisted.
    /// </returns>
    protected virtual int TryPersistValues(SQLiteValue[] values)
    {
      int num = 0;
      if (values != null)
      {
        foreach (SQLiteValue sqLiteValue in values)
        {
          if (sqLiteValue != null && sqLiteValue.Persist())
            ++num;
        }
      }
      return num;
    }

    /// <summary>
    /// This method should normally be used by the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Filter(System.Data.SQLite.SQLiteVirtualTableCursor,System.Int32,System.String,System.Data.SQLite.SQLiteValue[])" /> method in order to
    /// perform filtering of the result rows and/or to record the filtering
    /// criteria provided by the SQLite core library.
    /// </summary>
    /// <param name="indexNumber">
    /// Number used to help identify the selected index.
    /// </param>
    /// <param name="indexString">
    /// String used to help identify the selected index.
    /// </param>
    /// <param name="values">
    /// The values corresponding to each column in the selected index.
    /// </param>
    public virtual void Filter(int indexNumber, string indexString, SQLiteValue[] values)
    {
      this.CheckDisposed();
      if (values != null && this.TryPersistValues(values) != values.Length)
        throw new SQLiteException("failed to persist one or more values");
      this.indexNumber = indexNumber;
      this.indexString = indexString;
      this.values = values;
    }

    /// <summary>
    /// Determines the integer row sequence number for the current row.
    /// </summary>
    /// <returns>
    /// The integer row sequence number for the current row -OR- zero if
    /// it cannot be determined.
    /// </returns>
    public virtual int GetRowIndex() => this.rowIndex;

    /// <summary>
    /// Adjusts the integer row sequence number so that it refers to the
    /// next row.
    /// </summary>
    public virtual void NextRowIndex() => ++this.rowIndex;

    /// <summary>
    /// Returns the underlying SQLite native handle associated with this
    /// object instance.
    /// </summary>
    public virtual IntPtr NativeHandle
    {
      get
      {
        this.CheckDisposed();
        return this.nativeHandle;
      }
      internal set => this.nativeHandle = value;
    }

    /// <summary>Disposes of this object instance.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// Throws an <see cref="T:System.ObjectDisposedException" /> if this object
    /// instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteVirtualTableCursor).Name);
    }

    /// <summary>Disposes of this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this method is being called from the
    /// <see cref="M:System.Data.SQLite.SQLiteVirtualTableCursor.Dispose" /> method.  Zero if this method is being called
    /// from the finalizer.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
    }

    /// <summary>Finalizes this object instance.</summary>
    ~SQLiteVirtualTableCursor() => this.Dispose(false);
  }
}
