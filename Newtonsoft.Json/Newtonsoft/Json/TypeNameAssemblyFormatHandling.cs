﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.TypeNameAssemblyFormatHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Indicates the method that will be used during deserialization for locating and loading assemblies.
  /// </summary>
  public enum TypeNameAssemblyFormatHandling
  {
    /// <summary>
    /// In simple mode, the assembly used during deserialization need not match exactly the assembly used during serialization. Specifically, the version numbers need not match as the <c>LoadWithPartialName</c> method of the <see cref="T:System.Reflection.Assembly" /> class is used to load the assembly.
    /// </summary>
    Simple,
    /// <summary>
    /// In full mode, the assembly used during deserialization must match exactly the assembly used during serialization. The <c>Load</c> method of the <see cref="T:System.Reflection.Assembly" /> class is used to load the assembly.
    /// </summary>
    Full,
  }
}
