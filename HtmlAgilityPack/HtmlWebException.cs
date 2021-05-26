// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlWebException
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;

namespace HtmlAgilityPack
{
  /// <summary>
  /// Represents an exception thrown by the HtmlWeb utility class.
  /// </summary>
  public class HtmlWebException : Exception
  {
    /// <summary>Creates an instance of the HtmlWebException.</summary>
    /// <param name="message">The exception's message.</param>
    public HtmlWebException(string message)
      : base(message)
    {
    }
  }
}
