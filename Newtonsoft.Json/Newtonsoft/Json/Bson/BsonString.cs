// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Bson.BsonString
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Bson
{
  internal class BsonString : BsonValue
  {
    public int ByteCount { get; set; }

    public bool IncludeLength { get; }

    public BsonString(object value, bool includeLength)
      : base(value, BsonType.String)
    {
      this.IncludeLength = includeLength;
    }
  }
}
