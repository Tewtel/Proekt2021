// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.NavigateOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class NavigateOp : ScalarOp
  {
    private readonly RelProperty m_property;
    internal static readonly NavigateOp Pattern = new NavigateOp();

    internal NavigateOp(TypeUsage type, RelProperty relProperty)
      : base(OpType.Navigate, type)
    {
      this.m_property = relProperty;
    }

    private NavigateOp()
      : base(OpType.Navigate)
    {
    }

    internal override int Arity => 1;

    internal RelProperty RelProperty => this.m_property;

    internal RelationshipType Relationship => this.m_property.Relationship;

    internal RelationshipEndMember FromEnd => this.m_property.FromEnd;

    internal RelationshipEndMember ToEnd => this.m_property.ToEnd;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
