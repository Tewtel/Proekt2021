// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.EntityColumnMap
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Globalization;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class EntityColumnMap : TypedColumnMap
  {
    private readonly EntityIdentity m_entityIdentity;

    internal EntityColumnMap(
      TypeUsage type,
      string name,
      ColumnMap[] properties,
      EntityIdentity entityIdentity)
      : base(type, name, properties)
    {
      this.m_entityIdentity = entityIdentity;
    }

    internal EntityIdentity EntityIdentity => this.m_entityIdentity;

    [DebuggerNonUserCode]
    internal override void Accept<TArgType>(ColumnMapVisitor<TArgType> visitor, TArgType arg) => visitor.Visit(this, arg);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType, TArgType>(
      ColumnMapVisitorWithResults<TResultType, TArgType> visitor,
      TArgType arg)
    {
      return visitor.Visit(this, arg);
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "E{0}", (object) base.ToString());
  }
}
