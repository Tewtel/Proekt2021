// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.RelPropertyRef
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class RelPropertyRef : PropertyRef
  {
    private readonly RelProperty m_property;

    internal RelPropertyRef(RelProperty property) => this.m_property = property;

    internal RelProperty Property => this.m_property;

    public override bool Equals(object obj) => obj is RelPropertyRef relPropertyRef && this.m_property.Equals((object) relPropertyRef.m_property);

    public override int GetHashCode() => this.m_property.GetHashCode();

    public override string ToString() => this.m_property.ToString();
  }
}
