// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteSessionStreamManager
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;
using System.IO;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class manages a collection of <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" />
  /// instances. When used, it takes responsibility for creating, returning,
  /// and disposing of its <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" /> instances.
  /// </summary>
  internal sealed class SQLiteSessionStreamManager : IDisposable
  {
    /// <summary>
    /// The managed collection of <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" />
    /// instances, keyed by their associated <see cref="T:System.IO.Stream" />
    /// instance.
    /// </summary>
    private Dictionary<Stream, SQLiteStreamAdapter> streamAdapters;
    /// <summary>The flags associated with the connection.</summary>
    private SQLiteConnectionFlags flags;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs a new instance of this class using the specified
    /// connection flags.
    /// </summary>
    /// <param name="flags">
    /// The flags associated with the parent connection.
    /// </param>
    public SQLiteSessionStreamManager(SQLiteConnectionFlags flags)
    {
      this.flags = flags;
      this.InitializeStreamAdapters();
    }

    /// <summary>
    /// Makes sure the collection of <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" />
    /// is created.
    /// </summary>
    private void InitializeStreamAdapters()
    {
      if (this.streamAdapters != null)
        return;
      this.streamAdapters = new Dictionary<Stream, SQLiteStreamAdapter>();
    }

    /// <summary>
    /// Makes sure the collection of <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" />
    /// is disposed.
    /// </summary>
    private void DisposeStreamAdapters()
    {
      if (this.streamAdapters == null)
        return;
      foreach (KeyValuePair<Stream, SQLiteStreamAdapter> streamAdapter in this.streamAdapters)
        streamAdapter.Value?.Dispose();
      this.streamAdapters.Clear();
      this.streamAdapters = (Dictionary<Stream, SQLiteStreamAdapter>) null;
    }

    /// <summary>
    /// Attempts to return a <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" /> instance
    /// suitable for the specified <see cref="T:System.IO.Stream" />.
    /// </summary>
    /// <param name="stream">
    /// The <see cref="T:System.IO.Stream" /> instance.  If this value is null, a null
    /// value will be returned.
    /// </param>
    /// <returns>
    /// A <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" /> instance.  Typically, these
    /// are always freshly created; however, this method is designed to
    /// return the existing <see cref="T:System.Data.SQLite.SQLiteStreamAdapter" /> instance
    /// associated with the specified stream, should one exist.
    /// </returns>
    public SQLiteStreamAdapter GetAdapter(Stream stream)
    {
      this.CheckDisposed();
      if (stream == null)
        return (SQLiteStreamAdapter) null;
      SQLiteStreamAdapter liteStreamAdapter;
      if (this.streamAdapters.TryGetValue(stream, out liteStreamAdapter))
        return liteStreamAdapter;
      liteStreamAdapter = new SQLiteStreamAdapter(stream, this.flags);
      this.streamAdapters.Add(stream, liteStreamAdapter);
      return liteStreamAdapter;
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
        throw new ObjectDisposedException(typeof (SQLiteSessionStreamManager).Name);
    }

    /// <summary>Disposes or finalizes this object instance.</summary>
    /// <param name="disposing">
    /// Non-zero if this object is being disposed; otherwise, this object
    /// is being finalized.
    /// </param>
    private void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed || !disposing)
          return;
        this.DisposeStreamAdapters();
      }
      finally
      {
        this.disposed = true;
      }
    }

    /// <summary>Finalizes this object instance.</summary>
    ~SQLiteSessionStreamManager() => this.Dispose(false);
  }
}
