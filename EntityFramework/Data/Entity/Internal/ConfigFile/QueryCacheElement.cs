// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ConfigFile.QueryCacheElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Configuration;

namespace System.Data.Entity.Internal.ConfigFile
{
  internal class QueryCacheElement : ConfigurationElement
  {
    private const string SizeKey = "size";
    private const string CleaningIntervalInSecondsKey = "cleaningIntervalInSeconds";

    [ConfigurationProperty("size")]
    [IntegerValidator(MaxValue = 2147483647, MinValue = 0)]
    public int Size
    {
      get => (int) this["size"];
      set => this["size"] = (object) value;
    }

    [ConfigurationProperty("cleaningIntervalInSeconds")]
    [IntegerValidator(MaxValue = 2147483647, MinValue = 0)]
    public int CleaningIntervalInSeconds
    {
      get => (int) this["cleaningIntervalInSeconds"];
      set => this["cleaningIntervalInSeconds"] = (object) value;
    }
  }
}
