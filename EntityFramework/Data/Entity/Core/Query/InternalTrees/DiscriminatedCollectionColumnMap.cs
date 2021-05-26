// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.DiscriminatedCollectionColumnMap
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Globalization;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class DiscriminatedCollectionColumnMap : CollectionColumnMap
  {
    private readonly SimpleColumnMap m_discriminator;
    private readonly object m_discriminatorValue;

    internal DiscriminatedCollectionColumnMap(
      TypeUsage type,
      string name,
      ColumnMap elementMap,
      SimpleColumnMap[] keys,
      SimpleColumnMap[] foreignKeys,
      SimpleColumnMap discriminator,
      object discriminatorValue)
      : base(type, name, elementMap, keys, foreignKeys)
    {
      this.m_discriminator = discriminator;
      this.m_discriminatorValue = discriminatorValue;
    }

    internal SimpleColumnMap Discriminator => this.m_discriminator;

    internal object DiscriminatorValue => this.m_discriminatorValue;

    [DebuggerNonUserCode]
    internal override void Accept<TArgType>(ColumnMapVisitor<TArgType> visitor, TArgType arg) => visitor.Visit(this, arg);

    [DebuggerNonUserCode]
    internal override TResultType Accept<TResultType, TArgType>(
      ColumnMapVisitorWithResults<TResultType, TArgType> visitor,
      TArgType arg)
    {
      return visitor.Visit(this, arg);
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "M{{{0}}}", (object) this.Element);
  }
}
