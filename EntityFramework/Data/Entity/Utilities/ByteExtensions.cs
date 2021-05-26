// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.ByteExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Data.Entity.Utilities
{
  internal static class ByteExtensions
  {
    public static string ToHexString(this IEnumerable<byte> bytes)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in bytes)
        stringBuilder.Append(num.ToString("X2", (IFormatProvider) CultureInfo.InvariantCulture));
      return stringBuilder.ToString();
    }
  }
}
