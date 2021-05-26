// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.DbConfigurationLoader
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Internal;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class DbConfigurationLoader
  {
    public virtual Type TryLoadFromConfig(AppConfig config)
    {
      string configurationTypeName = config.ConfigurationTypeName;
      if (string.IsNullOrWhiteSpace(configurationTypeName))
        return (Type) null;
      Type type;
      try
      {
        type = Type.GetType(configurationTypeName, true);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(Strings.DbConfigurationTypeNotFound((object) configurationTypeName), ex);
      }
      return typeof (DbConfiguration).IsAssignableFrom(type) ? type : throw new InvalidOperationException(Strings.CreateInstance_BadDbConfigurationType((object) type.ToString(), (object) typeof (DbConfiguration).ToString()));
    }

    public virtual bool AppConfigContainsDbConfigurationType(AppConfig config) => !string.IsNullOrWhiteSpace(config.ConfigurationTypeName);
  }
}
