// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.IMetadataAnnotationSerializer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Implement this interface to allow custom annotations represented by <see cref="T:System.Data.Entity.Core.Metadata.Edm.MetadataProperty" /> instances to be
  /// serialized to and from the EDMX XML. Usually a serializer instance is set using the
  /// <see cref="M:System.Data.Entity.DbConfiguration.SetMetadataAnnotationSerializer(System.String,System.Func{System.Data.Entity.Infrastructure.IMetadataAnnotationSerializer})" /> method.
  /// </summary>
  public interface IMetadataAnnotationSerializer
  {
    /// <summary>
    /// Serializes the given annotation value into a string for storage in the EDMX XML.
    /// </summary>
    /// <param name="name">The name of the annotation that is being serialized.</param>
    /// <param name="value">The value to serialize.</param>
    /// <returns>The serialized value.</returns>
    string Serialize(string name, object value);

    /// <summary>
    /// Deserializes the given string back into the expected annotation value.
    /// </summary>
    /// <param name="name">The name of the annotation that is being deserialized.</param>
    /// <param name="value">The string to deserialize.</param>
    /// <returns>The deserialized annotation value.</returns>
    object Deserialize(string name, string value);
  }
}
