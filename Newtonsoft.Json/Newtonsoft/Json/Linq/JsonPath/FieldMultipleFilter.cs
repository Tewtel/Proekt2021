﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.FieldMultipleFilter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class FieldMultipleFilter : PathFilter
  {
    internal List<string> Names;

    public FieldMultipleFilter(List<string> names) => this.Names = names;

    public override IEnumerable<JToken> ExecuteFilter(
      JToken root,
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken jtoken1 in current)
      {
        if (jtoken1 is JObject o2)
        {
          foreach (string name1 in this.Names)
          {
            string name = name1;
            JToken jtoken2 = o2[name];
            if (jtoken2 != null)
              yield return jtoken2;
            if (errorWhenNoMatch)
              throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) name));
            name = (string) null;
          }
        }
        else if (errorWhenNoMatch)
          throw new JsonException("Properties {0} not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) string.Join(", ", this.Names.Select<string, string>((Func<string, string>) (n => "'" + n + "'"))), (object) jtoken1.GetType().Name));
        o2 = (JObject) null;
      }
    }
  }
}
