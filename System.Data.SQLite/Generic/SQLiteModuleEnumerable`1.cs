// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Generic.SQLiteModuleEnumerable`1
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Data.SQLite.Generic
{
  /// <summary>
  /// This class implements a virtual table module that exposes an
  /// <see cref="T:System.Collections.Generic.IEnumerable`1" /> object instance as a read-only virtual
  /// table.  It is not sealed and may be used as the base class for any
  /// user-defined virtual table class that wraps an
  /// <see cref="T:System.Collections.Generic.IEnumerable`1" /> object instance.
  /// </summary>
  public class SQLiteModuleEnumerable<T> : SQLiteModuleEnumerable
  {
    /// <summary>
    /// The <see cref="T:System.Collections.Generic.IEnumerable`1" /> instance containing the backing
    /// data for the virtual table.
    /// </summary>
    private IEnumerable<T> enumerable;
    private bool disposed;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="name">
    /// The name of the module.  This parameter cannot be null.
    /// </param>
    /// <param name="enumerable">
    /// The <see cref="T:System.Collections.Generic.IEnumerable`1" /> instance to expose as a virtual
    /// table.  This parameter cannot be null.
    /// </param>
    public SQLiteModuleEnumerable(string name, IEnumerable<T> enumerable)
      : base(name, (IEnumerable) enumerable)
    {
      this.enumerable = enumerable;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Open(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteVirtualTableCursor@)" /> method.
    /// </summary>
    /// <param name="table">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Open(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteVirtualTableCursor@)" /> method.
    /// </param>
    /// <param name="cursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Open(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteVirtualTableCursor@)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Open(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteVirtualTableCursor@)" /> method.
    /// </returns>
    public override SQLiteErrorCode Open(
      SQLiteVirtualTable table,
      ref SQLiteVirtualTableCursor cursor)
    {
      this.CheckDisposed();
      cursor = (SQLiteVirtualTableCursor) new SQLiteVirtualTableCursorEnumerator<T>(table, this.enumerable.GetEnumerator());
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Column(System.Data.SQLite.SQLiteVirtualTableCursor,System.Data.SQLite.SQLiteContext,System.Int32)" /> method.
    /// </summary>
    /// <param name="cursor">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Column(System.Data.SQLite.SQLiteVirtualTableCursor,System.Data.SQLite.SQLiteContext,System.Int32)" /> method.
    /// </param>
    /// <param name="context">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Column(System.Data.SQLite.SQLiteVirtualTableCursor,System.Data.SQLite.SQLiteContext,System.Int32)" /> method.
    /// </param>
    /// <param name="index">
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Column(System.Data.SQLite.SQLiteVirtualTableCursor,System.Data.SQLite.SQLiteContext,System.Int32)" /> method.
    /// </param>
    /// <returns>
    /// See the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.Column(System.Data.SQLite.SQLiteVirtualTableCursor,System.Data.SQLite.SQLiteContext,System.Int32)" /> method.
    /// </returns>
    public override SQLiteErrorCode Column(
      SQLiteVirtualTableCursor cursor,
      SQLiteContext context,
      int index)
    {
      this.CheckDisposed();
      if (!(cursor is SQLiteVirtualTableCursorEnumerator<T> cursorEnumerator))
        return this.CursorTypeMismatchError(cursor, typeof (SQLiteVirtualTableCursorEnumerator));
      if (cursorEnumerator.EndOfEnumerator)
        return this.CursorEndOfEnumeratorError(cursor);
      T current = ((IEnumerator<T>) cursorEnumerator).Current;
      if ((object) current != null)
        context.SetString(this.GetStringFromObject(cursor, (object) current));
      else
        context.SetNull();
      return SQLiteErrorCode.Ok;
    }

    /// <summary>
    /// Throws an <see cref="T:System.ObjectDisposedException" /> if this object
    /// instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteModuleEnumerable<T>).Name);
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
        int num = this.disposed ? 1 : 0;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
