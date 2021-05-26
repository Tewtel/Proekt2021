// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Mapping.TableMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Diagnostics;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Configuration.Mapping
{
  [DebuggerDisplay("{Table.Name}")]
  internal class TableMapping
  {
    private readonly EntityType _table;
    private readonly SortedEntityTypeIndex _entityTypes;
    private readonly List<ColumnMapping> _columns;

    public TableMapping(EntityType table)
    {
      this._table = table;
      this._entityTypes = new SortedEntityTypeIndex();
      this._columns = new List<ColumnMapping>();
    }

    public EntityType Table => this._table;

    public SortedEntityTypeIndex EntityTypes => this._entityTypes;

    public IEnumerable<ColumnMapping> ColumnMappings => (IEnumerable<ColumnMapping>) this._columns;

    public void AddEntityTypeMappingFragment(
      EntitySet entitySet,
      EntityType entityType,
      MappingFragment fragment)
    {
      this._entityTypes.Add(entitySet, entityType);
      EdmProperty defaultDiscriminator = fragment.GetDefaultDiscriminator();
      foreach (ColumnMappingBuilder columnMapping in fragment.ColumnMappings)
      {
        ColumnMappingBuilder cm = columnMapping;
        this.FindOrCreateColumnMapping(cm.ColumnProperty).AddMapping(entityType, cm.PropertyPath, fragment.ColumnConditions.Where<ConditionPropertyMapping>((Func<ConditionPropertyMapping, bool>) (cc => cc.Column == cm.ColumnProperty)), defaultDiscriminator == cm.ColumnProperty);
      }
      foreach (ConditionPropertyMapping conditionPropertyMapping in fragment.ColumnConditions.Where<ConditionPropertyMapping>((Func<ConditionPropertyMapping, bool>) (cc => fragment.ColumnMappings.All<ColumnMappingBuilder>((Func<ColumnMappingBuilder, bool>) (pm => pm.ColumnProperty != cc.Column)))))
        this.FindOrCreateColumnMapping(conditionPropertyMapping.Column).AddMapping(entityType, (IList<EdmProperty>) null, (IEnumerable<ConditionPropertyMapping>) new ConditionPropertyMapping[1]
        {
          conditionPropertyMapping
        }, (defaultDiscriminator == conditionPropertyMapping.Column ? 1 : 0) != 0);
    }

    private ColumnMapping FindOrCreateColumnMapping(EdmProperty column)
    {
      ColumnMapping columnMapping = this._columns.SingleOrDefault<ColumnMapping>((Func<ColumnMapping, bool>) (c => c.Column == column));
      if (columnMapping == null)
      {
        columnMapping = new ColumnMapping(column);
        this._columns.Add(columnMapping);
      }
      return columnMapping;
    }
  }
}
