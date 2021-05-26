// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.SimplePolymorphicColumnMap
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class SimplePolymorphicColumnMap : TypedColumnMap
  {
    private readonly SimpleColumnMap m_typeDiscriminator;
    private readonly Dictionary<object, TypedColumnMap> m_typedColumnMap;

    internal SimplePolymorphicColumnMap(
      TypeUsage type,
      string name,
      ColumnMap[] baseTypeColumns,
      SimpleColumnMap typeDiscriminator,
      Dictionary<object, TypedColumnMap> typeChoices)
      : base(type, name, baseTypeColumns)
    {
      this.m_typedColumnMap = typeChoices;
      this.m_typeDiscriminator = typeDiscriminator;
    }

    internal SimpleColumnMap TypeDiscriminator => this.m_typeDiscriminator;

    internal Dictionary<object, TypedColumnMap> TypeChoices => this.m_typedColumnMap;

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
      StringBuilder stringBuilder = new StringBuilder();
      string str = string.Empty;
      stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "P{{TypeId={0}, ", (object) this.TypeDiscriminator);
      foreach (KeyValuePair<object, TypedColumnMap> typeChoice in this.TypeChoices)
      {
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}({1},{2})", (object) str, typeChoice.Key, (object) typeChoice.Value);
        str = ",";
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }
  }
}
