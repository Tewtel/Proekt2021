// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// Represents contextual information associated with calls into <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbCommandTreeInterceptor" />
  /// implementations.
  /// </summary>
  /// <remarks>
  /// Instances of this class are publicly immutable for contextual information. To add
  /// contextual information use one of the With... or As... methods to create a new
  /// interception context containing the new information.
  /// </remarks>
  public class DbCommandTreeInterceptionContext : 
    DbInterceptionContext,
    IDbMutableInterceptionContext<DbCommandTree>,
    IDbMutableInterceptionContext
  {
    private readonly InterceptionContextMutableData<DbCommandTree> _mutableData = new InterceptionContextMutableData<DbCommandTree>();

    /// <summary>
    /// Constructs a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext" /> with no state.
    /// </summary>
    public DbCommandTreeInterceptionContext()
    {
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext" /> by copying state from the given
    /// interception context. Also see <see cref="M:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext.Clone" />
    /// </summary>
    /// <param name="copyFrom">The context from which to copy state.</param>
    public DbCommandTreeInterceptionContext(DbInterceptionContext copyFrom)
      : base(copyFrom)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInterceptionContext>(copyFrom, nameof (copyFrom));
    }

    internal InterceptionContextMutableData<DbCommandTree> MutableData => this._mutableData;

    InterceptionContextMutableData<DbCommandTree> IDbMutableInterceptionContext<DbCommandTree>.MutableData => this._mutableData;

    InterceptionContextMutableData IDbMutableInterceptionContext.MutableData => (InterceptionContextMutableData) this._mutableData;

    /// <summary>
    /// The original tree created by Entity Framework. Interceptors can change the
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext.Result" /> property to change the tree that will be used, but the
    /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext.OriginalResult" /> will always be the tree created by Entity Framework.
    /// </summary>
    public DbCommandTree OriginalResult => this._mutableData.OriginalResult;

    /// <summary>
    /// The command tree that will be used by Entity Framework. This starts as the tree contained in the
    /// the <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext.OriginalResult" /> property but can be set by interceptors to change
    /// the tree that will be used by Entity Framework.
    /// </summary>
    public DbCommandTree Result
    {
      get => this._mutableData.Result;
      set => this._mutableData.Result = value;
    }

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

    /// <inheritdoc />
    protected override DbInterceptionContext Clone() => (DbInterceptionContext) new DbCommandTreeInterceptionContext((DbInterceptionContext) this);

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.DbContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public DbCommandTreeInterceptionContext WithDbContext(
      DbContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbContext>(context, nameof (context));
      return (DbCommandTreeInterceptionContext) base.WithDbContext(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public DbCommandTreeInterceptionContext WithObjectContext(
      ObjectContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<ObjectContext>(context, nameof (context));
      return (DbCommandTreeInterceptionContext) base.WithObjectContext(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandTreeInterceptionContext" /> that contains all the contextual information in this
    /// interception context the <see cref="P:System.Data.Entity.Infrastructure.Interception.DbInterceptionContext.IsAsync" /> flag set to true.
    /// </summary>
    /// <returns>A new interception context associated with the async flag set.</returns>
    public DbCommandTreeInterceptionContext AsAsync() => (DbCommandTreeInterceptionContext) base.AsAsync();

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
