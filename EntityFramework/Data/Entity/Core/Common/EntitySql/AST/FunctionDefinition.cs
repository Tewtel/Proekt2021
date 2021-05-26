﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.FunctionDefinition
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class FunctionDefinition : Node
  {
    private readonly Identifier _name;
    private readonly NodeList<PropDefinition> _paramDefList;
    private readonly Node _body;
    private readonly int _startPosition;
    private readonly int _endPosition;

    internal FunctionDefinition(
      Identifier name,
      NodeList<PropDefinition> argDefList,
      Node body,
      int startPosition,
      int endPosition)
    {
      this._name = name;
      this._paramDefList = argDefList;
      this._body = body;
      this._startPosition = startPosition;
      this._endPosition = endPosition;
    }

    internal string Name => this._name.Name;

    internal NodeList<PropDefinition> Parameters => this._paramDefList;

    internal Node Body => this._body;

    internal int StartPosition => this._startPosition;

    internal int EndPosition => this._endPosition;
  }
}
