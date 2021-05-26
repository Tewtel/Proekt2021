// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.BoolLiteral
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Common.Utils.Boolean;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal abstract class BoolLiteral : InternalBase
  {
    internal static readonly IEqualityComparer<BoolLiteral> EqualityComparer = (IEqualityComparer<BoolLiteral>) new BoolLiteral.BoolLiteralComparer();
    internal static readonly IEqualityComparer<BoolLiteral> EqualityIdentifierComparer = (IEqualityComparer<BoolLiteral>) new BoolLiteral.IdentifierComparer();

    internal static TermExpr<DomainConstraint<BoolLiteral, Constant>> MakeTermExpression(
      BoolLiteral literal,
      IEnumerable<Constant> domain,
      IEnumerable<Constant> range)
    {
      Set<Constant> domain1 = new Set<Constant>(domain, Constant.EqualityComparer);
      Set<Constant> range1 = new Set<Constant>(range, Constant.EqualityComparer);
      return BoolLiteral.MakeTermExpression(literal, domain1, range1);
    }

    internal static TermExpr<DomainConstraint<BoolLiteral, Constant>> MakeTermExpression(
      BoolLiteral literal,
      Set<Constant> domain,
      Set<Constant> range)
    {
      domain.MakeReadOnly();
      range.MakeReadOnly();
      return new TermExpr<DomainConstraint<BoolLiteral, Constant>>((IEqualityComparer<DomainConstraint<BoolLiteral, Constant>>) System.Collections.Generic.EqualityComparer<DomainConstraint<BoolLiteral, Constant>>.Default, new DomainConstraint<BoolLiteral, Constant>(new DomainVariable<BoolLiteral, Constant>(literal, domain, BoolLiteral.EqualityIdentifierComparer), range));
    }

    internal abstract BoolExpr<DomainConstraint<BoolLiteral, Constant>> FixRange(
      Set<Constant> range,
      MemberDomainMap memberDomainMap);

    internal abstract BoolExpr<DomainConstraint<BoolLiteral, Constant>> GetDomainBoolExpression(
      MemberDomainMap domainMap);

    internal abstract BoolLiteral RemapBool(Dictionary<MemberPath, MemberPath> remap);

    internal abstract void GetRequiredSlots(
      MemberProjectionIndex projectedSlotMap,
      bool[] requiredSlots);

    internal abstract StringBuilder AsEsql(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull);

    internal abstract DbExpression AsCqt(DbExpression row, bool skipIsNotNull);

    internal abstract StringBuilder AsUserString(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull);

    internal abstract StringBuilder AsNegatedUserString(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull);

    protected virtual bool IsIdentifierEqualTo(BoolLiteral right) => this.IsEqualTo(right);

    protected abstract bool IsEqualTo(BoolLiteral right);

    protected virtual int GetIdentifierHash() => this.GetHashCode();

    private sealed class BoolLiteralComparer : IEqualityComparer<BoolLiteral>
    {
      public bool Equals(BoolLiteral left, BoolLiteral right)
      {
        if (left == right)
          return true;
        return left != null && right != null && left.IsEqualTo(right);
      }

      public int GetHashCode(BoolLiteral literal) => literal.GetHashCode();
    }

    private sealed class IdentifierComparer : IEqualityComparer<BoolLiteral>
    {
      public bool Equals(BoolLiteral left, BoolLiteral right)
      {
        if (left == right)
          return true;
        return left != null && right != null && left.IsIdentifierEqualTo(right);
      }

      public int GetHashCode(BoolLiteral literal) => literal.GetIdentifierHash();
    }
  }
}
