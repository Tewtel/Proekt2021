// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.StatSetClass
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.Collections.Generic;
using System.Text;

namespace AvitoAvtoRinger
{
  public static class StatSetClass
  {
    public static bool r = true;
    public static bool l = false;
    public static bool notify = false;
    public static int type = 0;
    public static int timeOut = 1000;
    private static int timerInterval = 120000;
    public static string proxyIp = "";
    public static string proxyPort = "";
    public static string proxyLogin = "";
    public static string proxyPassword = "";
    public static string appVersion = "";
    public static string AvitoBlockL = DateTime.Now.ToShortTimeString() + ": ================================================================================= " + Environment.NewLine + "Авито не ответило на запрос, в этом нет ничего страшного, программа попробует повторить запрос, если ошибка будет повторятся постоянно проверть правильность поискового запроса, либо выключите программу на 30 - 1 час, после чего запустите снова. (так же может помочь увеличение интервала между поисками, либо уменьшение количества поисков, попробуйте оптимизировать используя стоп и ключевые слова " + Environment.NewLine;
    public static string YoulaBlockL = DateTime.Now.ToShortTimeString() + ": ================================================================================= " + Environment.NewLine + "Юла не ответила на запрос, в этом нет ничего страшного, программа попробует повторить запрос, если ошибка будет повторятся постоянно проверть правильность поискового запроса, либо выключите программу на 30 - 1 час, после чего запустите снова. (так же может помочь увеличение интервала между поисками, либо уменьшение количества поисков, попробуйте оптимизировать используя стоп и ключевые слова " + Environment.NewLine;
    public static string AutoRuBlockL = DateTime.Now.ToShortTimeString() + ": ================================================================================= " + Environment.NewLine + "АвтоРу не ответило на запрос, в этом нет ничего страшного, программа попробует повторить запрос, если ошибка будет повторятся постоянно проверть правильность поискового запроса, либо выключите программу на 30 - 1 час, после чего запустите снова. (так же может помочь увеличение интервала между поисками, либо уменьшение количества поисков, попробуйте оптимизировать используя стоп и ключевые слова " + Environment.NewLine;
    public static string browser = "";
    public static string ACTkey = "";
    public static bool dynamicCheckBox = false;
    public static string proxyIpDynamic = "";
    public static string proxyPortDynamic = "";
    public static string proxyLoginDynamic = "";
    public static string proxyPasswordDynamic = "";
    public static bool bwsrOpen = true;
    public static bool RaiseAdverts = false;
    public static bool MagazCheck = false;
    public static bool AvitoDostavka = true;
    public static bool ptz = true;
    public static bool telgSend = false;
    public static StatSetClass.Permissions perm = StatSetClass.Permissions.Demka;
    public static Random rnd = new Random();
    public static List<string> stopWordsList = new List<string>();
    public static List<string> goalWordsList = new List<string>();
    public static string allstopWords = "";
    public static string allgoalWords = "";

    public static int TimerInterval
    {
      get => StatSetClass.timerInterval;
      set => StatSetClass.timerInterval = value * 1000;
    }

    public static string Base64Encode(string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

    public static string Base64Decode(string base64EncodedData)
    {
      try
      {
        return Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));
      }
      catch
      {
        return "";
      }
    }

    [Flags]
    public enum Permissions
    {
      Demka = 1,
      Avito = 2,
      Youla = 4,
      Olx = 8,
      Drom = 16, // 0x00000010
      Autoru = 64, // 0x00000040
      Cian = 128, // 0x00000080
      Ads = 32768, // 0x00008000
    }
  }
}
