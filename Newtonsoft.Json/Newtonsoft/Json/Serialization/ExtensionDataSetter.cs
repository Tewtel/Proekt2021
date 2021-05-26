// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ExtensionDataSetter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Sets extension data for an object during deserialization.
  /// </summary>
  /// <param name="o">The object to set extension data on.</param>
  /// <param name="key">The extension data key.</param>
  /// <param name="value">The extension data value.</param>
  public delegate void ExtensionDataSetter(object o, string key, object? value);
}
