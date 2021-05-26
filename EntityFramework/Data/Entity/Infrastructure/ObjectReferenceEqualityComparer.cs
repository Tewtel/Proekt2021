// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.ObjectReferenceEqualityComparer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>Compares objects using reference equality.</summary>
  [Serializable]
  public sealed class ObjectReferenceEqualityComparer : IEqualityComparer<object>
  {
    private static readonly ObjectReferenceEqualityComparer _default = new ObjectReferenceEqualityComparer();

    /// <summary>Gets the default instance.</summary>
    public static ObjectReferenceEqualityComparer Default => ObjectReferenceEqualityComparer._default;

    bool IEqualityComparer<object>.Equals(object x, object y) => x == y;

    int IEqualityComparer<object>.GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
  }
}
