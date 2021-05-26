// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlAzureRetriableExceptionDetector
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Data.SqlClient;

namespace System.Data.Entity.SqlServer
{
  internal static class SqlAzureRetriableExceptionDetector
  {
    public static bool ShouldRetryOn(Exception ex)
    {
      switch (ex)
      {
        case SqlException sqlException:
          foreach (SqlError error in sqlException.Errors)
          {
            switch (error.Number)
            {
              case 20:
              case 64:
              case 121:
              case 233:
              case 1205:
              case 10053:
              case 10054:
              case 10060:
              case 10928:
              case 10929:
              case 40197:
              case 40501:
              case 40613:
              case 41301:
              case 41302:
              case 41305:
              case 41325:
              case 41839:
              case 49918:
              case 49919:
              case 49920:
                return true;
              default:
                continue;
            }
          }
          return false;
        case TimeoutException _:
          return true;
        default:
          return false;
      }
    }
  }
}
