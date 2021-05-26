// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ConfigFile.DefaultConnectionFactoryElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Configuration;

namespace System.Data.Entity.Internal.ConfigFile
{
  internal class DefaultConnectionFactoryElement : ConfigurationElement
  {
    private const string TypeKey = "type";
    private const string ParametersKey = "parameters";

    [ConfigurationProperty("type", IsRequired = true)]
    public string FactoryTypeName
    {
      get => (string) this["type"];
      set => this["type"] = (object) value;
    }

    [ConfigurationProperty("parameters")]
    public ParameterCollection Parameters => (ParameterCollection) this["parameters"];

    public Type GetFactoryType() => Type.GetType(this.FactoryTypeName, true);
  }
}
