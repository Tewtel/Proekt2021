// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.TraceEventArgs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Passed during an Trace callback, these event arguments contain the UTF-8 rendering of the SQL statement text
  /// </summary>
  public class TraceEventArgs : EventArgs
  {
    /// <summary>
    /// SQL statement text as the statement first begins executing
    /// </summary>
    public readonly string Statement;

    internal TraceEventArgs(string statement) => this.Statement = statement;
  }
}
