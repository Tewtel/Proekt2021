// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteConvert
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.SQLite
{
  /// <summary>
  /// This base class provides datatype conversion services for the SQLite provider.
  /// </summary>
  public abstract class SQLiteConvert
  {
    /// <summary>
    /// This character is used to escape other characters, including itself, in
    /// connection string property names and values.
    /// </summary>
    internal const char EscapeChar = '\\';
    /// <summary>
    /// This character can be used to wrap connection string property names and
    /// values.  Normally, it is optional; however, when used, it must be the
    /// first -AND- last character of that connection string property name -OR-
    /// value.
    /// </summary>
    internal const char QuoteChar = '"';
    /// <summary>
    /// This character can be used to wrap connection string property names and
    /// values.  Normally, it is optional; however, when used, it must be the
    /// first -AND- last character of that connection string property name -OR-
    /// value.
    /// </summary>
    internal const char AltQuoteChar = '\'';
    /// <summary>
    /// The character is used to separate the name and value for a connection
    /// string property.  This character cannot be present in any connection
    /// string property name.  This character can be present in a connection
    /// string property value; however, this should be avoided unless deemed
    /// absolutely necessary.
    /// </summary>
    internal const char ValueChar = '=';
    /// <summary>
    /// This character is used to separate connection string properties.  When
    /// the "No_SQLiteConnectionNewParser" setting is enabled, this character
    /// may not appear in connection string property names -OR- values.
    /// </summary>
    internal const char PairChar = ';';
    /// <summary>
    /// The fallback default database type when one cannot be obtained from an
    /// existing connection instance.
    /// </summary>
    private const DbType FallbackDefaultDbType = DbType.Object;
    /// <summary>
    /// The format string for DateTime values when using the InvariantCulture or CurrentCulture formats.
    /// </summary>
    private const string FullFormat = "yyyy-MM-ddTHH:mm:ss.fffffffK";
    /// <summary>
    /// These are the characters that are special to the connection string
    /// parser.
    /// </summary>
    internal static readonly char[] SpecialChars = new char[5]
    {
      '"',
      '\'',
      ';',
      '=',
      '\\'
    };
    /// <summary>
    /// The fallback default database type name when one cannot be obtained from
    /// an existing connection instance.
    /// </summary>
    private static readonly string FallbackDefaultTypeName = string.Empty;
    /// <summary>
    /// The value for the Unix epoch (e.g. January 1, 1970 at midnight, in UTC).
    /// </summary>
    protected static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    /// <summary>
    /// The value of the OLE Automation epoch represented as a Julian day.  This
    /// field cannot be removed as the test suite relies upon it.
    /// </summary>
    private static readonly double OleAutomationEpochAsJulianDay = 2415018.5;
    /// <summary>
    /// This is the minimum Julian Day value supported by this library
    /// (148731163200000).
    /// </summary>
    private static readonly long MinimumJd = SQLiteConvert.computeJD(DateTime.MinValue);
    /// <summary>
    /// This is the maximum Julian Day value supported by this library
    /// (464269060799000).
    /// </summary>
    private static readonly long MaximumJd = SQLiteConvert.computeJD(DateTime.MaxValue);
    /// <summary>
    /// An array of ISO-8601 DateTime formats that we support parsing.
    /// </summary>
    private static string[] _datetimeFormats = new string[31]
    {
      "THHmmssK",
      "THHmmK",
      "HH:mm:ss.FFFFFFFK",
      "HH:mm:ssK",
      "HH:mmK",
      "yyyy-MM-dd HH:mm:ss.FFFFFFFK",
      "yyyy-MM-dd HH:mm:ssK",
      "yyyy-MM-dd HH:mmK",
      "yyyy-MM-ddTHH:mm:ss.FFFFFFFK",
      "yyyy-MM-ddTHH:mmK",
      "yyyy-MM-ddTHH:mm:ssK",
      "yyyyMMddHHmmssK",
      "yyyyMMddHHmmK",
      "yyyyMMddTHHmmssFFFFFFFK",
      "THHmmss",
      "THHmm",
      "HH:mm:ss.FFFFFFF",
      "HH:mm:ss",
      "HH:mm",
      "yyyy-MM-dd HH:mm:ss.FFFFFFF",
      "yyyy-MM-dd HH:mm:ss",
      "yyyy-MM-dd HH:mm",
      "yyyy-MM-ddTHH:mm:ss.FFFFFFF",
      "yyyy-MM-ddTHH:mm",
      "yyyy-MM-ddTHH:mm:ss",
      "yyyyMMddHHmmss",
      "yyyyMMddHHmm",
      "yyyyMMddTHHmmssFFFFFFF",
      "yyyy-MM-dd",
      "yyyyMMdd",
      "yy-MM-dd"
    };
    /// <summary>
    /// The internal default format for UTC DateTime values when converting
    /// to a string.
    /// </summary>
    private static readonly string _datetimeFormatUtc = SQLiteConvert._datetimeFormats[5];
    /// <summary>
    /// The internal default format for local DateTime values when converting
    /// to a string.
    /// </summary>
    private static readonly string _datetimeFormatLocal = SQLiteConvert._datetimeFormats[19];
    /// <summary>
    /// An UTF-8 Encoding instance, so we can convert strings to and from UTF-8
    /// </summary>
    private static Encoding _utf8 = (Encoding) new UTF8Encoding();
    /// <summary>The default DateTime format for this instance.</summary>
    internal SQLiteDateFormats _datetimeFormat;
    /// <summary>The default DateTimeKind for this instance.</summary>
    internal DateTimeKind _datetimeKind;
    /// <summary>The default DateTime format string for this instance.</summary>
    internal string _datetimeFormatString;
    private static Type[] _affinitytotype = new Type[12]
    {
      typeof (object),
      typeof (long),
      typeof (double),
      typeof (string),
      typeof (byte[]),
      typeof (object),
      null,
      null,
      null,
      null,
      typeof (DateTime),
      typeof (object)
    };
    private static DbType[] _typetodbtype = new DbType[19]
    {
      DbType.Object,
      DbType.Binary,
      DbType.Object,
      DbType.Boolean,
      DbType.SByte,
      DbType.SByte,
      DbType.Byte,
      DbType.Int16,
      DbType.UInt16,
      DbType.Int32,
      DbType.UInt32,
      DbType.Int64,
      DbType.UInt64,
      DbType.Single,
      DbType.Double,
      DbType.Decimal,
      DbType.DateTime,
      DbType.Object,
      DbType.String
    };
    private static int[] _dbtypetocolumnsize = new int[28]
    {
      int.MaxValue,
      int.MaxValue,
      1,
      1,
      8,
      8,
      8,
      8,
      8,
      16,
      2,
      4,
      8,
      int.MaxValue,
      1,
      4,
      int.MaxValue,
      8,
      2,
      4,
      8,
      8,
      int.MaxValue,
      int.MaxValue,
      int.MaxValue,
      int.MaxValue,
      8,
      10
    };
    private static object[] _dbtypetonumericprecision = new object[28]
    {
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 3,
      (object) DBNull.Value,
      (object) 19,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 53,
      (object) 53,
      (object) DBNull.Value,
      (object) 5,
      (object) 10,
      (object) 19,
      (object) DBNull.Value,
      (object) 3,
      (object) 24,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 5,
      (object) 10,
      (object) 19,
      (object) 53,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value
    };
    private static object[] _dbtypetonumericscale = new object[28]
    {
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 0,
      (object) DBNull.Value,
      (object) 4,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 0,
      (object) 0,
      (object) 0,
      (object) DBNull.Value,
      (object) 0,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) 0,
      (object) 0,
      (object) 0,
      (object) 0,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value,
      (object) DBNull.Value
    };
    private static Type[] _dbtypeToType = new Type[28]
    {
      typeof (string),
      typeof (byte[]),
      typeof (byte),
      typeof (bool),
      typeof (Decimal),
      typeof (DateTime),
      typeof (DateTime),
      typeof (Decimal),
      typeof (double),
      typeof (Guid),
      typeof (short),
      typeof (int),
      typeof (long),
      typeof (object),
      typeof (sbyte),
      typeof (float),
      typeof (string),
      typeof (DateTime),
      typeof (ushort),
      typeof (uint),
      typeof (ulong),
      typeof (double),
      typeof (string),
      typeof (string),
      typeof (string),
      typeof (string),
      typeof (DateTime),
      typeof (DateTimeOffset)
    };
    private static TypeAffinity[] _typecodeAffinities = new TypeAffinity[19]
    {
      TypeAffinity.Null,
      TypeAffinity.Blob,
      TypeAffinity.Null,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Int64,
      TypeAffinity.Double,
      TypeAffinity.Double,
      TypeAffinity.Double,
      TypeAffinity.DateTime,
      TypeAffinity.Null,
      TypeAffinity.Text
    };
    private static readonly SQLiteDbTypeMap _typeNames = SQLiteConvert.GetSQLiteDbTypeMap();

    /// <summary>Initializes the conversion class</summary>
    /// <param name="fmt">The default date/time format to use for this instance</param>
    /// <param name="kind">The DateTimeKind to use.</param>
    /// <param name="fmtString">The DateTime format string to use.</param>
    internal SQLiteConvert(SQLiteDateFormats fmt, DateTimeKind kind, string fmtString)
    {
      this._datetimeFormat = fmt;
      this._datetimeKind = kind;
      this._datetimeFormatString = fmtString;
    }

    /// <summary>
    /// Converts a string to a UTF-8 encoded byte array sized to include a null-terminating character.
    /// </summary>
    /// <param name="sourceText">The string to convert to UTF-8</param>
    /// <returns>A byte array containing the converted string plus an extra 0 terminating byte at the end of the array.</returns>
    public static byte[] ToUTF8(string sourceText)
    {
      if (sourceText == null)
        return (byte[]) null;
      byte[] bytes = new byte[SQLiteConvert._utf8.GetByteCount(sourceText) + 1];
      bytes[SQLiteConvert._utf8.GetBytes(sourceText, 0, sourceText.Length, bytes, 0)] = (byte) 0;
      return bytes;
    }

    /// <summary>
    /// Convert a DateTime to a UTF-8 encoded, zero-terminated byte array.
    /// </summary>
    /// <remarks>
    /// This function is a convenience function, which first calls ToString() on the DateTime, and then calls ToUTF8() with the
    /// string result.
    /// </remarks>
    /// <param name="dateTimeValue">The DateTime to convert.</param>
    /// <returns>The UTF-8 encoded string, including a 0 terminating byte at the end of the array.</returns>
    public byte[] ToUTF8(DateTime dateTimeValue) => SQLiteConvert.ToUTF8(this.ToString(dateTimeValue));

    /// <summary>
    /// Converts a UTF-8 encoded IntPtr of the specified length into a .NET string
    /// </summary>
    /// <param name="nativestring">The pointer to the memory where the UTF-8 string is encoded</param>
    /// <param name="nativestringlen">The number of bytes to decode</param>
    /// <returns>A string containing the translated character(s)</returns>
    public virtual string ToString(IntPtr nativestring, int nativestringlen) => SQLiteConvert.UTF8ToString(nativestring, nativestringlen);

    /// <summary>
    /// Converts a UTF-8 encoded IntPtr of the specified length into a .NET string
    /// </summary>
    /// <param name="nativestring">The pointer to the memory where the UTF-8 string is encoded</param>
    /// <param name="nativestringlen">The number of bytes to decode</param>
    /// <returns>A string containing the translated character(s)</returns>
    public static string UTF8ToString(IntPtr nativestring, int nativestringlen)
    {
      if (nativestring == IntPtr.Zero || nativestringlen == 0)
        return string.Empty;
      if (nativestringlen < 0)
      {
        nativestringlen = 0;
        while (Marshal.ReadByte(nativestring, nativestringlen) != (byte) 0)
          ++nativestringlen;
        if (nativestringlen == 0)
          return string.Empty;
      }
      byte[] numArray = new byte[nativestringlen];
      Marshal.Copy(nativestring, numArray, 0, nativestringlen);
      return SQLiteConvert._utf8.GetString(numArray, 0, nativestringlen);
    }

    /// <summary>
    /// Checks if the specified <see cref="T:System.Int64" /> is within the
    /// supported range for a Julian Day value.
    /// </summary>
    /// <param name="jd">The Julian Day value to check.</param>
    /// <returns>
    /// Non-zero if the specified Julian Day value is in the supported
    /// range; otherwise, zero.
    /// </returns>
    private static bool isValidJd(long jd) => jd >= SQLiteConvert.MinimumJd && jd <= SQLiteConvert.MaximumJd;

    /// <summary>
    /// Converts a Julian Day value from a <see cref="T:System.Double" /> to an
    /// <see cref="T:System.Int64" />.
    /// </summary>
    /// <param name="julianDay">
    /// The Julian Day <see cref="T:System.Double" /> value to convert.
    /// </param>
    /// <returns>
    /// The resulting Julian Day <see cref="T:System.Int64" /> value.
    /// </returns>
    private static long DoubleToJd(double julianDay) => (long) Math.Round(julianDay * 86400000.0);

    /// <summary>
    /// Converts a Julian Day value from an <see cref="T:System.Int64" /> to a
    /// <see cref="T:System.Double" />.
    /// </summary>
    /// <param name="jd">
    /// The Julian Day <see cref="T:System.Int64" /> value to convert.
    /// </param>
    /// <returns>
    /// The resulting Julian Day <see cref="T:System.Double" /> value.
    /// </returns>
    private static double JdToDouble(long jd) => (double) jd / 86400000.0;

    /// <summary>
    /// Converts a Julian Day value to a <see cref="T:System.DateTime" />.
    /// This method was translated from the "computeYMD" function in the
    /// "date.c" file belonging to the SQLite core library.
    /// </summary>
    /// <param name="jd">The Julian Day value to convert.</param>
    /// <param name="badValue">
    /// The <see cref="T:System.DateTime" /> value to return in the event that the
    /// Julian Day is out of the supported range.  If this value is null,
    /// an exception will be thrown instead.
    /// </param>
    /// <returns>
    /// A <see cref="T:System.DateTime" /> value that contains the year, month, and
    /// day values that are closest to the specified Julian Day value.
    /// </returns>
    private static DateTime computeYMD(long jd, DateTime? badValue)
    {
      if (!SQLiteConvert.isValidJd(jd))
        return badValue.HasValue ? badValue.Value : throw new ArgumentException("Not a supported Julian Day value.");
      int num1 = (int) ((jd + 43200000L) / 86400000L);
      int num2 = (int) (((double) num1 - 1867216.25) / 36524.25);
      int num3 = num1 + 1 + num2 - num2 / 4 + 1524;
      int num4 = (int) (((double) num3 - 122.1) / 365.25);
      int num5 = 36525 * num4 / 100;
      int num6 = (int) ((double) (num3 - num5) / 30.6001);
      int num7 = (int) (30.6001 * (double) num6);
      int day = num3 - num5 - num7;
      int month = num6 < 14 ? num6 - 1 : num6 - 13;
      int year = month > 2 ? num4 - 4716 : num4 - 4715;
      try
      {
        return new DateTime(year, month, day);
      }
      catch
      {
        if (badValue.HasValue)
          return badValue.Value;
        throw;
      }
    }

    /// <summary>
    /// Converts a Julian Day value to a <see cref="T:System.DateTime" />.
    /// This method was translated from the "computeHMS" function in the
    /// "date.c" file belonging to the SQLite core library.
    /// </summary>
    /// <param name="jd">The Julian Day value to convert.</param>
    /// <param name="badValue">
    /// The <see cref="T:System.DateTime" /> value to return in the event that the
    /// Julian Day value is out of the supported range.  If this value is
    /// null, an exception will be thrown instead.
    /// </param>
    /// <returns>
    /// A <see cref="T:System.DateTime" /> value that contains the hour, minute, and
    /// second, and millisecond values that are closest to the specified
    /// Julian Day value.
    /// </returns>
    private static DateTime computeHMS(long jd, DateTime? badValue)
    {
      if (!SQLiteConvert.isValidJd(jd))
        return badValue.HasValue ? badValue.Value : throw new ArgumentException("Not a supported Julian Day value.");
      Decimal num1 = (Decimal) (int) ((jd + 43200000L) % 86400000L) / 1000.0M;
      int num2 = (int) num1;
      int millisecond = (int) ((num1 - (Decimal) num2) * 1000.0M);
      Decimal num3 = num1 - (Decimal) num2;
      int hour = num2 / 3600;
      int num4 = num2 - hour * 3600;
      int minute = num4 / 60;
      int second = (int) (num3 + (Decimal) (num4 - minute * 60));
      try
      {
        DateTime minValue = DateTime.MinValue;
        return new DateTime(minValue.Year, minValue.Month, minValue.Day, hour, minute, second, millisecond);
      }
      catch
      {
        if (badValue.HasValue)
          return badValue.Value;
        throw;
      }
    }

    /// <summary>
    /// Converts a <see cref="T:System.DateTime" /> to a Julian Day value.
    /// This method was translated from the "computeJD" function in
    /// the "date.c" file belonging to the SQLite core library.
    /// Since the range of Julian Day values supported by this method
    /// includes all possible (valid) values of a <see cref="T:System.DateTime" />
    /// value, it should be extremely difficult for this method to
    /// raise an exception or return an undefined result.
    /// </summary>
    /// <param name="dateTime">
    /// The <see cref="T:System.DateTime" /> value to convert.  This value
    /// will be within the range of <see cref="F:System.DateTime.MinValue" />
    /// (00:00:00.0000000, January 1, 0001) to
    /// <see cref="F:System.DateTime.MaxValue" /> (23:59:59.9999999, December
    /// 31, 9999).
    /// </param>
    /// <returns>
    /// The nearest Julian Day value corresponding to the specified
    /// <see cref="T:System.DateTime" /> value.
    /// </returns>
    private static long computeJD(DateTime dateTime)
    {
      int year = dateTime.Year;
      int month = dateTime.Month;
      int day = dateTime.Day;
      if (month <= 2)
      {
        --year;
        month += 12;
      }
      int num1 = year / 100;
      int num2 = 2 - num1 + num1 / 4;
      return (long) (((double) (36525 * (year + 4716) / 100 + 306001 * (month + 1) / 10000 + day + num2) - 1524.5) * 86400000.0) + (long) (dateTime.Hour * 3600000 + dateTime.Minute * 60000 + dateTime.Second * 1000 + dateTime.Millisecond);
    }

    /// <summary>
    /// Converts a string into a DateTime, using the DateTimeFormat, DateTimeKind,
    /// and DateTimeFormatString specified for the connection when it was opened.
    /// </summary>
    /// <remarks>
    /// Acceptable ISO8601 DateTime formats are:
    /// <list type="bullet">
    /// <item><description>THHmmssK</description></item>
    /// <item><description>THHmmK</description></item>
    /// <item><description>HH:mm:ss.FFFFFFFK</description></item>
    /// <item><description>HH:mm:ssK</description></item>
    /// <item><description>HH:mmK</description></item>
    /// <item><description>yyyy-MM-dd HH:mm:ss.FFFFFFFK</description></item>
    /// <item><description>yyyy-MM-dd HH:mm:ssK</description></item>
    /// <item><description>yyyy-MM-dd HH:mmK</description></item>
    /// <item><description>yyyy-MM-ddTHH:mm:ss.FFFFFFFK</description></item>
    /// <item><description>yyyy-MM-ddTHH:mmK</description></item>
    /// <item><description>yyyy-MM-ddTHH:mm:ssK</description></item>
    /// <item><description>yyyyMMddHHmmssK</description></item>
    /// <item><description>yyyyMMddHHmmK</description></item>
    /// <item><description>yyyyMMddTHHmmssFFFFFFFK</description></item>
    /// <item><description>THHmmss</description></item>
    /// <item><description>THHmm</description></item>
    /// <item><description>HH:mm:ss.FFFFFFF</description></item>
    /// <item><description>HH:mm:ss</description></item>
    /// <item><description>HH:mm</description></item>
    /// <item><description>yyyy-MM-dd HH:mm:ss.FFFFFFF</description></item>
    /// <item><description>yyyy-MM-dd HH:mm:ss</description></item>
    /// <item><description>yyyy-MM-dd HH:mm</description></item>
    /// <item><description>yyyy-MM-ddTHH:mm:ss.FFFFFFF</description></item>
    /// <item><description>yyyy-MM-ddTHH:mm</description></item>
    /// <item><description>yyyy-MM-ddTHH:mm:ss</description></item>
    /// <item><description>yyyyMMddHHmmss</description></item>
    /// <item><description>yyyyMMddHHmm</description></item>
    /// <item><description>yyyyMMddTHHmmssFFFFFFF</description></item>
    /// <item><description>yyyy-MM-dd</description></item>
    /// <item><description>yyyyMMdd</description></item>
    /// <item><description>yy-MM-dd</description></item>
    /// </list>
    /// If the string cannot be matched to one of the above formats -OR-
    /// the DateTimeFormatString if one was provided, an exception will
    /// be thrown.
    /// </remarks>
    /// <param name="dateText">The string containing either a long integer number of 100-nanosecond units since
    /// System.DateTime.MinValue, a Julian day double, an integer number of seconds since the Unix epoch, a
    /// culture-independent formatted date and time string, a formatted date and time string in the current
    /// culture, or an ISO8601-format string.</param>
    /// <returns>A DateTime value</returns>
    public DateTime ToDateTime(string dateText) => SQLiteConvert.ToDateTime(dateText, this._datetimeFormat, this._datetimeKind, this._datetimeFormatString);

    /// <summary>
    /// Converts a string into a DateTime, using the specified DateTimeFormat,
    /// DateTimeKind and DateTimeFormatString.
    /// </summary>
    /// <remarks>
    /// Acceptable ISO8601 DateTime formats are:
    /// <list type="bullet">
    /// <item><description>THHmmssK</description></item>
    /// <item><description>THHmmK</description></item>
    /// <item><description>HH:mm:ss.FFFFFFFK</description></item>
    /// <item><description>HH:mm:ssK</description></item>
    /// <item><description>HH:mmK</description></item>
    /// <item><description>yyyy-MM-dd HH:mm:ss.FFFFFFFK</description></item>
    /// <item><description>yyyy-MM-dd HH:mm:ssK</description></item>
    /// <item><description>yyyy-MM-dd HH:mmK</description></item>
    /// <item><description>yyyy-MM-ddTHH:mm:ss.FFFFFFFK</description></item>
    /// <item><description>yyyy-MM-ddTHH:mmK</description></item>
    /// <item><description>yyyy-MM-ddTHH:mm:ssK</description></item>
    /// <item><description>yyyyMMddHHmmssK</description></item>
    /// <item><description>yyyyMMddHHmmK</description></item>
    /// <item><description>yyyyMMddTHHmmssFFFFFFFK</description></item>
    /// <item><description>THHmmss</description></item>
    /// <item><description>THHmm</description></item>
    /// <item><description>HH:mm:ss.FFFFFFF</description></item>
    /// <item><description>HH:mm:ss</description></item>
    /// <item><description>HH:mm</description></item>
    /// <item><description>yyyy-MM-dd HH:mm:ss.FFFFFFF</description></item>
    /// <item><description>yyyy-MM-dd HH:mm:ss</description></item>
    /// <item><description>yyyy-MM-dd HH:mm</description></item>
    /// <item><description>yyyy-MM-ddTHH:mm:ss.FFFFFFF</description></item>
    /// <item><description>yyyy-MM-ddTHH:mm</description></item>
    /// <item><description>yyyy-MM-ddTHH:mm:ss</description></item>
    /// <item><description>yyyyMMddHHmmss</description></item>
    /// <item><description>yyyyMMddHHmm</description></item>
    /// <item><description>yyyyMMddTHHmmssFFFFFFF</description></item>
    /// <item><description>yyyy-MM-dd</description></item>
    /// <item><description>yyyyMMdd</description></item>
    /// <item><description>yy-MM-dd</description></item>
    /// </list>
    /// If the string cannot be matched to one of the above formats -OR-
    /// the DateTimeFormatString if one was provided, an exception will
    /// be thrown.
    /// </remarks>
    /// <param name="dateText">The string containing either a long integer number of 100-nanosecond units since
    /// System.DateTime.MinValue, a Julian day double, an integer number of seconds since the Unix epoch, a
    /// culture-independent formatted date and time string, a formatted date and time string in the current
    /// culture, or an ISO8601-format string.</param>
    /// <param name="format">The SQLiteDateFormats to use.</param>
    /// <param name="kind">The DateTimeKind to use.</param>
    /// <param name="formatString">The DateTime format string to use.</param>
    /// <returns>A DateTime value</returns>
    public static DateTime ToDateTime(
      string dateText,
      SQLiteDateFormats format,
      DateTimeKind kind,
      string formatString)
    {
      switch (format)
      {
        case SQLiteDateFormats.Ticks:
          return SQLiteConvert.TicksToDateTime(Convert.ToInt64(dateText, (IFormatProvider) CultureInfo.InvariantCulture), kind);
        case SQLiteDateFormats.JulianDay:
          return SQLiteConvert.ToDateTime(Convert.ToDouble(dateText, (IFormatProvider) CultureInfo.InvariantCulture), kind);
        case SQLiteDateFormats.UnixEpoch:
          return SQLiteConvert.UnixEpochToDateTime(Convert.ToInt64(dateText, (IFormatProvider) CultureInfo.InvariantCulture), kind);
        case SQLiteDateFormats.InvariantCulture:
          return formatString != null ? DateTime.SpecifyKind(DateTime.ParseExact(dateText, formatString, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind) : DateTime.SpecifyKind(DateTime.Parse(dateText, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
        case SQLiteDateFormats.CurrentCulture:
          return formatString != null ? DateTime.SpecifyKind(DateTime.ParseExact(dateText, formatString, (IFormatProvider) DateTimeFormatInfo.CurrentInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind) : DateTime.SpecifyKind(DateTime.Parse(dateText, (IFormatProvider) DateTimeFormatInfo.CurrentInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
        default:
          return formatString != null ? DateTime.SpecifyKind(DateTime.ParseExact(dateText, formatString, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind) : DateTime.SpecifyKind(DateTime.ParseExact(dateText, SQLiteConvert._datetimeFormats, (IFormatProvider) DateTimeFormatInfo.InvariantInfo, kind == DateTimeKind.Utc ? DateTimeStyles.AdjustToUniversal : DateTimeStyles.None), kind);
      }
    }

    /// <summary>Converts a julianday value into a DateTime</summary>
    /// <param name="julianDay">The value to convert</param>
    /// <returns>A .NET DateTime</returns>
    public DateTime ToDateTime(double julianDay) => SQLiteConvert.ToDateTime(julianDay, this._datetimeKind);

    /// <summary>Converts a julianday value into a DateTime</summary>
    /// <param name="julianDay">The value to convert</param>
    /// <param name="kind">The DateTimeKind to use.</param>
    /// <returns>A .NET DateTime</returns>
    public static DateTime ToDateTime(double julianDay, DateTimeKind kind)
    {
      long jd = SQLiteConvert.DoubleToJd(julianDay);
      DateTime ymd = SQLiteConvert.computeYMD(jd, new DateTime?());
      DateTime hms = SQLiteConvert.computeHMS(jd, new DateTime?());
      return new DateTime(ymd.Year, ymd.Month, ymd.Day, hms.Hour, hms.Minute, hms.Second, hms.Millisecond, kind);
    }

    /// <summary>
    /// Converts the specified number of seconds from the Unix epoch into a
    /// <see cref="T:System.DateTime" /> value.
    /// </summary>
    /// <param name="seconds">
    /// The number of whole seconds since the Unix epoch.
    /// </param>
    /// <param name="kind">Either Utc or Local time.</param>
    /// <returns>
    /// The new <see cref="T:System.DateTime" /> value.
    /// </returns>
    internal static DateTime UnixEpochToDateTime(long seconds, DateTimeKind kind) => DateTime.SpecifyKind(SQLiteConvert.UnixEpoch.AddSeconds((double) seconds), kind);

    /// <summary>
    /// Converts the specified number of ticks since the epoch into a
    /// <see cref="T:System.DateTime" /> value.
    /// </summary>
    /// <param name="ticks">The number of whole ticks since the epoch.</param>
    /// <param name="kind">Either Utc or Local time.</param>
    /// <returns>
    /// The new <see cref="T:System.DateTime" /> value.
    /// </returns>
    internal static DateTime TicksToDateTime(long ticks, DateTimeKind kind) => new DateTime(ticks, kind);

    /// <summary>Converts a DateTime struct to a JulianDay double</summary>
    /// <param name="value">The DateTime to convert</param>
    /// <returns>The JulianDay value the Datetime represents</returns>
    public static double ToJulianDay(DateTime value) => SQLiteConvert.JdToDouble(SQLiteConvert.computeJD(value));

    /// <summary>
    /// Converts a DateTime struct to the whole number of seconds since the
    /// Unix epoch.
    /// </summary>
    /// <param name="value">The DateTime to convert</param>
    /// <returns>The whole number of seconds since the Unix epoch</returns>
    public static long ToUnixEpoch(DateTime value) => value.Subtract(SQLiteConvert.UnixEpoch).Ticks / 10000000L;

    /// <summary>
    /// Returns the DateTime format string to use for the specified DateTimeKind.
    /// If <paramref name="formatString" /> is not null, it will be returned verbatim.
    /// </summary>
    /// <param name="kind">The DateTimeKind to use.</param>
    /// <param name="formatString">The DateTime format string to use.</param>
    /// <returns>
    /// The DateTime format string to use for the specified DateTimeKind.
    /// </returns>
    private static string GetDateTimeKindFormat(DateTimeKind kind, string formatString)
    {
      if (formatString != null)
        return formatString;
      return kind != DateTimeKind.Utc ? SQLiteConvert._datetimeFormatLocal : SQLiteConvert._datetimeFormatUtc;
    }

    /// <summary>
    /// Converts a string into a DateTime, using the DateTimeFormat, DateTimeKind,
    /// and DateTimeFormatString specified for the connection when it was opened.
    /// </summary>
    /// <param name="dateValue">The DateTime value to convert</param>
    /// <returns>Either a string containing the long integer number of 100-nanosecond units since System.DateTime.MinValue, a
    /// Julian day double, an integer number of seconds since the Unix epoch, a culture-independent formatted date and time
    /// string, a formatted date and time string in the current culture, or an ISO8601-format date/time string.</returns>
    public string ToString(DateTime dateValue) => SQLiteConvert.ToString(dateValue, this._datetimeFormat, this._datetimeKind, this._datetimeFormatString);

    /// <summary>
    /// Converts a string into a DateTime, using the DateTimeFormat, DateTimeKind,
    /// and DateTimeFormatString specified for the connection when it was opened.
    /// </summary>
    /// <param name="dateValue">The DateTime value to convert</param>
    /// <param name="format">The SQLiteDateFormats to use.</param>
    /// <param name="kind">The DateTimeKind to use.</param>
    /// <param name="formatString">The DateTime format string to use.</param>
    /// <returns>Either a string containing the long integer number of 100-nanosecond units since System.DateTime.MinValue, a
    /// Julian day double, an integer number of seconds since the Unix epoch, a culture-independent formatted date and time
    /// string, a formatted date and time string in the current culture, or an ISO8601-format date/time string.</returns>
    public static string ToString(
      DateTime dateValue,
      SQLiteDateFormats format,
      DateTimeKind kind,
      string formatString)
    {
      switch (format)
      {
        case SQLiteDateFormats.Ticks:
          return dateValue.Ticks.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        case SQLiteDateFormats.JulianDay:
          return SQLiteConvert.ToJulianDay(dateValue).ToString((IFormatProvider) CultureInfo.InvariantCulture);
        case SQLiteDateFormats.UnixEpoch:
          return (dateValue.Subtract(SQLiteConvert.UnixEpoch).Ticks / 10000000L).ToString();
        case SQLiteDateFormats.InvariantCulture:
          return dateValue.ToString(formatString != null ? formatString : "yyyy-MM-ddTHH:mm:ss.fffffffK", (IFormatProvider) CultureInfo.InvariantCulture);
        case SQLiteDateFormats.CurrentCulture:
          return dateValue.ToString(formatString != null ? formatString : "yyyy-MM-ddTHH:mm:ss.fffffffK", (IFormatProvider) CultureInfo.CurrentCulture);
        default:
          return dateValue.Kind != DateTimeKind.Unspecified ? dateValue.ToString(SQLiteConvert.GetDateTimeKindFormat(dateValue.Kind, formatString), (IFormatProvider) CultureInfo.InvariantCulture) : DateTime.SpecifyKind(dateValue, kind).ToString(SQLiteConvert.GetDateTimeKindFormat(kind, formatString), (IFormatProvider) CultureInfo.InvariantCulture);
      }
    }

    /// <summary>
    /// Internal function to convert a UTF-8 encoded IntPtr of the specified length to a DateTime.
    /// </summary>
    /// <remarks>
    /// This is a convenience function, which first calls ToString() on the IntPtr to convert it to a string, then calls
    /// ToDateTime() on the string to return a DateTime.
    /// </remarks>
    /// <param name="ptr">A pointer to the UTF-8 encoded string</param>
    /// <param name="len">The length in bytes of the string</param>
    /// <returns>The parsed DateTime value</returns>
    internal DateTime ToDateTime(IntPtr ptr, int len) => this.ToDateTime(this.ToString(ptr, len));

    /// <summary>
    /// Smart method of splitting a string.  Skips quoted elements, removes the quotes.
    /// </summary>
    /// <remarks>
    /// This split function works somewhat like the String.Split() function in that it breaks apart a string into
    /// pieces and returns the pieces as an array.  The primary differences are:
    /// <list type="bullet">
    /// <item><description>Only one character can be provided as a separator character</description></item>
    /// <item><description>Quoted text inside the string is skipped over when searching for the separator, and the quotes are removed.</description></item>
    /// </list>
    /// Thus, if splitting the following string looking for a comma:<br />
    /// One,Two, "Three, Four", Five<br />
    /// <br />
    /// The resulting array would contain<br />
    /// [0] One<br />
    /// [1] Two<br />
    /// [2] Three, Four<br />
    /// [3] Five<br />
    /// <br />
    /// Note that the leading and trailing spaces were removed from each item during the split.
    /// </remarks>
    /// <param name="source">Source string to split apart</param>
    /// <param name="separator">Separator character</param>
    /// <returns>A string array of the split up elements</returns>
    public static string[] Split(string source, char separator)
    {
      char[] anyOf1 = new char[2]{ '"', separator };
      char[] anyOf2 = new char[1]{ '"' };
      int startIndex = 0;
      List<string> stringList = new List<string>();
      while (source.Length > 0)
      {
        int num1 = source.IndexOfAny(anyOf1, startIndex);
        if (num1 != -1)
        {
          if ((int) source[num1] == (int) anyOf1[0])
          {
            int num2 = source.IndexOfAny(anyOf2, num1 + 1);
            if (num2 != -1)
              startIndex = num2 + 1;
            else
              break;
          }
          else
          {
            string str = source.Substring(0, num1).Trim();
            if (str.Length > 1 && (int) str[0] == (int) anyOf2[0] && (int) str[str.Length - 1] == (int) str[0])
              str = str.Substring(1, str.Length - 2);
            source = source.Substring(num1 + 1).Trim();
            if (str.Length > 0)
              stringList.Add(str);
            startIndex = 0;
          }
        }
        else
          break;
      }
      if (source.Length > 0)
      {
        string str = source.Trim();
        if (str.Length > 1 && (int) str[0] == (int) anyOf2[0] && (int) str[str.Length - 1] == (int) str[0])
          str = str.Substring(1, str.Length - 2);
        stringList.Add(str);
      }
      string[] array = new string[stringList.Count];
      stringList.CopyTo(array, 0);
      return array;
    }

    /// <summary>
    /// Splits the specified string into multiple strings based on a separator
    /// and returns the result as an array of strings.
    /// </summary>
    /// <param name="value">
    /// The string to split into pieces based on the separator character.  If
    /// this string is null, null will always be returned.  If this string is
    /// empty, an array of zero strings will always be returned.
    /// </param>
    /// <param name="separator">
    /// The character used to divide the original string into sub-strings.
    /// This character cannot be a backslash or a double-quote; otherwise, no
    /// work will be performed and null will be returned.
    /// </param>
    /// <param name="keepQuote">
    /// If this parameter is non-zero, all double-quote characters will be
    /// retained in the returned list of strings; otherwise, they will be
    /// dropped.
    /// </param>
    /// <param name="error">
    /// Upon failure, this parameter will be modified to contain an appropriate
    /// error message.
    /// </param>
    /// <returns>
    /// The new array of strings or null if the input string is null -OR- the
    /// separator character is a backslash or a double-quote -OR- the string
    /// contains an unbalanced backslash or double-quote character.
    /// </returns>
    internal static string[] NewSplit(
      string value,
      char separator,
      bool keepQuote,
      ref string error)
    {
      if (separator == '\\' || separator == '"')
      {
        error = "separator character cannot be the escape or quote characters";
        return (string[]) null;
      }
      if (value == null)
      {
        error = "string value to split cannot be null";
        return (string[]) null;
      }
      int length = value.Length;
      if (length == 0)
        return new string[0];
      List<string> stringList = new List<string>();
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      bool flag1 = false;
      bool flag2 = false;
      while (num < length)
      {
        char ch = value[num++];
        if (flag1)
        {
          if (ch != '\\' && ch != '"' && (int) ch != (int) separator)
            stringBuilder.Append('\\');
          stringBuilder.Append(ch);
          flag1 = false;
        }
        else
        {
          switch (ch)
          {
            case '"':
              if (keepQuote)
                stringBuilder.Append(ch);
              flag2 = !flag2;
              continue;
            case '\\':
              flag1 = true;
              continue;
            default:
              if ((int) ch == (int) separator)
              {
                if (flag2)
                {
                  stringBuilder.Append(ch);
                  continue;
                }
                stringList.Add(stringBuilder.ToString());
                stringBuilder.Length = 0;
                continue;
              }
              stringBuilder.Append(ch);
              continue;
          }
        }
      }
      if (flag1 || flag2)
      {
        error = "unbalanced escape or quote character found";
        return (string[]) null;
      }
      if (stringBuilder.Length > 0)
        stringList.Add(stringBuilder.ToString());
      return stringList.ToArray();
    }

    /// <summary>
    /// Queries and returns the string representation for an object, using the
    /// specified (or current) format provider.
    /// </summary>
    /// <param name="obj">
    /// The object instance to return the string representation for.
    /// </param>
    /// <param name="provider">
    /// The format provider to use -OR- null if the current format provider for
    /// the thread should be used instead.
    /// </param>
    /// <returns>
    /// The string representation for the object instance -OR- null if the
    /// object instance is also null.
    /// </returns>
    public static string ToStringWithProvider(object obj, IFormatProvider provider)
    {
      switch (obj)
      {
        case null:
          return (string) null;
        case string _:
          return (string) obj;
        case IConvertible convertible:
          return convertible.ToString(provider);
        default:
          return obj.ToString();
      }
    }

    /// <summary>
    /// Attempts to convert an arbitrary object to the Boolean data type.
    /// Null object values are converted to false.  Throws an exception
    /// upon failure.
    /// </summary>
    /// <param name="obj">The object value to convert.</param>
    /// <param name="provider">The format provider to use.</param>
    /// <param name="viaFramework">
    /// If non-zero, a string value will be converted using the
    /// <see cref="M:System.Convert.ToBoolean(System.Object,System.IFormatProvider)" />
    /// method; otherwise, the <see cref="M:System.Data.SQLite.SQLiteConvert.ToBoolean(System.String)" />
    /// method will be used.
    /// </param>
    /// <returns>The converted boolean value.</returns>
    internal static bool ToBoolean(object obj, IFormatProvider provider, bool viaFramework)
    {
      if (obj == null)
        return false;
      TypeCode typeCode = Type.GetTypeCode(obj.GetType());
      switch (typeCode)
      {
        case TypeCode.Empty:
        case TypeCode.DBNull:
          return false;
        case TypeCode.Boolean:
          return (bool) obj;
        case TypeCode.Char:
          return (char) obj != char.MinValue;
        case TypeCode.SByte:
          return (sbyte) obj != (sbyte) 0;
        case TypeCode.Byte:
          return (byte) obj != (byte) 0;
        case TypeCode.Int16:
          return (short) obj != (short) 0;
        case TypeCode.UInt16:
          return (ushort) obj != (ushort) 0;
        case TypeCode.Int32:
          return (int) obj != 0;
        case TypeCode.UInt32:
          return (uint) obj != 0U;
        case TypeCode.Int64:
          return (long) obj != 0L;
        case TypeCode.UInt64:
          return (ulong) obj != 0UL;
        case TypeCode.Single:
          return (double) (float) obj != 0.0;
        case TypeCode.Double:
          return (double) obj != 0.0;
        case TypeCode.Decimal:
          return (Decimal) obj != 0M;
        case TypeCode.String:
          return !viaFramework ? SQLiteConvert.ToBoolean(SQLiteConvert.ToStringWithProvider(obj, provider)) : Convert.ToBoolean(obj, provider);
        default:
          throw new SQLiteException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Cannot convert type {0} to boolean", (object) typeCode));
      }
    }

    /// <summary>Convert a value to true or false.</summary>
    /// <param name="source">A string or number representing true or false</param>
    /// <returns></returns>
    public static bool ToBoolean(object source) => source is bool flag ? flag : SQLiteConvert.ToBoolean(SQLiteConvert.ToStringWithProvider(source, (IFormatProvider) CultureInfo.InvariantCulture));

    /// <summary>
    /// Converts an integer to a string that can be round-tripped using the
    /// invariant culture.
    /// </summary>
    /// <param name="value">
    /// The integer value to return the string representation for.
    /// </param>
    /// <returns>
    /// The string representation of the specified integer value, using the
    /// invariant culture.
    /// </returns>
    internal static string ToString(int value) => value.ToString((IFormatProvider) CultureInfo.InvariantCulture);

    /// <summary>
    /// Attempts to convert a <see cref="T:System.String" /> into a <see cref="T:System.Boolean" />.
    /// </summary>
    /// <param name="source">
    /// The <see cref="T:System.String" /> to convert, cannot be null.
    /// </param>
    /// <returns>
    /// The converted <see cref="T:System.Boolean" /> value.
    /// </returns>
    /// <remarks>
    /// The supported strings are "yes", "no", "y", "n", "on", "off", "0", "1",
    /// as well as any prefix of the strings <see cref="F:System.Boolean.FalseString" />
    /// and <see cref="F:System.Boolean.TrueString" />.  All strings are treated in a
    /// case-insensitive manner.
    /// </remarks>
    public static bool ToBoolean(string source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (string.Compare(source, 0, bool.TrueString, 0, source.Length, StringComparison.OrdinalIgnoreCase) == 0)
        return true;
      if (string.Compare(source, 0, bool.FalseString, 0, source.Length, StringComparison.OrdinalIgnoreCase) == 0)
        return false;
      switch (source.ToLower(CultureInfo.InvariantCulture))
      {
        case "y":
        case "yes":
        case "on":
        case "1":
          return true;
        case "n":
        case "no":
        case "off":
        case "0":
          return false;
        default:
          throw new ArgumentException(nameof (source));
      }
    }

    /// <summary>Converts a SQLiteType to a .NET Type object</summary>
    /// <param name="t">The SQLiteType to convert</param>
    /// <returns>Returns a .NET Type object</returns>
    internal static Type SQLiteTypeToType(SQLiteType t) => t.Type == DbType.Object ? SQLiteConvert._affinitytotype[(int) t.Affinity] : SQLiteConvert.DbTypeToType(t.Type);

    /// <summary>For a given intrinsic type, return a DbType</summary>
    /// <param name="typ">The native type to convert</param>
    /// <returns>The corresponding (closest match) DbType</returns>
    internal static DbType TypeToDbType(Type typ)
    {
      TypeCode typeCode = Type.GetTypeCode(typ);
      if (typeCode != TypeCode.Object)
        return SQLiteConvert._typetodbtype[(int) typeCode];
      if (typ == typeof (byte[]))
        return DbType.Binary;
      return typ == typeof (Guid) ? DbType.Guid : DbType.String;
    }

    /// <summary>Returns the ColumnSize for the given DbType</summary>
    /// <param name="typ">The DbType to get the size of</param>
    /// <returns></returns>
    internal static int DbTypeToColumnSize(DbType typ) => SQLiteConvert._dbtypetocolumnsize[(int) typ];

    internal static object DbTypeToNumericPrecision(DbType typ) => SQLiteConvert._dbtypetonumericprecision[(int) typ];

    internal static object DbTypeToNumericScale(DbType typ) => SQLiteConvert._dbtypetonumericscale[(int) typ];

    /// <summary>
    /// Determines the default database type name to be used when a
    /// per-connection value is not available.
    /// </summary>
    /// <param name="connection">
    /// The connection context for type mappings, if any.
    /// </param>
    /// <returns>The default database type name to use.</returns>
    private static string GetDefaultTypeName(SQLiteConnection connection)
    {
      if (HelperMethods.HasFlags(connection != null ? connection.Flags : SQLiteConnectionFlags.None, SQLiteConnectionFlags.NoConvertSettings))
        return SQLiteConvert.FallbackDefaultTypeName;
      string name = "Use_SQLiteConvert_DefaultTypeName";
      object obj = (object) null;
      string @default = (string) null;
      if (connection != null)
      {
        if (connection.TryGetCachedSetting(name, (object) @default, out obj))
          goto label_8;
      }
      try
      {
        obj = (object) UnsafeNativeMethods.GetSettingValue(name, @default) ?? (object) SQLiteConvert.FallbackDefaultTypeName;
      }
      finally
      {
        connection?.SetCachedSetting(name, obj);
      }
label_8:
      return SQLiteConvert.SettingValueToString(obj);
    }

    /// <summary>
    /// If applicable, issues a trace log message warning about falling back to
    /// the default database type name.
    /// </summary>
    /// <param name="dbType">The database value type.</param>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <param name="typeName">The textual name of the database type.</param>
    private static void DefaultTypeNameWarning(
      DbType dbType,
      SQLiteConnectionFlags flags,
      string typeName)
    {
      if (!HelperMethods.HasFlags(flags, SQLiteConnectionFlags.TraceWarning))
        return;
      Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "WARNING: Type mapping failed, returning default name \"{0}\" for type {1}.", (object) typeName, (object) dbType));
    }

    /// <summary>
    /// If applicable, issues a trace log message warning about falling back to
    /// the default database value type.
    /// </summary>
    /// <param name="typeName">The textual name of the database type.</param>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <param name="dbType">The database value type.</param>
    private static void DefaultDbTypeWarning(
      string typeName,
      SQLiteConnectionFlags flags,
      DbType? dbType)
    {
      if (string.IsNullOrEmpty(typeName) || !HelperMethods.HasFlags(flags, SQLiteConnectionFlags.TraceWarning))
        return;
      Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "WARNING: Type mapping failed, returning default type {0} for name \"{1}\".", (object) dbType, (object) typeName));
    }

    /// <summary>
    /// For a given database value type, return the "closest-match" textual database type name.
    /// </summary>
    /// <param name="connection">The connection context for custom type mappings, if any.</param>
    /// <param name="dbType">The database value type.</param>
    /// <param name="flags">The flags associated with the parent connection object.</param>
    /// <returns>The type name or an empty string if it cannot be determined.</returns>
    internal static string DbTypeToTypeName(
      SQLiteConnection connection,
      DbType dbType,
      SQLiteConnectionFlags flags)
    {
      string str = (string) null;
      if (connection != null)
      {
        flags |= connection.Flags;
        if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.UseConnectionTypes))
        {
          SQLiteDbTypeMap typeNames = connection._typeNames;
          SQLiteDbTypeMapping liteDbTypeMapping;
          if (typeNames != null && typeNames.TryGetValue(dbType, out liteDbTypeMapping))
            return liteDbTypeMapping.typeName;
        }
        str = connection.DefaultTypeName;
      }
      if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.NoGlobalTypes))
      {
        if (str != null)
          return str;
        string defaultTypeName = SQLiteConvert.GetDefaultTypeName(connection);
        SQLiteConvert.DefaultTypeNameWarning(dbType, flags, defaultTypeName);
        return defaultTypeName;
      }
      SQLiteDbTypeMapping liteDbTypeMapping1;
      if (SQLiteConvert._typeNames != null && SQLiteConvert._typeNames.TryGetValue(dbType, out liteDbTypeMapping1))
        return liteDbTypeMapping1.typeName;
      if (str != null)
        return str;
      string defaultTypeName1 = SQLiteConvert.GetDefaultTypeName(connection);
      SQLiteConvert.DefaultTypeNameWarning(dbType, flags, defaultTypeName1);
      return defaultTypeName1;
    }

    /// <summary>Convert a DbType to a Type</summary>
    /// <param name="typ">The DbType to convert from</param>
    /// <returns>The closest-match .NET type</returns>
    internal static Type DbTypeToType(DbType typ) => SQLiteConvert._dbtypeToType[(int) typ];

    /// <summary>
    /// For a given type, return the closest-match SQLite TypeAffinity, which only understands a very limited subset of types.
    /// </summary>
    /// <param name="typ">The type to evaluate</param>
    /// <param name="flags">The flags associated with the connection.</param>
    /// <returns>The SQLite type affinity for that type.</returns>
    internal static TypeAffinity TypeToAffinity(Type typ, SQLiteConnectionFlags flags)
    {
      TypeCode typeCode = Type.GetTypeCode(typ);
      switch (typeCode)
      {
        case TypeCode.Object:
          return typ == typeof (byte[]) || typ == typeof (Guid) ? TypeAffinity.Blob : TypeAffinity.Text;
        case TypeCode.Decimal:
          if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.GetDecimalAsText))
            return TypeAffinity.Text;
          break;
      }
      return SQLiteConvert._typecodeAffinities[(int) typeCode];
    }

    /// <summary>
    /// Builds and returns a map containing the database column types
    /// recognized by this provider.
    /// </summary>
    /// <returns>
    /// A map containing the database column types recognized by this
    /// provider.
    /// </returns>
    private static SQLiteDbTypeMap GetSQLiteDbTypeMap() => new SQLiteDbTypeMap((IEnumerable<SQLiteDbTypeMapping>) new SQLiteDbTypeMapping[74]
    {
      new SQLiteDbTypeMapping("BIGINT", DbType.Int64, false),
      new SQLiteDbTypeMapping("BIGUINT", DbType.UInt64, false),
      new SQLiteDbTypeMapping("BINARY", DbType.Binary, false),
      new SQLiteDbTypeMapping("BIT", DbType.Boolean, true),
      new SQLiteDbTypeMapping("BLOB", DbType.Binary, true),
      new SQLiteDbTypeMapping("BOOL", DbType.Boolean, false),
      new SQLiteDbTypeMapping("BOOLEAN", DbType.Boolean, false),
      new SQLiteDbTypeMapping("CHAR", DbType.AnsiStringFixedLength, true),
      new SQLiteDbTypeMapping("CLOB", DbType.String, false),
      new SQLiteDbTypeMapping("COUNTER", DbType.Int64, false),
      new SQLiteDbTypeMapping("CURRENCY", DbType.Decimal, false),
      new SQLiteDbTypeMapping("DATE", DbType.DateTime, false),
      new SQLiteDbTypeMapping("DATETIME", DbType.DateTime, true),
      new SQLiteDbTypeMapping("DECIMAL", DbType.Decimal, true),
      new SQLiteDbTypeMapping("DECIMALTEXT", DbType.Decimal, false),
      new SQLiteDbTypeMapping("DOUBLE", DbType.Double, false),
      new SQLiteDbTypeMapping("FLOAT", DbType.Double, false),
      new SQLiteDbTypeMapping("GENERAL", DbType.Binary, false),
      new SQLiteDbTypeMapping("GUID", DbType.Guid, false),
      new SQLiteDbTypeMapping("IDENTITY", DbType.Int64, false),
      new SQLiteDbTypeMapping("IMAGE", DbType.Binary, false),
      new SQLiteDbTypeMapping("INT", DbType.Int32, true),
      new SQLiteDbTypeMapping("INT8", DbType.SByte, false),
      new SQLiteDbTypeMapping("INT16", DbType.Int16, false),
      new SQLiteDbTypeMapping("INT32", DbType.Int32, false),
      new SQLiteDbTypeMapping("INT64", DbType.Int64, false),
      new SQLiteDbTypeMapping("INTEGER", DbType.Int64, true),
      new SQLiteDbTypeMapping("INTEGER8", DbType.SByte, false),
      new SQLiteDbTypeMapping("INTEGER16", DbType.Int16, false),
      new SQLiteDbTypeMapping("INTEGER32", DbType.Int32, false),
      new SQLiteDbTypeMapping("INTEGER64", DbType.Int64, false),
      new SQLiteDbTypeMapping("LOGICAL", DbType.Boolean, false),
      new SQLiteDbTypeMapping("LONG", DbType.Int64, false),
      new SQLiteDbTypeMapping("LONGCHAR", DbType.String, false),
      new SQLiteDbTypeMapping("LONGTEXT", DbType.String, false),
      new SQLiteDbTypeMapping("LONGVARCHAR", DbType.String, false),
      new SQLiteDbTypeMapping("MEMO", DbType.String, false),
      new SQLiteDbTypeMapping("MONEY", DbType.Decimal, false),
      new SQLiteDbTypeMapping("NCHAR", DbType.StringFixedLength, true),
      new SQLiteDbTypeMapping("NOTE", DbType.String, false),
      new SQLiteDbTypeMapping("NTEXT", DbType.String, false),
      new SQLiteDbTypeMapping("NUMBER", DbType.Decimal, false),
      new SQLiteDbTypeMapping("NUMERIC", DbType.Decimal, false),
      new SQLiteDbTypeMapping("NUMERICTEXT", DbType.Decimal, false),
      new SQLiteDbTypeMapping("NVARCHAR", DbType.String, true),
      new SQLiteDbTypeMapping("OLEOBJECT", DbType.Binary, false),
      new SQLiteDbTypeMapping("RAW", DbType.Binary, false),
      new SQLiteDbTypeMapping("REAL", DbType.Double, true),
      new SQLiteDbTypeMapping("SINGLE", DbType.Single, true),
      new SQLiteDbTypeMapping("SMALLDATE", DbType.DateTime, false),
      new SQLiteDbTypeMapping("SMALLINT", DbType.Int16, true),
      new SQLiteDbTypeMapping("SMALLUINT", DbType.UInt16, true),
      new SQLiteDbTypeMapping("STRING", DbType.String, false),
      new SQLiteDbTypeMapping("TEXT", DbType.String, false),
      new SQLiteDbTypeMapping("TIME", DbType.DateTime, false),
      new SQLiteDbTypeMapping("TIMESTAMP", DbType.DateTime, false),
      new SQLiteDbTypeMapping("TINYINT", DbType.Byte, true),
      new SQLiteDbTypeMapping("TINYSINT", DbType.SByte, true),
      new SQLiteDbTypeMapping("UINT", DbType.UInt32, true),
      new SQLiteDbTypeMapping("UINT8", DbType.Byte, false),
      new SQLiteDbTypeMapping("UINT16", DbType.UInt16, false),
      new SQLiteDbTypeMapping("UINT32", DbType.UInt32, false),
      new SQLiteDbTypeMapping("UINT64", DbType.UInt64, false),
      new SQLiteDbTypeMapping("ULONG", DbType.UInt64, false),
      new SQLiteDbTypeMapping("UNIQUEIDENTIFIER", DbType.Guid, true),
      new SQLiteDbTypeMapping("UNSIGNEDINTEGER", DbType.UInt64, true),
      new SQLiteDbTypeMapping("UNSIGNEDINTEGER8", DbType.Byte, false),
      new SQLiteDbTypeMapping("UNSIGNEDINTEGER16", DbType.UInt16, false),
      new SQLiteDbTypeMapping("UNSIGNEDINTEGER32", DbType.UInt32, false),
      new SQLiteDbTypeMapping("UNSIGNEDINTEGER64", DbType.UInt64, false),
      new SQLiteDbTypeMapping("VARBINARY", DbType.Binary, false),
      new SQLiteDbTypeMapping("VARCHAR", DbType.AnsiString, true),
      new SQLiteDbTypeMapping("VARCHAR2", DbType.AnsiString, false),
      new SQLiteDbTypeMapping("YESNO", DbType.Boolean, false)
    });

    /// <summary>
    /// Determines if a database type is considered to be a string.
    /// </summary>
    /// <param name="type">The database type to check.</param>
    /// <returns>
    /// Non-zero if the database type is considered to be a string, zero
    /// otherwise.
    /// </returns>
    internal static bool IsStringDbType(DbType type)
    {
      switch (type)
      {
        case DbType.AnsiString:
        case DbType.String:
        case DbType.AnsiStringFixedLength:
        case DbType.StringFixedLength:
          return true;
        default:
          return false;
      }
    }

    /// <summary>
    /// Determines and returns the runtime configuration setting string that
    /// should be used in place of the specified object value.
    /// </summary>
    /// <param name="value">The object value to convert to a string.</param>
    /// <returns>
    /// Either the string to use in place of the object value -OR- null if it
    /// cannot be determined.
    /// </returns>
    private static string SettingValueToString(object value)
    {
      if (value is string)
        return (string) value;
      return value?.ToString();
    }

    /// <summary>
    /// Determines the default <see cref="T:System.Data.DbType" /> value to be used when a
    /// per-connection value is not available.
    /// </summary>
    /// <param name="connection">
    /// The connection context for type mappings, if any.
    /// </param>
    /// <returns>
    /// The default <see cref="T:System.Data.DbType" /> value to use.
    /// </returns>
    private static DbType GetDefaultDbType(SQLiteConnection connection)
    {
      if (HelperMethods.HasFlags(connection != null ? connection.Flags : SQLiteConnectionFlags.None, SQLiteConnectionFlags.NoConvertSettings))
        return DbType.Object;
      bool flag = false;
      string name = "Use_SQLiteConvert_DefaultDbType";
      object obj = (object) null;
      string @default = (string) null;
      if (connection == null || !connection.TryGetCachedSetting(name, (object) @default, out obj))
        obj = (object) UnsafeNativeMethods.GetSettingValue(name, @default) ?? (object) DbType.Object;
      else
        flag = true;
      try
      {
        if (!(obj is DbType))
        {
          obj = SQLiteConnection.TryParseEnum(typeof (DbType), SQLiteConvert.SettingValueToString(obj), true);
          if (!(obj is DbType))
            obj = (object) DbType.Object;
        }
        return (DbType) obj;
      }
      finally
      {
        if (!flag && connection != null)
          connection.SetCachedSetting(name, obj);
      }
    }

    /// <summary>
    /// Converts the object value, which is assumed to have originated
    /// from a <see cref="T:System.Data.DataRow" />, to a string value.
    /// </summary>
    /// <param name="value">The value to be converted to a string.</param>
    /// <returns>
    /// A null value will be returned if the original value is null -OR-
    /// the original value is <see cref="F:System.DBNull.Value" />.  Otherwise,
    /// the original value will be converted to a string, using its
    /// (possibly overridden) <see cref="M:System.Object.ToString" /> method and
    /// then returned.
    /// </returns>
    public static string GetStringOrNull(object value)
    {
      if (value == null)
        return (string) null;
      if (value is string)
        return (string) value;
      return value == DBNull.Value ? (string) null : value.ToString();
    }

    /// <summary>
    /// Determines if the specified textual value appears to be a
    /// <see cref="T:System.DBNull" /> value.
    /// </summary>
    /// <param name="text">The textual value to inspect.</param>
    /// <returns>
    /// Non-zero if the text looks like a <see cref="T:System.DBNull" /> value,
    /// zero otherwise.
    /// </returns>
    internal static bool LooksLikeNull(string text) => text == null;

    /// <summary>
    /// Determines if the specified textual value appears to be an
    /// <see cref="T:System.Int64" /> value.
    /// </summary>
    /// <param name="text">The textual value to inspect.</param>
    /// <returns>
    /// Non-zero if the text looks like an <see cref="T:System.Int64" /> value,
    /// zero otherwise.
    /// </returns>
    internal static bool LooksLikeInt64(string text)
    {
      long result;
      return long.TryParse(text, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result) && string.Equals(result.ToString((IFormatProvider) CultureInfo.InvariantCulture), text, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines if the specified textual value appears to be a
    /// <see cref="T:System.Double" /> value.
    /// </summary>
    /// <param name="text">The textual value to inspect.</param>
    /// <returns>
    /// Non-zero if the text looks like a <see cref="T:System.Double" /> value,
    /// zero otherwise.
    /// </returns>
    internal static bool LooksLikeDouble(string text)
    {
      double result;
      return double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result) && string.Equals(result.ToString((IFormatProvider) CultureInfo.InvariantCulture), text, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines if the specified textual value appears to be a
    /// <see cref="T:System.DateTime" /> value.
    /// </summary>
    /// <param name="convert">
    /// The <see cref="T:System.Data.SQLite.SQLiteConvert" /> object instance configured with
    /// the chosen <see cref="T:System.DateTime" /> format.
    /// </param>
    /// <param name="text">The textual value to inspect.</param>
    /// <returns>
    /// Non-zero if the text looks like a <see cref="T:System.DateTime" /> in the
    /// configured format, zero otherwise.
    /// </returns>
    internal static bool LooksLikeDateTime(SQLiteConvert convert, string text)
    {
      if (convert == null)
        return false;
      try
      {
        DateTime dateTime = convert.ToDateTime(text);
        if (string.Equals(convert.ToString(dateTime), text, StringComparison.Ordinal))
          return true;
      }
      catch
      {
      }
      return false;
    }

    /// <summary>
    /// For a given textual database type name, return the "closest-match" database type.
    /// This method is called during query result processing; therefore, its performance
    /// is critical.
    /// </summary>
    /// <param name="connection">The connection context for custom type mappings, if any.</param>
    /// <param name="typeName">The textual name of the database type to match.</param>
    /// <param name="flags">The flags associated with the parent connection object.</param>
    /// <returns>The .NET DBType the text evaluates to.</returns>
    internal static DbType TypeNameToDbType(
      SQLiteConnection connection,
      string typeName,
      SQLiteConnectionFlags flags)
    {
      DbType? dbType = new DbType?();
      if (connection != null)
      {
        flags |= connection.Flags;
        if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.UseConnectionTypes))
        {
          SQLiteDbTypeMap typeNames = connection._typeNames;
          if (typeNames != null && typeName != null)
          {
            SQLiteDbTypeMapping liteDbTypeMapping;
            if (typeNames.TryGetValue(typeName, out liteDbTypeMapping))
              return liteDbTypeMapping.dataType;
            int length = typeName.IndexOf('(');
            if (length > 0 && typeNames.TryGetValue(typeName.Substring(0, length).TrimEnd(), out liteDbTypeMapping))
              return liteDbTypeMapping.dataType;
          }
        }
        dbType = connection.DefaultDbType;
      }
      if (HelperMethods.HasFlags(flags, SQLiteConnectionFlags.NoGlobalTypes))
      {
        if (dbType.HasValue)
          return dbType.Value;
        dbType = new DbType?(SQLiteConvert.GetDefaultDbType(connection));
        SQLiteConvert.DefaultDbTypeWarning(typeName, flags, dbType);
        return dbType.Value;
      }
      if (SQLiteConvert._typeNames != null && typeName != null)
      {
        SQLiteDbTypeMapping liteDbTypeMapping;
        if (SQLiteConvert._typeNames.TryGetValue(typeName, out liteDbTypeMapping))
          return liteDbTypeMapping.dataType;
        int length = typeName.IndexOf('(');
        if (length > 0 && SQLiteConvert._typeNames.TryGetValue(typeName.Substring(0, length).TrimEnd(), out liteDbTypeMapping))
          return liteDbTypeMapping.dataType;
      }
      if (dbType.HasValue)
        return dbType.Value;
      dbType = new DbType?(SQLiteConvert.GetDefaultDbType(connection));
      SQLiteConvert.DefaultDbTypeWarning(typeName, flags, dbType);
      return dbType.Value;
    }
  }
}
