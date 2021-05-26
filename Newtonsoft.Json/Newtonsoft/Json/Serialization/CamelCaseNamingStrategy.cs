// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CamelCaseNamingStrategy
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>A camel case naming strategy.</summary>
  public class CamelCaseNamingStrategy : NamingStrategy
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.CamelCaseNamingStrategy" /> class.
    /// </summary>
    /// <param name="processDictionaryKeys">
    /// A flag indicating whether dictionary keys should be processed.
    /// </param>
    /// <param name="overrideSpecifiedNames">
    /// A flag indicating whether explicitly specified property names should be processed,
    /// e.g. a property name customized with a <see cref="T:Newtonsoft.Json.JsonPropertyAttribute" />.
    /// </param>
    public CamelCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
    {
      this.ProcessDictionaryKeys = processDictionaryKeys;
      this.OverrideSpecifiedNames = overrideSpecifiedNames;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.CamelCaseNamingStrategy" /> class.
    /// </summary>
    /// <param name="processDictionaryKeys">
    /// A flag indicating whether dictionary keys should be processed.
    /// </param>
    /// <param name="overrideSpecifiedNames">
    /// A flag indicating whether explicitly specified property names should be processed,
    /// e.g. a property name customized with a <see cref="T:Newtonsoft.Json.JsonPropertyAttribute" />.
    /// </param>
    /// <param name="processExtensionDataNames">
    /// A flag indicating whether extension data names should be processed.
    /// </param>
    public CamelCaseNamingStrategy(
      bool processDictionaryKeys,
      bool overrideSpecifiedNames,
      bool processExtensionDataNames)
      : this(processDictionaryKeys, overrideSpecifiedNames)
    {
      this.ProcessExtensionDataNames = processExtensionDataNames;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.CamelCaseNamingStrategy" /> class.
    /// </summary>
    public CamelCaseNamingStrategy()
    {
    }

    /// <summary>Resolves the specified property name.</summary>
    /// <param name="name">The property name to resolve.</param>
    /// <returns>The resolved property name.</returns>
    protected override string ResolvePropertyName(string name) => StringUtils.ToCamelCase(name);
  }
}
