// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ColumnMap
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class ColumnMap
  {
    private TypeUsage _type;
    private string _name;
    internal const string DefaultColumnName = "Value";

    internal ColumnMap(TypeUsage type, string name)
    {
      this._type = type;
      this._name = name;
    }

    internal TypeUsage Type
    {
      get => this._type;
      set => this._type = value;
    }

    internal string Name
    {
      get => this._name;
      set => this._name = value;
    }

    internal bool IsNamed => this._name != null;

    [DebuggerNonUserCode]
    internal abstract void Accept<TArgType>(ColumnMapVisitor<TArgType> visitor, TArgType arg);

    [DebuggerNonUserCode]
    internal abstract TResultType Accept<TResultType, TArgType>(
      ColumnMapVisitorWithResults<TResultType, TArgType> visitor,
      TArgType arg);
  }
}
