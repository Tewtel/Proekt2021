﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.AllPropertyRef
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class AllPropertyRef : PropertyRef
  {
    internal static AllPropertyRef Instance = new AllPropertyRef();

    private AllPropertyRef()
    {
    }

    internal override PropertyRef CreateNestedPropertyRef(PropertyRef p) => p;

    public override string ToString() => "ALL";
  }
}
