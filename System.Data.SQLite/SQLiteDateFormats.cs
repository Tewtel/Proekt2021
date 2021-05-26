// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteDateFormats
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This implementation of SQLite for ADO.NET can process date/time fields in
  /// databases in one of six formats.
  /// </summary>
  /// <remarks>
  /// ISO8601 format is more compatible, readable, fully-processable, but less
  /// accurate as it does not provide time down to fractions of a second.
  /// JulianDay is the numeric format the SQLite uses internally and is arguably
  /// the most compatible with 3rd party tools.  It is not readable as text
  /// without post-processing.  Ticks less compatible with 3rd party tools that
  /// query the database, and renders the DateTime field unreadable as text
  /// without post-processing.  UnixEpoch is more compatible with Unix systems.
  /// InvariantCulture allows the configured format for the invariant culture
  /// format to be used and is human readable.  CurrentCulture allows the
  /// configured format for the current culture to be used and is also human
  /// readable.
  /// 
  /// The preferred order of choosing a DateTime format is JulianDay, ISO8601,
  /// and then Ticks.  Ticks is mainly present for legacy code support.
  /// </remarks>
  public enum SQLiteDateFormats
  {
    /// <summary>
    /// Use the value of DateTime.Ticks.  This value is not recommended and is not well supported with LINQ.
    /// </summary>
    Ticks = 0,
    /// <summary>The default format for this provider.</summary>
    Default = 1,
    /// <summary>
    /// Use the ISO-8601 format.  Uses the "yyyy-MM-dd HH:mm:ss.FFFFFFFK" format for UTC DateTime values and
    /// "yyyy-MM-dd HH:mm:ss.FFFFFFF" format for local DateTime values).
    /// </summary>
    ISO8601 = 1,
    /// <summary>
    /// The interval of time in days and fractions of a day since January 1, 4713 BC.
    /// </summary>
    JulianDay = 2,
    /// <summary>
    /// The whole number of seconds since the Unix epoch (January 1, 1970).
    /// </summary>
    UnixEpoch = 3,
    /// <summary>
    /// Any culture-independent string value that the .NET Framework can interpret as a valid DateTime.
    /// </summary>
    InvariantCulture = 4,
    /// <summary>
    /// Any string value that the .NET Framework can interpret as a valid DateTime using the current culture.
    /// </summary>
    CurrentCulture = 5,
  }
}
