﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.DbDispatchers
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// Provides access to all dispatchers through the <see cref="P:System.Data.Entity.Infrastructure.Interception.DbInterception.Dispatch" /> fluent API.
  /// </summary>
  public class DbDispatchers
  {
    private readonly DbCommandTreeDispatcher _commandTreeDispatcher = new DbCommandTreeDispatcher();
    private readonly DbCommandDispatcher _commandDispatcher = new DbCommandDispatcher();
    private readonly DbTransactionDispatcher _transactionDispatcher = new DbTransactionDispatcher();
    private readonly DbConnectionDispatcher _dbConnectionDispatcher = new DbConnectionDispatcher();
    private readonly DbConfigurationDispatcher _configurationDispatcher = new DbConfigurationDispatcher();
    private readonly CancelableEntityConnectionDispatcher _cancelableEntityConnectionDispatcher = new CancelableEntityConnectionDispatcher();
    private readonly CancelableDbCommandDispatcher _cancelableCommandDispatcher = new CancelableDbCommandDispatcher();

    internal DbDispatchers()
    {
    }

    internal virtual DbCommandTreeDispatcher CommandTree => this._commandTreeDispatcher;

    /// <summary>
    /// Provides methods for dispatching to <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbCommandInterceptor" /> interceptors for
    /// interception of methods on <see cref="T:System.Data.Common.DbCommand" />.
    /// </summary>
    public virtual DbCommandDispatcher Command => this._commandDispatcher;

    /// <summary>
    /// Provides methods for dispatching to <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbCommandInterceptor" /> interceptors for
    /// interception of methods on <see cref="T:System.Data.Common.DbTransaction" />.
    /// </summary>
    public virtual DbTransactionDispatcher Transaction => this._transactionDispatcher;

    /// <summary>
    /// Provides methods for dispatching to <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbCommandInterceptor" /> interceptors for
    /// interception of methods on <see cref="T:System.Data.Common.DbConnection" />.
    /// </summary>
    public virtual DbConnectionDispatcher Connection => this._dbConnectionDispatcher;

    internal virtual DbConfigurationDispatcher Configuration => this._configurationDispatcher;

    internal virtual CancelableEntityConnectionDispatcher CancelableEntityConnection => this._cancelableEntityConnectionDispatcher;

    internal virtual CancelableDbCommandDispatcher CancelableCommand => this._cancelableCommandDispatcher;

    internal virtual void AddInterceptor(IDbInterceptor interceptor)
    {
      this._commandTreeDispatcher.InternalDispatcher.Add(interceptor);
      this._commandDispatcher.InternalDispatcher.Add(interceptor);
      this._transactionDispatcher.InternalDispatcher.Add(interceptor);
      this._dbConnectionDispatcher.InternalDispatcher.Add(interceptor);
      this._cancelableEntityConnectionDispatcher.InternalDispatcher.Add(interceptor);
      this._cancelableCommandDispatcher.InternalDispatcher.Add(interceptor);
      this._configurationDispatcher.InternalDispatcher.Add(interceptor);
    }

    internal virtual void RemoveInterceptor(IDbInterceptor interceptor)
    {
      this._commandTreeDispatcher.InternalDispatcher.Remove(interceptor);
      this._commandDispatcher.InternalDispatcher.Remove(interceptor);
      this._transactionDispatcher.InternalDispatcher.Remove(interceptor);
      this._dbConnectionDispatcher.InternalDispatcher.Remove(interceptor);
      this._cancelableEntityConnectionDispatcher.InternalDispatcher.Remove(interceptor);
      this._cancelableCommandDispatcher.InternalDispatcher.Remove(interceptor);
      this._configurationDispatcher.InternalDispatcher.Remove(interceptor);
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

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the current instance.
    /// </summary>
    /// <returns>The exact runtime type of the current instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
