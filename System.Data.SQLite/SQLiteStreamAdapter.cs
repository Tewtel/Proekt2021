// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteStreamAdapter
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class is used to act as a bridge between a <see cref="T:System.IO.Stream" />
  /// instance and the delegates used with the native streaming API.
  /// </summary>
  internal sealed class SQLiteStreamAdapter : IDisposable
  {
    /// <summary>
    /// The managed stream instance used to in order to service the native
    /// delegates for both input and output.
    /// </summary>
    private Stream stream;
    /// <summary>The flags associated with the connection.</summary>
    private SQLiteConnectionFlags flags;
    /// <summary>
    /// The delegate used to provide input to the native streaming API.
    /// It will be null -OR- point to the <see cref="M:System.Data.SQLite.SQLiteStreamAdapter.Input(System.IntPtr,System.IntPtr,System.Int32@)" /> method.
    /// </summary>
    private UnsafeNativeMethods.xSessionInput xInput;
    /// <summary>
    /// The delegate used to provide output to the native streaming API.
    /// It will be null -OR- point to the <see cref="M:System.Data.SQLite.SQLiteStreamAdapter.Output(System.IntPtr,System.IntPtr,System.Int32)" /> method.
    /// </summary>
    private UnsafeNativeMethods.xSessionOutput xOutput;
    /// <summary>Non-zero if this object instance has been disposed.</summary>
    private bool disposed;

    /// <summary>
    /// Constructs a new instance of this class using the specified managed
    /// stream and connection flags.
    /// </summary>
    /// <param name="stream">
    /// The managed stream instance to be used in order to service the
    /// native delegates for both input and output.
    /// </param>
    /// <param name="flags">
    /// The flags associated with the parent connection.
    /// </param>
    public SQLiteStreamAdapter(Stream stream, SQLiteConnectionFlags flags)
    {
      this.stream = stream;
      this.flags = flags;
    }

    /// <summary>
    /// Queries and returns the flags associated with the connection for
    /// this instance.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.SQLite.SQLiteConnectionFlags" /> value.  There is no return
    /// value reserved to indicate an error.
    /// </returns>
    private SQLiteConnectionFlags GetFlags() => this.flags;

    /// <summary>
    /// Returns a delegate that wraps the <see cref="M:System.Data.SQLite.SQLiteStreamAdapter.Input(System.IntPtr,System.IntPtr,System.Int32@)" /> method,
    /// creating it first if necessary.
    /// </summary>
    /// <returns>
    /// A delegate that refers to the <see cref="M:System.Data.SQLite.SQLiteStreamAdapter.Input(System.IntPtr,System.IntPtr,System.Int32@)" /> method.
    /// </returns>
    public UnsafeNativeMethods.xSessionInput GetInputDelegate()
    {
      this.CheckDisposed();
      if (this.xInput == null)
        this.xInput = new UnsafeNativeMethods.xSessionInput(this.Input);
      return this.xInput;
    }

    /// <summary>
    /// Returns a delegate that wraps the <see cref="M:System.Data.SQLite.SQLiteStreamAdapter.Output(System.IntPtr,System.IntPtr,System.Int32)" /> method,
    /// creating it first if necessary.
    /// </summary>
    /// <returns>
    /// A delegate that refers to the <see cref="M:System.Data.SQLite.SQLiteStreamAdapter.Output(System.IntPtr,System.IntPtr,System.Int32)" /> method.
    /// </returns>
    public UnsafeNativeMethods.xSessionOutput GetOutputDelegate()
    {
      this.CheckDisposed();
      if (this.xOutput == null)
        this.xOutput = new UnsafeNativeMethods.xSessionOutput(this.Output);
      return this.xOutput;
    }

    /// <summary>
    /// This method attempts to read <paramref name="nData" /> bytes from
    /// the managed stream, writing them to the <paramref name="pData" />
    /// buffer.
    /// </summary>
    /// <param name="context">
    /// Optional extra context information.  Currently, this will always
    /// have a value of <see cref="F:System.IntPtr.Zero" />.
    /// </param>
    /// <param name="pData">
    /// A preallocated native buffer to receive the requested input bytes.
    /// It must be at least <paramref name="nData" /> bytes in size.
    /// </param>
    /// <param name="nData">
    /// Upon entry, the number of bytes to read.  Upon exit, the number of
    /// bytes actually read.  This value may be zero upon exit.
    /// </param>
    /// <returns>
    /// The value <see cref="F:System.Data.SQLite.SQLiteErrorCode.Ok" /> upon success -OR- an
    /// appropriate error code upon failure.
    /// </returns>
    private SQLiteErrorCode Input(IntPtr context, IntPtr pData, ref int nData)
    {
      try
      {
        Stream stream = this.stream;
        if (stream == null)
          return SQLiteErrorCode.Misuse;
        if (nData > 0)
        {
          byte[] numArray = new byte[nData];
          int length = stream.Read(numArray, 0, nData);
          if (length > 0 && pData != IntPtr.Zero)
            Marshal.Copy(numArray, 0, pData, length);
          nData = length;
        }
        return SQLiteErrorCode.Ok;
      }
      catch (Exception ex)
      {
        try
        {
          if (HelperMethods.LogCallbackExceptions(this.GetFlags()))
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "xSessionInput", (object) ex));
        }
        catch
        {
        }
      }
      return SQLiteErrorCode.IoErr_Read;
    }

    /// <summary>
    /// This method attempts to write <paramref name="nData" /> bytes to
    /// the managed stream, reading them from the <paramref name="pData" />
    /// buffer.
    /// </summary>
    /// <param name="context">
    /// Optional extra context information.  Currently, this will always
    /// have a value of <see cref="F:System.IntPtr.Zero" />.
    /// </param>
    /// <param name="pData">
    /// A preallocated native buffer containing the requested output
    /// bytes.  It must be at least <paramref name="nData" /> bytes in
    /// size.
    /// </param>
    /// <param name="nData">The number of bytes to write.</param>
    /// <returns>
    /// The value <see cref="F:System.Data.SQLite.SQLiteErrorCode.Ok" /> upon success -OR- an
    /// appropriate error code upon failure.
    /// </returns>
    private SQLiteErrorCode Output(IntPtr context, IntPtr pData, int nData)
    {
      try
      {
        Stream stream = this.stream;
        if (stream == null)
          return SQLiteErrorCode.Misuse;
        if (nData > 0)
        {
          byte[] numArray = new byte[nData];
          if (pData != IntPtr.Zero)
            Marshal.Copy(pData, numArray, 0, nData);
          stream.Write(numArray, 0, nData);
        }
        stream.Flush();
        return SQLiteErrorCode.Ok;
      }
      catch (Exception ex)
      {
        try
        {
          if (HelperMethods.LogCallbackExceptions(this.GetFlags()))
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "xSessionOutput", (object) ex));
        }
        catch
        {
        }
      }
      return SQLiteErrorCode.IoErr_Write;
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
        throw new ObjectDisposedException(typeof (SQLiteStreamAdapter).Name);
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
        if (this.xInput != null)
          this.xInput = (UnsafeNativeMethods.xSessionInput) null;
        if (this.xOutput != null)
          this.xOutput = (UnsafeNativeMethods.xSessionOutput) null;
        if (this.stream == null)
          return;
        this.stream = (Stream) null;
      }
      finally
      {
        this.disposed = true;
      }
    }

    /// <summary>Finalizes this object instance.</summary>
    ~SQLiteStreamAdapter() => this.Dispose(false);
  }
}
