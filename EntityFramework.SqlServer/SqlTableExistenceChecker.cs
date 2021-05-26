// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlTableExistenceChecker
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Text;

namespace System.Data.Entity.SqlServer
{
  internal class SqlTableExistenceChecker : TableExistenceChecker
  {
    public override bool AnyModelTableExistsInDatabase(
      ObjectContext context,
      DbConnection connection,
      IEnumerable<EntitySet> modelTables,
      string edmMetadataContextTableName)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (EntitySet modelTable in modelTables)
      {
        stringBuilder.Append("'");
        stringBuilder.Append((string) modelTable.MetadataProperties["Schema"].Value);
        stringBuilder.Append(".");
        stringBuilder.Append(this.GetTableName(modelTable));
        stringBuilder.Append("',");
      }
      stringBuilder.Remove(stringBuilder.Length - 1, 1);
      using (DbCommand command = connection.CreateCommand())
      {
        command.CommandText = "\r\nSELECT Count(*)\r\nFROM INFORMATION_SCHEMA.TABLES AS t\r\nWHERE t.TABLE_SCHEMA + '.' + t.TABLE_NAME IN (" + stringBuilder?.ToString() + ")\r\n    OR t.TABLE_NAME = '" + edmMetadataContextTableName + "'";
        bool flag = true;
        if (DbInterception.Dispatch.Connection.GetState(connection, context.InterceptionContext) == ConnectionState.Open)
        {
          flag = false;
          EntityTransaction currentTransaction = ((EntityConnection) context.Connection).CurrentTransaction;
          if (currentTransaction != null)
            command.Transaction = currentTransaction.StoreTransaction;
        }
        IDbExecutionStrategy executionStrategy = DbProviderServices.GetExecutionStrategy(connection);
        try
        {
          return executionStrategy.Execute<bool>((Func<bool>) (() =>
          {
            if (DbInterception.Dispatch.Connection.GetState(connection, context.InterceptionContext) == ConnectionState.Broken)
              DbInterception.Dispatch.Connection.Close(connection, context.InterceptionContext);
            if (DbInterception.Dispatch.Connection.GetState(connection, context.InterceptionContext) == ConnectionState.Closed)
              DbInterception.Dispatch.Connection.Open(connection, context.InterceptionContext);
            return (int) DbInterception.Dispatch.Command.Scalar(command, new DbCommandInterceptionContext(context.InterceptionContext)) > 0;
          }));
        }
        finally
        {
          if (flag && DbInterception.Dispatch.Connection.GetState(connection, context.InterceptionContext) != ConnectionState.Closed)
            DbInterception.Dispatch.Connection.Close(connection, context.InterceptionContext);
        }
      }
    }
  }
}
