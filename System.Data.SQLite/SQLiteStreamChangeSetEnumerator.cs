// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteStreamChangeSetEnumerator
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.IO;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents an <see cref="T:System.Collections.IEnumerator" /> that is capable of
  /// enumerating over a set of changes backed by a <see cref="T:System.IO.Stream" />
  /// instance.
  /// </summary>
  internal sealed class SQLiteStreamChangeSetEnumerator : SQLiteChangeSetEnumerator
  {
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs an instance of this class using the specified stream.
    /// </summary>
    /// <param name="stream">
    /// The <see cref="T:System.IO.Stream" /> where the raw byte data for the set of
    /// changes may be read.
    /// </param>
    /// <param name="connectionFlags">
    /// The flags associated with the parent connection.
    /// </param>
    public SQLiteStreamChangeSetEnumerator(Stream stream, SQLiteConnectionFlags connectionFlags)
      : base((SQLiteChangeSetIterator) SQLiteStreamChangeSetIterator.Create(stream, connectionFlags))
    {
    }

    /// <summary>
    /// Constructs an instance of this class using the specified stream.
    /// </summary>
    /// <param name="stream">
    /// The <see cref="T:System.IO.Stream" /> where the raw byte data for the set of
    /// changes may be read.
    /// </param>
    /// <param name="connectionFlags">
    /// The flags associated with the parent connection.
    /// </param>
    /// <param name="startFlags">
    /// The flags used to create the change set iterator.
    /// </param>
    public SQLiteStreamChangeSetEnumerator(
      Stream stream,
      SQLiteConnectionFlags connectionFlags,
      SQLiteChangeSetStartFlags startFlags)
      : base((SQLiteChangeSetIterator) SQLiteStreamChangeSetIterator.Create(stream, connectionFlags, startFlags))
    {
    }

    /// <summary>
    /// Throws an exception if this object instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteStreamChangeSetEnumerator).Name);
    }

    /// <summary>Disposes or finalizes this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this object is being disposed; otherwise, this object
    /// is being finalized.
    /// </param>
    protected override void Dispose(bool disposing)
    {
      try
      {
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
