﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// Represents contextual information associated with calls with return type <typeparamref name="TResult" />.
  /// </summary>
  /// <typeparam name="TResult">The return type of the target method.</typeparam>
  public abstract class MutableInterceptionContext<TResult> : 
    DbInterceptionContext,
    IDbMutableInterceptionContext<TResult>,
    IDbMutableInterceptionContext
  {
    private readonly InterceptionContextMutableData<TResult> _mutableData = new InterceptionContextMutableData<TResult>();

    /// <summary>
    /// Constructs a new <see cref="T:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1" /> with no state.
    /// </summary>
    protected MutableInterceptionContext()
    {
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1" /> by copying immutable state from the given
    /// interception context. Also see <see cref="M:System.Data.Entity.Infrastructure.Interception.DbInterceptionContext.Clone" />
    /// </summary>
    /// <param name="copyFrom">The context from which to copy state.</param>
    protected MutableInterceptionContext(DbInterceptionContext copyFrom)
      : base(copyFrom)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInterceptionContext>(copyFrom, nameof (copyFrom));
    }

    InterceptionContextMutableData<TResult> IDbMutableInterceptionContext<TResult>.MutableData => this._mutableData;

    InterceptionContextMutableData IDbMutableInterceptionContext.MutableData => (InterceptionContextMutableData) this._mutableData;

    /// <summary>
    /// If execution of the operation completes without throwing, then this property will contain
    /// the result of the operation. If the operation was suppressed or did not fail, then this property
    /// will always contain the default value for the generic type.
    /// </summary>
    /// <remarks>
    /// When an operation completes without throwing both this property and the <see cref="P:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.Result" />
    /// property are set. However, the <see cref="P:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.Result" /> property can be set or changed by interceptors,
    /// while this property will always represent the actual result returned by the operation, if any.
    /// </remarks>
    public TResult OriginalResult => this._mutableData.OriginalResult;

    /// <summary>
    /// If this property is set before the operation has executed, then execution of the operation will
    /// be suppressed and the set result will be returned instead. Otherwise, if the operation succeeds, then
    /// this property will be set to the returned result. In either case, interceptors that run
    /// after the operation can change this property to change the result that will be returned.
    /// </summary>
    /// <remarks>
    /// When an operation completes without throwing both this property and the <see cref="P:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.OriginalResult" />
    /// property are set. However, this property can be set or changed by interceptors, while the
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.OriginalResult" /> property will always represent the actual result returned by the
    /// operation, if any.
    /// </remarks>
    public TResult Result
    {
      get => this._mutableData.Result;
      set => this._mutableData.Result = value;
    }

    /// <summary>
    /// When true, this flag indicates that execution of the operation has been suppressed by
    /// one of the interceptors. This can be done before the operation has executed by calling
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.SuppressExecution" />, by setting an <see cref="P:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.Exception" /> to be thrown, or
    /// by setting the operation result using <see cref="P:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.Result" />.
    /// </summary>
    public bool IsExecutionSuppressed => this._mutableData.IsExecutionSuppressed;

    /// <summary>
    /// Gets or sets a value containing arbitrary user-specified state information associated with the operation.
    /// </summary>
    [Obsolete("Not safe when multiple interceptors are in use. Use SetUserState and FindUserState instead.")]
    public object UserState
    {
      get => this._mutableData.UserState;
      set => this._mutableData.UserState = value;
    }

    /// <summary>
    /// Gets a value containing arbitrary user-specified state information associated with the operation.
    /// </summary>
    /// <param name="key">A key used to identify the user state.</param>
    /// <returns>The user state set, or null if none was found for the given key.</returns>
    public object FindUserState(string key)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(key, nameof (key));
      return this._mutableData.FindUserState(key);
    }

    /// <summary>
    /// Sets a value containing arbitrary user-specified state information associated with the operation.
    /// </summary>
    /// <param name="key">A key used to identify the user state.</param>
    /// <param name="value">The state to set.</param>
    public void SetUserState(string key, object value)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(key, nameof (key));
      this._mutableData.SetUserState(key, value);
    }

    /// <summary>
    /// Prevents the operation from being executed if called before the operation has executed.
    /// </summary>
    /// <exception cref="T:System.InvalidOperationException">
    /// Thrown if this method is called after the operation has already executed.
    /// </exception>
    public void SuppressExecution() => this._mutableData.SuppressExecution();

    /// <summary>
    /// If execution of the operation fails, then this property will contain the exception that was
    /// thrown. If the operation was suppressed or did not fail, then this property will always be null.
    /// </summary>
    /// <remarks>
    /// When an operation fails both this property and the <see cref="P:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.Exception" /> property are set
    /// to the exception that was thrown. However, the <see cref="P:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.Exception" /> property can be set or
    /// changed by interceptors, while this property will always represent the original exception thrown.
    /// </remarks>
    public Exception OriginalException => this._mutableData.OriginalException;

    /// <summary>
    /// If this property is set before the operation has executed, then execution of the operation will
    /// be suppressed and the set exception will be thrown instead. Otherwise, if the operation fails, then
    /// this property will be set to the exception that was thrown. In either case, interceptors that run
    /// after the operation can change this property to change the exception that will be thrown, or set this
    /// property to null to cause no exception to be thrown at all.
    /// </summary>
    /// <remarks>
    /// When an operation fails both this property and the <see cref="P:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.OriginalException" /> property are set
    /// to the exception that was thrown. However, the this property can be set or changed by
    /// interceptors, while the <see cref="P:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1.OriginalException" /> property will always represent
    /// the original exception thrown.
    /// </remarks>
    public Exception Exception
    {
      get => this._mutableData.Exception;
      set => this._mutableData.Exception = value;
    }

    /// <summary>
    /// Set to the status of the <see cref="T:System.Threading.Tasks.Task`1" /> after an async operation has finished. Not used for
    /// synchronous operations.
    /// </summary>
    public TaskStatus TaskStatus => this._mutableData.TaskStatus;

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1" /> that contains all the contextual information in this
    /// interception context together with the <see cref="P:System.Data.Entity.Infrastructure.Interception.DbInterceptionContext.IsAsync" /> flag set to true.
    /// </summary>
    /// <returns>A new interception context associated with the async flag set.</returns>
    public MutableInterceptionContext<TResult> AsAsync() => (MutableInterceptionContext<TResult>) base.AsAsync();

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public MutableInterceptionContext<TResult> WithDbContext(
      DbContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbContext>(context, nameof (context));
      return (MutableInterceptionContext<TResult>) base.WithDbContext(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.MutableInterceptionContext`1" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public MutableInterceptionContext<TResult> WithObjectContext(
      ObjectContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<ObjectContext>(context, nameof (context));
      return (MutableInterceptionContext<TResult>) base.WithObjectContext(context);
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
