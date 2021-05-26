// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ParameterVar
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class ParameterVar : Var
  {
    private readonly string m_paramName;

    internal ParameterVar(int id, TypeUsage type, string paramName)
      : base(id, VarType.Parameter, type)
    {
      this.m_paramName = paramName;
    }

    internal string ParameterName => this.m_paramName;

    internal override bool TryGetName(out string name)
    {
      name = this.ParameterName;
      return true;
    }
  }
}
