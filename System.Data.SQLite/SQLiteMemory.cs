// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteMemory
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class contains static methods that are used to allocate,
  /// manipulate, and free native memory provided by the SQLite core library.
  /// </summary>
  internal static class SQLiteMemory
  {
    /// <summary>
    /// Determines if the native sqlite3_msize() API can be used, based on
    /// the available version of the SQLite core library.
    /// </summary>
    /// <returns>
    /// Non-zero if the native sqlite3_msize() API is supported by the
    /// SQLite core library.
    /// </returns>
    private static bool CanUseSize64() => UnsafeNativeMethods.sqlite3_libversion_number() >= 3008007;

    /// <summary>
    /// Allocates at least the specified number of bytes of native memory
    /// via the SQLite core library sqlite3_malloc() function and returns
    /// the resulting native pointer.  If the TRACK_MEMORY_BYTES option
    /// was enabled at compile-time, adjusts the number of bytes currently
    /// allocated by this class.
    /// </summary>
    /// <param name="size">The number of bytes to allocate.</param>
    /// <returns>
    /// The native pointer that points to a block of memory of at least the
    /// specified size -OR- <see cref="F:System.IntPtr.Zero" /> if the memory could
    /// not be allocated.
    /// </returns>
    public static IntPtr Allocate(int size) => UnsafeNativeMethods.sqlite3_malloc(size);

    /// <summary>
    /// Allocates at least the specified number of bytes of native memory
    /// via the SQLite core library sqlite3_malloc64() function and returns
    /// the resulting native pointer.  If the TRACK_MEMORY_BYTES option
    /// was enabled at compile-time, adjusts the number of bytes currently
    /// allocated by this class.
    /// </summary>
    /// <param name="size">The number of bytes to allocate.</param>
    /// <returns>
    /// The native pointer that points to a block of memory of at least the
    /// specified size -OR- <see cref="F:System.IntPtr.Zero" /> if the memory could
    /// not be allocated.
    /// </returns>
    public static IntPtr Allocate64(ulong size) => UnsafeNativeMethods.sqlite3_malloc64(size);

    /// <summary>
    /// Allocates at least the specified number of bytes of native memory
    /// via the SQLite core library sqlite3_malloc() function and returns
    /// the resulting native pointer without adjusting the number of
    /// allocated bytes currently tracked by this class.  This is useful
    /// when dealing with blocks of memory that will be freed directly by
    /// the SQLite core library.
    /// </summary>
    /// <param name="size">The number of bytes to allocate.</param>
    /// <returns>
    /// The native pointer that points to a block of memory of at least the
    /// specified size -OR- <see cref="F:System.IntPtr.Zero" /> if the memory could
    /// not be allocated.
    /// </returns>
    public static IntPtr AllocateUntracked(int size) => UnsafeNativeMethods.sqlite3_malloc(size);

    /// <summary>
    /// Allocates at least the specified number of bytes of native memory
    /// via the SQLite core library sqlite3_malloc64() function and returns
    /// the resulting native pointer without adjusting the number of
    /// allocated bytes currently tracked by this class.  This is useful
    /// when dealing with blocks of memory that will be freed directly by
    /// the SQLite core library.
    /// </summary>
    /// <param name="size">The number of bytes to allocate.</param>
    /// <returns>
    /// The native pointer that points to a block of memory of at least the
    /// specified size -OR- <see cref="F:System.IntPtr.Zero" /> if the memory could
    /// not be allocated.
    /// </returns>
    public static IntPtr Allocate64Untracked(ulong size) => UnsafeNativeMethods.sqlite3_malloc64(size);

    /// <summary>
    /// Gets and returns the actual size of the specified memory block
    /// that was previously obtained from the <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate(System.Int32)" />,
    /// <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate64(System.UInt64)" />, <see cref="M:System.Data.SQLite.SQLiteMemory.AllocateUntracked(System.Int32)" />, or
    /// <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate64Untracked(System.UInt64)" /> methods or directly from the
    /// SQLite core library.
    /// </summary>
    /// <param name="pMemory">
    /// The native pointer to the memory block previously obtained from
    /// the <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate(System.Int32)" />, <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate64(System.UInt64)" />,
    /// <see cref="M:System.Data.SQLite.SQLiteMemory.AllocateUntracked(System.Int32)" />, or
    /// <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate64Untracked(System.UInt64)" /> methods or directly from the
    /// SQLite core library.
    /// </param>
    /// <returns>
    /// The actual size, in bytes, of the memory block specified via the
    /// native pointer.
    /// </returns>
    public static int Size(IntPtr pMemory) => UnsafeNativeMethods.sqlite3_malloc_size_interop(pMemory);

    /// <summary>
    /// Gets and returns the actual size of the specified memory block
    /// that was previously obtained from the <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate(System.Int32)" />,
    /// <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate64(System.UInt64)" />, <see cref="M:System.Data.SQLite.SQLiteMemory.AllocateUntracked(System.Int32)" />, or
    /// <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate64Untracked(System.UInt64)" /> methods or directly from the
    /// SQLite core library.
    /// </summary>
    /// <param name="pMemory">
    /// The native pointer to the memory block previously obtained from
    /// the <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate(System.Int32)" />, <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate64(System.UInt64)" />,
    /// <see cref="M:System.Data.SQLite.SQLiteMemory.AllocateUntracked(System.Int32)" />, or
    /// <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate64Untracked(System.UInt64)" /> methods or directly from the
    /// SQLite core library.
    /// </param>
    /// <returns>
    /// The actual size, in bytes, of the memory block specified via the
    /// native pointer.
    /// </returns>
    public static ulong Size64(IntPtr pMemory) => UnsafeNativeMethods.sqlite3_msize(pMemory);

    /// <summary>
    /// Frees a memory block previously obtained from the
    /// <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate(System.Int32)" /> or <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate64(System.UInt64)" /> methods.  If
    /// the TRACK_MEMORY_BYTES option was enabled at compile-time, adjusts
    /// the number of bytes currently allocated by this class.
    /// </summary>
    /// <param name="pMemory">
    /// The native pointer to the memory block previously obtained from the
    /// <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate(System.Int32)" /> or <see cref="M:System.Data.SQLite.SQLiteMemory.Allocate64(System.UInt64)" /> methods.
    /// </param>
    public static void Free(IntPtr pMemory) => UnsafeNativeMethods.sqlite3_free(pMemory);

    /// <summary>
    /// Frees a memory block previously obtained from the SQLite core
    /// library without adjusting the number of allocated bytes currently
    /// tracked by this class.  This is useful when dealing with blocks of
    /// memory that were not allocated using this class.
    /// </summary>
    /// <param name="pMemory">
    /// The native pointer to the memory block previously obtained from the
    /// SQLite core library.
    /// </param>
    public static void FreeUntracked(IntPtr pMemory) => UnsafeNativeMethods.sqlite3_free(pMemory);
  }
}
