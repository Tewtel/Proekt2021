// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteDbTypeMap
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;

namespace System.Data.SQLite
{
  internal sealed class SQLiteDbTypeMap : Dictionary<string, SQLiteDbTypeMapping>
  {
    private Dictionary<DbType, SQLiteDbTypeMapping> reverse;

    public SQLiteDbTypeMap()
      : base((IEqualityComparer<string>) new TypeNameStringComparer())
    {
      this.reverse = new Dictionary<DbType, SQLiteDbTypeMapping>();
    }

    public SQLiteDbTypeMap(IEnumerable<SQLiteDbTypeMapping> collection)
      : this()
    {
      this.Add(collection);
    }

    public int Clear()
    {
      int num1 = 0;
      if (this.reverse != null)
      {
        num1 += this.reverse.Count;
        this.reverse.Clear();
      }
      int num2 = num1 + this.Count;
      base.Clear();
      return num2;
    }

    public void Add(IEnumerable<SQLiteDbTypeMapping> collection)
    {
      if (collection == null)
        throw new ArgumentNullException(nameof (collection));
      foreach (SQLiteDbTypeMapping liteDbTypeMapping in collection)
        this.Add(liteDbTypeMapping);
    }

    public void Add(SQLiteDbTypeMapping item)
    {
      if (item == null)
        throw new ArgumentNullException(nameof (item));
      if (item.typeName == null)
        throw new ArgumentException("item type name cannot be null");
      this.Add(item.typeName, item);
      if (!item.primary)
        return;
      this.reverse.Add(item.dataType, item);
    }

    public bool ContainsKey(DbType key) => this.reverse != null && this.reverse.ContainsKey(key);

    public bool TryGetValue(DbType key, out SQLiteDbTypeMapping value)
    {
      if (this.reverse != null)
        return this.reverse.TryGetValue(key, out value);
      value = (SQLiteDbTypeMapping) null;
      return false;
    }

    public bool Remove(DbType key) => this.reverse != null && this.reverse.Remove(key);
  }
}
