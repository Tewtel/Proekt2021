// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ConfigFile.ContextCollection
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Configuration;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Internal.ConfigFile
{
  internal class ContextCollection : ConfigurationElementCollection
  {
    private const string ContextKey = "context";

    protected override ConfigurationElement CreateNewElement() => (ConfigurationElement) new ContextElement();

    protected override object GetElementKey(ConfigurationElement element) => (object) ((ContextElement) element).ContextTypeName;

    public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

    protected override string ElementName => "context";

    protected override void BaseAdd(ConfigurationElement element)
    {
      object elementKey = this.GetElementKey(element);
      if (this.BaseGet(elementKey) != null)
        throw Error.ContextConfiguredMultipleTimes(elementKey);
      base.BaseAdd(element);
    }

    protected override void BaseAdd(int index, ConfigurationElement element)
    {
      object elementKey = this.GetElementKey(element);
      if (this.BaseGet(elementKey) != null)
        throw Error.ContextConfiguredMultipleTimes(elementKey);
      base.BaseAdd(index, element);
    }
  }
}
