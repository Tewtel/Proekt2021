// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.ItemComparer
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AvitoAvtoRinger
{
  internal class ItemComparer : IComparer
  {
    private int columnIndex = 0;
    private Form1 mainForm;
    private bool sortAscending = false;

    public ItemComparer(Form1 fm) => this.mainForm = fm;

    public int ColumnIndex
    {
      set
      {
        if (this.columnIndex == value)
        {
          this.sortAscending = !this.sortAscending;
        }
        else
        {
          this.columnIndex = value;
          this.sortAscending = true;
        }
        try
        {
          if (this.columnIndex == 0)
          {
            if (this.sortAscending)
              this.mainForm.ListVirtual.Sort((Comparison<Adverts1>) ((vc1, vc2) => vc2.date.CompareTo(vc1.date)));
            else
              this.mainForm.ListVirtual.Sort((Comparison<Adverts1>) ((vc1, vc2) => vc1.date.CompareTo(vc2.date)));
          }
          else if (this.columnIndex == 1)
          {
            if (this.sortAscending)
              this.mainForm.ListVirtual.Sort((Comparison<Adverts1>) ((vc1, vc2) => vc2.name.CompareTo(vc1.name)));
            else
              this.mainForm.ListVirtual.Sort((Comparison<Adverts1>) ((vc1, vc2) => vc1.name.CompareTo(vc2.name)));
          }
          else
          {
            if (this.columnIndex != 2)
              return;
            if (this.sortAscending)
              this.mainForm.ListVirtual.Sort((Comparison<Adverts1>) ((vc1, vc2) =>
              {
                Match match1 = Regex.Match(vc1.price.Replace(" ", ""), "[0-9]*");
                string s1 = !match1.Success ? "0" : match1.Value;
                Match match2 = Regex.Match(vc2.price.Replace(" ", ""), "[0-9]*");
                string s2 = !match2.Success ? "0" : match2.Value;
                if (s1 == "")
                  s1 = "0";
                if (s2 == "")
                  s2 = "0";
                return int.Parse(s2).CompareTo(int.Parse(s1));
              }));
            else
              this.mainForm.ListVirtual.Sort((Comparison<Adverts1>) ((vc1, vc2) =>
              {
                Match match1 = Regex.Match(vc1.price.Replace(" ", ""), "[0-9]*");
                string s1 = !match1.Success ? "0" : match1.Value;
                Match match2 = Regex.Match(vc2.price.Replace(" ", ""), "[0-9]*");
                string s2 = !match2.Success ? "0" : match2.Value;
                if (s1 == "")
                  s1 = "0";
                if (s2 == "")
                  s2 = "0";
                return int.Parse(s1).CompareTo(int.Parse(s2));
              }));
          }
        }
        catch (Exception ex)
        {
          this.mainForm.LogBox.AppendText("Ошибка при сортировке " + Environment.NewLine + ex.ToString() + Environment.NewLine);
        }
      }
    }

    public int Compare(object x, object y)
    {
      try
      {
        if (this.columnIndex == 0)
          return (DateTime.Parse(((ListViewItem) x).SubItems[this.columnIndex].Text) > DateTime.Parse(((ListViewItem) y).SubItems[this.columnIndex].Text) ? 1 : -1) * (this.sortAscending ? 1 : -1);
        if (this.columnIndex != 2)
          return string.Compare(((ListViewItem) x).SubItems[this.columnIndex].Text, ((ListViewItem) y).SubItems[this.columnIndex].Text) * (this.sortAscending ? 1 : -1);
        string str1 = ((ListViewItem) x).SubItems[this.columnIndex].Text;
        string str2 = ((ListViewItem) y).SubItems[this.columnIndex].Text;
        if (str1.Contains("Цена не указана"))
          str1 = "0 руб.";
        if (str2.Contains("Цена не указана"))
          str2 = "0 руб.";
        return (int.Parse(str1.Substring(0, str1.IndexOf(" руб.")).Replace(" ", "")) > int.Parse(str2.Substring(0, str2.IndexOf(" руб.")).Replace(" ", "")) ? 1 : -1) * (this.sortAscending ? 1 : -1);
      }
      catch (Exception ex)
      {
        this.mainForm.LogBox.AppendText(DateTime.Now.ToShortTimeString() + " Ошибка сортировки в listview " + ex.ToString() + Environment.NewLine);
        return 0;
      }
    }
  }
}
