// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.RelPropertyOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class RelPropertyOp : ScalarOp
  {
    private readonly RelProperty m_property;
    internal static readonly RelPropertyOp Pattern = new RelPropertyOp();

    private RelPropertyOp()
      : base(OpType.RelProperty)
    {
    }

    internal RelPropertyOp(TypeUsage type, RelProperty property)
      : base(OpType.RelProperty, type)
    {
      this.m_property = property;
    }

    internal override int Arity => 1;

    public RelProperty PropertyInfo => this.m_property;

    [DebuggerNonUserCode]
    internal override void Accept(BasicOpVisitor v, Node n) => v.Visit(this, n);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType>(BasicOpVisitorOfT<TResultType> v, Node n) => v.Visit(this, n);
  }
}
