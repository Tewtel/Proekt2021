// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ConfigFile.ParameterCollection
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Configuration;
using System.Linq;

namespace System.Data.Entity.Internal.ConfigFile
{
  internal class ParameterCollection : ConfigurationElementCollection
  {
    private const string ParameterKey = "parameter";
    private int _nextKey;

    protected override ConfigurationElement CreateNewElement()
    {
      ParameterElement parameterElement = new ParameterElement(this._nextKey);
      ++this._nextKey;
      return (ConfigurationElement) parameterElement;
    }

    protected override object GetElementKey(ConfigurationElement element) => (object) ((ParameterElement) element).Key;

    public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

    protected override string ElementName => "parameter";

    public virtual object[] GetTypedParameterValues() => this.Cast<ParameterElement>().Select<ParameterElement, object>((Func<ParameterElement, object>) (e => e.GetTypedParameterValue())).ToArray<object>();

    internal ParameterElement NewElement()
    {
      ConfigurationElement newElement = this.CreateNewElement();
      this.BaseAdd(newElement);
      return (ParameterElement) newElement;
    }
  }
}
