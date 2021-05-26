// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Pluralization.BidirectionalDictionary`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Infrastructure.Pluralization
{
  internal class BidirectionalDictionary<TFirst, TSecond>
  {
    internal Dictionary<TFirst, TSecond> FirstToSecondDictionary { get; set; }

    internal Dictionary<TSecond, TFirst> SecondToFirstDictionary { get; set; }

    internal BidirectionalDictionary()
    {
      this.FirstToSecondDictionary = new Dictionary<TFirst, TSecond>();
      this.SecondToFirstDictionary = new Dictionary<TSecond, TFirst>();
    }

    internal BidirectionalDictionary(
      Dictionary<TFirst, TSecond> firstToSecondDictionary)
      : this()
    {
      foreach (TFirst key in firstToSecondDictionary.Keys)
        this.AddValue(key, firstToSecondDictionary[key]);
    }

    internal virtual bool ExistsInFirst(TFirst value) => this.FirstToSecondDictionary.ContainsKey(value);

    internal virtual bool ExistsInSecond(TSecond value) => this.SecondToFirstDictionary.ContainsKey(value);

    internal virtual TSecond GetSecondValue(TFirst value) => this.ExistsInFirst(value) ? this.FirstToSecondDictionary[value] : default (TSecond);

    internal virtual TFirst GetFirstValue(TSecond value) => this.ExistsInSecond(value) ? this.SecondToFirstDictionary[value] : default (TFirst);

    internal void AddValue(TFirst firstValue, TSecond secondValue)
    {
      this.FirstToSecondDictionary.Add(firstValue, secondValue);
      if (this.SecondToFirstDictionary.ContainsKey(secondValue))
        return;
      this.SecondToFirstDictionary.Add(secondValue, firstValue);
    }
  }
}
