﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ScanFilter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System.Collections.Generic;


#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class ScanFilter : PathFilter
  {
    internal string? Name;

    public ScanFilter(string? name) => this.Name = name;

    public override IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken jtoken in current)
      {
        JToken c = jtoken;
        if (this.Name == null)
          yield return c;
        JToken value = c;
        while (true)
        {
          do
          {
            value = PathFilter.GetNextScanValue(c, (JToken) (value as JContainer), value);
            if (value != null)
            {
              if (value is JProperty jproperty7)
              {
                if (jproperty7.Name == this.Name)
                  yield return jproperty7.Value;
              }
            }
            else
              goto label_10;
          }
          while (this.Name != null);
          yield return value;
        }
label_10:
        value = (JToken) null;
        c = (JToken) null;
      }
    }
  }
}
