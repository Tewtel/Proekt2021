// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.UnsafeNativeMethodsPosix
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class declares P/Invoke methods to call native POSIX APIs.
  /// </summary>
  [SuppressUnmanagedCodeSecurity]
  internal static class UnsafeNativeMethodsPosix
  {
    /// <summary>For use with dlopen(), bind function calls lazily.</summary>
    internal const int RTLD_LAZY = 1;
    /// <summary>
    /// For use with dlopen(), bind function calls immediately.
    /// </summary>
    internal const int RTLD_NOW = 2;
    /// <summary>
    /// For use with dlopen(), make symbols globally available.
    /// </summary>
    internal const int RTLD_GLOBAL = 256;
    /// <summary>
    /// For use with dlopen(), opposite of RTLD_GLOBAL, and the default.
    /// </summary>
    internal const int RTLD_LOCAL = 0;
    /// <summary>
    /// For use with dlopen(), the defaults used by this class.
    /// </summary>
    internal const int RTLD_DEFAULT = 258;
    /// <summary>
    /// These are the characters used to separate the string fields within
    /// the raw buffer returned by the <see cref="M:System.Data.SQLite.UnsafeNativeMethodsPosix.uname(System.Data.SQLite.UnsafeNativeMethodsPosix.utsname_interop@)" /> P/Invoke method.
    /// </summary>
    private static readonly char[] utsNameSeparators = new char[1];

    /// <summary>
    /// This is the P/Invoke method that wraps the native Unix uname
    /// function.  See the POSIX documentation for full details on what it
    /// does.
    /// </summary>
    /// <param name="name">
    /// Structure containing a preallocated byte buffer to fill with the
    /// requested information.
    /// </param>
    /// <returns>Zero for success and less than zero upon failure.</returns>
    [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
    private static extern int uname(out UnsafeNativeMethodsPosix.utsname_interop name);

    /// <summary>
    /// This is the P/Invoke method that wraps the native Unix dlopen
    /// function.  See the POSIX documentation for full details on what it
    /// does.
    /// </summary>
    /// <param name="fileName">The name of the executable library.</param>
    /// <param name="mode">
    /// This must be a combination of the individual bit flags RTLD_LAZY,
    /// RTLD_NOW, RTLD_GLOBAL, and/or RTLD_LOCAL.
    /// </param>
    /// <returns>
    /// The native module handle upon success -OR- IntPtr.Zero on failure.
    /// </returns>
    [DllImport("__Internal", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
    internal static extern IntPtr dlopen(string fileName, int mode);

    /// <summary>
    /// This is the P/Invoke method that wraps the native Unix dlclose
    /// function.  See the POSIX documentation for full details on what it
    /// does.
    /// </summary>
    /// <param name="module">The handle to the loaded native library.</param>
    /// <returns>Zero upon success -OR- non-zero on failure.</returns>
    [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
    internal static extern int dlclose(IntPtr module);

    /// <summary>
    /// This method is a wrapper around the <see cref="M:System.Data.SQLite.UnsafeNativeMethodsPosix.uname(System.Data.SQLite.UnsafeNativeMethodsPosix.utsname_interop@)" /> P/Invoke
    /// method that extracts and returns the human readable strings from
    /// the raw buffer.
    /// </summary>
    /// <param name="utsName">
    /// This structure, which contains strings, will be filled based on the
    /// data placed in the raw buffer returned by the <see cref="M:System.Data.SQLite.UnsafeNativeMethodsPosix.uname(System.Data.SQLite.UnsafeNativeMethodsPosix.utsname_interop@)" />
    /// P/Invoke method.
    /// </param>
    /// <returns>Non-zero upon success; otherwise, zero.</returns>
    internal static bool GetOsVersionInfo(ref UnsafeNativeMethodsPosix.utsname utsName)
    {
      try
      {
        UnsafeNativeMethodsPosix.utsname_interop name;
        if (UnsafeNativeMethodsPosix.uname(out name) < 0 || name.buffer == null)
          return false;
        string str = Encoding.UTF8.GetString(name.buffer);
        if (str == null || UnsafeNativeMethodsPosix.utsNameSeparators == null)
          return false;
        string[] strArray = str.Trim(UnsafeNativeMethodsPosix.utsNameSeparators).Split(UnsafeNativeMethodsPosix.utsNameSeparators, StringSplitOptions.RemoveEmptyEntries);
        if (strArray == null)
          return false;
        UnsafeNativeMethodsPosix.utsname utsname = new UnsafeNativeMethodsPosix.utsname();
        if (strArray.Length >= 1)
          utsname.sysname = strArray[0];
        if (strArray.Length >= 2)
          utsname.nodename = strArray[1];
        if (strArray.Length >= 3)
          utsname.release = strArray[2];
        if (strArray.Length >= 4)
          utsname.version = strArray[3];
        if (strArray.Length >= 5)
          utsname.machine = strArray[4];
        utsName = utsname;
        return true;
      }
      catch
      {
      }
      return false;
    }

    /// <summary>
    /// This structure is used when running on POSIX operating systems
    /// to store information about the current machine, including the
    /// human readable name of the operating system as well as that of
    /// the underlying hardware.
    /// </summary>
    internal sealed class utsname
    {
      public string sysname;
      public string nodename;
      public string release;
      public string version;
      public string machine;
    }

    /// <summary>
    /// This structure is passed directly to the P/Invoke method to
    /// obtain the information about the current machine, including
    /// the human readable name of the operating system as well as
    /// that of the underlying hardware.
    /// </summary>
    private struct utsname_interop
    {
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
      public byte[] buffer;
    }
  }
}
