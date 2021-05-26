// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.HelperMethods
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace System.Data.SQLite
{
  /// <summary>
  /// This static class provides some methods that are shared between the
  /// native library pre-loader and other classes.
  /// </summary>
  internal static class HelperMethods
  {
    private const string DisplayNullObject = "<nullObject>";
    private const string DisplayEmptyString = "<emptyString>";
    private const string DisplayStringFormat = "\"{0}\"";
    private const string DisplayNullArray = "<nullArray>";
    private const string DisplayEmptyArray = "<emptyArray>";
    private const char ArrayOpen = '[';
    private const string ElementSeparator = ", ";
    private const char ArrayClose = ']';
    private static readonly char[] SpaceChars = new char[6]
    {
      '\t',
      '\n',
      '\r',
      '\v',
      '\f',
      ' '
    };
    /// <summary>
    /// This lock is used to protect the static <see cref="F:System.Data.SQLite.HelperMethods.isMono" /> and
    /// <see cref="F:System.Data.SQLite.HelperMethods.isDotNetCore" /> fields.
    /// </summary>
    private static readonly object staticSyncRoot = new object();
    /// <summary>This type is only present when running on Mono.</summary>
    private static readonly string MonoRuntimeType = "Mono.Runtime";
    /// <summary>This type is only present when running on .NET Core.</summary>
    private static readonly string DotNetCoreLibType = "System.CoreLib";
    /// <summary>
    /// Keeps track of whether we are running on Mono.  Initially null, it is
    /// set by the <see cref="M:System.Data.SQLite.HelperMethods.IsMono" /> method on its first call.  Later, it
    /// is returned verbatim by the <see cref="M:System.Data.SQLite.HelperMethods.IsMono" /> method.
    /// </summary>
    private static bool? isMono = new bool?();
    /// <summary>
    /// Keeps track of whether we are running on .NET Core.  Initially null,
    /// it is set by the <see cref="M:System.Data.SQLite.HelperMethods.IsDotNetCore" /> method on its first
    /// call.  Later, it is returned verbatim by the
    /// <see cref="M:System.Data.SQLite.HelperMethods.IsDotNetCore" /> method.
    /// </summary>
    private static bool? isDotNetCore = new bool?();
    /// <summary>
    /// Keeps track of whether we successfully invoked the
    /// <see cref="M:System.Diagnostics.Debugger.Break" /> method.  Initially null, it is set by
    /// the <see cref="M:System.Data.SQLite.HelperMethods.MaybeBreakIntoDebugger" /> method on its first call.
    /// </summary>
    private static bool? debuggerBreak = new bool?();

    /// <summary>
    /// Determines the ID of the current process.  Only used for debugging.
    /// </summary>
    /// <returns>
    /// The ID of the current process -OR- zero if it cannot be determined.
    /// </returns>
    private static int GetProcessId()
    {
      Process currentProcess = Process.GetCurrentProcess();
      return currentProcess == null ? 0 : currentProcess.Id;
    }

    /// <summary>
    /// Determines whether or not this assembly is running on Mono.
    /// </summary>
    /// <returns>Non-zero if this assembly is running on Mono.</returns>
    private static bool IsMono()
    {
      try
      {
        lock (HelperMethods.staticSyncRoot)
        {
          if (!HelperMethods.isMono.HasValue)
            HelperMethods.isMono = new bool?(Type.GetType(HelperMethods.MonoRuntimeType) != (Type) null);
          return HelperMethods.isMono.Value;
        }
      }
      catch
      {
      }
      return false;
    }

    /// <summary>
    /// Determines whether or not this assembly is running on .NET Core.
    /// </summary>
    /// <returns>Non-zero if this assembly is running on .NET Core.</returns>
    public static bool IsDotNetCore()
    {
      try
      {
        lock (HelperMethods.staticSyncRoot)
        {
          if (!HelperMethods.isDotNetCore.HasValue)
            HelperMethods.isDotNetCore = new bool?(Type.GetType(HelperMethods.DotNetCoreLibType) != (Type) null);
          return HelperMethods.isDotNetCore.Value;
        }
      }
      catch
      {
      }
      return false;
    }

    /// <summary>
    /// Resets the cached value for the "PreLoadSQLite_BreakIntoDebugger"
    /// configuration setting.
    /// </summary>
    internal static void ResetBreakIntoDebugger()
    {
      lock (HelperMethods.staticSyncRoot)
        HelperMethods.debuggerBreak = new bool?();
    }

    /// <summary>
    /// If the "PreLoadSQLite_BreakIntoDebugger" configuration setting is
    /// present (e.g. via the environment), give the interactive user an
    /// opportunity to attach a debugger to the current process; otherwise,
    /// do nothing.
    /// </summary>
    internal static void MaybeBreakIntoDebugger()
    {
      lock (HelperMethods.staticSyncRoot)
      {
        if (HelperMethods.debuggerBreak.HasValue)
          return;
      }
      if (UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_BreakIntoDebugger", (string) null) != null)
      {
        try
        {
          Console.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Attach a debugger to process {0} and press any key to continue.", (object) HelperMethods.GetProcessId()));
          Console.ReadKey();
        }
        catch (Exception ex)
        {
          try
          {
            Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Failed to issue debugger prompt, {0} may be unusable: {1}", (object) typeof (Console), (object) ex));
          }
          catch
          {
          }
        }
        try
        {
          Debugger.Break();
          lock (HelperMethods.staticSyncRoot)
            HelperMethods.debuggerBreak = new bool?(true);
        }
        catch
        {
          lock (HelperMethods.staticSyncRoot)
            HelperMethods.debuggerBreak = new bool?(false);
          throw;
        }
      }
      else
      {
        lock (HelperMethods.staticSyncRoot)
          HelperMethods.debuggerBreak = new bool?(false);
      }
    }

    /// <summary>
    /// Determines the ID of the current thread.  Only used for debugging.
    /// </summary>
    /// <returns>
    /// The ID of the current thread -OR- zero if it cannot be determined.
    /// </returns>
    internal static int GetThreadId() => AppDomain.GetCurrentThreadId();

    /// <summary>
    /// Determines if the specified flags are present within the flags
    /// associated with the parent connection object.
    /// </summary>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <param name="hasFlags">The flags to check for.</param>
    /// <returns>
    /// Non-zero if the specified flag or flags were present; otherwise,
    /// zero.
    /// </returns>
    internal static bool HasFlags(SQLiteConnectionFlags flags, SQLiteConnectionFlags hasFlags) => (flags & hasFlags) == hasFlags;

    /// <summary>Determines if preparing a query should be logged.</summary>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>
    /// Non-zero if the query preparation should be logged; otherwise, zero.
    /// </returns>
    internal static bool LogPrepare(SQLiteConnectionFlags flags) => HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogPrepare);

    /// <summary>Determines if pre-parameter binding should be logged.</summary>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>
    /// Non-zero if the pre-parameter binding should be logged; otherwise,
    /// zero.
    /// </returns>
    internal static bool LogPreBind(SQLiteConnectionFlags flags) => HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogPreBind);

    /// <summary>Determines if parameter binding should be logged.</summary>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>
    /// Non-zero if the parameter binding should be logged; otherwise, zero.
    /// </returns>
    internal static bool LogBind(SQLiteConnectionFlags flags) => HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogBind);

    /// <summary>
    /// Determines if an exception in a native callback should be logged.
    /// </summary>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>
    /// Non-zero if the exception should be logged; otherwise, zero.
    /// </returns>
    internal static bool LogCallbackExceptions(SQLiteConnectionFlags flags) => HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogCallbackException);

    /// <summary>Determines if backup API errors should be logged.</summary>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>
    /// Non-zero if the backup API error should be logged; otherwise, zero.
    /// </returns>
    internal static bool LogBackup(SQLiteConnectionFlags flags) => HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogBackup);

    /// <summary>
    /// Determines if logging for the <see cref="T:System.Data.SQLite.SQLiteModule" /> class is
    /// disabled.
    /// </summary>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>
    /// Non-zero if logging for the <see cref="T:System.Data.SQLite.SQLiteModule" /> class is
    /// disabled; otherwise, zero.
    /// </returns>
    internal static bool NoLogModule(SQLiteConnectionFlags flags) => HelperMethods.HasFlags(flags, SQLiteConnectionFlags.NoLogModule);

    /// <summary>
    /// Determines if <see cref="T:System.Data.SQLite.SQLiteModule" /> errors should be logged.
    /// </summary>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>
    /// Non-zero if the <see cref="T:System.Data.SQLite.SQLiteModule" /> error should be logged;
    /// otherwise, zero.
    /// </returns>
    internal static bool LogModuleError(SQLiteConnectionFlags flags) => HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogModuleError);

    /// <summary>
    /// Determines if <see cref="T:System.Data.SQLite.SQLiteModule" /> exceptions should be
    /// logged.
    /// </summary>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>
    /// Non-zero if the <see cref="T:System.Data.SQLite.SQLiteModule" /> exception should be
    /// logged; otherwise, zero.
    /// </returns>
    internal static bool LogModuleException(SQLiteConnectionFlags flags) => HelperMethods.HasFlags(flags, SQLiteConnectionFlags.LogModuleException);

    /// <summary>
    /// Determines if the current process is running on one of the Windows
    /// [sub-]platforms.
    /// </summary>
    /// <returns>Non-zero when running on Windows; otherwise, zero.</returns>
    internal static bool IsWindows()
    {
      switch (Environment.OSVersion.Platform)
      {
        case PlatformID.Win32S:
        case PlatformID.Win32Windows:
        case PlatformID.Win32NT:
        case PlatformID.WinCE:
          return true;
        default:
          return false;
      }
    }

    /// <summary>
    /// This is a wrapper around the
    /// <see cref="M:System.String.Format(System.IFormatProvider,System.String,System.Object[])" /> method.
    /// On Mono, it has to call the method overload without the
    /// <see cref="T:System.IFormatProvider" /> parameter, due to a bug in Mono.
    /// </summary>
    /// <param name="provider">
    /// This is used for culture-specific formatting.
    /// </param>
    /// <param name="format">The format string.</param>
    /// <param name="args">An array the objects to format.</param>
    /// <returns>The resulting string.</returns>
    internal static string StringFormat(
      IFormatProvider provider,
      string format,
      params object[] args)
    {
      return HelperMethods.IsMono() ? string.Format(format, args) : string.Format(provider, format, args);
    }

    public static string ToDisplayString(object value)
    {
      if (value == null)
        return "<nullObject>";
      string str = value.ToString();
      if (str.Length == 0)
        return "<emptyString>";
      if (str.IndexOfAny(HelperMethods.SpaceChars) < 0)
        return str;
      return HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "\"{0}\"", (object) str);
    }

    public static string ToDisplayString(Array array)
    {
      if (array == null)
        return "<nullArray>";
      if (array.Length == 0)
        return "<emptyArray>";
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object obj in array)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append(", ");
        stringBuilder.Append(HelperMethods.ToDisplayString(obj));
      }
      if (stringBuilder.Length > 0)
      {
        stringBuilder.Insert(0, '[');
        stringBuilder.Append(']');
      }
      return stringBuilder.ToString();
    }
  }
}
