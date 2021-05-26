// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlConsoleListener
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;
using System.Diagnostics;

namespace HtmlAgilityPack
{
  internal class HtmlConsoleListener : TraceListener
  {
    public override void Write(string Message) => this.Write(Message, "");

    public override void Write(string Message, string Category) => Console.Write("T:" + Category + ": " + Message);

    public override void WriteLine(string Message) => this.Write(Message + "\n");

    public override void WriteLine(string Message, string Category) => this.Write(Message + "\n", Category);
  }
}
