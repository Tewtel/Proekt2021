// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteModuleCommon
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Globalization;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class contains some virtual methods that may be useful for other
  /// virtual table classes.  It specifically does NOT implement any of the
  /// <see cref="T:System.Data.SQLite.ISQLiteManagedModule" /> interface methods.
  /// </summary>
  public class SQLiteModuleCommon : SQLiteModuleNoop
  {
    /// <summary>
    /// The CREATE TABLE statement used to declare the schema for the
    /// virtual table.
    /// </summary>
    private static readonly string declareSql = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "CREATE TABLE {0}(x);", (object) typeof (SQLiteModuleCommon).Name);
    /// <summary>
    /// Non-zero if different object instances with the same value should
    /// generate different row identifiers, where applicable.  This has no
    /// effect on the .NET Compact Framework.
    /// </summary>
    private bool objectIdentity;
    private bool disposed;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="name">
    /// The name of the module.  This parameter cannot be null.
    /// </param>
    public SQLiteModuleCommon(string name)
      : this(name, false)
    {
    }

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="name">
    /// The name of the module.  This parameter cannot be null.
    /// </param>
    /// <param name="objectIdentity">
    /// Non-zero if different object instances with the same value should
    /// generate different row identifiers, where applicable.  This
    /// parameter has no effect on the .NET Compact Framework.
    /// </param>
    public SQLiteModuleCommon(string name, bool objectIdentity)
      : base(name)
    {
      this.objectIdentity = objectIdentity;
    }

    /// <summary>
    /// Determines the SQL statement used to declare the virtual table.
    /// This method should be overridden in derived classes if they require
    /// a custom virtual table schema.
    /// </summary>
    /// <returns>
    /// The SQL statement used to declare the virtual table -OR- null if it
    /// cannot be determined.
    /// </returns>
    protected virtual string GetSqlForDeclareTable() => SQLiteModuleCommon.declareSql;

    /// <summary>
    /// Sets the table error message to one that indicates the virtual
    /// table cursor is of the wrong type.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance.
    /// </param>
    /// <param name="type">
    /// The <see cref="T:System.Type" /> that the virtual table cursor should be.
    /// </param>
    /// <returns>
    /// The value of <see cref="F:System.Data.SQLite.SQLiteErrorCode.Error" />.
    /// </returns>
    protected virtual SQLiteErrorCode CursorTypeMismatchError(
      SQLiteVirtualTableCursor cursor,
      Type type)
    {
      if (type != (Type) null)
        this.SetCursorError(cursor, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "not a \"{0}\" cursor", (object) type));
      else
        this.SetCursorError(cursor, "cursor type mismatch");
      return SQLiteErrorCode.Error;
    }

    /// <summary>
    /// Determines the string to return as the column value for the object
    /// instance value.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance
    /// associated with the previously opened virtual table cursor to be
    /// used.
    /// </param>
    /// <param name="value">
    /// The object instance to return a string representation for.
    /// </param>
    /// <returns>
    /// The string representation of the specified object instance or null
    /// upon failure.
    /// </returns>
    protected virtual string GetStringFromObject(SQLiteVirtualTableCursor cursor, object value)
    {
      if (value == null)
        return (string) null;
      return value is string ? (string) value : value.ToString();
    }

    /// <summary>
    /// Constructs an <see cref="T:System.Int64" /> unique row identifier from two
    /// <see cref="T:System.Int32" /> values.  The first <see cref="T:System.Int32" /> value
    /// must contain the row sequence number for the current row and the
    /// second value must contain the hash code of the key column value
    /// for the current row.
    /// </summary>
    /// <param name="rowIndex">
    /// The integer row sequence number for the current row.
    /// </param>
    /// <param name="hashCode">
    /// The hash code of the key column value for the current row.
    /// </param>
    /// <returns>The unique row identifier or zero upon failure.</returns>
    protected virtual long MakeRowId(int rowIndex, int hashCode) => (long) rowIndex << 32 | (long) (uint) hashCode;

    /// <summary>
    /// Determines the unique row identifier for the current row.
    /// </summary>
    /// <param name="cursor">
    /// The <see cref="T:System.Data.SQLite.SQLiteVirtualTableCursor" /> object instance
    /// associated with the previously opened virtual table cursor to be
    /// used.
    /// </param>
    /// <param name="value">
    /// The object instance to return a unique row identifier for.
    /// </param>
    /// <returns>The unique row identifier or zero upon failure.</returns>
    protected virtual long GetRowIdFromObject(SQLiteVirtualTableCursor cursor, object value) => this.MakeRowId(cursor != null ? cursor.GetRowIndex() : 0, SQLiteMarshal.GetHashCode(value, this.objectIdentity));

    /// <summary>
    /// Throws an <see cref="T:System.ObjectDisposedException" /> if this object
    /// instance has been disposed.
    /// </summary>
    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteModuleCommon).Name);
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
