// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.SerializationErrorCallback
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System.Runtime.Serialization;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Handles <see cref="T:Newtonsoft.Json.JsonSerializer" /> serialization error callback events.
  /// </summary>
  /// <param name="o">The object that raised the callback event.</param>
  /// <param name="context">The streaming context.</param>
  /// <param name="errorContext">The error context.</param>
  public delegate void SerializationErrorCallback(
    object o,
    StreamingContext context,
    ErrorContext errorContext);
}
