// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteChangeSetEnumerator
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents an <see cref="T:System.Collections.IEnumerator" /> that is capable of
  /// enumerating over a set of changes.  It serves as the base class for the
  /// <see cref="T:System.Data.SQLite.SQLiteMemoryChangeSetEnumerator" /> and
  /// <see cref="T:System.Data.SQLite.SQLiteStreamChangeSetEnumerator" /> classes.  It manages and
  /// owns an instance of the <see cref="T:System.Data.SQLite.SQLiteChangeSetIterator" /> class.
  /// </summary>
  internal abstract class SQLiteChangeSetEnumerator : 
    IEnumerator<ISQLiteChangeSetMetadataItem>,
    IDisposable,
    IEnumerator
  {
    /// <summary>
    /// This managed change set iterator is managed and owned by this
    /// class.  It will be disposed when this class is disposed.
    /// </summary>
    private SQLiteChangeSetIterator iterator;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs an instance of this class using the specified managed
    /// change set iterator.
    /// </summary>
    /// <param name="iterator">The managed iterator instance to use.</param>
    public SQLiteChangeSetEnumerator(SQLiteChangeSetIterator iterator) => this.SetIterator(iterator);

    /// <summary>
    /// Throws an exception if the managed iterator instance is invalid.
    /// </summary>
    private void CheckIterator()
    {
      if (this.iterator == null)
        throw new InvalidOperationException("iterator unavailable");
      this.iterator.CheckHandle();
    }

    /// <summary>Sets the managed iterator instance to a new value.</summary>
    /// <param name="iterator">The new managed iterator instance to use.</param>
    private void SetIterator(SQLiteChangeSetIterator iterator) => this.iterator = iterator;

    /// <summary>
    /// Disposes of the managed iterator instance and sets its value to
    /// null.
    /// </summary>
    private void CloseIterator()
    {
      if (this.iterator == null)
        return;
      this.iterator.Dispose();
      this.iterator = (SQLiteChangeSetIterator) null;
    }

    /// <summary>
    /// Disposes of the existing managed iterator instance and then sets it
    /// to a new value.
    /// </summary>
    /// <param name="iterator">The new managed iterator instance to use.</param>
    protected void ResetIterator(SQLiteChangeSetIterator iterator)
    {
      this.CloseIterator();
      this.SetIterator(iterator);
    }

    /// <summary>
    /// Returns the current change within the set of changes, represented
    /// by a <see cref="T:System.Data.SQLite.ISQLiteChangeSetMetadataItem" /> instance.
    /// </summary>
    public ISQLiteChangeSetMetadataItem Current
    {
      get
      {
        this.CheckDisposed();
        return (ISQLiteChangeSetMetadataItem) new SQLiteChangeSetMetadataItem(this.iterator);
      }
    }

    /// <summary>
    /// Returns the current change within the set of changes, represented
    /// by a <see cref="T:System.Data.SQLite.ISQLiteChangeSetMetadataItem" /> instance.
    /// </summary>
    object IEnumerator.Current
    {
      get
      {
        this.CheckDisposed();
        return (object) this.Current;
      }
    }

    /// <summary>
    /// Attempts to advance to the next item in the set of changes.
    /// </summary>
    /// <returns>
    /// Non-zero if more items are available; otherwise, zero.
    /// </returns>
    public bool MoveNext()
    {
      this.CheckDisposed();
      this.CheckIterator();
      return this.iterator.Next();
    }

    /// <summary>
    /// Throws <see cref="T:System.NotImplementedException" /> because not all the
    /// derived classes are able to support reset functionality.
    /// </summary>
    public virtual void Reset()
    {
      this.CheckDisposed();
      throw new NotImplementedException();
    }

    /// <summary>Disposes of this object instance.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    /// Throws an exception if this object instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteChangeSetEnumerator).Name);
    }

    /// <summary>Disposes or finalizes this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this object is being disposed; otherwise, this object
    /// is being finalized.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed || !disposing)
          return;
        this.CloseIterator();
      }
      finally
      {
        this.disposed = true;
      }
    }

    /// <summary>Finalizes this object instance.</summary>
    ~SQLiteChangeSetEnumerator() => this.Dispose(false);
  }
}
