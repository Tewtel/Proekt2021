// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.CollectionTypeDefinition
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class CollectionTypeDefinition : Node
  {
    private readonly Node _elementTypeDef;

    internal CollectionTypeDefinition(Node elementTypeDef) => this._elementTypeDef = elementTypeDef;

    internal Node ElementTypeDef => this._elementTypeDef;
  }
}
