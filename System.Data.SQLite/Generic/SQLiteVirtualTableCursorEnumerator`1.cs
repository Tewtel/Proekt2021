// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Generic.SQLiteVirtualTableCursorEnumerator`1
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Data.SQLite.Generic
{
  /// <summary>
  /// This class represents a virtual table cursor to be used with the
  /// <see cref="T:System.Data.SQLite.SQLiteModuleEnumerable" /> class.  It is not sealed and may
  /// be used as the base class for any user-defined virtual table cursor
  /// class that wraps an <see cref="T:System.Collections.Generic.IEnumerator`1" /> object instance.
  /// </summary>
  public class SQLiteVirtualTableCursorEnumerator<T> : 
    SQLiteVirtualTableCursorEnumerator,
    IEnumerator<T>,
    IDisposable,
    IEnumerator
  {
    /// <summary>
    /// The <see cref="T:System.Collections.Generic.IEnumerator`1" /> instance provided when this
    /// cursor was created.
    /// </summary>
    private IEnumerator<T> enumerator;
    private bool disposed;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this object instance.
    /// </param>
    /// <param name="enumerator">
    /// The <see cref="T:System.Collections.Generic.IEnumerator`1" /> instance to expose as a virtual
    /// table cursor.
    /// </param>
    public SQLiteVirtualTableCursorEnumerator(SQLiteVirtualTable table, IEnumerator<T> enumerator)
      : base(table, (IEnumerator) enumerator)
    {
      this.enumerator = enumerator;
    }

    /// <summary>
    /// Returns the value for the current row of the virtual table cursor
    /// using the <see cref="P:System.Collections.Generic.IEnumerator`1.Current" /> property of the
    /// <see cref="T:System.Collections.Generic.IEnumerator`1" /> object instance.
    /// </summary>
    T IEnumerator<T>.Current
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this.enumerator == null ? default (T) : this.enumerator.Current;
      }
    }

    /// <summary>
    /// Closes the virtual table cursor.  This method must not throw any
    /// exceptions.
    /// </summary>
    public override void Close()
    {
      if (this.enumerator != null)
        this.enumerator = (IEnumerator<T>) null;
      base.Close();
    }

    /// <summary>
    /// Throws an <see cref="T:System.ObjectDisposedException" /> if this object
    /// instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteVirtualTableCursorEnumerator<T>).Name);
    }

    /// <summary>Disposes of this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this method is being called from the
    /// <see cref="M:System.IDisposable.Dispose" /> method.  Zero if this method is
    /// being called from the finalizer.
    /// </param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed)
          return;
        this.Close();
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
