// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.DotExpr
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class DotExpr : Node
  {
    private readonly Node _leftExpr;
    private readonly Identifier _identifier;
    private bool? _isMultipartIdentifierComputed;
    private string[] _names;

    internal DotExpr(Node leftExpr, Identifier id)
    {
      this._leftExpr = leftExpr;
      this._identifier = id;
    }

    internal Node Left => this._leftExpr;

    internal Identifier Identifier => this._identifier;

    internal bool IsMultipartIdentifier(out string[] names)
    {
      if (this._isMultipartIdentifierComputed.HasValue)
      {
        names = this._names;
        return this._isMultipartIdentifierComputed.Value;
      }
      this._names = (string[]) null;
      if (this._leftExpr is Identifier leftExpr1)
        this._names = new string[2]
        {
          leftExpr1.Name,
          this._identifier.Name
        };
      string[] names1;
      if (this._leftExpr is DotExpr leftExpr2 && leftExpr2.IsMultipartIdentifier(out names1))
      {
        this._names = new string[names1.Length + 1];
        names1.CopyTo((Array) this._names, 0);
        this._names[this._names.Length - 1] = this._identifier.Name;
      }
      this._isMultipartIdentifierComputed = new bool?(this._names != null);
      names = this._names;
      return this._isMultipartIdentifierComputed.Value;
    }
  }
}
