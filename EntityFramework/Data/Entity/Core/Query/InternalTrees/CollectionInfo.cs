// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.CollectionInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class CollectionInfo
  {
    private readonly Var m_collectionVar;
    private readonly ColumnMap m_columnMap;
    private readonly VarList m_flattenedElementVars;
    private readonly VarVec m_keys;
    private readonly List<SortKey> m_sortKeys;
    private readonly object m_discriminatorValue;

    internal Var CollectionVar => this.m_collectionVar;

    internal ColumnMap ColumnMap => this.m_columnMap;

    internal VarList FlattenedElementVars => this.m_flattenedElementVars;

    internal VarVec Keys => this.m_keys;

    internal List<SortKey> SortKeys => this.m_sortKeys;

    internal object DiscriminatorValue => this.m_discriminatorValue;

    internal CollectionInfo(
      Var collectionVar,
      ColumnMap columnMap,
      VarList flattenedElementVars,
      VarVec keys,
      List<SortKey> sortKeys,
      object discriminatorValue)
    {
      this.m_collectionVar = collectionVar;
      this.m_columnMap = columnMap;
      this.m_flattenedElementVars = flattenedElementVars;
      this.m_keys = keys;
      this.m_sortKeys = sortKeys;
      this.m_discriminatorValue = discriminatorValue;
    }
  }
}
