// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.UnsafeNativeMethodsWin32
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;
using System.Security;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class declares P/Invoke methods to call native Win32 APIs.
  /// </summary>
  [SuppressUnmanagedCodeSecurity]
  internal static class UnsafeNativeMethodsWin32
  {
    /// <summary>
    /// This is the P/Invoke method that wraps the native Win32 LoadLibrary
    /// function.  See the MSDN documentation for full details on what it
    /// does.
    /// </summary>
    /// <param name="fileName">The name of the executable library.</param>
    /// <returns>
    /// The native module handle upon success -OR- IntPtr.Zero on failure.
    /// </returns>
    [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
    internal static extern IntPtr LoadLibrary(string fileName);

    /// <summary>
    /// This is the P/Invoke method that wraps the native Win32 GetSystemInfo
    /// function.  See the MSDN documentation for full details on what it
    /// does.
    /// </summary>
    /// <param name="systemInfo">
    /// The system information structure to be filled in by the function.
    /// </param>
    [DllImport("kernel32")]
    internal static extern void GetSystemInfo(
      out UnsafeNativeMethodsWin32.SYSTEM_INFO systemInfo);

    /// <summary>
    /// This enumeration contains the possible values for the processor
    /// architecture field of the system information structure.
    /// </summary>
    internal enum ProcessorArchitecture : ushort
    {
      Intel = 0,
      MIPS = 1,
      Alpha = 2,
      PowerPC = 3,
      SHx = 4,
      ARM = 5,
      IA64 = 6,
      Alpha64 = 7,
      MSIL = 8,
      AMD64 = 9,
      IA32_on_Win64 = 10, // 0x000A
      Unknown = 65535, // 0xFFFF
    }

    /// <summary>
    /// This structure contains information about the current computer. This
    /// includes the processor type, page size, memory addresses, etc.
    /// </summary>
    internal struct SYSTEM_INFO
    {
      public UnsafeNativeMethodsWin32.ProcessorArchitecture wProcessorArchitecture;
      public ushort wReserved;
      public uint dwPageSize;
      public IntPtr lpMinimumApplicationAddress;
      public IntPtr lpMaximumApplicationAddress;
      public IntPtr dwActiveProcessorMask;
      public uint dwNumberOfProcessors;
      public uint dwProcessorType;
      public uint dwAllocationGranularity;
      public ushort wProcessorLevel;
      public ushort wProcessorRevision;
    }
  }
}
