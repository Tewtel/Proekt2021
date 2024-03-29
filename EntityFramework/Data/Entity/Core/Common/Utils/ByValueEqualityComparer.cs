﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.ByValueEqualityComparer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.Utils
{
  internal sealed class ByValueEqualityComparer : IEqualityComparer<object>
  {
    internal static readonly ByValueEqualityComparer Default = new ByValueEqualityComparer();

    private ByValueEqualityComparer()
    {
    }

    public bool Equals(object x, object y)
    {
      if (object.Equals(x, y))
        return true;
      byte[] first = x as byte[];
      byte[] second = y as byte[];
      return first != null && second != null && ByValueEqualityComparer.CompareBinaryValues(first, second);
    }

    public int GetHashCode(object obj)
    {
      if (obj == null)
        return 0;
      return obj is byte[] bytes ? ByValueEqualityComparer.ComputeBinaryHashCode(bytes) : obj.GetHashCode();
    }

    internal static int ComputeBinaryHashCode(byte[] bytes)
    {
      int num = 0;
      int index1 = 0;
      for (int index2 = Math.Min(bytes.Length, 7); index1 < index2; ++index1)
        num = num << 5 ^ (int) bytes[index1];
      return num;
    }

    internal static bool CompareBinaryValues(byte[] first, byte[] second)
    {
      if (first.Length != second.Length)
        return false;
      for (int index = 0; index < first.Length; ++index)
      {
        if ((int) first[index] != (int) second[index])
          return false;
      }
      return true;
    }
  }
}
