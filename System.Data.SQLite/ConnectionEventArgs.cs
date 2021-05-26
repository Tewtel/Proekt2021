// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.ConnectionEventArgs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>Event data for connection event handlers.</summary>
  public class ConnectionEventArgs : System.EventArgs
  {
    /// <summary>The type of event being raised.</summary>
    public readonly SQLiteConnectionEventType EventType;
    /// <summary>
    /// The <see cref="T:System.Data.StateChangeEventArgs" /> associated with this event, if any.
    /// </summary>
    public readonly StateChangeEventArgs EventArgs;
    /// <summary>The transaction associated with this event, if any.</summary>
    public readonly IDbTransaction Transaction;
    /// <summary>The command associated with this event, if any.</summary>
    public readonly IDbCommand Command;
    /// <summary>The data reader associated with this event, if any.</summary>
    public readonly IDataReader DataReader;
    /// <summary>
    /// The critical handle associated with this event, if any.
    /// </summary>
    public readonly CriticalHandle CriticalHandle;
    /// <summary>
    /// Command or message text associated with this event, if any.
    /// </summary>
    public readonly string Text;
    /// <summary>Extra data associated with this event, if any.</summary>
    public readonly object Data;

    /// <summary>Constructs the object.</summary>
    /// <param name="eventType">The type of event being raised.</param>
    /// <param name="eventArgs">The base <see cref="F:System.Data.SQLite.ConnectionEventArgs.EventArgs" /> associated
    /// with this event, if any.</param>
    /// <param name="transaction">The transaction associated with this event, if any.</param>
    /// <param name="command">The command associated with this event, if any.</param>
    /// <param name="dataReader">The data reader associated with this event, if any.</param>
    /// <param name="criticalHandle">The critical handle associated with this event, if any.</param>
    /// <param name="text">The command or message text, if any.</param>
    /// <param name="data">The extra data, if any.</param>
    internal ConnectionEventArgs(
      SQLiteConnectionEventType eventType,
      StateChangeEventArgs eventArgs,
      IDbTransaction transaction,
      IDbCommand command,
      IDataReader dataReader,
      CriticalHandle criticalHandle,
      string text,
      object data)
    {
      this.EventType = eventType;
      this.EventArgs = eventArgs;
      this.Transaction = transaction;
      this.Command = command;
      this.DataReader = dataReader;
      this.CriticalHandle = criticalHandle;
      this.Text = text;
      this.Data = data;
    }
  }
}
