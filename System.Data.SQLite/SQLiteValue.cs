// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteValue
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents a value from the SQLite core library that can be
  /// passed to the sqlite3_value_*() and associated functions.
  /// </summary>
  public sealed class SQLiteValue : ISQLiteNativeHandle
  {
    /// <summary>The native value handle.</summary>
    private IntPtr pValue;
    private bool persisted;
    private object value;

    /// <summary>
    /// Constructs an instance of this class using the specified native
    /// value handle.
    /// </summary>
    /// <param name="pValue">The native value handle to use.</param>
    private SQLiteValue(IntPtr pValue) => this.pValue = pValue;

    /// <summary>
    /// Invalidates the native value handle, thereby preventing further
    /// access to it from this object instance.
    /// </summary>
    private void PreventNativeAccess() => this.pValue = IntPtr.Zero;

    /// <summary>
    /// Converts a native pointer to a native sqlite3_value structure into
    /// a managed <see cref="T:System.Data.SQLite.SQLiteValue" /> object instance.
    /// </summary>
    /// <param name="pValue">
    /// The native pointer to a native sqlite3_value structure to convert.
    /// </param>
    /// <returns>
    /// The managed <see cref="T:System.Data.SQLite.SQLiteValue" /> object instance or null upon
    /// failure.
    /// </returns>
    internal static SQLiteValue FromIntPtr(IntPtr pValue) => pValue == IntPtr.Zero ? (SQLiteValue) null : new SQLiteValue(pValue);

    /// <summary>
    /// Converts a logical array of native pointers to native sqlite3_value
    /// structures into a managed array of <see cref="T:System.Data.SQLite.SQLiteValue" />
    /// object instances.
    /// </summary>
    /// <param name="argc">
    /// The number of elements in the logical array of native sqlite3_value
    /// structures.
    /// </param>
    /// <param name="argv">
    /// The native pointer to the logical array of native sqlite3_value
    /// structures to convert.
    /// </param>
    /// <returns>
    /// The managed array of <see cref="T:System.Data.SQLite.SQLiteValue" /> object instances or
    /// null upon failure.
    /// </returns>
    internal static SQLiteValue[] ArrayFromSizeAndIntPtr(int argc, IntPtr argv)
    {
      if (argc < 0)
        return (SQLiteValue[]) null;
      if (argv == IntPtr.Zero)
        return (SQLiteValue[]) null;
      SQLiteValue[] sqLiteValueArray = new SQLiteValue[argc];
      int index = 0;
      int offset = 0;
      while (index < sqLiteValueArray.Length)
      {
        IntPtr pValue = SQLiteMarshal.ReadIntPtr(argv, offset);
        sqLiteValueArray[index] = pValue != IntPtr.Zero ? new SQLiteValue(pValue) : (SQLiteValue) null;
        ++index;
        offset += IntPtr.Size;
      }
      return sqLiteValueArray;
    }

    /// <summary>
    /// Returns the underlying SQLite native handle associated with this
    /// object instance.
    /// </summary>
    public IntPtr NativeHandle => this.pValue;

    /// <summary>
    /// Returns non-zero if the native SQLite value has been successfully
    /// persisted as a managed value within this object instance (i.e. the
    /// <see cref="P:System.Data.SQLite.SQLiteValue.Value" /> property may then be read successfully).
    /// </summary>
    public bool Persisted => this.persisted;

    /// <summary>
    /// If the managed value for this object instance is available (i.e. it
    /// has been previously persisted via the <see cref="M:System.Data.SQLite.SQLiteValue.Persist" />) method,
    /// that value is returned; otherwise, an exception is thrown.  The
    /// returned value may be null.
    /// </summary>
    public object Value
    {
      get
      {
        if (!this.persisted)
          throw new InvalidOperationException("value was not persisted");
        return this.value;
      }
    }

    /// <summary>
    /// Gets and returns the type affinity associated with this value.
    /// </summary>
    /// <returns>The type affinity associated with this value.</returns>
    public TypeAffinity GetTypeAffinity() => this.pValue == IntPtr.Zero ? TypeAffinity.None : UnsafeNativeMethods.sqlite3_value_type(this.pValue);

    /// <summary>
    /// Gets and returns the number of bytes associated with this value, if
    /// it refers to a UTF-8 encoded string.
    /// </summary>
    /// <returns>
    /// The number of bytes associated with this value.  The returned value
    /// may be zero.
    /// </returns>
    public int GetBytes() => this.pValue == IntPtr.Zero ? 0 : UnsafeNativeMethods.sqlite3_value_bytes(this.pValue);

    /// <summary>
    /// Gets and returns the <see cref="T:System.Int32" /> associated with this
    /// value.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Int32" /> associated with this value.
    /// </returns>
    public int GetInt() => this.pValue == IntPtr.Zero ? 0 : UnsafeNativeMethods.sqlite3_value_int(this.pValue);

    /// <summary>
    /// Gets and returns the <see cref="T:System.Int64" /> associated with
    /// this value.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Int64" /> associated with this value.
    /// </returns>
    public long GetInt64() => this.pValue == IntPtr.Zero ? 0L : UnsafeNativeMethods.sqlite3_value_int64(this.pValue);

    /// <summary>
    /// Gets and returns the <see cref="T:System.Double" /> associated with this
    /// value.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Double" /> associated with this value.
    /// </returns>
    public double GetDouble() => this.pValue == IntPtr.Zero ? 0.0 : UnsafeNativeMethods.sqlite3_value_double(this.pValue);

    /// <summary>
    /// Gets and returns the <see cref="T:System.String" /> associated with this
    /// value.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.String" /> associated with this value.  The value is
    /// converted from the UTF-8 encoding prior to being returned.
    /// </returns>
    public string GetString()
    {
      if (this.pValue == IntPtr.Zero)
        return (string) null;
      int len = 0;
      return SQLiteString.StringFromUtf8IntPtr(UnsafeNativeMethods.sqlite3_value_text_interop(this.pValue, ref len), len);
    }

    /// <summary>
    /// Gets and returns the <see cref="T:System.Byte" /> array associated with this
    /// value.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Byte" /> array associated with this value.
    /// </returns>
    public byte[] GetBlob() => this.pValue == IntPtr.Zero ? (byte[]) null : SQLiteBytes.FromIntPtr(UnsafeNativeMethods.sqlite3_value_blob(this.pValue), this.GetBytes());

    /// <summary>
    /// Gets and returns an <see cref="T:System.Object" /> instance associated with
    /// this value.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Object" /> associated with this value.  If the type
    /// affinity of the object is unknown or cannot be determined, a null
    /// value will be returned.
    /// </returns>
    public object GetObject()
    {
      switch (this.GetTypeAffinity())
      {
        case TypeAffinity.Uninitialized:
          return (object) null;
        case TypeAffinity.Int64:
          return (object) this.GetInt64();
        case TypeAffinity.Double:
          return (object) this.GetDouble();
        case TypeAffinity.Text:
          return (object) this.GetString();
        case TypeAffinity.Blob:
          return (object) this.GetBytes();
        case TypeAffinity.Null:
          return (object) DBNull.Value;
        default:
          return (object) null;
      }
    }

    /// <summary>
    /// Uses the native value handle to obtain and store the managed value
    /// for this object instance, thus saving it for later use.  The type
    /// of the managed value is determined by the type affinity of the
    /// native value.  If the type affinity is not recognized by this
    /// method, no work is done and false is returned.
    /// </summary>
    /// <returns>
    /// Non-zero if the native value was persisted successfully.
    /// </returns>
    public bool Persist()
    {
      switch (this.GetTypeAffinity())
      {
        case TypeAffinity.Uninitialized:
          this.value = (object) null;
          this.PreventNativeAccess();
          return this.persisted = true;
        case TypeAffinity.Int64:
          this.value = (object) this.GetInt64();
          this.PreventNativeAccess();
          return this.persisted = true;
        case TypeAffinity.Double:
          this.value = (object) this.GetDouble();
          this.PreventNativeAccess();
          return this.persisted = true;
        case TypeAffinity.Text:
          this.value = (object) this.GetString();
          this.PreventNativeAccess();
          return this.persisted = true;
        case TypeAffinity.Blob:
          this.value = (object) this.GetBytes();
          this.PreventNativeAccess();
          return this.persisted = true;
        case TypeAffinity.Null:
          this.value = (object) DBNull.Value;
          this.PreventNativeAccess();
          return this.persisted = true;
        default:
          return false;
      }
    }
  }
}
