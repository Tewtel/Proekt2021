﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonBinary
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonBinary : BsonValue
  {
    public BsonBinaryType BinaryType { get; set; }

    public BsonBinary(byte[] value, BsonBinaryType binaryType)
      : base((object) value, BsonType.Binary)
    {
      this.BinaryType = binaryType;
    }
  }
}
