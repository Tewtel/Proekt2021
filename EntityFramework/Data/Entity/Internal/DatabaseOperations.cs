// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.DatabaseOperations
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Internal
{
  internal class DatabaseOperations
  {
    public virtual bool Create(ObjectContext objectContext)
    {
      objectContext.CreateDatabase();
      return true;
    }

    public virtual bool Exists(
      DbConnection connection,
      int? commandTimeout,
      Lazy<StoreItemCollection> storeItemCollection)
    {
      if (connection.State == ConnectionState.Open)
        return true;
      try
      {
        return DbProviderServices.GetProviderServices(connection).DatabaseExists(connection, commandTimeout, storeItemCollection);
      }
      catch
      {
        try
        {
          connection.Open();
          return true;
        }
        catch (Exception ex)
        {
          return false;
        }
        finally
        {
          connection.Close();
        }
      }
    }

    public virtual void Delete(ObjectContext objectContext) => objectContext.DeleteDatabase();
  }
}
