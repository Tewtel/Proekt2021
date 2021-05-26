// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.SimplePropertyRef
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class SimplePropertyRef : PropertyRef
  {
    private readonly EdmMember m_property;

    internal SimplePropertyRef(EdmMember property) => this.m_property = property;

    internal EdmMember Property => this.m_property;

    public override bool Equals(object obj) => obj is SimplePropertyRef simplePropertyRef && Command.EqualTypes((EdmType) this.m_property.DeclaringType, (EdmType) simplePropertyRef.m_property.DeclaringType) && simplePropertyRef.m_property.Name.Equals(this.m_property.Name);

    public override int GetHashCode() => this.m_property.Name.GetHashCode();

    public override string ToString() => this.m_property.Name;
  }
}
