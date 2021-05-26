// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteString
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;
using System.Text;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class contains static methods that are used to deal with native
  /// UTF-8 string pointers to be used with the SQLite core library.
  /// </summary>
  internal static class SQLiteString
  {
    /// <summary>
    /// This is the maximum possible length for the native UTF-8 encoded
    /// strings used with the SQLite core library.
    /// </summary>
    private static int ThirtyBits = 1073741823;
    /// <summary>
    /// This is the <see cref="T:System.Text.Encoding" /> object instance used to handle
    /// conversions from/to UTF-8.
    /// </summary>
    private static readonly Encoding Utf8Encoding = Encoding.UTF8;

    /// <summary>
    /// Converts the specified managed string into the UTF-8 encoding and
    /// returns the array of bytes containing its representation in that
    /// encoding.
    /// </summary>
    /// <param name="value">The managed string to convert.</param>
    /// <returns>
    /// The array of bytes containing the representation of the managed
    /// string in the UTF-8 encoding or null upon failure.
    /// </returns>
    public static byte[] GetUtf8BytesFromString(string value) => value == null ? (byte[]) null : SQLiteString.Utf8Encoding.GetBytes(value);

    /// <summary>
    /// Converts the specified array of bytes representing a string in the
    /// UTF-8 encoding and returns a managed string.
    /// </summary>
    /// <param name="bytes">The array of bytes to convert.</param>
    /// <returns>The managed string or null upon failure.</returns>
    public static string GetStringFromUtf8Bytes(byte[] bytes) => bytes == null ? (string) null : SQLiteString.Utf8Encoding.GetString(bytes);

    /// <summary>
    /// Probes a native pointer to a string in the UTF-8 encoding for its
    /// terminating NUL character, within the specified length limit.
    /// </summary>
    /// <param name="pValue">The native NUL-terminated string pointer.</param>
    /// <param name="limit">
    /// The maximum length of the native string, in bytes.
    /// </param>
    /// <returns>
    /// The length of the native string, in bytes -OR- zero if the length
    /// could not be determined.
    /// </returns>
    public static int ProbeForUtf8ByteLength(IntPtr pValue, int limit)
    {
      int ofs = 0;
      if (pValue != IntPtr.Zero && limit > 0)
      {
        while (Marshal.ReadByte(pValue, ofs) != (byte) 0 && ofs < limit)
          ++ofs;
      }
      return ofs;
    }

    /// <summary>
    /// Converts the specified native NUL-terminated UTF-8 string pointer
    /// into a managed string.
    /// </summary>
    /// <param name="pValue">
    /// The native NUL-terminated UTF-8 string pointer.
    /// </param>
    /// <returns>The managed string or null upon failure.</returns>
    public static string StringFromUtf8IntPtr(IntPtr pValue) => SQLiteString.StringFromUtf8IntPtr(pValue, SQLiteString.ProbeForUtf8ByteLength(pValue, SQLiteString.ThirtyBits));

    /// <summary>
    /// Converts the specified native UTF-8 string pointer of the specified
    /// length into a managed string.
    /// </summary>
    /// <param name="pValue">The native UTF-8 string pointer.</param>
    /// <param name="length">The length of the native string, in bytes.</param>
    /// <returns>The managed string or null upon failure.</returns>
    public static string StringFromUtf8IntPtr(IntPtr pValue, int length)
    {
      if (pValue == IntPtr.Zero)
        return (string) null;
      if (length <= 0)
        return string.Empty;
      byte[] numArray = new byte[length];
      Marshal.Copy(pValue, numArray, 0, length);
      return SQLiteString.GetStringFromUtf8Bytes(numArray);
    }

    /// <summary>
    /// Converts the specified managed string into a native NUL-terminated
    /// UTF-8 string pointer using memory obtained from the SQLite core
    /// library.
    /// </summary>
    /// <param name="value">The managed string to convert.</param>
    /// <returns>
    /// The native NUL-terminated UTF-8 string pointer or
    /// <see cref="F:System.IntPtr.Zero" /> upon failure.
    /// </returns>
    public static IntPtr Utf8IntPtrFromString(string value) => SQLiteString.Utf8IntPtrFromString(value, true);

    /// <summary>
    /// Converts the specified managed string into a native NUL-terminated
    /// UTF-8 string pointer using memory obtained from the SQLite core
    /// library.
    /// </summary>
    /// <param name="value">The managed string to convert.</param>
    /// <param name="tracked">
    /// Non-zero to obtain memory from the SQLite core library without
    /// adjusting the number of allocated bytes currently being tracked
    /// by the <see cref="T:System.Data.SQLite.SQLiteMemory" /> class.
    /// </param>
    /// <returns>
    /// The native NUL-terminated UTF-8 string pointer or
    /// <see cref="F:System.IntPtr.Zero" /> upon failure.
    /// </returns>
    public static IntPtr Utf8IntPtrFromString(string value, bool tracked)
    {
      int length = 0;
      return SQLiteString.Utf8IntPtrFromString(value, tracked, ref length);
    }

    /// <summary>
    /// Converts the specified managed string into a native NUL-terminated
    /// UTF-8 string pointer using memory obtained from the SQLite core
    /// library.
    /// </summary>
    /// <param name="value">The managed string to convert.</param>
    /// <param name="length">The length of the native string, in bytes.</param>
    /// <returns>
    /// The native NUL-terminated UTF-8 string pointer or
    /// <see cref="F:System.IntPtr.Zero" /> upon failure.
    /// </returns>
    public static IntPtr Utf8IntPtrFromString(string value, ref int length) => SQLiteString.Utf8IntPtrFromString(value, true, ref length);

    /// <summary>
    /// Converts the specified managed string into a native NUL-terminated
    /// UTF-8 string pointer using memory obtained from the SQLite core
    /// library.
    /// </summary>
    /// <param name="value">The managed string to convert.</param>
    /// <param name="tracked">
    /// Non-zero to obtain memory from the SQLite core library without
    /// adjusting the number of allocated bytes currently being tracked
    /// by the <see cref="T:System.Data.SQLite.SQLiteMemory" /> class.
    /// </param>
    /// <param name="length">The length of the native string, in bytes.</param>
    /// <returns>
    /// The native NUL-terminated UTF-8 string pointer or
    /// <see cref="F:System.IntPtr.Zero" /> upon failure.
    /// </returns>
    public static IntPtr Utf8IntPtrFromString(string value, bool tracked, ref int length)
    {
      if (value == null)
        return IntPtr.Zero;
      IntPtr zero = IntPtr.Zero;
      byte[] utf8BytesFromString = SQLiteString.GetUtf8BytesFromString(value);
      if (utf8BytesFromString == null)
        return IntPtr.Zero;
      length = utf8BytesFromString.Length;
      IntPtr num = !tracked ? SQLiteMemory.AllocateUntracked(length + 1) : SQLiteMemory.Allocate(length + 1);
      if (num == IntPtr.Zero)
        return IntPtr.Zero;
      Marshal.Copy(utf8BytesFromString, 0, num, length);
      Marshal.WriteByte(num, length, (byte) 0);
      return num;
    }

    /// <summary>
    /// Converts a logical array of native NUL-terminated UTF-8 string
    /// pointers into an array of managed strings.
    /// </summary>
    /// <param name="argc">
    /// The number of elements in the logical array of native
    /// NUL-terminated UTF-8 string pointers.
    /// </param>
    /// <param name="argv">
    /// The native pointer to the logical array of native NUL-terminated
    /// UTF-8 string pointers to convert.
    /// </param>
    /// <returns>The array of managed strings or null upon failure.</returns>
    public static string[] StringArrayFromUtf8SizeAndIntPtr(int argc, IntPtr argv)
    {
      if (argc < 0)
        return (string[]) null;
      if (argv == IntPtr.Zero)
        return (string[]) null;
      string[] strArray = new string[argc];
      int index = 0;
      int offset = 0;
      while (index < strArray.Length)
      {
        IntPtr pValue = SQLiteMarshal.ReadIntPtr(argv, offset);
        strArray[index] = pValue != IntPtr.Zero ? SQLiteString.StringFromUtf8IntPtr(pValue) : (string) null;
        ++index;
        offset += IntPtr.Size;
      }
      return strArray;
    }

    /// <summary>
    /// Converts an array of managed strings into an array of native
    /// NUL-terminated UTF-8 string pointers.
    /// </summary>
    /// <param name="values">The array of managed strings to convert.</param>
    /// <param name="tracked">
    /// Non-zero to obtain memory from the SQLite core library without
    /// adjusting the number of allocated bytes currently being tracked
    /// by the <see cref="T:System.Data.SQLite.SQLiteMemory" /> class.
    /// </param>
    /// <returns>
    /// The array of native NUL-terminated UTF-8 string pointers or null
    /// upon failure.
    /// </returns>
    public static IntPtr[] Utf8IntPtrArrayFromStringArray(string[] values, bool tracked)
    {
      if (values == null)
        return (IntPtr[]) null;
      IntPtr[] numArray = new IntPtr[values.Length];
      for (int index = 0; index < numArray.Length; ++index)
        numArray[index] = SQLiteString.Utf8IntPtrFromString(values[index], tracked);
      return numArray;
    }
  }
}
