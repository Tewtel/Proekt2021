// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteMarshal
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class contains static methods that are used to perform several
  /// low-level data marshalling tasks between native and managed code.
  /// </summary>
  internal static class SQLiteMarshal
  {
    /// <summary>
    /// Returns a new <see cref="T:System.IntPtr" /> object instance based on the
    /// specified <see cref="T:System.IntPtr" /> object instance and an integer
    /// offset.
    /// </summary>
    /// <param name="pointer">
    /// The <see cref="T:System.IntPtr" /> object instance representing the base
    /// memory location.
    /// </param>
    /// <param name="offset">
    /// The integer offset from the base memory location that the new
    /// <see cref="T:System.IntPtr" /> object instance should point to.
    /// </param>
    /// <returns>
    /// The new <see cref="T:System.IntPtr" /> object instance.
    /// </returns>
    public static IntPtr IntPtrForOffset(IntPtr pointer, int offset) => new IntPtr(pointer.ToInt64() + (long) offset);

    /// <summary>
    /// Rounds up an integer size to the next multiple of the alignment.
    /// </summary>
    /// <param name="size">The size, in bytes, to be rounded up.</param>
    /// <param name="alignment">
    /// The required alignment for the return value.
    /// </param>
    /// <returns>
    /// The size, in bytes, rounded up to the next multiple of the
    /// alignment.  This value may end up being the same as the original
    /// size.
    /// </returns>
    public static int RoundUp(int size, int alignment)
    {
      int num = alignment - 1;
      return size + num & ~num;
    }

    /// <summary>
    /// Determines the offset, in bytes, of the next structure member.
    /// </summary>
    /// <param name="offset">
    /// The offset, in bytes, of the current structure member.
    /// </param>
    /// <param name="size">
    /// The size, in bytes, of the current structure member.
    /// </param>
    /// <param name="alignment">
    /// The alignment, in bytes, of the next structure member.
    /// </param>
    /// <returns>The offset, in bytes, of the next structure member.</returns>
    public static int NextOffsetOf(int offset, int size, int alignment) => SQLiteMarshal.RoundUp(offset + size, alignment);

    /// <summary>
    /// Reads a <see cref="T:System.Int32" /> value from the specified memory
    /// location.
    /// </summary>
    /// <param name="pointer">
    /// The <see cref="T:System.IntPtr" /> object instance representing the base
    /// memory location.
    /// </param>
    /// <param name="offset">
    /// The integer offset from the base memory location where the
    /// <see cref="T:System.Int32" /> value to be read is located.
    /// </param>
    /// <returns>
    /// The <see cref="T:System.Int32" /> value at the specified memory location.
    /// </returns>
    public static int ReadInt32(IntPtr pointer, int offset) => Marshal.ReadInt32(pointer, offset);

    /// <summary>
    /// Reads a <see cref="T:System.Int64" /> value from the specified memory
    /// location.
    /// </summary>
    /// <param name="pointer">
    /// The <see cref="T:System.IntPtr" /> object instance representing the base
    /// memory location.
    /// </param>
    /// <param name="offset">
    /// The integer offset from the base memory location where the
    /// <see cref="T:System.Int64" /> value to be read is located.
    /// </param>
    /// <returns>
    /// The <see cref="T:System.Int64" /> value at the specified memory location.
    /// </returns>
    public static long ReadInt64(IntPtr pointer, int offset) => Marshal.ReadInt64(pointer, offset);

    /// <summary>
    /// Reads a <see cref="T:System.Double" /> value from the specified memory
    /// location.
    /// </summary>
    /// <param name="pointer">
    /// The <see cref="T:System.IntPtr" /> object instance representing the base
    /// memory location.
    /// </param>
    /// <param name="offset">
    /// The integer offset from the base memory location where the
    /// <see cref="T:System.Double" /> to be read is located.
    /// </param>
    /// <returns>
    /// The <see cref="T:System.Double" /> value at the specified memory location.
    /// </returns>
    public static double ReadDouble(IntPtr pointer, int offset) => BitConverter.Int64BitsToDouble(Marshal.ReadInt64(pointer, offset));

    /// <summary>
    /// Reads an <see cref="T:System.IntPtr" /> value from the specified memory
    /// location.
    /// </summary>
    /// <param name="pointer">
    /// The <see cref="T:System.IntPtr" /> object instance representing the base
    /// memory location.
    /// </param>
    /// <param name="offset">
    /// The integer offset from the base memory location where the
    /// <see cref="T:System.IntPtr" /> value to be read is located.
    /// </param>
    /// <returns>
    /// The <see cref="T:System.IntPtr" /> value at the specified memory location.
    /// </returns>
    public static IntPtr ReadIntPtr(IntPtr pointer, int offset) => Marshal.ReadIntPtr(pointer, offset);

    /// <summary>
    /// Writes an <see cref="T:System.Int32" /> value to the specified memory
    /// location.
    /// </summary>
    /// <param name="pointer">
    /// The <see cref="T:System.IntPtr" /> object instance representing the base
    /// memory location.
    /// </param>
    /// <param name="offset">
    /// The integer offset from the base memory location where the
    /// <see cref="T:System.Int32" /> value to be written is located.
    /// </param>
    /// <param name="value">
    /// The <see cref="T:System.Int32" /> value to write.
    /// </param>
    public static void WriteInt32(IntPtr pointer, int offset, int value) => Marshal.WriteInt32(pointer, offset, value);

    /// <summary>
    /// Writes an <see cref="T:System.Int64" /> value to the specified memory
    /// location.
    /// </summary>
    /// <param name="pointer">
    /// The <see cref="T:System.IntPtr" /> object instance representing the base
    /// memory location.
    /// </param>
    /// <param name="offset">
    /// The integer offset from the base memory location where the
    /// <see cref="T:System.Int64" /> value to be written is located.
    /// </param>
    /// <param name="value">
    /// The <see cref="T:System.Int64" /> value to write.
    /// </param>
    public static void WriteInt64(IntPtr pointer, int offset, long value) => Marshal.WriteInt64(pointer, offset, value);

    /// <summary>
    /// Writes a <see cref="T:System.Double" /> value to the specified memory
    /// location.
    /// </summary>
    /// <param name="pointer">
    /// The <see cref="T:System.IntPtr" /> object instance representing the base
    /// memory location.
    /// </param>
    /// <param name="offset">
    /// The integer offset from the base memory location where the
    /// <see cref="T:System.Double" /> value to be written is located.
    /// </param>
    /// <param name="value">
    /// The <see cref="T:System.Double" /> value to write.
    /// </param>
    public static void WriteDouble(IntPtr pointer, int offset, double value) => Marshal.WriteInt64(pointer, offset, BitConverter.DoubleToInt64Bits(value));

    /// <summary>
    /// Writes a <see cref="T:System.IntPtr" /> value to the specified memory
    /// location.
    /// </summary>
    /// <param name="pointer">
    /// The <see cref="T:System.IntPtr" /> object instance representing the base
    /// memory location.
    /// </param>
    /// <param name="offset">
    /// The integer offset from the base memory location where the
    /// <see cref="T:System.IntPtr" /> value to be written is located.
    /// </param>
    /// <param name="value">
    /// The <see cref="T:System.IntPtr" /> value to write.
    /// </param>
    public static void WriteIntPtr(IntPtr pointer, int offset, IntPtr value) => Marshal.WriteIntPtr(pointer, offset, value);

    /// <summary>Generates a hash code value for the object.</summary>
    /// <param name="value">
    /// The object instance used to calculate the hash code.
    /// </param>
    /// <param name="identity">
    /// Non-zero if different object instances with the same value should
    /// generate different hash codes, where applicable.  This parameter
    /// has no effect on the .NET Compact Framework.
    /// </param>
    /// <returns>The hash code value -OR- zero if the object is null.</returns>
    public static int GetHashCode(object value, bool identity)
    {
      if (identity)
        return RuntimeHelpers.GetHashCode(value);
      return value == null ? 0 : value.GetHashCode();
    }
  }
}
