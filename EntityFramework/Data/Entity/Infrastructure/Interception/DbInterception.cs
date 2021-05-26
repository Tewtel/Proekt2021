// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.DbInterception
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// This is the registration point for <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbInterceptor" /> interceptors. Interceptors
  /// receive notifications when EF performs certain operations such as executing commands against
  /// the database. For example, see <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbCommandInterceptor" />.
  /// </summary>
  public static class DbInterception
  {
    private static readonly Lazy<DbDispatchers> _dispatchers = new Lazy<DbDispatchers>((Func<DbDispatchers>) (() => new DbDispatchers()));

    /// <summary>
    /// Registers a new <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbInterceptor" /> to receive notifications. Note that the interceptor
    /// must implement some interface that extends from <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbInterceptor" /> to be useful.
    /// </summary>
    /// <param name="interceptor">The interceptor to add.</param>
    public static void Add(IDbInterceptor interceptor)
    {
      System.Data.Entity.Utilities.Check.NotNull<IDbInterceptor>(interceptor, nameof (interceptor));
      DbInterception._dispatchers.Value.AddInterceptor(interceptor);
    }

    /// <summary>
    /// Removes a registered <see cref="T:System.Data.Entity.Infrastructure.Interception.IDbInterceptor" /> so that it will no longer receive notifications.
    /// If the given interceptor is not registered, then this is a no-op.
    /// </summary>
    /// <param name="interceptor">The interceptor to remove.</param>
    public static void Remove(IDbInterceptor interceptor)
    {
      System.Data.Entity.Utilities.Check.NotNull<IDbInterceptor>(interceptor, nameof (interceptor));
      DbInterception._dispatchers.Value.RemoveInterceptor(interceptor);
    }

    /// <summary>
    /// This is the entry point for dispatching to interceptors. This is usually only used internally by
    /// Entity Framework but it is provided publicly so that other code can make sure that registered
    /// interceptors are called when operations are performed on behalf of EF. For example, EF providers
    /// a may make use of this when executing commands.
    /// </summary>
    public static DbDispatchers Dispatch => DbInterception._dispatchers.Value;
  }
}
