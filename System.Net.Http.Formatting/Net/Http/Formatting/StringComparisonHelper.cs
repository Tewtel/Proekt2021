// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.StringComparisonHelper
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Web.Http;

namespace System.Net.Http.Formatting
{
  internal static class StringComparisonHelper
  {
    public static bool IsDefined(StringComparison value) => value == StringComparison.CurrentCulture || value == StringComparison.CurrentCultureIgnoreCase || (value == StringComparison.InvariantCulture || value == StringComparison.InvariantCultureIgnoreCase) || value == StringComparison.Ordinal || value == StringComparison.OrdinalIgnoreCase;

    public static void Validate(StringComparison value, string parameterName)
    {
      if (!StringComparisonHelper.IsDefined(value))
        throw Error.InvalidEnumArgument(parameterName, (int) value, typeof (StringComparison));
    }
  }
}
