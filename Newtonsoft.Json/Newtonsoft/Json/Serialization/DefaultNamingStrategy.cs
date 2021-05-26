// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultNamingStrategy
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// The default naming strategy. Property names and dictionary keys are unchanged.
  /// </summary>
  public class DefaultNamingStrategy : NamingStrategy
  {
    /// <summary>Resolves the specified property name.</summary>
    /// <param name="name">The property name to resolve.</param>
    /// <returns>The resolved property name.</returns>
    protected override string ResolvePropertyName(string name) => name;
  }
}
