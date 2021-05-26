// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteLog
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace System.Data.SQLite
{
  /// <summary>
  /// Manages the SQLite custom logging functionality and the associated
  /// callback for the whole process.
  /// </summary>
  public static class SQLiteLog
  {
    /// <summary>
    /// Object used to synchronize access to the static instance data
    /// for this class.
    /// </summary>
    private static object syncRoot = new object();
    /// <summary>
    /// Member variable to store the AppDomain.DomainUnload event handler.
    /// </summary>
    private static EventHandler _domainUnload;
    /// <summary>The default log event handler.</summary>
    private static SQLiteLogEventHandler _defaultHandler;
    /// <summary>
    /// The log callback passed to native SQLite engine.  This must live
    /// as long as the SQLite library has a pointer to it.
    /// </summary>
    private static SQLiteLogCallback _callback;
    /// <summary>The base SQLite object to interop with.</summary>
    private static SQLiteBase _sql;
    /// <summary>
    /// The number of times that the <see cref="M:System.Data.SQLite.SQLiteLog.Initialize(System.String)" />
    /// has been called when the logging subystem was actually eligible
    /// to be initialized (i.e. without the "No_SQLiteLog" environment
    /// variable being set).
    /// </summary>
    private static int _initializeCallCount;
    /// <summary>
    /// This will be non-zero if an attempt was already made to initialize
    /// the (managed) logging subsystem.
    /// </summary>
    private static int _attemptedInitialize;
    /// <summary>
    /// This will be non-zero if logging is currently enabled.
    /// </summary>
    private static bool _enabled;

    /// <summary>
    /// Member variable to store the application log handler to call.
    /// </summary>
    private static event SQLiteLogEventHandler _handlers;

    /// <summary>Initializes the SQLite logging facilities.</summary>
    public static void Initialize() => SQLiteLog.Initialize((string) null);

    /// <summary>Initializes the SQLite logging facilities.</summary>
    /// <param name="className">
    /// The name of the managed class that called this method.  This
    /// parameter may be null.
    /// </param>
    internal static void Initialize(string className)
    {
      if (UnsafeNativeMethods.GetSettingValue("No_SQLiteLog", (string) null) != null)
        return;
      Interlocked.Increment(ref SQLiteLog._initializeCallCount);
      if (UnsafeNativeMethods.GetSettingValue("Initialize_SQLiteLog", (string) null) == null && Interlocked.Increment(ref SQLiteLog._attemptedInitialize) > 1)
      {
        Interlocked.Decrement(ref SQLiteLog._attemptedInitialize);
      }
      else
      {
        if (SQLite3.StaticIsInitialized() || !AppDomain.CurrentDomain.IsDefaultAppDomain() && UnsafeNativeMethods.GetSettingValue("Force_SQLiteLog", (string) null) == null)
          return;
        lock (SQLiteLog.syncRoot)
        {
          if (SQLiteLog._domainUnload == null)
          {
            SQLiteLog._domainUnload = new EventHandler(SQLiteLog.DomainUnload);
            AppDomain.CurrentDomain.DomainUnload += SQLiteLog._domainUnload;
          }
          if (SQLiteLog._sql == null)
            SQLiteLog._sql = (SQLiteBase) new SQLite3(SQLiteDateFormats.ISO8601, DateTimeKind.Unspecified, (string) null, IntPtr.Zero, (string) null, false);
          if (SQLiteLog._callback == null)
          {
            SQLiteLog._callback = new SQLiteLogCallback(SQLiteLog.LogCallback);
            SQLiteErrorCode errorCode = SQLiteLog._sql.SetLogCallback(SQLiteLog._callback);
            if (errorCode != SQLiteErrorCode.Ok)
            {
              SQLiteLog._callback = (SQLiteLogCallback) null;
              throw new SQLiteException(errorCode, "Failed to configure managed assembly logging.");
            }
          }
          if (UnsafeNativeMethods.GetSettingValue("Disable_SQLiteLog", (string) null) == null)
            SQLiteLog._enabled = true;
          SQLiteLog.AddDefaultHandler();
        }
      }
    }

    /// <summary>Handles the AppDomain being unloaded.</summary>
    /// <param name="sender">Should be null.</param>
    /// <param name="e">The data associated with this event.</param>
    private static void DomainUnload(object sender, EventArgs e)
    {
      lock (SQLiteLog.syncRoot)
      {
        SQLiteLog.RemoveDefaultHandler();
        SQLiteLog._enabled = false;
        if (SQLiteLog._sql != null)
        {
          SQLiteErrorCode errorCode1 = SQLiteLog._sql.Shutdown();
          if (errorCode1 != SQLiteErrorCode.Ok)
            throw new SQLiteException(errorCode1, "Failed to shutdown interface.");
          SQLiteErrorCode errorCode2 = SQLiteLog._sql.SetLogCallback((SQLiteLogCallback) null);
          if (errorCode2 != SQLiteErrorCode.Ok)
            throw new SQLiteException(errorCode2, "Failed to shutdown logging.");
        }
        if (SQLiteLog._callback != null)
          SQLiteLog._callback = (SQLiteLogCallback) null;
        if (SQLiteLog._domainUnload == null)
          return;
        AppDomain.CurrentDomain.DomainUnload -= SQLiteLog._domainUnload;
        SQLiteLog._domainUnload = (EventHandler) null;
      }
    }

    /// <summary>
    /// This event is raised whenever SQLite raises a logging event.
    /// Note that this should be set as one of the first things in the
    /// application.
    /// </summary>
    public static event SQLiteLogEventHandler Log
    {
      add
      {
        lock (SQLiteLog.syncRoot)
        {
          SQLiteLog._handlers -= value;
          SQLiteLog._handlers += value;
        }
      }
      remove
      {
        lock (SQLiteLog.syncRoot)
          SQLiteLog._handlers -= value;
      }
    }

    /// <summary>
    /// If this property is true, logging is enabled; otherwise, logging is
    /// disabled.  When logging is disabled, no logging events will fire.
    /// </summary>
    public static bool Enabled
    {
      get
      {
        lock (SQLiteLog.syncRoot)
          return SQLiteLog._enabled;
      }
      set
      {
        lock (SQLiteLog.syncRoot)
          SQLiteLog._enabled = value;
      }
    }

    /// <summary>
    /// Log a message to all the registered log event handlers without going
    /// through the SQLite library.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    public static void LogMessage(string message) => SQLiteLog.LogMessage((object) null, message);

    /// <summary>
    /// Log a message to all the registered log event handlers without going
    /// through the SQLite library.
    /// </summary>
    /// <param name="errorCode">The SQLite error code.</param>
    /// <param name="message">The message to be logged.</param>
    public static void LogMessage(SQLiteErrorCode errorCode, string message) => SQLiteLog.LogMessage((object) errorCode, message);

    /// <summary>
    /// Log a message to all the registered log event handlers without going
    /// through the SQLite library.
    /// </summary>
    /// <param name="errorCode">The integer error code.</param>
    /// <param name="message">The message to be logged.</param>
    public static void LogMessage(int errorCode, string message) => SQLiteLog.LogMessage((object) errorCode, message);

    /// <summary>
    /// Log a message to all the registered log event handlers without going
    /// through the SQLite library.
    /// </summary>
    /// <param name="errorCode">
    /// The error code.  The type of this object value should be
    /// System.Int32 or SQLiteErrorCode.
    /// </param>
    /// <param name="message">The message to be logged.</param>
    private static void LogMessage(object errorCode, string message)
    {
      bool enabled;
      SQLiteLogEventHandler liteLogEventHandler;
      lock (SQLiteLog.syncRoot)
      {
        enabled = SQLiteLog._enabled;
        liteLogEventHandler = SQLiteLog._handlers == null ? (SQLiteLogEventHandler) null : SQLiteLog._handlers.Clone() as SQLiteLogEventHandler;
      }
      if (!enabled || liteLogEventHandler == null)
        return;
      liteLogEventHandler((object) null, new LogEventArgs(IntPtr.Zero, errorCode, message, (object) null));
    }

    /// <summary>
    /// Creates and initializes the default log event handler.
    /// </summary>
    private static void InitializeDefaultHandler()
    {
      lock (SQLiteLog.syncRoot)
      {
        if (SQLiteLog._defaultHandler != null)
          return;
        SQLiteLog._defaultHandler = new SQLiteLogEventHandler(SQLiteLog.LogEventHandler);
      }
    }

    /// <summary>
    /// Adds the default log event handler to the list of handlers.
    /// </summary>
    public static void AddDefaultHandler()
    {
      SQLiteLog.InitializeDefaultHandler();
      SQLiteLog.Log += SQLiteLog._defaultHandler;
    }

    /// <summary>
    /// Removes the default log event handler from the list of handlers.
    /// </summary>
    public static void RemoveDefaultHandler()
    {
      SQLiteLog.InitializeDefaultHandler();
      SQLiteLog.Log -= SQLiteLog._defaultHandler;
    }

    /// <summary>
    /// Internal proxy function that calls any registered application log
    /// event handlers.
    /// 
    /// WARNING: This method is used more-or-less directly by native code,
    ///          do not modify its type signature.
    /// </summary>
    /// <param name="pUserData">
    /// The extra data associated with this message, if any.
    /// </param>
    /// <param name="errorCode">
    /// The error code associated with this message.
    /// </param>
    /// <param name="pMessage">The message string to be logged.</param>
    private static void LogCallback(IntPtr pUserData, int errorCode, IntPtr pMessage)
    {
      bool enabled;
      SQLiteLogEventHandler liteLogEventHandler;
      lock (SQLiteLog.syncRoot)
      {
        enabled = SQLiteLog._enabled;
        liteLogEventHandler = SQLiteLog._handlers == null ? (SQLiteLogEventHandler) null : SQLiteLog._handlers.Clone() as SQLiteLogEventHandler;
      }
      if (!enabled || liteLogEventHandler == null)
        return;
      liteLogEventHandler((object) null, new LogEventArgs(pUserData, (object) errorCode, SQLiteConvert.UTF8ToString(pMessage, -1), (object) null));
    }

    /// <summary>
    /// Default logger.  Currently, uses the Trace class (i.e. sends events
    /// to the current trace listeners, if any).
    /// </summary>
    /// <param name="sender">Should be null.</param>
    /// <param name="e">The data associated with this event.</param>
    private static void LogEventHandler(object sender, LogEventArgs e)
    {
      if (e == null)
        return;
      string message = e.Message;
      string str1;
      if (message == null)
      {
        str1 = "<null>";
      }
      else
      {
        str1 = message.Trim();
        if (str1.Length == 0)
          str1 = "<empty>";
      }
      object errorCode = e.ErrorCode;
      string str2 = "error";
      if (errorCode is SQLiteErrorCode || errorCode is int)
      {
        switch ((SQLiteErrorCode) ((int) errorCode & (int) byte.MaxValue))
        {
          case SQLiteErrorCode.Ok:
            str2 = "message";
            break;
          case SQLiteErrorCode.Notice:
            str2 = "notice";
            break;
          case SQLiteErrorCode.Warning:
            str2 = "warning";
            break;
          case SQLiteErrorCode.Row:
          case SQLiteErrorCode.Done:
            str2 = "data";
            break;
        }
      }
      else if (errorCode == null)
        str2 = "trace";
      if (errorCode != null && !object.ReferenceEquals(errorCode, (object) string.Empty))
        Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "SQLite {0} ({1}): {2}", (object) str2, errorCode, (object) str1));
      else
        Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "SQLite {0}: {1}", (object) str2, (object) str1));
    }
  }
}
