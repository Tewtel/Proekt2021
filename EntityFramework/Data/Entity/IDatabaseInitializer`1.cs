﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.IDatabaseInitializer`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity
{
  /// <summary>
  /// An implementation of this interface is used to initialize the underlying database when
  /// an instance of a <see cref="T:System.Data.Entity.DbContext" /> derived class is used for the first time.
  /// This initialization can conditionally create the database and/or seed it with data.
  /// The strategy used is set using the static InitializationStrategy property of the
  /// <see cref="T:System.Data.Entity.Database" /> class.
  /// The following implementations are provided: <see cref="T:System.Data.Entity.DropCreateDatabaseIfModelChanges`1" />,
  /// <see cref="T:System.Data.Entity.DropCreateDatabaseAlways`1" />, <see cref="T:System.Data.Entity.CreateDatabaseIfNotExists`1" />.
  /// </summary>
  /// <typeparam name="TContext">The type of the context.</typeparam>
  public interface IDatabaseInitializer<in TContext> where TContext : DbContext
  {
    /// <summary>
    /// Executes the strategy to initialize the database for the given context.
    /// </summary>
    /// <param name="context"> The context. </param>
    void InitializeDatabase(TContext context);
  }
}
