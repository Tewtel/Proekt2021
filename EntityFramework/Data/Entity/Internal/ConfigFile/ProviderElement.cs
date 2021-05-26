// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ConfigFile.ProviderElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Configuration;

namespace System.Data.Entity.Internal.ConfigFile
{
  internal class ProviderElement : ConfigurationElement
  {
    private const string InvariantNameKey = "invariantName";
    private const string TypeKey = "type";

    [ConfigurationProperty("invariantName", IsRequired = true)]
    public string InvariantName
    {
      get => (string) this["invariantName"];
      set => this["invariantName"] = (object) value;
    }

    [ConfigurationProperty("type", IsRequired = true)]
    public string ProviderTypeName
    {
      get => (string) this["type"];
      set => this["type"] = (object) value;
    }
  }
}
