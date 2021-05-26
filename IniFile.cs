// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.IniFile
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AvitoAvtoRinger
{
  internal class IniFile
  {
    private string Path;

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(
      string Section,
      string Key,
      string Value,
      string FilePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(
      string Section,
      string Key,
      string Default,
      StringBuilder RetVal,
      int Size,
      string FilePath);

    public IniFile(string IniPath)
    {
      try
      {
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      DirectoryInfo directoryInfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Avtoringer");
      if (!directoryInfo.Exists)
        directoryInfo.Create();
      this.Path = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Avtoringer\\" + IniPath).ToString();
      if (File.Exists(this.Path))
        return;
      File.Create(this.Path);
    }

    public string ReadINI(string Section, string Key)
    {
      try
      {
        StringBuilder RetVal = new StringBuilder(65500);
        IniFile.GetPrivateProfileString(Section, Key, "", RetVal, 65500, this.Path);
        return RetVal.ToString();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        return "0";
      }
    }

    public void WriteINI(string Section, string Key, string Value)
    {
      try
      {
        IniFile.WritePrivateProfileString(Section, Key, Value, this.Path);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    public void DeleteKey(string Key, string Section = null)
    {
      try
      {
        this.WriteINI(Section, Key, (string) null);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    public void DeleteSection(string Section = null)
    {
      try
      {
        this.WriteINI(Section, (string) null, (string) null);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    public bool KeyExists(string Section, string Key)
    {
      try
      {
        return this.ReadINI(Section, Key).Length > 0;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
        return false;
      }
    }
  }
}
