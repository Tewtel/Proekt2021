﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.DiscriminatorMapInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class DiscriminatorMapInfo
  {
    internal EntityTypeBase RootEntityType;
    internal bool IncludesSubTypes;
    internal ExplicitDiscriminatorMap DiscriminatorMap;

    internal DiscriminatorMapInfo(
      EntityTypeBase rootEntityType,
      bool includesSubTypes,
      ExplicitDiscriminatorMap discriminatorMap)
    {
      this.RootEntityType = rootEntityType;
      this.IncludesSubTypes = includesSubTypes;
      this.DiscriminatorMap = discriminatorMap;
    }

    internal void Merge(
      EntityTypeBase neededRootEntityType,
      bool includesSubtypes,
      ExplicitDiscriminatorMap discriminatorMap)
    {
      if (this.RootEntityType == neededRootEntityType && this.IncludesSubTypes == includesSubtypes)
        return;
      if (!this.IncludesSubTypes || !includesSubtypes)
        this.DiscriminatorMap = (ExplicitDiscriminatorMap) null;
      if (TypeSemantics.IsSubTypeOf((EdmType) this.RootEntityType, (EdmType) neededRootEntityType))
      {
        this.RootEntityType = neededRootEntityType;
        this.DiscriminatorMap = discriminatorMap;
      }
      if (TypeSemantics.IsSubTypeOf((EdmType) neededRootEntityType, (EdmType) this.RootEntityType))
        return;
      this.DiscriminatorMap = (ExplicitDiscriminatorMap) null;
    }
  }
}
