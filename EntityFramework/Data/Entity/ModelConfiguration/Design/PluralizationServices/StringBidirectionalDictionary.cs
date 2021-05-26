// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Design.PluralizationServices.StringBidirectionalDictionary
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Pluralization;

namespace System.Data.Entity.ModelConfiguration.Design.PluralizationServices
{
  internal class StringBidirectionalDictionary : BidirectionalDictionary<string, string>
  {
    internal StringBidirectionalDictionary()
    {
    }

    internal StringBidirectionalDictionary(Dictionary<string, string> firstToSecondDictionary)
      : base(firstToSecondDictionary)
    {
    }

    internal override bool ExistsInFirst(string value) => base.ExistsInFirst(value.ToLowerInvariant());

    internal override bool ExistsInSecond(string value) => base.ExistsInSecond(value.ToLowerInvariant());

    internal override string GetFirstValue(string value) => base.GetFirstValue(value.ToLowerInvariant());

    internal override string GetSecondValue(string value) => base.GetSecondValue(value.ToLowerInvariant());
  }
}
