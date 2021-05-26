// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.IDbProviderFactoryResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// A service for obtaining the correct <see cref="T:System.Data.Common.DbProviderFactory" /> from a given
  /// <see cref="T:System.Data.Common.DbConnection" />.
  /// </summary>
  /// <remarks>
  /// On .NET 4.5 the provider is publicly accessible from the connection. On .NET 4 the
  /// default implementation of this service uses some heuristics to find the matching
  /// provider. If these fail then a new implementation of this service can be registered
  /// on <see cref="T:System.Data.Entity.DbConfiguration" /> to provide an appropriate resolution.
  /// </remarks>
  public interface IDbProviderFactoryResolver
  {
    /// <summary>
    /// Returns the <see cref="T:System.Data.Common.DbProviderFactory" /> for the given connection.
    /// </summary>
    /// <param name="connection"> The connection. </param>
    /// <returns> The provider factory for the connection. </returns>
    DbProviderFactory ResolveProviderFactory(DbConnection connection);
  }
}
