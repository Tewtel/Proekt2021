// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ISerializationBinder
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Allows users to control class loading and mandate what class to load.
  /// </summary>
  public interface ISerializationBinder
  {
    /// <summary>
    /// When implemented, controls the binding of a serialized object to a type.
    /// </summary>
    /// <param name="assemblyName">Specifies the <see cref="T:System.Reflection.Assembly" /> name of the serialized object.</param>
    /// <param name="typeName">Specifies the <see cref="T:System.Type" /> name of the serialized object</param>
    /// <returns>The type of the object the formatter creates a new instance of.</returns>
    Type BindToType(string? assemblyName, string typeName);

    /// <summary>
    /// When implemented, controls the binding of a serialized object to a type.
    /// </summary>
    /// <param name="serializedType">The type of the object the formatter creates a new instance of.</param>
    /// <param name="assemblyName">Specifies the <see cref="T:System.Reflection.Assembly" /> name of the serialized object.</param>
    /// <param name="typeName">Specifies the <see cref="T:System.Type" /> name of the serialized object.</param>
    void BindToName(Type serializedType, out string? assemblyName, out string? typeName);
  }
}
