// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteChangeSetBase
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Globalization;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the abstract concept of a set of changes.  It
  /// acts as the base class for the <see cref="T:System.Data.SQLite.SQLiteMemoryChangeSet" />
  /// and <see cref="T:System.Data.SQLite.SQLiteStreamChangeSet" /> classes.  It derives from
  /// the <see cref="T:System.Data.SQLite.SQLiteConnectionLock" /> class, which is used to hold
  /// the underlying native connection handle open until the instances of
  /// this class are disposed or finalized.  It also provides the ability
  /// to construct wrapped native delegates of the
  /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.xSessionFilter" /> and
  /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.xSessionConflict" /> types.
  /// </summary>
  internal class SQLiteChangeSetBase : SQLiteConnectionLock
  {
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs an instance of this class using the specified wrapped
    /// native connection handle.
    /// </summary>
    /// <param name="handle">
    /// The wrapped native connection handle to be associated with this
    /// change set.
    /// </param>
    /// <param name="flags">
    /// The flags associated with the connection represented by the
    /// <paramref name="handle" /> value.
    /// </param>
    internal SQLiteChangeSetBase(SQLiteConnectionHandle handle, SQLiteConnectionFlags flags)
      : base(handle, flags, true)
    {
    }

    /// <summary>
    /// Creates and returns a concrete implementation of the
    /// <see cref="T:System.Data.SQLite.ISQLiteChangeSetMetadataItem" /> interface.
    /// </summary>
    /// <param name="iterator">The native iterator handle to use.</param>
    /// <returns>
    /// An instance of the <see cref="T:System.Data.SQLite.ISQLiteChangeSetMetadataItem" />
    /// interface, which can be used to fetch metadata associated with
    /// the current item in this set of changes.
    /// </returns>
    private ISQLiteChangeSetMetadataItem CreateMetadataItem(
      IntPtr iterator)
    {
      return (ISQLiteChangeSetMetadataItem) new SQLiteChangeSetMetadataItem(SQLiteChangeSetIterator.Attach(iterator));
    }

    /// <summary>
    /// Attempts to create a
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.xSessionFilter" /> native delegate
    /// that invokes the specified
    /// <see cref="T:System.Data.SQLite.SessionTableFilterCallback" /> delegate.
    /// </summary>
    /// <param name="tableFilterCallback">
    /// The <see cref="T:System.Data.SQLite.SessionTableFilterCallback" /> to invoke when the
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.xSessionFilter" /> native delegate
    /// is called.  If this value is null then null is returned.
    /// </param>
    /// <param name="clientData">
    /// The optional application-defined context data.  This value may be
    /// null.
    /// </param>
    /// <returns>
    /// The created <see cref="T:System.Data.SQLite.UnsafeNativeMethods.xSessionFilter" />
    /// native delegate -OR- null if it cannot be created.
    /// </returns>
    protected UnsafeNativeMethods.xSessionFilter GetDelegate(
      SessionTableFilterCallback tableFilterCallback,
      object clientData)
    {
      return tableFilterCallback == null ? (UnsafeNativeMethods.xSessionFilter) null : (UnsafeNativeMethods.xSessionFilter) ((context, pTblName) =>
      {
        try
        {
          return tableFilterCallback(clientData, SQLiteString.StringFromUtf8IntPtr(pTblName)) ? 1 : 0;
        }
        catch (Exception ex)
        {
          try
          {
            if (HelperMethods.LogCallbackExceptions(this.GetFlags()))
              SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "xSessionFilter", (object) ex));
          }
          catch
          {
          }
        }
        return 0;
      });
    }

    /// <summary>
    /// Attempts to create a
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.xSessionConflict" /> native delegate
    /// that invokes the specified
    /// <see cref="T:System.Data.SQLite.SessionConflictCallback" /> delegate.
    /// </summary>
    /// <param name="conflictCallback">
    /// The <see cref="T:System.Data.SQLite.SessionConflictCallback" /> to invoke when the
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.xSessionConflict" /> native delegate
    /// is called.  If this value is null then null is returned.
    /// </param>
    /// <param name="clientData">
    /// The optional application-defined context data.  This value may be
    /// null.
    /// </param>
    /// <returns>
    /// The created <see cref="T:System.Data.SQLite.UnsafeNativeMethods.xSessionConflict" />
    /// native delegate -OR- null if it cannot be created.
    /// </returns>
    protected UnsafeNativeMethods.xSessionConflict GetDelegate(
      SessionConflictCallback conflictCallback,
      object clientData)
    {
      return conflictCallback == null ? (UnsafeNativeMethods.xSessionConflict) null : (UnsafeNativeMethods.xSessionConflict) ((context, type, iterator) =>
      {
        try
        {
          ISQLiteChangeSetMetadataItem metadataItem = this.CreateMetadataItem(iterator);
          if (metadataItem == null)
            throw new SQLiteException("could not create metadata item");
          return conflictCallback(clientData, type, metadataItem);
        }
        catch (Exception ex)
        {
          try
          {
            if (HelperMethods.LogCallbackExceptions(this.GetFlags()))
              SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "xSessionConflict", (object) ex));
          }
          catch
          {
          }
        }
        return SQLiteChangeSetConflictResult.Abort;
      });
    }

    /// <summary>
    /// Throws an exception if this object instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteChangeSetBase).Name);
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
        this.Unlock();
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }
  }
}
