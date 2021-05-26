// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteVirtualTableCursorEnumerator
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents a virtual table cursor to be used with the
  /// <see cref="T:System.Data.SQLite.SQLiteModuleEnumerable" /> class.  It is not sealed and may
  /// be used as the base class for any user-defined virtual table cursor
  /// class that wraps an <see cref="T:System.Collections.IEnumerator" /> object instance.
  /// </summary>
  public class SQLiteVirtualTableCursorEnumerator : SQLiteVirtualTableCursor, IEnumerator
  {
    /// <summary>
    /// The <see cref="T:System.Collections.IEnumerator" /> instance provided when this cursor
    /// was created.
    /// </summary>
    private IEnumerator enumerator;
    /// <summary>
    /// This value will be non-zero if false has been returned from the
    /// <see cref="M:System.Collections.IEnumerator.MoveNext" /> method.
    /// </summary>
    private bool endOfEnumerator;
    private bool disposed;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="table">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTable" /> object instance associated
    /// with this object instance.
    /// </param>
    /// <param name="enumerator">
    /// The <see cref="T:System.Collections.IEnumerator" /> instance to expose as a virtual
    /// table cursor.
    /// </param>
    public SQLiteVirtualTableCursorEnumerator(SQLiteVirtualTable table, IEnumerator enumerator)
      : base(table)
    {
      this.enumerator = enumerator;
      this.endOfEnumerator = true;
    }

    /// <summary>
    /// Advances to the next row of the virtual table cursor using the
    /// <see cref="M:System.Collections.IEnumerator.MoveNext" /> method of the
    /// <see cref="T:System.Collections.IEnumerator" /> object instance.
    /// </summary>
    /// <returns>
    /// Non-zero if the current row is valid; zero otherwise.  If zero is
    /// returned, no further rows are available.
    /// </returns>
    public virtual bool MoveNext()
    {
      this.CheckDisposed();
      this.CheckClosed();
      if (this.enumerator == null)
        return false;
      this.endOfEnumerator = !this.enumerator.MoveNext();
      if (!this.endOfEnumerator)
        this.NextRowIndex();
      return !this.endOfEnumerator;
    }

    /// <summary>
    /// Returns the value for the current row of the virtual table cursor
    /// using the <see cref="P:System.Collections.IEnumerator.Current" /> property of the
    /// <see cref="T:System.Collections.IEnumerator" /> object instance.
    /// </summary>
    public virtual object Current
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this.enumerator == null ? (object) null : this.enumerator.Current;
      }
    }

    /// <summary>
    /// Resets the virtual table cursor position, also invalidating the
    /// current row, using the <see cref="M:System.Collections.IEnumerator.Reset" /> method of
    /// the <see cref="T:System.Collections.IEnumerator" /> object instance.
    /// </summary>
    public virtual void Reset()
    {
      this.CheckDisposed();
      this.CheckClosed();
      if (this.enumerator == null)
        return;
      this.enumerator.Reset();
    }

    /// <summary>
    /// Returns non-zero if the end of the virtual table cursor has been
    /// seen (i.e. no more rows are available, including the current one).
    /// </summary>
    public virtual bool EndOfEnumerator
    {
      get
      {
        this.CheckDisposed();
        this.CheckClosed();
        return this.endOfEnumerator;
      }
    }

    /// <summary>Returns non-zero if the virtual table cursor is open.</summary>
    public virtual bool IsOpen
    {
      get
      {
        this.CheckDisposed();
        return this.enumerator != null;
      }
    }

    /// <summary>
    /// Closes the virtual table cursor.  This method must not throw any
    /// exceptions.
    /// </summary>
    public virtual void Close()
    {
      if (this.enumerator == null)
        return;
      this.enumerator = (IEnumerator) null;
    }

    /// <summary>
    /// Throws an <see cref="T:System.InvalidOperationException" /> if the virtual
    /// table cursor has been closed.
    /// </summary>
    public virtual void CheckClosed()
    {
      this.CheckDisposed();
      if (!this.IsOpen)
        throw new InvalidOperationException("virtual table cursor is closed");
    }

    /// <summary>
    /// Throws an <see cref="T:System.ObjectDisposedException" /> if this object
    /// instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteVirtualTableCursorEnumerator).Name);
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
