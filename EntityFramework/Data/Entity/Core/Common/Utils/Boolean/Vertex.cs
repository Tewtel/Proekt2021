// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.Vertex
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Diagnostics;
using System.Globalization;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal sealed class Vertex : IEquatable<Vertex>
  {
    internal static readonly Vertex One = new Vertex();
    internal static readonly Vertex Zero = new Vertex();
    internal readonly int Variable;
    internal readonly Vertex[] Children;

    private Vertex()
    {
      this.Variable = int.MaxValue;
      this.Children = new Vertex[0];
    }

    internal Vertex(int variable, Vertex[] children)
    {
      this.Variable = variable < int.MaxValue ? variable : throw EntityUtil.InternalError(EntityUtil.InternalErrorCode.BoolExprAssert, 0, (object) "exceeded number of supported variables");
      this.Children = children;
    }

    [Conditional("DEBUG")]
    private static void AssertConstructorArgumentsValid(int variable, Vertex[] children)
    {
      foreach (Vertex child in children)
        ;
    }

    internal bool IsOne() => Vertex.One == this;

    internal bool IsZero() => Vertex.Zero == this;

    internal bool IsSink() => this.Variable == int.MaxValue;

    public bool Equals(Vertex other) => this == other;

    public override bool Equals(object obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString()
    {
      if (this.IsOne())
        return "_1_";
      if (this.IsZero())
        return "_0_";
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "<{0}, {1}>", (object) this.Variable, (object) StringUtil.ToCommaSeparatedString((IEnumerable) this.Children));
    }
  }
}
