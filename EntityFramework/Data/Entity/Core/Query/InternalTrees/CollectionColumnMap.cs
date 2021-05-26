// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.CollectionColumnMap
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class CollectionColumnMap : ColumnMap
  {
    private readonly ColumnMap m_element;
    private readonly SimpleColumnMap[] m_foreignKeys;
    private readonly SimpleColumnMap[] m_keys;

    internal CollectionColumnMap(
      TypeUsage type,
      string name,
      ColumnMap elementMap,
      SimpleColumnMap[] keys,
      SimpleColumnMap[] foreignKeys)
      : base(type, name)
    {
      this.m_element = elementMap;
      this.m_keys = keys ?? new SimpleColumnMap[0];
      this.m_foreignKeys = foreignKeys ?? new SimpleColumnMap[0];
    }

    internal SimpleColumnMap[] ForeignKeys => this.m_foreignKeys;

    internal SimpleColumnMap[] Keys => this.m_keys;

    internal ColumnMap Element => this.m_element;
  }
}
