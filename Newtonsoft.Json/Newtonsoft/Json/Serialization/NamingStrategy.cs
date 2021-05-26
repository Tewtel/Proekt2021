// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.NamingStrategy
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// A base class for resolving how property names and dictionary keys are serialized.
  /// </summary>
  public abstract class NamingStrategy
  {
    /// <summary>
    /// A flag indicating whether dictionary keys should be processed.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool ProcessDictionaryKeys { get; set; }

    /// <summary>
    /// A flag indicating whether extension data names should be processed.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool ProcessExtensionDataNames { get; set; }

    /// <summary>
    /// A flag indicating whether explicitly specified property names,
    /// e.g. a property name customized with a <see cref="T:Newtonsoft.Json.JsonPropertyAttribute" />, should be processed.
    /// Defaults to <c>false</c>.
    /// </summary>
    public bool OverrideSpecifiedNames { get; set; }

    /// <summary>Gets the serialized name for a given property name.</summary>
    /// <param name="name">The initial property name.</param>
    /// <param name="hasSpecifiedName">A flag indicating whether the property has had a name explicitly specified.</param>
    /// <returns>The serialized property name.</returns>
    public virtual string GetPropertyName(string name, bool hasSpecifiedName) => hasSpecifiedName && !this.OverrideSpecifiedNames ? name : this.ResolvePropertyName(name);

    /// <summary>
    /// Gets the serialized name for a given extension data name.
    /// </summary>
    /// <param name="name">The initial extension data name.</param>
    /// <returns>The serialized extension data name.</returns>
    public virtual string GetExtensionDataName(string name) => !this.ProcessExtensionDataNames ? name : this.ResolvePropertyName(name);

    /// <summary>Gets the serialized key for a given dictionary key.</summary>
    /// <param name="key">The initial dictionary key.</param>
    /// <returns>The serialized dictionary key.</returns>
    public virtual string GetDictionaryKey(string key) => !this.ProcessDictionaryKeys ? key : this.ResolvePropertyName(key);

    /// <summary>Resolves the specified property name.</summary>
    /// <param name="name">The property name to resolve.</param>
    /// <returns>The resolved property name.</returns>
    protected abstract string ResolvePropertyName(string name);

    /// <summary>Hash code calculation</summary>
    /// <returns></returns>
    public override int GetHashCode() => ((this.GetType().GetHashCode() * 397 ^ this.ProcessDictionaryKeys.GetHashCode()) * 397 ^ this.ProcessExtensionDataNames.GetHashCode()) * 397 ^ this.OverrideSpecifiedNames.GetHashCode();

    /// <summary>Object equality implementation</summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj) => this.Equals(obj as NamingStrategy);

    /// <summary>Compare to another NamingStrategy</summary>
    /// <param name="other"></param>
    /// <returns></returns>
    protected bool Equals(NamingStrategy? other) => other != null && this.GetType() == other.GetType() && (this.ProcessDictionaryKeys == other.ProcessDictionaryKeys && this.ProcessExtensionDataNames == other.ProcessExtensionDataNames) && this.OverrideSpecifiedNames == other.OverrideSpecifiedNames;
  }
}
