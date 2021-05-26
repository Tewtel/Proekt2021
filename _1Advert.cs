// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger._1Advert
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

namespace AvitoAvtoRinger
{
  public class _1Advert
  {
    private string imageUrl = "";
    private string phone = "";
    private string description = "";

    public _1Advert(string imageUrl, string phone, string description)
    {
      this.imageUrl = imageUrl;
      this.phone = phone;
      this.description = description;
    }

    public string ImageUrl => this.imageUrl;

    public string Phone => this.description;

    public string Description => this.description;
  }
}
