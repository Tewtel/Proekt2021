// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.Identifier
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class Identifier : Node
  {
    private readonly string _name;
    private readonly bool _isEscaped;

    internal Identifier(string name, bool isEscaped, string query, int inputPos)
      : base(query, inputPos)
    {
      if (!isEscaped)
      {
        bool isIdentifierASCII = true;
        if (!CqlLexer.IsLetterOrDigitOrUnderscore(name, out isIdentifierASCII))
        {
          if (isIdentifierASCII)
            throw EntitySqlException.Create(this.ErrCtx, Strings.InvalidSimpleIdentifier((object) name), (Exception) null);
          throw EntitySqlException.Create(this.ErrCtx, Strings.InvalidSimpleIdentifierNonASCII((object) name), (Exception) null);
        }
      }
      this._name = name;
      this._isEscaped = isEscaped;
    }

    internal string Name => this._name;

    internal bool IsEscaped => this._isEscaped;
  }
}
