// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.JoinSymbol
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.SQLite.EF6
{
  internal sealed class JoinSymbol : Symbol
  {
    private List<Symbol> columnList;
    private List<Symbol> extentList;
    private List<Symbol> flattenedExtentList;
    private Dictionary<string, Symbol> nameToExtent;
    private bool isNestedJoin;

    internal List<Symbol> ColumnList
    {
      get
      {
        if (this.columnList == null)
          this.columnList = new List<Symbol>();
        return this.columnList;
      }
      set => this.columnList = value;
    }

    internal List<Symbol> ExtentList => this.extentList;

    internal List<Symbol> FlattenedExtentList
    {
      get
      {
        if (this.flattenedExtentList == null)
          this.flattenedExtentList = new List<Symbol>();
        return this.flattenedExtentList;
      }
      set => this.flattenedExtentList = value;
    }

    internal Dictionary<string, Symbol> NameToExtent => this.nameToExtent;

    internal bool IsNestedJoin
    {
      get => this.isNestedJoin;
      set => this.isNestedJoin = value;
    }

    public JoinSymbol(string name, TypeUsage type, List<Symbol> extents)
      : base(name, type)
    {
      this.extentList = new List<Symbol>(extents.Count);
      this.nameToExtent = new Dictionary<string, Symbol>(extents.Count, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (Symbol extent in extents)
      {
        this.nameToExtent[extent.Name] = extent;
        this.ExtentList.Add(extent);
      }
    }
  }
}
