// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.WebWorker
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.Collections.Generic;

namespace AvitoAvtoRinger
{
  internal class WebWorker
  {
    public List<Adverts1> GetAdvert(string url, Form1 fm1)
    {
      List<Adverts1> adverts1List = new List<Adverts1>();
      try
      {
        return new HtmlWorker().ParseAdverts(fm1, url);
      }
      catch (Exception ex)
      {
        fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": Ошибка при получении данных с сервера" + ex.ToString() + Environment.NewLine)));
        return adverts1List;
      }
    }
  }
}
