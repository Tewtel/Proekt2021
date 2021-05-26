// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.HtmlWorker
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AvitoAvtoRinger
{
  internal class HtmlWorker
  {
    public _1Advert GetAdevertInfo(string Url)
    {
      try
      {
        HtmlWeb htmlWeb = new HtmlWeb();
        htmlWeb.OverrideEncoding = Encoding.UTF8;
        htmlWeb.UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 83.0.4103.61 Safari / 537.36";
        if (Url.ToLower().Contains("youla.ru"))
          return new _1Advert("Youla", "", "");
        if (!Url.ToLower().Contains("avito.ru"))
          return new _1Advert("", "", "");
        HtmlAgilityPack.HtmlDocument htmlDocument = htmlWeb.Load(Url);
        string imageUrl = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='item-view-main js-item-view-main']//img[@elementtiming='bx.gallery']").Attributes["src"].Value;
        string description = "Ну удалось получить описание";
        try
        {
          description = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='item-view-main js-item-view-main']//div[@class='item-description-text']").InnerHtml.Replace("<br>", Environment.NewLine).Replace("<p>", Environment.NewLine).Replace("</p>", "");
        }
        catch
        {
        }
        return new _1Advert(imageUrl, "", description);
      }
      catch
      {
        return new _1Advert("", "", "");
      }
    }

    public List<Adverts1> ParseAdverts(Form1 fm1, string url)
    {
      try
      {
        List<Adverts1> adverts1List = new List<Adverts1>();
        HtmlWeb htmlWeb = new HtmlWeb();
        htmlWeb.OverrideEncoding = Encoding.UTF8;
        htmlWeb.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36";
        HtmlAgilityPack.HtmlDocument htmlDocument = StatSetClass.dynamicCheckBox ? htmlWeb.Load(url, StatSetClass.proxyIpDynamic, int.Parse(StatSetClass.proxyPortDynamic), StatSetClass.proxyLoginDynamic, StatSetClass.proxyPasswordDynamic) : htmlWeb.Load(url);
        if (url.ToLower().Contains("avito.ru"))
        {
          if ((StatSetClass.perm & StatSetClass.Permissions.Demka) != StatSetClass.Permissions.Demka && (StatSetClass.perm & StatSetClass.Permissions.Avito) != StatSetClass.Permissions.Avito)
            return adverts1List;
          if (StatSetClass.l)
          {
            try
            {
              new StreamWriter((Stream) new FileStream("logging\\logAVITO" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + StatSetClass.rnd.Next(1999, 31000).ToString() + ".html", FileMode.Create), Encoding.UTF8).Write(htmlDocument.DocumentNode.OuterHtml.ToString());
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Не могу записать лог", ex.Message);
            }
          }
          HtmlNodeCollection htmlNodeCollection = (HtmlNodeCollection) null;
          try
          {
            htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//div[@class='items-items-38oUm']/div[contains(@class, 'iva-item-root')]");
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show(ex.Message);
          }
          if (htmlNodeCollection != null)
          {
            fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Сканируем avito.ru " + Environment.NewLine)));
            foreach (HtmlNode htmlNode in (IEnumerable<HtmlNode>) htmlNodeCollection)
            {
              try
              {
                DateTime.Now.AddDays(-2.0);
                string id;
                try
                {
                  id = htmlNode.Attributes["data-item-id"].Value;
                }
                catch
                {
                  id = "0";
                }
                string str1;
                try
                {
                  str1 = htmlNode.SelectSingleNode("./div[@class='item__line']/div[@class='item_table-wrapper']/div[@class='description item_table-description']/div[@class='data']//a").InnerText.Replace("&nbsp;", "");
                }
                catch
                {
                  str1 = "ZERO";
                }
                string name;
                try
                {
                  name = htmlNode.SelectSingleNode(".//h3[contains(@class, 'title-root')]").InnerText.Replace("&nbsp;", " ").Replace("\n", "");
                  if (str1 != "ZERO")
                    name = name + " ==(" + str1 + ")==";
                }
                catch
                {
                  name = "Не удалось спарсить название, на мониторинг не влияет";
                }
                string str2;
                try
                {
                  try
                  {
                    str2 = htmlNode.SelectSingleNode(".//span[contains(@class, 'price-text')]").InnerText.ToString().Replace(" ", "");
                    MatchCollection matchCollection = new Regex("\\D*(\\d+)\\D*", RegexOptions.IgnoreCase).Matches(str2);
                    if (matchCollection.Count > 0)
                      str2 = matchCollection[0].Groups[1].Value;
                    if (str2 == "\nЦенанеуказана")
                      str2 = "Цена не указана";
                  }
                  catch
                  {
                    str2 = htmlNode.SelectSingleNode("./div[@class='item__line']/div[@class='item_table-wrapper']/div[@class='description item_table-description']/div[@class='snippet-price-row']/span[@class='snippet-price snippet-price-vas']").InnerText.Replace("&nbsp;", "").Replace(" ", "");
                    MatchCollection matchCollection = new Regex("\\D*(\\d+)\\D*", RegexOptions.IgnoreCase).Matches(str2);
                    if (matchCollection.Count > 0)
                      str2 = matchCollection[0].Groups[1].Value;
                    if (str2 == "\nЦенанеуказана")
                      str2 = "Цена не указана";
                  }
                }
                catch
                {
                  str2 = "Не удалось узнать цену, на поиск не влияет";
                }
                string str3 = "https://www.avito.ru" + htmlNode.SelectSingleNode(".//div[contains(@class, 'iva-item-titleStep')]/a").Attributes["href"].Value;
                DateTime date;
                try
                {
                  date = this.DateNormaliserAvito(htmlNode.SelectSingleNode(".//div[contains(@class, 'date-text')]").InnerText.Replace("&nbsp;", ""));
                }
                catch
                {
                  date = DateTime.Now;
                }
                if (StatSetClass.AvitoDostavka)
                {
                  string pattern = "avito.ru\\/([\\s\\S]*?)\\/";
                  string str4 = "";
                  MatchCollection matchCollection = new Regex(pattern, RegexOptions.IgnoreCase).Matches(str3);
                  if (matchCollection.Count > 0)
                    str4 = matchCollection[0].Groups[1].Value;
                  if (url.Contains(str4.Replace("-", "_")) || url.Contains(str4))
                  {
                    adverts1List.Add(new Adverts1(name, str2, str3, date, id: id));
                  }
                  else
                  {
                    StatSetClass.AvitoDostavka = false;
                    fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": В одном из поисков вы ищите не по конкретному городу, галочка 'Показывать объявления только по моему городу, будет снята, для продолжения корректной работы программы' " + Environment.NewLine)));
                    fm1.AvitoDostavkaCheckBox.Invoke((Action) (() => fm1.AvitoDostavkaCheckBox.Checked = false));
                  }
                }
                else
                  adverts1List.Add(new Adverts1(name, str2, str3, date, id: id));
              }
              catch (Exception ex)
              {
              }
            }
          }
          else if (StatSetClass.l)
            fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(StatSetClass.AvitoBlockL)));
          return adverts1List;
        }
        if (url.ToLower().Contains("olx.ua"))
        {
          if ((StatSetClass.perm & StatSetClass.Permissions.Demka) != StatSetClass.Permissions.Demka && (StatSetClass.perm & StatSetClass.Permissions.Olx) != StatSetClass.Permissions.Olx)
            return adverts1List;
          if (StatSetClass.l)
          {
            try
            {
              new StreamWriter((Stream) new FileStream("logging\\logOlx" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + StatSetClass.rnd.Next(1999, 31000).ToString() + ".html", FileMode.Create), Encoding.UTF8).Write(htmlDocument.DocumentNode.OuterHtml.ToString());
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Не могу записать лог", ex.Message);
            }
          }
          HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//table[@class='fixed offers breakword redesigned']/tbody/tr[@class='wrap']");
          if (htmlNodeCollection != null)
          {
            fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Сканируем olx.ua" + Environment.NewLine)));
            foreach (HtmlNode htmlNode in (IEnumerable<HtmlNode>) htmlNodeCollection)
            {
              try
              {
                DateTime date = DateTime.Now.AddDays(-2.0);
                string name = htmlNode.SelectSingleNode("./td/div/table/tbody/tr/td/div/h3/a/strong").InnerText.Replace("&nbsp;", " ").Replace("\n", "");
                string str = htmlNode.SelectSingleNode("./td/div/table/tbody/tr/td/div/p/strong").InnerText.Replace(" ", "").Replace("грн.", "");
                MatchCollection matchCollection = new Regex("\\D*(\\d+)\\D*", RegexOptions.IgnoreCase).Matches(str);
                if (matchCollection.Count > 0)
                  str = matchCollection[0].Groups[1].Value;
                if (str == "\nЦенанеуказана")
                  str = "Цена не указана";
                string link = htmlNode.SelectSingleNode("./td/div/table/tbody/tr/td/div/h3/a").Attributes["href"].Value;
                try
                {
                  date = this.DateNormaliserOLX(htmlNode.SelectSingleNode("./td/div/table/tbody/tr[2]/td/div/p/small[@class='breadcrumb x-normal'][2]/span").InnerText);
                }
                catch
                {
                  try
                  {
                    int num = (int) MessageBox.Show("Проверка времени объявления, не пройдена LOG 1");
                  }
                  catch
                  {
                  }
                }
                adverts1List.Add(new Adverts1(name, str, link, date));
              }
              catch (Exception ex)
              {
              }
            }
          }
          else if (StatSetClass.l)
            fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(StatSetClass.AvitoBlockL)));
          return adverts1List;
        }
        if (url.ToLower().Contains("auto.ru"))
        {
          if ((StatSetClass.perm & StatSetClass.Permissions.Demka) != StatSetClass.Permissions.Demka && (StatSetClass.perm & StatSetClass.Permissions.Autoru) != StatSetClass.Permissions.Autoru)
            return adverts1List;
          if (StatSetClass.l)
          {
            try
            {
              new StreamWriter((Stream) new FileStream("logging\\logAutoRu" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + StatSetClass.rnd.Next(1999, 31000).ToString() + ".html", FileMode.Create), Encoding.UTF8).Write(htmlDocument.DocumentNode.OuterHtml.ToString());
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Не могу записать лог", ex.Message);
            }
          }
          HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//div[@class='ListingCars-module__container ListingCars-module__list']");
          if (htmlNodeCollection != null)
          {
            fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Сканируем auto.ru" + Environment.NewLine)));
            foreach (HtmlNode htmlNode in (IEnumerable<HtmlNode>) htmlNodeCollection)
            {
              try
              {
                DateTime date = DateTime.Now.AddDays(-2.0);
                string name = htmlNode.SelectSingleNode("./td/div/table/tbody/tr/td/div/h3/a/strong").InnerText.Replace("&nbsp;", " ").Replace("\n", "");
                string str = htmlNode.SelectSingleNode("./td/div/table/tbody/tr/td/div/p/strong").InnerText.Replace(" ", "").Replace("грн.", "");
                MatchCollection matchCollection = new Regex("\\D*(\\d+)\\D*", RegexOptions.IgnoreCase).Matches(str);
                if (matchCollection.Count > 0)
                  str = matchCollection[0].Groups[1].Value;
                if (str == "\nЦенанеуказана")
                  str = "Цена не указана";
                string link = htmlNode.SelectSingleNode("./td/div/table/tbody/tr/td/div/h3/a").Attributes["href"].Value;
                try
                {
                  date = this.DateNormaliserOLX(htmlNode.SelectSingleNode("./td/div/table/tbody/tr[2]/td/div/p/small[@class='breadcrumb x-normal'][2]/span").InnerText);
                }
                catch
                {
                  try
                  {
                    int num = (int) MessageBox.Show("Проверка времени объявления, не пройдена LOG 1");
                  }
                  catch
                  {
                  }
                }
                adverts1List.Add(new Adverts1(name, str, link, date));
              }
              catch (Exception ex)
              {
              }
            }
          }
          else if (StatSetClass.l)
            fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(StatSetClass.AutoRuBlockL)));
          return adverts1List;
        }
        if ((StatSetClass.perm & StatSetClass.Permissions.Demka) != StatSetClass.Permissions.Demka && (StatSetClass.perm & StatSetClass.Permissions.Youla) != StatSetClass.Permissions.Youla)
          return adverts1List;
        if (url.Contains("auto.youla"))
        {
          if (StatSetClass.l)
          {
            try
            {
              new StreamWriter((Stream) new FileStream("logging\\logAuto.YOULA" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + StatSetClass.rnd.Next(1999, 31000).ToString() + ".html", FileMode.Create), Encoding.UTF8).Write(htmlDocument.DocumentNode.OuterHtml.ToString());
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Не могу записать лог", ex.Message);
            }
          }
          HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//span/article[@class='SerpSnippet_snippet__3O1t2 app_roundedBlockWithShadow__1rh6w']");
          if (htmlNodeCollection != null)
          {
            fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Сканируем auto.youla.ru" + Environment.NewLine)));
            foreach (HtmlNode htmlNode in (IEnumerable<HtmlNode>) htmlNodeCollection)
            {
              try
              {
                string str1 = "0";
                DateTime date = DateTime.Now.AddDays(-2.0);
                string name = htmlNode.SelectSingleNode("./div[@class='SerpSnippet_snippetContent__d8CHK']/div[@class='SerpSnippet_data__3ezjY']/div[@class='SerpSnippet_topInfo__1ZraC']/div[1]/div[@class='SerpSnippet_titleWrapper__38bZM']").InnerText.Replace("&nbsp;", " ").Replace("\n", "");
                try
                {
                  try
                  {
                    try
                    {
                      try
                      {
                        str1 = htmlNode.SelectSingleNode("./div[@class='SerpSnippet_snippetContent__d8CHK']/div[@class='SerpSnippet_data__3ezjY']/div[@class='SerpSnippet_topInfo__1ZraC']/div[@class='SerpSnippet_topInfoRight__pG1ha']/div[@class='SerpSnippet_price__1DHTI SerpSnippet_titleText__1Ex8A rouble']").InnerText.Replace("&nbsp;", "");
                      }
                      catch
                      {
                        str1 = htmlNode.SelectSingleNode("./div[@class='SerpSnippet_snippetContent__d8CHK']/div[@class='SerpSnippet_data__3ezjY']/div[@class='SerpSnippet_topInfo__1ZraC']/div[@class='SerpSnippet_topInfoRight__pG1ha']/div/div[@class='SerpSnippet_price__1DHTI SerpSnippet_titleText__1Ex8A rouble']").InnerText.Replace("&nbsp;", "");
                      }
                    }
                    catch
                    {
                      str1 = htmlNode.SelectSingleNode("./div[@class='SerpSnippet_snippetContent__d8CHK']/div[@class='SerpSnippet_data__3ezjY']/div[@class='SerpSnippet_topInfo__1ZraC']/div[@class='SerpSnippet_topInfoRight__pG1ha']/div[@class='SerpSnippet_price__1DHTI SerpSnippet_titleText__1Ex8A SerpSnippet_priceWithHighlight__396_g rouble']").InnerText.Replace("&nbsp;", "");
                    }
                  }
                  catch
                  {
                    str1 = htmlNode.SelectSingleNode("./div[@class='SerpSnippet_snippetContent__d8CHK']/div[@class='SerpSnippet_data__3ezjY']/div[@class='SerpSnippet_topInfo__1ZraC']/div[@class='SerpSnippet_topInfoRight__pG1ha']/div/div[@class='SerpSnippet_price__1DHTI SerpSnippet_titleText__1Ex8A SerpSnippet_priceWithHighlight__396_g rouble']").InnerText.Replace("&nbsp;", "");
                  }
                }
                catch
                {
                }
                string str2 = str1.Replace(" ", "");
                MatchCollection matchCollection = new Regex("\\D*(\\d+)\\D*", RegexOptions.IgnoreCase).Matches(str2);
                if (matchCollection.Count > 0)
                  str2 = matchCollection[0].Groups[1].Value;
                if (str2 == "\nЦенанеуказана")
                  str2 = "Цена не указана";
                string link = htmlNode.SelectSingleNode("./div[@class='SerpSnippet_snippetContent__d8CHK']/div[@class='SerpSnippet_colLeft__1qO8G']/a").Attributes["href"].Value;
                try
                {
                  date = this.DateNormaliserYoulaAvto(htmlNode.SelectSingleNode("./div/div[2]/div[1]/div[1]/div[2]").InnerText);
                }
                catch
                {
                  try
                  {
                    int num = (int) MessageBox.Show("Проверка времени объявления, не пройдена LOG 3");
                  }
                  catch
                  {
                  }
                }
                adverts1List.Add(new Adverts1(name, str2, link, date));
              }
              catch
              {
              }
            }
          }
          else if (StatSetClass.l)
            fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(StatSetClass.YoulaBlockL)));
        }
        else
        {
          HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//ul[@class='product_list _board_items']/li");
          if (StatSetClass.l)
          {
            try
            {
              new StreamWriter((Stream) new FileStream("logging\\logYOULA" + DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + StatSetClass.rnd.Next(1999, 31000).ToString() + ".html", FileMode.Create), Encoding.UTF8).Write(htmlDocument.DocumentNode.OuterHtml.ToString());
            }
            catch (Exception ex)
            {
              int num = (int) MessageBox.Show("Не могу записать лог", ex.Message);
            }
          }
          if (htmlNodeCollection != null)
          {
            fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Сканируем youla.ru" + Environment.NewLine)));
            foreach (HtmlNode htmlNode in (IEnumerable<HtmlNode>) htmlNodeCollection)
            {
              try
              {
                DateTime date = DateTime.Now.AddDays(-2.0);
                string name = htmlNode.SelectSingleNode("./a").Attributes["title"].Value.Replace("&nbsp;", " ").Replace("\n", "");
                string str1 = htmlNode.SelectSingleNode("./a/figure/figcaption/div[@class='product_item__description ']/ div").InnerText.Replace("&nbsp;", "").Replace(" ", "");
                MatchCollection matchCollection1 = new Regex("\\D*(\\d+)\\D*", RegexOptions.IgnoreCase).Matches(str1);
                if (matchCollection1.Count > 0)
                  str1 = matchCollection1[0].Groups[1].Value;
                if (str1 == "\nЦенанеуказана")
                  str1 = "Цена не указана";
                string str2 = "https://youla.ru" + htmlNode.SelectSingleNode("./a").Attributes["href"].Value;
                try
                {
                  date = this.DateNormaliserYoula(htmlNode.SelectSingleNode("./a/figure/figcaption/div[@class='product_item__date']/span[@class='hidden-xs']").InnerText);
                }
                catch
                {
                  try
                  {
                    int num = (int) MessageBox.Show("Проверка времени объявления, не пройдена LOG 4");
                  }
                  catch
                  {
                  }
                }
                string pattern = "youla.ru\\/(\\w+-?\\w+-?\\w*)\\/.*";
                string str3 = "";
                MatchCollection matchCollection2 = new Regex(pattern, RegexOptions.IgnoreCase).Matches(str2);
                if (matchCollection2.Count > 0)
                  str3 = matchCollection2[0].Groups[1].Value;
                if (url.Contains(str3))
                  adverts1List.Add(new Adverts1(name, str1, str2, date));
              }
              catch (Exception ex)
              {
              }
            }
          }
          else if (StatSetClass.l)
            fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(StatSetClass.YoulaBlockL)));
        }
        return adverts1List;
      }
      catch (Exception ex)
      {
        fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(DateTime.Now.ToShortTimeString() + ": ================================================================================= " + Environment.NewLine + "Общий сбой в системе, возможно получили временный бан " + Environment.NewLine)));
        fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText("Если ошибка будет повторятся, скорее всего придётся выключить программу на 1-2 часа, либо нужно обновление программы. Ищите информацию в телеграмм канале Avtoringer" + Environment.NewLine)));
        fm1.LogBox.Invoke((Action) (() => fm1.LogBox.AppendText(ex.Message.ToString() + Environment.NewLine)));
        return new List<Adverts1>();
      }
    }

    private DateTime DateNormaliserYoulaAvto(string time)
    {
      try
      {
        Match match = Regex.Match(time, " (\\d+) ");
        int num = 0;
        if (match.Success)
          num = int.Parse(match.Groups[1].Value);
        if (time.Contains("сек"))
          return DateTime.Now.AddSeconds((double) -num);
        if (time.Contains("мин"))
          return DateTime.Now.AddMinutes((double) -num);
        if (time.Contains("час"))
          return DateTime.Now.AddHours((double) -num);
        if (time.Contains("дн") || time.Contains("ден"))
          return DateTime.Now.AddDays((double) -num);
        return time.Contains("мес") ? DateTime.Now.AddMonths(-num) : DateTime.Now;
      }
      catch
      {
        return DateTime.Now;
      }
    }

    private DateTime DateNormaliserAvito(string time)
    {
      DateTime dateTime1 = DateTime.Today;
      dateTime1 = dateTime1.Date;
      DateTime dateTime2 = dateTime1.AddDays(-2.0);
      Match match = Regex.Match(time, " (\\d+) ");
      int num = 0;
      if (match.Success)
        num = int.Parse(match.Groups[1].Value);
      if (time.Contains("сек"))
        return DateTime.Now.AddSeconds((double) -num);
      if (time.Contains("мин"))
        return DateTime.Now.AddMinutes((double) -num);
      if (time.Contains("час"))
        return DateTime.Now.AddHours((double) -num);
      if (time.Contains("дн") || time.Contains("ден"))
        return DateTime.Now.AddDays((double) -num);
      return time.Contains("мес") ? DateTime.Now.AddMonths(-num) : dateTime2;
    }

    private DateTime DateNormaliserYoula(string time)
    {
      DateTime dateTime1 = DateTime.Today;
      dateTime1 = dateTime1.Date;
      DateTime dateTime2 = dateTime1.AddDays(-2.0);
      foreach (Match match in new Regex(".*(сегодня|вчера) в (\\d+):(\\d+)", RegexOptions.IgnoreCase).Matches(time))
      {
        DateTime dateTime3;
        if (match.Groups[1].ToString() == "сегодня")
        {
          dateTime3 = DateTime.Today;
          dateTime2 = dateTime3.Date;
        }
        else if (match.Groups[1].ToString() == "вчера")
        {
          dateTime3 = DateTime.Today;
          dateTime3 = dateTime3.Date;
          dateTime2 = dateTime3.AddDays(-1.0);
        }
        else
        {
          dateTime3 = DateTime.Today;
          dateTime3 = dateTime3.Date;
          dateTime2 = dateTime3.AddDays(-2.0);
        }
        dateTime2 = dateTime2.AddHours(double.Parse(match.Groups[2].ToString()));
        dateTime2 = dateTime2.AddMinutes(double.Parse(match.Groups[3].ToString()));
      }
      return dateTime2;
    }

    private DateTime DateNormaliserOLX(string time)
    {
      DateTime dateTime1 = DateTime.Today;
      dateTime1 = dateTime1.Date;
      DateTime dateTime2 = dateTime1.AddDays(-2.0);
      time = time.Replace("\t", "");
      time = time.Replace("\n", "");
      foreach (Match match in new Regex("(Сегодня|Вчера) (\\d+):(\\d+)", RegexOptions.IgnoreCase).Matches(time))
      {
        DateTime dateTime3;
        if (match.Groups[1].ToString() == "Сегодня")
        {
          dateTime3 = DateTime.Today;
          dateTime2 = dateTime3.Date;
        }
        else if (match.Groups[1].ToString() == "Вчера")
        {
          dateTime3 = DateTime.Today;
          dateTime3 = dateTime3.Date;
          dateTime2 = dateTime3.AddDays(-1.0);
        }
        else
        {
          dateTime3 = DateTime.Today;
          dateTime3 = dateTime3.Date;
          dateTime2 = dateTime3.AddDays(-2.0);
        }
        dateTime2 = dateTime2.AddHours(double.Parse(match.Groups[2].ToString()));
        dateTime2 = dateTime2.AddMinutes(double.Parse(match.Groups[3].ToString()));
      }
      return dateTime2;
    }
  }
}
