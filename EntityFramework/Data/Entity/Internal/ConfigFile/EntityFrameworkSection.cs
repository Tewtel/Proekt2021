// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Configuration;

namespace System.Data.Entity.Internal.ConfigFile
{
  internal class EntityFrameworkSection : ConfigurationSection
  {
    private const string DefaultConnectionFactoryKey = "defaultConnectionFactory";
    private const string ContextsKey = "contexts";
    private const string ProviderKey = "providers";
    private const string ConfigurationTypeKey = "codeConfigurationType";
    private const string InterceptorsKey = "interceptors";
    private const string QueryCacheKey = "queryCache";

    [ConfigurationProperty("defaultConnectionFactory")]
    public virtual DefaultConnectionFactoryElement DefaultConnectionFactory
    {
      get => (DefaultConnectionFactoryElement) this["defaultConnectionFactory"];
      set => this["defaultConnectionFactory"] = (object) value;
    }

    [ConfigurationProperty("codeConfigurationType")]
    public virtual string ConfigurationTypeName
    {
      get => (string) this["codeConfigurationType"];
      set => this["codeConfigurationType"] = (object) value;
    }

    [ConfigurationProperty("providers")]
    public virtual ProviderCollection Providers => (ProviderCollection) this["providers"];

    [ConfigurationProperty("contexts")]
    public virtual ContextCollection Contexts => (ContextCollection) this["contexts"];

    [ConfigurationProperty("interceptors")]
    public virtual InterceptorsCollection Interceptors => (InterceptorsCollection) this["interceptors"];

    [ConfigurationProperty("queryCache")]
    public virtual QueryCacheElement QueryCache
    {
      get => (QueryCacheElement) this["queryCache"];
      set => this["queryCache"] = (object) value;
    }
  }
}
