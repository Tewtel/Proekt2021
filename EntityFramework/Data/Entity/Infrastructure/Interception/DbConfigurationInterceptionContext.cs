﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.DbConfigurationInterceptionContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// Represents contextual information associated with calls into <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbConfigurationInterceptor" />
  /// implementations.
  /// </summary>
  /// <remarks>
  /// Instances of this class are publicly immutable for contextual information. To add
  /// contextual information use one of the With... or As... methods to create a new
  /// interception context containing the new information.
  /// </remarks>
  public class DbConfigurationInterceptionContext : DbInterceptionContext
  {
    /// <summary>
    /// Constructs a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbConfigurationInterceptionContext" /> with no state.
    /// </summary>
    public DbConfigurationInterceptionContext()
    {
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbConfigurationInterceptionContext" /> by copying state from the given
    /// interception context. Also see <see cref="M:System.Data.Entity.Infrastructure.Interception.DbConfigurationInterceptionContext.Clone" />
    /// </summary>
    /// <param name="copyFrom">The context from which to copy state.</param>
    public DbConfigurationInterceptionContext(DbInterceptionContext copyFrom)
      : base(copyFrom)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInterceptionContext>(copyFrom, nameof (copyFrom));
    }

    /// <inheritdoc />
    protected override DbInterceptionContext Clone() => (DbInterceptionContext) new DbConfigurationInterceptionContext((DbInterceptionContext) this);

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbConfigurationInterceptionContext" /> that contains all the contextual information in
    /// this interception context with the addition of the given <see cref="T:System.Data.Entity.DbContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public DbConfigurationInterceptionContext WithDbContext(
      DbContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbContext>(context, nameof (context));
      return (DbConfigurationInterceptionContext) base.WithDbContext(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbConfigurationInterceptionContext" /> that contains all the contextual information in
    /// this interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public DbConfigurationInterceptionContext WithObjectContext(
      ObjectContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<ObjectContext>(context, nameof (context));
      return (DbConfigurationInterceptionContext) base.WithObjectContext(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbConfigurationInterceptionContext" /> that contains all the contextual information in
    /// this interception context the <see cref="P:System.Data.Entity.Infrastructure.Interception.DbInterceptionContext.IsAsync" /> flag set to true.
    /// </summary>
    /// <returns>A new interception context associated with the async flag set.</returns>
    public DbConfigurationInterceptionContext AsAsync() => (DbConfigurationInterceptionContext) base.AsAsync();

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
