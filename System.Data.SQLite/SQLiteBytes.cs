// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteBytes
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class contains static methods that are used to deal with native
  /// pointers to memory blocks that logically contain arrays of bytes to be
  /// used with the SQLite core library.
  /// </summary>
  internal static class SQLiteBytes
  {
    /// <summary>
    /// Converts a native pointer to a logical array of bytes of the
    /// specified length into a managed byte array.
    /// </summary>
    /// <param name="pValue">
    /// The native pointer to the logical array of bytes to convert.
    /// </param>
    /// <param name="length">
    /// The length, in bytes, of the logical array of bytes to convert.
    /// </param>
    /// <returns>The managed byte array or null upon failure.</returns>
    public static byte[] FromIntPtr(IntPtr pValue, int length)
    {
      if (pValue == IntPtr.Zero)
        return (byte[]) null;
      if (length == 0)
        return new byte[0];
      byte[] destination = new byte[length];
      Marshal.Copy(pValue, destination, 0, length);
      return destination;
    }

    /// <summary>
    /// Converts a managed byte array into a native pointer to a logical
    /// array of bytes.
    /// </summary>
    /// <param name="value">The managed byte array to convert.</param>
    /// <returns>
    /// The native pointer to a logical byte array or null upon failure.
    /// </returns>
    public static IntPtr ToIntPtr(byte[] value)
    {
      int length = 0;
      return SQLiteBytes.ToIntPtr(value, ref length);
    }

    /// <summary>
    /// Converts a managed byte array into a native pointer to a logical
    /// array of bytes.
    /// </summary>
    /// <param name="value">The managed byte array to convert.</param>
    /// <param name="length">
    /// The length, in bytes, of the converted logical array of bytes.
    /// </param>
    /// <returns>
    /// The native pointer to a logical byte array or null upon failure.
    /// </returns>
    public static IntPtr ToIntPtr(byte[] value, ref int length)
    {
      if (value == null)
        return IntPtr.Zero;
      length = value.Length;
      if (length == 0)
        return IntPtr.Zero;
      IntPtr destination = SQLiteMemory.Allocate(length);
      if (destination == IntPtr.Zero)
        return IntPtr.Zero;
      Marshal.Copy(value, 0, destination, length);
      return destination;
    }
  }
}
