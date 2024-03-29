﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.ConsolidatedIndex
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.Model;
using System.Linq;

namespace System.Data.Entity.Infrastructure
{
  internal class ConsolidatedIndex
  {
    private readonly string _table;
    private IndexAttribute _index;
    private readonly IDictionary<int, string> _columns = (IDictionary<int, string>) new Dictionary<int, string>();

    public ConsolidatedIndex(string table, IndexAttribute index)
    {
      this._table = table;
      this._index = index;
    }

    public ConsolidatedIndex(string table, string column, IndexAttribute index)
      : this(table, index)
    {
      this._columns[index.Order] = column;
    }

    public static IEnumerable<ConsolidatedIndex> BuildIndexes(
      string tableName,
      IEnumerable<Tuple<string, EdmProperty>> columns)
    {
      List<ConsolidatedIndex> source = new List<ConsolidatedIndex>();
      foreach (Tuple<string, EdmProperty> column in columns)
      {
        foreach (IndexAttribute indexAttribute in column.Item2.Annotations.Where<MetadataProperty>((Func<MetadataProperty, bool>) (a => a.Name == "http://schemas.microsoft.com/ado/2013/11/edm/customannotation:Index")).Select<MetadataProperty, object>((Func<MetadataProperty, object>) (a => a.Value)).OfType<IndexAnnotation>().SelectMany<IndexAnnotation, IndexAttribute>((Func<IndexAnnotation, IEnumerable<IndexAttribute>>) (a => a.Indexes)))
        {
          IndexAttribute index = indexAttribute;
          ConsolidatedIndex consolidatedIndex = index.Name == null ? (ConsolidatedIndex) null : source.FirstOrDefault<ConsolidatedIndex>((Func<ConsolidatedIndex, bool>) (i => i.Index.Name == index.Name));
          if (consolidatedIndex == null)
            source.Add(new ConsolidatedIndex(tableName, column.Item1, index));
          else
            consolidatedIndex.Add(column.Item1, index);
        }
      }
      return (IEnumerable<ConsolidatedIndex>) source;
    }

    public IndexAttribute Index => this._index;

    public IEnumerable<string> Columns => this._columns.OrderBy<KeyValuePair<int, string>, int>((Func<KeyValuePair<int, string>, int>) (c => c.Key)).Select<KeyValuePair<int, string>, string>((Func<KeyValuePair<int, string>, string>) (c => c.Value));

    public string Table => this._table;

    public void Add(string columnName, IndexAttribute index)
    {
      if (this._columns.ContainsKey(index.Order))
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.OrderConflictWhenConsolidating((object) index.Name, (object) this._table, (object) index.Order, (object) this._columns[index.Order], (object) columnName));
      this._columns[index.Order] = columnName;
      CompatibilityResult compatibilityResult = this._index.IsCompatibleWith(index, true);
      if (!(bool) compatibilityResult)
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.ConflictWhenConsolidating((object) index.Name, (object) this._table, (object) compatibilityResult.ErrorMessage));
      this._index = this._index.MergeWith(index, true);
    }

    public CreateIndexOperation CreateCreateIndexOperation()
    {
      string[] array = this.Columns.ToArray<string>();
      CreateIndexOperation createIndexOperation1 = new CreateIndexOperation();
      createIndexOperation1.Name = this._index.Name ?? IndexOperation.BuildDefaultName((IEnumerable<string>) array);
      createIndexOperation1.Table = this._table;
      CreateIndexOperation createIndexOperation2 = createIndexOperation1;
      foreach (string str in array)
        createIndexOperation2.Columns.Add(str);
      if (this._index.IsClusteredConfigured)
        createIndexOperation2.IsClustered = this._index.IsClustered;
      if (this._index.IsUniqueConfigured)
        createIndexOperation2.IsUnique = this._index.IsUnique;
      return createIndexOperation2;
    }

    public DropIndexOperation CreateDropIndexOperation() => (DropIndexOperation) this.CreateCreateIndexOperation().Inverse;
  }
}
