// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.TrailingSpaceComparer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.Utils
{
  internal class TrailingSpaceComparer : IEqualityComparer<object>
  {
    internal static readonly TrailingSpaceComparer Instance = new TrailingSpaceComparer();
    private static readonly IEqualityComparer<object> _template = (IEqualityComparer<object>) EqualityComparer<object>.Default;

    private TrailingSpaceComparer()
    {
    }

    bool IEqualityComparer<object>.Equals(object x, object y) => x is string x1 && y is string y1 ? TrailingSpaceStringComparer.Instance.Equals(x1, y1) : TrailingSpaceComparer._template.Equals(x, y);

    int IEqualityComparer<object>.GetHashCode(object obj) => obj is string str ? TrailingSpaceStringComparer.Instance.GetHashCode(str) : TrailingSpaceComparer._template.GetHashCode(obj);
  }
}
