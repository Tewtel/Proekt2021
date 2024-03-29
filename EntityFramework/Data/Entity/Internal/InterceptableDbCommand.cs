﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InterceptableDbCommand
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Internal
{
  internal sealed class InterceptableDbCommand : DbCommand
  {
    private readonly DbCommand _command;
    private readonly DbInterceptionContext _interceptionContext;
    private readonly DbDispatchers _dispatchers;

    public InterceptableDbCommand(
      DbCommand command,
      DbInterceptionContext context,
      DbDispatchers dispatchers = null)
    {
      GC.SuppressFinalize((object) this);
      this._command = command;
      this._interceptionContext = context;
      this._dispatchers = dispatchers ?? DbInterception.Dispatch;
    }

    public DbInterceptionContext InterceptionContext => this._interceptionContext;

    public override void Prepare() => this._command.Prepare();

    public override string CommandText
    {
      get => this._command.CommandText;
      set => this._command.CommandText = value;
    }

    public override int CommandTimeout
    {
      get => this._command.CommandTimeout;
      set => this._command.CommandTimeout = value;
    }

    public override CommandType CommandType
    {
      get => this._command.CommandType;
      set => this._command.CommandType = value;
    }

    public override UpdateRowSource UpdatedRowSource
    {
      get => this._command.UpdatedRowSource;
      set => this._command.UpdatedRowSource = value;
    }

    protected override DbConnection DbConnection
    {
      get => this._command.Connection;
      set => this._command.Connection = value;
    }

    protected override DbParameterCollection DbParameterCollection => this._command.Parameters;

    protected override DbTransaction DbTransaction
    {
      get => this._command.Transaction;
      set => this._command.Transaction = value;
    }

    public override bool DesignTimeVisible
    {
      get => this._command.DesignTimeVisible;
      set => this._command.DesignTimeVisible = value;
    }

    public override void Cancel() => this._command.Cancel();

    protected override DbParameter CreateDbParameter() => this._command.CreateParameter();

    public override int ExecuteNonQuery() => !this._dispatchers.CancelableCommand.Executing(this._command, this._interceptionContext) ? 1 : this._dispatchers.Command.NonQuery(this._command, new DbCommandInterceptionContext(this._interceptionContext));

    public override object ExecuteScalar() => !this._dispatchers.CancelableCommand.Executing(this._command, this._interceptionContext) ? (object) null : this._dispatchers.Command.Scalar(this._command, new DbCommandInterceptionContext(this._interceptionContext));

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
      if (!this._dispatchers.CancelableCommand.Executing(this._command, this._interceptionContext))
        return (DbDataReader) new InterceptableDbCommand.NullDataReader();
      DbCommandInterceptionContext interceptionContext = new DbCommandInterceptionContext(this._interceptionContext);
      if (behavior != CommandBehavior.Default)
        interceptionContext = interceptionContext.WithCommandBehavior(behavior);
      return this._dispatchers.Command.Reader(this._command, interceptionContext);
    }

    public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return !this._dispatchers.CancelableCommand.Executing(this._command, this._interceptionContext) ? new Task<int>((Func<int>) (() => 1)) : this._dispatchers.Command.NonQueryAsync(this._command, new DbCommandInterceptionContext(this._interceptionContext), cancellationToken);
    }

    public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      return !this._dispatchers.CancelableCommand.Executing(this._command, this._interceptionContext) ? new Task<object>((Func<object>) (() => (object) null)) : this._dispatchers.Command.ScalarAsync(this._command, new DbCommandInterceptionContext(this._interceptionContext), cancellationToken);
    }

    protected override Task<DbDataReader> ExecuteDbDataReaderAsync(
      CommandBehavior behavior,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      if (!this._dispatchers.CancelableCommand.Executing(this._command, this._interceptionContext))
        return new Task<DbDataReader>((Func<DbDataReader>) (() => (DbDataReader) new InterceptableDbCommand.NullDataReader()));
      DbCommandInterceptionContext interceptionContext = new DbCommandInterceptionContext(this._interceptionContext);
      if (behavior != CommandBehavior.Default)
        interceptionContext = interceptionContext.WithCommandBehavior(behavior);
      return this._dispatchers.Command.ReaderAsync(this._command, interceptionContext, cancellationToken);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this._command != null)
        this._command.Dispose();
      base.Dispose(disposing);
    }

    private class NullDataReader : DbDataReader
    {
      private int _resultCount;
      private int _readCount;

      public override void Close()
      {
      }

      public override bool NextResult() => this._resultCount++ == 0;

      public override bool Read() => this._readCount++ == 0;

      public override bool IsClosed => false;

      public override int FieldCount => 0;

      public override int GetOrdinal(string name) => -1;

      public override object GetValue(int ordinal) => throw new NotImplementedException();

      public override DataTable GetSchemaTable() => throw new NotImplementedException();

      public override int Depth => throw new NotImplementedException();

      public override int RecordsAffected => 0;

      public override bool GetBoolean(int ordinal) => throw new NotImplementedException();

      public override byte GetByte(int ordinal) => throw new NotImplementedException();

      public override long GetBytes(
        int ordinal,
        long dataOffset,
        byte[] buffer,
        int bufferOffset,
        int length)
      {
        throw new NotImplementedException();
      }

      public override char GetChar(int ordinal) => throw new NotImplementedException();

      public override long GetChars(
        int ordinal,
        long dataOffset,
        char[] buffer,
        int bufferOffset,
        int length)
      {
        throw new NotImplementedException();
      }

      public override Guid GetGuid(int ordinal) => throw new NotImplementedException();

      public override short GetInt16(int ordinal) => throw new NotImplementedException();

      public override int GetInt32(int ordinal) => throw new NotImplementedException();

      public override long GetInt64(int ordinal) => throw new NotImplementedException();

      public override DateTime GetDateTime(int ordinal) => throw new NotImplementedException();

      public override string GetString(int ordinal) => throw new NotImplementedException();

      public override Decimal GetDecimal(int ordinal) => throw new NotImplementedException();

      public override double GetDouble(int ordinal) => throw new NotImplementedException();

      public override float GetFloat(int ordinal) => throw new NotImplementedException();

      public override string GetName(int ordinal) => throw new NotImplementedException();

      public override int GetValues(object[] values) => 0;

      public override bool IsDBNull(int ordinal) => true;

      public override object this[int ordinal] => throw new NotImplementedException();

      public override object this[string name] => throw new NotImplementedException();

      public override bool HasRows => throw new NotImplementedException();

      public override string GetDataTypeName(int ordinal) => throw new NotImplementedException();

      public override Type GetFieldType(int ordinal) => throw new NotImplementedException();

      public override IEnumerator GetEnumerator() => throw new NotImplementedException();
    }
  }
}
