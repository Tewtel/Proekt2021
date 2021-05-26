// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.MultipleDiscriminatorPolymorphicColumnMap
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class MultipleDiscriminatorPolymorphicColumnMap : TypedColumnMap
  {
    private readonly SimpleColumnMap[] m_typeDiscriminators;
    private readonly Dictionary<EntityType, TypedColumnMap> m_typeChoices;
    private readonly Func<object[], EntityType> m_discriminate;

    internal MultipleDiscriminatorPolymorphicColumnMap(
      TypeUsage type,
      string name,
      ColumnMap[] baseTypeColumns,
      SimpleColumnMap[] typeDiscriminators,
      Dictionary<EntityType, TypedColumnMap> typeChoices,
      Func<object[], EntityType> discriminate)
      : base(type, name, baseTypeColumns)
    {
      this.m_typeDiscriminators = typeDiscriminators;
      this.m_typeChoices = typeChoices;
      this.m_discriminate = discriminate;
    }

    internal SimpleColumnMap[] TypeDiscriminators => this.m_typeDiscriminators;

    internal Dictionary<EntityType, TypedColumnMap> TypeChoices => this.m_typeChoices;

    internal Func<object[], EntityType> Discriminate => this.m_discriminate;

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
      stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "P{{TypeId=<{0}>, ", (object) StringUtil.ToCommaSeparatedString((IEnumerable) this.TypeDiscriminators));
      foreach (KeyValuePair<EntityType, TypedColumnMap> typeChoice in this.TypeChoices)
      {
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}(<{1}>,{2})", (object) str, (object) typeChoice.Key, (object) typeChoice.Value);
        str = ",";
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }
  }
}
