// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.IFormatterLogger
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

namespace System.Net.Http.Formatting
{
  /// <summary>Specifies a callback interface that a formatter can use to log errors while reading.</summary>
  public interface IFormatterLogger
  {
    /// <summary>Logs an error.</summary>
    /// <param name="errorPath">The path to the member for which the error is being logged.</param>
    /// <param name="errorMessage">The error message to be logged.</param>
    void LogError(string errorPath, string errorMessage);

    /// <summary>Logs an error.</summary>
    /// <param name="errorPath">The path to the member for which the error is being logged.</param>
    /// <param name="exception">The error message.</param>
    void LogError(string errorPath, Exception exception);
  }
}
