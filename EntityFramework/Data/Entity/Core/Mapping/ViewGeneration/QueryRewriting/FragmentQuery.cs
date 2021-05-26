// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting.FragmentQuery
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;
using System.Globalization;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting
{
  internal class FragmentQuery : ITileQuery
  {
    private readonly BoolExpression m_fromVariable;
    private readonly string m_label;
    private readonly HashSet<MemberPath> m_attributes;
    private readonly BoolExpression m_condition;

    public HashSet<MemberPath> Attributes => this.m_attributes;

    public BoolExpression Condition => this.m_condition;

    public static FragmentQuery Create(
      BoolExpression fromVariable,
      CellQuery cellQuery)
    {
      BoolExpression condition = cellQuery.WhereClause.MakeCopy();
      condition.ExpensiveSimplify();
      return new FragmentQuery((string) null, fromVariable, (IEnumerable<MemberPath>) new HashSet<MemberPath>(cellQuery.GetProjectedMembers()), condition);
    }

    public static FragmentQuery Create(
      string label,
      RoleBoolean roleBoolean,
      CellQuery cellQuery)
    {
      BoolExpression condition = BoolExpression.CreateAnd(cellQuery.WhereClause.Create((BoolLiteral) roleBoolean), cellQuery.WhereClause).MakeCopy();
      condition.ExpensiveSimplify();
      return new FragmentQuery(label, (BoolExpression) null, (IEnumerable<MemberPath>) new HashSet<MemberPath>(), condition);
    }

    public static FragmentQuery Create(
      IEnumerable<MemberPath> attrs,
      BoolExpression whereClause)
    {
      return new FragmentQuery((string) null, (BoolExpression) null, attrs, whereClause);
    }

    public static FragmentQuery Create(BoolExpression whereClause) => new FragmentQuery((string) null, (BoolExpression) null, (IEnumerable<MemberPath>) new MemberPath[0], whereClause);

    internal FragmentQuery(
      string label,
      BoolExpression fromVariable,
      IEnumerable<MemberPath> attrs,
      BoolExpression condition)
    {
      this.m_label = label;
      this.m_fromVariable = fromVariable;
      this.m_condition = condition;
      this.m_attributes = new HashSet<MemberPath>(attrs);
    }

    public BoolExpression FromVariable => this.m_fromVariable;

    public string Description
    {
      get
      {
        string label = this.m_label;
        if (label == null && this.m_fromVariable != null)
          label = this.m_fromVariable.ToString();
        return label;
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (MemberPath attribute in this.Attributes)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.Append(',');
        stringBuilder.Append((object) attribute);
      }
      return this.Description != null && this.Description != stringBuilder.ToString() ? string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}: [{1} where {2}]", (object) this.Description, (object) stringBuilder, (object) this.Condition) : string.Format((IFormatProvider) CultureInfo.InvariantCulture, "[{0} where {1}]", (object) stringBuilder, (object) this.Condition);
    }

    internal static BoolExpression CreateMemberCondition(
      MemberPath path,
      Constant domainValue,
      MemberDomainMap domainMap)
    {
      return domainValue is TypeConstant ? BoolExpression.CreateLiteral((BoolLiteral) new TypeRestriction(new MemberProjectedSlot(path), new Domain(domainValue, domainMap.GetDomain(path))), domainMap) : BoolExpression.CreateLiteral((BoolLiteral) new ScalarRestriction(new MemberProjectedSlot(path), new Domain(domainValue, domainMap.GetDomain(path))), domainMap);
    }

    internal static IEqualityComparer<FragmentQuery> GetEqualityComparer(
      FragmentQueryProcessor qp)
    {
      return (IEqualityComparer<FragmentQuery>) new FragmentQuery.FragmentQueryEqualityComparer(qp);
    }

    private class FragmentQueryEqualityComparer : IEqualityComparer<FragmentQuery>
    {
      private readonly FragmentQueryProcessor _qp;

      internal FragmentQueryEqualityComparer(FragmentQueryProcessor qp) => this._qp = qp;

      public bool Equals(FragmentQuery x, FragmentQuery y) => x.Attributes.SetEquals((IEnumerable<MemberPath>) y.Attributes) && this._qp.IsEquivalentTo(x, y);

      public int GetHashCode(FragmentQuery q)
      {
        int num1 = 0;
        foreach (MemberPath attribute in q.Attributes)
          num1 ^= MemberPath.EqualityComparer.GetHashCode(attribute);
        int num2 = 0;
        int num3 = 0;
        foreach (MemberRestriction memberRestriction in q.Condition.MemberRestrictions)
        {
          num2 ^= MemberPath.EqualityComparer.GetHashCode(memberRestriction.RestrictedMemberSlot.MemberPath);
          foreach (Constant constant in memberRestriction.Domain.Values)
            num3 ^= Constant.EqualityComparer.GetHashCode(constant);
        }
        return num1 * 13 + num2 * 7 + num3;
      }
    }
  }
}
