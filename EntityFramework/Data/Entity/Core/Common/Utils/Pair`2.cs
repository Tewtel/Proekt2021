// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Pair`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Text;

namespace System.Data.Entity.Core.Common.Utils
{
  internal class Pair<TFirst, TSecond> : InternalBase
  {
    private readonly TFirst first;
    private readonly TSecond second;

    internal Pair(TFirst first, TSecond second)
    {
      this.first = first;
      this.second = second;
    }

    internal TFirst First => this.first;

    internal TSecond Second => this.second;

    public override int GetHashCode() => this.first.GetHashCode() << 5 ^ this.second.GetHashCode();

    public bool Equals(Pair<TFirst, TSecond> other) => this.first.Equals((object) other.first) && this.second.Equals((object) other.second);

    public override bool Equals(object other) => other is Pair<TFirst, TSecond> other1 && this.Equals(other1);

    internal override void ToCompactString(StringBuilder builder)
    {
      builder.Append("<");
      builder.Append((object) this.first);
      builder.Append(", " + this.second?.ToString());
      builder.Append(">");
    }

    internal class PairComparer : IEqualityComparer<Pair<TFirst, TSecond>>
    {
      internal static readonly Pair<TFirst, TSecond>.PairComparer Instance = new Pair<TFirst, TSecond>.PairComparer();
      private static readonly EqualityComparer<TFirst> _firstComparer = EqualityComparer<TFirst>.Default;
      private static readonly EqualityComparer<TSecond> _secondComparer = EqualityComparer<TSecond>.Default;

      private PairComparer()
      {
      }

      public bool Equals(Pair<TFirst, TSecond> x, Pair<TFirst, TSecond> y) => Pair<TFirst, TSecond>.PairComparer._firstComparer.Equals(x.First, y.First) && Pair<TFirst, TSecond>.PairComparer._secondComparer.Equals(x.Second, y.Second);

      public int GetHashCode(Pair<TFirst, TSecond> source) => source.GetHashCode();
    }
  }
}
