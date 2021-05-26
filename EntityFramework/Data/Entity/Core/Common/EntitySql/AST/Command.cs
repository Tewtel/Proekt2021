// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.Command
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class Command : Node
  {
    private readonly NodeList<NamespaceImport> _namespaceImportList;
    private readonly Statement _statement;

    internal Command(NodeList<NamespaceImport> nsImportList, Statement statement)
    {
      this._namespaceImportList = nsImportList;
      this._statement = statement;
    }

    internal NodeList<NamespaceImport> NamespaceImportList => this._namespaceImportList;

    internal Statement Statement => this._statement;
  }
}
