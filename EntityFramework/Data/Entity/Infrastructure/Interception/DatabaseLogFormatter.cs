// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Utilities;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// This is the default log formatter used when some <see cref="T:System.Action`1" /> is set onto the <see cref="P:System.Data.Entity.Database.Log" />
  /// property. A different formatter can be used by creating a class that inherits from this class and overrides
  /// some or all methods to change behavior.
  /// </summary>
  /// <remarks>
  /// To set the new formatter create a code-based configuration for EF using <see cref="T:System.Data.Entity.DbConfiguration" /> and then
  /// set the formatter class to use with <see cref="M:System.Data.Entity.DbConfiguration.SetDatabaseLogFormatter(System.Func{System.Data.Entity.DbContext,System.Action{System.String},System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter})" />.
  /// Note that setting the type of formatter to use with this method does change the way command are
  /// logged when <see cref="P:System.Data.Entity.Database.Log" /> is used. It is still necessary to set a <see cref="T:System.Action`1" />
  /// onto <see cref="P:System.Data.Entity.Database.Log" /> before any commands will be logged.
  /// For more low-level control over logging/interception see <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbCommandInterceptor" /> and
  /// <see cref="T:System.Data.Entity.Infrastructure.Interception.DbInterception" />.
  /// Interceptors can also be registered in the config file of the application.
  /// See http://go.microsoft.com/fwlink/?LinkId=260883 for more information about Entity Framework configuration.
  /// </remarks>
  public class DatabaseLogFormatter : 
    IDbCommandInterceptor,
    IDbInterceptor,
    IDbConnectionInterceptor,
    IDbTransactionInterceptor
  {
    private const string StopwatchStateKey = "__LoggingStopwatch__";
    private readonly WeakReference _context;
    private readonly Action<string> _writeAction;
    private readonly Stopwatch _stopwatch = new Stopwatch();

    /// <summary>
    /// Creates a formatter that will not filter by any <see cref="T:System.Data.Entity.DbContext" /> and will instead log every command
    /// from any context and also commands that do not originate from a context.
    /// </summary>
    /// <remarks>
    /// This constructor is not used when a delegate is set on <see cref="P:System.Data.Entity.Database.Log" />. Instead it can be
    /// used by setting the formatter directly using <see cref="M:System.Data.Entity.Infrastructure.Interception.DbInterception.Add(System.Data.Entity.Infrastructure.Interception.IDbInterceptor)" />.
    /// </remarks>
    /// <param name="writeAction">The delegate to which output will be sent.</param>
    public DatabaseLogFormatter(Action<string> writeAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<string>>(writeAction, nameof (writeAction));
      this._writeAction = writeAction;
    }

    /// <summary>
    /// Creates a formatter that will only log commands the come from the given <see cref="T:System.Data.Entity.DbContext" /> instance.
    /// </summary>
    /// <remarks>
    /// This constructor must be called by a class that inherits from this class to override the behavior
    /// of <see cref="P:System.Data.Entity.Database.Log" />.
    /// </remarks>
    /// <param name="context">
    /// The context for which commands should be logged. Pass null to log every command
    /// from any context and also commands that do not originate from a context.
    /// </param>
    /// <param name="writeAction">The delegate to which output will be sent.</param>
    public DatabaseLogFormatter(DbContext context, Action<string> writeAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<string>>(writeAction, nameof (writeAction));
      this._context = new WeakReference((object) context);
      this._writeAction = writeAction;
    }

    /// <summary>
    /// The context for which commands are being logged, or null if commands from all contexts are
    /// being logged.
    /// </summary>
    protected internal DbContext Context => this._context == null || !this._context.IsAlive ? (DbContext) null : (DbContext) this._context.Target;

    internal Action<string> WriteAction => this._writeAction;

    /// <summary>
    /// Writes the given string to the underlying write delegate.
    /// </summary>
    /// <param name="output">The string to write.</param>
    protected virtual void Write(string output) => this._writeAction(output);

    /// <summary>
    /// This property is obsolete. Using it can result in logging incorrect execution times. Call
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.GetStopwatch(System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext)" /> instead.
    /// </summary>
    [Obsolete("This stopwatch can give incorrect times. Use 'GetStopwatch' instead.")]
    protected internal Stopwatch Stopwatch => this._stopwatch;

    /// <summary>
    /// The stopwatch used to time executions. This stopwatch is started at the end of
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.NonQueryExecuting(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{System.Int32})" />, <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.ScalarExecuting(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{System.Object})" />, and <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.ReaderExecuting(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{System.Data.Common.DbDataReader})" />
    /// methods and is stopped at the beginning of the <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.NonQueryExecuted(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{System.Int32})" />, <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.ScalarExecuted(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{System.Object})" />,
    /// and <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.ReaderExecuted(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{System.Data.Common.DbDataReader})" /> methods. If these methods are overridden and the stopwatch is being used
    /// then the overrides should either call the base method or start/stop the stopwatch themselves.
    /// </summary>
    /// <param name="interceptionContext">The interception context for which the stopwatch will be obtained.</param>
    /// <returns>The stopwatch.</returns>
    protected internal Stopwatch GetStopwatch(
      DbCommandInterceptionContext interceptionContext)
    {
      if (this._context != null)
        return this._stopwatch;
      IDbMutableInterceptionContext interceptionContext1 = (IDbMutableInterceptionContext) interceptionContext;
      Stopwatch stopwatch = (Stopwatch) interceptionContext1.MutableData.FindUserState("__LoggingStopwatch__");
      if (stopwatch == null)
      {
        stopwatch = new Stopwatch();
        interceptionContext1.MutableData.SetUserState("__LoggingStopwatch__", (object) stopwatch);
      }
      return stopwatch;
    }

    private void RestartStopwatch(DbCommandInterceptionContext interceptionContext)
    {
      Stopwatch stopwatch = this.GetStopwatch(interceptionContext);
      stopwatch.Restart();
      if (stopwatch == this._stopwatch)
        return;
      this._stopwatch.Restart();
    }

    private void StopStopwatch(DbCommandInterceptionContext interceptionContext)
    {
      Stopwatch stopwatch = this.GetStopwatch(interceptionContext);
      stopwatch.Stop();
      if (stopwatch == this._stopwatch)
        return;
      this._stopwatch.Stop();
    }

    /// <summary>
    /// This method is called before a call to <see cref="M:System.Data.Common.DbCommand.ExecuteNonQuery" /> or
    /// one of its async counterparts is made.
    /// The default implementation calls <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Executing``1(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{``0})" /> and starts the stopwatch returned from
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.GetStopwatch(System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext)" />.
    /// </summary>
    /// <param name="command">The command being executed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void NonQueryExecuting(
      DbCommand command,
      DbCommandInterceptionContext<int> interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<int>>(interceptionContext, nameof (interceptionContext));
      this.Executing<int>(command, interceptionContext);
      this.RestartStopwatch((DbCommandInterceptionContext) interceptionContext);
    }

    /// <summary>
    /// This method is called after a call to <see cref="M:System.Data.Common.DbCommand.ExecuteNonQuery" /> or
    /// one of its async counterparts is made.
    /// The default implementation stopsthe stopwatch returned from <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.GetStopwatch(System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext)" /> and calls
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Executed``1(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{``0})" />.
    /// </summary>
    /// <param name="command">The command being executed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void NonQueryExecuted(
      DbCommand command,
      DbCommandInterceptionContext<int> interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<int>>(interceptionContext, nameof (interceptionContext));
      this.StopStopwatch((DbCommandInterceptionContext) interceptionContext);
      this.Executed<int>(command, interceptionContext);
    }

    /// <summary>
    /// This method is called before a call to <see cref="M:System.Data.Common.DbCommand.ExecuteReader(System.Data.CommandBehavior)" /> or
    /// one of its async counterparts is made.
    /// The default implementation calls <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Executing``1(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{``0})" /> and starts the stopwatch returned from
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.GetStopwatch(System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext)" />.
    /// </summary>
    /// <param name="command">The command being executed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ReaderExecuting(
      DbCommand command,
      DbCommandInterceptionContext<DbDataReader> interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<DbDataReader>>(interceptionContext, nameof (interceptionContext));
      this.Executing<DbDataReader>(command, interceptionContext);
      this.RestartStopwatch((DbCommandInterceptionContext) interceptionContext);
    }

    /// <summary>
    /// This method is called after a call to <see cref="M:System.Data.Common.DbCommand.ExecuteReader(System.Data.CommandBehavior)" /> or
    /// one of its async counterparts is made.
    /// The default implementation stopsthe stopwatch returned from <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.GetStopwatch(System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext)" /> and calls
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Executed``1(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{``0})" />.
    /// </summary>
    /// <param name="command">The command being executed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ReaderExecuted(
      DbCommand command,
      DbCommandInterceptionContext<DbDataReader> interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<DbDataReader>>(interceptionContext, nameof (interceptionContext));
      this.StopStopwatch((DbCommandInterceptionContext) interceptionContext);
      this.Executed<DbDataReader>(command, interceptionContext);
    }

    /// <summary>
    /// This method is called before a call to <see cref="M:System.Data.Common.DbCommand.ExecuteScalar" />  or
    /// one of its async counterparts is made.
    /// The default implementation calls <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Executing``1(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{``0})" /> and starts the stopwatch returned from
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.GetStopwatch(System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext)" />.
    /// </summary>
    /// <param name="command">The command being executed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ScalarExecuting(
      DbCommand command,
      DbCommandInterceptionContext<object> interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<object>>(interceptionContext, nameof (interceptionContext));
      this.Executing<object>(command, interceptionContext);
      this.RestartStopwatch((DbCommandInterceptionContext) interceptionContext);
    }

    /// <summary>
    /// This method is called after a call to <see cref="M:System.Data.Common.DbCommand.ExecuteScalar" />  or
    /// one of its async counterparts is made.
    /// The default implementation stopsthe stopwatch returned from <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.GetStopwatch(System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext)" /> and calls
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Executed``1(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{``0})" />.
    /// </summary>
    /// <param name="command">The command being executed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ScalarExecuted(
      DbCommand command,
      DbCommandInterceptionContext<object> interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<object>>(interceptionContext, nameof (interceptionContext));
      this.StopStopwatch((DbCommandInterceptionContext) interceptionContext);
      this.Executed<object>(command, interceptionContext);
    }

    /// <summary>
    /// Called whenever a command is about to be executed. The default implementation of this method
    /// filters by <see cref="T:System.Data.Entity.DbContext" /> set into <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Context" />, if any, and then calls
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.LogCommand``1(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{``0})" />. This method would typically only be overridden to change the
    /// context filtering behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the operation's results.</typeparam>
    /// <param name="command">The command that will be executed.</param>
    /// <param name="interceptionContext">Contextual information associated with the command.</param>
    public virtual void Executing<TResult>(
      DbCommand command,
      DbCommandInterceptionContext<TResult> interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<TResult>>(interceptionContext, nameof (interceptionContext));
      if (this.Context != null && !interceptionContext.DbContexts.Contains<DbContext>(this.Context, new Func<DbContext, DbContext, bool>(object.ReferenceEquals)))
        return;
      this.LogCommand<TResult>(command, interceptionContext);
    }

    /// <summary>
    /// Called whenever a command has completed executing. The default implementation of this method
    /// filters by <see cref="T:System.Data.Entity.DbContext" /> set into <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Context" />, if any, and then calls
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.LogResult``1(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{``0})" />.  This method would typically only be overridden to change the context
    /// filtering behavior.
    /// </summary>
    /// <typeparam name="TResult">The type of the operation's results.</typeparam>
    /// <param name="command">The command that was executed.</param>
    /// <param name="interceptionContext">Contextual information associated with the command.</param>
    public virtual void Executed<TResult>(
      DbCommand command,
      DbCommandInterceptionContext<TResult> interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<TResult>>(interceptionContext, nameof (interceptionContext));
      if (this.Context != null && !interceptionContext.DbContexts.Contains<DbContext>(this.Context, new Func<DbContext, DbContext, bool>(object.ReferenceEquals)))
        return;
      this.LogResult<TResult>(command, interceptionContext);
    }

    /// <summary>
    /// Called to log a command that is about to be executed. Override this method to change how the
    /// command is logged to <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.WriteAction" />.
    /// </summary>
    /// <typeparam name="TResult">The type of the operation's results.</typeparam>
    /// <param name="command">The command to be logged.</param>
    /// <param name="interceptionContext">Contextual information associated with the command.</param>
    public virtual void LogCommand<TResult>(
      DbCommand command,
      DbCommandInterceptionContext<TResult> interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<TResult>>(interceptionContext, nameof (interceptionContext));
      string output = command.CommandText ?? "<null>";
      if (output.EndsWith(Environment.NewLine, StringComparison.Ordinal))
      {
        this.Write(output);
      }
      else
      {
        this.Write(output);
        this.Write(Environment.NewLine);
      }
      if (command.Parameters != null)
      {
        foreach (DbParameter parameter in command.Parameters.OfType<DbParameter>())
          this.LogParameter<TResult>(command, interceptionContext, parameter);
      }
      this.Write(interceptionContext.IsAsync ? System.Data.Entity.Resources.Strings.CommandLogAsync((object) DateTimeOffset.Now, (object) Environment.NewLine) : System.Data.Entity.Resources.Strings.CommandLogNonAsync((object) DateTimeOffset.Now, (object) Environment.NewLine));
    }

    /// <summary>
    /// Called by <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.LogCommand``1(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{``0})" /> to log each parameter. This method can be called from an overridden
    /// implementation of <see cref="M:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.LogCommand``1(System.Data.Common.DbCommand,System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext{``0})" /> to log parameters, and/or can be overridden to
    /// change the way that parameters are logged to <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.WriteAction" />.
    /// </summary>
    /// <typeparam name="TResult">The type of the operation's results.</typeparam>
    /// <param name="command">The command being logged.</param>
    /// <param name="interceptionContext">Contextual information associated with the command.</param>
    /// <param name="parameter">The parameter to log.</param>
    public virtual void LogParameter<TResult>(
      DbCommand command,
      DbCommandInterceptionContext<TResult> interceptionContext,
      DbParameter parameter)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<TResult>>(interceptionContext, nameof (interceptionContext));
      System.Data.Entity.Utilities.Check.NotNull<DbParameter>(parameter, nameof (parameter));
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("-- ").Append(parameter.ParameterName).Append(": '").Append(parameter.Value == null || parameter.Value == DBNull.Value ? (object) "null" : parameter.Value).Append("' (Type = ").Append((object) parameter.DbType);
      if (parameter.Direction != ParameterDirection.Input)
        stringBuilder.Append(", Direction = ").Append((object) parameter.Direction);
      if (!parameter.IsNullable)
        stringBuilder.Append(", IsNullable = false");
      if (parameter.Size != 0)
        stringBuilder.Append(", Size = ").Append(parameter.Size);
      if (((IDbDataParameter) parameter).Precision != (byte) 0)
        stringBuilder.Append(", Precision = ").Append(((IDbDataParameter) parameter).Precision);
      if (((IDbDataParameter) parameter).Scale != (byte) 0)
        stringBuilder.Append(", Scale = ").Append(((IDbDataParameter) parameter).Scale);
      stringBuilder.Append(")").Append(Environment.NewLine);
      this.Write(stringBuilder.ToString());
    }

    /// <summary>
    /// Called to log the result of executing a command. Override this method to change how results are
    /// logged to <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.WriteAction" />.
    /// </summary>
    /// <typeparam name="TResult">The type of the operation's results.</typeparam>
    /// <param name="command">The command being logged.</param>
    /// <param name="interceptionContext">Contextual information associated with the command.</param>
    public virtual void LogResult<TResult>(
      DbCommand command,
      DbCommandInterceptionContext<TResult> interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCommand>(command, nameof (command));
      System.Data.Entity.Utilities.Check.NotNull<DbCommandInterceptionContext<TResult>>(interceptionContext, nameof (interceptionContext));
      Stopwatch stopwatch = this._stopwatch;
      if (this._context == null)
      {
        Stopwatch userState = (Stopwatch) ((IDbMutableInterceptionContext) interceptionContext).MutableData.FindUserState("__LoggingStopwatch__");
        if (userState != null)
          stopwatch = userState;
      }
      if (interceptionContext.Exception != null)
        this.Write(System.Data.Entity.Resources.Strings.CommandLogFailed((object) stopwatch.ElapsedMilliseconds, (object) interceptionContext.Exception.Message, (object) Environment.NewLine));
      else if (interceptionContext.TaskStatus.HasFlag((Enum) TaskStatus.Canceled))
      {
        this.Write(System.Data.Entity.Resources.Strings.CommandLogCanceled((object) stopwatch.ElapsedMilliseconds, (object) Environment.NewLine));
      }
      else
      {
        TResult result = interceptionContext.Result;
        string str = (object) result == null ? "null" : ((object) result is DbDataReader ? result.GetType().Name : result.ToString());
        this.Write(System.Data.Entity.Resources.Strings.CommandLogComplete((object) stopwatch.ElapsedMilliseconds, (object) str, (object) Environment.NewLine));
      }
      this.Write(Environment.NewLine);
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection beginning the transaction.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void BeginningTransaction(
      DbConnection connection,
      BeginTransactionInterceptionContext interceptionContext)
    {
    }

    /// <summary>
    /// Called after <see cref="M:System.Data.Common.DbConnection.BeginTransaction(System.Data.IsolationLevel)" /> is invoked.
    /// The default implementation of this method filters by <see cref="T:System.Data.Entity.DbContext" /> set into
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Context" />, if any, and then logs the event.
    /// </summary>
    /// <param name="connection">The connection that began the transaction.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void BeganTransaction(
      DbConnection connection,
      BeginTransactionInterceptionContext interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<BeginTransactionInterceptionContext>(interceptionContext, nameof (interceptionContext));
      if (this.Context != null && !interceptionContext.DbContexts.Contains<DbContext>(this.Context, new Func<DbContext, DbContext, bool>(object.ReferenceEquals)))
        return;
      if (interceptionContext.Exception != null)
        this.Write(System.Data.Entity.Resources.Strings.TransactionStartErrorLog((object) DateTimeOffset.Now, (object) interceptionContext.Exception.Message, (object) Environment.NewLine));
      else
        this.Write(System.Data.Entity.Resources.Strings.TransactionStartedLog((object) DateTimeOffset.Now, (object) Environment.NewLine));
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void EnlistingTransaction(
      DbConnection connection,
      EnlistTransactionInterceptionContext interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void EnlistedTransaction(
      DbConnection connection,
      EnlistTransactionInterceptionContext interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection being opened.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void Opening(
      DbConnection connection,
      DbConnectionInterceptionContext interceptionContext)
    {
    }

    /// <summary>
    /// Called after <see cref="M:System.Data.Common.DbConnection.Open" /> or its async counterpart is invoked.
    /// The default implementation of this method filters by <see cref="T:System.Data.Entity.DbContext" /> set into
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Context" />, if any, and then logs the event.
    /// </summary>
    /// <param name="connection">The connection that was opened.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void Opened(
      DbConnection connection,
      DbConnectionInterceptionContext interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<DbConnectionInterceptionContext>(interceptionContext, nameof (interceptionContext));
      if (this.Context != null && !interceptionContext.DbContexts.Contains<DbContext>(this.Context, new Func<DbContext, DbContext, bool>(object.ReferenceEquals)))
        return;
      if (interceptionContext.Exception != null)
        this.Write(interceptionContext.IsAsync ? System.Data.Entity.Resources.Strings.ConnectionOpenErrorLogAsync((object) DateTimeOffset.Now, (object) interceptionContext.Exception.Message, (object) Environment.NewLine) : System.Data.Entity.Resources.Strings.ConnectionOpenErrorLog((object) DateTimeOffset.Now, (object) interceptionContext.Exception.Message, (object) Environment.NewLine));
      else if (interceptionContext.TaskStatus.HasFlag((Enum) TaskStatus.Canceled))
        this.Write(System.Data.Entity.Resources.Strings.ConnectionOpenCanceledLog((object) DateTimeOffset.Now, (object) Environment.NewLine));
      else
        this.Write(interceptionContext.IsAsync ? System.Data.Entity.Resources.Strings.ConnectionOpenedLogAsync((object) DateTimeOffset.Now, (object) Environment.NewLine) : System.Data.Entity.Resources.Strings.ConnectionOpenedLog((object) DateTimeOffset.Now, (object) Environment.NewLine));
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection being closed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void Closing(
      DbConnection connection,
      DbConnectionInterceptionContext interceptionContext)
    {
    }

    /// <summary>
    /// Called after <see cref="M:System.Data.Common.DbConnection.Close" /> is invoked.
    /// The default implementation of this method filters by <see cref="T:System.Data.Entity.DbContext" /> set into
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Context" />, if any, and then logs the event.
    /// </summary>
    /// <param name="connection">The connection that was closed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void Closed(
      DbConnection connection,
      DbConnectionInterceptionContext interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<DbConnectionInterceptionContext>(interceptionContext, nameof (interceptionContext));
      if (this.Context != null && !interceptionContext.DbContexts.Contains<DbContext>(this.Context, new Func<DbContext, DbContext, bool>(object.ReferenceEquals)))
        return;
      if (interceptionContext.Exception != null)
        this.Write(System.Data.Entity.Resources.Strings.ConnectionCloseErrorLog((object) DateTimeOffset.Now, (object) interceptionContext.Exception.Message, (object) Environment.NewLine));
      else
        this.Write(System.Data.Entity.Resources.Strings.ConnectionClosedLog((object) DateTimeOffset.Now, (object) Environment.NewLine));
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ConnectionStringGetting(
      DbConnection connection,
      DbConnectionInterceptionContext<string> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ConnectionStringGot(
      DbConnection connection,
      DbConnectionInterceptionContext<string> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ConnectionStringSetting(
      DbConnection connection,
      DbConnectionPropertyInterceptionContext<string> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ConnectionStringSet(
      DbConnection connection,
      DbConnectionPropertyInterceptionContext<string> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ConnectionTimeoutGetting(
      DbConnection connection,
      DbConnectionInterceptionContext<int> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ConnectionTimeoutGot(
      DbConnection connection,
      DbConnectionInterceptionContext<int> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void DatabaseGetting(
      DbConnection connection,
      DbConnectionInterceptionContext<string> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void DatabaseGot(
      DbConnection connection,
      DbConnectionInterceptionContext<string> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void DataSourceGetting(
      DbConnection connection,
      DbConnectionInterceptionContext<string> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void DataSourceGot(
      DbConnection connection,
      DbConnectionInterceptionContext<string> interceptionContext)
    {
    }

    /// <summary>
    /// Called before <see cref="M:System.ComponentModel.Component.Dispose" /> is invoked.
    /// The default implementation of this method filters by <see cref="T:System.Data.Entity.DbContext" /> set into
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Context" />, if any, and then logs the event.
    /// </summary>
    /// <param name="connection">The connection being disposed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void Disposing(
      DbConnection connection,
      DbConnectionInterceptionContext interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      System.Data.Entity.Utilities.Check.NotNull<DbConnectionInterceptionContext>(interceptionContext, nameof (interceptionContext));
      if (this.Context != null && !interceptionContext.DbContexts.Contains<DbContext>(this.Context, new Func<DbContext, DbContext, bool>(object.ReferenceEquals)) || connection.State != ConnectionState.Open)
        return;
      this.Write(System.Data.Entity.Resources.Strings.ConnectionDisposedLog((object) DateTimeOffset.Now, (object) Environment.NewLine));
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection that was disposed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void Disposed(
      DbConnection connection,
      DbConnectionInterceptionContext interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ServerVersionGetting(
      DbConnection connection,
      DbConnectionInterceptionContext<string> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ServerVersionGot(
      DbConnection connection,
      DbConnectionInterceptionContext<string> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void StateGetting(
      DbConnection connection,
      DbConnectionInterceptionContext<ConnectionState> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="connection">The connection.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void StateGot(
      DbConnection connection,
      DbConnectionInterceptionContext<ConnectionState> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="transaction">The transaction.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ConnectionGetting(
      DbTransaction transaction,
      DbTransactionInterceptionContext<DbConnection> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="transaction">The transaction.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void ConnectionGot(
      DbTransaction transaction,
      DbTransactionInterceptionContext<DbConnection> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="transaction">The transaction.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void IsolationLevelGetting(
      DbTransaction transaction,
      DbTransactionInterceptionContext<IsolationLevel> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="transaction">The transaction.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void IsolationLevelGot(
      DbTransaction transaction,
      DbTransactionInterceptionContext<IsolationLevel> interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="transaction">The transaction being committed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void Committing(
      DbTransaction transaction,
      DbTransactionInterceptionContext interceptionContext)
    {
    }

    /// <summary>
    /// This method is called after <see cref="M:System.Data.Common.DbTransaction.Commit" /> is invoked.
    /// The default implementation of this method filters by <see cref="T:System.Data.Entity.DbContext" /> set into
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Context" />, if any, and then logs the event.
    /// </summary>
    /// <param name="transaction">The transaction that was committed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void Committed(
      DbTransaction transaction,
      DbTransactionInterceptionContext interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbTransaction>(transaction, nameof (transaction));
      System.Data.Entity.Utilities.Check.NotNull<DbTransactionInterceptionContext>(interceptionContext, nameof (interceptionContext));
      if (this.Context != null && !interceptionContext.DbContexts.Contains<DbContext>(this.Context, new Func<DbContext, DbContext, bool>(object.ReferenceEquals)))
        return;
      if (interceptionContext.Exception != null)
        this.Write(System.Data.Entity.Resources.Strings.TransactionCommitErrorLog((object) DateTimeOffset.Now, (object) interceptionContext.Exception.Message, (object) Environment.NewLine));
      else
        this.Write(System.Data.Entity.Resources.Strings.TransactionCommittedLog((object) DateTimeOffset.Now, (object) Environment.NewLine));
    }

    /// <summary>
    /// This method is called before <see cref="M:System.Data.Common.DbTransaction.Dispose" /> is invoked.
    /// The default implementation of this method filters by <see cref="T:System.Data.Entity.DbContext" /> set into
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Context" />, if any, and then logs the event.
    /// </summary>
    /// <param name="transaction">The transaction being disposed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void Disposing(
      DbTransaction transaction,
      DbTransactionInterceptionContext interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbTransaction>(transaction, nameof (transaction));
      System.Data.Entity.Utilities.Check.NotNull<DbTransactionInterceptionContext>(interceptionContext, nameof (interceptionContext));
      if (this.Context != null && !interceptionContext.DbContexts.Contains<DbContext>(this.Context, new Func<DbContext, DbContext, bool>(object.ReferenceEquals)) || transaction.Connection == null)
        return;
      this.Write(System.Data.Entity.Resources.Strings.TransactionDisposedLog((object) DateTimeOffset.Now, (object) Environment.NewLine));
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="transaction">The transaction that was disposed.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void Disposed(
      DbTransaction transaction,
      DbTransactionInterceptionContext interceptionContext)
    {
    }

    /// <summary>Does not write to log unless overridden.</summary>
    /// <param name="transaction">The transaction being rolled back.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void RollingBack(
      DbTransaction transaction,
      DbTransactionInterceptionContext interceptionContext)
    {
    }

    /// <summary>
    /// This method is called after <see cref="M:System.Data.Common.DbTransaction.Rollback" /> is invoked.
    /// The default implementation of this method filters by <see cref="T:System.Data.Entity.DbContext" /> set into
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DatabaseLogFormatter.Context" />, if any, and then logs the event.
    /// </summary>
    /// <param name="transaction">The transaction that was rolled back.</param>
    /// <param name="interceptionContext">Contextual information associated with the call.</param>
    public virtual void RolledBack(
      DbTransaction transaction,
      DbTransactionInterceptionContext interceptionContext)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbTransaction>(transaction, nameof (transaction));
      System.Data.Entity.Utilities.Check.NotNull<DbTransactionInterceptionContext>(interceptionContext, nameof (interceptionContext));
      if (this.Context != null && !interceptionContext.DbContexts.Contains<DbContext>(this.Context, new Func<DbContext, DbContext, bool>(object.ReferenceEquals)))
        return;
      if (interceptionContext.Exception != null)
        this.Write(System.Data.Entity.Resources.Strings.TransactionRollbackErrorLog((object) DateTimeOffset.Now, (object) interceptionContext.Exception.Message, (object) Environment.NewLine));
      else
        this.Write(System.Data.Entity.Resources.Strings.TransactionRolledBackLog((object) DateTimeOffset.Now, (object) Environment.NewLine));
    }

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
