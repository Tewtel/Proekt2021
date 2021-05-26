// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteDataAdapter
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.ComponentModel;
using System.Data.Common;

namespace System.Data.SQLite
{
  /// <summary>SQLite implementation of DbDataAdapter.</summary>
  [Designer("Microsoft.VSDesigner.Data.VS.SqlDataAdapterDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
  [DefaultEvent("RowUpdated")]
  [ToolboxItem("SQLite.Designer.SQLiteDataAdapterToolboxItem, SQLite.Designer, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139")]
  public sealed class SQLiteDataAdapter : DbDataAdapter
  {
    private bool disposeSelect = true;
    private static object _updatingEventPH = new object();
    private static object _updatedEventPH = new object();
    private bool disposed;

    /// <overloads>
    /// This class is just a shell around the DbDataAdapter.  Nothing from
    /// DbDataAdapter is overridden here, just a few constructors are defined.
    /// </overloads>
    /// <summary>Default constructor.</summary>
    public SQLiteDataAdapter()
    {
    }

    /// <summary>
    /// Constructs a data adapter using the specified select command.
    /// </summary>
    /// <param name="cmd">
    /// The select command to associate with the adapter.
    /// </param>
    public SQLiteDataAdapter(SQLiteCommand cmd)
    {
      this.SelectCommand = cmd;
      this.disposeSelect = false;
    }

    /// <summary>
    /// Constructs a data adapter with the supplied select command text and
    /// associated with the specified connection.
    /// </summary>
    /// <param name="commandText">
    /// The select command text to associate with the data adapter.
    /// </param>
    /// <param name="connection">
    /// The connection to associate with the select command.
    /// </param>
    public SQLiteDataAdapter(string commandText, SQLiteConnection connection) => this.SelectCommand = new SQLiteCommand(commandText, connection);

    /// <summary>
    /// Constructs a data adapter with the specified select command text,
    /// and using the specified database connection string.
    /// </summary>
    /// <param name="commandText">
    /// The select command text to use to construct a select command.
    /// </param>
    /// <param name="connectionString">
    /// A connection string suitable for passing to a new SQLiteConnection,
    /// which is associated with the select command.
    /// </param>
    public SQLiteDataAdapter(string commandText, string connectionString)
      : this(commandText, connectionString, false)
    {
    }

    /// <summary>
    /// Constructs a data adapter with the specified select command text,
    /// and using the specified database connection string.
    /// </summary>
    /// <param name="commandText">
    /// The select command text to use to construct a select command.
    /// </param>
    /// <param name="connectionString">
    /// A connection string suitable for passing to a new SQLiteConnection,
    /// which is associated with the select command.
    /// </param>
    /// <param name="parseViaFramework">
    /// Non-zero to parse the connection string using the built-in (i.e.
    /// framework provided) parser when opening the connection.
    /// </param>
    public SQLiteDataAdapter(string commandText, string connectionString, bool parseViaFramework)
    {
      SQLiteConnection connection = new SQLiteConnection(connectionString, parseViaFramework);
      this.SelectCommand = new SQLiteCommand(commandText, connection);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteDataAdapter).Name);
    }

    /// <summary>
    /// Cleans up resources (native and managed) associated with the current instance.
    /// </summary>
    /// <param name="disposing">
    /// Zero when being disposed via garbage collection; otherwise, non-zero.
    /// </param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (this.disposed || !disposing)
          return;
        if (this.disposeSelect && this.SelectCommand != null)
        {
          this.SelectCommand.Dispose();
          this.SelectCommand = (SQLiteCommand) null;
        }
        if (this.InsertCommand != null)
        {
          this.InsertCommand.Dispose();
          this.InsertCommand = (SQLiteCommand) null;
        }
        if (this.UpdateCommand != null)
        {
          this.UpdateCommand.Dispose();
          this.UpdateCommand = (SQLiteCommand) null;
        }
        if (this.DeleteCommand == null)
          return;
        this.DeleteCommand.Dispose();
        this.DeleteCommand = (SQLiteCommand) null;
      }
      finally
      {
        base.Dispose(disposing);
        this.disposed = true;
      }
    }

    /// <summary>Row updating event handler</summary>
    public event EventHandler<RowUpdatingEventArgs> RowUpdating
    {
      add
      {
        this.CheckDisposed();
        EventHandler<RowUpdatingEventArgs> eventHandler = (EventHandler<RowUpdatingEventArgs>) this.Events[SQLiteDataAdapter._updatingEventPH];
        if (eventHandler != null && value.Target is DbCommandBuilder)
        {
          EventHandler<RowUpdatingEventArgs> builder = (EventHandler<RowUpdatingEventArgs>) SQLiteDataAdapter.FindBuilder((MulticastDelegate) eventHandler);
          if (builder != null)
            this.Events.RemoveHandler(SQLiteDataAdapter._updatingEventPH, (Delegate) builder);
        }
        this.Events.AddHandler(SQLiteDataAdapter._updatingEventPH, (Delegate) value);
      }
      remove
      {
        this.CheckDisposed();
        this.Events.RemoveHandler(SQLiteDataAdapter._updatingEventPH, (Delegate) value);
      }
    }

    internal static Delegate FindBuilder(MulticastDelegate mcd)
    {
      if ((object) mcd != null)
      {
        Delegate[] invocationList = mcd.GetInvocationList();
        for (int index = 0; index < invocationList.Length; ++index)
        {
          if (invocationList[index].Target is DbCommandBuilder)
            return invocationList[index];
        }
      }
      return (Delegate) null;
    }

    /// <summary>Row updated event handler</summary>
    public event EventHandler<RowUpdatedEventArgs> RowUpdated
    {
      add
      {
        this.CheckDisposed();
        this.Events.AddHandler(SQLiteDataAdapter._updatedEventPH, (Delegate) value);
      }
      remove
      {
        this.CheckDisposed();
        this.Events.RemoveHandler(SQLiteDataAdapter._updatedEventPH, (Delegate) value);
      }
    }

    /// <summary>
    /// Raised by the underlying DbDataAdapter when a row is being updated
    /// </summary>
    /// <param name="value">The event's specifics</param>
    protected override void OnRowUpdating(RowUpdatingEventArgs value)
    {
      if (!(this.Events[SQLiteDataAdapter._updatingEventPH] is EventHandler<RowUpdatingEventArgs> eventHandler))
        return;
      eventHandler((object) this, value);
    }

    /// <summary>Raised by DbDataAdapter after a row is updated</summary>
    /// <param name="value">The event's specifics</param>
    protected override void OnRowUpdated(RowUpdatedEventArgs value)
    {
      if (!(this.Events[SQLiteDataAdapter._updatedEventPH] is EventHandler<RowUpdatedEventArgs> eventHandler))
        return;
      eventHandler((object) this, value);
    }

    /// <summary>Gets/sets the select command for this DataAdapter</summary>
    [DefaultValue(null)]
    [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public SQLiteCommand SelectCommand
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteCommand) base.SelectCommand;
      }
      set
      {
        this.CheckDisposed();
        this.SelectCommand = (DbCommand) value;
      }
    }

    /// <summary>Gets/sets the insert command for this DataAdapter</summary>
    [DefaultValue(null)]
    [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public SQLiteCommand InsertCommand
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteCommand) base.InsertCommand;
      }
      set
      {
        this.CheckDisposed();
        this.InsertCommand = (DbCommand) value;
      }
    }

    /// <summary>Gets/sets the update command for this DataAdapter</summary>
    [DefaultValue(null)]
    [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public SQLiteCommand UpdateCommand
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteCommand) base.UpdateCommand;
      }
      set
      {
        this.CheckDisposed();
        this.UpdateCommand = (DbCommand) value;
      }
    }

    /// <summary>Gets/sets the delete command for this DataAdapter</summary>
    [Editor("Microsoft.VSDesigner.Data.Design.DBCommandEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [DefaultValue(null)]
    public SQLiteCommand DeleteCommand
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteCommand) base.DeleteCommand;
      }
      set
      {
        this.CheckDisposed();
        this.DeleteCommand = (DbCommand) value;
      }
    }
  }
}
