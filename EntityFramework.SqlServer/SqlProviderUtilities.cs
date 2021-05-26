// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlProviderUtilities
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Data.Common;
using System.Data.Entity.SqlServer.Resources;
using System.Data.SqlClient;

namespace System.Data.Entity.SqlServer
{
  internal class SqlProviderUtilities
  {
    internal static SqlConnection GetRequiredSqlConnection(DbConnection connection) => connection is SqlConnection sqlConnection ? sqlConnection : throw new ArgumentException(Strings.Mapping_Provider_WrongConnectionType((object) typeof (SqlConnection)));
  }
}
