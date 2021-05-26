// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.TypeNameStringComparer
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;

namespace System.Data.SQLite
{
  internal sealed class TypeNameStringComparer : IEqualityComparer<string>, IComparer<string>
  {
    public bool Equals(string left, string right) => string.Equals(left, right, StringComparison.OrdinalIgnoreCase);

    public int GetHashCode(string value) => value != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(value) : throw new ArgumentNullException(nameof (value));

    public int Compare(string x, string y)
    {
      if (x == null && y == null)
        return 0;
      if (x == null)
        return -1;
      return y == null ? 1 : x.CompareTo(y);
    }
  }
}
