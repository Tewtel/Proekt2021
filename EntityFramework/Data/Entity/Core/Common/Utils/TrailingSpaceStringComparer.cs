// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.TrailingSpaceStringComparer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.Utils
{
  internal class TrailingSpaceStringComparer : IEqualityComparer<string>
  {
    internal static readonly TrailingSpaceStringComparer Instance = new TrailingSpaceStringComparer();

    private TrailingSpaceStringComparer()
    {
    }

    public bool Equals(string x, string y) => StringComparer.OrdinalIgnoreCase.Equals(TrailingSpaceStringComparer.NormalizeString(x), TrailingSpaceStringComparer.NormalizeString(y));

    public int GetHashCode(string obj) => StringComparer.OrdinalIgnoreCase.GetHashCode(TrailingSpaceStringComparer.NormalizeString(obj));

    internal static string NormalizeString(string value)
    {
      if (value == null || !value.EndsWith(" ", StringComparison.Ordinal))
        return value;
      return value.TrimEnd(' ');
    }
  }
}
