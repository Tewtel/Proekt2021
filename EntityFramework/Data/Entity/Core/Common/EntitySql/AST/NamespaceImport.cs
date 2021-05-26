// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.NamespaceImport
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class NamespaceImport : Node
  {
    private readonly Identifier _namespaceAlias;
    private readonly Node _namespaceName;

    internal NamespaceImport(Identifier identifier) => this._namespaceName = (Node) identifier;

    internal NamespaceImport(DotExpr dorExpr) => this._namespaceName = (Node) dorExpr;

    internal NamespaceImport(BuiltInExpr bltInExpr)
    {
      this._namespaceAlias = (Identifier) null;
      if (!(bltInExpr.Arg1 is Identifier identifier))
        throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, Strings.InvalidNamespaceAlias, (Exception) null);
      this._namespaceAlias = identifier;
      this._namespaceName = bltInExpr.Arg2;
    }

    internal Identifier Alias => this._namespaceAlias;

    internal Node NamespaceName => this._namespaceName;
  }
}
