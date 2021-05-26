// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteException
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Data.SQLite
{
  /// <summary>SQLite exception class.</summary>
  [Serializable]
  public sealed class SQLiteException : DbException, ISerializable
  {
    /// <summary>
    /// This value was copied from the "WinError.h" file included with the
    /// Platform SDK for Windows 10.
    /// </summary>
    private const int FACILITY_SQLITE = 1967;
    private SQLiteErrorCode _errorCode;

    /// <summary>Private constructor for use with serialization.</summary>
    /// <param name="info">
    /// Holds the serialized object data about the exception being thrown.
    /// </param>
    /// <param name="context">
    /// Contains contextual information about the source or destination.
    /// </param>
    private SQLiteException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._errorCode = (SQLiteErrorCode) info.GetInt32("errorCode");
      this.Initialize();
    }

    /// <summary>
    /// Public constructor for generating a SQLite exception given the error
    /// code and message.
    /// </summary>
    /// <param name="errorCode">The SQLite return code to report.</param>
    /// <param name="message">
    /// Message text to go along with the return code message text.
    /// </param>
    public SQLiteException(SQLiteErrorCode errorCode, string message)
      : base(SQLiteException.GetStockErrorMessage(errorCode, message))
    {
      this._errorCode = errorCode;
      this.Initialize();
    }

    /// <summary>
    /// Public constructor that uses the base class constructor for the error
    /// message.
    /// </summary>
    /// <param name="message">Error message text.</param>
    public SQLiteException(string message)
      : this(SQLiteErrorCode.Unknown, message)
    {
    }

    /// <summary>
    /// Public constructor that uses the default base class constructor.
    /// </summary>
    public SQLiteException() => this.Initialize();

    /// <summary>
    /// Public constructor that uses the base class constructor for the error
    /// message and inner exception.
    /// </summary>
    /// <param name="message">Error message text.</param>
    /// <param name="innerException">The original (inner) exception.</param>
    public SQLiteException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.Initialize();
    }

    /// <summary>
    /// Adds extra information to the serialized object data specific to this
    /// class type.  This is only used for serialization.
    /// </summary>
    /// <param name="info">
    /// Holds the serialized object data about the exception being thrown.
    /// </param>
    /// <param name="context">
    /// Contains contextual information about the source or destination.
    /// </param>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info?.AddValue("errorCode", (object) this._errorCode);
      base.GetObjectData(info, context);
    }

    /// <summary>
    /// Gets the associated SQLite result code for this exception as a
    /// <see cref="T:System.Data.SQLite.SQLiteErrorCode" />.  This property returns the same
    /// underlying value as the <see cref="P:System.Data.SQLite.SQLiteException.ErrorCode" /> property.
    /// </summary>
    public SQLiteErrorCode ResultCode => this._errorCode;

    /// <summary>
    /// Gets the associated SQLite return code for this exception as an
    /// <see cref="T:System.Int32" />.  For desktop versions of the .NET Framework,
    /// this property overrides the property of the same name within the
    /// <see cref="T:System.Runtime.InteropServices.ExternalException" />
    /// class.  This property returns the same underlying value as the
    /// <see cref="P:System.Data.SQLite.SQLiteException.ResultCode" /> property.
    /// </summary>
    public override int ErrorCode => (int) this._errorCode;

    /// <summary>
    /// This method performs extra initialization tasks.  It may be called by
    /// any of the constructors of this class.  It must not throw exceptions.
    /// </summary>
    private void Initialize()
    {
      if (this.HResult != -2147467259)
        return;
      int? hresultForErrorCode = SQLiteException.GetHResultForErrorCode(this.ResultCode);
      if (!hresultForErrorCode.HasValue)
        return;
      this.HResult = hresultForErrorCode.Value;
    }

    /// <summary>Maps a Win32 error code to an HRESULT.</summary>
    /// <param name="errorCode">
    /// The specified Win32 error code.  It must be within the range of zero
    /// (0) to 0xFFFF (65535).
    /// </param>
    /// <param name="success">
    /// Non-zero if the HRESULT should indicate success; otherwise, zero.
    /// </param>
    /// <returns>The integer value of the HRESULT.</returns>
    private static int MakeHResult(int errorCode, bool success) => errorCode & (int) ushort.MaxValue | 1967 | (success ? 0 : int.MinValue);

    /// <summary>
    /// Attempts to map the specified <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> onto an
    /// existing HRESULT -OR- a Win32 error code wrapped in an HRESULT.  The
    /// mappings may not have perfectly matching semantics; however, they do
    /// have the benefit of being unique within the context of this exception
    /// type.
    /// </summary>
    /// <param name="errorCode">
    /// The <see cref="T:System.Data.SQLite.SQLiteErrorCode" /> to map.
    /// </param>
    /// <returns>
    /// The integer HRESULT value -OR- null if there is no known mapping.
    /// </returns>
    private static int? GetHResultForErrorCode(SQLiteErrorCode errorCode)
    {
      switch (errorCode & SQLiteErrorCode.NonExtendedMask)
      {
        case SQLiteErrorCode.Ok:
          return new int?(0);
        case SQLiteErrorCode.Error:
          return new int?(SQLiteException.MakeHResult(31, false));
        case SQLiteErrorCode.Internal:
          return new int?(-2147418113);
        case SQLiteErrorCode.Perm:
          return new int?(SQLiteException.MakeHResult(5, false));
        case SQLiteErrorCode.Abort:
          return new int?(-2147467260);
        case SQLiteErrorCode.Busy:
          return new int?(SQLiteException.MakeHResult(170, false));
        case SQLiteErrorCode.Locked:
          return new int?(SQLiteException.MakeHResult(212, false));
        case SQLiteErrorCode.NoMem:
          return new int?(SQLiteException.MakeHResult(14, false));
        case SQLiteErrorCode.ReadOnly:
          return new int?(SQLiteException.MakeHResult(6009, false));
        case SQLiteErrorCode.Interrupt:
          return new int?(SQLiteException.MakeHResult(1223, false));
        case SQLiteErrorCode.IoErr:
          return new int?(SQLiteException.MakeHResult(1117, false));
        case SQLiteErrorCode.Corrupt:
          return new int?(SQLiteException.MakeHResult(1358, false));
        case SQLiteErrorCode.NotFound:
          return new int?(SQLiteException.MakeHResult(50, false));
        case SQLiteErrorCode.Full:
          return new int?(SQLiteException.MakeHResult(112, false));
        case SQLiteErrorCode.CantOpen:
          return new int?(SQLiteException.MakeHResult(1011, false));
        case SQLiteErrorCode.Protocol:
          return new int?(SQLiteException.MakeHResult(1460, false));
        case SQLiteErrorCode.Empty:
          return new int?(SQLiteException.MakeHResult(4306, false));
        case SQLiteErrorCode.Schema:
          return new int?(SQLiteException.MakeHResult(1931, false));
        case SQLiteErrorCode.TooBig:
          return new int?(-2147317563);
        case SQLiteErrorCode.Constraint:
          return new int?(SQLiteException.MakeHResult(8239, false));
        case SQLiteErrorCode.Mismatch:
          return new int?(SQLiteException.MakeHResult(1629, false));
        case SQLiteErrorCode.Misuse:
          return new int?(SQLiteException.MakeHResult(1609, false));
        case SQLiteErrorCode.NoLfs:
          return new int?(SQLiteException.MakeHResult(1606, false));
        case SQLiteErrorCode.Auth:
          return new int?(SQLiteException.MakeHResult(1935, false));
        case SQLiteErrorCode.Format:
          return new int?(SQLiteException.MakeHResult(11, false));
        case SQLiteErrorCode.Range:
          return new int?(-2147316575);
        case SQLiteErrorCode.NotADb:
          return new int?(SQLiteException.MakeHResult(1392, false));
        case SQLiteErrorCode.Notice:
        case SQLiteErrorCode.Warning:
        case SQLiteErrorCode.Row:
        case SQLiteErrorCode.Done:
          return new int?(SQLiteException.MakeHResult((int) errorCode, true));
        default:
          return new int?();
      }
    }

    /// <summary>
    /// Returns the error message for the specified SQLite return code.
    /// </summary>
    /// <param name="errorCode">The SQLite return code.</param>
    /// <returns>The error message or null if it cannot be found.</returns>
    private static string GetErrorString(SQLiteErrorCode errorCode) => typeof (SQLite3).InvokeMember(nameof (GetErrorString), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, (object) null, new object[1]
    {
      (object) errorCode
    }) as string;

    /// <summary>
    /// Returns the composite error message based on the SQLite return code
    /// and the optional detailed error message.
    /// </summary>
    /// <param name="errorCode">The SQLite return code.</param>
    /// <param name="message">Optional detailed error message.</param>
    /// <returns>Error message text for the return code.</returns>
    private static string GetStockErrorMessage(SQLiteErrorCode errorCode, string message) => HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "{0}{1}{2}", (object) SQLiteException.GetErrorString(errorCode), (object) Environment.NewLine, (object) message).Trim();

    public override string ToString() => HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "code = {0} ({1}), message = {2}", (object) this._errorCode, (object) (int) this._errorCode, (object) base.ToString());
  }
}
