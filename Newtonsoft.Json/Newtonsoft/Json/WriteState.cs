// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.WriteState
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies the state of the <see cref="T:Newtonsoft.Json.JsonWriter" />.
  /// </summary>
  public enum WriteState
  {
    /// <summary>
    /// An exception has been thrown, which has left the <see cref="T:Newtonsoft.Json.JsonWriter" /> in an invalid state.
    /// You may call the <see cref="M:Newtonsoft.Json.JsonWriter.Close" /> method to put the <see cref="T:Newtonsoft.Json.JsonWriter" /> in the <c>Closed</c> state.
    /// Any other <see cref="T:Newtonsoft.Json.JsonWriter" /> method calls result in an <see cref="T:System.InvalidOperationException" /> being thrown.
    /// </summary>
    Error,
    /// <summary>
    /// The <see cref="M:Newtonsoft.Json.JsonWriter.Close" /> method has been called.
    /// </summary>
    Closed,
    /// <summary>An object is being written.</summary>
    Object,
    /// <summary>An array is being written.</summary>
    Array,
    /// <summary>A constructor is being written.</summary>
    Constructor,
    /// <summary>A property is being written.</summary>
    Property,
    /// <summary>
    /// A <see cref="T:Newtonsoft.Json.JsonWriter" /> write method has not been called.
    /// </summary>
    Start,
  }
}
