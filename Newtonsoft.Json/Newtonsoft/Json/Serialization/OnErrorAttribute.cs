﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.OnErrorAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// When applied to a method, specifies that the method is called when an error occurs serializing an object.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, Inherited = false)]
  public sealed class OnErrorAttribute : Attribute
  {
  }
}
