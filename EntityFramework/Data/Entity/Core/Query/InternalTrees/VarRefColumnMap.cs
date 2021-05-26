// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.VarRefColumnMap
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Globalization;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class VarRefColumnMap : SimpleColumnMap
  {
    private readonly Var m_var;

    internal Var Var => this.m_var;

    internal VarRefColumnMap(TypeUsage type, string name, Var v)
      : base(type, name)
    {
      this.m_var = v;
    }

    internal VarRefColumnMap(Var v)
      : this(v.Type, (string) null, v)
    {
    }

    [DebuggerNonUserCode]
    internal override void Accept<TArgType>(ColumnMapVisitor<TArgType> visitor, TArgType arg) => visitor.Visit(this, arg);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType, TArgType>(
      ColumnMapVisitorWithResults<TResultType, TArgType> visitor,
      TArgType arg)
    {
      return visitor.Visit(this, arg);
    }

    public override string ToString()
    {
      if (this.IsNamed)
        return this.Name;
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}", (object) this.m_var.Id);
    }
  }
}
