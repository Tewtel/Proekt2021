// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Spatial.DbGeometryWellKnownValue
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Runtime.Serialization;

namespace System.Data.Entity.Spatial
{
  /// <summary>
  /// A data contract serializable representation of a <see cref="T:System.Data.Entity.Spatial.DbGeometry" /> value.
  /// </summary>
  [DataContract]
  public sealed class DbGeometryWellKnownValue
  {
    /// <summary> Gets or sets the coordinate system identifier (SRID) of this value. </summary>
    [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
    public int CoordinateSystemId { get; set; }

    /// <summary> Gets or sets the well known text representation of this value. </summary>
    [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 2)]
    public string WellKnownText { get; set; }

    /// <summary> Gets or sets the well known binary representation of this value. </summary>
    [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 3)]
    public byte[] WellKnownBinary { get; set; }
  }
}
