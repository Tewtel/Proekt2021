// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.NestedPropertyRef
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class NestedPropertyRef : PropertyRef
  {
    private readonly PropertyRef m_inner;
    private readonly PropertyRef m_outer;

    internal NestedPropertyRef(PropertyRef innerProperty, PropertyRef outerProperty)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(!(innerProperty is NestedPropertyRef), "innerProperty cannot be a NestedPropertyRef");
      this.m_inner = innerProperty;
      this.m_outer = outerProperty;
    }

    internal PropertyRef OuterProperty => this.m_outer;

    internal PropertyRef InnerProperty => this.m_inner;

    public override bool Equals(object obj) => obj is NestedPropertyRef nestedPropertyRef && this.m_inner.Equals((object) nestedPropertyRef.m_inner) && this.m_outer.Equals((object) nestedPropertyRef.m_outer);

    public override int GetHashCode() => this.m_inner.GetHashCode() ^ this.m_outer.GetHashCode();

    public override string ToString() => this.m_inner?.ToString() + "." + this.m_outer?.ToString();
  }
}
