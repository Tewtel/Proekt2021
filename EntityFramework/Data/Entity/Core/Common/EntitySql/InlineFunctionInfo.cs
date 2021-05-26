// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.InlineFunctionInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal abstract class InlineFunctionInfo
  {
    internal readonly System.Data.Entity.Core.Common.EntitySql.AST.FunctionDefinition FunctionDefAst;
    internal readonly List<DbVariableReferenceExpression> Parameters;

    internal InlineFunctionInfo(
      System.Data.Entity.Core.Common.EntitySql.AST.FunctionDefinition functionDef,
      List<DbVariableReferenceExpression> parameters)
    {
      this.FunctionDefAst = functionDef;
      this.Parameters = parameters;
    }

    internal abstract DbLambda GetLambda(SemanticResolver sr);
  }
}
