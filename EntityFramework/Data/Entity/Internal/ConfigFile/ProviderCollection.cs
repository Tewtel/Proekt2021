// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ConfigFile.ProviderCollection
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Configuration;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Internal.ConfigFile
{
  internal class ProviderCollection : ConfigurationElementCollection
  {
    private const string ProviderKey = "provider";

    protected override ConfigurationElement CreateNewElement() => (ConfigurationElement) new ProviderElement();

    protected override object GetElementKey(ConfigurationElement element) => (object) ((ProviderElement) element).InvariantName;

    public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

    protected override string ElementName => "provider";

    protected override void BaseAdd(ConfigurationElement element)
    {
      if (this.ValidateProviderElement(element))
        return;
      base.BaseAdd(element);
    }

    protected override void BaseAdd(int index, ConfigurationElement element)
    {
      if (this.ValidateProviderElement(element))
        return;
      base.BaseAdd(index, element);
    }

    private bool ValidateProviderElement(ConfigurationElement element)
    {
      object elementKey = this.GetElementKey(element);
      ProviderElement providerElement = (ProviderElement) this.BaseGet(elementKey);
      if (providerElement != null && providerElement.ProviderTypeName != ((ProviderElement) element).ProviderTypeName)
        throw new InvalidOperationException(Strings.ProviderInvariantRepeatedInConfig(elementKey));
      return providerElement != null;
    }

    public ProviderElement AddProvider(string invariantName, string providerTypeName)
    {
      ProviderElement newElement = (ProviderElement) this.CreateNewElement();
      base.BaseAdd((ConfigurationElement) newElement);
      newElement.InvariantName = invariantName;
      newElement.ProviderTypeName = providerTypeName;
      return newElement;
    }
  }
}
