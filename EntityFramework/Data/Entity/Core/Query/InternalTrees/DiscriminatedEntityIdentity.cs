// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.DiscriminatedEntityIdentity
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
using System.Text;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class DiscriminatedEntityIdentity : EntityIdentity
  {
    private readonly SimpleColumnMap m_entitySetColumn;
    private readonly EntitySet[] m_entitySetMap;

    internal DiscriminatedEntityIdentity(
      SimpleColumnMap entitySetColumn,
      EntitySet[] entitySetMap,
      SimpleColumnMap[] keyColumns)
      : base(keyColumns)
    {
      this.m_entitySetColumn = entitySetColumn;
      this.m_entitySetMap = entitySetMap;
    }

    internal SimpleColumnMap EntitySetColumnMap => this.m_entitySetColumn;

    internal EntitySet[] EntitySetMap => this.m_entitySetMap;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = string.Empty;
      stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "[(Keys={");
      foreach (SimpleColumnMap key in this.Keys)
      {
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}{1}", (object) str, (object) key);
        str = ",";
      }
      stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "})]");
      return stringBuilder.ToString();
    }
  }
}
