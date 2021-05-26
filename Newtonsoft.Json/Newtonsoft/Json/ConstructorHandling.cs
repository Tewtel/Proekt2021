// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.ConstructorHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how constructors are used when initializing objects during deserialization by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  public enum ConstructorHandling
  {
    /// <summary>
    /// First attempt to use the public default constructor, then fall back to a single parameterized constructor, then to the non-public default constructor.
    /// </summary>
    Default,
    /// <summary>
    /// Json.NET will use a non-public default constructor before falling back to a parameterized constructor.
    /// </summary>
    AllowNonPublicDefaultConstructor,
  }
}
