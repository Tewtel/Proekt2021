﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.TableMD
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.PlanCompiler;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class TableMD
  {
    private readonly List<ColumnMD> m_columns;
    private readonly List<ColumnMD> m_keys;
    private readonly EntitySetBase m_extent;
    private readonly bool m_flattened;

    private TableMD(EntitySetBase extent)
    {
      this.m_columns = new List<ColumnMD>();
      this.m_keys = new List<ColumnMD>();
      this.m_extent = extent;
    }

    internal TableMD(TypeUsage type, EntitySetBase extent)
      : this(extent)
    {
      this.m_columns.Add(new ColumnMD("element", type));
      this.m_flattened = !TypeUtils.IsStructuredType(type);
    }

    internal TableMD(
      IEnumerable<EdmProperty> properties,
      IEnumerable<EdmMember> keyProperties,
      EntitySetBase extent)
      : this(extent)
    {
      Dictionary<string, ColumnMD> dictionary = new Dictionary<string, ColumnMD>();
      this.m_flattened = true;
      foreach (EdmProperty property in properties)
      {
        ColumnMD columnMd = new ColumnMD((EdmMember) property);
        this.m_columns.Add(columnMd);
        dictionary[property.Name] = columnMd;
      }
      foreach (EdmMember keyProperty in keyProperties)
      {
        ColumnMD columnMd;
        if (dictionary.TryGetValue(keyProperty.Name, out columnMd))
          this.m_keys.Add(columnMd);
      }
    }

    internal EntitySetBase Extent => this.m_extent;

    internal List<ColumnMD> Columns => this.m_columns;

    internal List<ColumnMD> Keys => this.m_keys;

    internal bool Flattened => this.m_flattened;

    public override string ToString() => this.m_extent == null ? "Transient" : this.m_extent.Name;
  }
}
