// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.PrimitiveTypeVarInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class PrimitiveTypeVarInfo : VarInfo
  {
    private readonly List<Var> m_newVars;

    internal PrimitiveTypeVarInfo(Var newVar) => this.m_newVars = new List<Var>()
    {
      newVar
    };

    internal Var NewVar => this.m_newVars[0];

    internal override VarInfoKind Kind => VarInfoKind.PrimitiveTypeVarInfo;

    internal override List<Var> NewVars => this.m_newVars;
  }
}
