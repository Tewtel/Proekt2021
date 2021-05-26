// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.DomainConstraint`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal class DomainConstraint<T_Variable, T_Element>
  {
    private readonly DomainVariable<T_Variable, T_Element> _variable;
    private readonly Set<T_Element> _range;
    private readonly int _hashCode;

    internal DomainConstraint(DomainVariable<T_Variable, T_Element> variable, Set<T_Element> range)
    {
      this._variable = variable;
      this._range = range.AsReadOnly();
      this._hashCode = this._variable.GetHashCode() ^ this._range.GetElementsHashCode();
    }

    internal DomainConstraint(DomainVariable<T_Variable, T_Element> variable, T_Element element)
      : this(variable, new Set<T_Element>((IEnumerable<T_Element>) new T_Element[1]
      {
        element
      }).MakeReadOnly())
    {
    }

    internal DomainVariable<T_Variable, T_Element> Variable => this._variable;

    internal Set<T_Element> Range => this._range;

    internal DomainConstraint<T_Variable, T_Element> InvertDomainConstraint() => new DomainConstraint<T_Variable, T_Element>(this._variable, this._variable.Domain.Difference((IEnumerable<T_Element>) this._range).AsReadOnly());

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is DomainConstraint<T_Variable, T_Element> domainConstraint && this._hashCode == domainConstraint._hashCode && this._range.SetEquals(domainConstraint._range) && this._variable.Equals((object) domainConstraint._variable);
    }

    public override int GetHashCode() => this._hashCode;

    public override string ToString() => StringUtil.FormatInvariant("{0} in [{1}]", (object) this._variable, (object) this._range);
  }
}
