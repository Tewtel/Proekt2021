// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// Represents contextual information associated with calls to property setters of type <typeparamref name="TValue" />.
  /// </summary>
  /// <remarks>
  /// An instance of this class is passed to the dispatch methods and does not contain mutable information such as
  /// the result of the operation. This mutable information is obtained from the <see cref="T:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1" />
  /// that is passed to the interceptors. Instances of this class are publicly immutable. To add contextual information
  /// use one of the With... or As... methods to create a new interception context containing the new information.
  /// </remarks>
  /// <typeparam name="TValue">The type of the target property.</typeparam>
  public class PropertyInterceptionContext<TValue> : 
    DbInterceptionContext,
    IDbMutableInterceptionContext
  {
    private readonly InterceptionContextMutableData _mutableData = new InterceptionContextMutableData();
    private TValue _value;

    /// <summary>
    /// Constructs a new <see cref="T:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1" /> with no state.
    /// </summary>
    public PropertyInterceptionContext()
    {
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1" /> by copying immutable state from the given
    /// interception context. Also see <see cref="M:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1.Clone" />
    /// </summary>
    /// <param name="copyFrom">The context from which to copy state.</param>
    public PropertyInterceptionContext(DbInterceptionContext copyFrom)
      : base(copyFrom)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInterceptionContext>(copyFrom, nameof (copyFrom));
      if (!(copyFrom is PropertyInterceptionContext<TValue> interceptionContext))
        return;
      this._value = interceptionContext._value;
    }

    InterceptionContextMutableData IDbMutableInterceptionContext.MutableData => this._mutableData;

    /// <summary>
    /// The value that will be assigned to the target property.
    /// </summary>
    public TValue Value => this._value;

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
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.BeginTransactionInterceptionContext" /> that contains all the contextual information in this
    /// interception context together with the given property value.
    /// </summary>
    /// <param name="value">The value that will be assigned to the target property.</param>
    /// <returns>A new interception context associated with the given property value.</returns>
    public PropertyInterceptionContext<TValue> WithValue(TValue value)
    {
      PropertyInterceptionContext<TValue> interceptionContext = this.TypedClone();
      interceptionContext._value = value;
      return interceptionContext;
    }

    private PropertyInterceptionContext<TValue> TypedClone() => (PropertyInterceptionContext<TValue>) this.Clone();

    /// <inheritdoc />
    protected override DbInterceptionContext Clone() => (DbInterceptionContext) new PropertyInterceptionContext<TValue>((DbInterceptionContext) this);

    /// <summary>
    /// When true, this flag indicates that execution of the operation has been suppressed by
    /// one of the interceptors. This can be done before the operation has executed by calling
    /// <see cref="M:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1.SuppressExecution" /> or by setting an <see cref="P:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1.Exception" /> to be thrown
    /// </summary>
    public bool IsExecutionSuppressed => this._mutableData.IsExecutionSuppressed;

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
    /// When an operation fails both this property and the <see cref="P:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1.Exception" /> property are set
    /// to the exception that was thrown. However, the <see cref="P:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1.Exception" /> property can be set or
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
    /// When an operation fails both this property and the <see cref="P:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1.OriginalException" /> property are set
    /// to the exception that was thrown. However, the this property can be set or changed by
    /// interceptors, while the <see cref="P:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1.OriginalException" /> property will always represent
    /// the original exception thrown.
    /// </remarks>
    public Exception Exception
    {
      get => this._mutableData.Exception;
      set => this._mutableData.Exception = value;
    }

    /// <summary>
    /// Set to the status of the <see cref="T:System.Threading.Tasks.Task" /> after an async operation has finished. Not used for
    /// synchronous operations.
    /// </summary>
    public TaskStatus TaskStatus => this._mutableData.TaskStatus;

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1" /> that contains all the contextual information in this
    /// interception context together with the <see cref="P:System.Data.Entity.Infrastructure.Interception.DbInterceptionContext.IsAsync" /> flag set to true.
    /// </summary>
    /// <returns>A new interception context associated with the async flag set.</returns>
    public PropertyInterceptionContext<TValue> AsAsync() => (PropertyInterceptionContext<TValue>) base.AsAsync();

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public PropertyInterceptionContext<TValue> WithDbContext(
      DbContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbContext>(context, nameof (context));
      return (PropertyInterceptionContext<TValue>) base.WithDbContext(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.PropertyInterceptionContext`1" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public PropertyInterceptionContext<TValue> WithObjectContext(
      ObjectContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<ObjectContext>(context, nameof (context));
      return (PropertyInterceptionContext<TValue>) base.WithObjectContext(context);
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
