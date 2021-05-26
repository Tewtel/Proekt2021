// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Utilities.DatabaseCreator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Migrations.Utilities
{
  internal class DatabaseCreator
  {
    private readonly int? _commandTimeout;

    public DatabaseCreator(int? commandTimeout) => this._commandTimeout = commandTimeout;

    public virtual bool Exists(DbConnection connection)
    {
      using (EmptyContext emptyContext = new EmptyContext(connection))
      {
        emptyContext.Database.CommandTimeout = this._commandTimeout;
        return ((IObjectContextAdapter) emptyContext).ObjectContext.DatabaseExists();
      }
    }

    public virtual void Create(DbConnection connection)
    {
      using (EmptyContext emptyContext = new EmptyContext(connection))
      {
        emptyContext.Database.CommandTimeout = this._commandTimeout;
        ((IObjectContextAdapter) emptyContext).ObjectContext.CreateDatabase();
      }
    }

    public virtual void Delete(DbConnection connection)
    {
      using (EmptyContext emptyContext = new EmptyContext(connection))
      {
        emptyContext.Database.CommandTimeout = this._commandTimeout;
        ((IObjectContextAdapter) emptyContext).ObjectContext.DeleteDatabase();
      }
    }
  }
}
