// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonObjectId
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Bson
{
  /// <summary>Represents a BSON Oid (object id).</summary>
  [Obsolete("BSON reading and writing has been moved to its own package. See https://www.nuget.org/packages/Newtonsoft.Json.Bson for more details.")]
  public class BsonObjectId
  {
    /// <summary>Gets or sets the value of the Oid.</summary>
    /// <value>The value of the Oid.</value>
    public byte[] Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Bson.BsonObjectId" /> class.
    /// </summary>
    /// <param name="value">The Oid value.</param>
    public BsonObjectId(byte[] value)
    {
      ValidationUtils.ArgumentNotNull((object) value, nameof (value));
      this.Value = value.Length == 12 ? value : throw new ArgumentException("An ObjectId must be 12 bytes", nameof (value));
    }
  }
}
