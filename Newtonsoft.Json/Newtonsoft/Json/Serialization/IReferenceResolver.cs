﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.IReferenceResolver
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Used to resolve references when serializing and deserializing JSON by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  public interface IReferenceResolver
  {
    /// <summary>Resolves a reference to its object.</summary>
    /// <param name="context">The serialization context.</param>
    /// <param name="reference">The reference to resolve.</param>
    /// <returns>The object that was resolved from the reference.</returns>
    object ResolveReference(object context, string reference);

    /// <summary>Gets the reference for the specified object.</summary>
    /// <param name="context">The serialization context.</param>
    /// <param name="value">The object to get a reference for.</param>
    /// <returns>The reference to the object.</returns>
    string GetReference(object context, object value);

    /// <summary>
    /// Determines whether the specified object is referenced.
    /// </summary>
    /// <param name="context">The serialization context.</param>
    /// <param name="value">The object to test for a reference.</param>
    /// <returns>
    /// 	<c>true</c> if the specified object is referenced; otherwise, <c>false</c>.
    /// </returns>
    bool IsReferenced(object context, object value);

    /// <summary>Adds a reference to the specified object.</summary>
    /// <param name="context">The serialization context.</param>
    /// <param name="reference">The reference.</param>
    /// <param name="value">The object to reference.</param>
    void AddReference(object context, string reference, object value);
  }
}
