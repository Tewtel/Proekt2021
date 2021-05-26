// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.ScalarRestriction
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Common.Utils.Boolean;
using System.Linq;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal class ScalarRestriction : MemberRestriction
  {
    internal ScalarRestriction(MemberPath member, Constant value)
      : base(new MemberProjectedSlot(member), value)
    {
    }

    internal ScalarRestriction(
      MemberPath member,
      IEnumerable<Constant> values,
      IEnumerable<Constant> possibleValues)
      : base(new MemberProjectedSlot(member), values, possibleValues)
    {
    }

    internal ScalarRestriction(MemberProjectedSlot slot, Domain domain)
      : base(slot, domain)
    {
    }

    internal override BoolExpr<DomainConstraint<BoolLiteral, Constant>> FixRange(
      Set<Constant> range,
      MemberDomainMap memberDomainMap)
    {
      IEnumerable<Constant> domain = memberDomainMap.GetDomain(this.RestrictedMemberSlot.MemberPath);
      return new ScalarRestriction(this.RestrictedMemberSlot, new Domain((IEnumerable<Constant>) range, domain)).GetDomainBoolExpression(memberDomainMap);
    }

    internal override BoolLiteral RemapBool(Dictionary<MemberPath, MemberPath> remap) => (BoolLiteral) new ScalarRestriction(this.RestrictedMemberSlot.RemapSlot(remap), this.Domain);

    internal override MemberRestriction CreateCompleteMemberRestriction(
      IEnumerable<Constant> possibleValues)
    {
      return (MemberRestriction) new ScalarRestriction(this.RestrictedMemberSlot, new Domain(this.Domain.Values, possibleValues));
    }

    internal override StringBuilder AsEsql(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull)
    {
      return this.ToStringHelper(builder, blockAlias, skipIsNotNull, false);
    }

    internal override DbExpression AsCqt(DbExpression row, bool skipIsNotNull)
    {
      DbExpression cqt = (DbExpression) null;
      this.AsCql((Action<NegatedConstant, IEnumerable<Constant>>) ((negated, domainValues) => cqt = negated.AsCqt(row, domainValues, this.RestrictedMemberSlot.MemberPath, skipIsNotNull)), (Action<Set<Constant>>) (domainValues =>
      {
        cqt = this.RestrictedMemberSlot.MemberPath.AsCqt(row);
        if (domainValues.Count == 1)
          cqt = (DbExpression) cqt.Equal(domainValues.Single<Constant>().AsCqt(row, this.RestrictedMemberSlot.MemberPath));
        else
          cqt = Helpers.BuildBalancedTreeInPlace<DbExpression>((IList<DbExpression>) domainValues.Select<Constant, DbExpression>((Func<Constant, DbExpression>) (c => (DbExpression) cqt.Equal(c.AsCqt(row, this.RestrictedMemberSlot.MemberPath)))).ToList<DbExpression>(), (Func<DbExpression, DbExpression, DbExpression>) ((prev, next) => (DbExpression) prev.Or(next)));
      }), (Action) (() =>
      {
        DbExpression right = (DbExpression) this.RestrictedMemberSlot.MemberPath.AsCqt(row).IsNull().Not();
        cqt = cqt != null ? (DbExpression) cqt.And(right) : right;
      }), (Action) (() =>
      {
        DbExpression left = (DbExpression) this.RestrictedMemberSlot.MemberPath.AsCqt(row).IsNull();
        cqt = cqt != null ? (DbExpression) left.Or(cqt) : left;
      }), skipIsNotNull);
      return cqt;
    }

    internal override StringBuilder AsUserString(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull)
    {
      return this.ToStringHelper(builder, blockAlias, skipIsNotNull, true);
    }

    private StringBuilder ToStringHelper(
      StringBuilder inputBuilder,
      string blockAlias,
      bool skipIsNotNull,
      bool userString)
    {
      StringBuilder builder = new StringBuilder();
      this.AsCql((Action<NegatedConstant, IEnumerable<Constant>>) ((negated, domainValues) =>
      {
        if (userString)
          negated.AsUserString(builder, blockAlias, domainValues, this.RestrictedMemberSlot.MemberPath, skipIsNotNull);
        else
          negated.AsEsql(builder, blockAlias, domainValues, this.RestrictedMemberSlot.MemberPath, skipIsNotNull);
      }), (Action<Set<Constant>>) (domainValues =>
      {
        this.RestrictedMemberSlot.MemberPath.AsEsql(builder, blockAlias);
        if (domainValues.Count == 1)
        {
          builder.Append(" = ");
          if (userString)
            domainValues.Single<Constant>().ToCompactString(builder);
          else
            domainValues.Single<Constant>().AsEsql(builder, this.RestrictedMemberSlot.MemberPath, blockAlias);
        }
        else
        {
          builder.Append(" IN {");
          bool flag = true;
          foreach (Constant domainValue in domainValues)
          {
            if (!flag)
              builder.Append(", ");
            if (userString)
              domainValue.ToCompactString(builder);
            else
              domainValue.AsEsql(builder, this.RestrictedMemberSlot.MemberPath, blockAlias);
            flag = false;
          }
          builder.Append('}');
        }
      }), (Action) (() =>
      {
        int num = builder.Length == 0 ? 1 : 0;
        builder.Insert(0, '(');
        if (num == 0)
          builder.Append(" AND ");
        if (userString)
        {
          this.RestrictedMemberSlot.MemberPath.ToCompactString(builder, System.Data.Entity.Resources.Strings.ViewGen_EntityInstanceToken);
          builder.Append(" is not NULL)");
        }
        else
        {
          this.RestrictedMemberSlot.MemberPath.AsEsql(builder, blockAlias);
          builder.Append(" IS NOT NULL)");
        }
      }), (Action) (() =>
      {
        int num = builder.Length == 0 ? 1 : 0;
        StringBuilder stringBuilder = new StringBuilder();
        if (num == 0)
          stringBuilder.Append('(');
        if (userString)
        {
          this.RestrictedMemberSlot.MemberPath.ToCompactString(stringBuilder, blockAlias);
          stringBuilder.Append(" is NULL");
        }
        else
        {
          this.RestrictedMemberSlot.MemberPath.AsEsql(stringBuilder, blockAlias);
          stringBuilder.Append(" IS NULL");
        }
        if (num == 0)
          stringBuilder.Append(" OR ");
        builder.Insert(0, stringBuilder.ToString());
        if (num != 0)
          return;
        builder.Append(')');
      }), skipIsNotNull);
      inputBuilder.Append((object) builder);
      return inputBuilder;
    }

    private void AsCql(
      Action<NegatedConstant, IEnumerable<Constant>> negatedConstantAsCql,
      Action<Set<Constant>> varInDomain,
      Action varIsNotNull,
      Action varIsNull,
      bool skipIsNotNull)
    {
      NegatedConstant negatedConstant = (NegatedConstant) this.Domain.Values.FirstOrDefault<Constant>((Func<Constant, bool>) (c => c is NegatedConstant));
      if (negatedConstant != null)
      {
        negatedConstantAsCql(negatedConstant, this.Domain.Values);
      }
      else
      {
        Set<Constant> set = new Set<Constant>(this.Domain.Values, Constant.EqualityComparer);
        bool flag = false;
        if (set.Contains(Constant.Null))
        {
          flag = true;
          set.Remove(Constant.Null);
        }
        if (set.Contains(Constant.Undefined))
        {
          flag = true;
          set.Remove(Constant.Undefined);
        }
        int num = skipIsNotNull ? 0 : (this.RestrictedMemberSlot.MemberPath.IsNullable ? 1 : 0);
        if (set.Count > 0)
          varInDomain(set);
        if (num != 0)
          varIsNotNull();
        if (!flag)
          return;
        varIsNull();
      }
    }

    internal override void ToCompactString(StringBuilder builder)
    {
      this.RestrictedMemberSlot.ToCompactString(builder);
      builder.Append(" IN (");
      StringUtil.ToCommaSeparatedStringSorted(builder, (IEnumerable) this.Domain.Values);
      builder.Append(")");
    }
  }
}
