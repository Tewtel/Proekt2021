// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.IValueProvider
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>Provides methods to get and set values.</summary>
  public interface IValueProvider
  {
    /// <summary>Sets the value.</summary>
    /// <param name="target">The target to set the value on.</param>
    /// <param name="value">The value to set on the target.</param>
    void SetValue(object target, object? value);

    /// <summary>Gets the value.</summary>
    /// <param name="target">The target to get the value from.</param>
    /// <returns>The value.</returns>
    object? GetValue(object target);
  }
}
