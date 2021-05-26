// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.RowTypeDefinition
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class RowTypeDefinition : Node
  {
    private readonly NodeList<PropDefinition> _propDefList;

    internal RowTypeDefinition(NodeList<PropDefinition> propDefList) => this._propDefList = propDefList;

    internal NodeList<PropDefinition> Properties => this._propDefList;
  }
}
