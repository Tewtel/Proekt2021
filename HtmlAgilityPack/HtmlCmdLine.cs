// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlCmdLine
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;

namespace HtmlAgilityPack
{
  internal class HtmlCmdLine
  {
    internal static bool Help = false;

    static HtmlCmdLine() => HtmlCmdLine.ParseArgs();

    internal static string GetOption(string name, string def)
    {
      string ArgValue = def;
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      for (int index = 1; index < commandLineArgs.Length; ++index)
        HtmlCmdLine.GetStringArg(commandLineArgs[index], name, ref ArgValue);
      return ArgValue;
    }

    internal static string GetOption(int index, string def)
    {
      string ArgValue = def;
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      int num = 0;
      for (int index1 = 1; index1 < commandLineArgs.Length; ++index1)
      {
        if (HtmlCmdLine.GetStringArg(commandLineArgs[index1], ref ArgValue))
        {
          if (index == num)
            return ArgValue;
          ArgValue = def;
          ++num;
        }
      }
      return ArgValue;
    }

    internal static bool GetOption(string name, bool def)
    {
      bool ArgValue = def;
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      for (int index = 1; index < commandLineArgs.Length; ++index)
        HtmlCmdLine.GetBoolArg(commandLineArgs[index], name, ref ArgValue);
      return ArgValue;
    }

    internal static int GetOption(string name, int def)
    {
      int ArgValue = def;
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      for (int index = 1; index < commandLineArgs.Length; ++index)
        HtmlCmdLine.GetIntArg(commandLineArgs[index], name, ref ArgValue);
      return ArgValue;
    }

    private static void GetBoolArg(string Arg, string Name, ref bool ArgValue)
    {
      if (Arg.Length < Name.Length + 1 || '/' != Arg[0] && '-' != Arg[0] || !(Arg.Substring(1, Name.Length).ToLowerInvariant() == Name.ToLowerInvariant()))
        return;
      ArgValue = true;
    }

    private static void GetIntArg(string Arg, string Name, ref int ArgValue)
    {
      if (Arg.Length < Name.Length + 3 || '/' != Arg[0] && '-' != Arg[0])
        return;
      if (!(Arg.Substring(1, Name.Length).ToLowerInvariant() == Name.ToLowerInvariant()))
        return;
      try
      {
        ArgValue = Convert.ToInt32(Arg.Substring(Name.Length + 2, Arg.Length - Name.Length - 2));
      }
      catch
      {
      }
    }

    private static bool GetStringArg(string Arg, ref string ArgValue)
    {
      if ('/' == Arg[0] || '-' == Arg[0])
        return false;
      ArgValue = Arg;
      return true;
    }

    private static void GetStringArg(string Arg, string Name, ref string ArgValue)
    {
      if (Arg.Length < Name.Length + 3 || '/' != Arg[0] && '-' != Arg[0] || !(Arg.Substring(1, Name.Length).ToLowerInvariant() == Name.ToLowerInvariant()))
        return;
      ArgValue = Arg.Substring(Name.Length + 2, Arg.Length - Name.Length - 2);
    }

    private static void ParseArgs()
    {
      string[] commandLineArgs = Environment.GetCommandLineArgs();
      for (int index = 1; index < commandLineArgs.Length; ++index)
      {
        HtmlCmdLine.GetBoolArg(commandLineArgs[index], "?", ref HtmlCmdLine.Help);
        HtmlCmdLine.GetBoolArg(commandLineArgs[index], "h", ref HtmlCmdLine.Help);
        HtmlCmdLine.GetBoolArg(commandLineArgs[index], "help", ref HtmlCmdLine.Help);
      }
    }
  }
}
