// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ColumnMD
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class ColumnMD
  {
    private readonly string m_name;
    private readonly TypeUsage m_type;
    private readonly EdmMember m_property;

    internal ColumnMD(string name, TypeUsage type)
    {
      this.m_name = name;
      this.m_type = type;
    }

    internal ColumnMD(EdmMember property)
      : this(property.Name, property.TypeUsage)
    {
      this.m_property = property;
    }

    internal string Name => this.m_name;

    internal TypeUsage Type => this.m_type;

    internal bool IsNullable => this.m_property == null || TypeSemantics.IsNullable(this.m_property);

    public override string ToString() => this.m_name;
  }
}
