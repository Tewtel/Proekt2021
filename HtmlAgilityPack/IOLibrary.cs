// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.IOLibrary
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System.IO;
using System.Runtime.InteropServices;

namespace HtmlAgilityPack
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  internal struct IOLibrary
  {
    internal static void CopyAlways(string source, string target)
    {
      if (!File.Exists(source))
        return;
      Directory.CreateDirectory(Path.GetDirectoryName(target));
      IOLibrary.MakeWritable(target);
      File.Copy(source, target, true);
    }

    internal static void MakeWritable(string path)
    {
      if (!File.Exists(path))
        return;
      File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
    }
  }
}
