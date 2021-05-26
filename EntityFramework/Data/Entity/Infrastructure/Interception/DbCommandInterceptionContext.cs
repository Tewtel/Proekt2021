// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// Represents contextual information associated with calls into <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbCommandInterceptor" />
  /// implementations.
  /// </summary>
  /// <remarks>
  /// An instance of this class is passed to the dispatch methods of <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandDispatcher" />
  /// and does not contain mutable information such as the result of the operation. This mutable information
  /// is obtained from the <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext`1" /> that is passed to the interceptors.
  /// Instances of this class are publicly immutable. To add contextual information use one of the
  /// With... or As... methods to create a new interception context containing the new information.
  /// </remarks>
  public class DbCommandInterceptionContext : DbInterceptionContext
  {
    private CommandBehavior _commandBehavior;

    /// <summary>
    /// Constructs a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext" /> with no state.
    /// </summary>
    public DbCommandInterceptionContext()
    {
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext" /> by copying state from the given
    /// interception context. Also see <see cref="M:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext.Clone" />
    /// </summary>
    /// <param name="copyFrom">The context from which to copy state.</param>
    public DbCommandInterceptionContext(DbInterceptionContext copyFrom)
      : base(copyFrom)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInterceptionContext>(copyFrom, nameof (copyFrom));
      if (!(copyFrom is DbCommandInterceptionContext interceptionContext))
        return;
      this._commandBehavior = interceptionContext._commandBehavior;
    }

    /// <summary>
    /// The <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext.CommandBehavior" /> that will be used or has been used to execute the command with a
    /// <see cref="T:System.Data.Common.DbDataReader" />. This property is only used for <see cref="M:System.Data.Common.DbCommand.ExecuteReader(System.Data.CommandBehavior)" />
    /// and its async counterparts.
    /// </summary>
    public CommandBehavior CommandBehavior => this._commandBehavior;

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext" /> that contains all the contextual information in this
    /// interception context together with the given <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext.CommandBehavior" />.
    /// </summary>
    /// <param name="commandBehavior">The command behavior to associate.</param>
    /// <returns>A new interception context associated with the given command behavior.</returns>
    public DbCommandInterceptionContext WithCommandBehavior(
      CommandBehavior commandBehavior)
    {
      DbCommandInterceptionContext interceptionContext = this.TypedClone();
      interceptionContext._commandBehavior = commandBehavior;
      return interceptionContext;
    }

    private DbCommandInterceptionContext TypedClone() => (DbCommandInterceptionContext) this.Clone();

    /// <inheritdoc />
    protected override DbInterceptionContext Clone() => (DbInterceptionContext) new DbCommandInterceptionContext((DbInterceptionContext) this);

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public DbCommandInterceptionContext WithDbContext(DbContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbContext>(context, nameof (context));
      return (DbCommandInterceptionContext) base.WithDbContext(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public DbCommandInterceptionContext WithObjectContext(
      ObjectContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<ObjectContext>(context, nameof (context));
      return (DbCommandInterceptionContext) base.WithObjectContext(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext" /> that contains all the contextual information in this
    /// interception context the <see cref="P:System.Data.Entity.Infrastructure.Interception.DbInterceptionContext.IsAsync" /> flag set to true.
    /// </summary>
    /// <returns>A new interception context associated with the async flag set.</returns>
    public DbCommandInterceptionContext AsAsync() => (DbCommandInterceptionContext) base.AsAsync();

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
