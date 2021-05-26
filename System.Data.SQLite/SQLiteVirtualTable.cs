// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteVirtualTable
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents a managed virtual table implementation.  It is
  /// not sealed and should be used as the base class for any user-defined
  /// virtual table classes implemented in managed code.
  /// </summary>
  public class SQLiteVirtualTable : ISQLiteNativeHandle, IDisposable
  {
    /// <summary>
    /// The index within the array of strings provided to the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> and
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> methods containing the
    /// name of the module implementing this virtual table.
    /// </summary>
    private const int ModuleNameIndex = 0;
    /// <summary>
    /// The index within the array of strings provided to the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> and
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> methods containing the
    /// name of the database containing this virtual table.
    /// </summary>
    private const int DatabaseNameIndex = 1;
    /// <summary>
    /// The index within the array of strings provided to the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> and
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> methods containing the
    /// name of the virtual table.
    /// </summary>
    private const int TableNameIndex = 2;
    private string[] arguments;
    private SQLiteIndex index;
    private IntPtr nativeHandle;
    private bool disposed;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="arguments">
    /// The original array of strings provided to the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> and
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> methods.
    /// </param>
    public SQLiteVirtualTable(string[] arguments) => this.arguments = arguments;

    /// <summary>
    /// The original array of strings provided to the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Create(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> and
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Connect(System.Data.SQLite.SQLiteConnection,System.IntPtr,System.String[],System.Data.SQLite.SQLiteVirtualTable@,System.String@)" /> methods.
    /// </summary>
    public virtual string[] Arguments
    {
      get
      {
        this.CheckDisposed();
        return this.arguments;
      }
    }

    /// <summary>
    /// The name of the module implementing this virtual table.
    /// </summary>
    public virtual string ModuleName
    {
      get
      {
        this.CheckDisposed();
        string[] arguments = this.Arguments;
        return arguments != null && arguments.Length > 0 ? arguments[0] : (string) null;
      }
    }

    /// <summary>
    /// The name of the database containing this virtual table.
    /// </summary>
    public virtual string DatabaseName
    {
      get
      {
        this.CheckDisposed();
        string[] arguments = this.Arguments;
        return arguments != null && arguments.Length > 1 ? arguments[1] : (string) null;
      }
    }

    /// <summary>The name of the virtual table.</summary>
    public virtual string TableName
    {
      get
      {
        this.CheckDisposed();
        string[] arguments = this.Arguments;
        return arguments != null && arguments.Length > 2 ? arguments[2] : (string) null;
      }
    }

    /// <summary>
    /// The <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance containing all the
    /// data for the inputs and outputs relating to the most recent index
    /// selection.
    /// </summary>
    public virtual SQLiteIndex Index
    {
      get
      {
        this.CheckDisposed();
        return this.index;
      }
    }

    /// <summary>
    /// This method should normally be used by the
    /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" /> method in order to
    /// perform index selection based on the constraints provided by the
    /// SQLite core library.
    /// </summary>
    /// <param name="index">
    /// The <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance containing all the
    /// data for the inputs and outputs relating to index selection.
    /// </param>
    /// <returns>Non-zero upon success.</returns>
    public virtual bool BestIndex(SQLiteIndex index)
    {
      this.CheckDisposed();
      this.index = index;
      return true;
    }

    /// <summary>
    /// Attempts to record the renaming of the virtual table associated
    /// with this object instance.
    /// </summary>
    /// <param name="name">The new name for the virtual table.</param>
    /// <returns>Non-zero upon success.</returns>
    public virtual bool Rename(string name)
    {
      this.CheckDisposed();
      if (this.arguments == null || this.arguments.Length <= 2)
        return false;
      this.arguments[2] = name;
      return true;
    }

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
        throw new ObjectDisposedException(typeof (SQLiteVirtualTable).Name);
    }

    /// <summary>Disposes of this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this method is being called from the
    /// <see cref="M:System.Data.SQLite.SQLiteVirtualTable.Dispose" /> method.  Zero if this method is being called
    /// from the finalizer.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.disposed = true;
    }

    /// <summary>Finalizes this object instance.</summary>
    ~SQLiteVirtualTable() => this.Dispose(false);
  }
}
