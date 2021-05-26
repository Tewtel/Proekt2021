// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.ByteExtensions
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Data.Entity.SqlServer.Utilities
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
