// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteMemoryChangeSetEnumerator
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents an <see cref="T:System.Collections.IEnumerator" /> that is capable of
  /// enumerating over a set of changes contained entirely in memory.
  /// </summary>
  internal sealed class SQLiteMemoryChangeSetEnumerator : SQLiteChangeSetEnumerator
  {
    /// <summary>
    /// The raw byte data for this set of changes.  Since this data must
    /// be marshalled to a native memory buffer before being used, there
    /// must be enough memory available to store at least two times the
    /// amount of data contained within it.
    /// </summary>
    private byte[] rawData;
    /// <summary>The flags used to create the change set iterator.</summary>
    private SQLiteChangeSetStartFlags flags;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs an instance of this class using the specified raw byte
    /// data.
    /// </summary>
    /// <param name="rawData">
    /// The raw byte data containing the set of changes for this
    /// enumerator.
    /// </param>
    public SQLiteMemoryChangeSetEnumerator(byte[] rawData)
      : base((SQLiteChangeSetIterator) SQLiteMemoryChangeSetIterator.Create(rawData))
    {
      this.rawData = rawData;
      this.flags = SQLiteChangeSetStartFlags.None;
    }

    /// <summary>
    /// Constructs an instance of this class using the specified raw byte
    /// data.
    /// </summary>
    /// <param name="rawData">
    /// The raw byte data containing the set of changes for this
    /// enumerator.
    /// </param>
    /// <param name="flags">
    /// The flags used to create the change set iterator.
    /// </param>
    public SQLiteMemoryChangeSetEnumerator(byte[] rawData, SQLiteChangeSetStartFlags flags)
      : base((SQLiteChangeSetIterator) SQLiteMemoryChangeSetIterator.Create(rawData, flags))
    {
      this.rawData = rawData;
      this.flags = flags;
    }

    /// <summary>Resets the enumerator to its initial position.</summary>
    public override void Reset()
    {
      this.CheckDisposed();
      this.ResetIterator(this.flags == SQLiteChangeSetStartFlags.None ? (SQLiteChangeSetIterator) SQLiteMemoryChangeSetIterator.Create(this.rawData) : (SQLiteChangeSetIterator) SQLiteMemoryChangeSetIterator.Create(this.rawData, this.flags));
    }

    /// <summary>
    /// Throws an exception if this object instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteMemoryChangeSetEnumerator).Name);
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
        if (this.disposed)
          return;
        int num = disposing ? 1 : 0;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
