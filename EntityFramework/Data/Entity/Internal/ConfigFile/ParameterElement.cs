// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ConfigFile.ParameterElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Configuration;
using System.Globalization;

namespace System.Data.Entity.Internal.ConfigFile
{
  internal class ParameterElement : ConfigurationElement
  {
    private const string ValueKey = "value";
    private const string TypeKey = "type";

    public ParameterElement(int key) => this.Key = key;

    internal int Key { get; private set; }

    [ConfigurationProperty("value", IsRequired = true)]
    public string ValueString
    {
      get => (string) this["value"];
      set => this[nameof (value)] = (object) value;
    }

    [ConfigurationProperty("type", DefaultValue = "System.String")]
    public string TypeName
    {
      get => (string) this["type"];
      set => this["type"] = (object) value;
    }

    public object GetTypedParameterValue() => Convert.ChangeType((object) this.ValueString, Type.GetType(this.TypeName, true), (IFormatProvider) CultureInfo.InvariantCulture);
  }
}
