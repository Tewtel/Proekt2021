// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.Adverts1
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;

namespace AvitoAvtoRinger
{
  public class Adverts1 : IComparable<Adverts1>
  {
    public string name = "";
    public string price = "";
    public string link = "";
    public DateTime date = DateTime.Today.AddDays(-3.0);
    public string stopWord;
    public string targetWord;
    public string id = "";

    public Adverts1(
      string name,
      string price,
      string link,
      DateTime date,
      string stopWord = "",
      string targetWord = "",
      string id = "0")
    {
      try
      {
        if (name != null)
          this.name = name;
        if (price != null)
          this.price = price;
        if (link != null)
          this.link = link;
        this.date = date;
        this.stopWord = stopWord;
        this.targetWord = targetWord;
        this.id = id;
      }
      catch (Exception ex)
      {
        ex.ToString();
      }
    }

    public int CompareTo(Adverts1 AdvertToCompare)
    {
      if (this.date > AdvertToCompare.date)
        return 1;
      return this.date < AdvertToCompare.date ? -1 : 0;
    }
  }
}
