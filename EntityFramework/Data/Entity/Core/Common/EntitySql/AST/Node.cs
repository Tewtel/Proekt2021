// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.Node
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal abstract class Node
  {
    private ErrorContext _errCtx = new ErrorContext();

    internal Node()
    {
    }

    internal Node(string commandText, int inputPosition)
    {
      this._errCtx.CommandText = commandText;
      this._errCtx.InputPosition = inputPosition;
    }

    internal ErrorContext ErrCtx
    {
      get => this._errCtx;
      set => this._errCtx = value;
    }
  }
}
