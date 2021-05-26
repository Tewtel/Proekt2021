// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.TransactionContextInitializer`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Internal;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Transactions;

namespace System.Data.Entity.Infrastructure
{
  internal class TransactionContextInitializer<TContext> : IDatabaseInitializer<TContext>
    where TContext : TransactionContext
  {
    public void InitializeDatabase(TContext context)
    {
      EntityConnection connection = (EntityConnection) context.ObjectContext.Connection;
      if (connection.State != ConnectionState.Open)
        return;
      if (connection.CurrentTransaction == null)
        return;
      try
      {
        using (new TransactionScope(TransactionScopeOption.Suppress))
          context.Transactions.AsNoTracking<TransactionRow>().WithExecutionStrategy<TransactionRow>((IDbExecutionStrategy) new DefaultExecutionStrategy()).Count<TransactionRow>();
      }
      catch (EntityException ex)
      {
        DbContextInfo currentInfo = DbContextInfo.CurrentInfo;
        DbContextInfo.CurrentInfo = (DbContextInfo) null;
        try
        {
          IEnumerable<MigrationStatement> migrationStatements = TransactionContextInitializer<TContext>.GenerateMigrationStatements((TransactionContext) context);
          DbMigrator dbMigrator = new DbMigrator(context.InternalContext.MigrationsConfiguration, (DbContext) context, DatabaseExistenceState.Exists, true);
          using (new TransactionScope(TransactionScopeOption.Suppress))
            dbMigrator.ExecuteStatements(migrationStatements, connection.CurrentTransaction.StoreTransaction);
        }
        finally
        {
          DbContextInfo.CurrentInfo = currentInfo;
        }
      }
    }

    internal static IEnumerable<MigrationStatement> GenerateMigrationStatements(
      TransactionContext context)
    {
      if (DbConfiguration.DependencyResolver.GetService<Func<MigrationSqlGenerator>>((object) context.InternalContext.ProviderName) != null)
      {
        MigrationSqlGenerator sqlGenerator = context.InternalContext.MigrationsConfiguration.GetSqlGenerator(context.InternalContext.ProviderName);
        DbConnection connection = context.Database.Connection;
        CreateTableOperation createTableOperation = (CreateTableOperation) new EdmModelDiffer().Diff(new DbModelBuilder().Build(connection).GetModel(), context.GetModel()).Single<MigrationOperation>();
        string str = context.InternalContext.ModelProviderInfo != null ? context.InternalContext.ModelProviderInfo.ProviderManifestToken : DbConfiguration.DependencyResolver.GetService<IManifestTokenResolver>().ResolveManifestToken(connection);
        CreateTableOperation[] createTableOperationArray = new CreateTableOperation[1]
        {
          createTableOperation
        };
        string providerManifestToken = str;
        return sqlGenerator.Generate((IEnumerable<MigrationOperation>) createTableOperationArray, providerManifestToken);
      }
      return (IEnumerable<MigrationStatement>) new MigrationStatement[1]
      {
        new MigrationStatement()
        {
          Sql = ((IObjectContextAdapter) context).ObjectContext.CreateDatabaseScript(),
          SuppressTransaction = true
        }
      };
    }
  }
}
