// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.PropDefinition
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class PropDefinition : Node
  {
    private readonly Identifier _name;
    private readonly Node _typeDefExpr;

    internal PropDefinition(Identifier name, Node typeDefExpr)
    {
      this._name = name;
      this._typeDefExpr = typeDefExpr;
    }

    internal Identifier Name => this._name;

    internal Node Type => this._typeDefExpr;
  }
}
