// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.IsOfOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class IsOfOp : ScalarOp
  {
    private readonly TypeUsage m_isOfType;
    private readonly bool m_isOfOnly;
    internal static readonly IsOfOp Pattern = new IsOfOp();

    internal IsOfOp(TypeUsage isOfType, bool isOfOnly, TypeUsage type)
      : base(OpType.IsOf, type)
    {
      this.m_isOfType = isOfType;
      this.m_isOfOnly = isOfOnly;
    }

    private IsOfOp()
      : base(OpType.IsOf)
    {
    }

    internal override int Arity => 1;

    internal TypeUsage IsOfType => this.m_isOfType;

    internal bool IsOfOnly => this.m_isOfOnly;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
