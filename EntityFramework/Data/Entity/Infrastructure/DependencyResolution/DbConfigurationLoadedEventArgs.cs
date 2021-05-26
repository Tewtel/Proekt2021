// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.DbConfigurationLoadedEventArgs
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  /// <summary>
  /// Event arguments passed to <see cref="E:System.Data.Entity.DbConfiguration.Loaded" /> event handlers.
  /// </summary>
  public class DbConfigurationLoadedEventArgs : EventArgs
  {
    private readonly InternalConfiguration _internalConfiguration;

    internal DbConfigurationLoadedEventArgs(InternalConfiguration configuration) => this._internalConfiguration = configuration;

    /// <summary>
    /// Returns a snapshot of the <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver" /> that is about to be locked.
    /// Use the GetService methods on this object to get services that have been registered.
    /// </summary>
    public IDbDependencyResolver DependencyResolver => this._internalConfiguration.ResolverSnapshot;

    /// <summary>
    /// Call this method to add a <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver" /> instance to the Chain of
    /// Responsibility of resolvers that are used to resolve dependencies needed by the Entity Framework.
    /// </summary>
    /// <remarks>
    /// Resolvers are asked to resolve dependencies in reverse order from which they are added. This means
    /// that a resolver can be added to override resolution of a dependency that would already have been
    /// resolved in a different way.
    /// The only exception to this is that any dependency registered in the application's config file
    /// will always be used in preference to using a dependency resolver added here, unless the
    /// overrideConfigFile is set to true in which case the resolver added here will also override config
    /// file settings.
    /// </remarks>
    /// <param name="resolver"> The resolver to add. </param>
    /// <param name="overrideConfigFile">If true, then the resolver added will take precedence over settings in the config file.</param>
    public void AddDependencyResolver(IDbDependencyResolver resolver, bool overrideConfigFile)
    {
      System.Data.Entity.Utilities.Check.NotNull<IDbDependencyResolver>(resolver, nameof (resolver));
      this._internalConfiguration.CheckNotLocked(nameof (AddDependencyResolver));
      this._internalConfiguration.AddDependencyResolver(resolver, overrideConfigFile);
    }

    /// <summary>
    /// Call this method to add a <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver" /> instance to the Chain of Responsibility
    /// of resolvers that are used to resolve dependencies needed by the Entity Framework. Unlike the AddDependencyResolver
    /// method, this method puts the resolver at the bottom of the Chain of Responsibility such that it will only
    /// be used to resolve a dependency that could not be resolved by any of the other resolvers.
    /// </summary>
    /// <param name="resolver"> The resolver to add. </param>
    public void AddDefaultResolver(IDbDependencyResolver resolver)
    {
      System.Data.Entity.Utilities.Check.NotNull<IDbDependencyResolver>(resolver, nameof (resolver));
      this._internalConfiguration.CheckNotLocked(nameof (AddDefaultResolver));
      this._internalConfiguration.AddDefaultResolver(resolver);
    }

    /// <summary>
    /// Adds a wrapping resolver to the configuration that is about to be locked. A wrapping
    /// resolver is a resolver that incepts a service would have been returned by the resolver
    /// chain and wraps or replaces it with another service of the same type.
    /// </summary>
    /// <typeparam name="TService">The type of service to wrap or replace.</typeparam>
    /// <param name="serviceInterceptor">A delegate that takes the unwrapped service and key and returns the wrapped or replaced service.</param>
    public void ReplaceService<TService>(
      Func<TService, object, TService> serviceInterceptor)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TService, object, TService>>(serviceInterceptor, nameof (serviceInterceptor));
      this.AddDependencyResolver((IDbDependencyResolver) new WrappingDependencyResolver<TService>(this.DependencyResolver, serviceInterceptor), true);
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
