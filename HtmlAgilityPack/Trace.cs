// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.Trace
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

namespace HtmlAgilityPack
{
  internal class Trace
  {
    internal static Trace _current;

    internal static Trace Current
    {
      get
      {
        if (Trace._current == null)
          Trace._current = new Trace();
        return Trace._current;
      }
    }

    private void WriteLineIntern(string message, string category)
    {
    }

    public static void WriteLine(string message, string category) => Trace.Current.WriteLineIntern(message, category);
  }
}
