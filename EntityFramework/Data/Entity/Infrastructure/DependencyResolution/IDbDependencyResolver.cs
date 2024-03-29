﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  /// <summary>
  /// This interface is implemented by any object that can resolve a dependency, either directly
  /// or through use of an external container.
  /// </summary>
  /// <remarks>
  /// The public services currently resolved using IDbDependencyResolver are documented here:
  /// http://msdn.microsoft.com/en-us/data/jj680697
  /// </remarks>
  public interface IDbDependencyResolver
  {
    /// <summary>
    /// Attempts to resolve a dependency for a given contract type and optionally a given key.
    /// If the resolver cannot resolve the dependency then it must return null and not throw. This
    /// allows resolvers to be used in a Chain of Responsibility pattern such that multiple resolvers
    /// can be asked to resolve a dependency until one finally does.
    /// </summary>
    /// <param name="type"> The interface or abstract base class that defines the dependency to be resolved. The returned object is expected to be an instance of this type. </param>
    /// <param name="key"> Optionally, the key of the dependency to be resolved. This may be null for dependencies that are not differentiated by key. </param>
    /// <returns> The resolved dependency, which must be an instance of the given contract type, or null if the dependency could not be resolved. </returns>
    object GetService(Type type, object key);

    /// <summary>
    /// Attempts to resolve a dependencies for a given contract type and optionally a given key.
    /// If the resolver cannot resolve the dependency then it must return an empty enumeration and
    /// not throw. This method differs from <see cref="M:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver.GetService(System.Type,System.Object)" /> in that it returns all registered
    /// services for the given type and key combination.
    /// </summary>
    /// <param name="type"> The interface or abstract base class that defines the dependency to be resolved. Every returned object is expected to be an instance of this type. </param>
    /// <param name="key"> Optionally, the key of the dependency to be resolved. This may be null for dependencies that are not differentiated by key. </param>
    /// <returns> All services that resolve the dependency, which must be instances of the given contract type, or an empty enumeration if the dependency could not be resolved. </returns>
    IEnumerable<object> GetServices(Type type, object key);
  }
}
