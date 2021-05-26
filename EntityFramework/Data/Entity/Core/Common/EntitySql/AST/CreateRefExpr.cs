// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.CreateRefExpr
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class CreateRefExpr : Node
  {
    private readonly Node _entitySet;
    private readonly Node _keys;
    private readonly Node _typeIdentifier;

    internal CreateRefExpr(Node entitySet, Node keys)
      : this(entitySet, keys, (Node) null)
    {
    }

    internal CreateRefExpr(Node entitySet, Node keys, Node typeIdentifier)
    {
      this._entitySet = entitySet;
      this._keys = keys;
      this._typeIdentifier = typeIdentifier;
    }

    internal Node EntitySet => this._entitySet;

    internal Node Keys => this._keys;

    internal Node TypeIdentifier => this._typeIdentifier;
  }
}
