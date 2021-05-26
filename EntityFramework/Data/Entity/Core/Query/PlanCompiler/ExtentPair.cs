// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.ExtentPair
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class ExtentPair
  {
    private readonly EntitySetBase m_left;
    private readonly EntitySetBase m_right;

    internal EntitySetBase Left => this.m_left;

    internal EntitySetBase Right => this.m_right;

    public override bool Equals(object obj) => obj is ExtentPair extentPair && extentPair.Left.Equals((object) this.Left) && extentPair.Right.Equals((object) this.Right);

    public override int GetHashCode() => this.Left.GetHashCode() << 4 ^ this.Right.GetHashCode();

    internal ExtentPair(EntitySetBase left, EntitySetBase right)
    {
      this.m_left = left;
      this.m_right = right;
    }
  }
}
