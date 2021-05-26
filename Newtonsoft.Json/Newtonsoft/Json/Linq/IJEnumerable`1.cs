// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.IJEnumerable`1
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System.Collections;
using System.Collections.Generic;


#nullable enable
namespace Newtonsoft.Json.Linq
{
  /// <summary>
  /// Represents a collection of <see cref="T:Newtonsoft.Json.Linq.JToken" /> objects.
  /// </summary>
  /// <typeparam name="T">The type of token.</typeparam>
  public interface IJEnumerable<out T> : IEnumerable<T>, IEnumerable where T : JToken
  {
    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Linq.IJEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> with the specified key.
    /// </summary>
    /// <value></value>
    IJEnumerable<JToken> this[object key] { get; }
  }
}
