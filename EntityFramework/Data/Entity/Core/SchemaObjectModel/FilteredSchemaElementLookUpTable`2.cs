// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.FilteredSchemaElementLookUpTable`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class FilteredSchemaElementLookUpTable<T, S> : 
    IEnumerable<T>,
    IEnumerable,
    ISchemaElementLookUpTable<T>
    where T : S
    where S : SchemaElement
  {
    private readonly SchemaElementLookUpTable<S> _lookUpTable;

    public FilteredSchemaElementLookUpTable(SchemaElementLookUpTable<S> lookUpTable) => this._lookUpTable = lookUpTable;

    public IEnumerator<T> GetEnumerator() => this._lookUpTable.GetFilteredEnumerator<T>();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._lookUpTable.GetFilteredEnumerator<T>();

    public int Count
    {
      get
      {
        int num = 0;
        foreach (S s in this._lookUpTable)
        {
          if ((object) s is T)
            ++num;
        }
        return num;
      }
    }

    public bool ContainsKey(string key) => this._lookUpTable.ContainsKey(key) && (object) ((object) this._lookUpTable[key] as T) != null;

    public T this[string key]
    {
      get
      {
        S s = this._lookUpTable[key];
        if ((object) s == null)
          return default (T);
        return s is T obj ? obj : throw new InvalidOperationException(Strings.UnexpectedTypeInCollection((object) s.GetType(), (object) key));
      }
    }

    public T LookUpEquivalentKey(string key) => (object) this._lookUpTable.LookUpEquivalentKey(key) as T;
  }
}
