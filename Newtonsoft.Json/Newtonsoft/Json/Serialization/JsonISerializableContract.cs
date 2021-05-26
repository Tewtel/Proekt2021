// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonISerializableContract
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="T:System.Type" /> used by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  public class JsonISerializableContract : JsonContainerContract
  {
    /// <summary>
    /// Gets or sets the <see cref="T:System.Runtime.Serialization.ISerializable" /> object constructor.
    /// </summary>
    /// <value>The <see cref="T:System.Runtime.Serialization.ISerializable" /> object constructor.</value>
    public ObjectConstructor<object>? ISerializableCreator { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonISerializableContract" /> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    public JsonISerializableContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Serializable;
    }
  }
}
