﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ObjectConstructor`1
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>Represents a method that constructs an object.</summary>
  /// <typeparam name="T">The object type to create.</typeparam>
  public delegate object ObjectConstructor<T>(params object?[] args);
}
