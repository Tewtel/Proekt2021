// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DiagnosticsTraceWriter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;
using System.Diagnostics;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Represents a trace writer that writes to the application's <see cref="T:System.Diagnostics.TraceListener" /> instances.
  /// </summary>
  public class DiagnosticsTraceWriter : ITraceWriter
  {
    /// <summary>
    /// Gets the <see cref="T:System.Diagnostics.TraceLevel" /> that will be used to filter the trace messages passed to the writer.
    /// For example a filter level of <see cref="F:System.Diagnostics.TraceLevel.Info" /> will exclude <see cref="F:System.Diagnostics.TraceLevel.Verbose" /> messages and include <see cref="F:System.Diagnostics.TraceLevel.Info" />,
    /// <see cref="F:System.Diagnostics.TraceLevel.Warning" /> and <see cref="F:System.Diagnostics.TraceLevel.Error" /> messages.
    /// </summary>
    /// <value>
    /// The <see cref="T:System.Diagnostics.TraceLevel" /> that will be used to filter the trace messages passed to the writer.
    /// </value>
    public TraceLevel LevelFilter { get; set; }

    private TraceEventType GetTraceEventType(TraceLevel level)
    {
      switch (level)
      {
        case TraceLevel.Error:
          return TraceEventType.Error;
        case TraceLevel.Warning:
          return TraceEventType.Warning;
        case TraceLevel.Info:
          return TraceEventType.Information;
        case TraceLevel.Verbose:
          return TraceEventType.Verbose;
        default:
          throw new ArgumentOutOfRangeException(nameof (level));
      }
    }

    /// <summary>
    /// Writes the specified trace level, message and optional exception.
    /// </summary>
    /// <param name="level">The <see cref="T:System.Diagnostics.TraceLevel" /> at which to write this trace.</param>
    /// <param name="message">The trace message.</param>
    /// <param name="ex">The trace exception. This parameter is optional.</param>
    public void Trace(TraceLevel level, string message, Exception? ex)
    {
      if (level == TraceLevel.Off)
        return;
      TraceEventCache eventCache = new TraceEventCache();
      TraceEventType traceEventType = this.GetTraceEventType(level);
      foreach (TraceListener listener in System.Diagnostics.Trace.Listeners)
      {
        if (!listener.IsThreadSafe)
        {
          lock (listener)
            listener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
        }
        else
          listener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
        if (System.Diagnostics.Trace.AutoFlush)
          listener.Flush();
      }
    }
  }
}
