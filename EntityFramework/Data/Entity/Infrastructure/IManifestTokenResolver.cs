﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.IManifestTokenResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// A service for getting a provider manifest token given a connection.
  /// The <see cref="T:System.Data.Entity.Infrastructure.DefaultManifestTokenResolver" /> class is used by default and makes use of the
  /// underlying provider to get the token which often involves opening the connection.
  /// A different implementation can be used instead by adding an <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver" />
  /// to <see cref="T:System.Data.Entity.DbConfiguration" /> that may use any information in the connection to return
  /// the token. For example, if the connection is known to point to a SQL Server 2008 database then
  /// "2008" can be returned without opening the connection.
  /// </summary>
  public interface IManifestTokenResolver
  {
    /// <summary>
    /// Returns the manifest token to use for the given connection.
    /// </summary>
    /// <param name="connection"> The connection for which a manifest token is required. </param>
    /// <returns> The manifest token to use. </returns>
    string ResolveManifestToken(DbConnection connection);
  }
}
