// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.ParseResult
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees;

namespace System.Data.Entity.Core.Common.EntitySql
{
  /// <summary>Entity SQL Parser result information.</summary>
  public sealed class ParseResult
  {
    private readonly DbCommandTree _commandTree;
    private readonly ReadOnlyCollection<FunctionDefinition> _functionDefs;

    internal ParseResult(DbCommandTree commandTree, List<FunctionDefinition> functionDefs)
    {
      this._commandTree = commandTree;
      this._functionDefs = new ReadOnlyCollection<FunctionDefinition>((IList<FunctionDefinition>) functionDefs);
    }

    /// <summary> A command tree produced during parsing. </summary>
    public DbCommandTree CommandTree => this._commandTree;

    /// <summary>
    /// List of <see cref="T:System.Data.Entity.Core.Common.EntitySql.FunctionDefinition" /> objects describing query inline function definitions.
    /// </summary>
    public ReadOnlyCollection<FunctionDefinition> FunctionDefinitions => this._functionDefs;
  }
}
