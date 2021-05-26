// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteContext
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents a context from the SQLite core library that can
  /// be passed to the sqlite3_result_*() and associated functions.
  /// </summary>
  public sealed class SQLiteContext : ISQLiteNativeHandle
  {
    /// <summary>The native context handle.</summary>
    private IntPtr pContext;

    /// <summary>
    /// Constructs an instance of this class using the specified native
    /// context handle.
    /// </summary>
    /// <param name="pContext">The native context handle to use.</param>
    internal SQLiteContext(IntPtr pContext) => this.pContext = pContext;

    /// <summary>
    /// Returns the underlying SQLite native handle associated with this
    /// object instance.
    /// </summary>
    public IntPtr NativeHandle => this.pContext;

    /// <summary>Sets the context result to NULL.</summary>
    public void SetNull()
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_null(this.pContext);
    }

    /// <summary>
    /// Sets the context result to the specified <see cref="T:System.Double" />
    /// value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="T:System.Double" /> value to use.
    /// </param>
    public void SetDouble(double value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_double(this.pContext, value);
    }

    /// <summary>
    /// Sets the context result to the specified <see cref="T:System.Int32" />
    /// value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="T:System.Int32" /> value to use.
    /// </param>
    public void SetInt(int value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_int(this.pContext, value);
    }

    /// <summary>
    /// Sets the context result to the specified <see cref="T:System.Int64" />
    /// value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="T:System.Int64" /> value to use.
    /// </param>
    public void SetInt64(long value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_int64(this.pContext, value);
    }

    /// <summary>
    /// Sets the context result to the specified <see cref="T:System.String" />
    /// value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="T:System.String" /> value to use.  This value will be
    /// converted to the UTF-8 encoding prior to being used.
    /// </param>
    public void SetString(string value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      byte[] utf8BytesFromString = SQLiteString.GetUtf8BytesFromString(value);
      if (utf8BytesFromString == null)
        throw new ArgumentNullException(nameof (value));
      UnsafeNativeMethods.sqlite3_result_text(this.pContext, utf8BytesFromString, utf8BytesFromString.Length, (IntPtr) -1);
    }

    /// <summary>
    /// Sets the context result to the specified <see cref="T:System.String" />
    /// value containing an error message.
    /// </summary>
    /// <param name="value">
    /// The <see cref="T:System.String" /> value containing the error message text.
    /// This value will be converted to the UTF-8 encoding prior to being
    /// used.
    /// </param>
    public void SetError(string value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      byte[] utf8BytesFromString = SQLiteString.GetUtf8BytesFromString(value);
      if (utf8BytesFromString == null)
        throw new ArgumentNullException(nameof (value));
      UnsafeNativeMethods.sqlite3_result_error(this.pContext, utf8BytesFromString, utf8BytesFromString.Length);
    }

    /// <summary>
    /// Sets the context result to the specified <see cref="T:System.Data.SQLite.SQLiteErrorCode" />
    /// value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> value to use.
    /// </param>
    public void SetErrorCode(SQLiteErrorCode value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_error_code(this.pContext, value);
    }

    /// <summary>
    /// Sets the context result to contain the error code SQLITE_TOOBIG.
    /// </summary>
    public void SetErrorTooBig()
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_error_toobig(this.pContext);
    }

    /// <summary>
    /// Sets the context result to contain the error code SQLITE_NOMEM.
    /// </summary>
    public void SetErrorNoMemory()
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_error_nomem(this.pContext);
    }

    /// <summary>
    /// Sets the context result to the specified <see cref="T:System.Byte" /> array
    /// value.
    /// </summary>
    /// <param name="value">
    /// The <see cref="T:System.Byte" /> array value to use.
    /// </param>
    public void SetBlob(byte[] value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      UnsafeNativeMethods.sqlite3_result_blob(this.pContext, value, value.Length, (IntPtr) -1);
    }

    /// <summary>
    /// Sets the context result to a BLOB of zeros of the specified size.
    /// </summary>
    /// <param name="value">
    /// The number of zero bytes to use for the BLOB context result.
    /// </param>
    public void SetZeroBlob(int value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      UnsafeNativeMethods.sqlite3_result_zeroblob(this.pContext, value);
    }

    /// <summary>
    /// Sets the context result to the specified <see cref="T:System.Data.SQLite.SQLiteValue" />.
    /// </summary>
    /// <param name="value">
    /// The <see cref="T:System.Data.SQLite.SQLiteValue" /> to use.
    /// </param>
    public void SetValue(SQLiteValue value)
    {
      if (this.pContext == IntPtr.Zero)
        throw new InvalidOperationException();
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      UnsafeNativeMethods.sqlite3_result_value(this.pContext, value.NativeHandle);
    }
  }
}
