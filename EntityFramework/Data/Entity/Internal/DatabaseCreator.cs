﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.DatabaseCreator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Sql;

namespace System.Data.Entity.Internal
{
  internal class DatabaseCreator
  {
    private readonly IDbDependencyResolver _resolver;

    public DatabaseCreator()
      : this(DbConfiguration.DependencyResolver)
    {
    }

    public DatabaseCreator(IDbDependencyResolver resolver) => this._resolver = resolver;

    public virtual void CreateDatabase(
      InternalContext internalContext,
      Func<DbMigrationsConfiguration, DbContext, MigratorBase> createMigrator,
      ObjectContext objectContext)
    {
      if (internalContext.CodeFirstModel != null && this._resolver.GetService<Func<MigrationSqlGenerator>>((object) internalContext.ProviderName) != null)
      {
        createMigrator(internalContext.MigrationsConfiguration, internalContext.Owner).Update();
      }
      else
      {
        internalContext.DatabaseOperations.Create(objectContext);
        internalContext.SaveMetadataToDatabase();
      }
      internalContext.MarkDatabaseInitialized();
    }
  }
}
