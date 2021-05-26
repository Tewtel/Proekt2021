// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ConfigFile.ContextElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Configuration;

namespace System.Data.Entity.Internal.ConfigFile
{
  internal class ContextElement : ConfigurationElement
  {
    private const string TypeKey = "type";
    private const string CommandTimeoutKey = "commandTimeout";
    private const string DisableDatabaseInitializationKey = "disableDatabaseInitialization";
    private const string DatabaseInitializerKey = "databaseInitializer";

    [ConfigurationProperty("type", IsRequired = true)]
    public virtual string ContextTypeName
    {
      get => (string) this["type"];
      set => this["type"] = (object) value;
    }

    [ConfigurationProperty("commandTimeout")]
    public virtual int? CommandTimeout
    {
      get => (int?) this["commandTimeout"];
      set => this["commandTimeout"] = (object) value;
    }

    [ConfigurationProperty("disableDatabaseInitialization", DefaultValue = false)]
    public virtual bool IsDatabaseInitializationDisabled
    {
      get => (bool) this["disableDatabaseInitialization"];
      set => this["disableDatabaseInitialization"] = (object) value;
    }

    [ConfigurationProperty("databaseInitializer")]
    public virtual DatabaseInitializerElement DatabaseInitializer
    {
      get => (DatabaseInitializerElement) this["databaseInitializer"];
      set => this["databaseInitializer"] = (object) value;
    }
  }
}
