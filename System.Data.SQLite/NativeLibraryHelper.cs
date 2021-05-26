// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.NativeLibraryHelper
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This static class provides a thin wrapper around the native library
  /// loading features of the underlying platform.
  /// </summary>
  internal static class NativeLibraryHelper
  {
    /// <summary>
    /// Attempts to load the specified native library file using the Win32
    /// API.
    /// </summary>
    /// <param name="fileName">
    /// The file name of the native library to load.
    /// </param>
    /// <returns>
    /// The native module handle upon success -OR- IntPtr.Zero on failure.
    /// </returns>
    private static IntPtr LoadLibraryWin32(string fileName) => UnsafeNativeMethodsWin32.LoadLibrary(fileName);

    /// <summary>
    /// Attempts to determine the machine name of the current process using
    /// the Win32 API.
    /// </summary>
    /// <returns>
    /// The machine name for the current process -OR- null on failure.
    /// </returns>
    private static string GetMachineWin32()
    {
      try
      {
        UnsafeNativeMethodsWin32.SYSTEM_INFO systemInfo;
        UnsafeNativeMethodsWin32.GetSystemInfo(out systemInfo);
        return systemInfo.wProcessorArchitecture.ToString();
      }
      catch
      {
      }
      return (string) null;
    }

    /// <summary>
    /// Attempts to load the specified native library file using the POSIX
    /// API.
    /// </summary>
    /// <param name="fileName">
    /// The file name of the native library to load.
    /// </param>
    /// <returns>
    /// The native module handle upon success -OR- IntPtr.Zero on failure.
    /// </returns>
    private static IntPtr LoadLibraryPosix(string fileName) => UnsafeNativeMethodsPosix.dlopen(fileName, 258);

    /// <summary>
    /// Attempts to determine the machine name of the current process using
    /// the POSIX API.
    /// </summary>
    /// <returns>
    /// The machine name for the current process -OR- null on failure.
    /// </returns>
    private static string GetMachinePosix()
    {
      try
      {
        UnsafeNativeMethodsPosix.utsname utsName = (UnsafeNativeMethodsPosix.utsname) null;
        if (UnsafeNativeMethodsPosix.GetOsVersionInfo(ref utsName))
        {
          if (utsName != null)
            return utsName.machine;
        }
      }
      catch
      {
      }
      return (string) null;
    }

    /// <summary>Attempts to load the specified native library file.</summary>
    /// <param name="fileName">
    /// The file name of the native library to load.
    /// </param>
    /// <returns>
    /// The native module handle upon success -OR- IntPtr.Zero on failure.
    /// </returns>
    public static IntPtr LoadLibrary(string fileName)
    {
      NativeLibraryHelper.LoadLibraryCallback loadLibraryCallback = new NativeLibraryHelper.LoadLibraryCallback(NativeLibraryHelper.LoadLibraryWin32);
      if (!HelperMethods.IsWindows())
        loadLibraryCallback = new NativeLibraryHelper.LoadLibraryCallback(NativeLibraryHelper.LoadLibraryPosix);
      return loadLibraryCallback(fileName);
    }

    /// <summary>
    /// Attempts to determine the machine name of the current process.
    /// </summary>
    /// <returns>
    /// The machine name for the current process -OR- null on failure.
    /// </returns>
    public static string GetMachine()
    {
      NativeLibraryHelper.GetMachineCallback getMachineCallback = new NativeLibraryHelper.GetMachineCallback(NativeLibraryHelper.GetMachineWin32);
      if (!HelperMethods.IsWindows())
        getMachineCallback = new NativeLibraryHelper.GetMachineCallback(NativeLibraryHelper.GetMachinePosix);
      return getMachineCallback();
    }

    /// <summary>
    /// This delegate is used to wrap the concept of loading a native
    /// library, based on a file name, and returning the loaded module
    /// handle.
    /// </summary>
    /// <param name="fileName">
    /// The file name of the native library to load.
    /// </param>
    /// <returns>
    /// The native module handle upon success -OR- IntPtr.Zero on failure.
    /// </returns>
    private delegate IntPtr LoadLibraryCallback(string fileName);

    /// <summary>
    /// This delegate is used to wrap the concept of querying the machine
    /// name of the current process.
    /// </summary>
    /// <returns>
    /// The machine name for the current process -OR- null on failure.
    /// </returns>
    private delegate string GetMachineCallback();
  }
}
