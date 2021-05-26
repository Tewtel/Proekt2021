// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.DbProviderServicesExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Utilities
{
  internal static class DbProviderServicesExtensions
  {
    public static string GetProviderManifestTokenChecked(
      this DbProviderServices providerServices,
      DbConnection connection)
    {
      try
      {
        return providerServices.GetProviderManifestToken(connection);
      }
      catch (ProviderIncompatibleException ex)
      {
        throw new ProviderIncompatibleException(Strings.FailedToGetProviderInformation, (Exception) ex);
      }
    }
  }
}
