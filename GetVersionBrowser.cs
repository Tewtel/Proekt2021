// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.GetVersionBrowser
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;

namespace AvitoAvtoRinger
{
  public class GetVersionBrowser
  {
    public static List<Browser> GetBrowsers()
    {
      RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Clients\\StartMenuInternet") ?? Registry.LocalMachine.OpenSubKey("SOFTWARE\\Clients\\StartMenuInternet");
      if (registryKey1 == null)
        return (List<Browser>) null;
      string[] subKeyNames = registryKey1.GetSubKeyNames();
      List<Browser> browserList = new List<Browser>();
      foreach (string name in subKeyNames)
      {
        Browser browser = new Browser();
        RegistryKey registryKey2 = registryKey1.OpenSubKey(name);
        if (registryKey2 != null)
        {
          browser.NameBrowser = (string) registryKey2.GetValue((string) null);
          RegistryKey registryKey3 = registryKey2.OpenSubKey("shell\\open\\command");
          browser.PathBrowser = registryKey3 != null ? registryKey3.GetValue((string) null).ToString().StripQuotes() : (string) null;
          RegistryKey registryKey4 = registryKey2.OpenSubKey("DefaultIcon");
          browser.IconPath = registryKey4 != null ? registryKey4.GetValue((string) null).ToString().StripQuotes() : (string) null;
        }
        browserList.Add(browser);
        browser.Version = browser.PathBrowser != null ? FileVersionInfo.GetVersionInfo(browser.PathBrowser).FileVersion : "unknown";
      }
      return browserList;
    }
  }
}
