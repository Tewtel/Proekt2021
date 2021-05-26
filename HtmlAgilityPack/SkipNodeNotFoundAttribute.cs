// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.SkipNodeNotFoundAttribute
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;

namespace HtmlAgilityPack
{
  /// <summary>
  /// Tagging a property with this Attribute make Encapsulator to ignore that property if it causes an error.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
  public sealed class SkipNodeNotFoundAttribute : Attribute
  {
  }
}
