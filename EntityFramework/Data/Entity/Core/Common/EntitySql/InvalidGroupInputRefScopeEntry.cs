// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.InvalidGroupInputRefScopeEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class InvalidGroupInputRefScopeEntry : ScopeEntry
  {
    internal InvalidGroupInputRefScopeEntry()
      : base(ScopeEntryKind.InvalidGroupInputRef)
    {
    }

    internal override DbExpression GetExpression(string refName, ErrorContext errCtx)
    {
      string errorMessage = Strings.InvalidGroupIdentifierReference((object) refName);
      throw EntitySqlException.Create(errCtx, errorMessage, (Exception) null);
    }
  }
}
